//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function DebugHelper()
{
    this._debugText = XamlFactory_GetInstance().GetElement("DebugText");
}

var g_debugHelper = null;
function DebugHelper_GetInstance()
{
    if(g_debugHelper == null)
    {
        g_debugHelper = new DebugHelper();
    }
    
    return g_debugHelper;
}

DebugHelper.prototype = 
{
    Print: function(message)
    {
        this._debugText.Visibility = "Visible";
        this._debugText.Text = message;
    }
}