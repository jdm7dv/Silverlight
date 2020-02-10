//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function IdGenerator()
{
    this._id = 0;
}

IdGenerator.prototype =
{
    NextId: function()
    {
        return this._id++;
    }
}

var g_idGenerator = null;
function IdGenerator_GetInstance()
{
    if(g_idGenerator == null)
    {
        g_idGenerator = new IdGenerator();
    }
    return g_idGenerator;
}