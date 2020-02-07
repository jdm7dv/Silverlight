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
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client

Namespace ArcGISSilverlightSDK
	Partial Public Class SimpleClusterer
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub LoadGraphics()
			Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Specialty/ESRI_StatesCitiesRivers_USA/MapServer/0")
			AddHandler queryTask.ExecuteCompleted, AddressOf queryTask_ExecuteCompleted

			Dim query As Query = New ESRI.ArcGIS.Client.Tasks.Query()
			query.OutSpatialReferenceWKID = MyMap.SpatialReference.WKID
			query.ReturnGeometry = True
			query.Where = "1=1"
			queryTask.ExecuteAsync(query)
		End Sub

		Private Sub queryTask_ExecuteCompleted(ByVal sender As Object, ByVal args As QueryEventArgs)
			Dim featureSet As FeatureSet = args.FeatureSet

			If featureSet Is Nothing OrElse featureSet.Features.Count < 1 Then
				MessageBox.Show("No features retured from query")
				Return
			End If

			Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)

			For Each graphic As Graphic In featureSet.Features
				graphic.Symbol = MediumMarkerSymbol
				graphicsLayer.Graphics.Add(graphic)
			Next graphic
		End Sub

		Private Sub MyMap_ExtentChanged(ByVal sender As Object, ByVal e As ExtentEventArgs)
			If e.OldExtent Is Nothing Then
			LoadGraphics()
			End If
		End Sub
	End Class
End Namespace