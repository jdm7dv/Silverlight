﻿Imports Microsoft.VisualBasic
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
    Partial Public Class LabelPoints
        Inherits UserControl
        Private MyDrawObject As Draw

        Public Sub New()
            InitializeComponent()

            MyDrawObject = New Draw(MyMap) With {.FillSymbol = DefaultPolygonSymbol, .DrawMode = DrawMode.Polygon}

            AddHandler MyDrawObject.DrawComplete, AddressOf MyDrawObject_DrawComplete
        End Sub

        Private Sub ClearGraphicsButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()
        End Sub

        Private Sub AddPolygonButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            MyDrawObject.IsEnabled = True

        End Sub

        Private Sub MyDrawObject_DrawComplete(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.DrawEventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            Dim polygon As ESRI.ArcGIS.Client.Geometry.Polygon = TryCast(args.Geometry, ESRI.ArcGIS.Client.Geometry.Polygon)
            polygon.SpatialReference = New SpatialReference(4326)
            Dim graphic As New Graphic() With {.Symbol = DefaultPolygonSymbol, .Geometry = polygon}
            graphic.Attributes.Add("X", "Label Point Polygon")
            graphicsLayer.Graphics.Add(graphic)

            Dim geometryService As New GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Geometry/GeometryServer")
            AddHandler geometryService.SimplifyCompleted, AddressOf GeometryService_SimplifyCompleted
            AddHandler geometryService.Failed, AddressOf GeometryService_Failed

            Dim graphicsList As New List(Of Graphic)()
            graphicsList.Add(graphic)

            MyDrawObject.IsEnabled = False

            geometryService.SimplifyAsync(graphicsList)
        End Sub

        Private Sub GeometryService_SimplifyCompleted(ByVal sender As Object, ByVal e As GraphicsEventArgs)
            Dim geometryService As New GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Geometry/GeometryServer")
            AddHandler geometryService.LabelPointsCompleted, AddressOf GeometryService_LabelPointsCompleted
            AddHandler geometryService.Failed, AddressOf GeometryService_Failed

            geometryService.LabelPointsAsync(e.Results)
        End Sub

        Private Sub GeometryService_LabelPointsCompleted(ByVal sender As Object, ByVal args As GraphicsEventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)

            For Each graphic As Graphic In args.Results
                graphic.Symbol = DefaultRasterSymbol
                Dim mapPoint As MapPoint = TryCast(graphic.Geometry, MapPoint)
                graphic.Attributes.Add("X", mapPoint.X)
                graphic.Attributes.Add("Y", mapPoint.Y)
                graphicsLayer.Graphics.Add(graphic)
            Next graphic
        End Sub

        Private Sub GeometryService_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
            MessageBox.Show("Geometry Service error: " & e.Error.Message)
        End Sub
    End Class
End Namespace
