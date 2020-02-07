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
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client.Toolkit
Imports ESRI.ArcGIS.Client.Symbols
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports ESRI.ArcGIS.Client.ValueConverters

Namespace ArcGISSilverlightSDK
    Partial Public Class AttributeQuery
        Inherits UserControl
        Public Sub New()
            InitializeComponent()

            Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Demographics/ESRI_Census_USA/MapServer/5")
            AddHandler queryTask.ExecuteCompleted, AddressOf QueryTask_ExecuteCompleted
            AddHandler queryTask.Failed, AddressOf QueryTask_Failed

            Dim query As New ESRI.ArcGIS.Client.Tasks.Query()

            ' Specify fields to return from initial query
            query.OutFields.AddRange(New String() {"STATE_NAME"})

            ' Return all features
            query.Where = "1=1"
            queryTask.ExecuteAsync(query)
        End Sub

        Private Sub QueryComboBox_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            Dim index As Integer = QueryComboBox.SelectedIndex

            If index > 0 Then
                Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Demographics/ESRI_Census_USA/MapServer/5")
                AddHandler queryTask.ExecuteCompleted, AddressOf QueryTask_ExecuteCompleted
                AddHandler queryTask.Failed, AddressOf QueryTask_Failed

                Dim query As New ESRI.ArcGIS.Client.Tasks.Query()
                query.OutFields.Add("*")
                query.Text = QueryComboBox.SelectedItem.ToString()

                queryTask.ExecuteAsync(query)

                ' Remove the first entry ("Select...")
                QueryComboBox.Items.RemoveAt(0)
            End If
        End Sub

        Private Sub QueryTask_ExecuteCompleted(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.Tasks.QueryEventArgs)
            Dim featureSet As FeatureSet = args.FeatureSet

            ' If initial query to populate states combobox
            If featureSet.Features(0).Attributes.Count = 1 Then
                ' Just show on initial load
                QueryComboBox.Items.Add("Select...")

                For Each graphic As Graphic In args.FeatureSet.Features
                    QueryComboBox.Items.Add(graphic.Attributes("STATE_NAME").ToString())
                Next graphic

                QueryComboBox.SelectedIndex = 0
                Return
            End If

            ' If an item has been selected            
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()

            If featureSet IsNot Nothing AndAlso featureSet.Features.Count > 0 Then
                ' Show selected feature attributes in DataGrid
                Dim selectedFeature As Graphic = featureSet.Features(0)

                QueryDetailsDataGrid.ItemsSource = selectedFeature.Attributes

                ' Hightlight selected feature
                selectedFeature.Symbol = DefaultFillSymbol
                graphicsLayer.Graphics.Add(selectedFeature)

                ' Zoom to selected feature (define expand percentage)
                Dim selectedFeatureExtent As ESRI.ArcGIS.Client.Geometry.Envelope = selectedFeature.Geometry.Extent

                Dim expandPercentage As Double = 30

                Dim widthExpand As Double = selectedFeatureExtent.Width * (expandPercentage / 100)
                Dim heightExpand As Double = selectedFeatureExtent.Height * (expandPercentage / 100)

                Dim displayExtent As New ESRI.ArcGIS.Client.Geometry.Envelope(selectedFeatureExtent.XMin - (widthExpand / 2), selectedFeatureExtent.YMin - (heightExpand / 2), selectedFeatureExtent.XMax + (widthExpand / 2), selectedFeatureExtent.YMax + (heightExpand / 2))

                MyMap.ZoomTo(displayExtent)

                ' If DataGrid not visible (initial load), show it
                If DataGridScrollViewer.Visibility = Visibility.Collapsed Then
                    DataGridScrollViewer.Visibility = Visibility.Visible
                    QueryGrid.Height = Double.NaN
                    QueryGrid.UpdateLayout()
                End If
            Else
                QueryDetailsDataGrid.ItemsSource = Nothing
                DataGridScrollViewer.Visibility = Visibility.Collapsed
                QueryGrid.Height = Double.NaN
                QueryGrid.UpdateLayout()
            End If
        End Sub

        Private Sub QueryTask_Failed(ByVal sender As Object, ByVal args As TaskFailedEventArgs)
            MessageBox.Show("Query failed: " & args.Error.Message)
        End Sub
    End Class
End Namespace
