//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

/*
A node in a scene graph.  Scene graphs can be use to move
objects in a related manner, such as a finger moving in
reference to a hand.
*/

function SceneNode(geometryModel)
{
    //The geometry that is associated with this particular node
    this.GeometryModel = geometryModel;
    
    if(this.GeometryModel != null)
    {
        this.GeometryModel.SetSceneNode(this);
    }
    
    //The local transform that affects the geometry model
    this._localTransform = Matrix4x4_IdentityMatrix();
    
    //The world transform that is applied to the local transform
    //to get the overall transform.  The world transform represent
    //the transform of the parent objects of this scene node
    this._worldTransform = Matrix4x4_IdentityMatrix();
    
    //List of all of the child nodes of this node
    this.ChildNodes = new Array();
    
    this._parent = null;
    this._viewPort = null
}

SceneNode.prototype = 
{
    AddChild: function(child)
    {
        if(child._parent !== null)
        {
            //TODO: Update parent and remove from child collection
            alert('TODO: Not supported moving scene nodes to new parents');
        }
        
        child._parent = this;
        this.ChildNodes[this.ChildNodes.length] = child;
        
        //Need to update the transforms of all the children of this node
        //relative to the parent
        child._UpdateWorldTransform();
    },
    
    /*TODO:
    RemoveChild: function(child)
    {
    }*/
    
    GetWorldTransform: function()
    {
        return this._worldTransform;
    },
    
    GetLocalTransform: function()
    {
        return this._localTransform;
    },
    
    SetLocalTransform: function(localTransform)
    {
        this._localTransform = localTransform;
        this._UpdateWorldTransform();
        
        if(this._viewPort != null)
        {
            //If this property changed then we want to indicate
            //to the containing viewport that the scene is dirty
            //and needs to be rendered
            this._viewPort.SetRenderDirty();
        }
    },
    
    //Causes the world transform to be updated, taking into account
    //the world transform of the parent and the local transform
    _UpdateWorldTransform: function()
    {
        if(this._parent === null)
        {
            this._worldTransform = this._localTransform;
        }
        else
        {
            this._worldTransform = this._localTransform.Multiply(this._parent._worldTransform);
        }
        
        //Need to update the transform on all of the child nodes as well
        for(var i=0; i<this.ChildNodes.length; ++i)
        {
            this.ChildNodes[i]._UpdateWorldTransform();
        }
    },
    
    SetViewPort: function(viewPort)
    {
        this._viewPort = viewPort;
    },
    
    ToString: function()
    {
        return 'SceneNode';
    }
}