namespace Kit3D.Windows.Media
{
    using System;
    using Kit3D.Windows.Media.Media3D;

    public abstract class HitTestResult
    {
        internal HitTestResult(ModelVisual3D visualHit)
        {
            this.VisualHit = visualHit;
        }

        //This should be a DependencyObject, but since we cannot inherit
        //from this class and we do not care about 2D we will just expose
        //this as a ModelVisual3D for now.
        public ModelVisual3D VisualHit
        {
            get;
            private set;
        }
    }
}

