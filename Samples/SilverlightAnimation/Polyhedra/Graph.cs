// Implementation of a "graph" describing which nodes
// (representing faces in a 3D shape) are connected to which.
// Copyright Declan Brennan, 2007 (http://declan.brennan.name)
// Please don't use without permission

using System.Collections.Generic;
using System;
using Microsoft.DirectX;

/// <summary>
/// The graph object representing a 3D Shape
/// In theory if we assume all the sides are the same length, it is possible to
/// calculate the geometry of the final 3D shape knowing just which faces are connected to
/// which.
/// In practice it is easy to work out the dihedral angles where three faces meet at a corner
/// or where all the faces meeting at a corner are of the same type. However calculating the
/// dihedral angles for the general case is a lot more difficult and even be an open problem
/// for irregular polyhedrons.
/// So for now we'll cheat by also keeping caching the dihedral angles we expect in the
/// final polyhedron.
/// </summary>
public class Graph
{
   /// <summary>
   /// The collection of nodes in the graph indexed by node id. 
   /// </summary>
   private Dictionary<string, GraphNode> _nodes;
   /// <summary>
   /// The cache of dihedral angles expected been faces in the final 3D solid
   /// </summary>
   private DihedralCache _dihedrals;
   /// <summary>
   /// Locate the node with the specified node id or create it if it doesn't already exist 
   /// </summary>
   /// <param name="id"></param>
   /// <returns></returns>
   public GraphNode LookupNode(string id)
   {
      GraphNode rc = null;
      _nodes.TryGetValue(id, out rc);
      if (rc == null)
      {
         rc = new GraphNode(this, id);
         _nodes[id] = rc;
      }
      return rc;
   }

   /// <summary>
   /// Return the cache of diheral angles expected between the faces of the final 3D solid
   /// </summary>
   public DihedralCache DihedralCache
   {
      get
      {
         return _dihedrals;
      }
   }

   /// <summary>
   /// Construct a graph with the specified name
   /// This name is used to locate resources corresponding to the graph connectivity
   /// and the dihedral angles.
   /// </summary>
   /// <param name="name"></param>
   public Graph(string name)
   {
      System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
      string fullName = "Polyhedra.Shapes." + name;
      using (var s1 = asm.GetManifestResourceStream(fullName + ".shp"))
      using (var s2 = asm.GetManifestResourceStream(fullName + ".dihedrals"))
         init(s1, s2);
   }

   /// <summary>
   /// Initialize the set of nodes and the dihedral cache from the two streams passed in
   /// </summary>
   /// <param name="shapeStream"></param>
   /// <param name="dihedralStream"></param>
   private void init(System.IO.Stream shapeStream, System.IO.Stream dihedralStream)
   {
      using (System.IO.TextReader reader = new System.IO.StreamReader(shapeStream))
         readConnectivity(reader);
      if (dihedralStream != null)
         _dihedrals = new DihedralCache(dihedralStream);
   }

   /// <summary>
   /// Read in the set of nodes and their connectivity
   /// </summary>
   /// <param name="reader"></param>
   private void readConnectivity(System.IO.TextReader reader)
   {
      _nodes = new Dictionary<string, GraphNode>();
      while (reader.Peek() >= 0)
         processLine(reader.ReadLine());
   }

   /// <summary>
   /// Process a line corresponding to the connectivity for a single node
   /// </summary>
   /// <param name="line"></param>
   private void processLine(string line)
   {
      if (line.Trim().Length == 0)
         return;
      int p = line.IndexOf(":");
      GraphNode n = this.LookupNode(line.Substring(0, p));
      foreach (string ctId in line.Substring(p + 1).Split(new char[] { ',' }))
         n.ConnectedTo.Add(this.LookupNode(ctId));
   }

   /// <summary>
   /// Returns the number of Nodes in the Graph
   /// </summary>
   public int NumNodes
   {
      get
      {
         return _nodes.Count;
      }
   }

   /// <summary>
   /// Returns the collection of nodes in the graph
   /// </summary>
   public ICollection<GraphNode> GraphNodes
   {
      get
      {
         return _nodes.Values;
      }
   }
}

/// <summary>
/// An individual node in the graph corresponding to a face in the final solid
/// </summary>
public class GraphNode
{
   /// <summary>
   /// The unique id for this node 
   /// </summary>
   private string _id;
   /// <summary>
   /// The nodes that this node is connected to
   /// </summary>
   private List<GraphNode> _connectedTo = new List<GraphNode>();
   /// <summary>
   /// The graph that contains this node
   /// </summary>
   private Graph _graph;

   /// <summary>
   /// Construct a node with the specified id for the specified graph
   /// </summary>
   /// <param name="graph"></param>
   /// <param name="id"></param>
   public GraphNode(Graph graph, string id)
   {
      _id = id;
      _graph = graph;
   }

   /// <summary>
   /// The graph that contains this node
   /// </summary>
   public Graph Graph
   {
      get
      {
         return _graph;
      }
   }

   /// <summary>
   /// The nodes that this node is connected to
   /// </summary>
   public List<GraphNode> ConnectedTo
   {
      get
      {
         return _connectedTo;
      }
   }

   /// <summary>
   /// The number of connections that this node has
   /// </summary>
   public int NumConnections
   {
      get
      {
         return _connectedTo.Count;
      }
   }

   /// <summary>
   /// The unique id for this node in the graph
   /// </summary>
   public string Id
   {
      get
      {
         return _id;
      }
   }

}

/// <summary>
/// The cache of dihedral angles containing the angles between each pair of 
/// connected faces that should exist in the final solid
/// </summary>
public class DihedralCache
{
   /// <summary>
   /// The dihedral angles indexed by the pair of ids for the nodes to which they apply
   /// </summary>
   public Dictionary<string, float> _dihedrals;

   /// <summary>
   /// Construct the cache from the supplied open stream
   /// </summary>
   /// <param name="stream"></param>
   public DihedralCache(System.IO.Stream stream)
   {
      if (stream != null)
         using (System.IO.TextReader reader = new System.IO.StreamReader(stream))
            readAngles(reader);
   }

   /// <summary>
   /// Read in all the angles into the cache from the supplied open reader
   /// </summary>
   /// <param name="reader"></param>
   private void readAngles(System.IO.TextReader reader)
   {
      _dihedrals = new Dictionary<string, float>();
      while (reader.Peek() >= 0)
         processLine(reader.ReadLine());
   }

   /// <summary>
   /// Process a line corresponding to the angle expected between one pair of connected nodes
   /// </summary>
   /// <param name="line"></param>
   private void processLine(string line)
   {
      int p = line.IndexOf(':');
      float angle = float.Parse(line.Substring(p + 1));
      _dihedrals.Add(line.Substring(0, p), angle);
   }

   /// <summary>
   /// Return the angle expected in the final solid between the specified pair of nodes
   /// </summary>
   /// <param name="id1"></param>
   /// <param name="id2"></param>
   /// <returns></returns>
   public float GetDihedralAngle(string id1, string id2)
   {
      string key = id2 + "," + id1;
      return _dihedrals[key];
   }
}
