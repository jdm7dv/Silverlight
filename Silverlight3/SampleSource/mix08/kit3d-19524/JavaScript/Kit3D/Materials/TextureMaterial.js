//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

/*
A texture material allows us to map images onto faces of a 
geometry model
*/

function TextureMaterial(textureUri, width, height)
{        
    this._textureUri = textureUri; 
    this._width = width;
    this._height = height;       
    this._imageBrush = XamlFactory_GetInstance().CreateImageBrush(this._textureUri);
    
    //The first step is to calculate a matrice that will allow us to go from
    //UV space to texture space.  A value of u,v 0,0 coresponds to a texture
    //co-ord of 0,0.  A value of u,v 1,1 corresponds to a texture co-ord
    //of width,height
    
    var v0 = new Vertex(new Vector3(0, 0, 1), 0, 0, 0, 0.0, 0.0);
    var v1 = new Vertex(new Vector3(this._width, 0, 1), 0, 0, 0, 1.0, 0.0);
    var v2 = new Vertex(new Vector3(0, this._height, 1), 0, 0, 0, 0.0, 1.0);
    
    var mUV = new Matrix3x3(v1.U - v0.U, v1.V - v0.V, 0,
                            v2.U - v0.U, v2.V - v0.V, 0,
                            v0.U,        v0.V,         1);
    var mUVInv = mUV.Inverse();
        
    var mT = new Matrix3x3(v1.Position.X - v0.Position.X, v1.Position.Y - v0.Position.Y, 0,
                           v2.Position.X - v0.Position.X, v2.Position.Y - v0.Position.Y, 0,
                           v0.Position.X,                 v0.Position.Y,                 1);
    var mTInv = mT.Inverse();
    this._uvToTexture = mUVInv.Multiply(mT);
}

TextureMaterial.prototype = 
{
    GetBrush: function()
    {
        return this._imageBrush;
    },
    
    Clone: function()
    {
        return new TextureMaterial(this._textureUri, 
                                   this._width,
                                   this._height);
    },
    
    ToString: function()
    {
        return 'TextureMaterial [Uri=' + this._textureUri + ']';
    },
    
    
    SetVertices: function(v0, v1, v2)
    {
        //Need to convert the UV values of the vertices into texture space
        var v0Tex = this._uvToTexture.PreMultiplyVector(new Vector3(v0.U, v0.V, 1));
        var v1Tex = this._uvToTexture.PreMultiplyVector(new Vector3(v1.U, v1.V, 1));
        var v2Tex = this._uvToTexture.PreMultiplyVector(new Vector3(v2.U, v2.V, 1));
        
        var mTx = new Matrix3x3(v1Tex.X - v0Tex.X, v1Tex.Y - v0Tex.Y, 0,
                                v2Tex.X - v0Tex.X, v2Tex.Y - v0Tex.Y, 0,
                                v0Tex.X,           v0Tex.Y,           1);
        this._mTxInv = mTx.Inverse();
    },
    
    SetScreenValues: function(x0, y0, x1, y1, x2, y2)
    {
        //x0, y0, x1, y1, x2, y2 are the projected screen co-ordinates of the shape this
        //texture maps to
        
        //Now we have to map the texture co-ordinates of the triangle that the
        //texture is associated with to the projected screen co-ordinates
        
        //The texture space starts at the top left most position of this triangle
        var pmin = new Vector3(Math.min(x0, Math.min(x1, x2)),
                               Math.min(y0, Math.min(y1, y2)), 1);
                               
        //The image brush is aligned at the top left of the set of points associated with
        //the polygon we are drawing, so to turn the projected screen co-ordinates into 
        //texture space we just have to subtract the min value (i.e. top left)
        var p0New = (new Vector3(x0, y0, 1)).Subtract(pmin);
        var p1New = (new Vector3(x1, y1, 1)).Subtract(pmin);
        var p2New = (new Vector3(x2, y2, 1)).Subtract(pmin);
    
        var mTxPrime = new Matrix3x3(p1New.X - p0New.X, p1New.Y - p0New.Y, 0,
                                     p2New.X - p0New.X, p2New.Y - p0New.Y, 0,
                                     p0New.X,           p0New.Y,           1);
        
        var textToPoly = this._mTxInv.Multiply(mTxPrime); 
   
        //Update the matrix associated with the image brush, this will give
        //us an affine texture mapping
        var matrix = this._imageBrush.transform.Matrix;
        matrix.M11 = textToPoly._values[0];
        matrix.M12 = textToPoly._values[1];
        matrix.M21 = textToPoly._values[3];
        matrix.M22 = textToPoly._values[4];
        matrix.OffsetX = textToPoly._values[6];
        matrix.OffsetY = textToPoly._values[7];
    },
    
    ToString: function()
    {
        return 'TextureMaterial';
    }
}  