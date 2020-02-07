namespace Kit3D.Windows.Controls
{
    using System;
    using Kit3D.Windows.Media.Media3D;

    /// <summary>
    /// Internal helper class for hit testing
    /// </summary>
    internal struct HitResult
    {
        public ModelVisual3D Visual;
        public GeometryModel3D Model;
        public MeshGeometry3D Mesh;
        public Point3D Point;
        public double DistanceToRayOrigin;
        public int VertexIndex1;
        public int VertexIndex2;
        public int VertexIndex3;
    }
}
