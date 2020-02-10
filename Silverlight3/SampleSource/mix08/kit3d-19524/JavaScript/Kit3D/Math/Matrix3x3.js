//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

/*
Represents a 3x3 matrix
*/

function Matrix3x3_TranslateMatrix(tx, ty)
{
    return new Matrix3x3(1,  0,  0,
                         0,  1,  0,
                         tx, ty, 1);
}

function Matrix3x3_ScalingMatrix(sx, sy)
{
    return new Matrix3x3(sx, 0,  0,
                         0,  sy, 0,
                         0,  0,  1);
}

function Matrix3x3_IdentityMatrix()
{
    return new Matrix3x3(1, 0, 0,
                         0, 1, 0,
                         0, 0, 1);
}

function Matrix3x3_RotateZAroundOrigin(angleInRadians)
{
    return new Matrix3x3(Math.cos(angleInRadians),    Math.sin(angleInRadians), 0,
                         -1*Math.sin(angleInRadians), Math.cos(angleInRadians), 0,
                         0,                           0,                        1);
}
    
function Matrix3x3_RotateYAroundOrigin(angleInRadians)
{
    return new Matrix3x3(Math.cos(angleInRadians),      0, Math.sin(angleInRadians),
                         0,                             1, 0,
                         -1 * Math.sin(angleInRadians), 0, Math.cos(angleInRadians));
}

function Matrix3x3_RotateXAroundOrigin(angleInRadians)
{
    return new Matrix3x3(1, 0,                             0,
                         0, Math.cos(angleInRadians),      Math.sin(angleInRadians),
                         0, -1 * Math.sin(angleInRadians), Math.cos(angleInRadians));
}


/// Creates a new matrix, setting the individual elements
/// The format of the matrix created is:
/// 
/// a b c
/// d e f
/// g h i
function Matrix3x3(a, b, c,
                   d, e, f,
                   g, h, i)
{
    this._values = new Array(9);
    this._values[0] = a;  
    this._values[1] = b;  
    this._values[2] = c;  
    this._values[3] = d;
    this._values[4] = e;  
    this._values[5] = f; 
    this._values[6] = g;  
    this._values[7] = h;
    this._values[8] = i;  
}

Matrix3x3.prototype = 
{
    /// a b c
    /// d e f
    /// g h i
    /// 
    /// becomes
    /// 
    /// a d g
    /// b e h
    /// c f i
    Transpose: function()
    {
        return new Matrix3x3(this._values[0], this._values[3], this._values[6],
                             this._values[1], this._values[4], this._values[7],
                             this._values[2], this._values[5], this._values[8]);
    },
    
    Add: function(m)
    {
        return new Matrix3x3(this._values[0] + m._values[0], this._values[1] + m._values[1], this._values[2] + m._values[2],
                             this._values[3] + m._values[3], this._values[4] + m._values[4], this._values[5] + m._values[5],
                             this._values[6] + m._values[6], this._values[7] + m._values[7], this._values[8] + m._values[8]);
    },
    
    Subtract: function(m)
    {
        return new Matrix3x3(this._values[0] - m._values[0], this._values[1] - m._values[1], this._values[2] - m._values[2],
                             this._values[3] - m._values[3], this._values[4] - m._values[4], this._values[5] - m._values[5],
                             this._values[6] - m._values[6], this._values[7] - m._values[7], this._values[8] - m._values[8]);
    },
    
    Multiply: function(m)
    {
        return new Matrix3x3(this._values[0] * m._values[0] + this._values[1] * m._values[3] + this._values[2] * m._values[6],
                             this._values[0] * m._values[1] + this._values[1] * m._values[4] + this._values[2] * m._values[7],
                             this._values[0] * m._values[2] + this._values[1] * m._values[5] + this._values[2] * m._values[8],

                             this._values[3] * m._values[0] + this._values[4] * m._values[3] + this._values[5] * m._values[6],
                             this._values[3] * m._values[1] + this._values[4] * m._values[4] + this._values[5] * m._values[7],
                             this._values[3] * m._values[2] + this._values[4] * m._values[5] + this._values[5] * m._values[8],

                             this._values[6] * m._values[0] + this._values[7] * m._values[3] + this._values[8] * m._values[6],
                             this._values[6] * m._values[1] + this._values[7] * m._values[4] + this._values[8] * m._values[7],
                             this._values[6] * m._values[2] + this._values[7] * m._values[5] + this._values[8] * m._values[8]);
    },
    
    MultiplyScalar: function(f)
    {
        return new Matrix3x3(this._values[0] * f, this._values[1] * f, this._values[2] * f,
                             this._values[3] * f, this._values[4] * f, this._values[5] * f,
                             this._values[6] * f, this._values[7] * f, this._values[8] * f);
    },
    
    //Premultipls the matrix by a Vector3 instance
    PreMultiplyVector: function(v)
    {
        return new Vector3(v.X * this._values[0] + v.Y * this._values[3] + v.Z * this._values[6],
                           v.X * this._values[1] + v.Y * this._values[4] + v.Z * this._values[7],
                           v.X * this._values[2] + v.Y * this._values[5] + v.Z * this._values[8]);
    },
    
    SetColumns: function(col1, col2, col3)
    {
        this._values[0] = col1.X;
        this._values[3] = col1.Y;
        this._values[6] = col1.Z;

        this._values[1] = col2.X;
        this._values[4] = col2.Y;
        this._values[7] = col2.Z;

        this._values[2] = col3.X;
        this._values[5] = col3.Y;
        this._values[8] = col3.Z;
    },
    
    SetRow: function(row, rowIndex)
    {
        var startIndex = rowIndex * 3;
        this._values[startIndex] = row.X;
        this._values[startIndex + 1] = row.Y;
        this._values[startIndex + 2] = row.Z;
    },
    
    Determinant: function()
    {
        //Determinant for a 3x3 matrix
        //a b c   0 1 2
        //d e f   3 4 5 
        //g h i   6 7 8 
        //det = a(ei - fh) - b(di-fg) + c(dh-eg)
        return this._values[0] * (this._values[4] * this._values[8] - this._values[5] * this._values[7]) -
               this._values[1] * (this._values[3] * this._values[8] - this._values[5] * this._values[6]) +
               this._values[2] * (this._values[3] * this._values[7] - this._values[4] * this._values[6]);
    },
    
    Inverse: function()
    {
        var determinant = this.Determinant();
        if(MathHelperIsZero(determinant))
        {
            //TODO: Error handling
            alert('det is zero det=' + determinant + ', m=' + this.ToString());
        }
        
        //Inverse is the determinant times the matrix of cofactors transposed (adjoint matrix)
        var invDet = 1.0 / determinant;
        var cofactor0 = this._values[4] * this._values[8] - this._values[5] * this._values[7];
        var cofactor1 = -1 * (this._values[3] * this._values[8] - this._values[5] * this._values[6]);
        var cofactor2 = this._values[3] * this._values[7] - this._values[4] * this._values[6];
        var cofactor3 = -1 * (this._values[1] * this._values[8] - this._values[2] * this._values[7]);
        var cofactor4 = this._values[0] * this._values[8] - this._values[2] * this._values[6];
        var cofactor5 = -1 * (this._values[0] * this._values[7] - this._values[1] * this._values[6]);
        var cofactor6 = this._values[1] * this._values[5] - this._values[2] * this._values[4];
        var cofactor7 = -1 * (this._values[0] * this._values[5] - this._values[2] * this._values[3]);
        var cofactor8 = this._values[0] * this._values[4] - this._values[1] * this._values[3];

        var p = new Matrix3x3(cofactor0, cofactor3, cofactor6,
                              cofactor1, cofactor4, cofactor7,
                              cofactor2, cofactor5, cofactor8);
                          
        return new Matrix3x3(invDet * cofactor0, invDet * cofactor3, invDet * cofactor6,
                             invDet * cofactor1, invDet * cofactor4, invDet * cofactor7,
                             invDet * cofactor2, invDet * cofactor5, invDet * cofactor8);
    },
    
    ToString: function()
    {
        return this._values[0] + " " + this._values[1] + " " + this._values[2] + "\n" +
               this._values[3] + " " + this._values[4] + " " + this._values[5] + "\n" +
               this._values[6] + " " + this._values[7] + " " + this._values[8];
    }
}