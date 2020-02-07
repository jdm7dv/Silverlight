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
Imports ESRI.ArcGIS.Client.Geometry
Imports ESRI.ArcGIS.Client.Symbols

Namespace ArcGISSilverlightSDK
	Partial Public Class RoutingBarriers
		Inherits UserControl
		Private _routeTask As RouteTask
		Private _stops As New List(Of Graphic)()
		Private _barriers As New List(Of Graphic)()
		Private _routeParams As New RouteParameters()

		Public Sub New()
			InitializeComponent()

			_routeTask = New RouteTask("http://tasks.arcgisonline.com/ArcGIS/rest/services/NetworkAnalysis/ESRI_Route_NA/NAServer/Route")
			AddHandler _routeTask.SolveCompleted, AddressOf routeTask_SolveCompleted
			AddHandler _routeTask.Failed, AddressOf routeTask_Failed

			_routeParams.Stops = _stops
			_routeParams.Barriers = _barriers
		End Sub

		Private Sub MyMap_MouseClick(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Map.MouseEventArgs)

			If StopsRadioButton.IsChecked.Value Then
				Dim stopsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyStopsGraphicsLayer"), GraphicsLayer)
				Dim [stop] As New Graphic() With {.Geometry = e.MapPoint, .Symbol = StopSymbol}
				[stop].Attributes.Add("StopNumber", stopsLayer.Graphics.Count + 1)
				stopsLayer.Graphics.Add([stop])
				_stops.Add([stop])
			ElseIf BarriersRadioButton.IsChecked.Value Then
				Dim stopsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyBarriersGraphicsLayer"), GraphicsLayer)
				Dim barrier As New Graphic() With {.Geometry = e.MapPoint, .Symbol = BarrierSymbol}
				stopsLayer.Graphics.Add(barrier)
				_barriers.Add(barrier)
			End If
			If _stops.Count > 1 Then
				_routeTask.SolveAsync(_routeParams)
			End If
		End Sub

		Private Sub routeTask_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
			MessageBox.Show("Routing error: " & e.Error.Message)
			Dim graphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyStopsGraphicsLayer"), GraphicsLayer)
			graphicsLayer.Graphics.RemoveAt(graphicsLayer.Graphics.Count - 1)
		End Sub

		Private Sub routeTask_SolveCompleted(ByVal sender As Object, ByVal e As RouteEventArgs)
			Dim routeLayer As GraphicsLayer = TryCast(MyMap.Layers("MyRouteGraphicsLayer"), GraphicsLayer)
			Dim routeResult As RouteResult = e.RouteResults(0)
			routeResult.Route.Symbol = RouteSymbol

			routeLayer.Graphics.Clear()
			Dim lastRoute As Graphic = routeResult.Route

			routeLayer.Graphics.Add(lastRoute)
		End Sub

		Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			_stops.Clear()
			_barriers.Clear()

			For Each layer As Layer In MyMap.Layers
				If TypeOf layer Is GraphicsLayer Then
					TryCast(layer, GraphicsLayer).ClearGraphics()
				End If
			Next layer
		End Sub
	End Class
End Namespace
