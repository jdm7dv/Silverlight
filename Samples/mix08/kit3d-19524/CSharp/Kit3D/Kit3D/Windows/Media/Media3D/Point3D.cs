namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Windows;

    public struct Point3D
    {
        private double x;
        private double y;
        private double z;

        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        //For internal use only, not exposed in WPF libraries
        internal Point3D(Point p)
        {
            this.x = p.X;
            this.y = p.Y;
            this.z = 1;
        }

        internal Point3D(Point3D p)
        {
            this.x = p.X;
            this.y = p.Y;
            this.z = p.Z;
        }

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public double Z
        {
            get { return this.z; }
            set { this.z = value; }
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is Point3D))
            {
                return false;
            }

            return Equals(this, (Point3D)obj);
        }

        public bool Equals(Point3D p)
        {
            return Equals(this, p);
        }

        public static bool Equals(Point3D p1, Point3D p2)
        {
            //TODO: How does WPF handle this, check doubles?
            return p1.x.Equals(p2.x) && p1.y.Equals(p2.y) && p1.z.Equals(p2.z);
        }

        public override string ToString()
        {
            return "(" + this.X + ", " + this.Y + ", " + this.Z + ")";
        }

        public static explicit operator Vector3D(Point3D p)
        {
            return new Vector3D(p.x, p.y, p.z);
        }

        public static Vector3D Subtract(Point3D p1, Point3D p2)
        {
            return new Vector3D(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
        }

        public static Vector3D operator -(Point3D p1, Point3D p2)
        {
            return Subtract(p1, p2);
        }

        public static Point3D Subtract(Point3D p, Vector3D v)
        {
            return new Point3D(p.x - v.X, p.y - v.Y, p.z - v.Z);
        }

        public static Point3D operator -(Point3D p, Vector3D v)
        {
            return Subtract(p, v);
        }

        public static Point3D Add(Point3D point, Vector3D v)
        {
            return new Point3D(point.x + v.X, point.y + v.Y, point.z + v.Z);
        }

        public static Point3D operator +(Point3D point, Vector3D v)
        {
            return Add(point, v);
        }

        public static bool operator ==(Point3D p1, Point3D p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Point3D p1, Point3D p2)
        {
            return !p1.Equals(p2);
        }
    }
}
