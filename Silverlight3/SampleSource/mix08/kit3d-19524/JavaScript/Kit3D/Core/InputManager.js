//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function InputManager()
{
    this._onMouseEnterCallback = null;
    this._onMouseLeaveCallback = null;
}

InputManager.prototype =
{
    _OnMouseEnter: function(sender, mouseEventArgs)
    {
        if(this._onMouseEnterCallback != null)
        {
            //Figure out if this is a geometry model
            var geometryModel = GeometryManager_GetInstance().GetGeometryById(sender.Name.split('_')[0]);
            this._onMouseEnterCallback(geometryModel, mouseEventArgs);
        }
    },
    
    _OnMouseLeave: function(sender, mouseEventArgs)
    {
        if(this._onMouseLeaveCallback != null)
        {
            var geometryModel = GeometryManager_GetInstance().GetGeometryById(sender.Name.split('_')[0]);
            this._onMouseLeaveCallback(geometryModel, mouseEventArgs);
        }
    },
    
    SetOnMouseEnterCallback: function(callback)
    {
        this._onMouseEnterCallback = callback
    },
    
    SetOnMouseLeaveCallback: function(callback)
    {
        this._onMouseLeaveCallback = callback
    }
}

var g_InputManager = new InputManager();
function InputManager_GetInstance()
{
    return g_InputManager;
}

function _InputManager_MouseEnter(sender, mouseEventArgs)
{
    g_InputManager._OnMouseEnter(sender, mouseEventArgs);
}

function _InputManager_MouseLeave(sender, mouseEventArgs)
{
    g_InputManager._OnMouseLeave(sender, mouseEventArgs);
}