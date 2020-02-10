namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public class RotateTransform3D : Transform3D
    {
        private double centerX, centerY, centerZ;
        private Rotation3D rotation;

        public RotateTransform3D()
        {
            this.Rotation = new AxisAngleRotation3D();
        }

        public RotateTransform3D(Rotation3D rotation)
        {
            this.Rotation = rotation;
        }

        public RotateTransform3D(Rotation3D rotation, Point3D point)
        {
            this.Rotation = rotation;
            this.centerX = point.X;
            this.centerY = point.Y;
            this.centerZ = point.Z;
        }

        public RotateTransform3D(Rotation3D rotation, double centerX, double centerY, double centerZ)
        {
            this.Rotation = rotation;
            this.centerX = centerX;
            this.centerY = centerY;
            this.centerZ = centerZ;
        }

        public double CenterX
        {
            get
            {
                return this.centerX;
            }
            set
            {
                this.centerX = value;
                OnChanged();
            }
        }

        public double CenterY
        {
            get
            {
                return this.centerY;
            }
            set
            {
                this.centerY = value;
                OnChanged();
            }
        }

        public double CenterZ
        {
            get
            {
                return this.centerZ;
            }
            set
            {
                this.centerZ = value;
                OnChanged();
            }
        }

        public Rotation3D Rotation
        {
            get { return this.rotation; }
            set
            {
                if (this.rotation != null)
                {
                    this.rotation.Changed -= new EventHandler(rotation_Changed);
                }
                this.rotation = value;
                if (this.rotation != null)
                {
                    this.rotation.Changed += new EventHandler(rotation_Changed);
                }
                OnChanged();
            }
        }

        void rotation_Changed(object sender, EventArgs e)
        {
            OnChanged();
        }

        public override Matrix3D Value
        {
            get
            {
                if (this.Rotation == null)
                {
                    //TODO: Verify this is the behaviour of WPF
                    return Matrix3D.Identity;
                }

                return this.Rotation.GetMatrix(new Point3D(CenterX, CenterY, CenterZ));
            }
        }
    }
}
