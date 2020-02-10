namespace Kit3D.Objects
{
    using System;
    using Kit3D.Windows.Media.Media3D;

    /// <summary>
    /// Creates a Torus
    /// 
    /// Code taken from http://blogs.msdn.com/karstenj/archive/2004/08/20/217882.aspx
    /// and modified slightly.
    /// </summary>
    public sealed class Torus
    {
        public Torus(double innerRadius,
                     double outerRadius,
                     int torusSegmentCount,
                     int torusSegmentBandCount)
        {
            this.InnerRadius = innerRadius;
            this.OuterRadius = outerRadius;
            this.TorusSegmentCount = torusSegmentCount;
            this.TorusSegmentBandCount = torusSegmentBandCount;
        }

        public MeshGeometry3D MeshGeometry3D
        {
            get
            {
                MeshGeometry3D mesh = new MeshGeometry3D();

                //Loop rounds the circumference creating all of the individual segments of the torus
                for (int i = 0; i < this.TorusSegmentCount; ++i)
                {
                    //Calculate the starting angle of the torus segment
                    double torusAngle = System.Math.PI * 2 * ((double)i / this.TorusSegmentCount);

                    //Create a vector that is pointing in the current direction of the
                    //segment we want to create, this is a unit vector
                    Vector3D currentSegmentDirection = new Vector3D(System.Math.Cos(torusAngle), System.Math.Sin(torusAngle), 0);

                    //For each segment we want to create a number of bands going around
                    //the segment
                    for (int j = 0; j < this.TorusSegmentBandCount; ++j)
                    {
                        double bandAngle = System.Math.PI * 2 * ((double)j / this.TorusSegmentBandCount);
                        Vector3D currentBandDirection = System.Math.Cos(bandAngle) * currentSegmentDirection + new Vector3D(0, 0, System.Math.Sin(bandAngle));
                        
                        Point3D p = new Point3D(0, 0, 0) + this.InnerRadius * currentSegmentDirection + this.OuterRadius * currentBandDirection;
                        mesh.Positions.Add(p);
                        int a = mesh.Positions.Count - 1;
                        int b = mesh.Positions.Count - 2;
                        int c = mesh.Positions.Count - 2 - this.TorusSegmentBandCount;
                        int d = mesh.Positions.Count - 1 -this.TorusSegmentBandCount;

                        if (j == 0)
                        {
                            b += this.TorusSegmentBandCount;
                            c += this.TorusSegmentBandCount;
                        }

                        if (i == 0)
                        {
                            c += this.TorusSegmentBandCount * this.TorusSegmentCount;
                            d += this.TorusSegmentBandCount * this.TorusSegmentCount;
                        }

                        mesh.TriangleIndices.Add(a);
                        mesh.TriangleIndices.Add(b);
                        mesh.TriangleIndices.Add(c);
                        mesh.TriangleIndices.Add(a);
                        mesh.TriangleIndices.Add(c);
                        mesh.TriangleIndices.Add(d);
                    }
                }

                return mesh;
            }
        }

        public double InnerRadius
        {
            get;
            set;
        }

        public double OuterRadius
        {
            get;
            set;
        }

        /// <summary>
        /// The number of segments that create a single torus segment
        /// </summary>
        public int TorusSegmentBandCount
        {
            get;
            set;
        }

        /// <summary>
        /// The number of segments to break the torus circumference into.
        /// </summary>
        public int TorusSegmentCount
        {
            get;
            set;
        }
    }
}
