namespace Kit3D.Windows.Media.Media3D
{
    using System;
    using System.Windows.Media.Imaging;
    using System.Diagnostics;
    using System.Windows.Media;
    using System.Windows;

    public sealed class DiffuseMaterial : Material
    {
        private Kit3DBrush brush;
        private MatrixTransform brushTransform = new MatrixTransform();

        public DiffuseMaterial()
        {}

        public DiffuseMaterial(Kit3DBrush brush)
        {
            this.Brush = brush;
        }

        internal override Material Clone()
        {
            if (this.Brush == null)
            {
                return new DiffuseMaterial();
            }
            else
            {
                return new DiffuseMaterial(this.Brush.Clone());
            }
        }

        public Kit3DBrush Brush
        {
            get
            {
                return this.brush;
            }
            set
            {
                //TODO: If we update the brush's properties, should the geometry model
                //get notified and update all of the brushes as well?

                this.brush = value;
            }
        }

        //TODO: Remove
        bool isDegenerate = false;

        internal override void SetScreenValues(Triangle triangle,
                                               double x0, double y0,
                                               double x1, double y1,
                                               double x2, double y2)
        {
            if (isDegenerate)
            {
                return;
            }

            //If the model is not using an image brush then no need to perform all these extra
            //calculations.

            Point uv0;
            Point uv1;
            Point uv2;
            if (!(this.Brush.Brush is ImageBrush))
            {
                return;
            }
            else
            {
                uv0 = triangle.UV0;
                uv1 = triangle.UV1;
                uv2 = triangle.UV2;
            }

            //x0, y0, x1, y1, x2, y2 are the projected screen co-ordinates of the shape this
            //texture maps to

            //Need to convert the UV values of the vertices into texture space.
            //The U,V values are stored in the X,Y properties of the point
            //Point uv0 = triangle.UV0;
            //Point uv1 = triangle.UV1;
            //Point uv2 = triangle.UV2;
            Point3D v0Tex = new Point3D(uv0.X * this.brush.Width, uv0.Y * this.brush.Height, 1);
            Point3D v1Tex = new Point3D(uv1.X * this.brush.Width, uv1.Y * this.brush.Height, 1);
            Point3D v2Tex = new Point3D(uv2.X * this.brush.Width, uv2.Y * this.brush.Height, 1);

            //TODO: This can be cached
            Matrix3D textureInverse = new Matrix3D((v2Tex.X - v0Tex.X), (v2Tex.Y - v0Tex.Y), 0, 0,
                                                   (v1Tex.X - v0Tex.X), (v1Tex.Y - v0Tex.Y), 0, 0,
                                                   v0Tex.X, v0Tex.Y, 1, 0,
                                                   0, 0, 0, 1);
            textureInverse.Invert();

            //Now we have to map the texture co-ordinates of the triangle that the
            //texture is associated with to the projected screen co-ordinates

            double minX = (x0 < x1) ? x0 : x1;
            if(minX > x2)
            {
                minX = x2;
            }
            double minY = (y0 < y1) ? y0 : y1;
            if(minY > y2)
            {
                minY = y2;
            }

            double x0MinusMinX = x0 - minX;
            double y0MinusMinY = y0 - minY;
            double p2MinusP0X = x2 - x0MinusMinX - minX;
            double p2MinusP0Y = y2 - y0MinusMinY - minY;
            double p1MinusP0X = x1 - x0MinusMinX - minX;
            double p1MinusP0Y = y1 - y0MinusMinY - minY;

            Matrix3D screenMatrix = new Matrix3D(p2MinusP0X, p2MinusP0Y, 0, 0,
                                                 p1MinusP0X, p1MinusP0Y, 0, 0,
                                                 x0MinusMinX, y0MinusMinY, 1, 0,
                                                 0, 0, 0, 1);
            Matrix3D textToPoly = textureInverse * screenMatrix;

            //Update the matrix associated with the image brush, this will give
            //us an affine texture mapping
            Matrix matrix = new Matrix();
            matrix.M11 = textToPoly.M11;
            matrix.M12 = textToPoly.M12;
            matrix.M21 = textToPoly.M21;
            matrix.M22 = textToPoly.M22;
            matrix.OffsetX = textToPoly.M31;
            matrix.OffsetY = textToPoly.M32;

            this.brushTransform.Matrix = matrix;
            this.Brush.Brush.Transform = this.brushTransform;
        }
    }
}
