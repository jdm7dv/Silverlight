Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Collections.Generic
Imports System.Linq

Namespace ArcGISSilverlightSDK
    Partial Public Class MapLayersOnOff
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub DynamicLayer_Initialized(ByVal sender As Object, ByVal args As EventArgs)
            Dim dynamicServiceLayer As ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer = TryCast(sender, ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer)
            If dynamicServiceLayer.VisibleLayers Is Nothing Then
                dynamicServiceLayer.VisibleLayers = GetDefaultVisibleLayers(dynamicServiceLayer)
            End If
            UpdateLayerList(dynamicServiceLayer)
        End Sub

        Private Function GetDefaultVisibleLayers(ByVal dynamicService As ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer) As Integer()
            Dim visibleLayerIDList As New List(Of Integer)()

            Dim layerInfoArray() As ESRI.ArcGIS.Client.LayerInfo = dynamicService.Layers

            For index As Integer = 0 To layerInfoArray.Length - 1
                If layerInfoArray(index).DefaultVisibility Then
                    visibleLayerIDList.Add(index)
                End If
            Next index

            Return visibleLayerIDList.ToArray()
        End Function

        Private Sub UpdateLayerList(ByVal dynamicServiceLayer As ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer)
            Dim visibleLayerIDs() As Integer = dynamicServiceLayer.VisibleLayers

            If visibleLayerIDs Is Nothing Then
                visibleLayerIDs = GetDefaultVisibleLayers(dynamicServiceLayer)
            End If

            Dim visibleLayerList As New List(Of LayerListData)()

            Dim layerInfoArray() As ESRI.ArcGIS.Client.LayerInfo = dynamicServiceLayer.Layers

            For index As Integer = 0 To layerInfoArray.Length - 1
                visibleLayerList.Add(New LayerListData() With {.Visible = visibleLayerIDs.Contains(index), .ServiceName = dynamicServiceLayer.ID, .LayerName = layerInfoArray(index).Name, .LayerIndex = index})
            Next index

            LayerVisibilityListBox.ItemsSource = visibleLayerList
        End Sub

        Private Sub CheckBox_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim tickedCheckBox As CheckBox = TryCast(sender, CheckBox)

            Dim serviceName As String = tickedCheckBox.Tag.ToString()
            Dim visible As Boolean = CBool(tickedCheckBox.IsChecked)

            Dim layerIndex As Integer = Int32.Parse(tickedCheckBox.Name)

            Dim dynamicServiceLayer As ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer = TryCast(MyMap.Layers(serviceName), ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer)

            Dim visibleLayerList As List(Of Integer) = If(dynamicServiceLayer.VisibleLayers IsNot Nothing, dynamicServiceLayer.VisibleLayers.ToList(), New List(Of Integer)())

            If visible Then
                If (Not visibleLayerList.Contains(layerIndex)) Then
                    visibleLayerList.Add(layerIndex)
                End If
            Else
                If visibleLayerList.Contains(layerIndex) Then
                    visibleLayerList.Remove(layerIndex)
                End If
            End If

            dynamicServiceLayer.VisibleLayers = visibleLayerList.ToArray()

        End Sub

        Public Class LayerListData
            Private privateVisible As Boolean
            Public Property Visible() As Boolean
                Get
                    Return privateVisible
                End Get
                Set(ByVal value As Boolean)
                    privateVisible = value
                End Set
            End Property
            Private privateServiceName As String
            Public Property ServiceName() As String
                Get
                    Return privateServiceName
                End Get
                Set(ByVal value As String)
                    privateServiceName = value
                End Set
            End Property
            Private privateLayerName As String
            Public Property LayerName() As String
                Get
                    Return privateLayerName
                End Get
                Set(ByVal value As String)
                    privateLayerName = value
                End Set
            End Property
            Private privateLayerIndex As Integer
            Public Property LayerIndex() As Integer
                Get
                    Return privateLayerIndex
                End Get
                Set(ByVal value As Integer)
                    privateLayerIndex = value
                End Set
            End Property
        End Class
    End Class
End Namespace
