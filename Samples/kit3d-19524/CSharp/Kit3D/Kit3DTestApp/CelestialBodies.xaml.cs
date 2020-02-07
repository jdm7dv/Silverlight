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

namespace TestApp
{
    /// <summary>
    /// This sample was converted from Charles Petzold's 3D Programming for Windows
    /// book (really awesome book) www.charlespetzold.com
    /// </summary>
    public partial class CelestialBodies : UserControl
    {
        public CelestialBodies()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(SilverlightControl1_Loaded);
        }

        private Viewport3D viewport;
        private DateTime startAnimation = DateTime.Now;

        private MeshGeometry3D CreateCubeMesh()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                new Point3D(-0.5,0.5,0.5),
                new Point3D(0.5,0.5,0.5),
                new Point3D(-0.5,-0.5,0.5),
                new Point3D(0.5,-0.5,0.5),
                new Point3D(0.5,0.5,-0.5),
                new Point3D(-0.5, 0.5, -0.5),
                new Point3D(0.5,-0.5,-0.5),
                new Point3D(-0.5,-0.5,-0.5),
                new Point3D(-0.5,0.5,-0.5),
                new Point3D(-0.5,0.5,0.5),
                new Point3D(-0.5,-0.5,-0.5),
                new Point3D(-0.5,-0.5,0.5),
                new Point3D(0.5,0.5,0.5),
                new Point3D(0.5,0.5,-0.5),
                new Point3D(0.5,-0.5,0.5),
                new Point3D(0.5,-0.5,-0.5),
                new Point3D(-0.5,0.5,-0.5),
                new Point3D(0.5,0.5,-0.5),
                new Point3D(-0.5,0.5,0.5),
                new Point3D(0.5,0.5,0.5),
                new Point3D(0.5,-0.5,-0.5),
                new Point3D(-0.5,-0.5,-0.5),
                new Point3D(0.5,-0.5,0.5),
                new Point3D(-0.5,-0.5,0.5)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                0, 2, 1, 1, 2, 3,
                4, 6, 5, 5, 6, 7,
                8, 10, 9, 9, 10, 11,
                12, 14, 13, 13, 14, 15,
                16, 18, 17, 17, 18, 19,
                20, 22, 21, 21, 22, 23
            };

            return mesh;
        }

        RotateTransform3D earthRotate;
        RotateTransform3D moonRotate;
        RotateTransform3D sunRotate;
        RotateTransform3D earthMoonRotate;

        void SilverlightControl1_Loaded(object sender, RoutedEventArgs e)
        {
            ModelVisual3D sunVisual = new ModelVisual3D();
            GeometryModel3D sunModel = new GeometryModel3D();
            sunModel.Geometry = CreateCubeMesh();
            sunModel.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Yellow)));
            sunVisual.Content = sunModel;

            Transform3DGroup sunTransform = new Transform3DGroup();
            sunTransform.Children.Add(new ScaleTransform3D(3, 3, 3));

            sunRotate = new RotateTransform3D();
            sunRotate.Rotation = new AxisAngleRotation3D();
            sunTransform.Children.Add(sunRotate);
            sunModel.Transform = sunTransform;


            ModelVisual3D earthMoonVisual = new ModelVisual3D();
            Model3DGroup earthMoonModels = new Model3DGroup();
            earthMoonVisual.Content = earthMoonModels;

            GeometryModel3D earth = new GeometryModel3D();
            earth.Geometry = CreateCubeMesh();
            earth.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Cyan)));

            earthRotate = new RotateTransform3D();
            earthRotate.Rotation = new AxisAngleRotation3D();
            earth.Transform = earthRotate;
            earthMoonModels.Children.Add(earth);

            GeometryModel3D moon = new GeometryModel3D();
            moon.Geometry = CreateCubeMesh();
            moon.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.LightGray)));
            earthMoonModels.Children.Add(moon);

            Transform3DGroup moonTransforms = new Transform3DGroup();
            moonTransforms.Children.Add(new ScaleTransform3D(0.3, 0.3, 0.3));
            moonTransforms.Children.Add(new TranslateTransform3D(2, 0, 0));

            moonRotate = new RotateTransform3D();
            moonRotate.Rotation = new AxisAngleRotation3D();
            moonTransforms.Children.Add(moonRotate);

            moon.Transform = moonTransforms;

            Transform3DGroup earthMoonTransforms = new Transform3DGroup();
            earthMoonTransforms.Children.Add(new TranslateTransform3D(10, 0, 0));
            earthMoonRotate = new RotateTransform3D();
            earthMoonTransforms.Children.Add(earthMoonRotate);

            earthMoonVisual.Transform = earthMoonTransforms;
            viewport = new Viewport3D();
            viewport.Camera = new PerspectiveCamera(new Point3D(-5, 15,25),
                                                    new Vector3D(5, -15, -25),
                                                    new Vector3D(0,1,0),
                                                    45);
            viewport.Children.Add(sunVisual);
            viewport.Children.Add(earthMoonVisual);
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;

            this.LayoutRoot.Children.Add(viewport);
            Kit3D.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            ((AxisAngleRotation3D)earthMoonRotate.Rotation).Angle += 0.2;
            ((AxisAngleRotation3D)sunRotate.Rotation).Angle += 1;
            ((AxisAngleRotation3D)earthRotate.Rotation).Angle += 3;
            ((AxisAngleRotation3D)moonRotate.Rotation).Angle += 5;
        }
    }
}
