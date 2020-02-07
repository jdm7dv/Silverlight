using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Polyhedra
{
   /// <summary>
   /// This class is used to generate a spectrum of colors to apply to the faces
   /// of the polyhedrons.
   /// A Hue,Brightness,Saturation Model is employed to make it easy to get an
   /// even spread of colors. Triangles are allocated red with other polygons moving
   /// up the spectrum (frequency increasing with side count)
   /// Written by Declan Brennan, 2007 (http://declan.brennan.name)
   /// There's not much here but it's made freely available.
   /// </summary>
   public static class PolyhedronPalette
   {
      /// <summary>
      /// The colors for painting the polygons for Faces indexed by side count
      /// </summary>
      private static Dictionary<int, Color> _colors = new Dictionary<int, Color>();

      /// <summary>
      /// Return the color used to paint a polygon with a specified number of faces.
      /// </summary>
      /// <param name="numSides"></param>
      /// <returns></returns>
      public static Color Lookup(int numSides)
      {
         if (_colors.Count == 0)
            init();
         return _colors[numSides];
      }

      /// <summary>
      /// Initialise all the colors in the _colors dictionary according to the
      /// algorithm described above
      /// </summary>
      private static void init()
      {
         int[] numSides = new int[] { 3, 4, 5, 6, 8, 10 };
         float saturation = 1f;
         float brightness = .8f;
         float hue = 0f;
         float hueDelta = 1f / (numSides.Length); hueDelta /= 3;

         foreach (int ns in numSides)
         {
            _colors[ns] = colorFromHSB(hue * 255f, saturation * 255f, brightness * 255f);
            hue += hueDelta;
         }
      }

      /// <summary>
      /// Generate an .NET color from its hue, saturation and brightness
      /// </summary>
      /// <param name="h"></param>
      /// <param name="s"></param>
      /// <param name="br"></param>
      /// <returns></returns>
      private static Color colorFromHSB(float h, float s, float br)
      {
         float r = br;
         float g = br;
         float b = br;
         if (s != 0)
         {
            float max = br;
            float dif = br * s / 255f;
            float min = br - dif;

            h = h * 360f / 255f;

            if (h < 60f)
            {
               r = max; g = h * dif / 60f + min; b = min;
            }
            else if (h < 120f)
            {
               r = -(h - 120f) * dif / 60f + min; g = max; b = min;
            }
            else if (h < 180f)
            {
               r = min; g = max; b = (h - 120f) * dif / 60f + min;
            }
            else if (h < 240f)
            {
               r = min; g = -(h - 240f) * dif / 60f + min; b = max;
            }
            else if (h < 300f)
            {
               r = (h - 240f) * dif / 60f + min; g = min; b = max;
            }
            else if (h <= 360f)
            {
               r = max; g = min; b = -(h - 360f) * dif / 60 + min;
            }
            else
            {
               r = 0; g = 0; b = 0;
            }
         }
         return Color.FromArgb   // CHANGE !!!!!!
         (
            0xFF,
            (byte)Math.Round(Math.Min(Math.Max(r, 0), 255)),
            (byte)Math.Round(Math.Min(Math.Max(g, 0), 255)),
            (byte)Math.Round(Math.Min(Math.Max(b, 0), 255))
         );

      }
   }
}
