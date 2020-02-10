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

Namespace ArcGISSilverlightSDK
	Partial Public Class Routing
		Inherits UserControl
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub MyMap_MouseClick(ByVal sender As Object, ByVal e As ESRI.ArcGIS.Client.Map.MouseEventArgs)
			Dim stopsGraphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyStopsGraphicsLayer"), GraphicsLayer)
			Dim [stop] As New Graphic() With {.Geometry = e.MapPoint, .Symbol = StopSymbol}
			stopsGraphicsLayer.Graphics.Add([stop])

			If stopsGraphicsLayer.Graphics.Count > 1 Then
				Dim routeTask As RouteTask = TryCast(LayoutRoot.Resources("MyRouteTask"), RouteTask)
				If routeTask.IsBusy Then
					routeTask.CancelAsync()
					stopsGraphicsLayer.Graphics.RemoveAt(stopsGraphicsLayer.Graphics.Count - 1)
				End If
				routeTask.SolveAsync(New RouteParameters() With {.Stops = stopsGraphicsLayer})
			End If
		End Sub

		Private Sub MyRouteTask_Failed(ByVal sender As Object, ByVal e As TaskFailedEventArgs)
			MessageBox.Show("Routing error: " & e.Error.Message)
			Dim stopsGraphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyStopsGraphicsLayer"), GraphicsLayer)
			stopsGraphicsLayer.Graphics.RemoveAt(stopsGraphicsLayer.Graphics.Count - 1)
		End Sub

		Private Sub MyRouteTask_SolveCompleted(ByVal sender As Object, ByVal e As RouteEventArgs)
			Dim routeGraphicsLayer As GraphicsLayer = TryCast(MyMap.Layers("MyRouteGraphicsLayer"), GraphicsLayer)
			routeGraphicsLayer.Graphics.Clear()

			Dim routeResult As RouteResult = e.RouteResults(0)
			routeResult.Route.Symbol = RouteSymbol

			Dim lastRoute As Graphic = routeResult.Route

			Dim totalTime As Decimal = CDec(lastRoute.Attributes("Total_Time"))
			Dim tip As String = String.Format("{0} minutes", totalTime.ToString("#0.000"))
			lastRoute.Attributes.Add("TIP", tip)

			routeGraphicsLayer.Graphics.Add(lastRoute)
		End Sub
	End Class
End Namespace
