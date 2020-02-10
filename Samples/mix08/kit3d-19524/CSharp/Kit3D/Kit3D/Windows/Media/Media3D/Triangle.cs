namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Shapes;
    using System.Windows;
    using System.Windows.Media;

    //TODO: ImageBrush Opacity property does not seem to work - check it out
    //TODO: Make sure in all files only change values if they have really changed

    /// <summary>
    /// Represents a triangle in a geometry mesh
    /// </summary>
    internal sealed class Triangle
    {
        private Material frontMaterial;
        private Material backMaterial;
        private Polygon triangle;
        private double opacity;
        private Material currentMaterial;

        internal Triangle(MeshGeometry3D mesh,
                          int v0Index,
                          int v1Index,
                          int v2Index,
                          Material frontMaterial,
                          Material backMaterial)
        {
            Debug.Assert((frontMaterial != null) || (backMaterial != null), "Triangle created with a null texture");

            //The mesh which owns the triangle
            this.Mesh = mesh;

            //Indexes into the position and texture arrays.
            this.V0Index = v0Index;
            this.V1Index = v1Index;
            this.V2Index = v2Index;

            this.FrontMaterial = frontMaterial;
            this.BackMaterial = backMaterial;

            if (this.FrontMaterial != null)
            {
                this.CurrentMaterial = this.FrontMaterial;
            }
            else
            {
                this.CurrentMaterial = this.BackMaterial;
            }

            //Creates either the polygon or deep zoom control we are going to use
            CreateSilverlightVisual();
        }

        private void CreateSilverlightVisual()
        {
            if (this.triangle == null)
            {
                //This is what will actually be rendered onto the screen
                this.triangle = new Polygon();
                this.triangle.IsHitTestVisible = false;
                //this.triangle.Points = new System.Windows.Media.PointCollection();


                //TODO:Comment out point add
                this.triangle.Points.Add(new Point(0, 0));
                this.triangle.Points.Add(new Point(0, 0));
                this.triangle.Points.Add(new Point(0, 0));
                this.Visual = this.triangle;
                this.Opacity = 1;
            }
        }

        internal Material CurrentMaterial
        {
            get
            {
                return this.currentMaterial;
            }
            set
            {
                if (this.currentMaterial != value)
                {
                    this.currentMaterial = value;
                    CreateSilverlightVisual();

                    if (this.currentMaterial != null)
                    {
                        //TODO: Shouldn't need to cast here,  change design to remove this.
                        DiffuseMaterial material = this.currentMaterial as DiffuseMaterial;
                        if (material != null)
                        {
                            //Want a wireframe look to the triangles that make up the model
                            if (material.Brush.IsOutlineBrush)
                            {
                                SolidColorBrush cb = new SolidColorBrush();
                                this.triangle.Stroke = new SolidColorBrush(Colors.White);
                                this.triangle.StrokeThickness = 1;
                                this.triangle.Fill = new SolidColorBrush(Colors.White);
                                this.triangle.Fill.Opacity = 0.15;
                            }
                            else
                            {
                                this.triangle.Fill = ((DiffuseMaterial)this.currentMaterial).Brush.Brush;
                            }
                        }
                    }
                }
            }
        }

        internal Material FrontMaterial
        {
            get { return this.frontMaterial; }
            set
            {
                this.frontMaterial = null;
                if (value != null)
                {
                    this.frontMaterial = value.Clone();
                }
            }
        }

        internal Material BackMaterial
        {
            get { return this.backMaterial; }
            set
            {
                this.backMaterial = null;
                if (value != null)
                {
                    this.backMaterial = value.Clone();
                }
            }
        }

        /// <summary>
        /// Updates the polgon positions on the screen
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        internal void SetScreenPoints(double x0, double y0,
                                      double x1, double y1,
                                      double x2, double y2)
        {
            //x0, y0, x1, y1, x2, y2 are the projected screen points for 
            //each of the vertices

            //Setting the visual points is slow - not much we can do though
            //since we have to change the points, on my laptop this is taking
            //about 40ms to set just 1600 polygons - not good.
            System.Windows.Media.PointCollection pc = this.triangle.Points;
            pc.Clear();
            pc.Add(new Point(x0, y0));
            pc.Add(new Point(x1, y1));
            pc.Add(new Point(x2, y2));
            //pc[0] = new Point(x0, y0);
            //pc[1] = new Point(x1, y1);
            //pc[2] = new Point(x2, y2);

            if (this.CurrentMaterial != null)
            {
                //The material may need to know about the new points of the polygon
                //for example the TextureMaterial needs to know this in order to 
                //map correctly to the new polygon face
                this.CurrentMaterial.SetScreenValues(this, x0, y0, x1, y1, x2, y2);
            }
        }

        public UIElement Visual
        {
            get;
            private set;
        }

        private MeshGeometry3D Mesh
        {
            get;
            set;
        }

        public Point3D P0
        {
            get { return this.Mesh.Positions[this.V0Index]; }
        }

        public Point3D P1
        {
            get { return this.Mesh.Positions[this.V1Index]; }
        }

        public Point3D P2
        {
            get { return this.Mesh.Positions[this.V2Index]; }
        }

        //TODO: Cache these values
        public Point UV0
        {
            get { return this.Mesh.NormalizedTexturePoint(this.V0Index); }
        }

        public Point UV1
        {
            get { return this.Mesh.NormalizedTexturePoint(this.V1Index); }
        }

        public Point UV2
        {
            get { return this.Mesh.NormalizedTexturePoint(this.V2Index); }
        }

        internal int V0Index
        {
            get;
            set;
        }

        internal int V1Index
        {
            get;
            set;
        }

        internal int V2Index
        {
            get;
            set;
        }

        internal double ZIndex
        {
            get;
            set;
        }

        internal double ZIndexOverride
        {
            get;
            set;
        }

        public double Opacity
        {
            get { return this.opacity; }
            set
            {
                if (this.opacity != value)
                {
                    this.opacity = value;
                    this.Visual.Opacity = this.opacity;
                }
            }
        }
    }
}