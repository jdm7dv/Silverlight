namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public class TranslateTransform3D : AffineTransform3D
    {
        private double offsetX;
        private double offsetY;
        private double offsetZ;

        public TranslateTransform3D()
        {
        }

        public TranslateTransform3D(Vector3D offset)
        {
            this.offsetX = offset.X;
            this.offsetY = offset.Y;
            this.offsetZ = offset.Z;
        }

        public TranslateTransform3D(double tx, double ty, double tz)
        {
            this.offsetX = tx;
            this.offsetY = ty;
            this.offsetZ = tz;
        }

        public double OffsetX
        {
            get { return this.offsetX; }
            set
            {
                this.offsetX = value;
                OnChanged();
            }
        }

        public double OffsetY
        {
            get { return this.offsetY; }
            set
            {
                this.offsetY = value;
                OnChanged();
            }
        }

        public double OffsetZ
        {
            get { return this.offsetZ; }
            set
            {
                this.offsetZ = value;
                OnChanged();
            }
        }

        public override Matrix3D Value
        {
            get
            {
                //TODO: Cache this - implement Freezable mechanism?
                return new Matrix3D(1, 0, 0, 0,
                                    0, 1, 0, 0,
                                    0, 0, 1, 0,
                                    OffsetX, OffsetY, OffsetZ, 1);
            }
        }
    }
}
