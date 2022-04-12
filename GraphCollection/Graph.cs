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

    // Ordered dict of graph nodes: key = node name, value = dict of attribute_name -> attribute_value
    private OrderedDictionary nodes;
    // Dict of graph edges: key = edge_type, value = ordered dict of node -> list of nodes
    private Dictionary<string, OrderedDictionary> edges;
    // Dict of graph edges: key = edge_type, value = ordered dict of node -> list of nodes
    private Dictionary<string, OrderedDictionary> redges;
        
    // ==============================================================================================
    // CONSTRUCTOR
    // ==============================================================================================

    public Graph(string[] edge_types) {
        this.nodes = new OrderedDictionary();
        this.edges = new Dictionary<string, OrderedDictionary>();
        this.redges = new Dictionary<string, OrderedDictionary>();
        foreach (string edge_type in edge_types) {
            this.edges.Add(edge_type, new OrderedDictionary());
            this.redges.Add(edge_type, new OrderedDictionary());
        }
    }
    
    // ==============================================================================================
    // METHODS
    // ==============================================================================================

    /// <summary>
    /// Add a node to the graph
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="attribs">the node attributes, as a Dictionary.</param>
    public void AddNode(string node, Dictionary<string, object> attribs) {
        if (this.nodes.Contains(node)) {
            throw new Exception("Node already exists.");
        }
        this.nodes.Add(node, attribs);
        foreach (string edge_type in this.edges.Keys) {
            this.edges[edge_type].Add(node, new List<string>());
        }
        foreach (string edge_type in this.redges.Keys) {
            this.redges[edge_type].Add(node, new List<string>());
        }
    }

    /// <summary>
    /// Add an edge to the graph, from node <c>n0</c> to <c>n1</c>.
    /// </summary>
    /// <param name="node0">the start node name.</param>
    /// <param name="node1">the end node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    public void AddEdge(string node0, string node1, string edge_type) {
        if (!this.nodes.Contains(node0) && this.nodes.Contains(node1)) {
            throw new Exception("Node does not exist.");
        }
        List<string> nodes;
        nodes = (List<string>) this.edges[edge_type][node0];
        nodes.Add(node1);
        nodes = (List<string>) this.redges[edge_type][node1];
        nodes.Add(node0);
    }

    /// <summary>
    /// Add an edge type to the graph.
    /// </summary>
    /// <param name="edge_type">the type of edge.</param>
    public void AddEdgeType(string edge_type) {
        if (this.edges.ContainsKey(edge_type)) {
            throw new Exception("Edge type already exists.");
        }
        this.edges.Add(edge_type, new OrderedDictionary());
        this.redges.Add(edge_type, new OrderedDictionary());
    }

    /// <summary>
    /// Check if the node exists.
    /// </summary>
    /// <param name="node">the type of edge.</param>
    /// <returns>True if the node exists, false otherwise.</returns>
    public bool HasNode(string node) {
        return this.nodes.Contains(node);
    }

    /// <summary>
    /// Check if an edge type exists.
    /// </summary>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>True if teh edge type exists, false otherwise.</returns>
    public bool HasEdgeType(string edge_type) {
        return this.edges.ContainsKey(edge_type);
    }

    /// <summary>
    /// Get the successors of a node.
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of strings, the node names that are downstream of the specified node.</returns>
    public List<string> Successors(string node, string edge_type) {
        return (List<string>) this.edges[edge_type][node];
    }

    /// <summary>
    /// Get the predeccessors of a node.
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>A list of strings, the node names that are upstream of the specified node.</returns>
    public List<string> Predecessors(string node, string edge_type) {
        if (!this.edges.ContainsKey(edge_type)) {
            throw new Exception("Edge type already exists.");
        }
        if (!this.nodes.Contains(node)) {
            throw new Exception("Node does not exist.");
        }
        return (List<string>) this.redges[edge_type][node];
    }

    /// <summary>
    /// Get the total number of edges connected to a node, both incoming and outgoing.
    /// </summary>
    /// <param name="node">the node name.</param>
    /// <param name="edge_type">the type of edge.</param>
    /// <returns>An integer, the number of edges.</returns>
    public int Degree(string node, string edge_type) {
        if (!this.edges.ContainsKey(edge_type)) {
            throw new Exception("Edge type already exists.");
        }
        if (!this.nodes.Contains(node)) {
            throw new Exception("Node does not exist.");
        }
        List<string> nodes0 = (List<string>) this.edges[edge_type][node];
        List<string> nodes1 = (List<string>) this.redges[edge_type][node];
        return nodes0.Count + nodes1.Count;
    }
}


