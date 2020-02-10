Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Imaging
Imports System.Windows.Shapes
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client.Toolkit
Imports ESRI.ArcGIS.Client.Symbols
Imports System.Windows.Controls.Primitives
Imports System.Collections.ObjectModel

Namespace ArcGISSilverlightSDK
    Partial Public Class Identify
        Inherits UserControl
        Private _dataItems As List(Of DataItem) = Nothing

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub QueryPoint_MouseClick(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Map.MouseEventArgs)
            Dim clickPoint As ESRI.ArcGIS.Client.Geometry.MapPoint = e.MapPoint

            Dim identifyParams As ESRI.ArcGIS.Client.Tasks.IdentifyParameters = New IdentifyParameters() With {.Geometry = clickPoint, .MapExtent = MyMap.Extent, .Width = CInt(Fix(MyMap.ActualWidth)), .Height = CInt(Fix(MyMap.ActualHeight)), .LayerOption = LayerOption.visible}

            Dim identifyTask As New IdentifyTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Demographics/ESRI_Census_USA/MapServer")
            AddHandler identifyTask.ExecuteCompleted, AddressOf IdentifyTask_ExecuteCompleted
            AddHandler identifyTask.Failed, AddressOf IdentifyTask_Failed
            identifyTask.ExecuteAsync(identifyParams)

            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            graphicsLayer.ClearGraphics()
            Dim graphic As New ESRI.ArcGIS.Client.Graphic() With {.Geometry = clickPoint, .Symbol = DefaultPictureSymbol}
            graphicsLayer.Graphics.Add(graphic)
        End Sub

        Public Sub ShowFeatures(ByVal results As List(Of IdentifyResult))
            _dataItems = New List(Of DataItem)()

            If results IsNot Nothing AndAlso results.Count > 0 Then
                IdentifyComboBox.Items.Clear()
                For Each result As IdentifyResult In results
                    Dim feature As Graphic = result.Feature
                    Dim title As String = result.Value.ToString() & " (" & result.LayerName & ")"
                    _dataItems.Add(New DataItem() With {.Title = title, .Data = feature.Attributes})
                    IdentifyComboBox.Items.Add(title)
                Next result

                ' Workaround for bug with ComboBox 
                IdentifyComboBox.UpdateLayout()

                IdentifyComboBox.SelectedIndex = 0
            End If
        End Sub

        Private Sub cb_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            Dim index As Integer = IdentifyComboBox.SelectedIndex
            If index > -1 Then
                IdentifyDetailsDataGrid.ItemsSource = _dataItems(index).Data
            End If
        End Sub

        Private Sub IdentifyTask_ExecuteCompleted(ByVal sender As Object, ByVal args As IdentifyEventArgs)
            IdentifyDetailsDataGrid.ItemsSource = Nothing

            If args.IdentifyResults IsNot Nothing AndAlso args.IdentifyResults.Count > 0 Then
                If DataGridScrollViewer.Visibility = Visibility.Collapsed Then
                    DataGridScrollViewer.Visibility = Visibility.Visible
                    IdentifyGrid.Height = Double.NaN
                    IdentifyGrid.UpdateLayout()
                End If

                ShowFeatures(args.IdentifyResults)
            Else
                IdentifyComboBox.Items.Clear()
                IdentifyComboBox.UpdateLayout()

                If DataGridScrollViewer.Visibility = Visibility.Visible Then
                    DataGridScrollViewer.Visibility = Visibility.Collapsed
                    IdentifyGrid.Height = Double.NaN
                    IdentifyGrid.UpdateLayout()
                End If
            End If
        End Sub

        Public Class DataItem
            Private privateTitle As String
            Public Property Title() As String
                Get
                    Return privateTitle
                End Get
                Set(ByVal value As String)
                    privateTitle = value
                End Set
            End Property
            Private privateData As IDictionary(Of String, Object)
            Public Property Data() As IDictionary(Of String, Object)
                Get
                    Return privateData
                End Get
                Set(ByVal value As IDictionary(Of String, Object))
                    privateData = value
                End Set
            End Property
        End Class

        Private Sub IdentifyTask_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
            MessageBox.Show("Identify failed. Error: " & e.Error.Message)
        End Sub
    End Class
End Namespace

