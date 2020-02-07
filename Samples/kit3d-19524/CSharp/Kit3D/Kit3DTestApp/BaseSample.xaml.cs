using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Kit3D.Windows.Controls;
using Kit3D.Windows.Media.Media3D;
using Kit3D.Windows.Media;

namespace TestApp
{
    public partial class BaseSample : UserControl
    {
        public BaseSample()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(BaseSample_Loaded);
        }

        private Viewport3D viewport;
        private ModelVisual3D modelVisual;

        private void BaseSample_Loaded(object sender, RoutedEventArgs e)
        {
            // Create Viewport3D as content of window.
            viewport = new Viewport3D();
            viewport.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewport.VerticalAlignment = VerticalAlignment.Stretch;
            viewport.SizeChanged += new SizeChangedEventHandler(viewport_SizeChanged);

            //Contains all of the positions and triangle indices for the model, Right hand coord system
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                new Point3D(-5, 5, 5),
                new Point3D(5, 5, 5),
                new Point3D(5, -5, 5),
                new Point3D(-5, -5, 5)
            };

            //Specifies how triangles relate to index of positions collection
            mesh.TriangleIndices = new Int32Collection
            {
                3,1,0,
                3,2,1
            };

            //The model stores the mesh and the material used to paint the model
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = mesh;

            //Create a brush to paint the surface of the model
            model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(Colors.Blue)));

            //This pushes the polygons closer to eachother so the seams between
            //the polygons which get antialiased are not as visible
            model.SeamSmoothing = 1;

            // The ModelVisual3D class knows how to render the model in the viewport
            modelVisual = new ModelVisual3D();
            modelVisual.Content = model;
            viewport.Children.Add(modelVisual);

            //Add the Viewport3D to the Silverlight visual tree so that it is rendered
            this.LayoutRoot.Children.Add(viewport);

            //This event will be fired just before the frame is rendered
            Kit3D.Windows.Media.CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);

            //Capture mouse down event
            this.MouseLeftButtonDown += new MouseButtonEventHandler(BaseSample_MouseLeftButtonDown);
        }

        int i = 0;
        /// <summary>
        /// Called just before each frame is rendered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //Create a rotation matrix to rotate the model
            double angle = -45 + (i % 90);

            QuaternionRotation3D rotation = new QuaternionRotation3D(new Quaternion(new Vector3D(0, 1, 0), angle));
            RotateTransform3D rotateTransform = new RotateTransform3D(rotation);

            //Apply the transform the the model visual
            this.modelVisual.Transform = rotateTransform;

            //Update angle offset
            i++;
        }

        #region HitTesting

        void BaseSample_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DebugText.Text = "No model hit";

            Point viewportMousePosition = e.GetPosition(this.viewport);

            //See if the user clicked on any object in the 3D scene
            Kit3D.Windows.Media.VisualTreeHelper.HitTest(this.viewport,
                                                         null,
                                                         HitTestDown,
                                                         new PointHitTestParameters(viewportMousePosition));
        }

        /// <summary>
        /// Called when there is a hit from the mouse and a model
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        HitTestResultBehavior HitTestDown(HitTestResult result)
        {
            RayMeshGeometry3DHitTestResult meshResult = result as RayMeshGeometry3DHitTestResult;

            //If the torus was clicked, keep track of the transform and the fact
            //that the torus was under the mouse pointer
            if (meshResult.VisualHit == this.modelVisual)
            {
                this.DebugText.Text = "Model HIT!!";
            }

            //Stop hit testing from looking for other models;
            return HitTestResultBehavior.Stop;
        }

        #endregion

        #region View / Projection matrices


        /// <summary>
        /// Called when the viewport change ssize, if this happens we need to recompute the view/projection matrices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewport_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Matrix3D viewMatrix = ViewMatrix(new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), new Point3D(-2, 2, 35));
            Matrix3D projectionMatrix = ProjectionMatrix(this.viewport.ActualWidth / this.viewport.ActualHeight, 45, double.PositiveInfinity, 0.125);

            //Update camera
            MatrixCamera camera = new MatrixCamera(viewMatrix, projectionMatrix);
            viewport.Camera = camera;
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

        #endregion
    }
}
