namespace Kit3D.Windows.Media
{
    using System;
    using System.Windows;

    public class PointHitTestParameters : HitTestParameters
    {
        public PointHitTestParameters(Point point)
        {
            this.HitPoint = point;
        }

        public Point HitPoint
        {
            get;
            private set;
        }
    }
}

