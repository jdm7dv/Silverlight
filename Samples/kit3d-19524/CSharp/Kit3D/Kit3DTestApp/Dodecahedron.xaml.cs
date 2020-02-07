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
    public partial class Dodecahedron : UserControl
    {
        public Dodecahedron()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SilverlightControl1_Loaded);
        }

        void viewport_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Matrix3D viewMatrix = ViewMatrix(new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), new Point3D(0, 0, 10));
            Matrix3D projectionMatrix = ProjectionMatrix(this.viewport.ActualWidth / this.viewport.ActualHeight, 45, double.PositiveInfinity, 0.125);

            Matrix3D translate = new Matrix3D(1, 0, 0, 0,
                                              0, 1, 0, 0,
                                              0, 0, 1, 0,
                                              -0.5, 0, 0, 1);
            projectionMatrix.Append(translate);

            MatrixCamera camera = new MatrixCamera(viewMatrix, projectionMatrix);
            viewport.Camera = camera;
        }

        private AxisAngleRotation3D axisRotation;
        private Viewport3D viewport;
        private DateTime startAnimation = DateTime.Now;

        void SilverlightControl1_Loaded(object sender, RoutedEventArgs e)
        {
            string positions = "1.171 -0.724 0,1 -1 -1,1.618 0 -0.618,1.618 0 0.618,1 -1 1,0.618 -1.618 0,-1.171 -0.724 0,-1 -1 1,-1.618 0 0.618,-1.618 0 -0.618,-1 -1 -1,-0.618 -1.618 0,-1.171 0.724 0,-1 1 -1,-1.618 0 -0.618,-1.618 0 0.618,-1 1 1,-0.618 1.618 0,1.171 0.724 0,1 1 1,1.618 0 0.618,1.618 0 -0.618,1 1 -1,0.618 1.618 0,-0.724 0 -1.171,-1 1 -1,0 0.618 -1.618,0 -0.618 -1.618,-1 -1 -1,-1.618 0 -0.618,-0.724 0 1.171,-1 -1 1,0 -0.618 1.618,0 0.618 1.618,-1 1 1,-1.618 0 0.618,0.724 0 -1.171,1 -1 -1,0 -0.618 -1.618,0 0.618 -1.618,1 1 -1,1.618 0 -0.618,0.724 0 1.171,1 1 1,0 0.618 1.618,0 -0.618 1.618,1 -1 1,1.618 0 0.618,0 -1.171 -0.724,1 -1 -1,0.618 -1.618 0,-0.618 -1.618 0,-1 -1 -1,0 -0.618 -1.618,0 1.171 -0.724,-1 1 -1,-0.618 1.618 0,0.618 1.618 0,1 1 -1,0 0.618 -1.618,0 -1.171 0.724,-1 -1 1,-0.618 -1.618 0,0.618 -1.618 0,1 -1 1,0 -0.618 1.618,0 1.171 0.724,1 1 1,0.618 1.618 0,-0.618 1.618 0,-1 1 1,0 0.618 1.618";
            string triangleIndices = "0 1 2,0 2 3,0 3 4,0 4 5,0 5 1,6 7 8,6 8 9,6 9 10,6 10 11,6 11 7,12 13 14,12 14 15,12 15 16,12 16 17,12 17 13,18 19 20,18 20 21,18 21 22,18 22 23,18 23 19,24 25 26,24 26 27,24 27 28,24 28 29,24 29 25,30 31 32,30 32 33,30 33 34,30 34 35,30 35 31,36 37 38,36 38 39,36 39 40,36 40 41,36 41 37,42 43 44,42 44 45,42 45 46,42 46 47,42 47 43,48 49 50,48 50 51,48 51 52,48 52 53,48 53 49,54 55 56,54 56 57,54 57 58,54 58 59,54 59 55,60 61 62,60 62 63,60 63 64,60 64 65,60 65 61,66 67 68,66 68 69,66 69 70,66 70 71,66 71 67";

            Model3DGroup modelGroup = new Model3DGroup();

            Random r = new Random();
            string[] positionArray = positions.Split(',');
            string[] triangleArray = triangleIndices.Split(',');
            for(int i=0; i<triangleArray.Length; i+=5)
            {
                Int32Collection meshTriangles = new Int32Collection();
                string triangle = triangleArray[i];
                string[] val = triangle.Split(' ');
                meshTriangles.Add(int.Parse(val[0]));
                meshTriangles.Add(int.Parse(val[1]));
                meshTriangles.Add(int.Parse(val[2]));
                triangle = triangleArray[i+1];
                val = triangle.Split(' ');
                meshTriangles.Add(int.Parse(val[0]));
                meshTriangles.Add(int.Parse(val[1]));
                meshTriangles.Add(int.Parse(val[2]));
                triangle = triangleArray[i + 2];
                val = triangle.Split(' ');
                meshTriangles.Add(int.Parse(val[0]));
                meshTriangles.Add(int.Parse(val[1]));
                meshTriangles.Add(int.Parse(val[2]));
                triangle = triangleArray[i + 3];
                val = triangle.Split(' ');
                meshTriangles.Add(int.Parse(val[0]));
                meshTriangles.Add(int.Parse(val[1]));
                meshTriangles.Add(int.Parse(val[2]));
                triangle = triangleArray[i + 4];
                val = triangle.Split(' ');
                meshTriangles.Add(int.Parse(val[0]));
                meshTriangles.Add(int.Parse(val[1]));
                meshTriangles.Add(int.Parse(val[2]));

                Int32Collection finalMeshTriangles = new Int32Collection();
                Point3DCollection meshPositions = new Point3DCollection();
                int count = 0;
                foreach (int ti in meshTriangles)
                {
                    string position = positionArray[ti];
                    string[] pos = position.Split(' ');
                    meshPositions.Add(new Point3D(double.Parse(pos[0]),
                                                  double.Parse(pos[1]),
                                                  double.Parse(pos[2])));

                    finalMeshTriangles.Add(count++);
                }

                MeshGeometry3D mesh = new MeshGeometry3D();
                mesh.Positions = meshPositions;
                mesh.TriangleIndices = finalMeshTriangles;

                GeometryModel3D model = new GeometryModel3D();
                model.Geometry = mesh;
                model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Color.FromArgb(128, (byte)r.Next(0, 256), (byte)r.Next(0, 256), (byte)r.Next(0, 256)))));
                modelGroup.Children.Add(model);
            }

            ModelVisual3D modelVisual = new ModelVisual3D();
            modelVisual.Content = modelGroup;

            RotateTransform3D rotation = new RotateTransform3D();
            axisRotation = new AxisAngleRotation3D();
            axisRotation.Angle = 60;
            rotation.Rotation = axisRotation;
            modelVisual.Transform = rotation;

            viewport = new Viewport3D();
            this.viewport.SizeChanged += new SizeChangedEventHandler(viewport_SizeChanged);
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;
            //viewport.Camera = new PerspectiveCamera(new Point3D(0, 0, 10),
            //                                        new Vector3D(0, 0, -5),
            //                                        new Vector3D(0, 1, 0),
            //                                        45);

            viewport.Children.Add(modelVisual);

            this.LayoutRoot.Children.Add(viewport);

            startAnimation = DateTime.Now;

            Kit3D.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        // <summary>
        /// Calculates the matrix that transforms world coords to view coords
        /// </summary>
        /// <param name="lookDirection"></param>
        /// <param name="upDirection"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private Matrix3D ViewMatrix(Vector3D lookDirection, Vector3D upDirection, Point3D position)
        {
            Vector3D cameraZAxis = -lookDirection;
            cameraZAxis.Normalize();

            Vector3D cameraXAxis = Vector3D.CrossProduct(upDirection, cameraZAxis);
            cameraXAxis.Normalize();

            Vector3D cameraYAxis = Vector3D.CrossProduct(cameraZAxis, cameraXAxis);

            Vector3D cameraPosition = (Vector3D)position;
            double offsetX = -Vector3D.DotProduct(cameraXAxis, cameraPosition);
            double offsetY = -Vector3D.DotProduct(cameraYAxis, cameraPosition);
            double offsetZ = -Vector3D.DotProduct(cameraZAxis, cameraPosition);

            return new Matrix3D(cameraXAxis.X, cameraYAxis.X, cameraZAxis.X, 0,
                                cameraXAxis.Y, cameraYAxis.Y, cameraZAxis.Y, 0,
                                cameraXAxis.Z, cameraYAxis.Z, cameraZAxis.Z, 0,
                                offsetX, offsetY, offsetZ, 1);
        }

        /// <summary>
        /// Calculates the matrix that transforms view coords in 3D to 2D points on the
        /// projection plane
        /// </summary>
        /// <param name="nearPlaneDistance"></param>
        /// <param name="farPlaneDistance"></param>
        /// <param name="fieldOfView"></param>
        /// <returns></returns>
        internal Matrix3D ProjectionMatrix(double viewportAspectRatio, double fieldOfView, double farPlaneDistance, double nearPlaneDistance)
        {
            //Now we can create the projection matrix
            double aspectRatio = viewportAspectRatio;
            double xScale = 1.0 / System.Math.Tan(System.Math.PI * fieldOfView / 360);
            double yScale = aspectRatio * xScale;
            double zScale = (farPlaneDistance == double.PositiveInfinity) ? -1 : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            double zOffset = nearPlaneDistance * zScale;

            return new Matrix3D(xScale, 0, 0, 0,
                                0, yScale, 0, 0,
                                0, 0, zScale, -1,
                                0, 0, zOffset, 0);
        }

        Vector3D zeroSecond = new Vector3D(-1, 0, 0);
        Vector3D oneSecond = new Vector3D(0, -1, 0);
        Vector3D twoSeconds = new Vector3D(1, 0, 0);
        Vector3D threeSeconds = new Vector3D(0, 1, 0);
        Vector3D fourSeconds = new Vector3D(-1, 0, 0);

        int elapsedTime = 0;
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (this.viewport.Camera != null)
            {
                elapsedTime = (elapsedTime + 20) % 4000;
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
}
