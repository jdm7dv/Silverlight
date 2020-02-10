using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;

namespace ArcGISSilverlightSDK
{
	public partial class RoutingBarriers : UserControl
	{
		RouteTask _routeTask;
		List<Graphic> _stops = new List<Graphic>();
		List<Graphic> _barriers = new List<Graphic>();
		RouteParameters _routeParams = new RouteParameters();

		public RoutingBarriers()
		{
			InitializeComponent();

			_routeTask = 
                new RouteTask("http://tasks.arcgisonline.com/ArcGIS/rest/services/NetworkAnalysis/ESRI_Route_NA/NAServer/Route");
			_routeTask.SolveCompleted += routeTask_SolveCompleted;
			_routeTask.Failed += routeTask_Failed;

            _routeParams.Stops = _stops;
            _routeParams.Barriers = _barriers;        
        }

		private void MyMap_MouseClick(object sender, ESRI.ArcGIS.Client.Map.MouseEventArgs e)
		{
			
            if (StopsRadioButton.IsChecked.Value)
			{
                GraphicsLayer stopsLayer = MyMap.Layers["MyStopsGraphicsLayer"] as GraphicsLayer;
				Graphic stop = new Graphic() { Geometry = e.MapPoint, Symbol = StopSymbol };
                stop.Attributes.Add("StopNumber", stopsLayer.Graphics.Count + 1);
				stopsLayer.Graphics.Add(stop);
				_stops.Add(stop);
			}
            else if (BarriersRadioButton.IsChecked.Value)
			{
                GraphicsLayer stopsLayer = MyMap.Layers["MyBarriersGraphicsLayer"] as GraphicsLayer;
				Graphic barrier = new Graphic() { Geometry = e.MapPoint, Symbol = BarrierSymbol };
				stopsLayer.Graphics.Add(barrier);
				_barriers.Add(barrier);
			}
			if (_stops.Count > 1)
			{
				_routeTask.SolveAsync(_routeParams);
			}
		}

		private void routeTask_Failed(object sender, TaskFailedEventArgs e)
		{
			MessageBox.Show("Routing error: " + e.Error.Message);
            GraphicsLayer graphicsLayer = MyMap.Layers["MyStopsGraphicsLayer"] as GraphicsLayer;
			graphicsLayer.Graphics.RemoveAt(graphicsLayer.Graphics.Count - 1);
		}

		private void routeTask_SolveCompleted(object sender, RouteEventArgs e)
		{
			GraphicsLayer routeLayer = MyMap.Layers["MyRouteGraphicsLayer"] as GraphicsLayer;
			RouteResult routeResult = e.RouteResults[0];
			routeResult.Route.Symbol = RouteSymbol;

			routeLayer.Graphics.Clear();
            Graphic lastRoute = routeResult.Route;

			routeLayer.Graphics.Add(lastRoute);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_stops.Clear();
			_barriers.Clear();

            foreach (Layer layer in MyMap.Layers)
                if (layer is GraphicsLayer)
                    (layer as GraphicsLayer).ClearGraphics();
		}
	}
}
