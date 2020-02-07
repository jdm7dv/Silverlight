namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public sealed class AxisAngleRotation3D : Rotation3D
    {
        //TODO: What are the defaults for this?
        private Vector3D axis = new Vector3D(0, 1, 0);
        private double angle;

        public AxisAngleRotation3D()
        {
        }

        public AxisAngleRotation3D(Vector3D axis, double angle)
        {
            this.Axis = axis;
            this.Angle = angle;
        }

        public Vector3D Axis
        {
            get
            {
                return this.axis;
            }
            set
            {
                this.axis = value;
                OnChanged();
            }
        }

        public double Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
                OnChanged();
            }
        }

        internal override Matrix3D GetMatrix(Point3D center)
        {
            //Calculations from Essential Mathematics for games & interactive applications
            //James M. Van Verth & Lars M. Bishop

            //TODO:Cache this:
            Vector3D a = Axis;
            a.Normalize();

            //TODO: Internally use a Quaternion since it is more efficient to calculate

            //TODO: Look into caching all this
            double angle = (Angle % 360) * System.Math.PI / 180.0;
            double t = 1.0 - System.Math.Cos(angle);
            double c = System.Math.Cos(angle);
            double s = System.Math.Sin(angle);
            double x = a.X;
            double y = a.Y;
            double z = a.Z;

            //TODO: Optimize, do not multiple matrices, just expand out terms
            Matrix3D m = new Matrix3D(1, 0, 0, 0,
                                      0, 1, 0, 0,
                                      0, 0, 1, 0,
                                      -center.X, -center.Y, -center.Z, 1);

            m.Append(new Matrix3D(t * x * x + c, t * x * y + s * z, t * x * z - s * y, 0,
                                  t * x * y - s * z, t * y * y + c, t * y * z + s * x, 0,
                                  t * x * z + s * y, t * y * z - s * x, t * z * z + c, 0,
                                  0, 0, 0, 1));

            m.Append(new Matrix3D(1, 0, 0, 0,
                                  0, 1, 0, 0,
                                  0, 0, 1, 0,
                                  center.X, center.Y, center.Z, 1));

            return m;
        }
    }
}
