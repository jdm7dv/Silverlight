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
    public partial class BackMaterial : UserControl
    {
        private GeometryModel3D model;
        private ModelVisual3D modvis;
        private Viewport3D viewport;

        public BackMaterial()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CreatePlane();
            Kit3D.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }


        double rotation = 0;
        int count = 0;
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //Change the front material
            if (count == 100)
            {
                model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Green)));
            }

            if (count == 200)
            {
                model.BackMaterial = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Orange)));
            }

            if (modvis != null)
            {
                Transform3DGroup tg = new Transform3DGroup();

                tg.Children.Add(new RotateTransform3D(
                                    new AxisAngleRotation3D(
                                        new Vector3D(0 ,10 ,0), 
                                        rotation), 
                                        new Point3D(0,0,0)));
                modvis.Transform = tg;
            }

            rotation += 3;
            count++;
        }

        public void CreatePlane()
        {
            // Create Viewport3D as content of window.
            viewport = new Viewport3D();
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;

            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                new Point3D(-5, 5, 5),
                new Point3D(5, 5, 5),
                new Point3D(5, -5, 5),
                new Point3D(-5, -5, 5)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                3,1,0,
                3,2,1
            };

            model = new GeometryModel3D();
            model.Geometry = mesh;
            model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Blue)));

            //This pushes the polygons closer to eachother so the seams between
            //the polygons which get antialiased are not as visible
            model.SeamSmoothing = 1;

            // Create a ModelVisual3D for the GeometryModel3D.
            modvis = new ModelVisual3D();
            modvis.Content = model;
            viewport.Children.Add(modvis);

            // Create the camera.
            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(0, 0, 30), 
                            new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 45);
            viewport.Camera = cam;

            this.LayoutRoot.Children.Add(viewport);
        }
    }
}
