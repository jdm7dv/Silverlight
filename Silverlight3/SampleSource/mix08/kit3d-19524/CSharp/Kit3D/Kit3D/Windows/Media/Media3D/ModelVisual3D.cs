namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Kit3D.Util;
    using Kit3D.Math;
    using System.Windows;

    //TODO: Triangle degenerative cases, need to catch these exceptions and 
    //      handle them gracefully;

    public sealed class ModelVisual3D : Visual3D
    {
        /// <summary>
        /// The content that the visual is displaying
        /// </summary>
        private Model3D content;

        /// <summary>
        /// The local transform to apply to this visual
        /// </summary>
        private Transform3D transform;

        /// <summary>
        /// A list of all of the triangles that make up all of the 
        /// models that are part of this visual.
        /// </summary>
        private Dictionary<Model3D, List<Triangle>> allModelTriangles = new Dictionary<Model3D, List<Triangle>>();

        //TODO:Remove when ImageBrush bug fixed
        private double opacity = 1;

        private TriangleDirtyState triangleAreDirty = TriangleDirtyState.DirtyRectreateTriangles;

        /// <summary>
        /// Used to indicate if the visual is currently dirty and needs to be re-rendered
        /// </summary>
        private bool isDirty = true;

        public ModelVisual3D()
        {
            this.Transform = new MatrixTransform3D();

            //Indicate that we need to generate triangles
            this.TrianglesAreDirty = TriangleDirtyState.DirtyRectreateTriangles;

            //A list of the models that were last rendered
            this.RenderedModels = new List<Model3D>();
        }

        public Model3D Content
        {
            get { return this.content; }
            set
            {
                if (this.content != null)
                {
                    this.content.TransformChanged -= new Model3D.TransformChangedEventHandler(content_TransformChanged);
                    this.content.GeometryChanged -= new Model3D.GeometryChangedEventHandler(content_GeometryChanged);
                    this.content.MaterialChanged -= new Model3D.MaterialChangedEventHandler(content_MaterialChanged);
                }
                this.content = value;
                if (this.content != null)
                {
                    this.content.TransformChanged += new Model3D.TransformChangedEventHandler(content_TransformChanged);
                    this.content.GeometryChanged += new Model3D.GeometryChangedEventHandler(content_GeometryChanged);
                    this.content.MaterialChanged += new Model3D.MaterialChangedEventHandler(content_MaterialChanged);

                    //Need to update all of the world transforms for the models
                    this.content.SetParentWorldTransform(this, this.Transform.Value);
                }

                this.IsDirty = true;
            }
        }

        private void content_MaterialChanged()
        {
            //Make this dirty so all the triangles are reset again
            this.TrianglesAreDirty = TriangleDirtyState.DirtyRectreateTriangles;
        }

        private void content_GeometryChanged(Geometry3D.GeometryChangeType changeType)
        {
            //Depending on the kind of change we might need to recreate the triangles again
            switch (changeType)
            {
                //In the case where the triangle indices have changed i.e. the user change the
                //number of triangles or in the case when the user changed the associated geometry
                //then we need to flag the triangles as dirty so they get regenerated
                case Geometry3D.GeometryChangeType.GeometrySet:
                case Geometry3D.GeometryChangeType.TriangleIndices:
                    this.TrianglesAreDirty = TriangleDirtyState.DirtyTriangleIndicesChanged;
                    break;

                case Geometry3D.GeometryChangeType.Positions:
                case Geometry3D.GeometryChangeType.Texturecoordinates:
                    this.IsDirty = true;
                    break;

                default:
                    throw new Exception("Unknown geometrychange type");
            }
        }

        private void content_TransformChanged()
        {
            this.IsDirty = true;
        }

        /// <summary>
        /// Called when the viewport which the model visual is associated
        /// with changes.
        /// </summary>
        protected internal override void  OnViewportChanged()
        {
            //TODO: Verify this code path
            this.IsDirty = true;
        }

        private void CreateTriangles()
        {
            //When materials are updated need to update all of the associated triangles
            if ((this.content == null) || (this.Viewport == null))
            {
                return;
            }

            if (this.TrianglesAreDirty != TriangleDirtyState.NotDirty)
            {
                this.allModelTriangles.Clear();
                this.content.CreateTriangles(this.allModelTriangles, (this.TrianglesAreDirty == TriangleDirtyState.DirtyRectreateTriangles));

                this.Opacity = this.opacity;
                this.IsDirty = true;
                this.TrianglesAreDirty = TriangleDirtyState.NotDirty;
            }
        }

        /// <summary>
        /// Indicates we need to regenerate the triangles associated with this visual
        /// </summary>
        private TriangleDirtyState TrianglesAreDirty
        {
            get { return this.triangleAreDirty; }
            set
            {
                //If triangles have been marked as needing recreation then don't allow
                //to override with other values
                if ((this.triangleAreDirty == TriangleDirtyState.DirtyRectreateTriangles) &&
                    (value != TriangleDirtyState.NotDirty))
                {
                    return;
                }

                this.triangleAreDirty = value;

                if (this.triangleAreDirty != TriangleDirtyState.NotDirty)
                {
                    this.IsDirty = true;
                }
            }
        }

        private enum TriangleDirtyState
        {
            NotDirty,
            DirtyRectreateTriangles,
            DirtyTriangleIndicesChanged
        }

        public Transform3D Transform
        {
            get { return this.transform; }
            set
            {
                if (this.transform != null)
                {
                    this.transform.Changed -= new EventHandler(transform_Changed);
                }
                this.transform = value;
                if (this.transform != null)
                {
                    this.transform.Changed += new EventHandler(transform_Changed);
                }

                transform_Changed(null, null);
            }
        }

        private void transform_Changed(object sender, EventArgs e)
        {
            //Need to make sure we update the content, since this transform
            //affects them as well
            if (this.Content != null)
            {
                this.Content.SetParentWorldTransform(this, this.Transform.Value);
            }

            this.IsDirty = true;
        }

        public double Opacity
        {
            get
            {
                return this.opacity;
            }
            set
            {
                //if (this.opacity != value)
                {
                    this.opacity = value;

                    foreach (Model3D model in this.allModelTriangles.Keys)
                    {
                        //bool opacityChanged = false;
                        foreach (Triangle triangle in this.allModelTriangles[model])
                        {
                            if (this.opacity != triangle.Opacity)
                            {
                                triangle.Opacity = this.opacity;
                                //opacityChanged = true;
                            }
                        }
                        //if (opacityChanged)
                        {
                            //Mark the model as being dirty, the reason is that when the opacity
                            //changes this might cause the model to have to be rerendered, if we
                            //are changing the positions of the vertices to handle the antialiasing
                            //issue
                            //model.SetDirty();
                        }
                    }
                }
            }
        }

        internal override void Render(List<Triangle> trianglesToRender, List<ModelVisual3D> renderedVisuals)
        {
            this.RenderedModels.Clear();
            bool modelWasRendered = false;

            //Check if we really need to draw anything here or can
            //we just quickly return.
            if ((this.Content == null) ||
                (this.Viewport == null)) // ||
                //(this.Opacity_DO_NOT_USE_WILL_BE_DELETED <= 0.01))
            {
                return;
            }

            //Create all of the triangles associated with the visual, will only
            //create new triangles if the triangles are determinde to be dirty
            CreateTriangles();

            //If there are not any triangles then we don't need to do anything
            if((this.allModelTriangles == null) || 
               (this.allModelTriangles.Count == 0))
            {
                return;
            }

            //Render all of the relevant triangles
            Camera camera = this.Viewport.Camera;

            //Transforms points from NDC to screen coords
            Matrix3D ndcToScreenTransform = this.Viewport.NDCToScreenTransform;

            //Apply the camera transform which converts to NDC values, then
            //turn these into screen values
            Matrix3D worldToNDCTransform = camera.Value;// *ndcToScreenTransform;

            List<Triangle> allTriangles;
            foreach (Model3D model in this.allModelTriangles.Keys)
            {
                allTriangles = this.allModelTriangles[model];

                //If the viewport is dirty or the model is dirty then we have
                //to render, otherwise we can just add the triangles to the list
                //of triangles we are going to render since they have not moved

                //TODO:Make sure we have view frustum culling implemented
                if (!this.Viewport.IsViewportDirty && !model.IsDirty)
                {
                    //Just add triangles, no need to recompute them
                    trianglesToRender.AddRange(allTriangles);
                    modelWasRendered = true;
                    this.RenderedModels.Add(model);
                    continue;
                }
                
                //TODO: Check:
                //1. If model is not dirty then just reuse current triangles
                //2. Check if models triangles are visible or not, if not turn
                //   then off - back face culling
                //3. Cull based on the view frustum, will need to dynamically
                //   generate new triangles that are being clipped
                Matrix3D worldTransform = model.GetLocalToWorldTransform(this);

                //This matrix converts points from the objects local space to
                //screen space
                Matrix3D localToNDCTransform = worldTransform * worldToNDCTransform;

                bool trianglesRendered = false;
                foreach (Triangle triangle in allTriangles)
                {
                    //Want to project into homogeneous space without perspective divide so that we
                    //can perform clipping to view frustum volume.  If perform perspective divide
                    //before clipping points behind the camera will be projected infront of the camera
                    Point3D p0 = new Point3D(triangle.P0.X * localToNDCTransform.M11 + triangle.P0.Y * localToNDCTransform.M21 + triangle.P0.Z * localToNDCTransform.M31 + localToNDCTransform.OffsetX,
                                             triangle.P0.X * localToNDCTransform.M12 + triangle.P0.Y * localToNDCTransform.M22 + triangle.P0.Z * localToNDCTransform.M32 + localToNDCTransform.OffsetY,
                                             triangle.P0.X * localToNDCTransform.M13 + triangle.P0.Y * localToNDCTransform.M23 + triangle.P0.Z * localToNDCTransform.M33 + localToNDCTransform.OffsetZ);
                    Point3D p1 = new Point3D(triangle.P1.X * localToNDCTransform.M11 + triangle.P1.Y * localToNDCTransform.M21 + triangle.P1.Z * localToNDCTransform.M31 + localToNDCTransform.OffsetX,
                                             triangle.P1.X * localToNDCTransform.M12 + triangle.P1.Y * localToNDCTransform.M22 + triangle.P1.Z * localToNDCTransform.M32 + localToNDCTransform.OffsetY,
                                             triangle.P1.X * localToNDCTransform.M13 + triangle.P1.Y * localToNDCTransform.M23 + triangle.P1.Z * localToNDCTransform.M33 + localToNDCTransform.OffsetZ);
                    Point3D p2 = new Point3D(triangle.P2.X * localToNDCTransform.M11 + triangle.P2.Y * localToNDCTransform.M21 + triangle.P2.Z * localToNDCTransform.M31 + localToNDCTransform.OffsetX,
                                             triangle.P2.X * localToNDCTransform.M12 + triangle.P2.Y * localToNDCTransform.M22 + triangle.P2.Z * localToNDCTransform.M32 + localToNDCTransform.OffsetY,
                                             triangle.P2.X * localToNDCTransform.M13 + triangle.P2.Y * localToNDCTransform.M23 + triangle.P2.Z * localToNDCTransform.M33 + localToNDCTransform.OffsetZ);

                    double p0W = triangle.P0.X * localToNDCTransform.M14 + triangle.P0.Y * localToNDCTransform.M24 + triangle.P0.Z * localToNDCTransform.M34 + localToNDCTransform.M44;
                    double p1W = triangle.P1.X * localToNDCTransform.M14 + triangle.P1.Y * localToNDCTransform.M24 + triangle.P1.Z * localToNDCTransform.M34 + localToNDCTransform.M44;
                    double p2W = triangle.P2.X * localToNDCTransform.M14 + triangle.P2.Y * localToNDCTransform.M24 + triangle.P2.Z * localToNDCTransform.M34 + localToNDCTransform.M44;

                    //Qucik hacky near plane check - make better if needed
                    if ((p0.Z <= 0))// || (p0.Z >= p0W))
                    {
                        continue;
                    }
                    if ((p1.Z <= 0))// || (p1.Z >= p1W))
                    {
                        continue;
                    }
                    if ((p2.Z <= 0))// || (p2.Z >= p2W))
                    {
                        continue;
                    }

                    //Perform perspective divide
                    double p0WInverse = 1.0 / p0W;
                    double p1WInverse = 1.0 / p1W;
                    double p2WInverse = 1.0 / p2W;
                    p0 = new Point3D(p0.X * p0WInverse, p0.Y * p0WInverse, p0.Z * p0WInverse);
                    p1 = new Point3D(p1.X * p1WInverse, p1.Y * p1WInverse, p1.Z * p1WInverse);
                    p2 = new Point3D(p2.X * p2WInverse, p2.Y * p2WInverse, p2.Z * p2WInverse);

                    //Now values are in NDC space transform to screen
                    p0 = ndcToScreenTransform.Transform(p0);
                    p1 = ndcToScreenTransform.Transform(p1);
                    p2 = ndcToScreenTransform.Transform(p2);

                    //Cull the back facing triangles
                    bool isBackFacing = Vector3D.CrossProduct(p1 - p0, p2 - p0).Z >= 0;
                    if ((triangle.BackMaterial == null) && isBackFacing)
                    {
                        //The triangle does not have a back material
                        //so we will not render the triangle
                        continue;
                    }
                    else if (triangle.BackMaterial != null && isBackFacing)
                    {
                        //Use the back material
                        triangle.CurrentMaterial = triangle.BackMaterial;
                    }
                    else
                    {
                        //Use the front material
                        triangle.CurrentMaterial = triangle.FrontMaterial;
                    }

                    //Due to antialiasing in Silverlight between adjacent primitives
                    //we have to actually overlap polygons slighty so the seam is not 
                    //visible - however the seams will still be visible with
                    if (model.SeamSmoothing != 0)
                    {
                        Point3D screenTriangleCentroid = new Point3D((p0.X + p1.X + p2.X) * MathHelper.OneDivThree,
                                                                     (p0.Y + p1.Y + p2.Y) * MathHelper.OneDivThree,
                                                                     (p0.Z + p1.Z + p2.Z) * MathHelper.OneDivThree);

                        Vector3D p0FromCenter = p0 - screenTriangleCentroid;
                        p0FromCenter.Normalize();
                        Vector3D p1FromCenter = p1 - screenTriangleCentroid;
                        p1FromCenter.Normalize();
                        Vector3D p2FromCenter = p2 - screenTriangleCentroid;
                        p2FromCenter.Normalize();

                        //Define how much we should expand each triangle vertice
                        p0 = p0 + p0FromCenter * model.SeamSmoothing;
                        p1 = p1 + p1FromCenter * model.SeamSmoothing;
                        p2 = p2 + p2FromCenter * model.SeamSmoothing;
                    }

                    //Distance of the triangle from the camera
                    triangle.ZIndex = (p0.Z + p1.Z + p2.Z) * MathHelper.OneDivThree;
                    triangle.SetScreenPoints(p0.X, p0.Y,
                                             p1.X, p1.Y,
                                             p2.X, p2.Y);
                    trianglesToRender.Add(triangle);

                    //Set the override zindex
                    triangle.ZIndexOverride = this.ZIndexOverride;

                    //Indicate that some triangles were rendered
                    trianglesRendered = true;
                }

                if (trianglesRendered)
                {
                    this.RenderedModels.Add(model);
                    modelWasRendered = true;
                }
            }

            //Keep track of all of the visuals we rendered
            if (modelWasRendered)
            {
                renderedVisuals.Add(this);
            }

            this.IsDirty = false;
        }

        private bool IsDirty
        {
            get
            {
                return this.isDirty;
            }
            set
            {
                //TODO:Check scenarios when this becomes dirty - do we always need
                //to make the content dirty as well?
                this.isDirty = value;
                if (value)
                {
                    if (this.Content != null)
                    {
                        this.Content.IsDirty = true;
                    }

                    base.OnBecameDirty();
                }
            }
        }

        internal List<Model3D> RenderedModels
        {
            get;
            private set;
        }

        public double ZIndexOverride
        {
            get;
            set;
        }
    }
}
