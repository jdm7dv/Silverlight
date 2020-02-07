using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Polyhedra;
using Microsoft.DirectX;

namespace Polyhedra
{
   public partial class Page : UserControl
   {
      /// <summary>
      /// The currently active (selected) polyhedron in the wheel
      /// </summary>
      private FrameworkElement _activeObject;
      /// <summary>
      /// Set if automatic cycling through the polyhedra on the wheel is enabled
      /// </summary>
      private bool _cycling;
      /// <summary>
      /// The ids of the 18 available polyhedra- these ids are used to reference the resources for the
      /// graph connectivity and dihedral angles (See graph.cs)
      /// </summary>
      string[] _polyhedrons = { 
            "tetrahedron","cube","octahedron","dodecahedron","icosahedron","truncated_octahedron","truncated_cube",
            "truncated_cuboctahedron","truncated_dodecahedron","truncated_icosahedron","truncated_icosidodecahedron",
            "cuboctahedron","icosidodecahedron","rhombicosidodecahedron","rhombicuboctahedron","snub_cuboctahedron",
            "snub_icosidodecahedron","truncated_tetrahedron" };
      /// <summary>
      /// Keep track of the rotation of the polyhedron about the Y axis
      /// </summary>
      float _yaw = (float)Math.PI / 5;
      /// <summary>
      /// Keep track of how closed the polyhedron is
      /// 0= fully open, 1= fully closed.
      /// The parameter goes slightly outside these ranges but only to allow some additional time with the
      /// polyhedron completely closed and the net completely open
      /// </summary>
      float _howClosed = 1f;
      /// <summary>
      /// The amount to adjust the howClosed by for each frame
      /// </summary>
      float _howClosedDelta = .01f;
      /// <summary>
      /// The amount to adjust the yaw by for each frame
      /// </summary>
      float _yawDelta = 0.025f;
      /// <summary>
      /// The Net corresponding to the currently active Polyhedron
      /// </summary>
      Net _net = null;

      /// <summary>
      /// Perform initialisation: Set up the animation timer handler
      public Page()
      {
         // Required to initialize variables
         InitializeComponent();
         this.animationTimer.Completed += new EventHandler(animationTimer_Completed);
      }

      /// <summary>
      /// Return the index into the array of polyhedron ids corresponding to the currently active polyhedron
      /// </summary>
      private int currentPolyhedronIndex
      {
         get
         {
            if (this._activeObject == null)
               return -1;
            string name = _activeObject.Name;
            int i = name.Length;
            while (char.IsDigit(name[i - 1]))
               --i;
            return int.Parse(name.Substring(i));
         }
      }

      /// <summary>
      /// CurrentPolyhedronId gets or sets the currently active polyhedron by id
      /// The currently active polyhedron can also be set by moving the mouse over a polyhedron on the wheel
      /// or by enabling the Cycle button
      /// </summary>
      public string CurrentPolyhedronId
      {
         get
         {
            int j = this.currentPolyhedronIndex;
            if (j < 0)
               return "";
            return _polyhedrons[j];
         }
         set
         {
            if (this.CurrentPolyhedronId != value)
               this.activateObject((FrameworkElement)this.FindName("Canvas" + Array.IndexOf(this._polyhedrons, value)));
         }
      }

      /// <summary>
      /// Return the (human-readable) name of the Currently active Polyhedron. This is generated automatically
      /// from its id by replacing each underscore with a space and capitalizing the first letter of each word
      /// </summary>
      public string CurrentPolyhedronName
      {
         get
         {
            string rc = "";
            bool startWord = true;
            foreach (char ch in this.CurrentPolyhedronId)
            {
               if (ch == '_')
               {
                  rc += " ";
                  startWord = true;
               }
               else if (startWord)
               {
                  rc += char.ToUpper(ch);
                  startWord = false;
               }
               else
                  rc += ch;
            }
            return rc;
         }
      }

      /// <summary>
      /// Return the description of the Face Types for the currently active polyhedron
      /// </summary>
      public string CurrentFaceTypes
      {
         get
         {
            string rc = "";
            if (_net == null)
               return "";
            int[] numSides = new int[] { 3, 4, 5, 6, 8, 10 };
            string[] polygonNames = { "Triangle", "Square", "Pentagon", "Hexagon", "Octagon", "Decagon" };
            int j = 0;
            foreach (int ns in numSides)
            {
               int i = 0;
               foreach (FlatFace face in _net.Faces)
                  if (face.NumSides == ns)
                     i++;
               if (i > 0)
               {
                  if (rc.Length > 0)
                     rc += "\r\n";
                  rc += string.Format("{0} {1}s", i, polygonNames[j]);
               }
               j++;
            }
            rc += "\r\n" + _net.NumFaces.ToString() + " in Total";
            return rc;
         }
      }

      /// <summary>
      /// Update all name and face information for the newly activated Polyhedron
      /// </summary>
      private void updateInfo()
      {
         this.Info.Visibility = Visibility.Visible;
         this.PolyhedronNameInfo.Text = this.CurrentPolyhedronName;
         this.FaceTypesInfo.Text = this.CurrentFaceTypes;
      }

      bool _paused = false; // Is cycling currently paused?
      
      /// <summary>
      /// When the Cycle property is set, the animation automatically cycles through all the polyhedrons on the wheel
      /// The button's caption is set according to its latch state. If latched, the arrow is also animated using a storyboard
      /// </summary>
      public bool Cycling
      {
         get
         {
            return _cycling;
         }
         set
         {
            if (value != _cycling)
            {
               _cycling = value;
               if (_cycling)
               {
                  if (this._paused)
                     this.CycleLatched.Resume();
                  else
                     this.CycleLatched.Begin();
                  this.CycleCaption.Text = "Cycling";
               }
               else
               {
                  this.CycleLatched.Pause();
                  _paused = true;
                  this.CycleCaption.Text = "Cycle";
               }
            }
         }
      }

      /// <summary>
      /// Handle a click on the Cycle button by toggling its latch state
      /// </summary>
      /// <param name="o"></param>
      /// <param name="e"></param>
      public void CycleButtonLeftMouseDown(object o, EventArgs e)
      {
         this.Cycling = !this.Cycling;
      }

      /// <summary>
      /// Activate the object that is being hovered over in the polygon wheel
      /// This involves running storyboards for the newly deactivated object as well as the newly activated one
      /// It also involves updating the information displayed to reflect the new polyhedron
      /// Finally a graph corresponding to the new polyhedron is loaded and an animation frame is triggered
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      private bool activateObject(FrameworkElement obj)
      {
         if (!this.triggerStoryboard(obj, "MouseEnter"))
            return false;

         if (this._activeObject != null)
            this.triggerStoryboard(this._activeObject, "MouseLeave");
         this._activeObject = obj;
         Graph graph = new Graph(this.CurrentPolyhedronId);
         _net = NetBuilder.Build(graph);
         this.updateInfo();
         this.animationTimer.Begin();
         return true;
      }

      private object _mouseEnteredObj;

      /// <summary>
      /// Activate the object we have just moved the mouse over (if it has a storyboard)
      /// Use _mouseEnteredObj to disregard incorrect multiple "MouseEnter" events for the same object
      /// </summary>
      /// <param name="o"></param>
      /// <param name="e"></param>
      public void TriggerMouseEnter(object o, EventArgs e)
      {
         if (_mouseEnteredObj != o)
         {
            _mouseEnteredObj = o;
            if (this.activateObject(o as FrameworkElement))
               this.Cycling = false;
         }
      }

      /// <summary>
      /// Not in use. The "MouseLeave" storyboard is now begun for the currently active object when
      /// the "MouseEnter" event activates another object
      /// </summary>
      /// <param name="o"></param>
      /// <param name="e"></param>
      public void TriggerMouseLeave(object o, EventArgs e)
      {
      }

      /// <summary>
      /// Wire up events to triggers as only loaded is handled automatically in the Alpha
      /// </summary>
      /// <param name="o"></param>
      /// <param name="eventType"></param>
      private bool triggerStoryboard(object o, string eventType)
      {
         Canvas el = o as Canvas;

         string name = el.GetValue(NameProperty) as String;
         Storyboard sb = el.FindName(name + eventType) as Storyboard;
         if (sb != null)
            sb.Begin();
         return (sb != null);
      }

      /// <summary>
      /// Clip or clamp a value to a range
      /// </summary>
      /// <param name="v"></param>
      /// <param name="min"></param>
      /// <param name="max"></param>
      /// <returns></returns>
      private static float clip(float v, float min, float max)
      {
         return (v < min) ? min : ((v > max) ? max : v);
      }

      /// <summary>
      /// And last but not least the whole point of the whole application- perform a frame of the animation
      /// showing the polyhedron rotating while opening or closing
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void animationTimer_Completed(object sender, EventArgs e)
      {
         if (_net == null)
            return;
         Vector2 pivot = _net.Root.Center;
         Polyhedron polyhedron = PolyhedronBuilder.Build(_net, clip(_howClosed, 0, 1));
         int n = polyhedron.Faces.Count;
         Projector.Project(polyhedron, _yaw, new Vector3(pivot.X, 0f, pivot.Y), (n <= 20) ? ((n <= 8) ? 1.6f : 1.2f) : 1.0f, this.Animation);
         _yaw += _yawDelta;
         _howClosed += _howClosedDelta;
         if (_howClosed > 1.0f)
            _howClosedDelta = +_howClosedDelta;
         /*else if (_howClosed < -0.2f)
         {
            _howClosedDelta = -_howClosedDelta;
            if (this.Cycling)
            {
               int i = (this.currentPolyhedronIndex + this._polyhedrons.Length - 1) % this._polyhedrons.Length;
               this.CurrentPolyhedronId = this._polyhedrons[i];
            }
         }*/
         this.animationTimer.Begin();
      }
   }
}
