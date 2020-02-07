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

using Kit3D.Util;
using Kit3D.Objects;

namespace TestApp
{
    public partial class GeneralTransformTesting : UserControl
    {
        public GeneralTransformTesting()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(GeneralTransformTesting_Loaded);
        }

        void GeneralTransformTesting_Loaded(object sender, RoutedEventArgs e)
        {
            mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                new Point3D(-1, 1, 0),
                new Point3D(1, 1, 0),
                new Point3D(1, -1, 0),
                new Point3D(-1, -1, 0)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                2, 1, 0,
                2, 0, 3
            };

            mesh.TextureCoordinates = new Kit3D.Windows.Media.PointCollection
            {
                new Point(0,0),
                new Point(1,0),
                new Point(1,1),
                new Point(0,1)
            };

            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = mesh;
            model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Red)));

            modelVisual = new ModelVisual3D();
            modelVisual.Content = model;

            viewport = new Viewport3D();
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;

            viewport.Camera = new PerspectiveCamera(new Point3D(0, 0, 10),
                                                    new Vector3D(0, 0, -1),
                                                    new Vector3D(0, 1, 0),
                                                    45);
            viewport.Children.Add(modelVisual);
            this.LayoutRoot.Children.Add(viewport);

            p.Fill = new SolidColorBrush(Colors.Purple);
            p.Fill.Opacity = 0.3;
            this.LayoutRoot.Children.Add(p);

            axis = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            modelVisual.Transform = new RotateTransform3D(axis);

            //Hook up to the rendering event so we can do custom animation
            Kit3D.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        Viewport3D viewport;
        MeshGeometry3D mesh;
        ModelVisual3D modelVisual;
        Polygon p = new Polygon();
        AxisAngleRotation3D axis;
        double d = 0;

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (modelVisual != null)
            {
                GeneralTransform3DTo2D t = modelVisual.TransformToAncestor(viewport);
                Point ps1 = t.Transform(mesh.Positions[0]);
                Point ps2 = t.Transform(mesh.Positions[1]);
                Point ps3 = t.Transform(mesh.Positions[2]);
                Point ps4 = t.Transform(mesh.Positions[3]);

                p.Points = new System.Windows.Media.PointCollection();
                p.Points.Add(ps1);
                p.Points.Add(ps2);
                p.Points.Add(ps3);
                p.Points.Add(ps4);

                d += 3;
                axis.Angle = d;
            }
        }
    }
}
