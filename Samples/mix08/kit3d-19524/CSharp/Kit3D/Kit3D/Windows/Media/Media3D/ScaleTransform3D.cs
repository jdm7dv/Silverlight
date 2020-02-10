namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public class ScaleTransform3D : AffineTransform3D
    {
        private double scaleX, scaleY, scaleZ;
        private double centerX, centerY, centerZ;

        public ScaleTransform3D()
        {
            this.scaleX = 1;
            this.scaleY = 1;
            this.scaleZ = 1;
        }

        public ScaleTransform3D(Vector3D scale)
        {
            this.scaleX = scale.X;
            this.scaleY = scale.Y;
            this.scaleZ = scale.Z;
        }

        public ScaleTransform3D(Vector3D scale, Point3D center)
        {
            this.scaleX = scale.X;
            this.scaleY = scale.Y;
            this.scaleZ = scale.Z;
            this.centerX = center.X;
            this.centerY = center.Y;
            this.centerZ = center.Z;
        }

        public ScaleTransform3D(double sx, double sy, double sz)
        {
            this.scaleX = sx;
            this.scaleY = sy;
            this.scaleZ = sz;
        }

        public ScaleTransform3D(double sx, double sy, double sz, double cx, double cy, double cz)
        {
            this.scaleX = sx;
            this.scaleY = sy;
            this.scaleZ = sz;
            this.centerX = cx;
            this.centerY = cy;
            this.centerZ = cz;
        }

        public double CenterX
        {
            get { return this.centerX; }
            set
            {
                this.centerX = value;
                OnChanged();
            }
        }

        public double CenterY
        {
            get { return this.centerY; }
            set
            {
                this.centerY = value;
                OnChanged();
            }
        }

        public double CenterZ
        {
            get { return this.centerZ; }
            set
            {
                this.centerZ = value;
                OnChanged();
            }
        }

        public double ScaleX
        {
            get { return this.scaleX; }
            set
            {
                this.scaleX = value;
                OnChanged();
            }
        }

        public double ScaleY
        {
            get { return this.scaleY; }
            set
            {
                this.scaleY = value;
                OnChanged();
            }
        }

        public double ScaleZ
        {
            get { return this.scaleZ; }
            set
            {
                this.scaleZ = value;
                OnChanged();
            }
        }

        public override Matrix3D Value
        {
            get
            {
                if ((CenterX == 0) &&
                   (CenterY == 0) &&
                   (CenterZ == 0))
                {
                    //Just a scale around 0,0,0
                    return new Matrix3D(ScaleX, 0, 0, 0,
                                        0, ScaleY, 0, 0,
                                        0, 0, ScaleZ, 0,
                                        0, 0, 0, 1);
                }
                else
                {
                    //Need to scale relative to a point
                    Matrix3D result = Matrix3D.Identity;
                    result.ScaleAt(new Vector3D(ScaleX, ScaleY, ScaleZ), new Point3D(CenterX, CenterY, CenterZ));
                    return result;
                }
            }
        }
    }
}
