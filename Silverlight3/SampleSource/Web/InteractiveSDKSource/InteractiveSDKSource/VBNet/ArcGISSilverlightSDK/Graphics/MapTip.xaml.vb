﻿Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.Windows.Browser
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Tasks

Namespace ArcGISSilverlightSDK
    Partial Public Class GraphicsMapTip
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MyGraphics_Initialized(ByVal sender As Object, ByVal args As EventArgs)
            Dim query As New ESRI.ArcGIS.Client.Tasks.Query() With {.Geometry = New ESRI.ArcGIS.Client.Geometry.Envelope(-180, 0, 0, 90)}
            query.OutFields.Add("*")

            Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Demographics/ESRI_Census_USA/MapServer/5")
            AddHandler queryTask.ExecuteCompleted, AddressOf QueryTask_ExecuteCompleted
            queryTask.ExecuteAsync(query)
        End Sub

        Private Sub QueryTask_ExecuteCompleted(ByVal sender As Object, ByVal queryArgs As ESRI.ArcGIS.Client.Tasks.QueryEventArgs)
            If queryArgs.FeatureSet Is Nothing Then
                Return
            End If

            Dim resultFeatureSet As FeatureSet = queryArgs.FeatureSet
            Dim graphicsLayer As ESRI.ArcGIS.Client.GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), ESRI.ArcGIS.Client.GraphicsLayer)

            If resultFeatureSet IsNot Nothing AndAlso resultFeatureSet.Features.Count > 0 Then
                For Each graphicFeature As ESRI.ArcGIS.Client.Graphic In resultFeatureSet.Features
                    graphicFeature.Symbol = DefaultFillSymbol
                    graphicsLayer.Graphics.Add(graphicFeature)
                Next graphicFeature
            End If
        End Sub
    End Class
End Namespace
