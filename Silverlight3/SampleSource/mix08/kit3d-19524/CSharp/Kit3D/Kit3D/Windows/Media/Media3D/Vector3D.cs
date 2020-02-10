namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public struct Vector3D
    {
        private double x;
        private double y;
        private double z;

        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region Properties

        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        public double Z
        {
            get
            {
                return this.z;
            }
            set
            {
                this.z = value;
            }
        }

        public double Length
        {
            get
            {
                return MathHelper.SquareRoot(this.x * this.x + this.y * this.y + this.z * this.z);
            }
        }

        public double LengthSquared
        {
            get
            {
                return this.x * this.x + this.y * this.y + this.z * this.z;
            }
        }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^
                   this.y.GetHashCode() ^
                   this.z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is Vector3D))
            {
                return false;
            }

            return Equals(this, (Vector3D)obj);
        }

        public bool Equals(Vector3D v)
        {
            return Equals(this, v);
        }

        public static bool Equals(Vector3D v1, Vector3D v2)
        {
            //TODO: Use this?
            /*
            return MathHelper.AreEqual(v1.x, v2.x) &&
                   MathHelper.AreEqual(v1.y, v2.y) &&
                   MathHelper.AreEqual(v1.z, v2.z);*/
            
            return v1.x.Equals(v2.x) &&
                   v1.y.Equals(v2.y) &&
                   v1.z.Equals(v2.z);
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }

        public static double DotProduct(Vector3D v1, Vector3D v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static Vector3D CrossProduct(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.y * v2.z - v1.z * v2.y,
                                v1.z * v2.x - v1.x * v2.z,
                                v1.x * v2.y - v1.y * v2.x);
        }

        public void Negate()
        {
            this.x *= -1;
            this.y *= -1;
            this.z *= -1;
        }

        public void Normalize()
        {
            double lengthSquared = this.LengthSquared;
            double inverseSquareRoot = MathHelper.InverseSquareRoot(lengthSquared);
            this.x *= inverseSquareRoot;
            this.y *= inverseSquareRoot;
            this.z *= inverseSquareRoot;
        }

        public static Point3D Add(Vector3D v, Point3D p)
        {
            return new Point3D(v.x + p.X, v.y + p.Y, v.z + p.Z);
        }

        public static Vector3D Add(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Point3D operator +(Vector3D v, Point3D p)
        {
            return Add(v, p);
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return Add(v1, v2);
        }

        public static Point3D Subtract(Vector3D v, Point3D p)
        {
            return new Point3D(v.x - p.X, v.y - p.Y, v.z - p.Z);
        }

        public static Vector3D Subtract(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Point3D operator -(Vector3D v, Point3D p)
        {
            return Subtract(v, p);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return Subtract(v1, v2);
        }

        public static Vector3D Multiply(double scalar, Vector3D v)
        {
            return new Vector3D(v.x * scalar, v.y * scalar, v.z * scalar);
        }

        public static Vector3D Multiply(Vector3D v, double scalar)
        {
            return new Vector3D(v.x * scalar, v.y * scalar, v.z * scalar);
        }

        public static Vector3D operator *(double scalar, Vector3D v)
        {
            return Multiply(scalar, v);
        }

        public static Vector3D operator *(Vector3D v, double scalar)
        {
            return Multiply(v, scalar);
        }

        public static Vector3D Divide(Vector3D v, double scalar)
        {
            double div = 1.0/scalar;
            return new Vector3D(v.x * div, v.y * div, v.z * div);
        }

        public static Vector3D operator /(Vector3D v, double scalar)
        {
            return Divide(v, scalar);
        }

        public static bool operator ==(Vector3D v1, Vector3D v2)
        {
            return Equals(v1, v2);
        }

        public static bool operator !=(Vector3D v1, Vector3D v2)
        {
            return !Equals(v1, v2);
        }

        public static explicit operator Point3D(Vector3D v)
        {
            return new Point3D(v.x, v.y, v.z);
        }

        public static Vector3D operator -(Vector3D v)
        {
            return new Vector3D(-v.x, -v.y, -v.z);
        }

        #endregion
    }
}
