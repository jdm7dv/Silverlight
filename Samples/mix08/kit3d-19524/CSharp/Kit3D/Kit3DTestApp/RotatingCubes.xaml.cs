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
    /// This sample was based on an excellent set of WPF tutorials at
    /// http://blogs.inetium.com/blogs/mhodnick/archive/2006/03/30/34.aspx
    /// </summary>
    public partial class RotatingCubes : UserControl
    {
        public RotatingCubes()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SilverlightControl1_Loaded);
        }

        private MeshGeometry3D CreateCubeMesh()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                new Point3D(0,0,0),
                new Point3D(1,0,0),
                new Point3D(1,0,1),
                new Point3D(0,0,0),
                new Point3D(1,0,1),
                new Point3D(0,0,1),
                new Point3D(0,1,0),
                new Point3D(1,1,1),
                new Point3D(1,1,0),
                new Point3D(0,1,0),
                new Point3D(0,1,1),
                new Point3D(1,1,1),
                new Point3D(0,0,0),
                new Point3D(0,1,0),
                new Point3D(1,1,0),
                new Point3D(0,0,0),
                new Point3D(1,1,0),
                new Point3D(1,0,0),
                new Point3D(1,0,0),
                new Point3D(1,1,0),
                new Point3D(1,1,1),
                new Point3D(1,0,0),
                new Point3D(1,1,1),
                new Point3D(1,0,1),
                new Point3D(1,0,1),
                new Point3D(1,1,1),
                new Point3D(0,1,1),
                new Point3D(1,0,1),
                new Point3D(0,1,1),
                new Point3D(0,0,1),
                new Point3D(0,0,1),
                new Point3D(0,1,1),
                new Point3D(0,1,0),
                new Point3D(0,0,1),
                new Point3D(0,1,0),
                new Point3D(0,0,0)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                0,1,2,3,4,5,
                6,7,8,9,10,11,
                12,13,14,15,16,17,
                18,19,20,21,22,23,
                24,25,26,27,28,29,
                30,31,32,33,34,35
            };

            return mesh;
        }

        private RotateTransform3D groupRotationX, groupRotationY;
        private Model3DGroup cubeGroup;

        private ModelVisual3D LoadCubes(int height, int width, int depth, double step)
        {
            cubeGroup = new Model3DGroup();
            double currentX = 0;
            double currentY = 0;
            double currentZ = 0;

            for (int h = 0; h < height; h++)
            {
                currentY += step;
                currentX = 0;

                for (int w = 0; w < width; w++)
                {
                    currentX += step;
                    currentZ = 0;

                    for (int d = 0; d < depth; d++)
                    {
                        currentZ += step;

                        Model3D cube = GetCubeModel();
                        cube.Transform = GetCubeTransforms(new Vector3D(currentX, currentY, currentZ));
                        cubeGroup.Children.Add(cube);
                    }
                }
            }

            AxisAngleRotation3D rotationY = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 360);
            RotateTransform3D groupSpinY = new RotateTransform3D(rotationY);
            AxisAngleRotation3D rotationX = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 360);
            RotateTransform3D groupSpinX = new RotateTransform3D(rotationX);

            
            //TODO: Check WPF see how the bounds property works
            Rect3D groupRect = cubeGroup.Bounds;
            groupSpinX.CenterX = groupRect.X + groupRect.SizeX / 2;
            groupSpinX.CenterY = groupRect.Y + groupRect.SizeY / 2;
            groupSpinX.CenterZ = groupRect.Z + groupRect.SizeZ / 2;
            groupSpinY.CenterX = groupRect.X + groupRect.SizeX / 2;
            groupSpinY.CenterY = groupRect.Y + groupRect.SizeY / 2;
            groupSpinY.CenterZ = groupRect.Z + groupRect.SizeZ / 2;
           
            /*
            groupSpinX.CenterX = width / 2;
            groupSpinX.CenterY = 0;
            groupSpinX.CenterZ = width / 2;
            groupSpinY.CenterX = 0;
            groupSpinY.CenterY = height / 2;
            groupSpinY.CenterZ = height / 2;
            */

            //Test out a transform group
            Transform3DGroup allTransforms = new Transform3DGroup();
            allTransforms.Children.Add(groupSpinY);
            allTransforms.Children.Add(groupSpinX);

            ModelVisual3D visual = new ModelVisual3D();
            visual.Content = cubeGroup;
            visual.Transform = allTransforms;

            groupRotationX = groupSpinX;
            groupRotationY = groupSpinY;
            return visual;
        }

        private Transform3DGroup GetCubeTransforms(Vector3D finalTranslateVector)
        {
            Transform3DGroup transforms = new Transform3DGroup();
            TranslateTransform3D shrink = new TranslateTransform3D(-.5, 0, -.5);
            ScaleTransform3D scale = new ScaleTransform3D(new Vector3D(.5, .5, .5));
            TranslateTransform3D move = new TranslateTransform3D(finalTranslateVector);

            transforms.Children.Add(shrink);
            transforms.Children.Add(scale);
            transforms.Children.Add(move);
            return transforms;

        }

        private Model3D GetCubeModel()
        {
            GeometryModel3D geometry = new GeometryModel3D();
            geometry.Geometry = CreateCubeMesh();
            geometry.SeamSmoothing = 0.8;

            Material colorMaterial = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(GetRandomColor())));
            geometry.Material = colorMaterial;
            return geometry;
        }

        private Random random = new Random();
        private Color GetRandomColor()
        {
            int next = random.Next(0, 100);

            if (next > 90)
            {
                return Colors.Red;
            }
            if (next > 80)
            {
                return Colors.Blue;
            }
            if (next > 70)
            {
                return Colors.Green;
            }
            if (next > 60)
            {
                return Colors.Brown;
            }
            if (next > 50)
            {
                return Colors.Purple;
            }
            if (next > 40)
            {
                return Colors.Yellow;
            }
            if (next > 30)
            {
                return Colors.Gray;
            }
            if (next > 20)
            {
                return Colors.Orange;
            }
            if (next > 10)
            {
                return Colors.Magenta;
            }

            return Colors.Orange;

        }

        private Viewport3D viewport;
        private ModelVisual3D modelVisual;
        void SilverlightControl1_Loaded(object sender, RoutedEventArgs e)
        {
            viewport = new Viewport3D();
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;
            viewport.Camera = new PerspectiveCamera(new Point3D(4, 4, 25),
                                                    new Vector3D(0, 0, -1),
                                                    new Vector3D(0, 1, 0),
                                                    45);

            modelVisual = LoadCubes(4, 4, 4, 1.6);
            viewport.Children.Add(modelVisual);

            viewport.ShowModelBoundingBoxes = true;

            this.LayoutRoot.Children.Add(viewport);

            Kit3D.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            ((AxisAngleRotation3D)groupRotationX.Rotation).Angle += 5;
            ((AxisAngleRotation3D)groupRotationY.Rotation).Angle += 3;
        }

    }
}
