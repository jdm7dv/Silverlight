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
    Partial Public Class DriveTimes
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MyMap_MouseClick(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Map.MouseEventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()

            Dim graphic As New Graphic() With {.Symbol = DefaultMarkerSymbol, .Geometry = e.MapPoint}
            graphic.Attributes.Add("Info", "Start location")
            Dim latlon As String = String.Format("{0}, {1}", e.MapPoint.X, e.MapPoint.Y)
            graphic.Attributes.Add("LatLon", latlon)
            graphic.SetZIndex(1)
            graphicsLayer.Graphics.Add(graphic)

            Dim geoprocessorTask As New Geoprocessor("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Network/ESRI_DriveTime_US/GPServer/CreateDriveTimePolygons")
            AddHandler geoprocessorTask.ExecuteCompleted, AddressOf GeoprocessorTask_ExecuteCompleted
            AddHandler geoprocessorTask.Failed, AddressOf GeoprocessorTask_Failed

            Dim parameters As New List(Of GPParameter)()
            parameters.Add(New GPFeatureRecordSetLayer("Input_Location", e.MapPoint))
            parameters.Add(New GPString("Drive_Times", "1 2 3"))

            geoprocessorTask.ExecuteAsync(parameters)
        End Sub

        Private Sub GeoprocessorTask_ExecuteCompleted(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.Tasks.GPExecuteCompleteEventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)

            For Each parameter As GPParameter In args.Results.OutParameters
                If TypeOf parameter Is GPFeatureRecordSetLayer Then
                    Dim gpLayer As GPFeatureRecordSetLayer = TryCast(parameter, GPFeatureRecordSetLayer)

                    Dim bufferSymbols As New List(Of FillSymbol)(New FillSymbol() {FillSymbol1, FillSymbol2, FillSymbol3})

                    Dim count As Integer = 0
                    For Each graphic As Graphic In gpLayer.FeatureSet.Features
                        graphic.Symbol = bufferSymbols(count)
                        graphic.Attributes.Add("Info", String.Format("{0} minute buffer ", 3 - count))
                        graphicsLayer.Graphics.Add(graphic)
                        count += 1
                    Next graphic
                End If
            Next parameter
        End Sub

        Private Sub GeoprocessorTask_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
            MessageBox.Show("Geoprocessing service failed: " & e.Error.Message)
        End Sub
    End Class
End Namespace
