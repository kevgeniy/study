using System;

namespace AVLTree {
	class MainClass {
		public static void Main (string[] args) {
			AVLTree avlTree = new AVLTree ();
			avlTree.Include (1);
			avlTree.Include (22);
			avlTree.Include (25);
			Console.WriteLine ();
		}
	}

	class AVLTree {
		public Node Root { get; private set; }
		//объект вершина
		private class Node {
			public Node Left;
			public Node Right;
			public int Delta;
			public readonly int Key;

			public Node (int key) {
				this.Key = key;
			}

			public Node (int key, Node left, Node right) {
				this.Key = key;
				this.Right = right;
				this.Left = left;
			}
		}

		public void Include (int key) {
			if (Root == null)
				Root = new Node (key);
			else
				IncludeKey (key, Root);
		}

		/// <summary>
		/// Include the specified key and root. Возвращает логическое значение, показывающее, изменилась ли высота поддерева
		/// метод Exclude абсолютно аналогичен, только при вызове LeftLongUpdate или RightLongUpdate надо возвращать true а не false
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="root">Root.</param>
		private bool IncludeKey (int key, ref Node root) {
			if (root.Key == key) {
				return false;
			} else if (root.Key > key) {
				if (root.Left == null) {
					root.Left = new Node (key);
					root.Delta -= 1;
					if (root.Right == null)
						return true;
					return false;
				}
				bool fl = IncludeKey (key, ref root.Left);
				if (fl)
					root.Delta -= 1;
				if (root.Delta == -2)
					root = LeftLongUpdate (root);
				return false;
			} else {
				if (root.Right == null) {
					root.Right = new Node (key);
					root.Delta += 1;
					if (root.Left == null)
						return true;
					return false;
				}
				bool fl = IncludeKey (key, ref root.Right);
				if (fl)
					root.Delta += 1;
				if (root.Delta == 2)
					root = RightLongUpdate (root);
				return false;
			}
		}
		//правое вращение
		private void RightRotate (Node root) {
			Node left = root.Left;
			root.Left = left.Right;
			left.Right = root;
			root.Delta = left.Delta + 1;
			left.Delta = root.Delta;
			return left;
		}
		//левое вращение
		private void LeftRotate (Node root) {
			Node right = root.Right;
			root.Right = right.Left;
			right.Left = root;
			root.Delta = right.Delta - 1;
			right.Delta = root.Delta;
			return right;
		}
		//если правое поддерево root длиннее на 2 чем левое
		private Node RightLongUpdate (Node root) {
			if (root.Right.Delta >= 0)
				return LeftRotate (root);
			else {
				root.Right = RightRotate (root.Right);
				return LeftRotate (root);
			}
		}
		//если левое поддерево root длиннее на 2 чем правое
		private Node LeftLongUpdate (Node root) {
			if (root.Left <= 0)
				return RightRotate (root);
			else {
				root.Left = LeftRotate (root.Left);
				return RightRotate (root);
			}
		}
	}
}
