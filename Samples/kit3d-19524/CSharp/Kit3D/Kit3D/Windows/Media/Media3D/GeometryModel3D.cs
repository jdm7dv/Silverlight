namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections.Generic;
    using Kit3D.Math;
    using Kit3D.Windows.Controls;

    public class GeometryModel3D : Model3D
    {
        private Geometry3D geometry;
        private Material frontMaterial;
        private Material backMaterial;

        public GeometryModel3D()
        {}

        public GeometryModel3D(Geometry3D geometry, Material material)
        {
            this.Geometry = geometry;
            this.Material = material;
        }

        public Geometry3D Geometry
        {
            get { return this.geometry; }
            set
            {
                if (this.geometry != null)
                {
                    this.geometry.GeometryChanged -= new Geometry3D.GeometryChangedEventHandler(geometry_GeometryChanged);
                }
                this.geometry = value;
                if (this.geometry != null)
                {
                    this.geometry.GeometryChanged += new Geometry3D.GeometryChangedEventHandler(geometry_GeometryChanged);
                }

                //Only support MeshGeometry3D for now
                if ((this.geometry != null) && !(this.geometry is MeshGeometry3D))
                {
                    throw new Exception("Unknown geometry type:" + this.geometry.ToString());
                }
                OnGeometryChanged(Geometry3D.GeometryChangeType.GeometrySet);
            }
        }

        void geometry_GeometryChanged(Geometry3D.GeometryChangeType changeType)
        {
            OnGeometryChanged(changeType);
        }

        public Material Material
        {
            get { return this.frontMaterial; }
            set
            {
                if (this.frontMaterial != null)
                {
                    this.frontMaterial.Changed -= new EventHandler(Material_Changed);
                }
                this.frontMaterial = value;

                if (this.frontMaterial != null)
                {
                    this.frontMaterial.Changed += new EventHandler(Material_Changed);
                }
                OnMaterialChanged();
            }
        }

        public Material BackMaterial
        {
            get { return this.backMaterial; }
            set
            {
                if (this.backMaterial != null)
                {
                    this.backMaterial.Changed -= new EventHandler(Material_Changed);
                }
                this.backMaterial = value;

                if (this.backMaterial != null)
                {
                    this.backMaterial.Changed += new EventHandler(Material_Changed);
                }
                OnMaterialChanged();
            }
        }

        void Material_Changed(object sender, EventArgs e)
        {
            OnMaterialChanged();
        }

        internal override void CreateTriangles(Dictionary<Model3D, List<Triangle>> models, bool materialsChanged)
        {
            if (this.Geometry == null)
            {
                return;
            }
            models.Add(this, this.Geometry.CreateTriangles(this.Material, this.BackMaterial, materialsChanged));
        }

        protected override Rect3D GetBounds()
        {
            //Now can compare for min values
            if (this.Geometry == null)
            {
                return Rect3D.Empty;
            }

            //Make sure we take into account the current transform
            //on the model
            return this.Transform.Value.Transform(this.Geometry.LocalBounds);
        }

        internal override void IntersectsWith(ModelVisual3D visual, Ray ray, List<HitResult> results)
        {
            if (this.Geometry == null)
            {
                return;
            }

            this.Geometry.IntersectsWith(visual, this, ray, results);
        }
    }
}
