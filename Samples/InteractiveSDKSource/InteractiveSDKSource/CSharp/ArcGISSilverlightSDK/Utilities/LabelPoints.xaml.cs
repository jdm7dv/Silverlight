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
    public partial class LabelPoints : UserControl
    {
		private Draw MyDrawObject;
		
		public LabelPoints()
        {
            InitializeComponent();

			
			MyDrawObject = new Draw(MyMap)
			{
				FillSymbol = DefaultPolygonSymbol,
				DrawMode = DrawMode.Polygon
			};

			MyDrawObject.DrawComplete += MyDrawObject_DrawComplete;
        }

        private void ClearGraphicsButton_Click(object sender, RoutedEventArgs e)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.ClearGraphics();
        }

        private void AddPolygonButton_Click(object sender, RoutedEventArgs e)
        {
			MyDrawObject.IsEnabled = true;;
        }

        private void MyDrawObject_DrawComplete(object sender, ESRI.ArcGIS.Client.DrawEventArgs args)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            ESRI.ArcGIS.Client.Geometry.Polygon polygon = args.Geometry as ESRI.ArcGIS.Client.Geometry.Polygon;
            polygon.SpatialReference = new SpatialReference(4326);
            Graphic graphic = new Graphic()
            {
                Symbol = DefaultPolygonSymbol,
                Geometry = polygon
            };
            graphic.Attributes.Add("X", "Label Point Polygon");
            graphicsLayer.Graphics.Add(graphic);

            GeometryService geometryService =
            new GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Geometry/GeometryServer");
            geometryService.SimplifyCompleted += GeometryService_SimplifyCompleted;
            geometryService.Failed += GeometryService_Failed;

            List<Graphic> graphicsList = new List<Graphic>();
            graphicsList.Add(graphic);

            MyDrawObject.IsEnabled = false;

            geometryService.SimplifyAsync(graphicsList);
        }

        void GeometryService_SimplifyCompleted(object sender, GraphicsEventArgs e)
        {
            GeometryService geometryService =
            new GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Geometry/GeometryServer");
            geometryService.LabelPointsCompleted += GeometryService_LabelPointsCompleted;
            geometryService.Failed += GeometryService_Failed;

            geometryService.LabelPointsAsync(e.Results);
        }

        private void GeometryService_LabelPointsCompleted(object sender, GraphicsEventArgs args)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            foreach (Graphic graphic in args.Results)
            {
                graphic.Symbol = DefaultRasterSymbol;
                MapPoint mapPoint = graphic.Geometry as MapPoint;
                graphic.Attributes.Add("X", mapPoint.X);
                graphic.Attributes.Add("Y", mapPoint.Y);
                graphicsLayer.Graphics.Add(graphic);
            }
        }

        private void GeometryService_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Geometry Service error: " + e.Error);
        }
    }
}
