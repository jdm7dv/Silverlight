namespace Kit3D.Windows.Media.Media3D
{
    using System;

    public abstract class RayHitTestResult : HitTestResult
    {
        internal RayHitTestResult(ModelVisual3D visualHit, Model3D modelHit)
            : base(visualHit)
        {
            this.ModelHit = modelHit;
        }

        public Model3D ModelHit
        {
            get;
            private set;
        }
    }
}
