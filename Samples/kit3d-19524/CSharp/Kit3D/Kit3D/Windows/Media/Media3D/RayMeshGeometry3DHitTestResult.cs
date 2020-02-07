namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using Kit3D.Windows.Controls;

    public class RayMeshGeometry3DHitTestResult : RayHitTestResult
    {
        internal RayMeshGeometry3DHitTestResult(HitResult hitResult)
            : base(hitResult.Visual, hitResult.Model)
        {
            this.MeshHit = hitResult.Mesh;
            this.DistanceToRayOrigin = hitResult.DistanceToRayOrigin;
            this.VertexIndex1 = hitResult.VertexIndex1;
            this.VertexIndex2 = hitResult.VertexIndex2;
            this.VertexIndex3 = hitResult.VertexIndex3;
            this.PointHit = hitResult.Point;
        }

        public double DistanceToRayOrigin
        {
            get;
            private set;
        }

        public MeshGeometry3D MeshHit
        {
            get;
            private set;
        }

        public Point3D PointHit
        {
            get;
            private set;
        }

        public int VertexIndex1
        {
            get;
            private set;
        }

        public int VertexIndex2
        {
            get;
            private set;
        }

        public int VertexIndex3
        {
            get;
            private set;
        }
    }
}
