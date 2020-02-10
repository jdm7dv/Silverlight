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
using System.Windows.Media.Imaging;

namespace TestApp
{
    /// <summary>
    /// This sample was converted from Charles Petzold's 3D Programming for Windows
    /// book (really awesome book) www.charlespetzold.com
    /// </summary>
    public partial class AnimateAxis : UserControl
    {
        public AnimateAxis()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(SilverlightControl1_Loaded);
        }

        AxisAngleRotation3D axisRotation;
        Viewport3D viewport;
        DateTime startAnimation = DateTime.Now;
        PerspectiveCamera camera;

        void SilverlightControl1_Loaded(object sender, RoutedEventArgs e)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                
                new Point3D(-0.5, 0.5, 0),
                new Point3D(0.5, 0.5, 0),
                new Point3D(-0.5, -0.5, 0),
                new Point3D(0.5, -0.5, 0)
                
                /*
                new Point3D(-0.5, 0.5, 0),
                new Point3D(0.7, 0.7, 0),
                new Point3D(-0.7, -0.5, 0),
                new Point3D(0.7, -0.3, 0)
                 */
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

            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = mesh;
            //model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Cyan)));

            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri("/Garden.jpg", UriKind.Relative));
            model.Material = new DiffuseMaterial(new Kit3DBrush(img, 1024, 768));

            //Have to move the polygons closer to eachother due to Silverlights antialiasing algorithm
            model.SeamSmoothing = 1;

            viewport = new Viewport3D();
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;
            PerspectiveCamera c = new PerspectiveCamera(new Point3D(0, 0, 30),
                                                    new Vector3D(0, 0, -1),
                                                    new Vector3D(0, 1, 0),
                                                    15);
            c.FarPlaneDistance = 10000;
            camera = c;
            viewport.Camera = camera;

            //The ModelVisual3D class knows how to render the Model3D instance, we can create a number
            //of these and place them at different locations in the world co-ord
            for (int x = 0; x < 4; ++x)
            {
                for (int y = 0; y < 4; ++y)
                {
                    ModelVisual3D modelVisual = new ModelVisual3D();
                    modelVisual.Content = model;
                    //ScaleTransform3D st = new ScaleTransform3D(2, 2, 1, 0, 0, 0);
                    //TranslateTransform3D tt = new TranslateTransform3D(-4 + x*2.5, -4 + y*2.5, 0);
                    //Transform3DGroup g = new Transform3DGroup();
                    //g.Children.Add(st);
                    //g.Children.Add(tt);
                    modelVisual.Transform = new TranslateTransform3D(-2 + x, -2 + y, 0);
                    viewport.Children.Add(modelVisual);
                }
            }

            //We will apply the transform to the model, this way all model visual instances 
            //will draw the updated visual
            RotateTransform3D rotation = new RotateTransform3D();
            axisRotation = new AxisAngleRotation3D();
            axisRotation.Angle = 60;
            rotation.Rotation = axisRotation;
            model.Transform = rotation;

            //Add the viewport to the page
            this.LayoutRoot.Children.Add(viewport);
            startAnimation = DateTime.Now;

            Kit3D.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            AnimationLoop_Completed(null, null);
        }

        Vector3D zeroSecond = new Vector3D(-1, 0, 0);
        Vector3D oneSecond = new Vector3D(0, -1, 0);
        Vector3D twoSeconds = new Vector3D(1, 0, 0);
        Vector3D threeSeconds = new Vector3D(0, 1, 0);
        Vector3D fourSeconds = new Vector3D(-1, 0, 0);

        int elapsedTime = 0;
        private void AnimationLoop_Completed(object sender, EventArgs e)
        {
            //camera.FieldOfView -= 0.1;
            //camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z-0.1);
            //TODO: Work on animation classes :-) and add an extra 50 hours to every day

            elapsedTime = (elapsedTime + 30) % 4000;
            double startX, endX, startY, endY, startZ, endZ;
            double ratio;
            if (elapsedTime <= 1000)
            {
                startX = zeroSecond.X;
                endX = oneSecond.X;
                startY = zeroSecond.Y;
                endY = oneSecond.Y;
                startZ = zeroSecond.Z;
                endZ = oneSecond.Z;
                ratio = elapsedTime / 1000.0;
            }
            else if (elapsedTime > 1000 && elapsedTime <= 2000)
            {
                startX = oneSecond.X;
                endX = twoSeconds.X;
                startY = oneSecond.Y;
                endY = twoSeconds.Y;
                startZ = oneSecond.Z;
                endZ = twoSeconds.Z;
                ratio = (elapsedTime - 1000) / 1000.0;
            }
            else if (elapsedTime > 2000 && elapsedTime <= 3000)
            {
                startX = twoSeconds.X;
                endX = threeSeconds.X;
                startY = twoSeconds.Y;
                endY = threeSeconds.Y;
                startZ = twoSeconds.Z;
                endZ = threeSeconds.Z;
                ratio = (elapsedTime - 2000) / 1000.0;
            }
            else
            {
                startX = threeSeconds.X;
                endX = fourSeconds.X;
                startY = threeSeconds.Y;
                endY = fourSeconds.Y;
                startZ = threeSeconds.Z;
                endZ = fourSeconds.Z;
                ratio = (elapsedTime - 3000) / 1000.0;
            }

            
            axisRotation.Axis = new Vector3D(startX + (endX - startX) * ratio,
                                             startY + (endY - startY) * ratio,
                                             startZ + (endZ - startZ) * ratio);
        }
    }
}
