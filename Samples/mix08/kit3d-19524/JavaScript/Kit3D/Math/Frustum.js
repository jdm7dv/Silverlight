//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function Frustum(viewport)
{
    this._viewPort = viewport;
    
    //Work out the parameters needed for calculating the frustum plane normals
    var camera = this._viewPort.GetCamera();
    var cameraPosition = camera.Position;
    
    var farDistance = this._viewPort.GetFarDistance();
    var nearDistance = this._viewPort.GetNearDistance();
    var nearPlaneHalfHeight = Math.tan(camera.FieldOfView/2) * nearDistance;
    var nearPlaneHalfWidth = nearPlaneHalfHeight * (this._viewPort.GetWidth() / this._viewPort.GetHeight());
    var farPlaneHalfHeight = Math.tan(camera.FieldOfView/2) * farDistance;
    var farPlaneHalfWidth = farPlaneHalfHeight * (this._viewPort.GetWidth() / this._viewPort.GetHeight());
    
    //The center point of the near and far plane
    this._nearCenter = camera.Position.Add(camera.ViewDirection.MultiplyScalar(nearDistance));
    this._farCenter = camera.Position.Add(camera.ViewDirection.MultiplyScalar(farDistance));

    //The extremes of the near plane
    var nearTopLeft = this._nearCenter.Add(camera.Left.MultiplyScalar(nearPlaneHalfWidth)).Add(camera.Up.MultiplyScalar(nearPlaneHalfHeight));
    var nearTopRight = this._nearCenter.Add(camera.Left.MultiplyScalar(-1 * nearPlaneHalfWidth)).Add(camera.Up.MultiplyScalar(nearPlaneHalfHeight));
    var nearBottomLeft = this._nearCenter.Add(camera.Left.MultiplyScalar(nearPlaneHalfWidth)).Add(camera.Up.MultiplyScalar(-1 * nearPlaneHalfHeight));
    var nearBottomRight = this._nearCenter.Add(camera.Left.MultiplyScalar(-1 * nearPlaneHalfWidth)).Add(camera.Up.MultiplyScalar(-1 * nearPlaneHalfHeight));
    
    var farTopLeft = this._farCenter.Add(camera.Left.MultiplyScalar(farPlaneHalfWidth)).Add(camera.Up.MultiplyScalar(farPlaneHalfHeight));
    var farTopRight = this._farCenter.Add(camera.Left.MultiplyScalar(-1 * farPlaneHalfWidth)).Add(camera.Up.MultiplyScalar(farPlaneHalfHeight));
    var farBottomLeft = this._farCenter.Add(camera.Left.MultiplyScalar(farPlaneHalfWidth)).Add(camera.Up.MultiplyScalar(-1 * farPlaneHalfHeight));
    var farBottomRight = this._farCenter.Add(camera.Left.MultiplyScalar(-1 * farPlaneHalfWidth)).Add(camera.Up.MultiplyScalar(-1 * farPlaneHalfHeight));
        
    var leftNormal = farTopLeft.Subtract(nearBottomLeft).CrossProduct(farBottomLeft.Subtract(nearBottomLeft));
    var rightNormal = farBottomRight.Subtract(nearBottomRight).CrossProduct(farTopRight.Subtract(nearBottomRight));
    var topNormal = farTopRight.Subtract(nearTopRight).CrossProduct(farTopLeft.Subtract(nearTopRight));
    var bottomNormal = farBottomLeft.Subtract(nearBottomLeft).CrossProduct(farBottomRight.Subtract(nearBottomLeft));
    
    //Calculate the planes for the frustum which we can use to clip objects against
    //all of the normals poiint into the view frustum
    this._leftPlane = new Plane(leftNormal, farBottomLeft);
    this._rightPlane = new Plane(rightNormal, farBottomRight);
    this._topPlane = new Plane(topNormal, farTopLeft);
    this._bottomPlane = new Plane(bottomNormal, farBottomLeft);
}

var Frustum_Outside = 0;
var Frustum_Inside = 1;
var Frustum_IntersectPlanes = 2;

Frustum.prototype = 
{
    //Returns:
    //1 -> if the bounding geometry of the model is inside the Frustum
    //0 -> if the bounding geometry of the model is outside the Frustum
    //2 -> if the bounding geometry of the model intersects with some of the planes of the frustum
    ContainsGeometry: function(geometryModel, localToWorldTransform)
    {
        //TODO: Need to support partial clipping
        
        //The geometry bounding centroid is in local co-ordinates, need
        //to multiply by world transform before culling to view frustum planes
        var centroid = geometryModel.BoundingCentroid;
        centroid = localToWorldTransform.PreMultiplyVector(centroid);
        var radius = geometryModel.BoundingRadius;
        
        //Quick check against the near and far planes
        if(((centroid.Z - radius) < this._nearCenter.Z) || 
           (centroid.Z + radius > this._farCenter.Z))
        {
            return Frustum_Outside;
        }
    
        //Check left plane
        var distance = this._leftPlane.PointDistance(centroid);
        if(distance - radius < 0)
        {
            return Frustum_Outside;
        }
        
        //Check right plane
        distance = this._rightPlane.PointDistance(centroid);
        if(distance - radius < 0)
        {
            return Frustum_Outside;
        }
        
        //Check top plane
        distance = this._topPlane.PointDistance(centroid);
        if(distance - radius < 0)
        {
            return Frustum_Outside;
        }
                
        //Check bottom plane
        distance = this._bottomPlane.PointDistance(centroid);
        if(distance - radius < 0)
        {
            return Frustum_Outside;
        }
        
        //If the geometry is inside all of the planes then it must be visible
        return Frustum_Inside;
    },
    
    ToString: function()
    {
        return 'Frustum';
    }
}