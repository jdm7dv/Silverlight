using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kit3D.Windows.Media.Media3D;

namespace Kit3DTest
{
    [TestClass]
    public class QuaternionRotation3DTest
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            QuaternionRotation3D qr = new QuaternionRotation3D();
            Quaternion q = qr.Quaternion;

            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");
        }

        [TestMethod]
        public void Constructor1Test()
        {
            QuaternionRotation3D qr = new QuaternionRotation3D(new Quaternion(10, 20, 30, 405));
            Quaternion q = qr.Quaternion;
            Assert.AreEqual<double>(10, q.X, "q.X");
            Assert.AreEqual<double>(20, q.Y, "q.Y");
            Assert.AreEqual<double>(30, q.Z, "q.Z");
            Assert.AreEqual<double>(405, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(10.556757854207119, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.267261241912424, 0.534522483824849, 0.801783725737273), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");
        }

        [TestMethod]
        public void QuaternionPropertyTest()
        {
            QuaternionRotation3D qr = new QuaternionRotation3D();
            Quaternion q = new Quaternion(10, 40, 90, 120);
            qr.Quaternion = q;

            Assert.AreEqual<double>(q.X, qr.Quaternion.X, "q.X");
            Assert.AreEqual<double>(q.Y, qr.Quaternion.Y, "q.Y");
            Assert.AreEqual<double>(q.Z, qr.Quaternion.Z, "q.Z");
            Assert.AreEqual<double>(q.W, qr.Quaternion.W, "q.W");
            Assert.AreEqual<double>(q.Angle, qr.Quaternion.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(q.Axis, qr.Quaternion.Axis), "q.Axis");
            Assert.AreEqual<bool>(q.IsIdentity, qr.Quaternion.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(q.IsNormalized, qr.Quaternion.IsNormalized, "q.IsNormalized");
        }
    }
}
