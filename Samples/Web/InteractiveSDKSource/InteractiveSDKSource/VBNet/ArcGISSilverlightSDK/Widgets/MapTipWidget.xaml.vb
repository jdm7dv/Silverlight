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
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client.Toolkit
Imports ESRI.ArcGIS.Client.Symbols
Imports System.Collections.ObjectModel

Namespace ArcGISSilverlightSDK
    Partial Public Class MapTipWidget
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MyGraphicsLayer_Initialized(ByVal sender As Object, ByVal args As EventArgs)
            Dim query As New ESRI.ArcGIS.Client.Tasks.Query() With {.Geometry = New ESRI.ArcGIS.Client.Geometry.Envelope(-180, 0, 0, 90)}
            query.OutFields.Add("*")

            Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Demographics/ESRI_Census_USA/MapServer/5")
            AddHandler queryTask.ExecuteCompleted, AddressOf QueryTask_ExecuteCompleted
            queryTask.ExecuteAsync(query)
        End Sub

        Private Sub QueryTask_ExecuteCompleted(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.Tasks.QueryEventArgs)
            Dim featureSet As FeatureSet = args.FeatureSet

            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            MyMapTip.GraphicsLayer = graphicsLayer

            If featureSet IsNot Nothing AndAlso featureSet.Features.Count > 0 Then
                For Each feature As Graphic In featureSet.Features
                    feature.Symbol = DefaultFillSymbol
                    graphicsLayer.Graphics.Add(feature)
                Next feature
            End If
        End Sub
    End Class
End Namespace
