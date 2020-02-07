using System;
using System.Collections.Generic;
using System.Text;

/// This file implements (at least partially) the various mathematical objects in DirectX such as
/// Vector2,Vector3, Matrix and Plane.
/// This is to facilate the porting of code that uses DirectX math to non-DirectX environments
/// such as Silverlight or hosted Web Sites.
///
/// Written by Declan Brennan, 2007 (http://declan.brennan.name)
///
/// I am using the Microsoft.DirectX namespace for compatibility but this code has absolutely nothing to
/// do with Microsoft. It's all my own fault.
/// 
/// Made available under the GNU Lesser General Public License:
/// http://www.opensource.org/licenses/lgpl-license.php
///
/// Use with caution. It was written in a hurry. There are ommissions and there may be bugs
/// 
namespace Microsoft.DirectX
{
   /// <summary>
   /// You can find out about Quaternions here: http://en.wikipedia.org/wiki/Quaternion
   /// One of their main applications is the modeling of 3D rotations
   /// </summary>
   public struct Quaternion
   {
      float X, Y, Z, W;

      /// <summary>
      /// Construct a quaternion
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="z"></param>
      /// <param name="w"></param>
      public Quaternion(float x, float y, float z, float w)
      {
         X = x; Y = y; Z = z; W = w;
      }
      /// <summary>
      /// Make this quaterion unit length
      /// </summary>
      void Normalize()
      {
         float L = (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
         if (L > 1.0e-7)
         {
            X /= L; Y /= L; Z /= L; W /= L;
         }
      }
      /// <summary>
      /// The multiplicative Identity (1) for Quaterions
      /// </summary>
      public static Quaternion Identity
      {
         get
         {
            return new Quaternion(0f, 0f, 0f, 1f);
         }
      }
      /// <summary>
      /// The additive Identity (0) for Quaterniosn
      /// </summary>
      public static Quaternion Zero
      {
         get
         {
            return new Quaternion();
         }
      }
      /// <summary>
      /// The Grassmann Product of two quaternions
      /// </summary>
      /// <param name="q2"></param>
      /// <param name="q1"></param>
      /// <returns></returns>
      public static Quaternion operator *(Quaternion q2, Quaternion q1)
      {
         return new Quaternion(q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y,
                               q1.W * q2.Y + q1.Y * q2.W + q1.Z * q2.X - q1.X * q2.Z,
                               q1.W * q2.Z + q1.Z * q2.W + q1.X * q2.Y - q1.Y * q2.X,

                               q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z);
      }
      /// <summary>
      /// Return the quaternion representing a rotation about an arbitrary axis
      /// </summary>
      /// <param name="axis"></param>
      /// <param name="angle"></param>
      /// <returns></returns>
      public static Quaternion RotationAxis(Vector3 axis, float angle)
      {
         Vector3 a0 = axis; a0.Normalize();
         float halfSin = (float)Math.Sin(angle / 2f);
         float halfCos = (float)Math.Cos(angle / 2f);
         return new Quaternion(a0.X * halfSin, a0.Y * halfSin, a0.Z * halfSin, halfCos);
      }
      /// <summary>
      /// Return the rotation axis and angle represented by a quaternion
      /// </summary>
      /// <param name="q"></param>
      /// <returns></returns>
      public static AxisAngle ToAxisAngle(Quaternion q)
      {
         AxisAngle aa = new AxisAngle();
         aa.Angle = 2 * (float)Math.Acos(q.W);
         // float scale = (float)Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z);
         float scale = (float)Math.Sin(aa.Angle / 2);
         if (scale == 0)
            aa.Axis = new Vector3();
         else
            aa.Axis = new Vector3(q.X / scale, q.Y / scale, q.Z / scale);
         return aa;
      }
   }

   /// <summary>
   /// A three dimensional vector
   /// See http://en.wikipedia.org/wiki/Cross_product , http://en.wikipedia.org/wiki/Dot_product
   /// </summary>
   public struct Vector3
   {
      public float X, Y, Z;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="z"></param>
      public Vector3(float x, float y, float z)
      {
         X = x; Y = y; Z = z;
      }
      /// <summary>
      /// The Additive identity- Zero
      /// </summary>
      public static Vector3 Empty = new Vector3(0, 0, 0);
      /// <summary>
      /// Return the length of this vector
      /// </summary>
      /// <returns></returns>
      public float Length()
      {
         return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
      }
      /// <summary>
      /// Make this vector unit length
      /// </summary>
      public void Normalize()
      {
         float l = this.Length();
         if (l > 0)
         {
            X /= l;
            Y /= l;
            Z /= l;
         }
      }
      /// <summary>
      /// Return the square of the length of this vector
      /// </summary>
      /// <returns></returns>
      public float LengthSq()
      {
         return X * X + Y * Y + Z * Z;
      }
      /// <summary>
      /// Return the scalar dot product of two vectors
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static float DotProduct(Vector3 a, Vector3 b)
      {
         return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
      }
      /// <summary>
      /// Return the additive inverse of this vector
      /// </summary>
      /// <param name="v"></param>
      /// <returns></returns>
      public static Vector3 operator -(Vector3 v)
      {
         return new Vector3(-v.X, -v.Y, -v.Z);
      }
      /// <summary>
      /// Return sum of two vectors
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static Vector3 operator +(Vector3 a, Vector3 b)
      {
         return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
      }
      /// <summary>
      /// Subtract one vector from another
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static Vector3 operator -(Vector3 a, Vector3 b)
      {
         return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
      }
      /// <summary>
      /// Multiply a vector by a scalar
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static Vector3 operator *(Vector3 a, float b)
      {
         return new Vector3(a.X * b, a.Y * b, a.Z * b);
      }
      /// <summary>
      /// Multiply a scalar by a vector
      /// </summary>
      /// <param name="b"></param>
      /// <param name="a"></param>
      /// <returns></returns>
      public static Vector3 operator *(float b, Vector3 a)
      {
         return new Vector3(a.X * b, a.Y * b, a.Z * b);
      }
      /// <summary>
      /// Return the vector cross-product of two vectors
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static Vector3 CrossProduct(Vector3 a, Vector3 b)
      {
         return new Vector3(
              a.Y * b.Z - a.Z * b.Y,
              a.Z * b.X - a.X * b.Z,
              a.X * b.Y - a.Y * b.X
             );
      }
      /// <summary>
      /// Transform a vector by multiplying it by the supplied transformation matrix
      /// </summary>
      /// <param name="v"></param>
      /// <param name="m"></param>
      /// <returns></returns>
      public static Vector3 TransformCoordinate(Vector3 v, Matrix m)
      {
         float w = m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44;
         return new Vector3(
               (m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14) / w,
               (m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24) / w,
               (m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34) / w);
      }
      /// <summary>
      /// Tranform an array of vectors by multiplying it by the supplied transformation matrix
      /// </summary>
      /// <param name="vs"></param>
      /// <param name="m"></param>
      /// <returns></returns>
      public static Vector3[] TransformCoordinate(Vector3[] vs, Matrix m)
      {
         Vector3[] rc = new Vector3[vs.Length];
         int i = 0;
         foreach (Vector3 v in vs)
            rc[i++] = TransformCoordinate(v, m);
         return rc;
      }
   }

   /// <summary>
   /// A two dimensional vector
   /// </summary>
   public struct Vector2
   {
      public float X, Y;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      public Vector2(float x, float y)
      {
         X = x; Y = y;
      }
      /// <summary>
      /// Calculcate the hash code for this instance
      /// </summary>
      /// <returns>The hash code for this instance</returns>
      public override int GetHashCode()
      {
         return X.GetHashCode()^Y.GetHashCode();
      }
      /// <summary>
      /// Checks if two Vectors have an equal value
      /// </summary>
      /// <param name="obj"></param>
      /// <returns>true if the vectors are equal</returns>
      public override bool Equals(object obj)
      {
         if (obj is Vector2)
         {
            Vector2 v= (Vector2)obj;
            return this.X.Equals(v.X) && this.Y.Equals(v.Y);
         }
         else
            return false;
      }
      /// <summary>
      /// Return the length of this vector
      /// </summary>
      /// <returns></returns>
      public float Length()
      {
         return (float)Math.Sqrt(X * X + Y * Y);
      }
      /// <summary>
      /// The additive Identity (zero)
      /// </summary>
      public static Vector2 Empty = new Vector2(0, 0);
      /// <summary>
      /// Make this vector unit length
      /// </summary>
      public void Normalize()
      {
         float l = Length();
         if (l > 0)
         {
            X /= l; Y /= l;
         }
      }
      /// <summary>
      /// Return the scalar dot product of two vectors
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static float DotProduct(Vector2 a, Vector2 b)
      {
         return a.X * b.X + a.Y * b.Y;
      }
      /// <summary>
      /// Check if two vectors are equal
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static bool operator ==(Vector2 a, Vector2 b)
      {
         return a.X == b.X && a.Y == b.Y;
      }
      /// <summary>
      /// Check if two vectors are not equal
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static bool operator !=(Vector2 a, Vector2 b)
      {
         return a.X != b.X || a.Y != b.Y;
      }
      /// <summary>
      /// Return the additive inverse of a vector
      /// </summary>
      /// <param name="v"></param>
      /// <returns></returns>
      public static Vector2 operator -(Vector2 v)
      {
         return new Vector2(-v.X, -v.Y);
      }
      /// <summary>
      /// Return the sum of two vectors
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static Vector2 operator +(Vector2 a, Vector2 b)
      {
         return new Vector2(a.X + b.X, a.Y + b.Y);
      }
      /// <summary>
      /// Substract one vector from another
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static Vector2 operator -(Vector2 a, Vector2 b)
      {
         return new Vector2(a.X - b.X, a.Y - b.Y);
      }
      /// <summary>
      /// Return the product of a vector and a scalar
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <returns></returns>
      public static Vector2 operator *(Vector2 a, float b)
      {
         return new Vector2(a.X * b, a.Y * b);
      }
      /// <summary>
      /// Return the product of a scalar and a vector 
      /// </summary>
      /// <param name="b"></param>
      /// <param name="a"></param>
      /// <returns></returns>
      public static Vector2 operator *(float b, Vector2 a)
      {
         return new Vector2(a.X * b, a.Y * b);
      }
      /// <summary>
      /// Transform a vector by multiplying it by the supplied transformation matrix
      /// </summary>
      /// <param name="v"></param>
      /// <param name="m"></param>
      /// <returns></returns>
      public static Vector2 TransformCoordinate(Vector2 v, Matrix m)
      {
         float w = m.M41 * v.X + m.M42 * v.Y + m.M44;
         return new Vector2(
                      (m.M11 * v.X + m.M12 * v.Y + m.M14) / w,
                      (m.M21 * v.X + m.M22 * v.Y + m.M24) / w
                      );
      }
      /// <summary>
      /// Tranform an array of vectors by multiplying it by the supplied transformation matrix
      /// </summary>
      /// <param name="vs"></param>
      /// <param name="m"></param>
      /// <returns></returns>
      public static Vector2[] TransformCoordinate(Vector2[] vs, Matrix m)
      {
         Vector2[] rc = new Vector2[vs.Length];
         int i = 0;
         foreach (Vector2 v in vs)
            rc[i++] = TransformCoordinate(v, m);
         return rc;
      }
   }

   /// <summary>
   /// A two dimensional plane in a 3 dimensional space
   /// </summary>
   public struct Plane
   {
      public float A, B, C, D;
      /// <summary>
      /// Construct a plane from equation parameters
      /// </summary>
      /// <param name="a"></param>
      /// <param name="b"></param>
      /// <param name="c"></param>
      /// <param name="d"></param>
      public Plane(float a, float b, float c, float d)
      {
         A = a; B = b; C = c; D = d;
      }
      /// <summary>
      /// Construct the plane through three (non colinear) points
      /// </summary>
      /// <param name="v1"></param>
      /// <param name="v2"></param>
      /// <param name="v3"></param>
      /// <returns></returns>
      public static Plane FromPoints(Vector3 v1, Vector3 v2, Vector3 v3)
      {
         return Plane.FromPointNormal(v1, Vector3.CrossProduct(v2 - v1, v3 - v1));
      }
      /// <summary>
      /// Construct the plane through one point that is perpendicular to a normal vector
      /// </summary>
      /// <param name="p"></param>
      /// <param name="n"></param>
      /// <returns></returns>
      public static Plane FromPointNormal(Vector3 p, Vector3 n)
      {
         Vector3 n1 = n; n1.Normalize();
         return new Plane(n1.X, n1.Y, n1.Z, -Vector3.DotProduct(n1, p));
      }
      /// <summary>
      /// Normalize the parameters for this plane
      /// </summary>
      public void Normalize()
      {
         float l = (float)Math.Sqrt(A * A + B * B + C * C);
         A /= l; B /= l; C /= l; D /= l;
      }
   }

   /// <summary>
   /// Represents a rotation by an angle arround an arbitrary axis
   /// </summary>
   public struct AxisAngle
   {
      public float Angle;
      public Vector3 Axis;
   }

   /// <summary>
   /// A four dimensional matrix useful for homogeneous linear transformations
   /// in 3D space (rotations, translations, scalings etc)
   /// See http://en.wikipedia.org/wiki/Homogeneous_linear_transformation
   /// </summary>
   public struct Matrix
   {
      public float M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44;
      /// <summary>
      /// Construct a matrix from all its values
      /// Note: I accidentally used a different convention here than Microsoft with rows
      /// and columns reversed.
      /// </summary>
      /// <param name="m11"></param>
      /// <param name="m12"></param>
      /// <param name="m13"></param>
      /// <param name="m14"></param>
      /// <param name="m21"></param>
      /// <param name="m22"></param>
      /// <param name="m23"></param>
      /// <param name="m24"></param>
      /// <param name="m31"></param>
      /// <param name="m32"></param>
      /// <param name="m33"></param>
      /// <param name="m34"></param>
      /// <param name="m41"></param>
      /// <param name="m42"></param>
      /// <param name="m43"></param>
      /// <param name="m44"></param>
      public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
      {
         M11 = m11; M12 = m12; M13 = m13; M14 = m14;
         M21 = m21; M22 = m22; M23 = m23; M24 = m24;
         M31 = m31; M32 = m32; M33 = m33; M34 = m34;
         M41 = m41; M42 = m42; M43 = m43; M44 = m44;
      }
      /// <summary>
      /// Return the Matrix corresponding to a rotation by a angle about an arbitrary axis in 3D
      /// </summary>
      /// <param name="axis"></param>
      /// <param name="angle"></param>
      /// <returns></returns>
      public static Matrix RotationAxis(Vector3 axis, float angle)
      {
         Vector3 a0 = axis;
         a0.Normalize();
         float cos = (float)Math.Cos(angle);
         float oneLessCos = 1 - cos;
         float sin = (float)Math.Sin(angle);
         return new Matrix
         (
            cos + oneLessCos * a0.X * a0.X, oneLessCos * a0.X * a0.Y - sin * a0.Z, oneLessCos * a0.X * a0.Z + sin * a0.Y, 0,
            oneLessCos * a0.Y * a0.X + sin * a0.Z, cos + oneLessCos * a0.Y * a0.Y, oneLessCos * a0.Y * a0.Z - sin * a0.X, 0,
            oneLessCos * a0.Z * a0.X - sin * a0.Y, oneLessCos * a0.Z * a0.Y + sin * a0.X, cos + oneLessCos * a0.Z * a0.Z, 0,
            0, 0, 0, 1
         );
      }
      /// <summary>
      /// Return a general purpose 2D transformation matrix
      /// Note: This is currently only partially implemented
      /// </summary>
      /// <param name="scalingCenter"></param>
      /// <param name="scalingRotation"></param>
      /// <param name="scaling"></param>
      /// <param name="rotationCenter"></param>
      /// <param name="rotation"></param>
      /// <param name="translation"></param>
      /// <returns></returns>
      public static Matrix Transformation2D(Vector2 scalingCenter, float scalingRotation, Vector2 scaling, Vector2 rotationCenter, float rotation, Vector2 translation)
      {
         Vector2 rc = rotationCenter;
         float cos = (float)Math.Cos(rotation);
         float sin = (float)Math.Sin(rotation);
         return new Matrix
         (
             cos, -sin, 0, rc.X - cos * rc.X + sin * rc.Y,
             sin, cos, 0, rc.Y - sin * rc.X - cos * rc.Y,
             0, 0, 1, 0,
             0, 0, 0, 1
         );
      }
      /// <summary>
      /// Return the matrix representing the rotation about the X axis in 3D
      /// </summary>
      /// <param name="a"></param>
      /// <returns></returns>
      public static Matrix RotationX(float a)
      {
         return new Matrix
         (
             1, 0, 0, 0,
             0, (float)Math.Cos(a), -(float)Math.Sin(a), 0,
             0, (float)Math.Sin(a), (float)Math.Cos(a), 0,
             0, 0, 0, 1
         );
      }
      /// <summary>
      /// Return the matrix representing the rotation about the Y axis in 3D
      /// </summary>
      /// <param name="a"></param>
      /// <returns></returns>
      public static Matrix RotationY(float a)
      {
         return new Matrix
         (
             (float)Math.Cos(a), 0, (float)Math.Sin(a), 0,
             0, 1, 0, 0,
             -(float)Math.Sin(a), 0, (float)Math.Cos(a), 0,
             0, 0, 0, 1
         );
      }
      /// <summary>
      /// Return the matrix representing the rotation about the Z axis in 3D
      /// </summary>
      /// <param name="a"></param>
      /// <returns></returns>
      public static Matrix RotationZ(float a)
      {
         return new Matrix
         (
             (float)Math.Cos(a), -(float)Math.Sin(a), 0, 0,
             (float)Math.Sin(a), (float)Math.Cos(a), 0, 0,
             0, 0, 1, 0,
             0, 0, 0, 1
         );
      }
      /// <summary>
      /// Return the matrix representing a 3D translation (i.e. rigid move)
      /// </summary>
      /// <param name="v"></param>
      /// <returns></returns>
      public static Matrix Translation(Vector3 v)
      {
         return new Matrix(1, 0, 0, v.X,
                           0, 1, 0, v.Y,
                           0, 0, 1, v.Z,
                           0, 0, 0, 1);

      }
      /// <summary>
      /// Return the determinant of a 2 by 2 matrix
      /// </summary>
      /// <param name="m11"></param>
      /// <param name="m12"></param>
      /// <param name="m21"></param>
      /// <param name="m22"></param>
      /// <returns></returns>
      private static float det2x2(float m11, float m12, float m21, float m22)
      {
         return m11 * m22 - m12 * m21;
      }
      /// <summary>
      /// Return the determinant of a 3 by 3 matrix
      /// </summary>
      /// <param name="m11"></param>
      /// <param name="m12"></param>
      /// <param name="m13"></param>
      /// <param name="m21"></param>
      /// <param name="m22"></param>
      /// <param name="m23"></param>
      /// <param name="m31"></param>
      /// <param name="m32"></param>
      /// <param name="m33"></param>
      /// <returns></returns>
      private static float det3x3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
      {
         return m11 * det2x2(m22, m23, m32, m33) - m12 * det2x2(m21, m23, m32, m33) + m13 * det2x2(m21, m22, m31, m32);
      }
      /// <summary>
      /// Return the determinant of this 4 by 4 matrix
      /// See: http://en.wikipedia.org/wiki/Determinant
      /// </summary>
      public float Determinant
      {
         get
         {
            return M11 * det3x3(M22, M23, M24, M32, M33, M34, M42, M43, M44)
                 - M12 * det3x3(M21, M23, M24, M31, M33, M34, M41, M43, M44)
                 + M13 * det3x3(M21, M22, M24, M31, M32, M34, M41, M42, M44)
                 - M14 * det3x3(M21, M22, M23, M31, M32, M33, M41, M42, M43);
         }
      }
      /// <summary>
      /// Produce the inverse of this Matrix (Hasn't been tested)
      /// </summary>
      /// <returns></returns>
      public Matrix Invert()
      {
         return Matrix.Invert(this);
      }
      /// <summary>
      /// Product the inverse of the supplied Matrix (Hasn't been tested)
      /// </summary>
      /// <param name="m"></param>
      /// <returns></returns>
      public static Matrix Invert(Matrix m)
      {
         float d = m.Determinant;
         if (d == 0)
            throw new InvalidOperationException("Can't invert a matrix with a zero determinant");
         return new Matrix(+det3x3(m.M22, m.M32, m.M42, m.M23, m.M33, m.M43, m.M24, m.M34, m.M44) / d,
                           -det3x3(m.M12, m.M32, m.M42, m.M13, m.M33, m.M43, m.M14, m.M34, m.M44) / d,
                           +det3x3(m.M12, m.M22, m.M42, m.M13, m.M23, m.M43, m.M14, m.M24, m.M44) / d,
                           -det3x3(m.M12, m.M22, m.M32, m.M13, m.M23, m.M33, m.M14, m.M24, m.M34) / d,

                           -det3x3(m.M21, m.M31, m.M41, m.M23, m.M33, m.M43, m.M24, m.M34, m.M44) / d,
                           +det3x3(m.M11, m.M31, m.M41, m.M13, m.M33, m.M43, m.M14, m.M34, m.M44) / d,
                           -det3x3(m.M11, m.M21, m.M41, m.M13, m.M23, m.M43, m.M14, m.M24, m.M44) / d,
                           +det3x3(m.M11, m.M21, m.M31, m.M13, m.M23, m.M33, m.M14, m.M24, m.M34) / d,

                           +det3x3(m.M21, m.M31, m.M41, m.M22, m.M32, m.M42, m.M24, m.M34, m.M44) / d,
                           -det3x3(m.M11, m.M31, m.M41, m.M12, m.M32, m.M42, m.M14, m.M34, m.M44) / d,
                           +det3x3(m.M11, m.M21, m.M41, m.M12, m.M22, m.M42, m.M14, m.M24, m.M44) / d,
                           -det3x3(m.M11, m.M21, m.M31, m.M12, m.M22, m.M32, m.M14, m.M24, m.M34) / d,

                           -det3x3(m.M21, m.M31, m.M41, m.M22, m.M32, m.M42, m.M23, m.M33, m.M43) / d,
                           +det3x3(m.M11, m.M31, m.M41, m.M12, m.M32, m.M42, m.M13, m.M33, m.M43) / d,
                           -det3x3(m.M11, m.M21, m.M41, m.M12, m.M22, m.M42, m.M13, m.M23, m.M43) / d,
                           +det3x3(m.M11, m.M21, m.M31, m.M12, m.M22, m.M32, m.M13, m.M23, m.M33) / d
                          );


      }
      /// <summary>
      /// The Multiplicative Identity (i.e. One)
      /// </summary>
      public static Matrix Identity
      {
         get
         {
            return new Matrix(1, 0, 0, 0,
                              0, 1, 0, 0,
                              0, 0, 1, 0,
                              0, 0, 0, 1);
         }
      }
      /// <summary>
      /// The Additive Identity (i.e. Zero)
      /// </summary>
      public static Matrix Zero
      {
         get
         {
            return new Matrix();
         }
      }
      /// <summary>
      /// Return the product of two 4 by 4 matrices. This is useful
      /// to combine two linear transformations (e.g. a rotation and a translation)
      /// into a single one.
      /// </summary>
      /// <param name="b"></param>
      /// <param name="a"></param>
      /// <returns></returns>
      public static Matrix operator *(Matrix b, Matrix a)
      {
         return new Matrix(
           a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
           a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
           a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
           a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

           a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
           a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
           a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
           a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

           a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
           a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
           a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
           a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

           a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
           a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
           a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
           a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44);
      }
   }
}
