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
    public partial class HitTesting2 : UserControl
    {
        private Viewport3D viewport;
        private GeometryModel3D model;
        private GeometryModel3D model2, model3;

        public HitTesting2()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(HitTesting2_Loaded);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(HitTesting2_MouseLeftButtonDown);
        }

        void HitTesting2_Loaded(object sender, RoutedEventArgs e)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                new Point3D(-1, 1, 0),
                new Point3D(1, 1, 0),
                new Point3D(1, -1, 0),
                new Point3D(-1, -1, 0)/*,
                new Point3D(0, 2, 2),
                new Point3D(2, 2, 2),
                new Point3D(2, 0, 2),
                new Point3D(0, 0, 2)*/
            };

            mesh.TriangleIndices = new Int32Collection
            {
                2,1,0,
                2,0,3/*,
                6,5,4,
                6,4,7*/
            };

            model = new GeometryModel3D();
            model.SeamSmoothing = 1;
            model.Geometry = mesh;
            model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Red)));
            //model.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 180));
            
            model2 = new GeometryModel3D();
            model2.SeamSmoothing = 1;
            model2.Geometry = mesh;
            model2.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Green)));
            model2.Transform = new TranslateTransform3D(0.2, 0, 1);

            model3 = new GeometryModel3D();
            model3.SeamSmoothing = 1;
            model3.Geometry = mesh;
            model3.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Yellow)));
            model3.Transform = new TranslateTransform3D(0.4, 0, 2);

            Model3DGroup modelGroup = new Model3DGroup();
            modelGroup.Children.Add(model3);
            modelGroup.Children.Add(model);
            modelGroup.Children.Add(model2);
            

            ModelVisual3D modelVisual = new ModelVisual3D();
            modelVisual.Content = modelGroup;

            //ModelVisual3D lightVisual = new ModelVisual3D();
            //lightVisual.Content = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));

            viewport = new Viewport3D();
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;
            viewport.Children.Add(modelVisual);
            //viewport.Children.Add(lightVisual);

            viewport.Camera = new PerspectiveCamera(new Point3D(0, 0, 10),
                                                    new Vector3D(0, 0, -1),
                                                    new Vector3D(0, 1, 0),
                                                    45);

            this.LayoutRoot.Children.Add(viewport);
            ((Storyboard)this.FindName("AnimationLoop")).Begin();
        }

        void HitTesting2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Kit3D.Windows.Media.VisualTreeHelper.HitTest(viewport, null, HitTest, new PointHitTestParameters(e.GetPosition(viewport)));
        }

        private HitTestResultBehavior HitTest(HitTestResult result)
        {
            RayMeshGeometry3DHitTestResult res = result as RayMeshGeometry3DHitTestResult;
            if (res.ModelHit == model)
            {
                //int i = 0;
            }
            else if (res.ModelHit == model2)
            {
                //int i = 0;
            }
            else if (res.ModelHit == model3)
            {
                //int i = 0;
            }

            return HitTestResultBehavior.Continue;
        }
    }
}
