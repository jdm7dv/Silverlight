//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

//The XamlFactory is used to create all of the Xaml related elements
//TODO: Change name
function XamlFactory(silverlightControl)
{
    this._silverlightControl = silverlightControl;
}

var g_xamlFactory = null;

//The XamlManager is a singleton instance.
function XamlFactory_GetInstance(silverlightControl)
{
    if(g_xamlFactory == null)
    {
        g_xamlFactory = new XamlFactory(silverlightControl);
    }
    
    return g_xamlFactory;
}

XamlFactory.prototype = 
{
    CreateImageBrush: function(imageUri)
    {
        var xamlString = '<ImageBrush AlignmentX="Left" AlignmentY="Top" Stretch="None" ImageSource="' + imageUri + '" >' +
                             '<ImageBrush.Transform>' + 
                                 '<MatrixTransform>' +
                                     '<MatrixTransform.Matrix>' +
                                         '<Matrix M11="1" M12="0" M21="0" M22="1" OffsetX="0" OffsetY="0" />' +
                                     '</MatrixTransform.Matrix>' +
                                 '</MatrixTransform>' +
                             '</ImageBrush.Transform>' +
                         '</ImageBrush>';
                
        var imageBrush = this._CreateXaml(xamlString);
        return imageBrush;
    },
    
    CreateSolidBrush: function(color)
    {
        var xamlString = '<SolidColorBrush Color="' + color + '" />'
        return this._CreateXaml(xamlString);
    },
    
    CreatePolygon: function(name)
    {
        var xamlString = '<Polygon xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" MouseEnter="_InputManager_MouseEnter" MouseLeave="_InputManager_MouseLeave" x:Name="' + name + '" Visibility="Visible" Canvas.ZIndex="1" Points="" />';
        var polygon = this._CreateXaml(xamlString);
        return polygon;
    },
    
    CreateCanvas: function()
    {
        var xamlString = '<Canvas xmlns="http://schemas.microsoft.com/client/2007" ' +
                                  'xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" ' +
                                  'Visibility="Visible" />';
        var canvas = this._CreateXaml(xamlString);
        return canvas;
    },
    
    GetElement: function(elementName)
    {
        return this._silverlightControl.findName(elementName);
    },
    
    _CreateXaml: function(xaml)
    {
        return this._silverlightControl.getHost().content.createFromXaml(xaml);
    }
}