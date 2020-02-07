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
Imports System.Text
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Tasks

Namespace ArcGISSilverlightSDK
    Partial Public Class Relation
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            AddHandler MyMap.Layers.LayersInitialized, AddressOf Layers_LayersInitialized
        End Sub

        Private Sub Layers_LayersInitialized(ByVal sender As Object, ByVal args As EventArgs)
            Dim polygonLayer As GraphicsLayer = TryCast(MyMap.Layers("MyPolygonGraphicsLayer"), GraphicsLayer)

            For i As Integer = 0 To polygonLayer.Graphics.Count - 1
                Dim graphic As Graphic = polygonLayer.Graphics(i)

                If (Not graphic.Attributes.ContainsKey("Name")) Then
                    graphic.Attributes.Add("Name", String.Format("Polygon_{0}", i))
                    graphic.Attributes.Add("Relation", Nothing)
                End If
            Next i
        End Sub

        Private Sub MyMap_MouseClick(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Map.MouseEventArgs)
            Dim pointLayer As GraphicsLayer = TryCast(MyMap.Layers("MyPointGraphicsLayer"), GraphicsLayer)
            Dim name As String = String.Format("Point_{0}", pointLayer.Graphics.Count)

            Dim mapPoint As MapPoint = e.MapPoint
            mapPoint.SpatialReference = MyMap.SpatialReference

            Dim graphic As New Graphic() With {.Symbol = DefaultPointMarkerSymbol, .Geometry = mapPoint}

            graphic.Attributes.Add("Name", name)
            graphic.Attributes.Add("Relation", Nothing)

            pointLayer.Graphics.Add(graphic)
        End Sub

        Private Sub ExecuteRelationButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim geometryService As New GeometryService("http://sampleserver2.arcgisonline.com/ArcGIS/rest/services/" & "Geometry/GeometryServer")
            AddHandler geometryService.RelationCompleted, AddressOf GeometryService_RelationCompleted
            AddHandler geometryService.Failed, AddressOf GeometryService_Failed

            Dim pointLayer As GraphicsLayer = TryCast(MyMap.Layers("MyPointGraphicsLayer"), GraphicsLayer)
            If pointLayer.Graphics.Count < 1 Then
                MessageBox.Show("No points available")
                Return
            End If

            For Each graphic As Graphic In pointLayer.Graphics
                graphic.Attributes("Relation") = Nothing
            Next graphic

            Dim polygonLayer As GraphicsLayer = TryCast(MyMap.Layers("MyPolygonGraphicsLayer"), GraphicsLayer)
            For Each graphic As Graphic In polygonLayer.Graphics
                graphic.Attributes("Relation") = Nothing
            Next graphic

            ExecuteRelationButton.Visibility = Visibility.Collapsed

            geometryService.RelationAsync(pointLayer.Graphics.ToList(), polygonLayer.Graphics.ToList(), GeometryRelation.esriGeometryRelationWithin, Nothing)
        End Sub

        Private Sub GeometryService_RelationCompleted(ByVal sender As Object, ByVal args As RelationEventArgs)
            Dim pointLayer As GraphicsLayer = TryCast(MyMap.Layers("MyPointGraphicsLayer"), GraphicsLayer)
            Dim polygonLayer As GraphicsLayer = TryCast(MyMap.Layers("MyPolygonGraphicsLayer"), GraphicsLayer)

            Dim results As List(Of GeometryRelationPair) = args.Results
            For Each pair As GeometryRelationPair In results
                If pointLayer.Graphics(pair.Graphic1Index).Attributes("Relation") Is Nothing Then
                    pointLayer.Graphics(pair.Graphic1Index).Attributes("Relation") = String.Format("Within Polygon {0}", pair.Graphic2Index)
                Else
                    pointLayer.Graphics(pair.Graphic1Index).Attributes("Relation") &= "," & pair.Graphic2Index.ToString()
                End If

                If polygonLayer.Graphics(pair.Graphic2Index).Attributes("Relation") Is Nothing Then
                    polygonLayer.Graphics(pair.Graphic2Index).Attributes("Relation") = String.Format("Contains Point {0}", pair.Graphic1Index)
                Else
                    polygonLayer.Graphics(pair.Graphic2Index).Attributes("Relation") &= "," & pair.Graphic1Index.ToString()
                End If
            Next pair

            ExecuteRelationButton.Visibility = Visibility.Visible
        End Sub

        Private Sub GeometryService_Failed(ByVal sender As Object, ByVal args As TaskFailedEventArgs)
            MessageBox.Show("Geometry Service error: " & args.Error.Message)
        End Sub
    End Class
End Namespace
