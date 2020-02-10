/*
 * Copyright Mark Dawson 2008 - http://www.markdawson.org/kit3D
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Kit3D.Windows.Media.Animation;
using System.Collections.Generic;
using Kit3D.Math;
using Kit3D.Windows.Controls;

namespace Kit3D.Windows.Media.Media3D
{
    public abstract class Model3D : Animatable
    {
        //Raised when the models transform is updated
        internal delegate void TransformChangedEventHandler();
        internal event TransformChangedEventHandler TransformChanged;

        internal delegate void MaterialChangedEventHandler();
        internal event MaterialChangedEventHandler MaterialChanged;

        internal delegate void GeometryChangedEventHandler(Geometry3D.GeometryChangeType changeType);
        internal event GeometryChangedEventHandler GeometryChanged;

        //TODO:Verify what is the initial value of the transform
        private Transform3D localTransform = new MatrixTransform3D();
        protected Dictionary<ModelVisual3D, Matrix3D> localToWorldTransforms = new Dictionary<ModelVisual3D, Matrix3D>();
        protected Dictionary<ModelVisual3D, Matrix3D> parentWorldTransforms = new Dictionary<ModelVisual3D, Matrix3D>();

        /// <summary>
        /// A list containing all of the visual that are associated with this model
        /// </summary>
        private List<ModelVisual3D> visuals = new List<ModelVisual3D>();

        protected Model3D()
        {
            this.IsDirty = true;
        }

        /// <summary>
        /// Get / set the transform that will affect the Model
        /// </summary>
        public Transform3D Transform
        {
            get { return this.localTransform; }
            set
            {
                if (this.localTransform != null)
                {
                    this.localTransform.Changed -= new EventHandler(localTransform_Changed);
                }
                this.localTransform = value;
                if (this.localTransform != null)
                {
                    this.localTransform.Changed += new EventHandler(localTransform_Changed);
                }

                //Need to update the world transform, so we can use this for
                //rendering.
                localTransform_Changed(null, null);
            }
        }

        private void localTransform_Changed(object sender, EventArgs e)
        {
            //Get the new local to world transform for this model
            this.UpdateWorldTransform();

            //This event just indicates that we need to redraw but the ModelVisual3D
            //instance does not need to recreate the polygons for the item
            OnTransformChanged();
        }

        protected internal void OnTransformChanged()
        {
            TransformChangedEventHandler handler = TransformChanged;
            if (handler != null)
            {
                handler();
            }
        }

        internal void SetParentWorldTransform(ModelVisual3D visual, Matrix3D transform)
        {
            //TODO:Only set if they are different? Is this more efficient in
            //the long run - see usage pattern for this function call.
            this.parentWorldTransforms[visual] = transform;
            this.UpdateWorldTransform();
        }

        /// <summary>
        /// Returns the LocalToWorld transform for the model which is associated
        /// with the visual
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        internal Matrix3D GetLocalToWorldTransform(ModelVisual3D visual)
        {
            //TODO: Will this only be called when the visual is associated with
            //      the model? Verify.
            if (!this.localToWorldTransforms.ContainsKey(visual))
            {
                return this.localTransform.Value;
            }
            return this.localToWorldTransforms[visual];
        }

        /*
         * Local Transform - this is just for the model
         * LocalToWorldTransform -> depends on the visual the object is associated with
         * ParentWorldTransform -> depends on the visual the object is associated with
        */
        internal virtual void UpdateWorldTransform()
        {
            //TODO: What if the user set the local transform to NULL?
            //Need to investigate what WPF does in this scenario.

            foreach (ModelVisual3D modelVisual in this.parentWorldTransforms.Keys)
            {
                //TODO: Verify always have a parent transform when have local transform?
                this.localToWorldTransforms[modelVisual] = this.Transform.Value * this.parentWorldTransforms[modelVisual];
            }

            //Make sure we are re-rendered, since the transform has changed
            this.IsDirty = true;
        }

        internal abstract void CreateTriangles(Dictionary<Model3D, List<Triangle>> models, bool materialsChanged);

        //TODO: Implement
        public Rect3D Bounds
        {
            get
            {
                return this.GetBounds();
            }
        }

        protected abstract Rect3D GetBounds();

        /// <summary>
        /// Get / set the amount of smooting to use to reduce the visible seam
        /// between triangles.  A recommended value is around 0.8, this is defaulted
        /// to 0 and with this value you will see breaks between each polygon.
        /// </summary>
        public double SeamSmoothing
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates if this model needs to be re-rendered
        /// </summary>
        internal bool IsDirty
        {
            get;
            set;
        }

        /*
        internal void SetDirty()
        {
            OnChanged();
        }
        */

        protected void OnMaterialChanged()
        {
            this.IsDirty = true;
            MaterialChangedEventHandler handler = MaterialChanged;
            if (handler != null)
            {
                handler();
            }
        }

        protected void OnGeometryChanged(Geometry3D.GeometryChangeType changeType)
        {
            this.IsDirty = true;
            GeometryChangedEventHandler handler = GeometryChanged;
            if (handler != null)
            {
                handler(changeType);
            }
        }

        protected override void OnChanged()
        {
            this.IsDirty = true;
            base.OnChanged();
        }

        /// <summary>
        /// Returns true if the model intersects with the ray
        /// </summary>
        /// <param name="ray">The ray must be in the local space of the model</param>
        /// <returns></returns>
        internal abstract void IntersectsWith(ModelVisual3D visual, Ray ray, List<HitResult> results);
    }
}
