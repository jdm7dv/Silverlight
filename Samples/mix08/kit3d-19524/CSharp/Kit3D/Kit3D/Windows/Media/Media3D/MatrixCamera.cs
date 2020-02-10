/*
 * Copyright Mark Dawson 2008 - http://www.markdawson.org/kit3D
*/

using System;

namespace Kit3D.Windows.Media.Media3D
{
    /// <summary>
    /// Represents a matrix camera
    /// </summary>
    public sealed class MatrixCamera : Camera
    {
        private Matrix3D viewMatrix;
        private Matrix3D projectionMatrix;
        private Matrix3D valueMatrix;
        private Point3D position;
        private Matrix3D inverseViewMatrix;
        private bool inverseViewMatrixIsDirty = true;
        private bool positionIsDirty = true;

        public MatrixCamera() :
            this(Matrix3D.Identity, Matrix3D.Identity)
        {}

        public MatrixCamera(Matrix3D viewMatrix, Matrix3D projectionMatrix)
        {
            this.ViewMatrix = viewMatrix;
            this.ProjectionMatrix = projectionMatrix;
            this.ValueMatrixIsDirty = true;
        }

        public Matrix3D ViewMatrix
        {
            get{ return this.viewMatrix; }
            set
            {
                this.viewMatrix = value;

                this.inverseViewMatrixIsDirty = true;
                this.positionIsDirty = true;
                this.ValueMatrixIsDirty = true;

                //Signal camera properties have changed, things like
                //the viewport will be listening to know they have to
                //redraw themselves.
                OnChanged();
            }
        }

        internal Matrix3D InverseViewMatrix
        {
            get
            {
                if (this.inverseViewMatrixIsDirty)
                {
                    this.inverseViewMatrix = this.ViewMatrix;
                    this.inverseViewMatrix.Invert();

                    this.inverseViewMatrixIsDirty = false;
                    this.ValueMatrixIsDirty = true;
                }
                return this.inverseViewMatrix;
            }
        }

        internal Point3D Position
        {
            get
            {
                if (this.positionIsDirty)
                {
                    Matrix3D m = this.InverseViewMatrix;
                    this.position = m.Transform(new Point3D(0, 0, 0));
                    this.positionIsDirty = false;
                }
                return this.position;
            }
        }

        public Matrix3D ProjectionMatrix
        {
            get{ return this.projectionMatrix; }
            set
            {
                this.projectionMatrix = value;
                this.ValueMatrixIsDirty = true;
                OnChanged();
            }
        }

        //HACK - remove this, not guaranteed to work, only with projection matrix
        internal double FieldOfView
        {
            get
            {
                return (360.0 * System.Math.Atan2(1, this.ProjectionMatrix.M11)) / System.Math.PI;
            }
        }

        internal override Matrix3D Value
        {
            get
            {
                if (this.ValueMatrixIsDirty)
                {
                    this.valueMatrix = this.ViewMatrix;
                    this.valueMatrix.Append(this.ProjectionMatrix);
                    this.ValueMatrixIsDirty = false;
                }
                return this.valueMatrix;
            }
        }

        /// <summary>
        /// If true indicates the value matrix needs to be recalculated
        /// </summary>
        private bool ValueMatrixIsDirty
        {
            get;
            set;
        }

        internal override void Invalidate()
        {
            //Nothing to do, don't rely on any external values
        }
    }
}
