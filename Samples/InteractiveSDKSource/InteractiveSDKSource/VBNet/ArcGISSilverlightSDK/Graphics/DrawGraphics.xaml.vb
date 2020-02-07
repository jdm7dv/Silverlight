Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Controls
Imports ESRI.ArcGIS.Client
Imports ESRI.ArcGIS.Client.Symbols
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Tasks
Imports ESRI.ArcGIS.Client.Toolkit

Namespace ArcGISSilverlightSDK
    Partial Public Class DrawGraphics
        Inherits UserControl
        Private MyDrawObject As Draw
        Private _activeSymbol As Symbol = Nothing

        Public Sub New()
            InitializeComponent()

            MyDrawObject = New Draw(MyMap) With {.LineSymbol = TryCast(LayoutRoot.Resources("DrawLineSymbol"), LineSymbol), .FillSymbol = TryCast(LayoutRoot.Resources("DrawFillSymbol"), FillSymbol)}

            AddHandler MyDrawObject.DrawComplete, AddressOf MyDrawObject_DrawComplete
        End Sub

        Private Sub MyToolbar_ToolbarItemClicked(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Toolkit.SelectedToolbarItemArgs)
            Select Case e.Index
                Case 0 ' Point
                    MyDrawObject.DrawMode = DrawMode.Point
                    _activeSymbol = TryCast(LayoutRoot.Resources("DefaultMarkerSymbol"), Symbol)
                Case 1 ' Polyline
                    MyDrawObject.DrawMode = DrawMode.Polyline
                    _activeSymbol = TryCast(LayoutRoot.Resources("DefaultLineSymbol"), Symbol)
                Case 2 ' Polygon
                    MyDrawObject.DrawMode = DrawMode.Polygon
                    _activeSymbol = TryCast(LayoutRoot.Resources("DefaultFillSymbol"), Symbol)
                Case 3 ' Rectangle
                    MyDrawObject.DrawMode = DrawMode.Rectangle
                    _activeSymbol = TryCast(LayoutRoot.Resources("DefaultFillSymbol"), Symbol)
                Case 4 ' Freehand
                    MyDrawObject.DrawMode = DrawMode.Freehand
                    _activeSymbol = TryCast(LayoutRoot.Resources("DefaultLineSymbol"), Symbol)
                Case Else ' Clear Graphics
                    MyDrawObject.DrawMode = DrawMode.None
                    Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
                    graphicsLayer.ClearGraphics()
            End Select
            MyDrawObject.IsEnabled = (MyDrawObject.DrawMode <> DrawMode.None)
        End Sub

        Private Sub MyToolbar_ToolbarIndexChanged(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Toolkit.SelectedToolbarItemArgs)
            StatusTextBlock.Text = e.Item.Text
        End Sub


        Private Sub MyDrawObject_DrawComplete(ByVal sender As Object, ByVal args As ESRI.ArcGIS.Client.DrawEventArgs)
            Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyGraphicsLayer"), GraphicsLayer)
            Dim graphic As New ESRI.ArcGIS.Client.Graphic() With {.Geometry = args.Geometry, .Symbol = _activeSymbol}
            graphicsLayer.Graphics.Add(graphic)
        End Sub
    End Class
End Namespace
