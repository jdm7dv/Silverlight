Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Tasks

Namespace ArcGISSilverlightSDK
    Partial Public Class Lengths
        Inherits UserControl
        Private MyDrawObject As Draw

        Public Sub New()
            InitializeComponent()
            MyDrawObject = New Draw(MyMap) With {.DrawMode = DrawMode.Polyline, .IsEnabled = True, .LineSymbol = DefaultLineSymbol}
            AddHandler MyDrawObject.DrawComplete, AddressOf MyDrawObject_DrawComplete
            AddHandler MyDrawObject.DrawBegin, AddressOf MyDrawObject_DrawBegin
        End Sub

        Private Sub MyDrawObject_DrawBegin(ByVal sender As Object, ByVal args As EventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()
        End Sub

        Private Sub MyDrawObject_DrawComplete(ByVal sender As Object, ByVal args As DrawEventArgs)
            Dim polyline As ESRI.ArcGIS.Client.Geometry.Polyline = TryCast(args.Geometry, ESRI.ArcGIS.Client.Geometry.Polyline)
            polyline.SpatialReference = New SpatialReference(4326)
            Dim graphic As New Graphic() With {.Symbol = DefaultLineSymbol, .Geometry = polyline}

            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.Graphics.Add(graphic)

            Dim geometryService As New GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Geometry/GeometryServer")
            AddHandler geometryService.ProjectCompleted, AddressOf GeometryService_ProjectCompleted
            AddHandler geometryService.Failed, AddressOf GeometryService_Failed

            Dim graphicList As New List(Of Graphic)()
            graphicList.Add(graphic)

            ' GeometryService.Lengths returns distances in the units of the spatial reference.
            ' The units in the map view's projection is decimal degrees. 
            ' Use the Project method to convert graphic points to a projection that uses a measured unit (e.g. meters).
            ' If the map units are in measured units, the call to Project is unnecessary.
            ' Important: Use a projection appropriate for your area of interest.
            geometryService.ProjectAsync(graphicList, New SpatialReference(32611))
        End Sub

        Private Sub GeometryService_ProjectCompleted(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.Tasks.GraphicsEventArgs)
            Dim geometryService As New GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Geometry/GeometryServer")
            AddHandler geometryService.LengthsCompleted, AddressOf GeometryService_LengthsCompleted

            ' Execute the Lengths method using the returned projected graphics
            geometryService.LengthsAsync(args.Results)
        End Sub

        Private Sub GeometryService_LengthsCompleted(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.Tasks.LengthsEventArgs)
            ' convert results from meters into miles for our display
            Dim miles As Double = args.Results(0) * 0.0006213700922
            ResponseTextBlock.Text = String.Format("The distance of the polyline is {0} miles.", Math.Round(miles, 3))
        End Sub

        Private Sub GeometryService_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
            MessageBox.Show("Geometry Service error: " & e.Error.Message)
        End Sub
    End Class
End Namespace
