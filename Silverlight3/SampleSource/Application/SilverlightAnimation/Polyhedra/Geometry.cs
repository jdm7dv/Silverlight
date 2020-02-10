using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.DirectX;

/// <summary>
/// This class slightly aguments the functionality of the DirectX Math classes.
/// Written by Declan Brennan, 2007 (http://declan.brennan.name)
/// There's not much here but it's made freely available.
/// </summary>
public static class Geometry
{
   /// <summary>
   /// Return the Matrix representing a perspective view where the view point is located d from the view 
   /// plane in the z direction
   /// </summary>
   /// <param name="d"></param>
   /// <returns></returns>
   public static Matrix Perspective(float d)
   {
      Matrix rc = Matrix.Identity;
      rc.M43 = 1 / d;
      return rc;
   }

   /// <summary>
   /// Convert a set of Vectors to Points scaling them to fit in a viewport
   /// </summary>
   /// <param name="vs"></param>
   /// <param name="xmin"></param>
   /// <param name="ymin"></param>
   /// <param name="range"></param>
   /// <param name="w"></param>
   /// <param name="h"></param>
   /// <returns></returns>
   public static Point[] Vector3ToPoint(Vector3[] vs, float xmin, float ymin, float range, float w, float h)
   {
      Point[] rc = new Point[vs.Length];
      int i = 0;
      foreach (Vector3 v in vs)
      {
         rc[i].X = (v.X - xmin) * w / range;
         rc[i].Y = (v.Y - ymin) * h / range;
         i++;
      }
      return rc;
   }

   /// <summary>
   /// Return the geometric center of a set of 3D points
   /// </summary>
   /// <param name="points"></param>
   /// <returns></returns>
   public static Vector3 Center(Vector3[] points)
   {
      float sumx = 0;
      float sumy = 0;
      float sumz = 0;

      foreach (Vector3 p in points)
      {
         sumx += p.X;
         sumy += p.Y;
         sumz += p.Z;
      }
      return new Vector3(sumx / points.Length, sumy / points.Length, sumz / points.Length);
   }

   /// <summary>
   /// Return the geometric center of a set of 2D points
   /// </summary>
   /// <param name="points"></param>
   /// <returns></returns>
   public static Vector2 Center(Vector2[] points)
   {
      float sumx = 0;
      float sumy = 0;
      foreach (Vector2 p in points)
      {
         sumx += p.X;
         sumy += p.Y;
      }
      return new Vector2(sumx / points.Length, sumy / points.Length);
   }

   /// <summary>
   /// Return a set of vectors corresponding to the sides of a regular Polygon
   /// </summary>
   /// <param name="from">The start point of the first side</param>
   /// <param name="to">The end point of the first side</param>
   /// <param name="numSides">The number of sides in the polygon</param>
   /// <returns></returns>
   public static Vector2[] GetPolyVectors(Vector2 from, Vector2 to, int numSides)
   {
      Vector2[] points = new Vector2[numSides];
      Matrix rotate = Matrix.RotationZ((float)Math.PI * 2 / numSides);

      Vector2 side = to - from;
      Vector2 point = from;
      for (int i = 0; i < numSides; i++)
      {
         points[i] = point;
         point = point + side;
         side = Vector2.TransformCoordinate(side, rotate);
      }
      return points;
   }
}



