//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function AnimationManager()
{
    this._frameRate = 20;
    
    //Get reference to the animation storyboard
    this._storyBoard = XamlFactory_GetInstance().GetElement("AnimationLoop");
    
    //For debugging purposes
    this._frameRateCounter = XamlFactory_GetInstance().GetElement("FrameRateCounter");
    
    if(this._storyBoard == null)
    {
        alert('Error - need to make sure you have a <StoryBoard> element in the canvas');
    }
    
    //This is a user callback so that they can receive notification
    //of each frame that occurs
    this._onFrameCompleteCallback = null;
    this._frameCount = 0;
    this._paused = true;
    this._startTime = 0;
    
    //Will set the duration property of the StoryBoard element
    this.SetFrameRate(this._frameRate);
}

var g_animationManager = null;

//Animation manager is a singleton and should be accessed through
//a call to this function
function AnimationManager_GetInstance()
{
    if(g_animationManager == null)
    {
        g_animationManager = new AnimationManager();
    }
    
    return g_animationManager;
}

//This is called each time the animation storyboard completes
//it will forward the frame completed event into the internal handler
function AnimationManager_OnFrameComplete(sender, eventArgs)
{
    AnimationManager_GetInstance()._OnFrameComplete();
}

AnimationManager.prototype = 
{
    Start: function()
    {
        this._paused = false;
        this._startTime = (new Date()).getTime();
        this._frameCount = 0;
        this._storyBoard.Begin();
    },
    
    Stop: function()
    {
        this._paused = true;
    },
    
    SetFrameRate: function(frameRate)
    {
        this._frameRate = frameRate;
        this._storyBoard.duration = "00:00:" + Math.round(100 * (1.0/this._frameRate)) / 100.0;
    },
    
    GetFrameRate: function()
    {
        return this._frameRate;
    },
    
    SetOnFrameCompleteCallback: function(callback)
    {
        this._onFrameCompleteCallback = callback;
    },
    
    ShowFrameRateCounter: function(show)
    {
        this._frameRateCounter.Visibility = (show) ? 'Visible' : 'Collapsed';
    },
    
    _OnFrameComplete: function()
    {
        //Call the callback the user has specified, if they wanted to 
        //receive notification that a frame has completed.
        if(this._onFrameCompleteCallback != null)
        {
            this._onFrameCompleteCallback(this._frameCount);
        }
        
        //TODO:Do better than this
        if(this._frameCount > 10000000)
        {
            this._frameCount = 0;
        }
        
        this._frameCount++;
        this._frameRateCounter.text = (this._frameCount / (((new Date()).getTime() - this._startTime) / 1000)) + '';
        
        //Make sure we restart the storyboard for the next frame
        //unless the caller paused the animation
        if(!this._paused) this._storyBoard.Begin(); 
    },
    
    ToString: function()
    {
        return 'AnimationManager';
    }
}