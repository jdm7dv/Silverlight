namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public abstract class Transform3D : GeneralTransform3D
    {
        public abstract Matrix3D Value
        {
            get;
        }
    }
}
