namespace GraphCollection;

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

/// <summary>
/// Class <c>Graph</c> is a generic graph data structure.
/// Edges are directed and have an edge_type attribute, a string.
/// The edge_type creates a set of sub-graphs within the graph.
/// </summary>
public class Graph : object {

    // ==============================================================================================
    // PROPERTIES
    // ==============================================================================================
    // the graph is created using a sigle dict of nodes and multiple dicst of edges
    // 
    // for each edge type, there are two dicts, forward and reverse
    // these edges dicts vary based on the edge type
    // 
    // M2M forward: key = start node, value = [end nodes]
    // M2M reverse: key = start node, value = [end nodes]
    // 
    // M2O forward: key = start node, value = single end node
    // M2O reverse: key = start node, value = [end nodes]
    // 
    // O2M forward: key = start node, value = [end nodes]
    // O2M reverse: key = start node, value = single end node
    // 
    // O2O forward: key = start node, value = single end node
    // O2O reverse: key = start node, value = single end node
    //
    public static string O2O = "o2o"; // one to one
    public static string M2M = "m2m"; // many to many
    public static string M2O = "m2o"; // many to one
    public static string O2M = "o2m"; // one to many
    // Ordered dict of graph nodes: key = node name, value = dict of attribute_name -> attribute_value
    private OrderedDictionary nodes;
    // Ordered dict of graph nodes: key = edge_type, value = x2x
    private Dictionary<string, string> edge_types;
    // Dict of graph edges: key is the edge_type, value is a dict of edges 
    private Dictionary<string, OrderedDictionary> edges_fwd;
    // Dict of graph edges: key is the edge_type, value is a dict of edges 
    private Dictionary<string, OrderedDictionary> edges_rev;
    // ==============================================================================================
    // CONSTRUCTOR
    // ==============================================================================================
    public Graph() {
        this.nodes = new OrderedDictionary();
        this.edge_types = new Dictionary<string, string>();
        this.edges_fwd = new Dictionary<string, OrderedDictionary>();
        this.edges_rev = new Dictionary<string, OrderedDictionary>();
    }
    // ==============================================================================================
    // METHODS
    // ==============================================================================================
    /// <summary>
    /// Add a node to the graph. Throws an error if the node already exists.
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="attribs">the node attributes, as a Dictionary.</param>
    public void AddNode(string node, Dictionary<string, object> attribs) {
        if (this.nodes.Contains(node)) {
            throw new ArgumentException("Node already exists.");
        }
        this.nodes.Add(node, attribs);
    }
    /// <summary>
    /// Get the attributes of a node in the graph. Throws an error if the node does not exist.
    /// </summary>
    /// <param name="node">The node name.</param>
    /// <returns>The node attributes, as a Dictionary.</param>
    public Dictionary<string, object> GetNodeAttribs(string node) {
        if (!this.nodes.Contains(node)) {
            throw new ArgumentException("Node does not exist.");
        }
        object? attribs = this.nodes[node];
        if (attribs is null) throw new Exception();
        return (Dictionary<string, object>) attribs;
    }
    /// <summary>
    /// Get a list of nodes that have an outgoing edge of type edge_type.
    /// </summary>
    /// <param name="node">The node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of nodes.</param>
    public List<string> GetNodesWithOutEdge(string node, string edge_type) {
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        return (List<string>) this.edges_fwd[edge_type].Keys;
    }
    /// <summary>
    /// Get a list of nodes that have an incoming edge of type edge_type.
    /// </summary>
    /// <param name="node">The node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of nodes.</param>
    public List<string> GetNodesWithInEdge(string node, string edge_type) {
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        return (List<string>) this.edges_rev[edge_type].Keys;
    }
    /// <summary>
    /// Return True if the node exists in the graph.
    /// </summary>
    /// <param name="node">The node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of nodes.</param>
    public bool HasNode(string node) {
         return this.nodes.Contains(node);
    }
    /// <summary>
    /// Add an edge to the graph, from node <c>n0</c> to <c>n1</c>.
    /// </summary>
    /// <param name="node0">the start node name.</param>
    /// <param name="node1">the end node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    public void AddEdge(string node0, string node1, string edge_type) {
        if (!this.nodes.Contains(node0) && this.nodes.Contains(node1)) {
            throw new ArgumentException("Node does not exist.");
        }
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        string x2x = this.edge_types[edge_type];
        /// add edge from n0 to n1
        if (this.edges_fwd[edge_type].Contains(node0)) {
            object? nodes =this.edges_fwd[edge_type][node0];
            if (nodes is null) throw new Exception();
            ((List<string>) nodes).Add( node1 );
        } else {
            this.edges_fwd[edge_type][node0] = new List<string>(){node1};
        }
        /// add rev edge from n1 to n0
        if (this.edges_rev[edge_type].Contains(node1)) {
            object? nodes = this.edges_rev[edge_type][node1];
            if (nodes is null) throw new Exception();
            ((List<string>) nodes).Add( node0 );
        } else {
            this.edges_rev[edge_type][node1] = new List<string>(){node0};
        }
    }
    /// <summary>
    /// Return True if an edge from n0 to n1 exists in the graph.
    /// </summary>
    /// <param name="node0">the name of the edge start node</param>
    /// <param name="node1">the name of the edge end node</param>
    /// <param name="edge_type">the type of edge.</param>
    public bool HasEdge(string node0, string node1, string edge_type) {
        if (!this.nodes.Contains(node0) && this.nodes.Contains(node1)) {
            throw new ArgumentException("Node does not exist.");
        }
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist exists.");
        }
        if (!this.edges_fwd.ContainsKey(node0)) {
            return false;
        }
        object? nodes = this.edges_fwd[edge_type][node0];
        if (nodes is null) throw new Exception();
        return ((List<string>) nodes).Contains(node1);
    }
    /// <summary>
    /// Add an edge type to the graph.
    /// </summary>
    /// <param name="edge_type">the type of edge.</param>
    public void AddEdgeType(string edge_type, string x2x) {
        if (this.edge_types.ContainsKey(edge_type)) {
            throw new Exception("Edge type already exists.");
        }
        this.edge_types[edge_type] = x2x;
        this.edges_fwd[edge_type] = new OrderedDictionary();
        this.edges_rev[edge_type] = new OrderedDictionary();
    }
    /// <summary>
    /// Check if an edge type exists.
    /// </summary>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>True if the edge type exists, false otherwise.</returns>
    public bool HasEdgeType(string edge_type) {
        return this.edge_types.ContainsKey(edge_type);
    }
    /// <summary>
    /// Get the successors of a node.
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of strings, the node names that are downstream of the specified node.</returns>
    public string? Successor(string node, string edge_type) {
        if (!this.nodes.Contains(node)) {
            throw new ArgumentException("Node does not exist.");
        }
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        string x2x = this.edge_types[edge_type];
        if (x2x == Graph.M2M || x2x == Graph.O2M) {
            throw new Exception("Edge type " + x2x + " has multiple successors.");
        }
        if (!this.edges_fwd[edge_type].Contains(node)) {
            return null;
        }
        object? nodes = this.edges_fwd[edge_type][node];
        if (nodes is null) throw new Exception();
        return ((List<string>) nodes)[0];
    }
    /// <summary>
    /// Get the successors of a node.
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of strings, the node names that are downstream of the specified node.</returns>
    public List<string> Successors(string node, string edge_type) {
        if (!this.nodes.Contains(node)) {
            throw new ArgumentException("Node does not exist.");
        }
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        string x2x = this.edge_types[edge_type];
        if (x2x == Graph.M2O || x2x == Graph.O2O) {
            throw new Exception("Edge type " + x2x + " has one successor.");
        }
        if (!this.edges_fwd[edge_type].Contains(node)) {
            return new List<string>();
        }
        object? nodes = this.edges_fwd[edge_type][node];
        if (nodes is null) throw new Exception();
        return ((List<string>) nodes);
    }
    /// <summary>
    /// Get the predeccessors of a node.
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of strings, the node names that are upstream of the specified node.</returns>
    public string? Predecessor(string node, string edge_type) {
        if (!this.nodes.Contains(node)) {
            throw new ArgumentException("Node does not exist.");
        }
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        string x2x = this.edge_types[edge_type];
        if (x2x == Graph.M2M || x2x == Graph.M2O) {
            throw new Exception("Edge type " + x2x + " has multiple predecessors.");
        }
        if (!this.edges_rev[edge_type].Contains(node)) {
            return null;
        }
        object? nodes = this.edges_rev[edge_type][node];
        if (nodes is null) throw new Exception();
        return ((List<string>) nodes)[0];
    }
    /// <summary>
    /// Get the predeccessors of a node.
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of strings, the node names that are upstream of the specified node.</returns>
    public List<string> Predecessors(string node, string edge_type) {
        if (!this.nodes.Contains(node)) {
            throw new ArgumentException("Node does not exist.");
        }
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        string x2x = this.edge_types[edge_type];
        if (x2x == Graph.O2M || x2x == Graph.O2O) {
            throw new Exception("Edge type " + x2x + " has one predecessor.");
        }
        if (!this.edges_rev[edge_type].Contains(node)) {
            return new List<string>();
        }
        object? nodes = this.edges_rev[edge_type][node];
        if (nodes is null) throw new Exception();
        return (List<string>) nodes;
    }
    /// <summary>
    /// Count the the number of incoming edges.
    /// The 'in degree' is the number of reverse edges of type 'ent_type' linked to node 'n'.
    /// </summary>
    /// <param name="node">the name of the node for which to count incoming edges.</param>
    /// <param name="edge_type">the edge type.</param>
    /// <returns>An integer, the number of incoming edges.</returns>
    public int DegreeIn(string node, string edge_type) {
        if (!this.nodes.Contains(node)) {
            throw new ArgumentException("Node does not exist.");
        }
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        string x2x = this.edge_types[edge_type];
        /// calc reverse degree
        if (!this.edges_rev[edge_type].Contains(node)) {
            return 0;
        } else if (x2x == Graph.M2M || x2x == Graph.M2O) {
            object? nodes = this.edges_rev[edge_type][node];
            if (nodes is null) throw new Exception();
            return ((List<string>) nodes).Count;
        } else {
            return 1;
        }
    }
    /// <summary>
    /// Count the the number of outgoing edges.
    /// The 'out degree' is the number of forward edges of type 'ent_type' linked to node 'n'.
    /// </summary>
    /// <param name="node">the name of the node for which to count outgoing edges.</param>
    /// <param name="edge_type">the edge type.</param>
    /// <returns>An integer, the number of outgoing edges.</returns>
    public int DegreeOut(string node, string edge_type) {
        if (!this.nodes.Contains(node)) {
            throw new ArgumentException("Node does not exist.");
        }
        if (!this.edge_types.ContainsKey(edge_type)) {
            throw new ArgumentException("Edge type does not exist.");
        }
        string x2x = this.edge_types[edge_type];
        /// calc reverse degree
        if (!this.edges_fwd[edge_type].Contains(node)) {
            return 0;
        } else if (x2x == Graph.M2M || x2x == Graph.M2O) {
            object? nodes = this.edges_fwd[edge_type][node];
            if (nodes is null) throw new Exception();
            return  ((List<string>) nodes).Count;
        } else {
            return 1;
        }
    }
    /// <summary>
    /// Get the total number of edges connected to a node, both incoming and outgoing.
    /// </summary>
    /// <param name="node">the name of the node for which to count edges.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>An integer, the number of edges.</returns>
    public int Degree(string node, string edge_type) {
        return this.DegreeIn(node, edge_type) + this.DegreeOut(node, edge_type);
    }
}


