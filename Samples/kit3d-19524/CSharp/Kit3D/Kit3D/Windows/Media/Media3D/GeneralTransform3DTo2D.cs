namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Windows;

    public sealed class GeneralTransform3DTo2D : Freezable
    {
        internal GeneralTransform3DTo2D(Matrix3D transform)
        {
            this.Matrix = transform;
        }

        public Point Transform(Point3D point)
        {
            Point3D p = this.Matrix.Transform(point);
            return new Point(p.X, p.Y);
        }

        private Matrix3D Matrix
        {
            get;
            set;
        }
    }
}
