namespace Namespace {
    
    using @print_function = @@__future__.print_function;
    
    using @absolute_import = @@__future__.absolute_import;
    
    using @division = @@__future__.division;
    
    using @unicode_literals = @@__future__.unicode_literals;
    
    using OrderedDict = collections.OrderedDict;
    
    using json;
    
    using Entity = xml.dom.minidom.Entity;
    
    using graph = sim_model.graph;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class Module {
        
        // 
        //     A class that defines a set of constants for different entity types. 
        //     These types are used when adding an attrbute to the model.
        //     
        public class ENT_TYPE
            : object {
            
            public object POSIS = "posis";
            
            public object VERTS = "verts";
            
            public object EDGES = "edges";
            
            public object WIRES = "wires";
            
            public object POINTS = "points";
            
            public object PLINES = "plines";
            
            public object PGONS = "pgons";
            
            public object COLLS = "colls";
            
            public object MODEL = "model";
        }
        
        // 
        //     A class that defines a set of constants for possible data types for attributes. 
        //     These types are used when adding an attrbute to the model.
        //     
        public class DATA_TYPE
            : object {
            
            public object NUM = "number";
            
            public object STR = "string";
            
            public object BOOL = "boolean";
            
            public object LIST = "list";
            
            public object DICT = "dict";
        }
        
        public class _NODE_TYPE
            : object {
            
            public object ENT = "entity";
            
            public object ATTRIB = "attrib";
            
            public object ATTRIB_VAL = "attrib_val";
            
            public object META = "meta";
        }
        
        public class _EDGE_TYPE
            : object {
            
            public object ENT = "entity";
            
            public object ATTRIB = "attrib";
            
            public object META = "meta";
        }
        
        public static object _COLL_ENT_TYPES = new List<object> {
            ENT_TYPE.POINTS,
            ENT_TYPE.PLINES,
            ENT_TYPE.PGONS,
            ENT_TYPE.COLLS
        };
        
        public static object _ENT_PREFIX = new Dictionary<object, object> {
            {
                "posis",
                "ps"},
            {
                "verts",
                "_v"},
            {
                "edges",
                "_e"},
            {
                "wires",
                "_w"},
            {
                "points",
                "pt"},
            {
                "plines",
                "pl"},
            {
                "pgons",
                "pg"},
            {
                "colls",
                "co"}};
        
        public static object _ENT_SEQ = new Dictionary<object, object> {
            {
                ENT_TYPE.POSIS,
                0},
            {
                ENT_TYPE.VERTS,
                1},
            {
                ENT_TYPE.EDGES,
                2},
            {
                ENT_TYPE.WIRES,
                3},
            {
                ENT_TYPE.POINTS,
                4},
            {
                ENT_TYPE.PLINES,
                4},
            {
                ENT_TYPE.PGONS,
                4},
            {
                ENT_TYPE.COLLS,
                5}};
        
        public static object _ENT_SEQ_COLL_POINT_POSI = new Dictionary<object, object> {
            {
                ENT_TYPE.POSIS,
                0},
            {
                ENT_TYPE.VERTS,
                1},
            {
                ENT_TYPE.POINTS,
                2},
            {
                ENT_TYPE.COLLS,
                3}};
        
        public static object _ENT_SEQ_COLL_PLINE_POSI = new Dictionary<object, object> {
            {
                ENT_TYPE.POSIS,
                0},
            {
                ENT_TYPE.VERTS,
                1},
            {
                ENT_TYPE.EDGES,
                2},
            {
                ENT_TYPE.WIRES,
                3},
            {
                ENT_TYPE.PLINES,
                4},
            {
                ENT_TYPE.COLLS,
                5}};
        
        public static object _ENT_SEQ_COLL_PGON_POSI = new Dictionary<object, object> {
            {
                ENT_TYPE.POSIS,
                0},
            {
                ENT_TYPE.VERTS,
                1},
            {
                ENT_TYPE.EDGES,
                2},
            {
                ENT_TYPE.WIRES,
                3},
            {
                ENT_TYPE.PGONS,
                4},
            {
                ENT_TYPE.COLLS,
                5}};
        
        // 
        //     A class for creating Spatial Information Models (SIM models).
        // 
        //     The model can contain three types of objects: points, polyline, and polygons.
        // 
        //     Objects are creating by specifing positions, which have XYZ coordinates.
        // 
        //     Objects can be groouped into collections. Collections can contain heterogeneous sets of points,
        //     polylines, polygons and other collections.
        // 
        //     Objects have sub-entities that define their topology. The three types of sub-entities are
        //     vertices, edges, and wires. Attributes can also be attached to these sub-emtities. 
        // 
        //     - Point objects contain just one vertex.
        //     - Polyline objects contain vertices, edges, and one wire.
        //     - Polygon objects cotain vertices, edges and multiple wires. The first wire specifies the 
        //       polygon boundary. Subsequent wires spcify the polygon holes.
        //     
        public class SIM
            : object {
            
            public SIM() {
                // graph
                this.graph = graph.Graph(new List<object> {
                    _EDGE_TYPE.ENT,
                    _EDGE_TYPE.ATTRIB,
                    _EDGE_TYPE.META
                });
                // create meta nodes
                var meta = new List<object> {
                    ENT_TYPE.POSIS,
                    ENT_TYPE.VERTS,
                    ENT_TYPE.EDGES,
                    ENT_TYPE.WIRES,
                    ENT_TYPE.POINTS,
                    ENT_TYPE.PLINES,
                    ENT_TYPE.PGONS,
                    ENT_TYPE.COLLS
                };
                foreach (var ent_type in meta) {
                    this.graph.add_node(ent_type, node_type: _NODE_TYPE.META);
                    this.graph.add_node(ent_type + "_attribs", node_type: _NODE_TYPE.META);
                }
                // add xyz attrib
                this._graph_add_attrib(ENT_TYPE.POSIS, "xyz", DATA_TYPE.LIST);
            }
            
            // ==============================================================================================
            // UTILITY 
            // ==============================================================================================
            public virtual object _check_type(object value) {
                var val_type = type(value);
                if (val_type == @int || val_type == float) {
                    return DATA_TYPE.NUM;
                }
                if (val_type == str || val_type == unicode) {
                    return DATA_TYPE.STR;
                }
                if (val_type == @bool) {
                    return DATA_TYPE.BOOL;
                }
                if (val_type == list) {
                    return DATA_TYPE.LIST;
                }
                if (val_type == dict) {
                    return DATA_TYPE.DICT;
                }
                throw Exception("Data type is not recognised:", value.ToString(), type(value));
            }
            
            // ==============================================================================================
            // PRIVATE GRAPH METHODS
            // ==============================================================================================
            public virtual object _graph_add_ent(object enty_type) {
                var n = _ENT_PREFIX[enty_type] + this.graph.degree(enty_type, edge_type: _EDGE_TYPE.META).ToString();
                this.graph.add_node(n, node_type: _NODE_TYPE.ENT, ent_type: enty_type);
                this.graph.add_edge(enty_type, n, edge_type: _EDGE_TYPE.META);
                return n;
            }
            
            public virtual object _graph_attrib_node_name(object ent_type, object name) {
                return "att_" + ent_type + "_" + name;
            }
            
            public virtual object _graph_add_attrib(object ent_type, object name, object data_type) {
                var n = this._graph_attrib_node_name(ent_type, name);
                this.graph.add_node(n, node_type: _NODE_TYPE.ATTRIB, ent_type: ent_type, name: name, data_type: data_type);
                this.graph.add_edge(ent_type + "_attribs", n, edge_type: _EDGE_TYPE.META);
                return n;
            }
            
            public virtual object _graph_attrib_val_node_name(object value) {
                return "val_" + value.ToString();
            }
            
            public virtual object _graph_add_attrib_val(object att_n, object value) {
                var att_val_n = this._graph_attrib_val_node_name(value);
                if (!this.graph.nodes.Contains(att_val_n)) {
                    this.graph.add_node(att_val_n, node_type: _NODE_TYPE.ATTRIB_VAL, value: value);
                    this.graph.add_edge(att_val_n, att_n, edge_type: _EDGE_TYPE.ATTRIB);
                }
                return att_val_n;
            }
            
            // ==============================================================================================
            // ADD METHODS FOR ENTITIES
            // ==============================================================================================
            // Add a position to the model, specifying the XYZ coordinates.
            // 
            //         :param xyz: The XYZ coordinates, a list of three numbers.
            //         :return: The ID of the new position.
            //         
            public virtual object add_posi(object xyz) {
                var posi_n = this._graph_add_ent(ENT_TYPE.POSIS);
                this.set_attrib_val(posi_n, "xyz", xyz);
                return posi_n;
            }
            
            // Add a point object to the model, specifying a single position.
            // 
            //         :param posi: A position ID.
            //         :return: The ID of the new point.
            //         
            public virtual object add_point(object posi) {
                var vert_n = this._graph_add_ent(ENT_TYPE.VERTS);
                var point_n = this._graph_add_ent(ENT_TYPE.POINTS);
                this.graph.add_edge(vert_n, posi, edge_type: _EDGE_TYPE.ENT);
                this.graph.add_edge(point_n, vert_n, edge_type: _EDGE_TYPE.ENT);
                return point_n;
            }
            
            // Add a polyline object to the model, specifying a list of positions.
            // 
            //         :param posis: A list of position IDs.
            //         :param closed: A boolean indicating if the polyline is closed or open.
            //         :return: The ID of the new polyline.
            //         
            public virtual object add_pline(object posis, object closed) {
                object edge_n;
                // vertices
                var verts_n = new List<object>();
                foreach (var posi_n in posis) {
                    var vert_n = this._graph_add_ent(ENT_TYPE.VERTS);
                    this.graph.add_edge(vert_n, posi_n, edge_type: _EDGE_TYPE.ENT);
                    verts_n.append(vert_n);
                }
                // edges
                var edges_n = new List<object>();
                foreach (var i in Enumerable.Range(0, verts_n.Count - 1)) {
                    edge_n = this._graph_add_ent(ENT_TYPE.EDGES);
                    this.graph.add_edge(edge_n, verts_n[i], edge_type: _EDGE_TYPE.ENT);
                    this.graph.add_edge(edge_n, verts_n[i + 1], edge_type: _EDGE_TYPE.ENT);
                    edges_n.append(edge_n);
                }
                if (closed) {
                    edge_n = this._graph_add_ent(ENT_TYPE.EDGES);
                    this.graph.add_edge(edge_n, verts_n[-1], edge_type: _EDGE_TYPE.ENT);
                    this.graph.add_edge(edge_n, verts_n[0], edge_type: _EDGE_TYPE.ENT);
                    edges_n.append(edge_n);
                }
                // wire
                var wire_n = this._graph_add_ent(ENT_TYPE.WIRES);
                foreach (var i in Enumerable.Range(0, edges_n.Count)) {
                    this.graph.add_edge(wire_n, edges_n[i], edge_type: _EDGE_TYPE.ENT);
                }
                // pline
                var pline_n = this._graph_add_ent(ENT_TYPE.PLINES);
                this.graph.add_edge(pline_n, wire_n, edge_type: _EDGE_TYPE.ENT);
                //  return
                return pline_n;
            }
            
            // Add a polygon object to the model, specifying a list of positions.
            // 
            //         :param posis: A list of position IDs.
            //         :return: The ID of the new polygon.
            //         
            public virtual object add_pgon(object posis) {
                // vertices
                var verts_n = new List<object>();
                foreach (var posi_n in posis) {
                    var vert_n = this._graph_add_ent(ENT_TYPE.VERTS);
                    this.graph.add_edge(vert_n, posi_n, edge_type: _EDGE_TYPE.ENT);
                    verts_n.append(vert_n);
                }
                verts_n.append(verts_n[0]);
                // edges
                var edges_n = new List<object>();
                foreach (var i in Enumerable.Range(0, verts_n.Count - 1)) {
                    var v0 = verts_n[i];
                    var v1 = verts_n[i + 1];
                    var edge_n = this._graph_add_ent(ENT_TYPE.EDGES);
                    this.graph.add_edge(edge_n, v0, edge_type: _EDGE_TYPE.ENT);
                    this.graph.add_edge(edge_n, v1, edge_type: _EDGE_TYPE.ENT);
                    edges_n.append(edge_n);
                }
                // wire
                var wire_n = this._graph_add_ent(ENT_TYPE.WIRES);
                foreach (var i in Enumerable.Range(0, edges_n.Count)) {
                    this.graph.add_edge(wire_n, edges_n[i], edge_type: _EDGE_TYPE.ENT);
                }
                // pline
                var pgon_n = this._graph_add_ent(ENT_TYPE.PGONS);
                this.graph.add_edge(pgon_n, wire_n, edge_type: _EDGE_TYPE.ENT);
                //  return
                return pgon_n;
            }
            
            // Add a new empty collection to the model.
            // 
            //         :return: The ID of the collection.
            //         
            public virtual object add_coll() {
                return this._graph_add_ent(ENT_TYPE.COLLS);
            }
            
            // Add an entity to an existing collection in the model.
            //         Collections can contain points, polylines, polygons, and other collections.
            //         Collections cannot contain positions, vertices, edges or wires.
            // 
            //         :param coll: The ID of the collection to which the entity will be added.
            //         :param ent: The ID of the entity to be added to the collection.
            //         :return: No value.
            //         
            public virtual object add_coll_ent(object coll, object ent) {
                var ent_type = this.graph.nodes[ent].get("ent_type");
                if (!_COLL_ENT_TYPES.Contains(ent_type)) {
                    throw Exception("Invalid entitiy for collections.");
                }
                this.graph.add_edge(coll, ent, edge_type: _EDGE_TYPE.ENT);
            }
            
            // ==============================================================================================
            // ATTRIBUTE METHODS
            // ==============================================================================================
            // Create a new attribute in the model, specifying the entity type, the attribute name, and
            //         the data type. Note that for each entity type, the attribute name must be a unique name.
            // 
            //         :param ent_type: The entity type for the attribute. (See ENT_TYPE)
            //         :param att_name: The name of the attribute to create. 
            //         :param att_data_type: The data type for the attribute values. (See DATA_TYPE)
            //         :return: No value.
            //         
            public virtual object add_attrib(object ent_type, object att_name, object att_data_type) {
                var att_n = this._graph_attrib_node_name(ent_type, att_name);
                if (this.graph.nodes.get(att_n) == null) {
                    this._graph_add_attrib(ent_type, att_name, att_data_type);
                } else if (this.graph.nodes[att_n].get("data_type") != att_data_type) {
                    throw Exception("Attribute already exists with different data type");
                }
            }
            
            // Set the value of an attribute, specifying the entity in the model, the attribute name and
            //         the attribute value. 
            //         
            //         Note that an attribute with the specified name must already exist in 
            //         the model. If the attribute does not exist, an exception will be thrown. In addition, the 
            //         attribute value and the data type for the attribute must match.
            // 
            //         :param ent: The ID of the entity.
            //         :param att_name: The name of the attribute.
            //         :param att_value: The attribute value to set.
            //         :return: No value.
            //         
            public virtual object set_attrib_val(object ent, object att_name, object att_value) {
                var ent_type = this.graph.nodes[ent].get("ent_type");
                var att_n = this._graph_attrib_node_name(ent_type, att_name);
                if (ent_type != this.graph.nodes[att_n].get("ent_type")) {
                    throw Exception("Entity and attribute have different types.");
                }
                var data_type = this._check_type(att_value);
                if (this.graph.nodes[att_n].get("data_type") != data_type) {
                    throw Exception("Attribute value has the wrong data type: " + att_value.ToString());
                }
                var att_val_n = this._graph_add_attrib_val(att_n, att_value);
                this.graph.add_edge(ent, att_val_n, edge_type: _EDGE_TYPE.ATTRIB);
            }
            
            // Get an attribute value from an entity in the model, specifying the attribute name.
            // 
            //         :param ent: The ID of the entity for which to get the attribute value.
            //         :param name: The name of the attribute.
            //         :return: The attribute value or None if no value.
            //         
            public virtual object get_attrib_val(object ent, object name) {
                // TODO this look slow!
                var ent_type = this.graph.nodes[ent].get("ent_type");
                var att_n = this._graph_attrib_node_name(ent_type, name);
                var att_vals_n = this.graph.successors(ent, _EDGE_TYPE.ATTRIB);
                foreach (var att_val_n in att_vals_n) {
                    var atts_n = this.graph.successors(att_val_n, _EDGE_TYPE.ATTRIB);
                    if (atts_n && atts_n[0] == att_n) {
                        return this.graph.nodes[att_val_n].get("value");
                    }
                }
                return null;
            }
            
            // Set an attribute value from the model, specifying a name and value. Model attributes are
            //         top level attributes that apply to the whole model. As such, they are not attached to any
            //         specific entities.
            // 
            //         :param att_name: The name of the attribute.
            //         :param att_value: The attribute value to set.
            //         :return: No value.
            //         
            public virtual object set_model_attrib_val(object att_name, object att_value) {
                this.graph.data[att_name] = att_value;
            }
            
            // Get an attribute value from the model, specifying a name. Model attributes are
            //         top level attributes that apply to the whole model. As such, they are not attached to any
            //         specific entities.
            // 
            //         :param att_name: The name of the attribute.
            //         :return: The attribute value or None if no value.
            //         
            public virtual object get_model_attrib_val(object att_name) {
                return this.graph.data[att_name];
            }
            
            // ==============================================================================================
            // GET METHODS FOR ENTITIES
            // ==============================================================================================
            // Get the number of entities in the model of a specific type.  
            // 
            //         :param ent_type: The type of entity to search for in the model.
            //         :return: A number of entities of the specified type in the model.
            //         
            public virtual object num_ents(object ent_type) {
                return this.graph.degree(ent_type, _EDGE_TYPE.META);
            }
            
            public virtual object _get_ent_seq(object target_ent_type, object source_ent_type) {
                if (target_ent_type == ENT_TYPE.POINTS || source_ent_type == ENT_TYPE.POINTS) {
                    return _ENT_SEQ_COLL_POINT_POSI;
                } else if (target_ent_type == ENT_TYPE.PLINES || source_ent_type == ENT_TYPE.PLINES) {
                    return _ENT_SEQ_COLL_PLINE_POSI;
                } else if (target_ent_type == ENT_TYPE.PGONS || source_ent_type == ENT_TYPE.PGONS) {
                    return _ENT_SEQ_COLL_PGON_POSI;
                }
                return _ENT_SEQ;
            }
            
            // Get entities of a specific type. A list of entity IDs is returned.
            // 
            //         If source_ents is None, then all entities of the specified type in the model are returned.
            //         If there are no entities of that type in the model, then an empty list is returned. 
            // 
            //         If source_ents contains a list of entities, then entities will be extracted from the source
            //         ents. For example, if ent_type is 'posis' and 'source_ents' is a polyline and a polygon,
            //         then a list containing the positions used in the polyline and polygon are returned. 
            //         Similarly, if ent_type is 'pgons' and 'source_ents' is a list of positions,
            //         then a list of polygons is returned, of polygons that make use of the specified positions.
            // 
            //         :param target_ent_type: The type of entity to get from the model.
            //         :param source_ents: None, or a single entity ID or a list of entity IDs from which to get the target entities.
            //         :return: A list of unique entity IDs.
            //         
            public virtual object get_ents(object target_ent_type, object source_ents = null) {
                if (source_ents == null) {
                    return this.graph.successors(target_ent_type, _EDGE_TYPE.META);
                }
                // not a list
                if (!object.ReferenceEquals(type(source_ents), list)) {
                    return this._nav(target_ent_type, source_ents);
                }
                // a list with one item
                if (source_ents.Count == 1) {
                    return this._nav(target_ent_type, source_ents[0]);
                }
                // a list with multiple items
                var ents_set = OrderedDict();
                foreach (var source_ent in source_ents) {
                    foreach (var target_ent in this._nav(target_ent_type, source_ent)) {
                        ents_set[target_ent] = null;
                    }
                }
                return ents_set.keys().ToList();
            }
            
            // TODO more tests needed
            public virtual object _nav(object target_ent_type, object source_ent) {
                var source_ent_type = this.graph.nodes[source_ent].get("ent_type");
                var ent_seq = this._get_ent_seq(target_ent_type, source_ent_type);
                if (source_ent_type == target_ent_type) {
                    // TODO nav colls of colls
                    return new List<object> {
                        source_ent
                    };
                }
                var dist = ent_seq[source_ent_type] - ent_seq[target_ent_type];
                if (dist == 1) {
                    return this.graph.successors(source_ent, _EDGE_TYPE.ENT);
                }
                if (dist == -1) {
                    return this.graph.predecessors(source_ent, _EDGE_TYPE.ENT);
                }
                var navigate = dist > 0 ? this.graph.successors : this.graph.predecessors;
                var ents = new List<object> {
                    source_ent
                };
                var target_ents_set = OrderedDict();
                while (ents) {
                    var ent_set = OrderedDict();
                    foreach (var ent in ents) {
                        foreach (var target_ent in navigate(ent, _EDGE_TYPE.ENT)) {
                            var this_ent_type = this.graph.nodes[target_ent].get("ent_type");
                            if (ent_seq.Contains(this_ent_type)) {
                                if (this_ent_type == target_ent_type) {
                                    target_ents_set[target_ent] = null;
                                } else if (dist > 0 && ent_seq[this_ent_type] > ent_seq[target_ent_type]) {
                                    ent_set[target_ent] = null;
                                } else if (dist < 0 && ent_seq[this_ent_type] < ent_seq[target_ent_type]) {
                                    ent_set[target_ent] = null;
                                }
                            }
                        }
                    }
                    ents = ent_set.keys();
                }
                return target_ents_set.keys();
            }
            
            // def get_ents2(self, target_ent_type, source_ents = None):
            //     """Get entities of a specific type. A list of entity IDs is returned.
            //     If source_ents is None, then all entities of the specified type in the model are returned.
            //     If there are no entities of that type in the model, then an empty list is returned. 
            //     If source_ents contains a list of entities, then entities will be extracted from the source
            //     ents. For example, if ent_type is 'posis' and 'source_ents' is a polyline and a polygon,
            //     then a list containing the positions used in the polyline and polygon are returned. 
            //     Similarly, if ent_type is 'pgons' and 'source_ents' is a list of positions,
            //     then a list of polygons is returned, of polygons that make use of the specified positions.
            //     :param target_ent_type: The type of entity to get from the model.
            //     :param source_ents: None, or a single entity ID or a list of entity IDs from which to get the target entities.
            //     :return: A list of unique entity IDs.
            //     """
            //     if source_ents == None:
            //         return self.graph.successors(target_ent_type, _EDGE_TYPE.META)
            //     source_ents = source_ents if type(source_ents) is list else [source_ents]
            //     ents_set = OrderedDict() # ordered set
            //     for source_ent in source_ents:
            //         for target_ent in self._nav2(target_ent_type, source_ent):
            //             ents_set[target_ent] = None # ordered set
            //     return list(ents_set.keys())
            // def _nav2(self, target_ent_type, source_ent):
            //     source_ent_type = self.graph.nodes[source_ent].get('ent_type')
            //     if source_ent_type == target_ent_type and source_ent_type != ENT_TYPE.COLLS:
            //         return source_ent
            //     ent_seq = self._get_ent_seq(target_ent_type, source_ent_type)
            //     dir_down = ent_seq[source_ent_type] > ent_seq[target_ent_type]
            //     navigate = self.graph.successors if dir_down else self.graph.predecessors
            //     ents = [source_ent]
            //     target_ents_set = OrderedDict()
            //     while ents:
            //         ent_set = OrderedDict()
            //         for ent in ents:
            //             for target_ent in navigate(ent, _EDGE_TYPE.ENT):
            //                 this_ent_type = self.graph.nodes[target_ent].get('ent_type')
            //                 if this_ent_type in ent_seq:
            //                     if this_ent_type == target_ent_type:
            //                         target_ents_set[target_ent] = None # orderd set
            //                     elif ent_seq[this_ent_type] > ent_seq[target_ent_type] if dir_down else ent_seq[this_ent_type] < ent_seq[target_ent_type]:
            //                         ent_set[target_ent] = None # orderd set
            //         ents = ent_set.keys()
            //     return target_ents_set.keys()
            // def get_ents3(self, target_ent_type, source_ents = None):
            //     print ("get ents")
            //     if source_ents == None:
            //         return self.graph.successors(target_ent_type, _EDGE_TYPE.META)
            //     if not type(source_ents) is list:
            //         source_ent_type = self.graph.nodes[source_ents].get('ent_type')
            //         ent_seq = self._get_ent_seq(target_ent_type, source_ent_type)
            //         return self._nav_recursive_one(target_ent_type, source_ents, ent_seq)
            //     ents_set = OrderedDict()
            //     for source_ent in source_ents:
            //         source_ent_type = self.graph.nodes[source_ent].get('ent_type')
            //         ent_seq = self._get_ent_seq(target_ent_type, source_ent_type)
            //         for ent  in self._nav_recursive(target_ent_type, source_ent, ent_seq):
            //             ents_set[ent] = None # ordered set
            //     return ents_set.keys()
            // def _nav_recursive_one(self, target_ent_type, source_ent, ent_seq):
            //     source_ent_type = self.graph.nodes[source_ent].get('ent_type')
            //     # if not source_ent_type in ent_seq:
            //     #     print (">>", source_ent, ent_seq)
            //     if source_ent_type == target_ent_type and source_ent_type != ENT_TYPE.COLLS:
            //         return source_ent
            //     dist = ent_seq[source_ent_type] - ent_seq[target_ent_type]
            //     if dist == 1:
            //         return self.graph.successors(source_ent, _EDGE_TYPE.ENT)
            //     if dist == -1:
            //         return self.graph.predecessors(source_ent, _EDGE_TYPE.ENT)
            //     if dist > 1:
            //         return self._nav_recursive(target_ent_type, self.graph.successors(source_ent, _EDGE_TYPE.ENT), ent_seq)
            //     if dist < -1:
            //         return self._nav_recursive(target_ent_type, self.graph.predecessors(source_ent, _EDGE_TYPE.ENT), ent_seq)
            // def _nav_recursive(self, target_ent_type, source_ents, ent_seq):
            //     if not type(source_ents) is list:
            //         return self._nav_recursive_one(target_ent_type, source_ents, ent_seq)
            //     if len(source_ents) == 1:
            //         return self._nav_recursive_one(target_ent_type, source_ents[0], ent_seq)
            //     ents_set = OrderedDict()
            //     for a_source_ent in source_ents:
            //         for ent in self._nav_recursive(target_ent_type, a_source_ent, ent_seq):
            //             ents_set[ent] = None # ordered set
            //     return ents_set.keys()
            // Get the position ID for a point.
            // 
            //         :param point: A point ID from which to get the position.
            //         :return: A position ID. 
            //         
            public virtual object get_point_posi(object point) {
                var vert = this.graph.successors(point, _EDGE_TYPE.ENT)[0];
                return this.graph.successors(vert, _EDGE_TYPE.ENT)[0];
            }
            
            // Get a list of position IDs for a polyline. If the polyline is closed, the first and last
            //         positions will be the same.
            // 
            //         :param pline: A polyline ID from which to get the positions.
            //         :return: A list of position IDs. The list may contain duplicates.
            //         
            public virtual object get_pline_posis(object pline) {
                var wire = this.graph.successors(pline, _EDGE_TYPE.ENT)[0];
                var edges = this.graph.successors(wire, _EDGE_TYPE.ENT);
                var verts = (from edge in edges
                    select this.graph.successors(edge, _EDGE_TYPE.ENT)[0]).ToList();
                var posis = (from vert in verts
                    select this.graph.successors(vert, _EDGE_TYPE.ENT)[0]).ToList();
                var last_vert = this.graph.successors(edges[-1], _EDGE_TYPE.ENT)[1];
                var last_posi = this.graph.successors(last_vert, _EDGE_TYPE.ENT)[0];
                posis.append(last_posi);
                return posis;
            }
            
            // Get a list of lists of position IDs for an polygon. Each list represents one of the
            //         polygon wires. All wires are assumed to be closed. (The last position is not duplicated.)
            // 
            //         :param pgon: A polygon ID from which to get the positions.
            //         :return: A list of lists of position IDs. The lists may contain duplicates.
            //         
            public virtual object get_pgon_posis(object pgon) {
                var posis = new List<object>();
                foreach (var wire in this.graph.successors(pgon, _EDGE_TYPE.ENT)) {
                    var edges = this.graph.successors(wire, _EDGE_TYPE.ENT);
                    var verts = (from edge in edges
                        select this.graph.successors(edge, _EDGE_TYPE.ENT)[0]).ToList();
                    var wire_posis = (from vert in verts
                        select this.graph.successors(vert, _EDGE_TYPE.ENT)[0]).ToList();
                    posis.append(wire_posis);
                }
                return posis;
            }
            
            // ==============================================================================================
            // QUERY
            // ==============================================================================================
            // Check if a polyline is open or closed.
            // 
            //         :param pline: A polyline ID.
            //         :return: True if closed, False if open.
            //         
            public virtual object pline_is_closed(object pline) {
                var edges = this.graph.successors(this.graph.successors(pline, _EDGE_TYPE.ENT)[0], _EDGE_TYPE.ENT);
                var start = this.graph.successors(this.graph.successors(edges[0], _EDGE_TYPE.ENT)[0], _EDGE_TYPE.ENT);
                var end = this.graph.successors(this.graph.successors(edges[-1], _EDGE_TYPE.ENT)[1], _EDGE_TYPE.ENT);
                return start == end;
            }
            
            // ==============================================================================================
            // EXPORT
            // ==============================================================================================
            // Print information about the model. This is mainly used for debugging.
            //         
            //         :return: A string describing the data in the model.
            //         
            public virtual object info() {
                var nodes = map(n => "- " + n + ": " + this.graph.nodes[n].ToString(), this.graph.nodes);
                nodes = "\n".join(nodes);
                var all_edges = "";
                foreach (var edge_type in this.graph.edge_types) {
                    var edges = map(e => "- " + e + ": " + this.graph.edges[edge_type][e].ToString(), this.graph.edges[edge_type]);
                    edges = "\n".join(edges);
                    all_edges = all_edges + "\n EDGES: " + edge_type + "\n" + edges + "\n";
                }
                return "NODES: \n" + nodes + "\n" + all_edges + "\n\n\n";
            }
            
            // Return JSON representing that data in the model.
            //         
            //         :return: JSON data.
            //         
            public virtual object to_json() {
                // get entities from graph
                var posi_ents = this.graph.successors(ENT_TYPE.POSIS, _EDGE_TYPE.META);
                var vert_ents = this.graph.successors(ENT_TYPE.VERTS, _EDGE_TYPE.META);
                var edge_ents = this.graph.successors(ENT_TYPE.EDGES, _EDGE_TYPE.META);
                var wire_ents = this.graph.successors(ENT_TYPE.WIRES, _EDGE_TYPE.META);
                var point_ents = this.graph.successors(ENT_TYPE.POINTS, _EDGE_TYPE.META);
                var pline_ents = this.graph.successors(ENT_TYPE.PLINES, _EDGE_TYPE.META);
                var pgon_ents = this.graph.successors(ENT_TYPE.PGONS, _EDGE_TYPE.META);
                var coll_ents = this.graph.successors(ENT_TYPE.COLLS, _EDGE_TYPE.META);
                // create maps for entity name -> entity index
                var posis_dict = zip(posi_ents, Enumerable.Range(0, posi_ents.Count)).ToDictionary();
                var verts_dict = zip(vert_ents, Enumerable.Range(0, vert_ents.Count)).ToDictionary();
                var edges_dict = zip(edge_ents, Enumerable.Range(0, edge_ents.Count)).ToDictionary();
                var wires_dict = zip(wire_ents, Enumerable.Range(0, wire_ents.Count)).ToDictionary();
                var points_dict = zip(point_ents, Enumerable.Range(0, point_ents.Count)).ToDictionary();
                var plines_dict = zip(pline_ents, Enumerable.Range(0, pline_ents.Count)).ToDictionary();
                var pgons_dict = zip(pgon_ents, Enumerable.Range(0, pgon_ents.Count)).ToDictionary();
                var colls_dict = zip(coll_ents, Enumerable.Range(0, coll_ents.Count)).ToDictionary();
                // create the geometry data
                var geometry = new Dictionary<object, object> {
                    {
                        "num_posis",
                        this.graph.degree(ENT_TYPE.POSIS, _EDGE_TYPE.META)},
                    {
                        "points",
                        new List<object>()},
                    {
                        "plines",
                        new List<object>()},
                    {
                        "pgons",
                        new List<object>()},
                    {
                        "coll_points",
                        new List<object>()},
                    {
                        "coll_plines",
                        new List<object>()},
                    {
                        "coll_pgons",
                        new List<object>()},
                    {
                        "coll_colls",
                        new List<object>()}};
                foreach (var point_ent in point_ents) {
                    var posi_i = this.get_point_posi(point_ent);
                    geometry["points"].append(posis_dict[posi_i]);
                }
                foreach (var pline_ent in pline_ents) {
                    var posis_i = this.get_pline_posis(pline_ent);
                    geometry["plines"].append((from posi_i in posis_i
                        select posis_dict[posi_i]).ToList());
                }
                foreach (var pgon_ent in pgon_ents) {
                    var wires_posis_i = this.get_pgon_posis(pgon_ent);
                    geometry["pgons"].append((from posis_i in wires_posis_i
                        select (from posi_i in posis_i
                            select posis_dict[posi_i]).ToList()).ToList());
                }
                foreach (var coll_ent in coll_ents) {
                    geometry["coll_points"].append(new List<object>());
                    geometry["coll_plines"].append(new List<object>());
                    geometry["coll_pgons"].append(new List<object>());
                    geometry["coll_colls"].append(new List<object>());
                    foreach (var ent in this.graph.successors(coll_ent, _EDGE_TYPE.ENT)) {
                        var ent_type = this.graph.nodes[ent].get("ent_type");
                        if (ent_type == ENT_TYPE.POINTS) {
                            geometry["coll_points"][-1].append(points_dict[ent]);
                        } else if (ent_type == ENT_TYPE.PLINES) {
                            geometry["coll_plines"][-1].append(plines_dict[ent]);
                        } else if (ent_type == ENT_TYPE.PGONS) {
                            geometry["coll_pgons"][-1].append(pgons_dict[ent]);
                        } else if (ent_type == ENT_TYPE.COLLS) {
                            geometry["coll_colls"][-1].append(colls_dict[ent]);
                        }
                    }
                }
                // get attribs from graph
                var posi_attribs = this.graph.successors(ENT_TYPE.POSIS + "_attribs", _EDGE_TYPE.META);
                var vert_attribs = this.graph.successors(ENT_TYPE.VERTS + "_attribs", _EDGE_TYPE.META);
                var edge_attribs = this.graph.successors(ENT_TYPE.EDGES + "_attribs", _EDGE_TYPE.META);
                var wire_attribs = this.graph.successors(ENT_TYPE.WIRES + "_attribs", _EDGE_TYPE.META);
                var point_attribs = this.graph.successors(ENT_TYPE.POINTS + "_attribs", _EDGE_TYPE.META);
                var pline_attribs = this.graph.successors(ENT_TYPE.PLINES + "_attribs", _EDGE_TYPE.META);
                var pgon_attribs = this.graph.successors(ENT_TYPE.PGONS + "_attribs", _EDGE_TYPE.META);
                var coll_attribs = this.graph.successors(ENT_TYPE.COLLS + "_attribs", _EDGE_TYPE.META);
                // create the attribute data
                Func<object, object, object> _attribData = (attribs,ent_dict) => {
                    var attribs_data = new List<object>();
                    foreach (var att_n in attribs) {
                        var data = OrderedDict();
                        var att_vals_n = this.graph.predecessors(att_n, _EDGE_TYPE.ATTRIB);
                        data["name"] = this.graph.nodes[att_n].get("name");
                        data["data_type"] = this.graph.nodes[att_n].get("data_type");
                        data["values"] = new List<object>();
                        data["entities"] = new List<object>();
                        foreach (var att_val_n in att_vals_n) {
                            data["values"].append(this.graph.nodes[att_val_n].get("value"));
                            var idxs = (from ent in this.graph.predecessors(att_val_n, _EDGE_TYPE.ATTRIB)
                                select ent_dict[ent]).ToList();
                            data["entities"].append(idxs);
                        }
                        attribs_data.append(data);
                    }
                    return attribs_data;
                };
                var attributes = new Dictionary<object, object> {
                    {
                        "posis",
                        _attribData(posi_attribs, posis_dict)},
                    {
                        "verts",
                        _attribData(vert_attribs, verts_dict)},
                    {
                        "edges",
                        _attribData(edge_attribs, edges_dict)},
                    {
                        "wires",
                        _attribData(wire_attribs, wires_dict)},
                    {
                        "points",
                        _attribData(point_attribs, points_dict)},
                    {
                        "plines",
                        _attribData(pline_attribs, plines_dict)},
                    {
                        "pgons",
                        _attribData(pgon_attribs, pgons_dict)},
                    {
                        "colls",
                        _attribData(coll_attribs, colls_dict)},
                    {
                        "model",
                        this.graph.data.items().ToList()}};
                // create the json
                var data = new Dictionary<object, object> {
                    {
                        "type",
                        "SIM"},
                    {
                        "version",
                        "0.1"},
                    {
                        "geometry",
                        geometry},
                    {
                        "attributes",
                        attributes}};
                return data;
            }
            
            // Return a JSON formatted string representing that data in the model.
            //         
            //         :return: A JSON string in the SIM format.
            //         
            public virtual object to_json_str() {
                return json.dumps(this.to_json());
            }
        }
    }
}
