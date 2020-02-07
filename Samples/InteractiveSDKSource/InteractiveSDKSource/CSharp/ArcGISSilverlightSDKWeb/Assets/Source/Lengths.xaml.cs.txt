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
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Tasks;

namespace ArcGISSilverlightSDK
{
    public partial class Lengths : UserControl
    {
		private Draw MyDrawObject;
		
		public Lengths()
        {
            InitializeComponent();
			MyDrawObject = new Draw(MyMap)
			{
				DrawMode = DrawMode.Polyline,
				IsEnabled = true,
				LineSymbol = DefaultLineSymbol
			};
			MyDrawObject.DrawComplete += MyDrawObject_DrawComplete;
			MyDrawObject.DrawBegin += MyDrawObject_DrawBegin;
        }

        private void MyDrawObject_DrawBegin(object sender, EventArgs args)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.ClearGraphics();
        }

        private void MyDrawObject_DrawComplete(object sender, DrawEventArgs args)
        {
            ESRI.ArcGIS.Client.Geometry.Polyline polyline = args.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline;
            polyline.SpatialReference = new SpatialReference(4326);
            Graphic graphic = new Graphic()
            {
                Symbol = DefaultLineSymbol,
                Geometry = polyline
            };

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Add(graphic);

            GeometryService geometryService =
            new GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Geometry/GeometryServer");
            geometryService.ProjectCompleted += GeometryService_ProjectCompleted;
            geometryService.Failed += GeometryService_Failed;

            List<Graphic> graphicList = new List<Graphic>();
            graphicList.Add(graphic);

            // GeometryService.Lengths returns distances in the units of the spatial reference.
            // The units in the map view's projection is decimal degrees. 
            // Use the Project method to convert graphic points to a projection that uses a measured unit (e.g. meters).
            // If the map units are in measured units, the call to Project is unnecessary.
            // Important: Use a projection appropriate for your area of interest.
            geometryService.ProjectAsync(graphicList, new SpatialReference(32611));
        }

        private void GeometryService_ProjectCompleted(object sender, ESRI.ArcGIS.Client.Tasks.GraphicsEventArgs args)
        {
            GeometryService geometryService =
            new GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Geometry/GeometryServer");
            geometryService.LengthsCompleted += GeometryService_LengthsCompleted;

            // Execute the Lengths method using the returned projected graphics
            geometryService.LengthsAsync(args.Results);
        }

        private void GeometryService_LengthsCompleted(object sender, ESRI.ArcGIS.Client.Tasks.LengthsEventArgs args)
        {
            // convert results from meters into miles for our display
            double miles = args.Results[0] * 0.0006213700922;
            ResponseTextBlock.Text = String.Format("The distance of the polyline is {0} miles.", Math.Round(miles, 3));
        }

        private void GeometryService_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Geometry Service error: " + e.Error);
        }
    }
}
