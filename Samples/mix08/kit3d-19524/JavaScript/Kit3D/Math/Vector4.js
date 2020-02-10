//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function Vector4(x, y, z, w)
{
    this.X = x;
    this.Y = y;
    this.Z = z;
    this.W = w;
}

Vector4.prototype = 
{
    //Returns the dot product of the current vector, with vector v
    DotProduct: function(v)
    {
        return this.X * v.X + this.Y * v.Y + this.Z * v.Z + this.W * v.W;
    },
    
    //Returns a normalized version of the current vector
    Normalize: function()
    {
        var lengthSquared = this.LengthSquared();
        if(MathHelperIsZero(lengthSquared))
        {
            return new Vector4(0,0,0,0);
        }
        
        var inverseSquareRoot = MathHelperInverseSquareRoot(lengthSquared);
        return new Vector4(this.X * inverseSquareRoot,
                           this.Y * inverseSquareRoot,
                           this.Z * inverseSquareRoot,
                           this.W * inverseSquareRoot);
    },
    
    //Returns the length of the vector
    Length: function()
    {
        return MathHelperSquareRoot(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
    },
    
    //Returns the length of the vector squared
    LengthSquared: function()
    {
        return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
    },
    
    //Returns the addition of this vector with v
    Add: function(v)
    {
        return new Vector4(this.X + v.X, 
                           this.Y + v.Y,
                           this.Z + v.Z,
                           this.W + v.W);
    },
    
    CrossProduct: function(v)
    {
        alert('TODO: Implement CrossProduct');
    },
    
    Subtract: function(v)
    {
        return new Vector4(this.X - v.X,
                           this.Y - v.Y,
                           this.Z - v.Z,
                           this.W - v.W);
    },
    
    Multiply: function(f)
    {
        return new Vector4(this.X * f,
                           this.Y * f,
                           this.Z * f,
                           this.W * f);
    },
    
    Divide: function(f)
    {
        return new Vector4(this.X / f,
                           this.Y / f,
                           this.Z / f,
                           this.W / f);
    },
    
    Equals: function(v)
    {
        return MathHelperAreEqual(this.X, v.X) &&
               MathHelperAreEqual(this.Y, v.Y) &&
               MathHelperAreEqual(this.Z, v.Z) &&
               MathHelperareEqual(this.W, v.W);
    },
    
    ToString: function()
    {
        return 'X:' + this.X + '\n' +
               'Y:' + this.Y + '\n' +
               'Z:' + this.Z + '\n' +
               'W:' + this.W + '\n';
    }
}