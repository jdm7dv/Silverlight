//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

//Represents a camera in world co-ordinates
function Camera(position, lookAt, worldUp, fieldOfView)
{
    //The position of the camera in world co-ordinates
    this.Position = position;
    
    //The position that the camera is looking at in world co-ordinates
    this._lookAt = lookAt;
    
    //The direction the camera is looking
    this.ViewDirection = this._lookAt.Subtract(this.Position).Normalize();
    
    var worldUpNormalized = worldUp.Normalize();
    this.Up = worldUpNormalized.Subtract(this.ViewDirection.MultiplyScalar(worldUpNormalized.DotProduct(this.ViewDirection))).Normalize();
    
    this.Left = this.ViewDirection.CrossProduct(this.Up).Normalize();
    
    //The angle of view, this value is in radians
    this.FieldOfView = fieldOfView;
}

Camera.prototype = 
{
    ToString: function()
    {
        return 'Camera';
    }
}