namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using Kit3D.Windows.Media.Animation;
    using System.Collections.Generic;
    using Kit3D.Math;
    using Kit3D.Windows.Controls;

    public abstract class Geometry3D : Animatable
    {
        /// <summary>
        /// The type of change in the geometry that just took place
        /// </summary>
        public enum GeometryChangeType
        {
            Positions,
            Texturecoordinates,
            TriangleIndices,
            GeometrySet
        }

        internal delegate void GeometryChangedEventHandler(GeometryChangeType changeType);
        internal event GeometryChangedEventHandler GeometryChanged;

        protected Geometry3D()
        {
        }

        internal abstract List<Triangle> CreateTriangles(Material material,
                                                         Material backMaterial,
                                                         bool materialChanged);

        //TODO: How to get rid of this method?  Needed currently by
        //the ModelGeometry3D class
        internal abstract Point3DCollection PositionsInternal
        {
            get;
        }

        /// <summary>
        /// Returns a Rect3D that contains the bounds of the object
        /// in local space
        /// </summary>
        internal abstract Rect3D LocalBounds
        {
            get;
        }

        /// <summary>
        /// Checks for intersection between the ray and the model
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        internal abstract void IntersectsWith(ModelVisual3D visual, GeometryModel3D model, Ray ray, List<HitResult> results);

        protected void OnGeometryChanged(GeometryChangeType changeType)
        {
            GeometryChangedEventHandler handler = GeometryChanged;
            if (handler != null)
            {
                handler(changeType);
            }
        }
    }
}
