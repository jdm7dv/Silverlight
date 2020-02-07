namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using Kit3D.Windows.Media.Animation;

    public abstract class Rotation3D : Animatable
    {
        internal abstract Matrix3D GetMatrix(Point3D center);
    }
}
