/*
 * Copyright Mark Dawson 2008 - http://www.markdawson.org/kit3D
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Kit3D.Windows.Media.Media3D
{
    //TODO:When things change raise events
    public sealed class MatrixTransform3D : Transform3D
    {
        private Matrix3D matrix;

        public MatrixTransform3D()
        {
            this.matrix = Matrix3D.Identity;
        }

        public MatrixTransform3D(Matrix3D matrix)
        {
            this.matrix = matrix;
        }

        public Matrix3D Matrix
        {
            get { return this.matrix; }
            set
            {
                this.matrix = value;
                OnChanged();
            }
        }

        public override Matrix3D Value
        {
            get { return this.matrix; }
        }
    }
}
