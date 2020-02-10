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
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client.Toolkit
Imports System.Windows.Data
Imports ESRI.ArcGIS.Client.ValueConverters

Namespace ArcGISSilverlightSDK
    Partial Public Class SpatialQuery
        Inherits UserControl
        Private _inputSymbol As Symbol = Nothing
        Private MyDrawSurface As Draw

        Public Sub New()
            InitializeComponent()

            MyDrawSurface = New Draw(MyMap) With {.LineSymbol = TryCast(LayoutRoot.Resources("DefaultLineSymbol"), LineSymbol), .FillSymbol = TryCast(LayoutRoot.Resources("DefaultFillSymbol"), FillSymbol)}
            AddHandler MyDrawSurface.DrawComplete, AddressOf MyDrawSurface_DrawComplete
        End Sub

        Private Sub esriTools_ToolbarItemClicked(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Toolkit.SelectedToolbarItemArgs)
            Select Case e.Index
                Case 0 ' Point
                    MyDrawSurface.DrawMode = DrawMode.Point
                    _inputSymbol = DefaultMarkerSymbol
                Case 1 ' Polyline
                    MyDrawSurface.DrawMode = DrawMode.Polyline
                    _inputSymbol = DefaultLineSymbol
                Case 2 ' Polygon
                    MyDrawSurface.DrawMode = DrawMode.Polygon
                    _inputSymbol = DefaultFillSymbol
                Case 3 ' Rectangle
                    MyDrawSurface.DrawMode = DrawMode.Rectangle
                    _inputSymbol = DefaultFillSymbol
                Case Else ' Clear
                    MyDrawSurface.DrawMode = DrawMode.None
                    Dim selectionGraphicslayer As GraphicsLayer = TryCast(MyMap.Layers("MySelectionGraphicsLayer"), GraphicsLayer)
                    selectionGraphicslayer.ClearGraphics()
                    Dim drawGraphicslayer As GraphicsLayer = TryCast(MyMap.Layers("MyDrawGraphicsLayer"), GraphicsLayer)
                    drawGraphicslayer.ClearGraphics()
                    QueryDetailsDataGrid.ItemsSource = Nothing
            End Select
            MyDrawSurface.IsEnabled = (MyDrawSurface.DrawMode <> DrawMode.None)
            StatusTextBlock.Text = e.Item.Text
        End Sub

        Private Sub MyDrawSurface_DrawComplete(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.DrawEventArgs)
            Dim selectionGraphicslayer As GraphicsLayer = TryCast(MyMap.Layers("MySelectionGraphicsLayer"), GraphicsLayer)
            selectionGraphicslayer.ClearGraphics()

            Dim drawGraphicslayer As GraphicsLayer = TryCast(MyMap.Layers("MyDrawGraphicsLayer"), GraphicsLayer)
            drawGraphicslayer.ClearGraphics()

            Dim graphic As New ESRI.ArcGIS.Client.Graphic() With {.Geometry = args.Geometry, .Symbol = _inputSymbol}

            drawGraphicslayer.Graphics.Add(graphic)

            Dim queryTask As New QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" & "Demographics/ESRI_Census_USA/MapServer/5")
            AddHandler queryTask.ExecuteCompleted, AddressOf QueryTask_ExecuteCompleted
            AddHandler queryTask.Failed, AddressOf QueryTask_Failed

            ' Bind data grid to query results
            Dim resultFeaturesBinding As New Binding("LastResult.Features")
            resultFeaturesBinding.Source = queryTask
            QueryDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding)
            Dim query As Query = New ESRI.ArcGIS.Client.Tasks.Query()

            ' Specify fields to return from query
            query.OutFields.AddRange(New String() {"STATE_NAME", "SUB_REGION", "STATE_FIPS", "STATE_ABBR", "POP2000", "POP2007"})
            query.Geometry = args.Geometry

            ' Return geometry with result features
            query.ReturnGeometry = True

            queryTask.ExecuteAsync(query)
        End Sub

        Private Sub QueryTask_ExecuteCompleted(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.Tasks.QueryEventArgs)
            Dim featureSet As FeatureSet = args.FeatureSet

            If featureSet Is Nothing OrElse featureSet.Features.Count < 1 Then
                MessageBox.Show("No features retured from query")
                Return
            End If

            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MySelectionGraphicsLayer"), GraphicsLayer)

            If featureSet IsNot Nothing AndAlso featureSet.Features.Count > 0 Then
                For Each feature As Graphic In featureSet.Features
                    feature.Symbol = ResultsFillSymbol

                    'graphicsLayer.Graphics.Add(feature);
                    graphicsLayer.Graphics.Insert(0, feature)
                Next feature
            End If

            MyDrawSurface.IsEnabled = False
        End Sub

        Private Sub QueryTask_Failed(ByVal sender As Object, ByVal args As TaskFailedEventArgs)
            MessageBox.Show("Query failed: " & args.Error.Message)
        End Sub
    End Class

    ' Helper class to simplify XAML data grid column declaration
    Public Class GraphicAttributeColumn
        Inherits DataGridTextColumn
        Public Sub New()
            Dim b As New System.Windows.Data.Binding("Attributes")
            b.Converter = New DictionaryConverter()
            Me.Binding = b
        End Sub

        Private attributeName_Renamed As String
        Public Property AttributeName() As String
            Get
                Return attributeName_Renamed
            End Get
            Set(ByVal value As String)
                If Binding IsNot Nothing AndAlso TypeOf Binding.Converter Is DictionaryConverter Then
                    Binding.ConverterParameter = value
                End If
                attributeName_Renamed = value
            End Set
        End Property
    End Class
End Namespace
