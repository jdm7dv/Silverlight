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

Namespace ArcGISSilverlightSDK
    Partial Public Class AddLayerDynamically
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub AddLayerButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            MyMap.Layers.Clear()

            Dim NewTiledLayer As New ESRI.ArcGIS.Client.ArcGISTiledMapServiceLayer()
            AddHandler NewTiledLayer.Initialized, AddressOf NewTiledLayerInitialized

            NewTiledLayer.Url = UrlTextBox.Text
            MyMap.Layers.Add(NewTiledLayer)
        End Sub

        Private Sub NewTiledLayerInitialized(ByVal sender As Object, ByVal args As EventArgs)
            Dim layer As ESRI.ArcGIS.Client.ArcGISTiledMapServiceLayer = CType(sender, ESRI.ArcGIS.Client.ArcGISTiledMapServiceLayer)
            MyMap.ZoomTo(layer.InitialExtent)
        End Sub

    End Class
End Namespace