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
    public partial class HitTesting : UserControl
    {
        private ModelVisual3D torusVisual;
        private ModelVisual3D imageVisual;
        private GeometryModel3D imageModel;

        private Viewport3D viewport;
        private Point lastMouseDownPosition;
        private bool torusSelected = false;
        private Matrix3D lastTransform = Matrix3D.Identity;

        public HitTesting()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(HitTesting_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(HitTesting_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(HitTesting_MouseMove);
        }

        private void HitTesting_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.torusSelected = false;
        }

        void HitTesting_MouseMove(object sender, MouseEventArgs e)
        {
            if (torusSelected)
            {
                Point p = e.GetPosition(this.viewport);
                double xDiff = p.X - lastMouseDownPosition.X;

                Matrix3D m = lastTransform;

                //Rotate around y axis
                RotateTransform3D r = new RotateTransform3D();
                r.Rotation = new AxisAngleRotation3D(new Vector3D(0,1,0), xDiff * 0.5);
                m.Append(r.Value);
                this.torusVisual.Transform = new MatrixTransform3D(m);
                this.imageModel.Transform = new MatrixTransform3D(r.Value);
            }
        }

        private void HitTesting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.lastMouseDownPosition = e.GetPosition(this.viewport);

            //See if the user clicked on any object in the 3D scene
            Kit3D.Windows.Media.VisualTreeHelper.HitTest(this.viewport,
                                                         null,
                                                         HitTestDown,
                                                         new PointHitTestParameters(this.lastMouseDownPosition));
        }

        HitTestResultBehavior HitTestDown(HitTestResult result)
        {
            RayMeshGeometry3DHitTestResult meshResult = result as RayMeshGeometry3DHitTestResult;

            //If the torus was clicked, keep track of the transform and the fact
            //that the torus was under the mouse pointer
            if (meshResult.VisualHit == this.torusVisual)
            {
                this.lastTransform = meshResult.VisualHit.Transform.Value;
                this.torusSelected = true;
            }

            //Keep looking for other models
            return HitTestResultBehavior.Continue;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadModels();

            Storyboard s = this.FindName("AnimationLoop") as Storyboard;
            s.Begin();
        }

        private void AnimationLoop_Completed(object sender, EventArgs e)
        {
            ((Storyboard)sender).Begin();
        }

        public void LoadModels()
        {
            this.viewport = new Viewport3D();
            this.viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.viewport.VerticalAlignment = VerticalAlignment.Stretch;

            //Get the mesh for the Torus
            Torus torus = new Torus(4, 0.3, 35, 8);
            MeshGeometry3D torusMesh = torus.MeshGeometry3D;

            GeometryModel3D torusModel = new GeometryModel3D();
            torusModel.Geometry = torusMesh;
            torusModel.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Blue)));

            //Want to render the torus in the scene, but rotate it and move it around a bit
            this.torusVisual = new ModelVisual3D();

            Transform3DGroup tg = new Transform3DGroup();
            tg.Children.Add(new ScaleTransform3D(1, 1, 1));

            RotateTransform3D r = new RotateTransform3D();
            r.Rotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90);
            tg.Children.Add(r);

            tg.Children.Add(new TranslateTransform3D(0, 1, 0));

            torusVisual.Transform = tg;
            torusVisual.Content = torusModel;
            this.viewport.Children.Add(torusVisual);
            //torusVisual.Opacity_DO_NOT_USE_WILL_BE_DELETED = 0.6;

            this.imageVisual = new ModelVisual3D();
            this.imageVisual.Transform = new ScaleTransform3D(6, 6, 1);
            imageModel = new GeometryModel3D();
            imageModel.Geometry = GeneratePlaneMesh();
            imageModel.SeamSmoothing = 1.5;

            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri("/Garden.jpg", UriKind.Relative));
            imageModel.Material = new DiffuseMaterial(new Kit3DBrush(img, 1024, 768));
            imageModel.BackMaterial = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Red)));
            imageVisual.Content = imageModel;
            this.viewport.Children.Add(imageVisual);

            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(0, 3, 15),
                                                          new Vector3D(0, -0.3, -1),
                                                          new Vector3D(0, 1, 0),
                                                          45);
            this.viewport.Camera = cam;
            this.LayoutRoot.Children.Add(viewport);
        }

        private MeshGeometry3D GeneratePlaneMesh()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                
                new Point3D(-0.5, 0.5, 0),
                new Point3D(0.5, 0.5, 0),
                new Point3D(-0.5, -0.5, 0),
                new Point3D(0.5, -0.5, 0)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                0, 2, 1,
                1, 2, 3
            };

            mesh.TextureCoordinates = new Kit3D.Windows.Media.PointCollection
            {
                new Point(0, 0),
                new Point(1, 0),
                new Point(0, 1),
                new Point(1, 1)
            };

            return mesh;
        }
    }
}
