//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

/*
Represents a Plane in 3D space
*/

//normal - normal to the plane
//p - point lying on the plane
function Plane(normal, p)
{
    this._normal = normal.Normalize();
    this._point = p;
    
    //Generalized plane equation is ax + by + cz + d
    //can store the first three components, then we can
    //easily calculate the distance from a point to the plane
    this._d = -1 * this._normal.DotProduct(p);
}

Plane.prototype = 
{
    //Returns the shortest distance between the point and the plane
    PointDistance: function(point)
    {
        //n . point = ax + by + cz
        return this._d + this._normal.DotProduct(point);
    },
    
    ToString: function()
    {
        return 'Plane: n=' + this._normal.ToString() + ', partial dist=' + this._partialDistance + ', pip=' + this._point.ToString();
    }
}