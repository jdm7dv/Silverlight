namespace Kit3D.Windows.Media
{
    using System;
    using System.Windows;

    /// <summary>
    /// Not supported, just here for completeness in HitTest method signature
    /// </summary>
    /// <param name="potentialHitTestTarget"></param>
    /// <returns></returns>
    public delegate HitTestFilterBehavior HitTestFilterCallback(DependencyObject potentialHitTestTarget);
}

