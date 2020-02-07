/*
 * Copyright Mark Dawson 2008 - http://www.markdawson.org/kit3D
*/

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

using Kit3D.Windows.Controls;
using Kit3D.Windows.Media;
using Kit3D.Windows.Media.Media3D;
using System.Threading;
using System.Windows.Media.Imaging;

namespace TestApp
{
    //This does not render anything, I just use it for checking the bounds of
    //the Model3Ds are calculated correctly
    public partial class BoundsTesting : UserControl
    {
        public BoundsTesting()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BoundsTesting_Loaded);
        }

        void BoundsTesting_Loaded(object sender, RoutedEventArgs e)
        {
            Viewport3D viewport = new Viewport3D();

            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                new Point3D(-0.5, 0.5, 0),
                new Point3D(0.5,0.5,0),
                new Point3D(0.5, -0.5, 0),
                new Point3D(-0.5, -0.5, 0)
            };
            mesh.TriangleIndices = new Int32Collection
            {
                3,1,0,
                3,2,1
            };

            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = mesh;
            model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Red)));

            ModelVisual3D modelVisual = new ModelVisual3D();
            modelVisual.Content = model;

            viewport.Camera = new PerspectiveCamera(new Point3D(0, 0, 5),
                                                    new Vector3D(0, 0, -1),
                                                    new Vector3D(0, 1, 0),
                                                    45);
            viewport.Children.Add(modelVisual);
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;
            this.LayoutRoot.Children.Add(viewport);

            //Just testing that the bounds match exactly to the numbers created 
            //in WPF

            Rect3D bounds = model.Bounds;

            model.Transform = new ScaleTransform3D(2, 2, 1, 0, 0, 0);
            Rect3D bounds2 = model.Bounds;

            model.Transform = new TranslateTransform3D(10, 5, -2);
            Rect3D bounds3 = model.Bounds;

            RotateTransform3D rt = new RotateTransform3D();
            rt.Rotation = new AxisAngleRotation3D(new Vector3D(1, 0.8, 2.2), 66);
            model.Transform = rt;
            Rect3D bounds4 = model.Bounds;
        }

        private void AnimationLoop_Completed(object sender, EventArgs e)
        {

        }
    }
}
