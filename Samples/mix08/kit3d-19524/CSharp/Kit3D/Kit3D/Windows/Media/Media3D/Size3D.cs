namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public struct Size3D
    {
        private static Size3D empty;
        private double x;
        private double y;
        private double z;

        static Size3D()
        {
            empty = CreateEmpty();
        }

        private static Size3D CreateEmpty()
        {
            Size3D empty = new Size3D();
            empty.x = double.NegativeInfinity;
            empty.y = double.NegativeInfinity;
            empty.z = double.NegativeInfinity;

            return empty;
        }

        public Size3D(double x, double y, double z)
        {
            if (x < 0 || y < 0 || z < 0)
            {
                throw new ArgumentException("Values for x,y and z cannot be less than 0");
            }

            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region Properties 

        public bool IsEmpty
        {
            get
            {
                return this.x < 0;
            }
        }

        public static Size3D Empty
        {
            get
            {
                return Size3D.empty;
            }
        }

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

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Size3D))
            {
                return false;
            }
            return Equals(this, (Size3D)obj);
        }

        public bool Equals(Size3D size)
        {
            return Equals(this, size);
        }

        public static bool Equals(Size3D s1, Size3D s2)
        {
            if (s1.IsEmpty && s2.IsEmpty)
            {
                return true;
            }

            return s1.x.Equals(s2.x) &&
                   s1.y.Equals(s2.y) &&
                   s1.z.Equals(s2.z);
        }

        public static bool operator ==(Size3D s1, Size3D s2)
        {
            return s1.Equals(s2);
        }

        public static bool operator !=(Size3D s1, Size3D s2)
        {
            return !s1.Equals(s2);
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return this.x.GetHashCode() ^
                   this.y.GetHashCode() ^
                   this.z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", this.X, this.Y, this.Z);
        }
    }
}
