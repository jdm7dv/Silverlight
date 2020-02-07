//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function GeometryManager()
{
    this._geometryList = new Array();
}

GeometryManager.prototype = 
{
    CreateGeometry: function()
    {
        var gId = IdGenerator_GetInstance().NextId();
        var g = new _GeometryModel(gId);
        
        //Will refer to the geometry model by its id as a string
        this._geometryList[gId + ''] = g;
        return g;
    },
    
    GetGeometryById: function(id)
    {
        return this._geometryList[id];
    }
}

var g_geometryManager = new GeometryManager();
function GeometryManager_GetInstance()
{
    return g_geometryManager;
}