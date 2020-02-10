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
Imports System.Windows.Media.Imaging
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Tasks

Namespace ArcGISSilverlightSDK
    Partial Public Class BufferQuery
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MyMap_MouseClick(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Map.MouseEventArgs)
            Dim clickGraphic As New Graphic()
            clickGraphic.Symbol = DefaultMarkerSymbol
            clickGraphic.Geometry = e.MapPoint
            ' Input spatial reference for buffer operation defined by first feature of input geometry array
            clickGraphic.Geometry.SpatialReference = MyMap.SpatialReference

            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()

            clickGraphic.SetZIndex(2)
            graphicsLayer.Graphics.Add(clickGraphic)

            Dim geometryService As New GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Geometry/GeometryServer")
            AddHandler geometryService.BufferCompleted, AddressOf GeometryService_BufferCompleted
            AddHandler geometryService.Failed, AddressOf GeometryService_Failed

            ' Important: Use a projection appropriate for your area of interest.
            Dim bufferParams As New ESRI.ArcGIS.Client.Tasks.BufferParameters() With {.BufferSpatialReference = New SpatialReference(32610), .OutSpatialReference = MyMap.SpatialReference, .Unit = LinearUnit.Meter}
            bufferParams.Distances.Add(100)
            bufferParams.Features.Add(clickGraphic)

            geometryService.BufferAsync(bufferParams)
        End Sub

        Private Sub GeometryService_BufferCompleted(ByVal sender As Object, ByVal args As GraphicsEventArgs)
            Dim bufferGraphic As New Graphic()
            bufferGraphic.Geometry = args.Results(0).Geometry
            bufferGraphic.Symbol = BufferSymbol
            bufferGraphic.SetZIndex(1)

            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.Graphics.Add(bufferGraphic)

            Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Portland/Portland_ESRI_LandBase_AGO/MapServer/1")
            AddHandler queryTask.ExecuteCompleted, AddressOf QueryTask_ExecuteCompleted
            AddHandler queryTask.Failed, AddressOf QueryTask_Failed

            Dim query As New ESRI.ArcGIS.Client.Tasks.Query()
            query.ReturnGeometry = True
            query.Geometry = bufferGraphic.Geometry
            queryTask.ExecuteAsync(query)
        End Sub

        Private Sub QueryTask_ExecuteCompleted(ByVal sender As Object, ByVal args As QueryEventArgs)
            If args.FeatureSet Is Nothing Then
                Return
            End If

            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            For Each selectedGraphic As Graphic In args.FeatureSet.Features
                selectedGraphic.Symbol = ParcelSymbol
                graphicsLayer.Graphics.Add(selectedGraphic)
            Next selectedGraphic
        End Sub

        Private Sub GeometryService_Failed(ByVal sender As Object, ByVal args As TaskFailedEventArgs)
            MessageBox.Show("Geometry service failed: " & args.Error.Message)
        End Sub

        Private Sub QueryTask_Failed(ByVal sender As Object, ByVal args As TaskFailedEventArgs)
            MessageBox.Show("Query failed: " & args.Error.Message)
        End Sub

    End Class
End Namespace
