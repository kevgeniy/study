using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuickGraph;
using System.IO;

namespace graphOrder {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public IBidirectionalGraph<object, IEdge<object>> GraphToVisualize { get;	private set; }
		public MainWindow() {
			CreateGraphToVisualize();
			InitializeComponent();
		}

		struct segment {
			public int Begin { get; set; }
			public int End { get; set; }
			public segment(int begin, int end)
				:this()	{
				this.Begin = begin;
				this.End = end;
			}
		}

		private bool TryToCompare(segment first, segment second, out int ans) {
				segment delta = new segment(first.Begin - second.Begin, first.End - second.End);
				if (delta.Begin == 0 && delta.End == 0) {
					ans = 0;
					return true;
				}
				if (delta.Begin <= 0 && delta.End >= 0) {
					ans = 1;
					return true;
				}
				if (delta.Begin >= 0 && delta.End <= 0) {
					ans = -1;
					return true;
				}
				ans = 0;
				return false;
			}

		private void CreateGraphToVisualize() {
			segment[] graph = GetGraph("input.txt");
			BidirectionalGraph<object, IEdge<object>> g = new BidirectionalGraph<object,IEdge<object>>();
			for (int i = 0; i < graph.Length; i++)
				g.AddVertex(i);
			for (int i = 0; i < graph.Length; i++) {
				int ans;
				for (int j = i + 1; j < graph.Length; j++)
					if (TryToCompare(graph[i], graph[j], out ans)) {
						if (ans > 0)
							g.AddEdge(new Edge<object>(j, i));
						if (ans < 0)
							g.AddEdge(new Edge<object>(i, j));
						else
							g.AddEdge(new Edge<object>(i, j));
						g.AddEdge(new Edge<object>(j, i));
					}
			}
			GraphToVisualize = g;
		}
		private segment[] GetGraph(string fileName) {
			List<segment> segments = new List<segment>();
			StreamReader reader = new StreamReader(fileName);
			while (!reader.EndOfStream) {
				segments.Add(ParseSegment(reader.ReadLine()));
			}
			return segments.ToArray();
		}
		private segment ParseSegment(string line) {
			string[] lines = line.Split(' ');
			segment currentSegment;
			if(lines.Length < 2)
				throw new Exception("Uncorrect segment in input file");
			currentSegment = new segment (Convert.ToInt32(lines[0]), Convert.ToInt32(lines[1]));
			for (int i = 1; i < lines.Length / 2; i++) {
				segment newSegment = new segment(Convert.ToInt32(lines[2 * i]), Convert.ToInt32(lines[2 * i + 1]));
				if (!TryToIntersect(ref currentSegment, newSegment))
					throw new Exception("Uncorrect segment in input file");
			}
			return currentSegment;

		}
		private bool TryToIntersect(ref segment currentSegment, segment newSegment) {
			var copySegment = new segment(Math.Max(currentSegment.Begin, newSegment.Begin), Math.Min(currentSegment.End, newSegment.End));
			if (copySegment.Begin > copySegment.End)
				return false;
			currentSegment = copySegment;
			return true;
		}
	}
}
