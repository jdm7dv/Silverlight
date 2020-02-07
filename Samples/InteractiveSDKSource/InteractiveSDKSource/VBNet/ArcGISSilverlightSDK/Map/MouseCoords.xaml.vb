Imports Microsoft.VisualBasic
Imports System
Imports System.Windows
Imports System.Windows.Controls

Namespace ArcGISSilverlightSDK
    Partial Public Class MouseCoords
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub StreetMap_Initialized(ByVal sender As Object, ByVal args As EventArgs)
            AddHandler MyMap.MouseMove, AddressOf MyMap_MouseMove
        End Sub

        Private Sub MyMap_MouseMove(ByVal sender As Object, ByVal args As System.Windows.Input.MouseEventArgs)
            If MyMap.Extent IsNot Nothing Then
                Dim screenPoint As System.Windows.Point = args.GetPosition(MyMap)
                ScreenCoordsTextBlock.Text = String.Format("Screen Coords: X = {0}, Y = {1}", screenPoint.X, screenPoint.Y)

                Dim mapPoint As ESRI.ArcGIS.Client.Geometry.MapPoint = MyMap.ScreenToMap(screenPoint)
                MapCoordsTextBlock.Text = String.Format("Map Coords: X = {0}, Y = {1}", Math.Round(mapPoint.X, 4), Math.Round(mapPoint.Y, 4))
            End If
        End Sub
    End Class
End Namespace