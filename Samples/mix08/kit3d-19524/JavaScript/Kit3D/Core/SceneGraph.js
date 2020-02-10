//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

/*
A scenegraph contains scene nodes, that describe the transforms to apply
to different geometry models.  You can create a hierarchy of scene nodes
where a parent scene node will affect the transform of all of its children. For
example you may want to animate an arm which has two bones and a hand that all
move relative to one another, you can do that using a scenenode
*/

function SceneGraph()
{
    this.Root = new SceneNode(null);
    this._viewPort = null;
}

SceneGraph.prototype =
{    
    ToString: function()
    {
        return 'SceneGraph';
    },
    
    SetViewPort: function(viewPort)
    {
        this._viewPort = viewPort;
        this._UpdateViewPort(this.Root);
    },
    
    _UpdateViewPort: function(sceneNode)
    {
        sceneNode.SetViewPort(this._viewPort);
        var childNodes = sceneNode.ChildNodes;
        for(var i=0; i<childNodes.length; ++i)
        {
            this._UpdateViewPort(childNodes[i]);
        }
    }
}