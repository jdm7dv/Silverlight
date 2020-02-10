using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.DirectX;

namespace Polyhedra
{
   /// This class is used to project the Polyhedron
   /// onto the xaml canvas using a perspective transformation. The z-order of the 
   /// polygons corresponding to the individual faces is set so that they draw from back to front. 
   /// They are made semi-transparent so you can see the whole solid.generate a spectrum of colors to 
   /// apply to the faces of the polyhedrons.
   /// Written by Declan Brennan, 2007 (http://declan.brennan.name)
   /// This code is made freely available.
   /// </summary>
   public static class Projector
   {
      // Return the distance of the furthest point from the pivot in the center of the root face
      private static float spread(Polyhedron polyhedron, Vector3 pivot)
      {
         float maxD2 = 0f;
         foreach (Face face in polyhedron.Faces)
            foreach (Vector3 v in face.Points)
            {
               float d2 = (v - pivot).LengthSq();
               if (d2 > maxD2)
                  maxD2= d2;
            }
         return (float)Math.Sqrt(maxD2);
      }

      /// <summary>
      /// Do the actual projection of the supplied polygon into the canvas
      /// Before the actual projection the polyhedron is rotation by the yaw angle around a vertical Y axis through the pivot point
      /// at the center of the root Face. 
      /// The viewpoint is raised vertically and turned to look down towards the polyhedron (an X axis rotation)
      /// The the persective transformation is applied before the Polygons are generated. The z dimension in Vector3
      /// is used to generate the z-index in the xaml polygons
      /// </summary>
      /// <param name="polyhedron"></param>
      /// <param name="yaw"></param>
      /// <param name="pivot"></param>
      /// <param name="mag"></param>
      /// <param name="canvas"></param>
      public static void Project(Polyhedron polyhedron, float yaw, Vector3 pivot, float mag,Canvas canvas)
      {
         float angle= (float)Math.PI / 4;
         float w = spread(polyhedron, pivot)*mag;
         Vector3 viewPoint = new Vector3(0, w*0.6f, w);
         pivot.Y = -w / 2;
         if (polyhedron.Faces.Count <= 8)
            pivot.Y += w / 4;

         Matrix projection = Matrix.Translation(-pivot) * Matrix.RotationY(yaw) *Matrix.RotationX(angle) * Matrix.Translation(viewPoint) * Geometry.Perspective(5);

         Canvas childCanvas = new Canvas();
         childCanvas.Width = canvas.Width;
         childCanvas.Height = canvas.Height;
         Vector3 projectedPivot = Vector3.TransformCoordinate(pivot, projection);

         foreach (Face face in polyhedron.Faces)
         {
            Vector3[] p = Vector3.TransformCoordinate(face.Points, projection);
            Vector3 c = Geometry.Center(p);
            int zIndex = - (int)(c.Z * int.MaxValue / 200);
            System.Windows.Shapes.Polygon pg = new System.Windows.Shapes.Polygon();
            pg.SetValue(Canvas.ZIndexProperty, zIndex);
            Point[] points = Geometry.Vector3ToPoint(p, projectedPivot.X - w / 2, projectedPivot.Y - w / 2, w, (float)canvas.Width, (float)canvas.Height);
            //pg.Points. = points; ////// CHANGE !!!!
            foreach (Point pt in points)
               pg.Points.Add(pt);
            pg.StrokeThickness = 1;
            pg.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            System.Windows.Media.Color col= PolyhedronPalette.Lookup(face.NumSides);
            pg.Fill = new System.Windows.Media.SolidColorBrush(col);
            pg.Opacity = 0.8;
            childCanvas.Children.Add(pg);
         }
         canvas.Children.Clear();
         canvas.Children.Add(childCanvas);
      }
   }
}
