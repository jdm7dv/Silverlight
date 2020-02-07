// Implementation of a 2D "net" of FlatFaces that can folded to generate a 3D Polyhedron
// Copyright Declan Brennan, 2007 (http://declan.brennan.name)
// Please don't use without permission

using System.Collections.Generic;
using Microsoft.DirectX;
using System;
using System.Windows;

/// <summary>
/// NetBuilder constructs a flat two dimensional net consisting of the set of FlatFaces
/// that can then be folded to form the final 3D solid
/// </summary>
public static class NetBuilder
{
   /// <summary>
   /// The length of each side of a FlatFace in the net
   /// </summary>
   static float _sideLen = 1.0f;

   /// <summary>
   /// Build a Net from the supplied Graph
   /// </summary>
   /// <param name="graph"></param>
   /// <returns></returns>
   public static Net Build(Graph graph)
   {
      GraphNode firstNode = graph.LookupNode("1");

      List<GraphNode> toProcess = new List<GraphNode>();
      Net rc = null;

      Vector2[] points = Geometry.GetPolyVectors(new Vector2(_sideLen, 0), new Vector2(0, 0), firstNode.NumConnections);
      FlatFace rootFace = new FlatFace(firstNode, null, points);
      if (rc == null)
         rc = new Net(rootFace);
      rc.AddFace(rootFace);

      rc.GraphNodeToFace[firstNode] = rootFace;
      toProcess.Add(firstNode);

      while (toProcess.Count > 0)
      {
         List<GraphNode> nextToProcess = new List<GraphNode>();

         foreach (GraphNode node in toProcess)
         {
            FlatFace face = rc.GraphNodeToFace[node];

            int s0 = 0;
            if (face.Parent != null)
            {
               foreach (GraphNode childNode in node.ConnectedTo)
               {
                  FlatFace child = null;
                  rc.GraphNodeToFace.TryGetValue(childNode, out child);
                  if (childNode != null)
                     if (childNode == face.Parent.GraphNode)
                        break;
                  s0++;
               }
            }

            for (int s = 0; s < node.NumConnections; s++)
            {
               int f = (s0 + s) % node.NumConnections;
               GraphNode childNode = node.ConnectedTo[f];
               if (!rc.GraphNodeToFace.ContainsKey(childNode))
               {
                  points = Geometry.GetPolyVectors(face.Points[(s + 1) % face.NumSides], face.Points[s], childNode.NumConnections);
                  FlatFace childFace = new FlatFace(childNode, face, points);
                  rc.AddFace(childFace);
                  rc.GraphNodeToFace[childNode] = childFace;
                  face.ConnectedTo[s] = childFace;
                  nextToProcess.Add(childNode);
               }
            }
         }
         toProcess = nextToProcess;
      }
      return rc;
   }
}

/// <summary>
/// A complete "net" of FlatFaces that can be folded to form a polyhedron
/// </summary>
public class Net
{
   /// <summary>
   /// The net is actually a "tree" of FlatFaces. This is the root.
   /// </summary>
   FlatFace _root;
   /// <summary>
   /// The collection of FlatFaces (in the net) indexed by GraphNode (in the graph)
   /// </summary>
   Dictionary<GraphNode, FlatFace> _graphNodeToFace = new Dictionary<GraphNode, FlatFace>();
   /// <summary>
   /// A simple list of all the FlatFaces in this Net
   /// </summary>
   List<FlatFace> _faces = new List<FlatFace>();

   /// <summary>
   /// The collection of FlatFaces (in the net) indexed by GraphNode (in the graph)
   /// </summary>
   public Dictionary<GraphNode, FlatFace> GraphNodeToFace
   {
      get
      {
         return _graphNodeToFace;
      }
   }

   /// <summary>
   /// The FlatFace that is the root of the tree that forms the Net
   /// </summary>
   public FlatFace Root
   {
      get
      {
         return _root;
      }
   }

   /// <summary>
   /// Construct a Net
   /// </summary>
   /// <param name="root">The FlatFace that is the root of the tee that forms the Net</param>
   public Net(FlatFace root)
   {
      _root = root;
   }

   /// <summary>
   /// Add a FlatFace to this Net
   /// </summary>
   /// <param name="face"></param>
   public void AddFace(FlatFace face)
   {
      _faces.Add(face);
   }

   /// <summary>
   /// Provide an enumerator for the set of FlatFaces in this Net
   /// </summary>
   public IEnumerable<FlatFace> Faces
   {
      get
      {
         return _faces;
      }
   }

   /// <summary>
   /// Return the number of FlatFaces in this Net
   /// (which should end up being the same as the number of GraphNodes in the Graph
   /// that the Net is based on
   /// </summary>
   public int NumFaces
   {
      get
      {
         return _faces.Count;
      }
   }
}

/// <summary>
/// A single FlatFace in the Net that can be folded to form the 3D solid
/// </summary>
public class FlatFace
{
   /// <summary>
   /// The set of points corresponding to the corners of this FlatFace
   /// </summary>
   Vector2[] _points;

   /// <summary>
   /// The set of FlatFaces to which this FlatFace is connected forming part of a tree structure
   /// </summary>
   FlatFace[] _connectedTo;
   /// <summary>
   /// The parent of this FlatFace in the tree or null if this FlatFace is the root
   /// </summary>
   FlatFace _parent;
   /// <summary>
   /// The GraphNode in the Graph that was the prototype for this FlatFace
   /// </summary>
   GraphNode _graphNode;

   /// <summary>
   /// The GraphNode in the Graph that was the prototype for this FlatFace
   /// </summary>
   public GraphNode GraphNode
   {
      get
      {
         return _graphNode;
      }

   }

   /// <summary>
   /// The parent of this FlatFace in the tree or null if this FlatFace is the root
   /// </summary>
   public FlatFace Parent
   {
      get
      {
         return _parent;
      }
   }

   /// <summary>
   /// Construct a FlatFace
   /// </summary>
   /// <param name="graphNode">The Node in the Graph that this Face should correspond to</param>
   /// <param name="parent">The parent of this FlatFace in the "tree" that forms the Net</param>
   /// <param name="points"></param>
   public FlatFace(GraphNode graphNode, FlatFace parent, Vector2[] points)
   {
      _graphNode = graphNode;
      _parent = parent;
      _points = points;
      _connectedTo = new FlatFace[points.Length];
   }

   /// <summary>
   /// The set of FlatFaces to which this FlatFace is connected forming part of a tree structure
   /// </summary>
   public FlatFace[] ConnectedTo
   {
      get
      {
         return _connectedTo;
      }
   }

   /// <summary>
   /// The points corresponding to the corners of this FlatFace
   /// </summary>
   public Vector2[] Points
   {
      get
      {
         return _points;
      }
   }

   /// <summary>
   /// The number of Sides that this FlatFace has
   /// (which is also the same as the number of corners
   /// </summary>
   public int NumSides
   {
      get
      {
         return _points.Length;
      }
   }

   /// <summary>
   /// The geometric center of this FlatFace
   /// </summary>
   public Vector2 Center
   {
      get
      {
         return Geometry.Center(Points);
      }
   }
}
