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

namespace ArcGISSilverlightSDK
{
    public partial class Routing : UserControl
    {
        public Routing()
        {
            InitializeComponent();
        }

        private void MyMap_MouseClick(object sender, ESRI.ArcGIS.Client.Map.MouseEventArgs e)
        {
            GraphicsLayer stopsGraphicsLayer = MyMap.Layers["MyStopsGraphicsLayer"] as GraphicsLayer;
            Graphic stop = new Graphic() { Geometry = e.MapPoint, Symbol = StopSymbol };
            stopsGraphicsLayer.Graphics.Add(stop);

            if (stopsGraphicsLayer.Graphics.Count > 1)
            {
                RouteTask routeTask = LayoutRoot.Resources["MyRouteTask"] as RouteTask;
                if (routeTask.IsBusy)
                {
                    routeTask.CancelAsync();
                    stopsGraphicsLayer.Graphics.RemoveAt(stopsGraphicsLayer.Graphics.Count - 1);
                }
                routeTask.SolveAsync(new RouteParameters() { Stops = stopsGraphicsLayer });
            }
        }

        private void MyRouteTask_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Routing error: " + e.Error.Message);
            GraphicsLayer stopsGraphicsLayer = MyMap.Layers["MyStopsGraphicsLayer"] as GraphicsLayer;
            stopsGraphicsLayer.Graphics.RemoveAt(stopsGraphicsLayer.Graphics.Count - 1);
        }

        private void MyRouteTask_SolveCompleted(object sender, RouteEventArgs e)
        {
            GraphicsLayer routeGraphicsLayer = MyMap.Layers["MyRouteGraphicsLayer"] as GraphicsLayer;
            routeGraphicsLayer.Graphics.Clear();

            RouteResult routeResult = e.RouteResults[0];
            routeResult.Route.Symbol = RouteSymbol;

            Graphic lastRoute = routeResult.Route;

            decimal totalTime = (decimal)lastRoute.Attributes["Total_Time"];
            string tip = string.Format("{0} minutes", totalTime.ToString("#0.000"));
            lastRoute.Attributes.Add("TIP", tip);

            routeGraphicsLayer.Graphics.Add(lastRoute);
        }
    }
}
