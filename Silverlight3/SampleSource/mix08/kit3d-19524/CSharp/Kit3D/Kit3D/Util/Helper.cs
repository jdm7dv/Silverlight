namespace Kit3D.Util
{
    using System;
    using Kit3D.Windows.Media.Media3D;
    using System.Windows.Media;

    /// <summary>
    /// Contains some methods for helping with common tasks
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Given a Rect3D instance, returns a GeometryModel3D that represents
        /// the Rect3D
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static GeometryModel3D BoundingModel(Rect3D rect, Color color)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection
            {
                new Point3D(rect.Location),
                new Point3D(rect.X + rect.SizeX, rect.Y, rect.Z),
                new Point3D(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z),
                new Point3D(rect.X, rect.Y + rect.SizeY, rect.Z),

                new Point3D(rect.X, rect.Y, rect.Z + rect.SizeZ),
                new Point3D(rect.X + rect.SizeX, rect.Y, rect.Z + rect.SizeZ),
                new Point3D(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z + rect.SizeZ),
                new Point3D(rect.X, rect.Y + rect.SizeY, rect.Z + rect.SizeZ),
            };

            mesh.TriangleIndices = new Kit3D.Windows.Media.Int32Collection
            {
                //Front face
                4,5,6,
                4,6,7,

                
                //Back Face
                1,0,6,
                1,6,2,

                //Right Face
                5,1,2,
                5,2,6,

                //Left Face
                0,4,7,
                0,7,3,

                //Top Face
                6,2,3,
                6,3,7,

                //Bottom Face
                5,4,0,
                5,0,1
                 
            };

            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = mesh;
            model.Material = new DiffuseMaterial(new Kit3DBrush(new SolidColorBrush(color)));

            return model;
        }
    }
}
