using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kit3D.Windows.Media.Media3D;

namespace Kit3DTest
{
    /// <summary>
    /// Tests all of the functionality of the Quaternion struct
    /// </summary>
    [TestClass]
    public class QuaternionTest
    {
        /// <summary>
        /// Verifies the default constructor works as expected and setting
        /// basic properties updates the Quaternion accordingly
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            Quaternion q = new Quaternion();
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion();
            q.X = 55;
            Assert.AreEqual<double>(55, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(177.9167466479, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(1, 0, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion();
            q.Y = 66;
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(66, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(178.263897100, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion();
            q.Z = 77;
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(77, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(178.511881594, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 0, 1), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion();
            q.W = 88;
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(88, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");
        }

        /// <summary>
        /// Tests the (x,y,z,w) constructor
        /// </summary>
        [TestMethod]
        public void Constructor1Test()
        {
            Quaternion q = new Quaternion(10, 20, 40, 720);
            Assert.AreEqual<double>(10, q.X, "q.X");
            Assert.AreEqual<double>(20, q.Y, "q.Y");
            Assert.AreEqual<double>(40, q.Z, "q.Z");
            Assert.AreEqual<double>(720, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(7.283571272, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.218217890235992, 0.436435780471985, 0.87287156094397), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion(0, 0, 0, 0);
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(0, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion(0, 0, 0, 1);
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");
        }

        /// <summary>
        /// Verifies the (Vector3D, double) constructor works as expected
        /// </summary>
        [TestMethod]
        public void Constructor2Test()
        {
            Quaternion q = new Quaternion(new Vector3D(101.5, 22.9, 87.33), 1050.5);
            Assert.IsTrue(MathHelper.AreEqual(0.19023571324430275, q.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(0.042920175697483083, q.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(0.16367768312930994, q.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(-0.96704593891394308, q.W), "q.W");
            Assert.IsTrue(MathHelper.AreEqual(330.5, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.747188757136826, 0.168577561955008, 0.642876789761172), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");
        }

        /// <summary>
        /// Verifies the Quaternion.Identity property works as expected.
        /// </summary>
        [TestMethod]
        public void IdentityTest()
        {
            Quaternion q = Quaternion.Identity;
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");
        }

        /// <summary>
        /// Verifies that the Normalize method works correctly
        /// </summary>
        [TestMethod]
        public void NormalizeTest()
        {
            Quaternion q = new Quaternion(15, 30, 99, 742);
            Assert.AreEqual<double>(15, q.X, "q.X");
            Assert.AreEqual<double>(30, q.Y, "q.Y");
            Assert.AreEqual<double>(99, q.Z, "q.Z");
            Assert.AreEqual<double>(742, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(16.0372853572, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.143502894482573, 0.287005788965146, 0.947119103584981), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q.Normalize();
            Assert.IsTrue(MathHelper.AreEqual(0.020017979771941304, q.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(0.040035959543882609, q.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(0.1321186664948126, q.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(0.99022273271869654, q.W), "q.W");
            Assert.IsTrue(MathHelper.AreEqual(16.037285357, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.143502894482573, 0.287005788965146, 0.947119103584981), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion();
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");

            q.Normalize();
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");
        }

        /// <summary>
        /// Verifies the conjugate method.
        /// </summary>
        [TestMethod]
        public void ConjugateTest()
        {
            Quaternion q = new Quaternion();
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");

            q.Conjugate();
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion(15, 30, 99, 742);
            Assert.AreEqual<double>(15, q.X, "q.X");
            Assert.AreEqual<double>(30, q.Y, "q.Y");
            Assert.AreEqual<double>(99, q.Z, "q.Z");
            Assert.AreEqual<double>(742, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(16.0372853572, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.143502894482573, 0.287005788965146, 0.947119103584981), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q.Conjugate();
            Assert.AreEqual<double>(-15, q.X, "q.X");
            Assert.AreEqual<double>(-30, q.Y, "q.Y");
            Assert.AreEqual<double>(-99, q.Z, "q.Z");
            Assert.AreEqual<double>(742, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(16.0372853572, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(-0.143502894482573, -0.287005788965146, -0.947119103584981), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");
        }

        /// <summary>
        /// Verifies the invert method.
        /// </summary>
        [TestMethod]
        public void InvertTest()
        {
            Quaternion q = new Quaternion();
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");
            
            q.Invert();
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(1, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(true, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(true, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion(0, 0, 0, 0);
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(0, q.W, "q.W");
            Assert.AreEqual<double>(0, q.Angle, "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q.Invert();
            Assert.AreEqual<double>(double.NaN, q.X, "q.X");
            Assert.AreEqual<double>(double.NaN, q.Y, "q.Y");
            Assert.AreEqual<double>(double.NaN, q.Z, "q.Z");
            Assert.AreEqual<double>(double.NaN, q.W, "q.W");
            Assert.AreEqual<double>(double.NaN, q.Angle, "q.Angle");
            Assert.AreEqual<double>(double.NaN, q.Axis.X, "q.Axis.X");
            Assert.AreEqual<double>(double.NaN, q.Axis.Y, "q.Axis.Y");
            Assert.AreEqual<double>(double.NaN, q.Axis.Z, "q.Axis.Z");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = new Quaternion(15, 30, 99, 742);
            Assert.AreEqual<double>(15, q.X, "q.X");
            Assert.AreEqual<double>(30, q.Y, "q.Y");
            Assert.AreEqual<double>(99, q.Z, "q.Z");
            Assert.AreEqual<double>(742, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(16.0372853572, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.143502894482573, 0.287005788965146, 0.947119103584981), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q.Invert();
            Assert.IsTrue(MathHelper.AreEqual(-0.000026714634276656752, q.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(-0.000053429268553313505, q.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(-0.00017631658622593456, q.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(0.0013214839088852874, q.W), "q.W");
            Assert.IsTrue(MathHelper.AreEqual(16.0372853572, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(-0.143502894482573, -0.287005788965146, -0.947119103584981), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");
        }

        /// <summary>
        /// Verifies adding two Quaternions works correctly
        /// </summary>
        [TestMethod]
        public void AddTest()
        {
            Quaternion q1 = new Quaternion(20, 40, 90, 559);
            Quaternion q2 = new Quaternion(3, 77, 32, 1030);
            Quaternion q = q1 + q2;
            Assert.AreEqual<double>(23, q.X, "q.X");
            Assert.AreEqual<double>(117, q.Y, "q.Y");
            Assert.AreEqual<double>(122, q.Z, "q.Z");
            Assert.AreEqual<double>(1589, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(12.25546738794197, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.13482375476581, 0.685842578591294, 0.715152090496905), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Add(q1, q2);
            Assert.AreEqual<double>(23, q.X, "q.X");
            Assert.AreEqual<double>(117, q.Y, "q.Y");
            Assert.AreEqual<double>(122, q.Z, "q.Z");
            Assert.AreEqual<double>(1589, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(12.25546738794197, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.13482375476581, 0.685842578591294, 0.715152090496905), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q1 = new Quaternion();
            q2 = new Quaternion(3, 77, 32, 1030);
            q = q1 + q2;
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1031, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.25370172287144, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312,0.922834273751935,0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Add(q1, q2);
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1031, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.25370172287144, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312,0.922834273751935,0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q1 = new Quaternion();
            q2 = new Quaternion(3, 77, 32, 1030);
            q = q2 + q1;
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1031, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.25370172287144, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312, 0.922834273751935, 0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Add(q2, q1);
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1031, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.25370172287144, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312, 0.922834273751935, 0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q1 = new Quaternion();
            q2 = new Quaternion();
            q = q1 + q2;
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(2, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(0, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Add(q1, q2);
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(2, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(0, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");
        }

        /// <summary>
        /// Verifies subtracting two Quaternions works correctly
        /// </summary>
        [TestMethod]
        public void SubtractTest()
        {
            Quaternion q1 = new Quaternion(20, 40, 90, 559);
            Quaternion q2 = new Quaternion(3, 77, 32, 1030);
            Quaternion q = q1 - q2;
            Assert.AreEqual<double>(17, q.X, "q.X");
            Assert.AreEqual<double>(-37, q.Y, "q.Y");
            Assert.AreEqual<double>(58, q.Z, "q.Z");
            Assert.AreEqual<double>(-471, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(342.88706022926482, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.239889128778138,-0.522111633223005,0.818445262890117), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Subtract(q1, q2);
            Assert.AreEqual<double>(17, q.X, "q.X");
            Assert.AreEqual<double>(-37, q.Y, "q.Y");
            Assert.AreEqual<double>(58, q.Z, "q.Z");
            Assert.AreEqual<double>(-471, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(342.88706022926482, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.239889128778138,-0.522111633223005,0.818445262890117), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q1 = new Quaternion();
            q2 = new Quaternion(3, 77, 32, 1030);
            q = q1 - q2;
            Assert.AreEqual<double>(-3, q.X, "q.X");
            Assert.AreEqual<double>(-77, q.Y, "q.Y");
            Assert.AreEqual<double>(-32, q.Z, "q.Z");
            Assert.AreEqual<double>(-1029, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(350.72839077943036, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(-0.0359545820942312, -0.922834273751935, -0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Subtract(q1, q2);
            Assert.AreEqual<double>(-3, q.X, "q.X");
            Assert.AreEqual<double>(-77, q.Y, "q.Y");
            Assert.AreEqual<double>(-32, q.Z, "q.Z");
            Assert.AreEqual<double>(-1029, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(350.72839077943036, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(-0.0359545820942312, -0.922834273751935, -0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q1 = new Quaternion();
            q2 = new Quaternion(3, 77, 32, 1030);
            q = q2 - q1;
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1029, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.271609220569653, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312,0.922834273751935,0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Subtract(q2, q1);
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1029, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.271609220569653, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312,0.922834273751935,0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q1 = new Quaternion();
            q2 = new Quaternion();
            q = q1 - q2;
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(0, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(0, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Subtract(q1, q2);
            Assert.AreEqual<double>(0, q.X, "q.X");
            Assert.AreEqual<double>(0, q.Y, "q.Y");
            Assert.AreEqual<double>(0, q.Z, "q.Z");
            Assert.AreEqual<double>(0, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(0, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0, 1, 0), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");
        }

        [TestMethod]
        public void MultiplyTest()
        {
            Quaternion q1 = new Quaternion(12, 99, -55, 1192);
            Quaternion q2 = new Quaternion(3, 77, 32, 1030);
            Quaternion q = q1 * q2;
            Assert.AreEqual<double>(23339.0, q.X, "q.X");
            Assert.AreEqual<double>(193205.0, q.Y, "q.Y");
            Assert.AreEqual<double>(-17879.0, q.Z, "q.Z");
            Assert.AreEqual<double>(1221861.0, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(18.17427678772107, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.119424374497594, 0.988619318514407, -0.0914858559339514), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Multiply(q1, q2);
            Assert.AreEqual<double>(23339.0, q.X, "q.X");
            Assert.AreEqual<double>(193205.0, q.Y, "q.Y");
            Assert.AreEqual<double>(-17879.0, q.Z, "q.Z");
            Assert.AreEqual<double>(1221861.0, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(18.17427678772107, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.119424374497594, 0.988619318514407, -0.0914858559339514), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q1 = new Quaternion(12, 99, -55, 1192);
            q2 = new Quaternion(3, 77, 32, 1030);
            q = q2 * q1;
            Assert.AreEqual<double>(8533.0, q.X, "q.X");
            Assert.AreEqual<double>(194303.0, q.Y, "q.Y");
            Assert.AreEqual<double>(-19133.0, q.Z, "q.Z");
            Assert.AreEqual<double>(1221861.0, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(18.17427678772107, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0436628899090781,0.994237723895887,-0.0979025047029639), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Multiply(q2, q1);
            Assert.AreEqual<double>(8533.0, q.X, "q.X");
            Assert.AreEqual<double>(194303.0, q.Y, "q.Y");
            Assert.AreEqual<double>(-19133.0, q.Z, "q.Z");
            Assert.AreEqual<double>(1221861.0, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(18.17427678772107, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0436628899090781,0.994237723895887,-0.0979025047029639), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q1 = new Quaternion();
            q2 = new Quaternion(3, 77, 32, 1030);
            q = q1 * q2;
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1030, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.2626468354348876, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312,0.922834273751935,0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Multiply(q1, q2);
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1030, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.2626468354348876, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312,0.922834273751935,0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = q2 * q1;
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1030, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.2626468354348876, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312,0.922834273751935,0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Multiply(q2, q1);
            Assert.AreEqual<double>(3, q.X, "q.X");
            Assert.AreEqual<double>(77, q.Y, "q.Y");
            Assert.AreEqual<double>(32, q.Z, "q.Z");
            Assert.AreEqual<double>(1030, q.W, "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.2626468354348876, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312, 0.922834273751935, 0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");
        }

        [TestMethod]
        public void SlerpTest()
        {
            Quaternion q1 = new Quaternion(97, 13, 101, 1220);
            Quaternion q2 = new Quaternion(3, 77, 32, 1030);
            Quaternion q = Quaternion.Slerp(q1, q2, 0);
            Assert.IsTrue(MathHelper.AreEqual(96.999999999999886, q.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(12.999999999999984, q.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(100.99999999999987, q.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(1219.9999999999984, q.W), "q.W");
            Assert.IsTrue(MathHelper.AreEqual(13.151709428599014, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.689714837784379,0.0924360091875972,0.718156686765178), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Slerp(q1, q2, 1);
            Assert.IsTrue(MathHelper.AreEqual(2.999999999999996, q.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(76.9999999999999, q.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(31.999999999999961, q.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(1029.9999999999986, q.W), "q.W");
            Assert.IsTrue(MathHelper.AreEqual(9.2626468354348876, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.0359545820942312,0.922834273751935,0.383515542338467), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            q = Quaternion.Slerp(q1, q2, 0.693);
            Assert.IsTrue(MathHelper.AreEqual(28.666022426867155, q.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(59.874000377073685, q.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(50.9714220915683, q.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(1086.3957796470379, q.W), "q.W");
            Assert.IsTrue(MathHelper.AreEqual(8.81055088696762, q.Angle), "q.Angle");
            Assert.IsTrue(TestHelper.AreEqual(new Vector3D(0.342508981812873,0.715389899611416,0.609019612911996), q.Axis), "q.Axis");
            Assert.AreEqual<bool>(false, q.IsIdentity, "q.IsIdentity");
            Assert.AreEqual<bool>(false, q.IsNormalized, "q.IsNormalized");

            //Make sure we have the shortest distance always
            Quaternion from = new Quaternion(0, 1, 0, 0);
            Quaternion to45 = new Quaternion(0.707, 0.707, 0, 0);
            Quaternion to90 = new Quaternion(1, 0, 0, 0);
            Quaternion to135 = new Quaternion(0.707, -0.707, 0, 0);
            Quaternion to180 = new Quaternion(0, -1, 0, 0);
            Quaternion to225 = new Quaternion(-0.707, -0.707, 0, 0);
            Quaternion to315 = new Quaternion(-1, 0, 0, 0);

            Quaternion to45Half = Quaternion.Slerp(from, to45, 0.5, true);
            Quaternion to90Half = Quaternion.Slerp(from, to90, 0.5, true);
            Quaternion to135Half = Quaternion.Slerp(from, to135, 0.5, true);
            Quaternion to180Half = Quaternion.Slerp(from, to180, 0.5, true);
            Quaternion to225Half = Quaternion.Slerp(from, to225, 0.5, true);
            Quaternion to315Half = Quaternion.Slerp(from, to315, 0.5, true);

            Assert.IsTrue(MathHelper.AreEqual(0.382654536493283, to45Half.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(0.923809771705674, to45Half.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(0, to45Half.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(0, to45Half.W), "q.W");

            Assert.IsTrue(MathHelper.AreEqual(0.707106781186547, to90Half.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(0.707106781186547, to90Half.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(0, to90Half.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(0, to90Half.W), "q.W");

            Assert.IsTrue(MathHelper.AreEqual(-0.382654536493283, to135Half.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(0.923809771705674, to135Half.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(0, to135Half.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(0, to135Half.W), "q.W");

            Assert.IsTrue(MathHelper.AreEqual(0, to180Half.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(1, to180Half.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(0, to180Half.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(0, to180Half.W), "q.W");

            Assert.IsTrue(MathHelper.AreEqual(0.382654536493283, to225Half.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(0.923809771705674, to225Half.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(0, to225Half.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(0, to225Half.W), "q.W");

            Assert.IsTrue(MathHelper.AreEqual(-0.707106781186547, to315Half.X), "q.X");
            Assert.IsTrue(MathHelper.AreEqual(0.707106781186547, to315Half.Y), "q.Y");
            Assert.IsTrue(MathHelper.AreEqual(0, to315Half.Z), "q.Z");
            Assert.IsTrue(MathHelper.AreEqual(0, to315Half.W), "q.W");
            /*
            to45Half.ToString()
            "0.382654536493283,0.923809771705674,0,0"
            to90Half.ToString()
            "0.707106781186547,0.707106781186547,0,0"
            to135Half.ToString()
            "-0.382654536493283,0.923809771705674,0,0"
            to180Half.ToString()
            "0,1,0,0"
            to225Half.ToString()
            "0.382654536493283,0.923809771705674,0,0"
            to315Half.ToString()
            "-0.707106781186547,0.707106781186547,0,0"
             * */
        }

        /// <summary>
        /// Verifies that a RotationTransform3D which has a Quaternion set for the 
        /// Rotation properties behaves correctly
        /// </summary>
        [TestMethod]
        public void RotateTransform3DQuaternionTest()
        {
            RotateTransform3D rt = new RotateTransform3D();
            rt.Rotation = new QuaternionRotation3D(new Quaternion(97, 13, 101, 1220));
            Matrix3D m = rt.Value;
            Assert.AreEqual<double>(-20739.0, m.M11, "m.M11");
            Assert.AreEqual<double>(248962.0, m.M12, "m.M12");
            Assert.AreEqual<double>(-12126.0, m.M13, "m.M13");
            Assert.AreEqual<double>(0.0, m.M14, "m.M14");
            Assert.AreEqual<double>(-243918.0, m.M21, "m.M21");
            Assert.AreEqual<double>(-39219.0, m.M22, "m.M22");
            Assert.AreEqual<double>(239306.0, m.M23, "m.M23");
            Assert.AreEqual<double>(0.0, m.M24, "m.M24");
            Assert.AreEqual<double>(51314.0, m.M31, "m.M31");
            Assert.AreEqual<double>(-234054.0, m.M32, "m.M32");
            Assert.AreEqual<double>(-19155.0, m.M33, "m.M33");
            Assert.AreEqual<double>(0.0, m.M34, "m.M34");
            Assert.AreEqual<double>(0.0, m.OffsetX, "m.OffsetX");
            Assert.AreEqual<double>(0.0, m.OffsetY, "m.OffsetY");
            Assert.AreEqual<double>(0.0, m.OffsetZ, "m.OffsetZ");
            Assert.AreEqual<double>(1.0, m.M44, "m.M44");

            rt = new RotateTransform3D();
            rt.CenterX = 121;
            rt.CenterY = -99;
            rt.CenterZ = 555;
            rt.Rotation = new QuaternionRotation3D(new Quaternion(97, 13, 101, 1220));
            m = rt.Value;
            Assert.AreEqual<double>(-20739.0, m.M11, "m.M11");
            Assert.AreEqual<double>(248962.0, m.M12, "m.M12");
            Assert.AreEqual<double>(-12126.0, m.M13, "m.M13");
            Assert.AreEqual<double>(0.0, m.M14, "m.M14");
            Assert.AreEqual<double>(-243918.0, m.M21, "m.M21");
            Assert.AreEqual<double>(-39219.0, m.M22, "m.M22");
            Assert.AreEqual<double>(239306.0, m.M23, "m.M23");
            Assert.AreEqual<double>(0.0, m.M24, "m.M24");
            Assert.AreEqual<double>(51314.0, m.M31, "m.M31");
            Assert.AreEqual<double>(-234054.0, m.M32, "m.M32");
            Assert.AreEqual<double>(-19155.0, m.M33, "m.M33");
            Assert.AreEqual<double>(0.0, m.M34, "m.M34");
            Assert.AreEqual<double>(-50117612.0, m.OffsetX, "m.OffsetX");
            Assert.AreEqual<double>(95892788.0, m.OffsetY, "m.OffsetY");
            Assert.AreEqual<double>(35790120.0, m.OffsetZ, "m.OffsetZ");
            Assert.AreEqual<double>(1.0, m.M44, "m.M44");

            rt = new RotateTransform3D();
            rt.Rotation = new QuaternionRotation3D(new Quaternion(97, 13, 101, 0));
            m = rt.Value;
            Assert.AreEqual<double>(-20739.0, m.M11, "m.M11");
            Assert.AreEqual<double>(2522.0, m.M12, "m.M12");
            Assert.AreEqual<double>(19594.0, m.M13, "m.M13");
            Assert.AreEqual<double>(0.0, m.M14, "m.M14");
            Assert.AreEqual<double>(2522.0, m.M21, "m.M21");
            Assert.AreEqual<double>(-39219.0, m.M22, "m.M22");
            Assert.AreEqual<double>(2626.0, m.M23, "m.M23");
            Assert.AreEqual<double>(0.0, m.M24, "m.M24");
            Assert.AreEqual<double>(19594.0, m.M31, "m.M31");
            Assert.AreEqual<double>(2626.0, m.M32, "m.M32");
            Assert.AreEqual<double>(-19155.0, m.M33, "m.M33");
            Assert.AreEqual<double>(0.0, m.M34, "m.M34");
            Assert.AreEqual<double>(0.0, m.OffsetX, "m.OffsetX");
            Assert.AreEqual<double>(0.0, m.OffsetY, "m.OffsetY");
            Assert.AreEqual<double>(0.0, m.OffsetZ, "m.OffsetZ");
            Assert.AreEqual<double>(1.0, m.M44, "m.M44");
        }
    }
}
