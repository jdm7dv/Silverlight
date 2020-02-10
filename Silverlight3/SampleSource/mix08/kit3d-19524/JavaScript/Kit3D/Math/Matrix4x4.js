//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

/*
Represents  a4x4 homogenous matrix
*/

function Matrix4x4_TranslateMatrix(tx, ty, tz)
{
    return new Matrix4x4(1,  0,  0,  0,
                         0,  1,  0,  0,
                         0,  0,  1,  0,
                         tx, ty, tz, 1);
}

function Matrix4x4_ScalingMatrix(sx, sy, sz)
{
    return new Matrix4x4(sx, 0,  0,  0,
                         0,  sy, 0,  0,
                         0,  0,  sz, 0,
                         0,  0,  0,  1);
}

function Matrix4x4_IdentityMatrix()
{
    return new Matrix4x4(1, 0, 0, 0,
                         0, 1, 0, 0,
                         0, 0, 1, 0,
                         0, 0, 0, 1);
}

function Matrix4x4_RotateZAroundOrigin(angleInRadians)
{
    return new Matrix4x4(Math.cos(angleInRadians),    Math.sin(angleInRadians), 0, 0,
                         -1*Math.sin(angleInRadians), Math.cos(angleInRadians), 0, 0,
                         0,                           0,                        1, 0,
                         0,                           0,                        0, 1);
}
    
function Matrix4x4_RotateYAroundPoint(angleInRadians, point)
{
    return (new Matrix4x4(1,            0,            0,            0,
                          0,            1,            0,            0,
                          0,            0,            1,            0,
                          -1 * point.X, -1 * point.Y, -1 * point.Z, 1)).Multiply(
                            new Matrix4x4(Math.cos(angleInRadians),      0, Math.sin(angleInRadians), 0,
                                          0,                             1, 0,                        0,
                                          -1 * Math.sin(angleInRadians), 0, Math.cos(angleInRadians), 0,
                                          0,                             0, 0,                        1)).Multiply(new Matrix4x4(
                                                                       1, 0, 0, 0,
                                                                       0, 1, 0, 0,
                                                                       0, 0, 1, 0,
                                                                       point.X, point.Y, point.Z, 1));
}

function Matrix4x4_RotateXAroundPoint(angleInRadians, point)
{
    return (new Matrix4x4(1, 0, 0, 0,
                         0, 1, 0, 0,
                         0, 0, 1, 0,
                         -1 * point.X, -1 * point.Y, -1 * point.Z, 1)).Multiply(new Matrix4x4(1, 0, 0, 0,
                         0, Math.cos(angleInRadians), Math.sin(angleInRadians), 0,
                         0, -1 * Math.sin(angleInRadians), Math.cos(angleInRadians), 0,
                         0, 0, 0, 1)).Multiply(new Matrix4x4(1, 0, 0, 0,
                                                             0, 1, 0, 0,
                                                             0, 0, 1, 0,
                                                             point.X, point.Y, point.Z, 1));
}

function Matrix4x4_RotateZAroundPoint(angleInRadians, point)
{
    return (new Matrix4x4(1, 0, 0, 0,
                         0, 1, 0, 0,
                         0, 0, 1, 0,
                         -1 * point.X, -1 * point.Y, -1 * point.Z, 1)).Multiply(new Matrix4x4(Math.cos(angleInRadians), Math.sin(angleInRadians), 0, 0,
                         -1*Math.sin(angleInRadians), Math.cos(angleInRadians), 0, 0,
                         0, 0, 1, 0,
                         0, 0, 0, 1)).Multiply(new Matrix4x4(1, 0, 0, 0,
                                                             0, 1, 0, 0,
                                                             0, 0, 1, 0,
                                                             point.X, point.Y, point.Z, 1));
}

function Matrix4x4_RotateYAroundOrigin(angleInRadians)
{
    return new Matrix4x4(Math.cos(angleInRadians),      0, Math.sin(angleInRadians), 0,
                         0,                             1, 0,                        0,
                         -1 * Math.sin(angleInRadians), 0, Math.cos(angleInRadians), 0,
                         0,                             0, 0,                        1);
}

function Matrix4x4_RotateXAroundOrigin(angleInRadians)
{
    return new Matrix4x4(1, 0,                             0,                        0,
                         0, Math.cos(angleInRadians),      Math.sin(angleInRadians), 0,
                         0, -1 * Math.sin(angleInRadians), Math.cos(angleInRadians), 0,
                         0, 0,                             0,                        1);
}


/// Creates a new matrix, setting the individual elements
/// The format of the matrix created is:
/// 
/// a b c d
/// e f g h
/// i j k l
/// m n o p
function Matrix4x4(a, b, c, d,
                   e, f, g, h,
                   i, j, k, l,
                   m, n, o, p)
{
    this._values = new Array(16);
    this._values[0] = a;  
    this._values[1] = b;  
    this._values[2] = c;  
    this._values[3] = d;
    this._values[4] = e;  
    this._values[5] = f; 
    this._values[6] = g;  
    this._values[7] = h;
    this._values[8] = i;  
    this._values[9] = j;  
    this._values[10] = k; 
    this._values[11] = l;
    this._values[12] = m; 
    this._values[13] = n; 
    this._values[14] = o; 
    this._values[15] = p;
}

Matrix4x4.prototype = 
{
    /// a b c d
    /// e f g h
    /// i j k l
    /// m n o p
    /// 
    /// becomes
    /// 
    /// a e i m
    /// b f j n
    /// c g k o
    /// d h l p
    Transpose: function()
    {
        return new Matrix4x4(this._values[0], this._values[4], this._values[8], this._values[12],
                             this._values[1], this._values[5], this._values[9], this._values[13],
                             this._values[2], this._values[6], this._values[10], this._values[14],
                             this._values[3], this._values[7], this._values[11], this._values[15]);
    },
    
    Add: function(m)
    {
        return new Matrix4x4(this._values[0] + m._values[0], this._values[1] + m._values[1], this._values[2] + m._values[2], this._values[3] + m._values[3],
                             this._values[4] + m._values[4], this._values[5] + m._values[5], this._values[6] + m._values[6], this._values[7] + m._values[7],
                             this._values[8] + m._values[8], this._values[9] + m._values[9], this._values[10] + m._values[10], this._values[11] + m._values[11],
                             this._values[12] + m._values[12], this._values[13] + m._values[13], this._values[14] + m._values[14], this._values[15] + m._values[15]);
    },
    
    Subtract: function(m)
    {
        return new Matrix4x4(this._values[0] - m._values[0], this._values[1] - m._values[1], this._values[2] - m._values[2], this._values[3] - m._values[3],
                             this._values[4] - m._values[4], this._values[5] - m._values[5], this._values[6] - m._values[6], this._values[7] - m._values[7],
                             this._values[8] - m._values[8], this._values[9] - m._values[9], this._values[10] - m._values[10], this._values[11] - m._values[11],
                             this._values[12] - m._values[12], this._values[13] - m._values[13], this._values[14] - m._values[14], this._values[15] - m._values[15]);
    },
    
    Multiply: function(m)
    {
        return new Matrix4x4(this._values[0] * m._values[0] + this._values[1] * m._values[4] + this._values[2] * m._values[8] + this._values[3] * m._values[12],
                             this._values[0] * m._values[1] + this._values[1] * m._values[5] + this._values[2] * m._values[9] + this._values[3] * m._values[13],
                             this._values[0] * m._values[2] + this._values[1] * m._values[6] + this._values[2] * m._values[10] + this._values[3] * m._values[14],
                             this._values[0] * m._values[3] + this._values[1] * m._values[7] + this._values[2] * m._values[11] + this._values[3] * m._values[15],

                             this._values[4] * m._values[0] + this._values[5] * m._values[4] + this._values[6] * m._values[8] + this._values[7] * m._values[12],
                             this._values[4] * m._values[1] + this._values[5] * m._values[5] + this._values[6] * m._values[9] + this._values[7] * m._values[13],
                             this._values[4] * m._values[2] + this._values[5] * m._values[6] + this._values[6] * m._values[10] + this._values[7] * m._values[14],
                             this._values[4] * m._values[3] + this._values[5] * m._values[7] + this._values[6] * m._values[11] + this._values[7] * m._values[15],

                             this._values[8] * m._values[0] + this._values[9] * m._values[4] + this._values[10] * m._values[8] + this._values[11] * m._values[12],
                             this._values[8] * m._values[1] + this._values[9] * m._values[5] + this._values[10] * m._values[9] + this._values[11] * m._values[13],
                             this._values[8] * m._values[2] + this._values[9] * m._values[6] + this._values[10] * m._values[10] + this._values[11] * m._values[14],
                             this._values[8] * m._values[3] + this._values[9] * m._values[7] + this._values[10] * m._values[11] + this._values[11] * m._values[15],

                             this._values[12] * m._values[0] + this._values[13] * m._values[4] + this._values[14] * m._values[8] + this._values[15] * m._values[12],
                             this._values[12] * m._values[1] + this._values[13] * m._values[5] + this._values[14] * m._values[9] + this._values[15] * m._values[13],
                             this._values[12] * m._values[2] + this._values[13] * m._values[6] + this._values[14] * m._values[10] + this._values[15] * m._values[14],
                             this._values[12] * m._values[3] + this._values[13] * m._values[7] + this._values[14] * m._values[11] + this._values[15] * m._values[15]);
    },
    
    MultiplyScalar: function(f)
    {
        return new Matrix4x4(this._values[0] * f, this._values[1] * f, this._values[2] * f, this._values[3] * f,
                             this._values[4] * f, this._values[5] * f, this._values[6] * f, this._values[7] * f,
                             this._values[8] * f, this._values[9] * f, this._values[10] * f, this._values[11] * f,
                             this._values[12] * f, this._values[13] * f, this._values[14] * f, this._values[15] * f);
    },
    
    //Premultipls the matrix by a Vector4 instance
    PreMultiplyVector: function(v)
    {
        return new Vector4(v.X * this._values[0] + v.Y * this._values[4] + v.Z * this._values[8] + v.W * this._values[12],
                           v.X * this._values[1] + v.Y * this._values[5] + v.Z * this._values[9] + v.W * this._values[13],
                           v.X * this._values[2] + v.Y * this._values[6] + v.Z * this._values[10] + v.W * this._values[14],
                           v.X * this._values[3] + v.Y * this._values[7] + v.Z * this._values[11] + v.W * this._values[15]);
    },
    
    SetColumns: function(col1, col2, col3, col4)
    {
        this._values[0] = col1.X;
        this._values[4] = col1.Y;
        this._values[8] = col1.Z;
        this._values[12] = col1.W;

        this._values[1] = col2.X;
        this._values[5] = col2.Y;
        this._values[9] = col2.Z;
        this._values[13] = col2.W;

        this._values[2] = col3.X;
        this._values[6] = col3.Y;
        this._values[10] = col3.Z;
        this._values[14] = col3.W;

        this._values[3] = col4.X;
        this._values[7] = col4.Y;
        this._values[11] = col4.Z;
        this._values[15] = col4.W;
    },
    
    SetRow: function(row, rowIndex)
    {
        var startIndex = rowIndex * 4;
        this._values[startIndex] = row.X;
        this._values[startIndex + 1] = row.Y;
        this._values[startIndex + 2] = row.Z;
        this._values[startIndex + 3] = row.W;
    },
    
    Determinant: function()
    {
        //Determinant for a 4x4 matrix
        alert('TODO: Implemented Determinant for 4x4 matrix');
    },
    
    Inverse: function()
    {
        alert('TODO: Implement inverse for 4x4 matrix');
    },
    
    ToString: function()
    {
        return this._values[0] + " " + this._values[1] + " " + this._values[2] + " " + this._values[3] + "\n" +
               this._values[4] + " " + this._values[5] + " " + this._values[6] + " " + this._values[7] + "\n" +
               this._values[8] + " " + this._values[9] + " " + this._values[10] + " " + this._values[11] + "\n" +
               this._values[12] + " " + this._values[13] + " " + this._values[14] + " " + this._values[15];
    }
}