//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function Vector3(x, y, z)
{
    //Since these are really simple, will expose the fields publically
    //since we will be accessing them so much
    this.X = x;
    this.Y = y;
    this.Z = z;
}

Vector3.prototype = 
{
    //Returns the dot product of the current vector, with vector v
    DotProduct: function(v)
    {
        return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
    },
    
    //Returns a normalized version of the current vector
    Normalize: function()
    {
        var lengthSquared = this.LengthSquared();
        if(MathHelperIsZero(lengthSquared))
        {
            return new Vector3(0,0,0);
        }
        
        var inverseSquareRoot = MathHelperInverseSquareRoot(lengthSquared);
        return new Vector3(this.X * inverseSquareRoot,
                           this.Y * inverseSquareRoot,
                           this.Z * inverseSquareRoot);
    },
    
    /// Returns the cross product of the instance vector with v.  The length of
    /// the cross product is equal to the area of the parallelogram created by
    /// u and v.
    /// v x w == -w x v
    /// v x v == 0
    /// 
    /// i  j  k
    /// vx vy vz
    /// wx wy wz
    CrossProduct: function(v)
    {
        return new Vector3(this.Y * v.Z - this.Z * v.Y,
                           this.Z * v.X - this.X * v.Z,
                           this.X * v.Y - this.Y * v.X);
    },
    
    //Returns the length of the vector
    Length: function()
    {
        return MathHelperSquareRoot(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
    },
    
    //Returns the length of the vector squared
    LengthSquared: function()
    {
        return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
    },
    
    //Returns the addition of this vector with v
    Add: function(v)
    {
        return new Vector3(this.X + v.X, 
                           this.Y + v.Y,
                           this.Z + v.Z);
    },
    
    Subtract: function(v)
    {
        return new Vector3(this.X - v.X,
                           this.Y - v.Y,
                           this.Z - v.Z);
    },
    
    MultiplyScalar: function(f)
    {
        return new Vector3(this.X * f,
                           this.Y * f,
                           this.Z * f);
    },
    
    Divide: function(f)
    {
        return new Vector3(this.X / f,
                           this.Y / f,
                           this.Z / f);
    },
    
    Equals: function(v)
    {
        return MathHelperAreEqual(this.X, v.X) &&
               MathHelperAreEqual(this.Y, v.Y) &&
               MathHelperAreEqual(this.Z, v.Z);
    },
    
    ToString: function()
    {
        return 'X:' + this.X + '\n' +
               'Y:' + this.Y + '\n' +
               'Z:' + this.Z + '\n';
    }
}