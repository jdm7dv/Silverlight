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
    public partial class Simplify : UserControl
    {
        private Graphic _unsimplifiedGraphic = new Graphic();

        public Simplify()
        {
            InitializeComponent();
        }

        private void MyMap_ExtentChanged(object sender, ExtentEventArgs e)
        {
            if (e.OldExtent == null)
                drawPolygon();
        }

        private void drawPolygon()
        {
            MapPoint center = MyMap.Extent.GetCenter();
            double lat = center.Y;
            double lon = center.X + 500;
            double latOffset = 500;
            double lonOffset = 500;
            ESRI.ArcGIS.Client.Geometry.PointCollection points = new ESRI.ArcGIS.Client.Geometry.PointCollection()
            {
                new MapPoint(lon - lonOffset, lat),
                new MapPoint(lon, lat + latOffset),
                new MapPoint(lon + lonOffset, lat),
                new MapPoint(lon, lat - latOffset),
                new MapPoint(lon - lonOffset, lat),
                new MapPoint(lon - 2 * lonOffset, lat + latOffset),
                new MapPoint(lon - 3 * lonOffset, lat),
                new MapPoint(lon - 2 * lonOffset, lat - latOffset),
                new MapPoint(lon - 1.5 * lonOffset, lat + latOffset),
                new MapPoint(lon - lonOffset, lat)
            };
            ESRI.ArcGIS.Client.Geometry.Polygon polygon = new ESRI.ArcGIS.Client.Geometry.Polygon();
            polygon.Rings.Add(points);
            polygon.SpatialReference = MyMap.SpatialReference;
            _unsimplifiedGraphic.Geometry = polygon;

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            _unsimplifiedGraphic.Symbol = PolygonFillSymbol;
            graphicsLayer.Graphics.Add(_unsimplifiedGraphic);
        }

        private void QueryOnlyButton_Click(object sender, RoutedEventArgs e)
        {
            doQuery(_unsimplifiedGraphic.Geometry);
        }

        private void SimplifyAndQueryButton_Click(object sender, RoutedEventArgs e)
        {
            GeometryService geometryService =
            new GeometryService("http://serverapps.esri.com/ArcGIS/rest/services/" +
            "Geometry/GeometryServer");
            geometryService.SimplifyCompleted += GeometryService_SimplifyCompleted;
            geometryService.Failed += GeometryService_Failed;

            List<Graphic> graphicList = new List<Graphic>();
            graphicList.Add(_unsimplifiedGraphic);
            geometryService.SimplifyAsync(graphicList);
        }

        private void GeometryService_SimplifyCompleted(object sender, GraphicsEventArgs args)
        {
            doQuery(args.Results[0].Geometry);
        }

        private void doQuery(ESRI.ArcGIS.Client.Geometry.Geometry geometry)
        {
            QueryTask queryTask = new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Portland/ESRI_LandBase_WebMercator/MapServer/1");
            queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            queryTask.Failed += QueryTask_Failed;

            ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query()
            {
                Geometry = geometry,
                SpatialRelationship = SpatialRelationship.esriSpatialRelContains
            };
            query.OutFields.Add("*");
            queryTask.ExecuteAsync(query);
        }

        private void QueryTask_ExecuteCompleted(object sender, QueryEventArgs args)
        {
            if (args.FeatureSet == null)
                return;
            FeatureSet featureSet = args.FeatureSet;
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.ClearGraphics();

            if (featureSet != null && featureSet.Features.Count > 0)
            {
                foreach (Graphic feature in featureSet.Features)
                {
                    ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
                    {
                        Geometry = feature.Geometry,
                        Symbol = ParcelFillSymbol
                    };
                    graphicsLayer.Graphics.Add(graphic);
                }
            }
            graphicsLayer.Graphics.Add(_unsimplifiedGraphic);
        }

        private void GeometryService_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Geometry Service error: " + e.Error);
        }

        private void QueryTask_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Query failed: " + e.Error);
        }
    }
}
