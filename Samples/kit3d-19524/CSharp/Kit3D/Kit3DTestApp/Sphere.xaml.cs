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
    /// <summary>
    /// This sample was converted from Charles Petzold's 3D Programming for Windows
    /// book (really awesome book) www.charlespetzold.com
    /// </summary>
    public partial class Sphere : UserControl
    {
        private ModelVisual3D modvis;
        private Viewport3D viewport;

        public Sphere()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BeachBallSphere();
            Kit3D.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        double rotation = 0;
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
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
        }

        public void BeachBallSphere()
        {
            // Create Viewport3D as content of window.
            viewport = new Viewport3D();
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;
            //Kit3D.Windows.Media.CompositionTarget.FrameRate = 15;

            // Get the MeshGeometry3D from the GenerateSphere method.
            MeshGeometry3D mesh = GenerateSphere(new Point3D(0, 0, 0), 1, 36, 18);

            //1224 triangles

            // Define the GeometryModel3D.
            GeometryModel3D geomod = new GeometryModel3D();
            geomod.Geometry = mesh;
            geomod.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Blue)));

            //This pushes the polygons away from eachother so that we can see the different
            //polygons in the demo
            geomod.SeamSmoothing = -1;

            // Create a ModelVisual3D for the GeometryModel3D.
            modvis = new ModelVisual3D();
            modvis.Content = geomod;
            viewport.Children.Add(modvis);

            // Create the camera.
            PerspectiveCamera cam = new PerspectiveCamera(new Point3D(0, 0, 8),
                                                          new Vector3D(0, 0, -1),
                                                          new Vector3D(0, 1, 0),
                                                          45);
            viewport.Camera = cam;

            this.LayoutRoot.Children.Add(viewport);
        }

        MeshGeometry3D GenerateSphere(Point3D center, double radius,
                                      int slices, int stacks)
        {
            // Create the MeshGeometry3D.
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Fill the Position, Normals, and TextureCoordinates collections.
            for (int stack = 0; stack <= stacks; stack++)
            {
                double phi = Math.PI / 2 - stack * Math.PI / stacks;
                double y = radius * Math.Sin(phi);
                double scale = -radius * Math.Cos(phi);

                for (int slice = 0; slice <= slices; slice++)
                {
                    double theta = slice * 2 * Math.PI / slices;
                    double x = scale * Math.Sin(theta);
                    double z = scale * Math.Cos(theta);

                    Vector3D normal = new Vector3D(x, y, z);
                    //mesh.Normals.Add(normal);
                    mesh.Positions.Add(normal + center);
                    mesh.TextureCoordinates.Add(
                            new Point((double)slice / slices,
                                      (double)stack / stacks));
                }
            }

            // Fill the TriangleIndices collection.
            for (int stack = 0; stack < stacks; stack++)
            {
                for (int slice = 0; slice < slices; slice++)
                {
                    int n = slices + 1; // Keep the line length down.

                    if (stack != 0)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                    }
                    if (stack != stacks - 1)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice + 1);
                    }
                }
            }
            return mesh;
        }
    }
}
