namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections.Generic;
    using Kit3D.Math;
    using Kit3D.Windows.Controls;

    public class Model3DGroup : Model3D
    {
        private Model3DCollection children;

        public Model3DGroup()
        {
            this.Children = new Model3DCollection();
        }

        public Model3DCollection Children
        {
            get { return this.children; }
            set
            {
                if (this.children != null)
                {
                    this.children.TransformChanged -= new Model3DCollection.TransformChangedEventHandler(children_TransformChanged);
                    this.children.GeometryChanged -= new Model3DCollection.GeometryChangedEventHandler(children_GeometryChanged);
                    this.children.MaterialChanged -= new Model3DCollection.MaterialChangedEventHandler(children_MaterialChanged);
                }
                this.children = value;
                if (this.children != null)
                {
                    this.children.TransformChanged += new Model3DCollection.TransformChangedEventHandler(children_TransformChanged);
                    this.children.GeometryChanged += new Model3DCollection.GeometryChangedEventHandler(children_GeometryChanged);
                    this.children.MaterialChanged += new Model3DCollection.MaterialChangedEventHandler(children_MaterialChanged);
                }

                //TODO:What kind of event change should this be?
                OnChanged();
            }
        }

        private void children_MaterialChanged()
        {
            OnMaterialChanged();
        }

        private void children_GeometryChanged(Geometry3D.GeometryChangeType changeType)
        {
            OnGeometryChanged(changeType);
        }

        private void children_TransformChanged()
        {
            OnTransformChanged();
        }

        internal override void CreateTriangles(Dictionary<Model3D, List<Triangle>> models, bool materialChanged)
        {
            foreach (Model3D model in this.Children)
            {
                model.CreateTriangles(models, materialChanged);
            }
        }

        internal override void UpdateWorldTransform()
        {
            base.UpdateWorldTransform();

            //Need to update all of the children in the group with the new
            //world transform of the parent;
            if (this.Children != null)
            {
                foreach (ModelVisual3D modelVisual in this.parentWorldTransforms.Keys)
                {
                    foreach (Model3D model in this.Children)
                    {
                        model.SetParentWorldTransform(modelVisual, this.GetLocalToWorldTransform(modelVisual));
                    }
                }
            }
        }

        protected override Rect3D GetBounds()
        {
            Rect3D bounds = Rect3D.Empty;
            foreach (Model3D model in this.Children)
            {
                bounds.Union(model.Bounds);
            }
            return bounds;
        }

        internal override void IntersectsWith(ModelVisual3D visual, Ray ray, List<HitResult> results)
        {
            foreach (Model3D model in this.Children)
            {
                model.IntersectsWith(visual, ray, results);
            }
        }
    }
}
