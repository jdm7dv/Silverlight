namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using Kit3D.Windows.Media.Animation;

    public abstract class Material : Animatable
    {
        /// <summary>
        /// Called to update the position of the triangles on the screen
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        internal abstract void SetScreenValues(Triangle triangle,
                                               double x0, double y0,
                                               double x1, double y1,
                                               double x2, double y2);

        internal abstract Material Clone();
    }
}
