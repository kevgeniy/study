using System;
using System.Collections.Generic;
using System.Text;

namespace graph
{
    class Program
    {
        public static void Main()
        {
            /*
            string str1 = "1 2 2 3 3 4 2 5";
            Graph graph1 = new Graph(str1.StrToGraph());

            foreach (var item in graph1.GetNodes())
                Console.WriteLine(item);

            string str2 = "1 2 2 3 2 4 3 6 1 6";
            Graph graph2 = new Graph(str2.StrToGraph());

            Graph graph3 = graph1 + graph2;

            foreach (var item in graph1.GetNodes())
                Console.WriteLine(item);
            foreach (var item in graph1.GetNodes())
            {
                Console.WriteLine(item);
                IArc[] arcs;
                if (graph1.TryGetArcs(item, out arcs))
                    foreach (var element in arcs)
                        Console.WriteLine(element);
                else
                    Console.WriteLine("no arcs");
            }*/

            Components a = new Components();
            a.AddArc(new Arc(new Node(1), new Node(2)));
            a.AddArc(new Arc(new Node(1), new Node(3)));
            a.RemoveArc(new Arc(new Node(1), new Node(2)));
            a.AddArc(new Arc(new Node(4), new Node(5)));
            var  b = a.AllNodes(1);
            foreach(var item in b)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
public static class GraphHelper
{
    public static Incidence StrToGraph(this string representation)
    {
        var lines = representation.Split(' ', ',', ';');
        Incidence graph = new Incidence();
        for(int i = 0; i < lines.Length; i += 2)
        {
            graph.AddArc( new Arc( new Node(Convert.ToInt32(lines[i])), new Node(Convert.ToInt32(lines[i + 1])) ) );
        }
        return graph;
    }
}
 
#region 1. Interfaces

#region 1.1 IGraph
public interface IGraph
{
    bool TryGetNode(int ID, out INode node);
    bool TryGetArcs(INode node, out IArc[] arcs);
    bool TryGetNodes(INode node, out INode[] nodes);
    INode[] GetNodes();
    IArc[] GetArcs();
    void RemoveNode(INode node);
    void RemoveArc(IArc arc);
    void AddNode(INode node);
    void AddArc(IArc arc);
    IGraph Clone();
}
#endregion

#region 1.2 INode
public interface INode : IEquatable<INode>
{
    int ID { get; }
    int Color { get; set; }
    INode Clone();
}
#endregion

#region 1.3 IArc
public interface IArc : IEquatable<IArc>
{
    INode Begin { get; }
    INode End { get; }
    int Color { get; set; }
    IArc Clone();                                                   // not necessery really
}
#endregion

#endregion

#region 2. Base implementations of the interfaces

#region 2.1 INode

/// <summary>
/// INode's base implementation without anything yet
/// </summary>
public class Node : INode
{
    private readonly int _id;
    public int ID { get { return _id; } }
    public int Color { get; set; }

    public Node(int ID, int color = 0)
        : base()
    {
        this._id = ID;
        this.Color = color;
    }
    public INode GetNode(int ID, int color = 0)                                                     //not necessary
    {
        return new Node(ID, color);
    }
    public bool Equals(INode node)
    {
        return node != null && node.ID == this.ID;
    }
    public override bool Equals(object obj)
    {
        if (obj is INode)
            return Equals(obj as INode);
        return false;
    }
    public override string ToString()
    {
        return String.Format("ID = {0}, Color = {1}", ID, Color);
    }
    public override int GetHashCode()
    {
        return ID;
    }
    public INode Clone()
    {
        return new Node(ID);
    }
}
#endregion

#region 2.2 IArc
/// <summary>
/// IArc's base implementation without anything yet
/// </summary>
public class Arc : IArc 
{
    private readonly INode _begin;
    private readonly INode _end;
    public INode Begin { get { return _begin; } }
    public INode End { get { return _end; } }
    public int Color { get; set; }
    public Arc(INode begin, INode end, int color = 0)
        : base()
    {
        if (begin == null || end == null)
            throw new ArgumentNullException();
        this._begin = begin;
        this._end = end;
        this.Color = color;
    }
    public bool Equals(IArc arc)
    {
        return arc != null && (this.Begin.Equals(arc.Begin) && this.End.Equals(arc.End) || this.End.Equals(arc.Begin) && this.Begin.Equals(arc.End));
    }
    public override bool Equals(object obj)
    {
        if (obj is IArc)
            return Equals(obj as IArc);
        return false;
    }
    public override string ToString()
    {
        return String.Format("BeginNodeID = {0}, EndNodeID = {1}, Color = {2}", Begin.ID, End.ID, Color);
    }
    public override int GetHashCode()
    {
        return Begin.ID + End.ID * 37;
    }
    public IArc Clone()
    {
        return new Arc(_begin.Clone(), _end.Clone());
    }
}

#endregion

#region 2.3 IGraph

/// <summary>
/// IGraph's base implementation
/// </summary>
public class Incidence: IGraph
{
    protected Dictionary<INode, List<IArc>> _adjacencyList;
    protected List<INode> _nodes;
    protected List<IArc> _arcs;

	public INode GetNode(int i) {
		if (i < 0 || i >= _nodes.Count)
			throw new IndexOutOfRangeException();
		return _nodes[i];
	}

	public bool TryGetNode(int i, out INode node) {
		node = null;
		if (i < 0 || i >= _nodes.Count)
			return false;
		node = _nodes[i];
		return true;
	}

    public Incidence()
    {
        _adjacencyList = new Dictionary<INode, List<IArc>>();
        _nodes = new List<INode>();
        _arcs = new List<IArc>();
    }

   #region Get/TryGet methods
    public bool TryGetNode(int ID, out INode node)
    {
        foreach(INode element in _nodes)
        {
            if(element.ID == ID)
            {
                node = element;
                return true;
            }
        }
        node = null; 
        return false;
    }
    public bool TryGetNodes(INode node, out INode[] nodes)
    {
        IArc[] arcs;
        nodes = null;
        if (!TryGetArcs(node, out arcs))
            return false;

        List<INode> ans = new List<INode>();
        foreach (var item in arcs)
        {
            ans.Add(item.Begin.Equals(node) ? item.End : item.Begin);
        }
        nodes = ans.ToArray();
        return true;
    }
    public bool TryGetArcs(INode node, out IArc[] arcs)
    {
        if (node == null)
            throw new ArgumentNullException();
        if (_nodes == null || _adjacencyList == null)
            throw new NullReferenceException();

        foreach(INode element in _nodes)
            if (node.Equals(element))
            {
                arcs = _adjacencyList[node].ToArray();
                return true;
            }
        arcs = null;
        return false;                                                                                    ///TODO null or default???
    }
    public INode[] GetNodes()
    {
        if (_nodes == null)
            throw new NullReferenceException();
        return _nodes.ToArray();
    }
    public IArc[] GetArcs()
    {
        if (_arcs == null)
            throw new NullReferenceException();
        return _arcs.ToArray();
    }

#endregion

   #region Remove/Add methods
    public virtual void RemoveNode(INode node)
    {
        if (node == null)
            throw new ArgumentNullException();
        if (_nodes == null || _arcs == null || _adjacencyList == null)
            throw new NullReferenceException();

        foreach (IArc arc in _adjacencyList[node])
        {
            _adjacencyList[arc.Begin.Equals(node) ? arc.End : arc.Begin].Remove(arc);
            _arcs.Remove(arc);
        }
        _nodes.Remove(node);
        _adjacencyList.Remove(node);
    }
    public virtual void RemoveArc(IArc arc)
    {
        if (arc == null)
            throw new ArgumentNullException();
        if (_arcs == null || _adjacencyList == null)
            throw new NullReferenceException();

        _arcs.Remove(arc);
        _adjacencyList[arc.Begin].Remove(arc);
        _adjacencyList[arc.End].Remove(arc);
    }
    public virtual void AddNode(INode node)
    {
        if (node == null)
            throw new ArgumentNullException();
        if (_nodes == null ||_adjacencyList == null)
            throw new NullReferenceException();

        foreach(INode element in _nodes)
        {
            if (node.Equals(element))
                return;
        }
        INode newNode = node.Clone();
        _nodes.Add(newNode);
        _adjacencyList[newNode] = new List<IArc>();
    }
    public virtual void AddArc(IArc arc)
    {
        if (arc == null)
            throw new ArgumentNullException();
        if (_arcs == null || _adjacencyList == null)
            throw new NullReferenceException();

        foreach(IArc element in _arcs)
        {
            if (element.Equals(arc))
                return;
        }
        IArc newArc = arc.Clone();
        INode begin = newArc.Begin;
        INode end = newArc.End;
        AddNode(begin);
        AddNode(end);

        _arcs.Add(newArc);
        _adjacencyList[end].Add(newArc);
        _adjacencyList[begin].Add(newArc);
    }
   #endregion

   #region Object method's override
    public override int GetHashCode()
    {
        int hash = 0;
        foreach(INode node in _nodes)
        {
            hash ^= node.ID * 3;
        }
        return hash;
    }
    public bool Equals(Incidence graph)
    {
        if (graph._nodes.Count != _nodes.Count || graph._arcs.Count != _arcs.Count)
            return false;
        foreach(INode node in _nodes)
        {
            if (!graph._nodes.Contains(node))
                return false;
        }
        foreach(IArc arc in _arcs)
        {
            if (!graph._arcs.Contains(arc))
                return false;
        }
        return true;
    }
    public override bool Equals(object obj)
    {
        if (obj is Incidence)
            return Equals(obj as Incidence);
        return false;
    }
    public override string ToString()
    {
        return String.Format("Nodes {0}, Arcs {1}", _nodes.Count, _arcs.Count);
    }
    #endregion

    public IGraph Clone()
    {
        Incidence incidence = new Incidence();
        for (int i = 0; i < _arcs.Count; i++)
        {
            incidence.AddArc(_arcs[i]);
        }
        return incidence;

    }
}
#endregion

#region 2.4 Components : IGraph

public class Components : Incidence, IGraph
{
    Dictionary<INode, int> _components;
    int _maxComponent;
    public Components()
        :base()
    {
        _components = new Dictionary<INode, int>();
    }
    public INode[] AllNodes(int component)
    {
        List<INode> nodes = new List<INode>();
        foreach (var item in _components)
        {
            if (item.Value == component)
                nodes.Add(item.Key);
        }
        return nodes.ToArray();
    }
    private void BFS(INode node, int newComponent)
    {
        Queue<INode> queue = new Queue<INode>();
        queue.Enqueue(node);
        _components[node] = newComponent;
        while (queue.Count != 0)
        {
            INode nd = queue.Dequeue();
            INode[] nodes;
            if (!this.TryGetNodes(nd, out nodes))
                continue;
            for(int i = 0; i < nodes.Length; i++)
                if (_components[nodes[i]] != newComponent)
                {
                    _components[nodes[i]] = newComponent;
                    queue.Enqueue(nodes[i]);
                }
        }
        
    }

    public override void AddArc(IArc arc)
    {
        if (arc == null)
            throw new ArgumentNullException();
        if (_arcs == null || _adjacencyList == null)
            throw new NullReferenceException();

        foreach (IArc element in _arcs)
        {
            if (element.Equals(arc))
                return;
        }
        IArc newArc = arc.Clone();

        _arcs.Add(newArc);
        INode begin = newArc.Begin;
        INode end = newArc.End;
        base.AddNode(begin);
        base.AddNode(end);
        _adjacencyList[end].Add(newArc);
        _adjacencyList[begin].Add(newArc);
        if(!_components.ContainsKey(begin))
        {
            if (!_components.ContainsKey(end))
                _components[end] = _components[begin] = ++_maxComponent;
            else
                _components[begin] = _components[end];
        }
        else
        {
            if (!_components.ContainsKey(end))
                _components[end] = _components[begin];
            else
                BFS(begin, Math.Min(_components[begin], _components[end]));
        }

    }
    public override void AddNode(INode node)
    {
        if (node == null)
            throw new ArgumentNullException();
        if (_nodes == null || _adjacencyList == null)
            throw new NullReferenceException();
        foreach (INode element in _nodes)
        {
            if (node.Equals(element))
                return;
        }
        INode newNode = node.Clone();
        _nodes.Add(newNode);
        _components[newNode] = ++_maxComponent;
        _adjacencyList[newNode] = new List<IArc>();
    }
    public override void RemoveArc(IArc arc)
    {
        if (arc == null)
            throw new ArgumentNullException();
        if (_arcs == null || _adjacencyList == null)
            throw new NullReferenceException();
        _arcs.Remove(arc);
        _adjacencyList[arc.Begin].Remove(arc);
        _adjacencyList[arc.End].Remove(arc);
        if (_components[arc.End] == _components[arc.Begin])
        {
            _components[arc.End] *= -1;
            BFS(arc.Begin, _components[arc.Begin]);
            if (_components[arc.End] != _components[arc.Begin])
                BFS(arc.End, ++_maxComponent);
        }
    }
    public override void RemoveNode(INode node)
    {
        if (node == null)
            throw new ArgumentNullException();
        if (_nodes == null || _arcs == null || _adjacencyList == null)
            throw new NullReferenceException();

        INode[] nodes;
        bool flag = this.TryGetNodes(node, out nodes);
        _nodes.Remove(node);
        _components.Remove(node);
        foreach (IArc arc in _adjacencyList[node])
        {
            _adjacencyList[arc.Begin.Equals(node) ? arc.End : arc.Begin].Remove(arc);
            _arcs.Remove(arc);
        }
        _adjacencyList.Remove(node);

        if (flag)
        {
            for (int i = 1; i < nodes.Length; i++)
            {
                _components[nodes[i]] *= -1;
            }
            BFS(nodes[0], _components[nodes[0]]);
            for (int i = 1; i < nodes.Length; i++)
                if (_components[nodes[i]] < 0)
                    BFS(nodes[i], ++_maxComponent);
        }

    }
}
#endregion

#region 2.3 IGraph Extension class (DI)

/// <summary>
/// Useful Extension for Incidence
/// </summary>
public class Graph: IGraph
{
    //inside graph
    IGraph _insideGraph;

    /// <summary>
    /// single constructor
    /// </summary>
    /// <param name="insideGraph"> IGraph for inside representation</param>
    public Graph(IGraph insideGraph)
    {
        if (insideGraph == null)
            throw new ArgumentNullException();
        _insideGraph = insideGraph;
    }

    #region Get/TryGet methods
    /// <summary>
    /// InsideGraph important implementation
    /// </summary>
    /// <returns></returns>
    public INode[] GetNodes()
    {
        if (_insideGraph == null)
            throw new NullReferenceException();
        return _insideGraph.GetNodes();
    }
    public IArc[] GetArcs()
    {
        if (_insideGraph == null)
            throw new NullReferenceException();
        return _insideGraph.GetArcs();
    }
    public bool TryGetArcs(INode node, out IArc[] arcs)
    {
        if (node == null)
            throw new ArgumentNullException();
        if (_insideGraph == null)
            throw new NullReferenceException();
        return _insideGraph.TryGetArcs(node, out arcs);
    }    
    public bool TryGetNode(int ID, out INode node)
    {
        if (_insideGraph == null)
            throw new NullReferenceException();
        return _insideGraph.TryGetNode(ID, out node);
    }
    public bool TryGetNodes(INode node, out INode[] nodes)
    {
        return TryGetNodes(node, out nodes);
    }
    #endregion


    /// <summary>
    /// check if is such arc
    /// </summary>
    /// <param name="arc"></param>
    /// <returns></returns>
    public bool IsArc(IArc arc)
    {
        if (_insideGraph == null)
            throw new NullReferenceException();
        INode node;
        if (_insideGraph.TryGetNode(arc.Begin.ID, out node) && _insideGraph.TryGetNode(arc.End.ID, out node))
            return true;
        return false;

    }

    #region Add/Remove methods
        #region 1. AddNode
    /// <summary>
    /// Group of methods and operator overloads that add some (0-...) nodes to the graph, 
    /// AddNode(INode node) only is basic method
    /// </summary>
    /// <param name="node"></param>
    public virtual void AddNode(INode node)
    {
        if (node == null)
            throw new ArgumentNullException();
        if (_insideGraph == null)
            throw new NullReferenceException();
        _insideGraph.AddNode(node.Clone());
    }
    public void AddNode(INode node1, INode node2)
    {
        if (node1 == null || node2 == null)
            throw new ArgumentNullException();
            AddNode(node1);
            AddNode(node2);
    }
    public void AddNode(INode node1, INode node2, INode node3)
    {
        if (node1 == null || node2 == null || node3 == null)
            throw new ArgumentNullException();
            AddNode(node1);
            AddNode(node2);
            AddNode(node3);
    }
    public void AddNode(INode node1, INode node2, INode node3, INode node4)
    {
        if (node1 == null || node2 == null || node3 == null || node4 == null)
            throw new ArgumentNullException();
            AddNode(node1);
            AddNode(node2);
            AddNode(node3);
            AddNode(node4);
    }
    public void AddNode(INode node1, INode node2, INode node3, INode node4, __arglist)
    {
        ArgIterator argIterator = new ArgIterator(__arglist);
        int num = argIterator.GetRemainingCount() + 4;
        INode[] array = new INode[num];
        array[0] = node1;
        array[1] = node2;
        array[2] = node3;
        array[3] = node4;
        for (int i = 4; i < num; i++)
            array[i] = (INode)TypedReference.ToObject(argIterator.GetNextArg());
        AddNode(array);
    }
    public void AddNode(INode[] nodes)
    {
        if (nodes == null)
            throw new ArgumentNullException();
        for (int i = 0; i < nodes.Length; i++)
            AddNode(nodes[i]);
    }
    public static Graph operator +(Graph graph, INode node)
    {
        if (graph == null || node == null)
            throw new ArgumentNullException();
        Graph newGraph = new Graph(graph._insideGraph.Clone());
        newGraph.AddNode(node);
        return newGraph;
    }
    public static Graph operator +(INode node, Graph graph)
    {
        return graph + node;
    }
    #endregion

        #region 2. AddArc
    /// <summary>
    /// Group of methods and operator overloads that add some (0-...) arcs to the graph, 
    /// AddArc(IArc arc) only is basic method
    /// </summary>
    /// <param name="arc"></param>
    public virtual void AddArc(IArc arc)
    {
        if (arc == null)
            throw new ArgumentNullException();
        if (_insideGraph == null)
            throw new NullReferenceException();
        _insideGraph.AddArc(arc.Clone());
    }
    public void AddArc(IArc arc1, IArc arc2)
    {
        if (arc1 == null || arc2 == null)
            throw new ArgumentNullException();
        AddArc(arc1);
        AddArc(arc2);
    }
    public void AddArc(IArc arc1, IArc arc2, IArc arc3)
    {
        if (arc1 == null || arc2 == null || arc3 == null)
            throw new ArgumentNullException();
            AddArc(arc1);
            AddArc(arc2);
            AddArc(arc3);
    }
    public void AddArc(IArc arc1, IArc arc2, IArc arc3, IArc arc4)
    {
        if (arc1 == null || arc2 == null || arc3 == null || arc4 == null)
            throw new ArgumentNullException();
            AddArc(arc1);
            AddArc(arc2);
            AddArc(arc3);
            AddArc(arc4);
    }
    public void AddArc(IArc arc1, IArc arc2, IArc arc3, IArc arc4, __arglist)
    {
        ArgIterator argIterator = new ArgIterator(__arglist);
        int num = argIterator.GetRemainingCount() + 4;
        IArc[] array = new IArc[num];
        array[0] = arc1;
        array[1] = arc2;
        array[2] = arc3;
        array[3] = arc4;
        for (int i = 4; i < num; i++)
            array[i] = (IArc)TypedReference.ToObject(argIterator.GetNextArg());
    }
    public void AddArc(IArc[] arcs)
    {
        if (arcs == null)
            throw new ArgumentNullException();
        for (int i = 0; i < arcs.Length; i++)
            AddArc(arcs[i]);
    }
    public static Graph operator +(Graph graph, IArc arc)
    {
        if (graph == null || arc == null)
            throw new ArgumentNullException();
        Graph newGraph = new Graph(graph._insideGraph.Clone());
        newGraph.AddArc(arc);
        return newGraph;
    }
    public static Graph operator +(IArc arc, Graph graph)
    {
        return graph + arc;
    }
    #endregion

        #region 3. RemoveNode
    /// <summary>
    /// Method and operator overload that remove node from the graph
    /// </summary>
    /// <param name="node"></param>
    public virtual void RemoveNode(INode node)
    {
        if (node == null)
            throw new ArgumentNullException();
        if (_insideGraph == null)
            throw new NullReferenceException();
        _insideGraph.RemoveNode(node);
    }
    public static Graph operator -(Graph graph, INode node)
    {
        if (graph == null || node == null)
            throw new ArgumentNullException();
        Graph newGraph = new Graph(graph._insideGraph.Clone());
        newGraph.RemoveNode(node);
        return newGraph;
    }
    #endregion

        #region 4. RemoveArc
    /// <summary>
    /// Method and operator overload that remove arc from the graph
    /// </summary>
    /// <param name="arc"></param>
    public virtual void RemoveArc(IArc arc)
    {
        if (arc == null)
            throw new ArgumentNullException();
        if (_insideGraph == null)
            throw new NullReferenceException();
        _insideGraph.RemoveArc(arc);
    }
    public static Graph operator -(Graph graph, IArc arc)
    {
        if (graph == null || arc == null)
            throw new ArgumentNullException();
        Graph newGraph = new Graph(graph._insideGraph.Clone());
        newGraph.RemoveArc(arc);
        return newGraph;
    }
    #endregion

        #region 5. AddGraph
    /// <summary>
    /// Method and operator overload that add graph to the graph
    /// </summary>
    /// <param name="graph"></param>
    public void AddGraph(Graph graph)
    {
        if (graph == null)
            throw new ArgumentNullException();
        Graph newGraph = new Graph(graph._insideGraph.Clone());
        INode[] nodes = newGraph.GetNodes();
        for (int i = 0; i < nodes.Length; i++)
        {
            IArc[] arcs;
            if (!newGraph.TryGetArcs(nodes[i], out arcs) || arcs.Length == 0) 
                this.AddNode(nodes[i]);
            else
                for (int j = 0; j < arcs.Length; j++)
                    this.AddArc(arcs[j]);
        }
    }
    public static Graph operator +(Graph first, Graph second)
    {
        if (first == null || second == null)
            throw new ArgumentNullException();
        Graph newGraphFirst = new Graph(first._insideGraph.Clone());
        newGraphFirst.AddGraph(second);
        return newGraphFirst;
    }
    #endregion

        #region 6. RemoveGraph
    /// <summary>
    /// Method and operator overload that remove graph from the graph
    /// </summary>
    /// <param name="graph"></param>
    public void RemoveGraph(Graph graph)
    {
        if (graph == null)
            throw new ArgumentNullException();
        Graph newGraph = new Graph(graph._insideGraph.Clone());
        INode[] nodes = newGraph.GetNodes();
        if (nodes == null)
            throw new NullReferenceException();
        for (int i = 0; i < nodes.Length; i++)
            RemoveNode(nodes[i]);
    }
    public static Graph operator -(Graph first, Graph second)
    {
        if (first == null || second == null)
            throw new ArgumentNullException();
        Graph newGraphFirst = new Graph(first._insideGraph.Clone());
        newGraphFirst.RemoveGraph(second);
        return newGraphFirst;
    }
#endregion
    #endregion

    #region Object methods overlods
    public bool Equals(Graph graph)
    {
        return _insideGraph.Equals(graph as IGraph);
    }
    public override bool Equals(object obj)
    {
        return _insideGraph.Equals(obj);
    }
    public override int GetHashCode()
    {
        return _insideGraph.GetHashCode();
    }
    public override string ToString()
    {
        return _insideGraph.ToString();
    }
    #endregion
    public IGraph Clone()
    {
        return new Graph(_insideGraph.Clone());
    }
}
#endregion

#endregion