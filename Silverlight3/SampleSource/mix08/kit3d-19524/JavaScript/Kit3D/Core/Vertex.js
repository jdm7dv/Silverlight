//Kit3D - Copyright Mark Dawson
//If you want to reuse, please see the license terms at: http://www.codeplex.com/Kit3D/Project/License.aspx

function Vertex(position, r, g, b, u, v)
{
    this.Position = position;
    this.Red = r;
    this.Green = g;
    this.Blue = b;
    this.U = u;
    this.V = v;
}

Vertex.prototype = 
{
    ToString: function()
    {
        return 'Vertex[r=' + this.R + ', g=' + this.G + ', b=' + this.B + 
               ', u=' + this.U + ', v=' + this.V + ', position=' + this.Position.ToString() + ']';
    }
}