using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4 {
	class Program {
		static void Main(string[] args) {
			Console.ReadLine();
		}
		private static void DoIncreaseUpdate(Node<AddMonoid, AddMonoid, IdentityMeasure<int, int>, int, int> tree, Node<AddMonoid, AddMonoid, IdentityMeasure<int, int>, int, int> thisTree) {
			if (tree == null)
				return;
			if (tree.State == state.None || tree.State == state.Changed) {
				tree.State = state.Increased;
				tree.Change = thisTree.Change;
				return;
			}
			tree.Change += thisTree.Change;
		}
		private static void DoAssignUpdate(Node<AddMonoid, AddMonoid, IdentityMeasure<int, int>, int, int> tree, Node<AddMonoid, AddMonoid, IdentityMeasure<int, int>, int, int> thisTree) {
			if (tree == null)
				return;
			tree.State = state.Assigned;
			tree.Change = thisTree.Change;
		}
	}
	public interface IMonoid<T> {
		T Zero { get; }
		T Append(T first, T second);
	}
	public interface IMeasure<TValue, TData> {
		TValue Measure { get; }
		TData Data { get; set; }
	}
	public static class Singleton<T> where T : new() {
		private static readonly T _instance = new T();
		public static T Instance { get { return _instance; } }
	}
	public enum state {
			None,
			Increased,
			Assigned,
			Changed
		}
	public class Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>
		where TMonoid : IMonoid<TValue>, new()
		where TDataMonoid : IMonoid<TData>, new()
		where TMeasure : IMeasure<TValue, TData>, new() {	
		/// <summary>
		/// Для вершины, абстракция данных и меры
		/// </summary>
		public readonly TMeasure Data;
		public TValue Measure { get; set; }

		public readonly Node<TMonoid, TDataMonoid, TMeasure, TValue, TData> Left, Right;
		private int _size;
		public int Size { get { return _size; } private set { _size = value; } }


		/// <summary>
		/// состояние и "задержанное изменение"
		/// </summary>
		public state State;
		public TData Change;

		/// <summary>
		/// 2 делегата для обновления состояние вершины (рекурсивно вниз) при увеличении/присваивании
		/// </summary>
		private Action<Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>> IncreaseUpdate, AssigneUpdate;

		/// <summary>
		/// создание листа
		/// </summary>
		/// <param name="data"></param>
		public Node(
			TMeasure data, 
			Action<Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>> assign, 
			Action<Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>> update) {
			Data = data;
			Left = Right = null;
			_size = 1;
			Measure = Data.Measure;
			AssigneUpdate = assign;
			IncreaseUpdate = update;
			State = state.None;
		}
		/// <summary>
		/// создание промежуточной вершины
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		public Node(Node<TMonoid, TDataMonoid, TMeasure, TValue, TData> left,
			Node<TMonoid, TDataMonoid, TMeasure, TValue, TData> right,
			Action<Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>> assign, 
			Action<Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>> update) {
			Left = left;
			Right = right;
			AssigneUpdate = assign;
			IncreaseUpdate = update;
			_size = (Left == null ? 0 : Left.Size) + (Right == null ? 0 : Right.Size);
			Measure = Singleton<TMonoid>.Instance.Append(left.Measure, right.Measure);
			State = state.None;
		}

		private void Update() {
			if (State == state.None)
				return;
			if (State == state.Increased) {
				IncreaseUpdate(this);
				State = state.None;
				return;
			}
			if (State == state.Assigned) {
				AssigneUpdate(this);
				State = state.None;
			}
			if (State == state.Changed) {
				State = state.None;
				if (Left != null)
					Left.Update();
				if (Right != null)
					Right.Update();
				if (Left == null) {
					Measure = Right.Measure;
					return;
				}
				if (Right == null) {
					Measure = Left.Measure;
					return;
				}
				Measure = Singleton<TMonoid>.Instance.Append(Left.Measure, Right.Measure);
			}
		}
		public TValue MeasureOn(int start, int length) {
			Update();
			if (Left != null && start == 0 && length == Left.Size) return Left.Measure;
			if (Right != null && start == (Left == null ? 0 : Left.Size) && length == Right.Size) return Right.Measure;

			if (Left != null && start + length <= Left.Size)
				return Left.MeasureOn(start, length);
			if (Right != null && start >= (Left == null ? 0 : Left.Size))
				return Right.MeasureOn(start - (Left == null ? 0 : Left.Size), length);

			var monoidV = Singleton<TMonoid>.Instance;
			if (Left != null) {
				if(Right != null)
					return monoidV.Append(Left.MeasureOn(start, Left.Size - start), Right.MeasureOn(0, length - (Left.Size - start)));
				return Left.MeasureOn(start, Left.Size - start);
			}
			else if (Right != null)
				return Right.MeasureOn(0, length + start);
			else return Measure; 
		}
		public void Assign(int start, int length, TData value) {
			if (this.State == state.Increased)
				Update();
			this.State = state.Changed;
			if (Left == null && Right == null) {
				Data.Data = value;
				Measure = Data.Measure;
				State = state.None;
				return;
			}
			if (Right != null && Left != null && start == 0 && length == Left.Size + Right.Size) {
				State = state.Assigned;
				Change = value;
				return;
			}
			if (Right == null || start + length <= Left.Size) {
				Left.Assign(start, length, value);
				return;
			}
			if (Left == null || start >= Left.Size) {
				Right.Assign(start - Left.Size, length, value);
				return;
			}
			Left.Assign(start, Left.Size - start, value);
			Right.Assign(0, start + length - Left.Size, value);
		}
		public void Increase(int start, int length, TData value) {
			if (this.State == state.Assigned)
				Update();
			if (this.State == state.None)
				this.State = state.Changed;
			if (Left == null && Right == null) {
				Data.Data = Singleton<TDataMonoid>.Instance.Append(Data.Data, value);
				Measure = Data.Measure;
				State = state.None;
				return;
			}
			if (Left != null && Right != null && start == 0 && length == Left.Size + Right.Size) {
				if (this.State == state.None || this.State == state.Changed) {
					State = state.Increased;
					Change = value;
					return;
				}
				Change = Singleton<TDataMonoid>.Instance.Append(Change, value);
				return;
			}

			if (Right == null || start + length <= Left.Size) {
				Left.Increase(start, length, value);
				return;
			}
			if (Left == null || start >= Left.Size) {
				Right.Increase(start - (Left == null ? 0 : Left.Size), length, value);
				return;
			}
			Left.Increase(start, Left.Size - start, value);
			Right.Increase(0, start + length - Left.Size, value);
		}
		public void FullUpdate() {
			if (State == state.None)
				return;
			Update();
			if (Left != null)
				Left.FullUpdate();
			if (Right != null)
				Right.FullUpdate();
		}
	}

	public class AddMonoid : IMonoid<int> {
		public AddMonoid() { }
		public int Zero { get; private set; }
		public int Append(int a, int b) {
			return a + b;
		}
	}

	public class IdentityMeasure<Q, T> : IMeasure<Q, T> where T : Q {
		public IdentityMeasure() {
			_data = default(T);
		}
		public IdentityMeasure(T value) {
			_data = value;
		}
		private T _data;
		public Q Measure { get { return _data; } }
		public T Data { get { return _data; } set { _data = value; } }
	}
	public class Tree<TMonoid, TDataMonoid, TMeasure, TValue, TData>
		where TMonoid : IMonoid<TValue>, new()
		where TDataMonoid : IMonoid<TData>, new()
		where TMeasure : IMeasure<TValue, TData>, new() {
		Node<TMonoid, TDataMonoid, TMeasure, TValue, TData> head;
		int treeLength;

		Action<Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>> IncreaseUpdate, AssigneUpdate;
		public Tree(TData[] data, Action<Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>> Assign,
			Action<Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>> Increase, Func<TData, TMeasure> convert) {
			if (data == null || Increase == null || Assign == null)
				throw new ArgumentNullException();
			IncreaseUpdate = Increase;
			AssigneUpdate = Assign;
			treeLength = data.Length;
			head = DoTree(0, data.Length - 1, data);
		}
		private Node<TMonoid, TDataMonoid, TMeasure, TValue, TData> DoTree(int l, int r, TData[] data) {
			if (l == r) {
				var measure = new TMeasure();
				measure.Data = data[l];
				return new Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>(measure, AssigneUpdate, IncreaseUpdate);
			}
			int middle = (l + r) / 2;
			return new Node<TMonoid, TDataMonoid, TMeasure, TValue, TData>(
				DoTree(l, middle, data),
				DoTree(middle + 1, r, data),
				AssigneUpdate,
				IncreaseUpdate);
		}
		public void Increase(int start, int length, TData value) {
			Validate(start, length);
			head.Increase(start, length, value);
		}
		public void Assign(int start, int length, TData value) {
			Validate(start, length);
			head.Assign(start, length, value);
		}
		public void FullUpdate() {
			head.FullUpdate();
		}
		public TValue MeasureOn(int start, int length) {
			Validate(start, length);
			return head.MeasureOn(start, length);
		}
		private void Validate(int start, int length) {
			if (start + length > this.treeLength || start < 0 || length <= 0)
				throw new ArgumentOutOfRangeException();
		}
	}
}