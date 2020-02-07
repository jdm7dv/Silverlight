// Build an open Polyhedron formed by partially closing the supplied Net
// Copyright Declan Brennan, 2007 (http://declan.brennan.name)
// Please don't use without permission

using System;
using System.Collections.Generic;
using Microsoft.DirectX;

/// <summary>
/// Build a Polyhedron from a Net
/// </summary>
public static class PolyhedronBuilder
{
   /// <summary>
   /// Build a partially closed Polyhedron from the supplied Net and a parameter
   /// (from 0 to 1) indicating how much to close it
   /// </summary>
   public static Polyhedron Build(Net n, float howClosed)
    {
        Face root = new Face(n.Root);
        root.Articulate(howClosed);
        Polyhedron rc = new Polyhedron();
        root.AddToPolyhedron(rc);
        return rc;
    }
}

/// <summary>
/// A partially closed Polyhedron
/// </summary>
public class Polyhedron
{
   /// <summary>
   /// The set of Faces in this Polyhedron
   /// </summary>
   public List<Face> Faces = new List<Face>();

   public Polyhedron()
   {
   }

   /// <summary>
   /// Add a face to this polyhedron
   /// </summary>
   /// <param name="face"></param>
   public void AddFace(Face face)
   {
      Faces.Add(face);
   }
}

/// <summary>
/// A face oriented in 3D space that is part of a partially closed polyhedron
/// </summary>
public class Face
{
   /// <summary>
   /// The set of points corresponding to the corners of this Face
   /// </summary>
   Vector3[] _points;
   /// <summary>
   /// The FlatFace (in the Net) that this Face is based on
   /// </summary>
   FlatFace _flatFace;
   /// <summary>
   /// Because the Polyhedron may not be fully closed the Faces still form a tree
   /// This is the set of Faces that are connected to this Face by "hindges"
   /// </summary>
   List<Face> _connectedTo;
   /// <summary>
   /// The angle that this Face forms at the hindge with its parent Face
   /// </summary>
   float _dihedralAngle;

   /// <summary>
   /// The FlatFace (in the Net) that this Face is based on
   /// </summary>
   public FlatFace FlatFace
   {
      get
      {
         return _flatFace;
      }
   }

   /// <summary>
   /// The unique Id for this Face which is the same as the Id of its corresponding GraphNode
   /// </summary>
   public string Id
   {
      get
      {
         return _flatFace.GraphNode.Id;
      }
   }

   /// <summary>
   /// The array of 3D Points corresponding to the corners of this Face
   /// </summary>
   public Vector3[] Points
   {
      get
      {
         return _points;
      }
      set
      {
         _points = value;
      }
   }

   /// <summary>
   /// The number of sides (edges) on this Face
   /// </summary>
   public int NumSides
   {
      get
      {
         return _points.Length;
      }
   }

   /// <summary>
   /// The set of child Faces that this Face is connected to in the tree
   /// </summary>
   public List<Face> ConnectedTo
   {
      get
      {
         return _connectedTo;
      }
   }

   /// <summary>
   /// Construct a 3D face from the corresponding 2D FlatFace in the Net
   /// As well as copying the corner points from the FlatFace we also get the dihedral angle from the cache that
   /// this Face should make along the hinge with its parent in a fully closed Polyhedron
   /// This face is then moved by hinge rotation.
   /// It is discarded and recreated from the 2D FlatFace before the next frame in the animation
   /// </summary>
   /// <param name="face"></param>
   public Face(FlatFace face)
   {
      _points = new Vector3[face.GraphNode.NumConnections];
      for (int s = 0; s < _points.Length; s++)
      {
         _points[s].X = face.Points[s].X;
         _points[s].Z = face.Points[s].Y;
      }
      _flatFace = face;
      _connectedTo = new List<Face>();
      if (face.Parent != null)
         _dihedralAngle = face.GraphNode.Graph.DihedralCache.GetDihedralAngle(face.Parent.GraphNode.Id, face.GraphNode.Id);

      foreach (FlatFace child in face.ConnectedTo)
         if (child != null)
            _connectedTo.Add(new Face(child));
   }

   /// <summary>
   /// Add this Face and all its children to the specified Polyhedron
   /// </summary>
   /// <param name="polyhedron"></param>
   public void AddToPolyhedron(Polyhedron polyhedron)
   {
      polyhedron.AddFace(this);
      foreach (Face child in this._connectedTo)
         if (child != null)
            child.AddToPolyhedron(polyhedron);
   }

   /// <summary>
   /// Return the geometric center of this face in 3D
   /// </summary>
   public Vector3 Center
   {
      get
      {
         int num = 0;
         Vector3 sum = new Vector3();
         foreach (Vector3 p in Points)
         {
            sum += p;
            num++;
         }
         return sum * (1.0f / num);
      }
   }

   /// <summary>
   /// Perform the rotation of a face about the hinge with its parent.
   /// The parent is always joined at the first two points on the polygon so these form the end points of the hinge
   /// </summary>
   /// <param name="proportion"></param>
   public void Articulate(float proportion)
   {
      if (proportion > 1.0f)
         throw new ArgumentException("Proportion cannot be greater than 1");
      Articulate(_points[0], _points[1], proportion);
   }

   /// <summary>
   /// Rotate this face and all its child faces about the hinge joining this Face to its parent in the tree
   /// This will rotate a set of faces rigidly about the hinge
   /// The dihedral angle is multiplied by proportion to decide how far to close the polyhedron
   /// Finally recurse to perform the same operation for all hinges of children in the Face tree
   /// </summary>
   /// <param name="axisFrom"></param>
   /// <param name="axisTo"></param>
   /// <param name="proportion"></param>
   public void Articulate(Vector3 axisFrom, Vector3 axisTo, float proportion)
   {
      Vector3 axis = axisTo - axisFrom;
      Matrix transform = Matrix.Translation(-axisFrom) * Matrix.RotationAxis(axis, proportion * _dihedralAngle) * Matrix.Translation(axisFrom);
      Transform(transform);
      foreach (Face child in _connectedTo)
         if (child != null)
            child.Articulate(proportion);
   }

   /// <summary>
   /// Apply the transformation Matrix to all the points in this Face and all the "child" faces
   /// connected to it
   /// </summary>
   /// <param name="transform"></param>
   public void Transform(Matrix transform)
   {
      _points = Vector3.TransformCoordinate(_points, transform);
      foreach (Face child in _connectedTo)
         child.Transform(transform);
   }
}
