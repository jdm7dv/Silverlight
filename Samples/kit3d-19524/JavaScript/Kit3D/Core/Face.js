//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function Face(geometryId, faceId, v0, v1, v2, frontMaterial, backMaterial)
{
    this.V0 = v0;
    this.V1 = v1;
    this.V2 = v2;
    this._geometryId = geometryId;
    this._faceId = faceId;
    
    //Track the centroid of the face
    this.Centroid = v0.Position.Add(v1.Position).Add(v2.Position).MultiplyScalar(1.0/3);
    this.Centroid = new Vector4(this.Centroid.X, this.Centroid.Y, this.Centroid.Z, 1);
    
    //Because a meterial is really something like an ImageBrush, we need to
    //make sure that we clone the material for each face, because we will be
    //setting different properties on the brushes for each face
    this._frontMaterial = (frontMaterial != null) ? frontMaterial.Clone() : null;
    if(this._frontMaterial != null)
    {
        this._frontMaterial.SetVertices(this.V0, this.V1, this.V2);
    }
    
    this._backMaterial = (backMaterial != null) ? backMaterial.Clone() : null;
    if(this._backMaterial != null)
    {
        this._backMaterial.SetVertices(this.V0, this.V1, this.V2);
    }
    this._currentMaterial = null;
                        
    //This creates a Polygon element and adds it to the canvas  
    this.Visual = XamlFactory_GetInstance().CreatePolygon(this._geometryId + '_' + this._faceId);
}

Face.prototype = 
{
    SetScreenPoints: function(x0, y0, x1, y1, x2, y2)
    {
        //x0, y0, x1, y1, x2, y2 are the projected screen points for 
        //each of the vertices
        this.Visual.Points = x0 + ',' + y0 + ' ' + x1 + ',' + y1 + ' ' + x2 + ',' + y2;
        
        if(this._currentMaterial != null)
        {
            //The material may need to know about the new points of the polygon
            //for example the TextureMaterial needs to know this in order to 
            //map correctly to the new polygon face
            this._currentMaterial.SetScreenValues(x0, y0, x1, y1, x2, y2);
        }
    },
    
    Clone: function(geometryId)
    {
        return new Face(geometryId,
                        IdGenerator_GetInstance().NextId(),
                        this.V0, 
                        this.V1, 
                        this.V2,
                        this._frontMaterial, 
                        this._backMaterial);
    },
    
    SetZIndex: function(zIndex)
    {
        //This is a slow function call - need to see if
        //rendering the DOM offline is faster.
        this.Visual['Canvas.ZIndex'] = zIndex;
    },
    
    SetVisible: function(visible)
    {
        this.Visual.Visibility = (visible) ? "visible" : "Collapsed";
    },
    
    GetBackMaterial: function()
    {
        return this._backMaterial;
    },
    
    SetFrontMaterial: function(frontMaterial)
    {
        if(this._currentMaterial == this._frontMaterial)
        {
            this._currentMaterial = frontMaterial;
        }
        this._frontMaterial = frontMaterial;
    },
    
    SetBackMaterial: function(backMaterial)
    {
        if(this._currentMaterial == this._backMaterial)
        {
            this._currentMaterial = backMaterial;
        }
        this._backMaterial = backMaterial;
    },
    
    //The front material should be used to draw onto the face
    UseFrontMaterial: function()
    {
        this._SetCurrentMaterial(this._frontMaterial);
    },
    
    UseBackMaterial: function()
    {
        this._SetCurrentMaterial(this._backMaterial);
    },
    
    _SetCurrentMaterial: function(material)
    {
        if(this._currentMaterial != material)
        {
            this._currentMaterial = material;
            
            if(this._currentMaterial != null)
            {
                this.Visual.fill = this._currentMaterial.GetBrush();
            }
        }
    },
    
    ToString: function()
    {
        return 'Face';
    }
}