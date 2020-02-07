namespace Kit3D.Windows.Controls
{
    using System;
    using System.Windows.Threading;
    using Kit3D.Windows.Media.Media3D;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Kit3D.Util;
    using Kit3D.Windows.Media;
    using Kit3D.Math;
    using System.Windows.Controls;
    using System.Windows;

    public sealed class Viewport3D : Visual
    {
        private Camera camera;
        private Visual3DCollection children;
        private Canvas renderTarget;
        private List<Triangle> trianglesToRender = new List<Triangle>();
        private UIElementCollection currentOrderedTriangles;
        private List<ModelVisual3D> renderedVisuals = new List<ModelVisual3D>();

        public Viewport3D()
        {
            this.Children = new Visual3DCollection();
            this.Children.Viewport = this;

            //This canvas contains the canvas where all the normal 3D items are contained and
            //also the canvas that contains the DeepZoom controls
            Canvas mainCanvas = new Canvas();

            //This is the location where all of the polygons will be drawn
            //it is just a canvas that fills the full size of the viewport area
            this.renderTarget = new Canvas();

            //TODO:Uncomment?
            //this.renderTarget.Background = new SolidColorBrush(Colors.Transparent);
            this.renderTarget.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.renderTarget.VerticalAlignment = VerticalAlignment.Stretch;
            this.currentOrderedTriangles = this.renderTarget.Children;

            mainCanvas.Children.Add(this.renderTarget);
            this.Content = mainCanvas;

            //Want to know when the viewport has been resized so that we can
            //re-render the content
            this.SizeChanged += new SizeChangedEventHandler(Viewport3D_SizeChanged);

            //Hook up to receive rendering events, this event is raised after
            //CompositionTarget.Rendering, so at this point users of Kit3D have
            //had chance to update their custom animation
            Kit3D.Windows.Media.CompositionTarget.RenderingComplete += new EventHandler(CompositionTarget_RenderingComplete);
        }

        private void CompositionTarget_RenderingComplete(object sender, EventArgs e)
        {
            Render();
        }

        void Viewport3D_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.IsViewportDirty = true;
            this.VPWidth = this.ActualWidth;
            this.VPHeight = this.ActualHeight;

            //Indicate to the camera it may have to refresh itself, since the
            //viewport size has changed
            if (this.Camera != null)
            {
                this.Camera.Invalidate();
            }
        }

        public Camera Camera
        {
            get { return this.camera; }
            set 
            {
                if (this.camera != null)
                {
                    this.camera.Changed -= new EventHandler(camera_Changed);
                }
                this.camera = value;
                if (this.camera != null)
                {
                    this.camera.Changed += new EventHandler(camera_Changed);
                }
                this.camera.Viewport = this;
                this.IsViewportDirty = true;
            }
        }

        void camera_Changed(object sender, EventArgs e)
        {
            //If any of the properties of the camera change then we need to
            //re-render the scene
            this.IsViewportDirty = true;
        }

        public Visual3DCollection Children
        {
            get { return this.children; }
            set
            {
                if (this.children != null)
                {
                    this.children.Changed -= new EventHandler(children_Changed);
                }
                this.children = value;
                if (this.children != null)
                {
                    this.children.Changed += new EventHandler(children_Changed);
                }
                this.IsContentDirty = true;
            }
        }

        void children_Changed(object sender, EventArgs e)
        {
            //Need to re-render this view, something changed in the visuals
            this.IsContentDirty = true;
        }

        //TODO: Find better method name or mechanism for rendering?
        private void Render()
        {
            //If the contents of the viewport has not changed or the viewport
            //is not currently visible then there is no point rendering the 
            //contents of the viewport
            if ((this.VPHeight != 0) && 
                (this.VPWidth != 0) &&
                (this.IsContentDirty || this.IsViewportDirty) &&
                (this.Visibility == Visibility.Visible))
            {
                //Keep a list of all of the triangles we need to render
                //once we have determined which triangles are visible.
                trianglesToRender.Clear();

                //List of dirty models.  The reason why we need this list is because
                //the same model can be used by more than one visual, so we only want
                //to set it's dirty flag to clean after all the rendering has completed
                this.renderedVisuals.Clear();

                //This will make triangles that are no longer visible collapsed
                foreach (Visual3D visual in this.Children)
                {
                    visual.Render(trianglesToRender, renderedVisuals);
                }

                //Now reorder all of the face triangles
                trianglesToRender.Sort(delegate(Triangle x, Triangle y)
                {
                    if (x.ZIndexOverride != y.ZIndexOverride)
                    {
                        return x.ZIndexOverride.CompareTo(y.ZIndexOverride);
                    }
                    else
                    {
                        return y.ZIndex.CompareTo(x.ZIndex);
                    }
                });

                //Put the triangles on the canvas in the correct zorder, it is very
                //slow to use the ZIndex property of the canvas, so just reordering
                //the items in the DOM is a lot quicker
                this.currentOrderedTriangles.Clear();
                foreach (Triangle triangle in this.trianglesToRender)
                {
                    //if (!this.currentOrderedTriangles.Contains(triangle.Visual))
                    {
                        this.currentOrderedTriangles.Add(triangle.Visual);
                    }
                }

                //Indicate that we don't need to redraw the scene until it
                //is changed again
                this.IsViewportDirty = false;
                this.IsContentDirty = false;

                //Reset all of the models dirty flag
                foreach (ModelVisual3D visual in renderedVisuals)
                {
                    foreach (Model3D model in visual.RenderedModels)
                    {
                        model.IsDirty = false;
                    }
                }
            }
        }

        /// <summary>
        /// Indicates that the content the viewport is displaying is now
        /// dirty and needs to be redrawn
        /// </summary>
        private bool isContentDirty = false;
        internal bool IsContentDirty
        {
            get { return this.isContentDirty; }
            set { this.isContentDirty = value; }
        }

        /// <summary>
        /// Indicates that the viewport is dirty, i.e. the camera with which it is 
        /// associated is dirty, so we ned to redraw all of the models, even it they
        /// are not dirty internally.
        /// </summary>
        private bool isViewportDirty = false;
        internal bool IsViewportDirty
        {
            get { return this.isViewportDirty; }
            set { this.isViewportDirty = value; }
        }

        /// <summary>
        /// If set to true all of the individual models will be shown with
        /// their axis aligned bounding box
        /// </summary>
        public bool ShowModelBoundingBoxes
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the matrix which converts NDC coords to viewport screen
        /// coords.  Point 0,0 is the top left of the viewport and vw,vh is
        /// the bottom right of the viewport.  The NDC coord has 0,0 at the
        /// center of the viewport, -1,1 at the top left of the viewport and
        /// 1,-1 at the bottom right of the viewport.  
        /// </summary>
        internal Matrix3D NDCToScreenTransform
        {
            get
            {
                //TODO: Cache

                //Create the NDC to screen matrix, know NDC is kept at 2 units high
                // y' = - (hs / 2) * Yndc + hs/2 + sx
                // x' = (ws / 2) * Xndc + ws/2 + sy
                // z' = (ds / 2) * Zndc + ds/2   //ds is [0,1] where our ndc z was from -1 to 1
                double screenDepth = 1;
                Matrix3D ndcToScreen = new Matrix3D(this.VPWidth / 2, 0, 0, 0,
                                                    0, -this.VPHeight / 2, 0, 0,
                                                    0, 0, screenDepth / 2, 0,
                                                    this.VPWidth / 2, this.VPHeight / 2, screenDepth / 2, 1);

                return ndcToScreen;
            }
        }

        /// <summary>
        /// Transform to turn screen points to points in the view plane, with a z value of -d
        /// where d is the distance from the camera to the view plane.
        /// //TODO: Need to rethink this - people not always using the perspective projection matrix
        /// </summary>
        public Matrix3D ScreenToViewTransform
        {
            get
            {
                double fieldOfViewInDegrees = 0;
                PerspectiveCamera camera = this.Camera as PerspectiveCamera;
                if (camera == null)
                {
                    MatrixCamera matrixCamera = this.Camera as MatrixCamera;
                    if (matrixCamera == null)
                    {
                        throw new NotSupportedException("Unsupported camera type");
                    }

                    fieldOfViewInDegrees = matrixCamera.FieldOfView;
                }
                else
                {
                    fieldOfViewInDegrees = camera.FieldOfView;
                }

                double width = this.VPWidth;
                double height = this.VPHeight;
                double aspectRatio = this.AspectRatio;
                double depth = 1.0 / System.Math.Tan(System.Math.PI * fieldOfViewInDegrees / 360.0);

                return new Matrix3D(2 / width, 0, 0, 0,
                                    0, -2 / (height * aspectRatio), 0, 0,
                                    -1, 1 / aspectRatio, -depth, 0,
                                    0, 0, 0, 1);
            }
        }

        /// <summary>
        /// Returns the ratio of the width to the height of the viewport on the screen
        /// </summary>
        internal double AspectRatio
        {
            get
            {
                return this.VPWidth / this.VPHeight;
            }
        }

        public double VPWidth
        {
            get;
            set;
        }

        public double VPHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Calculates all of the visuals that intersect with the ray that extends from the
        /// point that was clicked on the viewport
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal List<HitResult> HitTest3D(PointHitTestParameters hitPoint)
        {
            List<HitResult> results = new List<HitResult>();

            //HitPoint refers the to 2D screen co-ords that was clicked by the user on the
            //Viewport3D control

            //Need to transform the 2D screen co-ord into 3D co-ords and then create the
            //ray that extends from this point.  Once we have the ray we can see what 
            //objects in the scene it intersects with

            //TODO: This should be in the perspective camera

            if (this.Camera != null)
            {
                Point3D cameraPosition = new Point3D();

                //TODO:Cache this value
                Matrix3D viewToWorld = Matrix3D.Identity;
                if (this.Camera is MatrixCamera)
                {
                    MatrixCamera camera = this.Camera as MatrixCamera;
                    cameraPosition = camera.Position;
                    viewToWorld = camera.InverseViewMatrix;
                }
                else if (this.Camera is PerspectiveCamera)
                {
                    PerspectiveCamera camera = this.Camera as PerspectiveCamera;

                    cameraPosition = camera.Position;
                    viewToWorld = camera.InverseViewMatrix;
                }
                else
                {
                    throw new Exception("Camera type not supported, cannot perform hit test");
                }


                //First convert the screen co-ordinates to view space co-ordinates.  The new 
                //point in view space will lie on the projection plane
                Matrix3D screenToViewTransform = this.ScreenToViewTransform;

                //First step is to change the screen point into view space
                Point3D viewSpacePoint = screenToViewTransform.Transform(new Point3D(hitPoint.HitPoint.X,
                                                                                     hitPoint.HitPoint.Y,
                                                                                     1));

                Point3D worldSpacePoint = viewToWorld.Transform(viewSpacePoint);

                //Now we have our ray, see what items it intersects with from the item
                //we have currently rendered to the screen
                foreach (ModelVisual3D visual in this.renderedVisuals)
                {
                    foreach(Model3D model in visual.RenderedModels)
                    {
                        //Get transform to go from world space to local space
                        //for the model for this paricular visual
                        Matrix3D worldToLocal = model.GetLocalToWorldTransform(visual);
                        worldToLocal.Invert();

                        //This is a point in local space which we can use the create the ray
                        Point3D localPoint = worldToLocal.Transform(worldSpacePoint);

                        //TODO: Should not be casting this here - what about MatrixCamera
                        Point3D eyePointInLocal = cameraPosition;
                        eyePointInLocal = worldToLocal.Transform(eyePointInLocal);

                        Ray ray = new Ray(eyePointInLocal, localPoint - eyePointInLocal);

                        //For each model we now will see if the ray intersects any of the
                        //triangles that are part of the model.
                        model.IntersectsWith(visual, ray, results);
                    }
                }
            }

            //Make sure closest hit is first in the list and sorted ascending
            results.Sort((Comparison<HitResult>)((x, y) => x.DistanceToRayOrigin.CompareTo(y.DistanceToRayOrigin)));
            return results;
        }
    }
}
