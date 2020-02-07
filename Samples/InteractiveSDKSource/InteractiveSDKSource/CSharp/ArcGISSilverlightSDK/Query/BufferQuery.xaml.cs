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
using System.Windows.Media.Imaging;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Tasks;

namespace ArcGISSilverlightSDK
{
    public partial class BufferQuery : UserControl
    {
        GeometryService _geometryService;
        QueryTask _queryTask;

        public BufferQuery()
        {
            InitializeComponent();

            _geometryService =
            new GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Geometry/GeometryServer");
            _geometryService.BufferCompleted += GeometryService_BufferCompleted;
            _geometryService.Failed += GeometryService_Failed;

            _queryTask = new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Portland/Portland_ESRI_LandBase_AGO/MapServer/1");
            _queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            _queryTask.Failed += QueryTask_Failed;
        }

        private void MyMap_MouseClick(object sender, ESRI.ArcGIS.Client.Map.MouseEventArgs e)
        {
            _geometryService.CancelAsync();
            _queryTask.CancelAsync();

            Graphic clickGraphic = new Graphic();
            clickGraphic.Symbol = DefaultMarkerSymbol;
            clickGraphic.Geometry = e.MapPoint;
            // Input spatial reference for buffer operation defined by first feature of input geometry array
            clickGraphic.Geometry.SpatialReference = MyMap.SpatialReference;

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.ClearGraphics();

            clickGraphic.SetZIndex(2);
            graphicsLayer.Graphics.Add(clickGraphic);

            // Use a projection appropriate for your area of interest
            ESRI.ArcGIS.Client.Tasks.BufferParameters bufferParams = new ESRI.ArcGIS.Client.Tasks.BufferParameters()
            {
                BufferSpatialReference = new SpatialReference(32611),
                OutSpatialReference = MyMap.SpatialReference,
                Unit = LinearUnit.Meter
            };
            bufferParams.Distances.Add(100);
            bufferParams.Features.Add(clickGraphic);

            _geometryService.BufferAsync(bufferParams);
        }

        void GeometryService_BufferCompleted(object sender, GraphicsEventArgs args)
        {
            Graphic bufferGraphic = new Graphic();
            bufferGraphic.Geometry = args.Results[0].Geometry;
            bufferGraphic.Symbol = BufferSymbol;
            bufferGraphic.SetZIndex(1);

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Add(bufferGraphic);
            
            ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query();
            query.ReturnGeometry = true;
            query.Geometry = bufferGraphic.Geometry;
            _queryTask.ExecuteAsync(query);
        }

        private void QueryTask_ExecuteCompleted(object sender, QueryEventArgs args)
        {
            if (args.FeatureSet == null)
                return;

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            foreach (Graphic selectedGraphic in args.FeatureSet.Features)
            {
                selectedGraphic.Symbol = ParcelSymbol;
                graphicsLayer.Graphics.Add(selectedGraphic);
            }
        }

        private void GeometryService_Failed(object sender, TaskFailedEventArgs args)
        {
            MessageBox.Show("Geometry service failed: " + args.Error);
        }

        private void QueryTask_Failed(object sender, TaskFailedEventArgs args)
        {
            MessageBox.Show("Query failed: " + args.Error);
        }

    }
}
