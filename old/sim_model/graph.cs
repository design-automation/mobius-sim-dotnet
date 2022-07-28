namespace Namespace {
    
    using @absolute_import = @@__future__.absolute_import;
    
    using @division = @@__future__.division;
    
    using @print_function = @@__future__.print_function;
    
    using @unicode_literals = @@__future__.unicode_literals;
    
    using OrderedDict = collections.OrderedDict;
    
    using System.Collections;
    
    using System.Collections.Generic;
    
    public static class Module {
        
        public class Graph
            : object {
            
            public Graph(object edge_types) {
                this.edge_types = edge_types;
                this.nodes = OrderedDict();
                this.edges = OrderedDict();
                this.edges_rev = OrderedDict();
                this.data = OrderedDict();
                foreach (var edge_type in edge_types) {
                    this.edges[edge_type] = OrderedDict();
                    this.edges_rev[edge_type] = OrderedDict();
                }
            }
            
            // ==============================================================================================
            // METHODS
            // ==============================================================================================
            public virtual object add_node(object n, Hashtable attribs) {
                this.nodes[n] = attribs;
                foreach (var edge_type in this.edges.keys()) {
                    this.edges[edge_type][n] = new List<object>();
                }
                foreach (var edge_type in this.edges_rev.keys()) {
                    this.edges_rev[edge_type][n] = new List<object>();
                }
            }
            
            public virtual object add_edge(object n0, object n1, object edge_type) {
                if (!this.nodes.Contains(n0) && this.nodes.Contains(n1)) {
                    throw Exception("Node does not exist.");
                }
                this.edges[edge_type][n0].append(n1);
                this.edges_rev[edge_type][n1].append(n0);
            }
            
            public virtual object successors(object n, object edge_type) {
                return this.edges[edge_type][n];
            }
            
            public virtual object predecessors(object n, object edge_type) {
                return this.edges_rev[edge_type][n];
            }
            
            public virtual object degree(object n, object edge_type) {
                return this.edges[edge_type][n].Count + this.edges_rev[edge_type][n].Count;
            }
        }
    }
}
