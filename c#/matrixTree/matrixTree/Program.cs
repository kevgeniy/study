//В коде используются битовые операции
//т.к. в дереве очень много действий с числом 2 а такие бинароные операции выполняются очень быстро
// a & 1 равно 0 тогда и только тогда когда а четно т.е. ((a & 1) == 0) равносильно (a % 2 == 0)
// а >> 1 деление на 2 (целочисленное, без остатка)
// а << 1 умножение на 2
// 1 << a это 2^a
//Нумерация совпадает с конспектом:
//вершины на уровне с 1
//номера уровней с 1 от листьев к корню
using System;

namespace matrixTree {
	class MainClass {
		public static void Main (string[] args) {
			MatrixTree mTree = new MatrixTree ();
			mTree.Include (2);
			mTree.Include (8);
			mTree.Exclude (8);
			mTree.Exclude (2);
			Console.ReadLine ();
		}
	}
	//Узел будущего дерева
	class Node {
		public Node Left;
		public Node Right;
		public bool IsParasitic;
		//public InfType Information; некая информация(какого-то типа) которая может быть ассоциирована с ключом(числом)
		public int Key { get; private set; }

		public Node (int key, bool isParasitic) 
		: this(key, null, null, isParasitic) {
		}

		public Node (int key, Node left, Node right, bool isParasitic /*InfType information*/) {
			this.Key = key;
			this.Left = left;
			this.Right = right;
			this.IsParasitic = isParasitic; 
		}
	}

	class MatrixTree {
		public Node EvenHead { get; private set; }

		public Node OddHead { get; private set; }

		/// <summary>
		/// Include the specified key. Если бы в типе Node было поле Information здесь логично было бы передавать не key 
		/// a newNode:
		/// <code>
		/// MatrixTree mtree = new MatrixTree();
		/// InfType info = new InfType();
		/// Node newNode = new Node(3, null, null, info);
		/// mtree.Include(newNode);
		/// </code>
		/// </summary>
		/// <param name="key">key.</param>
		public void Include (int key) {
			//head - корень поддерева четность которого, совпадает с четностью ключа
			Node head = ((key & 1) == 0) ? EvenHead : OddHead;
			//если в поддереве еще нет ни одного узла
			if (head == null) {
				SetHead (key, new Node (key, false));
				return;
			}
			//высчитаем, ключ общего предка head и Node(key)
			int ancestorKey = GetAncestor (head.Key, key);
			Node newHead = new Node (ancestorKey, true);
			//начиная с общего предка спускаемся вниз, добавляя нужные узлы-паразиты, 
			//если нужно, пока не дойдем до вершины head или вершины с ключом key
			AddNode (newHead, head);
			AddNode (newHead, new Node (key, false));
			SetHead (newHead.Key, newHead);
		}
		//ищет ключ общего предка вершин с ключами key1 и key2
		private int GetAncestor (int key1, int key2) {
			pair location1 = GetLocation (key1);
			pair location2 = GetLocation (key2);
			//наибольший уровень должен быть у location1
			if (location1.Level < location2.Level) {
				pair loc = location1;
				location1 = location2;
				location2 = loc;
			}
			int delta = 1 << (location1.Level - location2.Level);
			//позиция последнего элемента поддерева (поддерева с корнем которому соответствует location1) на уровне location2.Level
			int last = location1.Position * delta;
			//позиция первого элемента поддерева (поддерева с корнем которому соответствует location1) на уровне location2.Level
			int first = last - delta + 1;
			//если вершина которой соответствует location2 не в нашем диапазоне то от вершины которой соответствует
			//location1 поднимаемся к ее родителю, в зависимости от четности 
			//location1.Position интервал сдвигается левее или правее
			while (first > location2.Position || last < location2.Position) {
				if ((location1.Position & 1) == 0) {
					first -= delta;
				} else {
					last += delta;
				}
				//каждый раз размер охватываемоего интервала увеличивается в 2 раза (бинарное дерево)
				delta <<= 1;
				location1.Position >>= 1;
			}
			// можно заметить, что если взять цельный интервал (есть узел для которого этот интервал составляет на каком-то
			// уровне весь интервал) то первый общий предок есть среднее арифметическое первого и последнего элемента интервала
			// (1 << location2.Level) * ((2 * first - 1) + (2 * last - 1)) / 2
			return (1 << location2.Level) * (first + last - 1);
		}
		//для понятности
		private class pair {
			public int Level;
			public int Position;

			public pair (int level, int position) {
				this.Level = level;
				this.Position = position;
			}
		}
		//по ключу возвращает его уровень и позицию на уровне, вся нумерация соответствует конспекту Лаврова
		private pair GetLocation (int key) {
			//если ключ нечетный достаточно увеличить его на 1 до соответствуещего четного
			if ((key & 1) != 0) {
				key++;
			}
			//находим максимальную степень двойки у множителя
			int level = 0; //L в конспекте
			while ((key & 1) == 0) {
				level++;
				key >>= 1;
			}
			//зная (2Y +1) получаем Y
			int position = (key + 1) >> 1; // Y в конспекте
			return new pair (level, position);
		}
		//добавляет newNode в дeрево корнем которого является head
		private void AddNode (Node head, Node newNode) {
			while (head.Key != newNode.Key) {
				if (head.Key < newNode.Key) {
					if (head.Right == null)
						head.Right = GetRightDescendant (head.Key);
					head = head.Right;
				} else {
					if (head.Left == null)
						head.Left = GetLeftDescendant (head.Key);
					head = head.Left;
				}
			}
			head.IsParasitic = false;
		}
		//по ключу возвращает правого паразитного потомка
		private Node GetRightDescendant (int key) {
			pair location = GetLocation (key);
			//int newKey = (1 << (location.Level - 1)) * (2 * (2 * location.Position) - 1); Сократим:
			int newKey = (1 << (location.Level - 1)) * (4 * location.Position - 1);
			return new Node (newKey, true);
		}
		//по ключу возвращает левого паразитного потомка
		private Node GetLeftDescendant (int key) {
			pair location = GetLocation (key);
			//int newKey = (1 << (location.Level - 1)) * (2 * (2 * location.Position - 1) - 1); Сократим:
			int newKey = (1 << (location.Level - 1)) * (4 * location.Position - 3);
			return new Node (newKey, true);
		}
		//обновлят EvenHead или  OddHead в зависимости от четности key
		private void SetHead (int key, Node newHead) {
			if ((key & 1) == 0) {
				EvenHead = newHead;
			} else {
				OddHead = newHead;
			}
		}
		//считается что вершина с ключом key в дереве есть
		public void Exclude (int key) {
			Node head = (key & 1) == 0 ? EvenHead : OddHead;
			Node currentNode = head;
			//последний пройденный нами узел который не надо удалять, чтобы по необходимости "подрезать" дереао снизу
			Node lastNotDeleted = head;
			while (currentNode.Key != key) {
				if (currentNode.IsParasitic == false || currentNode.Left != null && currentNode.Right != null)
					lastNotDeleted = currentNode;
				if (currentNode.Key > key)
					currentNode = currentNode.Left;
				else
					currentNode = currentNode.Right;
			}
			currentNode.IsParasitic = true;
			// если наш удаляемый узел - последний в цепочке паразитных(возможно длина цепочки 0)
			if (currentNode.Left == null && currentNode.Right == null) {
				if (lastNotDeleted.Key > key)
					lastNotDeleted.Left = null;
				else
					lastNotDeleted.Right = null;
				currentNode = lastNotDeleted;
			}
			//если мы удалили корень или одну из его ветвей то возможно надо "подрезать" дерево сверху
			if (currentNode.Key == head.Key) {
				currentNode = ReduceHead (lastNotDeleted);
				SetHead (key, currentNode);
			}
		}
		//пытается подрезать дерево сверху и возвращает новый корень
		private Node ReduceHead (Node newHead) {
			while (newHead.IsParasitic) {
				if (newHead.Left == null && newHead.Right == null) {
					newHead = null;
					break;
				} else if (newHead.Left == null) {
					newHead = newHead.Right;
				} else if (newHead.Right == null) {
					newHead = newHead.Left;
				} else {
					break;
				}
			}
			return newHead;
		}
	}
}
