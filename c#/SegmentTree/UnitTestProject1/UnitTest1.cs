using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApplication4;


namespace UnitTestProject1 {


	[TestClass]
	public class UnitTest1 {
		public Tree<AddMonoid, AddMonoid, IdentityMeasure<int, int>, int, int> getter(){
			
			Action<Node<AddMonoid, AddMonoid, IdentityMeasure<int, int>, int, int>> IncreaseUpdate = (tree) => {
				tree.State = state.None;
				if (tree.Left == null && tree.Right == null) {
					tree.Data.Data += tree.Change;
					tree.Measure = tree.Data.Measure;
					return;
				}
				tree.Measure += tree.Size * tree.Change;
				if (tree.Left != null)
					DoIncreaseUpdate(tree.Left, tree);
				if (tree.Right != null)
					DoIncreaseUpdate(tree.Right, tree);
			};
			Action<Node<AddMonoid, AddMonoid, IdentityMeasure<int, int>, int, int>> AssigneUpdate = (tree) => {
				tree.State = state.None;
				if (tree.Left == null && tree.Right == null) {
					tree.Data.Data = tree.Change;
					tree.Measure = tree.Data.Measure;
					return;
				}
				tree.Measure = tree.Size * tree.Change;
				if (tree.Left != null)
					DoAssignUpdate(tree.Left, tree);
				if (tree.Right != null)
					DoAssignUpdate(tree.Right, tree);
			};
			Tree<AddMonoid, AddMonoid, IdentityMeasure<int, int>, int, int> currentTree = new Tree<AddMonoid,	AddMonoid, IdentityMeasure<int, int>, int, int>(
					new int[] { 1, -1, 1, -1, 1, -1, 1 },
					AssigneUpdate,
					IncreaseUpdate,
					(a) => new IdentityMeasure<int, int>(a));
			return currentTree;
	}
		[TestMethod]
		public void  Test() {
			var currentTree = getter();
			currentTree.Assign(0, 7, 0);
			currentTree.Increase(0, 7, 5);
			if (currentTree.MeasureOn(0, 7) != 35)
				Console.WriteLine("Fail in Test1");
		}
		[TestMethod]
		public void Test2() {
			var currentTree = getter();
			currentTree.Increase(0, 3, 6);
			currentTree.Assign(2, 5, 3);
			if (currentTree.MeasureOn(0, 7) != 37)
				Console.WriteLine("Fail in Test2");
		}
		[TestMethod]
		public void Test3() {
			var currentTree = getter();
			currentTree.Assign(0, 1, 4);
			if (currentTree.MeasureOn(0, 7) != 30)
				Console.WriteLine("Fail in Test3");
		}

		[TestMethod]
		public void Test4() {
			var currentTree = getter();
			currentTree.Increase(4, 3, 1);
			if (currentTree.MeasureOn(0, 7) != 33)
				Console.WriteLine("Fail in Test4");
		}

		[TestMethod]
		public void Test5() {
			var currentTree = getter();
			currentTree.Increase(3, 3, 3);
			currentTree.Increase(3, 3, -3);
			currentTree.Assign(0, 1, 2);
			if (currentTree.MeasureOn(0, 7) != 31)
				Console.WriteLine("Fail in Test5");
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
}
