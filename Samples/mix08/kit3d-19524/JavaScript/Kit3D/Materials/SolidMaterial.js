//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function SolidMaterial()
{
    this._brush = XamlFactory_GetInstance().CreateSolidBrush("Black");
}

SolidMaterial.prototype = 
{
    //The solid brush currently just uses the color of the first vertice
    //passed in to be used as the fill color
    //TODO: Improve
    SetVertices: function(v0, v1, v2)
    {
        this._brush.color = this._RgbToHex(v0.Red, v0.Green, v0.Blue);
    },
    
    SetScreenValues: function(x0, y0, x1, y1, x2, y2)
    {
    },
    
    _RgbToHex: function(r, g, b)
    {
        return '#' + this._ToHex(r) + this._ToHex(g) + this._ToHex(b);
    },
    
    _ToHex: function(n)
    {
        if (n==null) 
        {
           return "00";
        }
        n=parseInt(n); 
        
        if(n==0 || isNaN(n))
        {
            return "00";
        }
        
        n=Math.max(0,n); 
        n=Math.min(n,255);
        n=Math.round(n);
        return "0123456789ABCDEF".charAt((n-n%16)/16) + "0123456789ABCDEF".charAt(n%16);
    },
    
    GetBrush: function()
    {
        return this._brush;
    },
    
    Clone: function()
    {
        return new SolidMaterial();
    },
    
    ToString: function()
    {
        return 'SolidMaterial';
    }
}