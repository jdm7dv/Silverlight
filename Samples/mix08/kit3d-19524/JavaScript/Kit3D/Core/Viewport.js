//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function Viewport(x, y, width, height, camera)
{
    this._width = width;
    this._height = height;
    this._x = x;
    this._y = y;
    
    //Indicates if we need to render or not
    this._isDirty = true;
    
    //The distances of the near and the far plane, items outside of these will be culled
    this._far = 2000;
    this._near = 1;
    
    //The scengraph contains all of the GeometryModels that are to be
    //displayed in the viewport
    this._sceneGraph = null;
    
    //This transform, converts world co-ordinates to screen co-ordinates
    this._worldToScreenTransform = Matrix4x4_IdentityMatrix();
    
    //Indicates that we want to clip models against the view frustum
    this._frustumCulling = true;
    
    //View frustum, will is used to cull objects that are not visible
    this._frustum = null;
    
    //Geometry that is not outside of the view frustum
    this._visibleGeometry = new Array();
    
    //The camera is used to control what the viewer sees
    this._camera = null;
    this.SetCamera(camera);
}

Viewport.prototype = 
{    
    SetRenderDirty: function()
    {
        //If this flag is true and the caller calls the Render
        //method the scene will be rendered, if this flag is false
        //the render method immediately will return.
        this._isDirty = true;
    },
    
    //The user can turn frustum culling on or off, this might make sense
    //if the caller knows all objects are in the view area
    SetFrustumCulling: function(enable)
    {
        this._frustumCulling = enable;
    },
    
    //TODO: Verify we can set a new camera
    SetCamera: function(camera)
    {
        this._camera = camera;
        this._UpdateTransform();
        
        //Create the new view Frustum, this is used
        //to help clip objects that are not visible
        this._frustum = new Frustum(this);
        
        this._isDirty = true;
    },
    
    GetCamera: function()
    {
        return this._camera;
    },
    
    GetNearDistance: function()
    {
        return this._near;
    },
    
    GetFarDistance: function()
    {
        return this._far;
    },
    
    GetWidth: function()
    {
        return this._width;
    },
    
    GetHeight: function()
    {
        return this._height;
    },
    
    GetSceneGraph: function()
    {
        return this._sceneGraph;
    },
    
    SetSceneGraph: function(sceneGraph)
    {
        this._sceneGraph = sceneGraph;
        this._sceneGraph.SetViewPort(this);
        this._isDirty = true;
    },

    //Calling render will dra the contents associated with the scene
    //graph on the screen
    Render: function()
    {        
        //If nothing has changed since last time, there is no need 
        //to render anything, this way we are not using up all of
        //the processor unnecessarily when we do not need to render
        if(!this._isDirty)
        {
           return;
        }
        
        //This will store all of the geometry that is not clipped
        this._visibleGeometry = new Array();
        
        //Clips the geometry to the view Frustum
        this._ClipGeometry(this._sceneGraph.Root);

        //Draw all of the vertices
        for(var i=0; i<this._visibleGeometry.length; ++i)
        {
            this._RenderNode(this._visibleGeometry[i]);
        }
        
        //Assuming nothing asyncronous is executing while we render
        this._isDirty = false;
    },
    
    _ClipGeometry: function(sceneNode)
    {
        var geometry = sceneNode.GeometryModel;
        if(geometry != null)
        {
            if(!this._frustumCulling || this._frustum.ContainsGeometry(geometry, sceneNode.GetWorldTransform()) == Frustum_Inside)
            {
                this._visibleGeometry[this._visibleGeometry.length] = sceneNode;
            }
            else
            {
                //Need to set all of the faces to invisible, ideally would just set
                //the geometry model to invisible if each geometry model was inside 
                //it's own canvas but then ZIndex would not work across geometry models
                geometry.SetVisible(false);
            }
        }
        
        //Look at all of the children and see if they need to be clipped
        var children = sceneNode.ChildNodes;
        for(var i=0; i<children.length; ++i)
        {
            this._ClipGeometry(children[i]);
        }
    },
    
    _DrawFace: function(x0, y0, x1, y1, x2, y2, centroidZ, currentFace)
    {            
        //x0, y0, x1, y1, x2, y2 - are the points of the face set into                       
        currentFace.SetScreenPoints(x0, y0, x1, y1, x2, y2);
        currentFace.SetZIndex(centroidZ);
        currentFace.SetVisible(true);
    },
    
    _RenderNode: function(sceneNode)
    {
        var model = sceneNode.GeometryModel;
        if(model != null)
        {
            //Need to draw all of the faces associated with the geometry model
            var faces = model.Faces;
            var numberOfFaces = faces.length;
            var currentFace = null;
            var cameraPosition = new Vector4(this._camera.Position.X, this._camera.Position.Y, this._camera.Position.Z, 1)
            
            var v1p = new Vector3(1,1,1);
            var v2p = new Vector3(1,1,1);
            var v3p = new Vector3(1,1,1);
            //This transform, converts from local object space to projected screen space
            var localToScreen = sceneNode.GetWorldTransform().Multiply(this._worldToScreenTransform);
            for(var i=0; i<numberOfFaces; ++i)
            {
                currentFace = faces[i];
                var v1 = currentFace.V0.Position;
                var v2 = currentFace.V1.Position;
                var v3 = currentFace.V2.Position;
                
                var v1temp = localToScreen.PreMultiplyVector(new Vector4(v1.X, v1.Y, v1.Z, 1));
                var w1 = 1.0/v1temp.W;
                //This is the projected point on the screen
                v1p.X = v1temp.X * w1;
                v1p.Y = v1temp.Y * w1;
                v1p.Z = v1temp.Z * w1;
                
                var v2temp = localToScreen.PreMultiplyVector(new Vector4(v2.X, v2.Y, v2.Z, 1));
                var w2 = 1.0/v2temp.W;
                v2p.X = v2temp.X * w2;
                v2p.Y = v2temp.Y * w2;
                v2p.Z = v2temp.Z * w2;
                                      
                var v3temp = localToScreen.PreMultiplyVector(new Vector4(v3.X, v3.Y, v3.Z, 1));
                var w3 = 1.0/v3temp.W;
                v3p.X = v3temp.X * w3;
                v3p.Y = v3temp.Y * w3;
                v3p.Z = v3temp.Z * w3;
                
                if (v2p.Subtract(v1p).CrossProduct(v3p.Subtract(v1p)).DotProduct(this._camera.ViewDirection) > 0)
                {
                    currentFace.UseFrontMaterial();
                    var centroid = sceneNode.GetWorldTransform().PreMultiplyVector(currentFace.Centroid);
                    var z = centroid.Subtract(cameraPosition).Length();
                  
                    //Want to keep the z index in the negative range of values for silverlight
                    //so the largest distance possible from the camera is the far plane, we can
                    //scale all values using this knowledge, to be inbetween 0 and the min value
                    //allowed by silverlight which is the min value of a negative integer
                    z = Math.floor(-2000000000 * (z / this._far));
        
                    this._DrawFace(v1p.X, v1p.Y,
                                   v2p.X, v2p.Y,
                                   v3p.X, v3p.Y,
                                   z, 
                                   currentFace);
                }
                else
                {
                    //If the back face does not have a back material, we will cull it
                    //otherwise will use the back material to display it
                    if(currentFace.GetBackMaterial() == null)
                    {
                        currentFace.SetVisible(false);
                    }
                    else
                    {
                        currentFace.UseBackMaterial();
                        
                        var centroid = sceneNode.GetWorldTransform().PreMultiplyVector(currentFace.Centroid);
                        var z = centroid.Subtract(cameraPosition).Length();
                        z = Math.floor(-2000000000 * (z / this._far));
                        this._DrawFace(v1p.X, v1p.Y,
                                       v2p.X, v2p.Y,
                                       v3p.X, v3p.Y,
                                       z,
                                       currentFace);
                    }
                }
            } //for
        } //if
    },

    _UpdateTransform: function()
    {
        //Based on the location of the camera we need to derived
        //the world co-ordinate transform to screen space
        
        var direction = this._camera.ViewDirection;
        var up = this._camera.Up.Subtract(this._camera.ViewDirection.MultiplyScalar(this._camera.Up.DotProduct(this._camera.ViewDirection)));
            
        var side = direction.CrossProduct(up).Normalize();

        //Set the rotation matrices
        var worldToView = Matrix4x4_IdentityMatrix();
        worldToView.SetColumns(new Vector4(side.X, side.Y, side.Z, 0), 
                               new Vector4(up.X, up.Y, up.Z, 0), 
                               new Vector4(direction.X, direction.Y, direction.Z, 0),
                               new Vector4(0,0,0,1));

        //Update the position matrices
        var pos = worldToView.PreMultiplyVector(new Vector4(this._camera.Position.X,
                                                            this._camera.Position.Y,
                                                            this._camera.Position.Z, 1)).Multiply(-1);
        worldToView.SetRow(pos, 3);

        //Takes the view coordinates and projects them onto the view plane
        //Need to calculate the distance to the view window with the specified fov
        //The view plane is 2 units high, so knowing this we can calculate the distance
        //from the camera to the view plane
        var d = (1.0 / Math.tan(this._camera.FieldOfView / 2));

        //Need the aspect ratio of the viewport
        var aspectRatio = this._width / this._height;

        //This is the projection matrix that will convert into NDC co-ordinates between -1 and 1 in the x,y plane
        var projection = new Matrix4x4(d / aspectRatio, 0, 0, 0,
                                       0, d, 0, 0,
                                       0, 0, this._far / (this._far - this._near), -1,
                                       0, 0, -(this._near * this._far) / (this._far - this._near), 0);


        //Create the NDC to screen matrix, know NDC is kept at 2 units high
        // y' = - (hs / 2) * Yndc + hs/2 + sx
        // x' = (ws / 2) * Xndc + ws/2 + sy
        // z' = (ds / 2) * Zndc + ds/2   //ds is [0,1] where our ndc z was from 0 to 1
        var screenDepth = 1;

        var ndcToScreen = new Matrix4x4(this._width / 2, 0, 0, 0,
                                        0, this._height / 2, 0, 0,
                                        0, 0, screenDepth, 0,
                                        this._width / 2 + this._x, this._height / 2 + this._y, screenDepth, 1);

        //Need to define the total overall transformation
        //model->world->view->projection->screen
        this._worldToScreenTransform = worldToView.Multiply(projection).Multiply(ndcToScreen);
    },
    
    ToString: function()
    {
        return 'Viewport';
    }
}