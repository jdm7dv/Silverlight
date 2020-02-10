//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function _GeometryModel(id)
{   
    //An array of all of the individual vertices required to
    //make up the geometry model
    this._vertices = null;
    
    //Currently use a bounding sphere for culling / collision, these values
    //are in local co-ordinates
    this.BoundingRadius = 0;
    this.BoundingCentroid = null;
    
    //Each geometry model is made up of a number of faces
    this.Faces = new Array();
    
    //The face can have both a front and back material, depending on the 
    //direction of the face relative to the camera, either the front or
    //the back material may be used.  If no back material is set, then the
    //face will be culled when the back face is facing the camera
    this._frontMaterial = null;
    this._backMaterial = null;
    
    //This is the container for all of the faces
    this._canvas = XamlFactory_GetInstance().GetElement("RenderTarget");
    
    //Get a unique id for this object
    this._id = id
    
    //The scene node that contains the geometry
    this._sceneNode = null;
}

_GeometryModel.prototype =
{
    //If a caller wants to reuse a model, they can clone it
    //then add that model to the canvas
    Clone: function()
    {
        var g = GeometryManager_GetInstance().CreateGeometry();
        
        g._vertices = new Array();
        for(var i=0; i<this._vertices.length; ++i)
        {
            g._vertices[i] = this._vertices[i];
        }
        
        g.BoundingRadius = this.BoundingRadius;
        g.BoundingCentroid = this.BoundingCentroid;
        g._frontMaterial = this._frontMaterial;
        g._backMaterial = this._backMaterial;
        
        g.Faces = new Array(this.Faces.length);
        g._id = IdGenerator_GetInstance().NextId();
        for(var i = 0; i<this.Faces.length; ++i)
        {
            g.Faces[i] = this.Faces[i].Clone(g._id);
            g._canvas.children.add(g.Faces[i].Visual);
        }
        
        //We need a different unique id.
        g._sceneNode = null;
        
        return g;
    },
    
    SetVisible: function(visible)
    {
        for(var i=0; i<this.Faces.length; ++i)
        {
            this.Faces[i].SetVisible(visible);
        }
    },
    
    SetFrontMaterial: function(frontMaterial)
    {
        this._frontMaterial = frontMaterial;
        
        //Need to update all of the faces with the new material
        for(var i=0; i<this.Faces.length; ++i)
        {
            this.Faces[i].SetFrontMaterial(this._frontMaterial);
        }
    },
    
    SetBackMaterial: function(backMaterial)
    {
        this._backMaterial = backMaterial;
        for(var i=0; i<this.Faces.length; ++i)
        {
            this.Faces[i].SetBackMaterial(this._backMaterial);
        }
    },
    
    SetVertices: function(vertices)
    {
        //An array of all of the individual vertices required to
        //make up the geometry model
        this._vertices = vertices;
        
        //Calculate the bounding sphere center and the radius
        var x = 0;
        var y = 0;
        var z = 0;
        var currentVertice = null;
        this.BoundingRadius = 0;
        this.BoundingCentroid = null;
        
        for(i=0; i<this._vertices.length; ++i)
        {
            currentVertexPosition = this._vertices[i].Position;
            x += currentVertexPosition.X;
            y += currentVertexPosition.Y;
            z += currentVertexPosition.Z;
            if(Math.abs(currentVertexPosition.X) > this.BoundingRadius)
            {
                this.BoundingRadius = currentVertexPosition.X;
            }
            if(Math.abs(currentVertexPosition.Y) > this.BoundingRadius)
            {
                this.BoundingRadius = currentVertexPosition.Y;
            }
            if(Math.abs(currentVertexPosition.Z) > this.BoundingRadius)
            {
                this.BoundingRadius = currentVertexPosition.Z;
            }
        }
        
        if(this._vertices.length != 0)
        {
            var reciprocal = 1.0 / this._vertices.length;
            this.BoundingCentroid = new Vector4(x * reciprocal, y * reciprocal, z * reciprocal, 1);
        }
    },
    
    AddFace: function(v0Index, v1Index, v2Index)
    {
        var v0 = this._vertices[v0Index];
        var v1 = this._vertices[v1Index];
        var v2 = this._vertices[v2Index];
        
        this.Faces[this.Faces.length] = new Face(this._id,
                                                 this.Faces.length, 
                                                 v0, v1, v2,
                                                 this._frontMaterial,
                                                 this._backMaterial);
        
        //Want to add the faces visul representation inside the canvas element that contains
        //all of the geometry models faces
        this._canvas.children.add(this.Faces[this.Faces.length-1].Visual);
    },
    
    SetSceneNode: function(sceneNode)
    {
        this._sceneNode = sceneNode
    },
    
    GetSceneNode: function()
    {
        return this._sceneNode;
    },
    
    ToString: function()
    {
        return 'GeometryModel';
    }
}