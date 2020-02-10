using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Kit3D.Windows.Media.Media3D;

namespace Kit3DTest
{
    public static class TestHelper
    {
        /// <summary>
        /// Returns true if the two vectors are considered to be equal within
        /// a certain tolerance level
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool AreEqual(Vector3D v1, Vector3D v2)
        {
            return MathHelper.AreEqual(v1.X, v2.X) &&
                   MathHelper.AreEqual(v1.Y, v2.Y) &&
                   MathHelper.AreEqual(v1.Z, v2.Z);
        }
    }
}
