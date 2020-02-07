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
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry

Namespace ArcGISSilverlightSDK
    Partial Public Class SurfaceProfile
        Inherits UserControl
        Private MyDrawObject As Draw

        Public Sub New()
            InitializeComponent()
            MyDrawObject = New Draw(MyMap) With {.LineSymbol = BlueLineSymbol, .DrawMode = DrawMode.Polyline, .IsEnabled = True}

            AddHandler MyDrawObject.DrawComplete, AddressOf MyDrawObject_OnDrawComplete
            AddHandler MyDrawObject.DrawBegin, AddressOf MyDrawObject_OnDrawBegin
        End Sub

        Private Sub MyDrawObject_OnDrawComplete(ByVal sender As Object, ByVal args As DrawEventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)

            Dim graphic As New Graphic() With {.Symbol = BlueLineSymbol, .Geometry = args.Geometry}
            graphicsLayer.Graphics.Add(graphic)

            Dim geoprocessorTask As New Geoprocessor("http://sampleserver2.arcgisonline.com/ArcGIS/rest/services/" & "Elevation/ESRI_Elevation_World/GPServer/ProfileService")
            AddHandler geoprocessorTask.ExecuteCompleted, AddressOf GeoprocessorTask_ExecuteCompleted
            AddHandler geoprocessorTask.Failed, AddressOf GeoprocessorTask_Failed

            Dim parameters As New List(Of GPParameter)()
            parameters.Add(New GPFeatureRecordSetLayer("Input_Polylines", args.Geometry))
            parameters.Add(New GPLong("Image_Width", 600))
            parameters.Add(New GPLong("Image_Height", 250))
            parameters.Add(New GPBoolean("Display_Segments", True))

            geoprocessorTask.ExecuteAsync(parameters)
        End Sub

        Private Sub GeoprocessorTask_ExecuteCompleted(ByVal sender As Object, ByVal args As GPExecuteCompleteEventArgs)
            Dim gpParameter As GPParameter = args.Results.OutParameters(0)
            If TypeOf gpParameter Is GPFeatureRecordSetLayer Then
                Dim gpLayer As GPFeatureRecordSetLayer = TryCast(gpParameter, GPFeatureRecordSetLayer)
                ProfileImage.Source = New BitmapImage(New Uri(gpLayer.FeatureSet.Features(0).Attributes("profileURL").ToString(), UriKind.Absolute))
                ProfileView.Visibility = Visibility.Visible
            End If
        End Sub

        Private Sub MyDrawObject_OnDrawBegin(ByVal sender As Object, ByVal args As EventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()
            ProfileView.Visibility = Visibility.Collapsed
        End Sub

        Private Sub CloseProfileImage_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            ProfileView.Visibility = Visibility.Collapsed
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()
        End Sub

        Private Sub SizeProfileImage_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Dim width As Double = ProfileImage.Width
            If width <> 600 Then
                ProfileImage.Width = 600
                ProfileImage.Height = 250
            Else
                ProfileImage.Width = 250
                ProfileImage.Height = 104
            End If
        End Sub

        Private Sub GeoprocessorTask_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
            MessageBox.Show("Geoprocessor failed. Error: " & e.Error.Message)
        End Sub

    End Class
End Namespace
