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
Imports System.Windows.Media.Imaging
Imports System.Windows.Shapes
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Tasks

Namespace ArcGISSilverlightSDK
    Partial Public Class BufferPoint
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MyMap_MouseClick(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Map.MouseEventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()

            e.MapPoint.SpatialReference = New SpatialReference(4326)
            Dim graphic As Graphic = New ESRI.ArcGIS.Client.Graphic() With {.Geometry = e.MapPoint, .Symbol = DefaultClickSymbol}
            graphic.SetZIndex(1)
            graphicsLayer.Graphics.Add(graphic)

            Dim geometryService As New GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Geometry/GeometryServer")
            AddHandler geometryService.BufferCompleted, AddressOf GeometryService_BufferCompleted
            AddHandler geometryService.Failed, AddressOf GeometryService_Failed

            ' Important: Use a projection appropriate for your area of interest.
            Dim bufferParams As New BufferParameters() With {.Unit = LinearUnit.StatuteMile, .BufferSpatialReference = New SpatialReference(32611), .OutSpatialReference = MyMap.SpatialReference}
            bufferParams.Features.Add(graphic)
            bufferParams.Distances.Add(10)

            geometryService.BufferAsync(bufferParams)
        End Sub

        Private Sub GeometryService_BufferCompleted(ByVal sender As Object, ByVal args As GraphicsEventArgs)
            Dim results As IList(Of Graphic) = args.Results
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)

            For Each graphic As Graphic In results
                graphic.Symbol = DefaultBufferSymbol
                graphicsLayer.Graphics.Add(graphic)
            Next graphic
        End Sub

        Private Sub GeometryService_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
            MessageBox.Show("Geometry Service error: " & e.Error.Message)
        End Sub
    End Class
End Namespace
