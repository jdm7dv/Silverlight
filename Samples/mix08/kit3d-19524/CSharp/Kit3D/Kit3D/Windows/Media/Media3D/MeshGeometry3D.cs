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

using Kit3D.Windows.Media;
using System.Collections.Generic;
using Kit3D.Math;
using Kit3D.Windows.Controls;

namespace Kit3D.Windows.Media.Media3D
{
    //TODO: Test if any of the collections change that we get the events.


    public sealed class MeshGeometry3D : Geometry3D
    {
        private bool boundingBoxIsDirty = true;
        private Int32Collection triangleIndices;
        private Point3DCollection positions;
        private Kit3D.Windows.Media.PointCollection textureCoordinates;
        private bool needNormalization = false;
        private Rect3D localBoundingRect = Rect3D.Empty;
        private Point textureMinPoint = new Point(double.PositiveInfinity, double.PositiveInfinity);
        private Point textureMaxPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
        //private Vector3DCollection normals;

        public MeshGeometry3D()
        {
            this.Triangles = new List<Triangle>();
            this.NormalizeTextureCoordinates = true;
            this.TriangleIndices = new Int32Collection();
            this.Positions = new Point3DCollection();
            this.TextureCoordinates = new PointCollection();

            //this.Normals = new Vector3DCollection();
        }

        public Int32Collection TriangleIndices
        {
            get { return this.triangleIndices; }
            set
            {
                if (this.triangleIndices != null)
                {
                    this.triangleIndices.Changed -= new EventHandler(triangleIndices_Changed);
                }
                this.triangleIndices = value;
                if (this.triangleIndices != null)
                {
                    this.triangleIndices.Changed += new EventHandler(triangleIndices_Changed);
                }
                OnGeometryChanged(GeometryChangeType.TriangleIndices);
            }
        }

        private void triangleIndices_Changed(object sender, EventArgs e)
        {
            OnGeometryChanged(GeometryChangeType.TriangleIndices);
        }

        public Point3DCollection Positions
        {
            get { return this.positions; }
            set
            {
                if (this.positions != null)
                {
                    this.positions.Changed -= new EventHandler(positions_Changed);
                }
                this.positions = value;
                if (this.positions != null)
                {
                    this.positions.Changed += new EventHandler(positions_Changed);
                }
                OnGeometryChanged(GeometryChangeType.Positions);
            }
        }

        private void positions_Changed(object sender, EventArgs e)
        {
            OnGeometryChanged(GeometryChangeType.Positions);
        }

        public PointCollection TextureCoordinates
        {
            get { return this.textureCoordinates; }
            set
            {
                if (this.textureCoordinates != null)
                {
                    this.textureCoordinates.Changed -= new EventHandler(textureCoordinates_Changed);
                }
                this.textureCoordinates = value;
                if (this.textureCoordinates != null)
                {
                    this.textureCoordinates.Changed += new EventHandler(textureCoordinates_Changed);
                }

                //Need to figure out if the user puts in texture values larger than
                //1 or less than 0
                CalculateTextureExtremes();

                OnGeometryChanged(GeometryChangeType.Texturecoordinates);
            }
        }

        private void textureCoordinates_Changed(object sender, EventArgs e)
        {
            CalculateTextureExtremes();
            OnGeometryChanged(GeometryChangeType.Texturecoordinates);
        }

        internal Point NormalizedTexturePoint(int index)
        {
            if (this.needNormalization)
            {
                //TODO: Optimize - simply stores all of the processed
                //      values in a list and use that instead.
                Point p = this.TextureCoordinates[index];
                return new Point(-this.textureMinPoint.X + (p.X / (this.textureMaxPoint.X - this.textureMinPoint.X)),
                                 -this.textureMinPoint.Y + (p.Y / (this.textureMaxPoint.Y - this.textureMinPoint.Y)));
            }
            else
            {
                return this.TextureCoordinates[index];
            }
        }

        private void CalculateTextureExtremes()
        {
            this.needNormalization = false;

            if (this.TextureCoordinates == null)
            {
                return;
            }

            this.textureMinPoint = new Point(double.PositiveInfinity, double.PositiveInfinity);
            this.textureMaxPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);

            foreach (Point p in this.TextureCoordinates)
            {
                if (p.X < this.textureMinPoint.X)
                {
                    this.textureMinPoint.X = p.X;
                }

                if (p.Y < this.textureMinPoint.Y)
                {
                    this.textureMinPoint.Y = p.Y;
                }

                if (p.X > this.textureMaxPoint.X)
                {
                    this.textureMaxPoint.X = p.X;
                }

                if (p.Y > this.textureMaxPoint.Y)
                {
                    this.textureMaxPoint.Y = p.Y;
                }
            }

            //User has gone outside of the range of the 0,0 -> 1,1 which means
            //we need to do some extra processing
            if (this.NormalizeTextureCoordinates &&
                (this.textureMinPoint.X < 0 ||
                this.textureMinPoint.Y < 0 ||
                this.textureMaxPoint.X > 1 ||
                this.textureMaxPoint.Y > 1))
            {
                this.needNormalization = true;
            }
        }

        public bool NormalizeTextureCoordinates
        {
            get;
            set;
        }

        //NOT SUPPORTED
        /*
        public Vector3DCollection Normals
        {
            get { return this.normals; }
            set
            {
                if (this.normals != null)
                {
                    this.normals.Changed -= new EventHandler(Collection_Changed);
                }
                this.normals = value;
                if (this.normals != null)
                {
                    this.normals.Changed += new EventHandler(Collection_Changed);
                }
                OnChanged();
            }
        }
        */

        /// <summary>
        /// TODO:Remove this eventually?
        /// </summary>
        internal override Point3DCollection PositionsInternal
        {
            get { return this.Positions; }
        }

        internal override List<Triangle> CreateTriangles(Material material,
                                                         Material backMaterial,
                                                         bool materialsChanged)
        {
            List<Triangle> triangles = new List<Triangle>();
            int startIndex = 0;
            if (!materialsChanged)
            {
                //Don't need to create new triangles if we already have enough triangles
                //that have been initialized with the same materials
                double numberOfRequiredTriangles = this.triangleIndices.Count / 3;
                int trianglesToReuseCount = (int)System.Math.Min(this.Triangles.Count, numberOfRequiredTriangles);
                for (int i = 0; i < trianglesToReuseCount; ++i)
                {
                    //Triangle is already created just need to update its position indexes
                    Triangle t = this.Triangles[i];
                    int index = i * 3;
                    t.V0Index = this.triangleIndices[index];
                    t.V1Index = this.triangleIndices[index + 1];
                    t.V2Index = this.triangleIndices[index + 2];

                    //Add to the list of triangles we need
                    triangles.Add(t);
                }
                startIndex = trianglesToReuseCount * 3;
            }

            bool newTrianglesAdded = false;
            for (int triangleIndex = startIndex; triangleIndex < this.triangleIndices.Count; triangleIndex+=3)
            {
                //Note Model should always be set for this method to be called
                Triangle t = new Triangle(this,
                                          this.triangleIndices[triangleIndex],
                                          this.triangleIndices[triangleIndex + 1],
                                          this.triangleIndices[triangleIndex + 2],
                                          material,
                                          backMaterial);
                triangles.Add(t);

                newTrianglesAdded = true;
            }

            if (newTrianglesAdded)
            {
                this.Triangles = triangles;
            }

            return triangles;
        }

        private List<Triangle> Triangles
        {
            get;
            set;
        }

        /// <summary>
        /// The bounding box around the model
        /// </summary>
        internal override Rect3D LocalBounds
        {
            get
            {
                if (this.boundingBoxIsDirty)
                {
                    CalculateLocalBoundingRect();
                    this.boundingBoxIsDirty = false;
                }
                return this.localBoundingRect;
            }
        }

        private void CalculateLocalBoundingRect()
        {
            if ((this.Positions == null) || (this.Positions.Count == 0))
            {
                this.localBoundingRect = Rect3D.Empty;
            }

            Point3D minPoint = new Point3D(double.PositiveInfinity,
                                           double.PositiveInfinity,
                                           double.PositiveInfinity);
            Point3D maxPoint = new Point3D(double.NegativeInfinity,
                                           double.NegativeInfinity,
                                           double.NegativeInfinity);

            foreach (Point3D point in this.Positions)
            {
                if (point.X < minPoint.X)
                {
                    minPoint.X = point.X;
                }
                if (point.Y < minPoint.Y)
                {
                    minPoint.Y = point.Y;
                }
                if (point.Z < minPoint.Z)
                {
                    minPoint.Z = point.Z;
                }

                if (point.X > maxPoint.X)
                {
                    maxPoint.X = point.X;
                }
                if (point.Y > maxPoint.Y)
                {
                    maxPoint.Y = point.Y;
                }
                if (point.Z > maxPoint.Z)
                {
                    maxPoint.Z = point.Z;
                }
            }
            this.localBoundingRect = new Rect3D(minPoint, maxPoint - minPoint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ray">Ray must be in local space of the object</param>
        /// <returns></returns>
        internal override void IntersectsWith(ModelVisual3D visual, GeometryModel3D model, Ray ray, List<HitResult> results)
        {
            //TODO: Some of this logic belongs in the Triangle class, need to
            //      seperate GUI triangle from mathematical Triangle representation

            if ((this.TriangleIndices == null) || (this.TriangleIndices.Count < 3))
            {
                return;
            }

            //Perform a relatively inexpensive test to see if the ray intersects with
            //the local bounds of the geometry, if it doesn't we can return
            //straight away at this point, no need to check in more detail
            if (!this.LocalBounds.IntersectsWith(ray))
            {
                //There is no intersection with the ray
                return;
            }

            //TODO: The following is expensive, carry out some performance testing
            //      for possible worse case scenarios, want the UI feedback to be performant
            //      if possible when using hit testing.

            //For more information on math used here, see "Essential Mathematics for games
            //and interactive applications" by James M. Van Verth and Lars M. Bishop

            //Since the ray intersects with the local bounds we need to
            //test each of the triangles and see if the ray intersects with
            //any of the triangles, since it is possible that the ray travelled
            //through some white space in the AABB.
            for (int index = 0; index < this.TriangleIndices.Count; index += 3)
            {
                Point3D p0 = this.Positions[this.TriangleIndices[index]];
                Point3D p1 = this.Positions[this.TriangleIndices[index + 1]];
                Point3D p2 = this.Positions[this.TriangleIndices[index + 2]];

                Vector3D e1 = p1 - p0;
                Vector3D e2 = p2 - p0;
                Vector3D p = Vector3D.CrossProduct(ray.Direction, e2);

                //Point inside triangle is:
                //T(u,v) = p0 + u * e1 + v * e2 where:
                // u >= 0
                // v >= 0
                // u+v <= 1
                // u,v are the barycentric coordinates for the triangle

                double a = Vector3D.DotProduct(e1, p);
                if (MathHelper.IsZero(a))
                {
                    //The ray and the triangle are parallel, they will not cross
                    continue;
                }

                double f = 1.0 / a;
                Vector3D s = ray.Origin - p0;
                double u = f * Vector3D.DotProduct(s, p);
                if ((u < 0) || (u > 1))
                {
                    //Outside of the triangle
                    continue;
                }

                Vector3D q = Vector3D.CrossProduct(s, e1);
                double v = f * Vector3D.DotProduct(ray.Direction, q);
                if ((v < 0) || (u + v > 1))
                {
                    //Outside of the triangle
                    continue;
                }

                //Need to check the ray intersection is in the direction of the ray
                double t = f * Vector3D.DotProduct(e2, q);
                if (t < 0)
                {
                    //Outside of the triangle
                    continue;
                }
                else
                {
                    //This is an intersection
                    HitResult result = new HitResult
                    {
                        Mesh = this,
                        Model = model,
                        Visual = visual,
                        DistanceToRayOrigin = t,
                        VertexIndex1 = this.TriangleIndices[index],
                        VertexIndex2 = this.TriangleIndices[index+1],
                        VertexIndex3 = this.TriangleIndices[index+2],
                        Point = ray.Origin + t * ray.Direction
                    };
                    results.Add(result);

                    //Don't want to stop looping here because it is possible
                    //that the ray passes through more than one triangle that
                    //is part of the same model so we have to check every triangle
                }
            }
        }
    }
}
