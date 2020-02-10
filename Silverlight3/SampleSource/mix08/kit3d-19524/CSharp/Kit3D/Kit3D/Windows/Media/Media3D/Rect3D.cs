namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using Kit3D.Math;

    public struct Rect3D
    {
        private double x;
        private double y;
        private double z;
        private double sizeX;
        private double sizeY;
        private double sizeZ;
        private static Rect3D empty;
        
        static Rect3D()
        {
            Rect3D.empty = new Rect3D();
            empty.x = double.PositiveInfinity;
            empty.y = double.PositiveInfinity;
            empty.z = double.PositiveInfinity;
            empty.sizeX = double.NegativeInfinity;
            empty.sizeY = double.NegativeInfinity;
            empty.sizeZ = double.NegativeInfinity;
        }

        public Rect3D(Point3D location, Size3D size)
        {
            if (size.IsEmpty)
            {
                this = Rect3D.empty;
            }
            else
            {
                this.x = location.X;
                this.y = location.Y;
                this.z = location.Z;
                this.sizeX = size.X;
                this.sizeY = size.Y;
                this.sizeZ = size.Z;
            }
        }

        public Rect3D(Point3D point, Vector3D v) 
            : this(point, point+v)
        {
        }

        public Rect3D(Point3D p1, Point3D p2)
        {
            this.x = System.Math.Min(p1.X, p2.X);
            this.y = System.Math.Min(p1.Y, p2.Y);
            this.z = System.Math.Min(p1.Z, p2.Z);

            this.sizeX = System.Math.Max(p1.X, p2.X) - this.x;
            this.sizeY = System.Math.Max(p1.Y, p2.Y) - this.y;
            this.sizeZ = System.Math.Max(p1.Z, p2.Z) - this.z;
        }

        public Rect3D(double x, double y, double z, double sizeX, double sizeY, double sizeZ)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            if (sizeX < 0 || sizeY < 0 || sizeZ < 0)
            {
                throw new ArgumentException("It is not valid to set a size dimension to less than 0 in a Rect3D structure");
            }
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
        }

        public bool IsEmpty
        {
            get
            {
                return this.SizeX < 0;
            }
        }

        public double X
        {
            get{ return this.x; }
            set
            {
                if(IsEmpty)
                {
                    throw new InvalidOperationException("Cannot change the X property for an Empty Rect3D");
                }
                this.x = value;
            }
        }

        public double Y
        {
            get{ return this.y; }
            set
            {
                if(IsEmpty)
                {
                    throw new InvalidOperationException("Cannot change the Y property for an Empty Rect3D");
                }
                this.y = value;
            }
        }

        public double Z
        {
            get{ return this.z; }
            set
            {
                if(IsEmpty)
                {
                    throw new InvalidOperationException("Cannot change the Z property for an Empty Rect3D");
                }
                this.z = value;
            }
        }

        public double SizeX
        {
            get { return this.sizeX; }
            set
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Cannot change the SizeX property for an empty Rect3D");
                }
                if (value < 0)
                {
                    throw new ArgumentException("Values less than 0 are not allowed for the SizeX property");
                }
                this.sizeX = value;
            }
        }

        public double SizeY
        {
            get { return this.sizeY; }
            set
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Cannot change the SizeY property for an empty Rect3D");
                }
                if (value < 0)
                {
                    throw new ArgumentException("Values less than 0 are not allowed for the SizeY property");
                }
                this.sizeY = value;
            }
        }

        public double SizeZ
        {
            get { return this.sizeZ; }
            set
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Cannot change the SizeZ property for an empty Rect3D");
                }
                if (value < 0)
                {
                    throw new ArgumentException("Values less than 0 are not allowed for the SizeZ property");
                }
                this.sizeZ = value;
            }
        }

        public Point3D Location
        {
            get { return new Point3D(X, Y, Z); }
            set
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Cannot set the location of an empty Rect3D");
                }
                this.X = value.X;
                this.Y = value.Y;
                this.Z = value.Z;
            }
        }

        public static Rect3D Empty
        {
            get { return Rect3D.empty; }
        }

        public bool Contains(Point3D p)
        {
            return Contains(p.X, p.Y, p.Z);
        }

        public bool Contains(Rect3D rect)
        {
            if (IsEmpty)
            {
                return false;
            }

            return (rect.X >= this.X) &&
                   (rect.Y >= this.Y) &&
                   (rect.Z >= this.Z) &&
                   (rect.X + rect.SizeX <= this.X + this.SizeX) &&
                   (rect.Y + rect.SizeY <= this.Y + this.SizeY) &&
                   (rect.Z + rect.SizeZ <= this.Z + this.SizeZ);
        }

        public bool Contains(double x, double y, double z)
        {
            if (IsEmpty)
            {
                return false;
            }

            return (x >= this.X) &&
                   (x <= (this.X + this.SizeX)) &&
                   (y >= this.Y) &&
                   (y <= (this.Y + this.SizeY)) &&
                   (z >= this.Z) &&
                   (z <= (this.Z + this.SizeZ));
        }

        public void Union(Point3D p)
        {
            Union(new Rect3D(p, p));
        }

        public void Union(Rect3D rect)
        {
            if (IsEmpty)
            {
                this = rect;
            }
            else
            {
                double minX = System.Math.Min(this.X, rect.X);
                double minY = System.Math.Min(this.Y, rect.Y);
                double minZ = System.Math.Min(this.Z, rect.Z);

                this.SizeX = System.Math.Max(this.X + this.SizeX, rect.X + rect.SizeX) - minX;
                this.SizeY = System.Math.Max(this.Y + this.SizeY, rect.Y + rect.SizeY) - minY;
                this.SizeZ = System.Math.Max(this.Z + this.SizeZ, rect.Z + rect.SizeZ) - minZ;
                this.x = minX;
                this.y = minY;
                this.z = minZ;
            }
        }

        public static Rect3D Union(Rect3D rect, Point3D p)
        {
            rect.Union(p);
            return rect;
        }

        public static Rect3D Union(Rect3D rect1, Rect3D rect2)
        {
            rect1.Union(rect2);
            return rect1;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}",
                                 this.X,
                                 this.Y,
                                 this.Z,
                                 this.SizeX,
                                 this.SizeY,
                                 this.SizeZ);
        }

        /// <summary>
        /// Used to test if the ray intersects with an AABB
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        internal bool IntersectsWith(Ray ray)
        {
            //For more information on how this works see "Essential Mathematics for games and
            //interactive applications" by James M. Van Verth & Lars M. Bishop

            double maxS = double.NegativeInfinity;
            double minT = double.PositiveInfinity;

            //Need to check the ray against all of the axis and test
            //to see if it intersects with the axis aligned bounding box

            //X-Axis checking
            if (MathHelper.IsZero(ray.Direction.X))
            {
                if ((ray.Origin.X < this.X) || (ray.Origin.X > (this.X + this.SizeX)))
                {
                    return false;
                }
            }
            else
            {
                //Find components along the ray that intersect with the min/max
                //bounds of the AABB in the x-axis
                double s = (this.X - ray.Origin.X) / ray.Direction.X;
                double t = ((this.X + this.SizeX) - ray.Origin.X) / ray.Direction.X;
                if (s > t)
                {
                    double temp = s;
                    s = t;
                    t = temp;
                }

                if (s > maxS)
                {
                    maxS = s;
                }
                if (t < minT)
                {
                    minT = t;
                }

                //Check that the values are not behind the origin of the ray or
                //fallen outside of the bounds of the AABB in the x-axis
                if ((minT < 0) || (maxS > minT))
                {
                    return false;
                }
            }

            //Y-Axis
            if (MathHelper.IsZero(ray.Direction.Y))
            {
                if ((ray.Origin.Y < this.Y) || (ray.Origin.Y > (this.Y + this.SizeY)))
                {
                    return false;
                }
            }
            else
            {
                double s = (this.Y - ray.Origin.Y) / ray.Direction.Y;
                double t = ((this.Y + this.SizeY) - ray.Origin.Y) / ray.Direction.Y;
                if (s > t)
                {
                    double temp = s;
                    s = t;
                    t = temp;
                }

                if (s > maxS)
                {
                    maxS = s;
                }
                if (t < minT)
                {
                    minT = t;
                }

                if ((minT < 0) || (maxS > minT))
                {
                    return false;
                }
            }

            //Z-Axis
            if (MathHelper.IsZero(ray.Direction.Z))
            {
                if ((ray.Origin.Z < this.Z) || (ray.Origin.Z > (this.Z + this.SizeZ)))
                {
                    return false;
                }
            }
            else
            {
                double s = (this.Z - ray.Origin.Z) / ray.Direction.Z;
                double t = ((this.Z + this.SizeZ) - ray.Origin.Z) / ray.Direction.Z;
                if (s > t)
                {
                    double temp = s;
                    s = t;
                    t = temp;
                }

                if (s > maxS)
                {
                    maxS = s;
                }
                if (t < minT)
                {
                    minT = t;
                }

                if ((minT < 0) || (maxS > minT))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
