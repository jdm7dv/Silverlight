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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Tasks;

namespace ArcGISSilverlightSDK
{
    public partial class BufferPoint : UserControl
    {
        public BufferPoint()
        {
            InitializeComponent();
        }

        private void MyMap_MouseClick(object sender, ESRI.ArcGIS.Client.Map.MouseEventArgs e)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.ClearGraphics();

            e.MapPoint.SpatialReference = new SpatialReference(4326);
            Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
            {
                Geometry = e.MapPoint,
                Symbol = DefaultClickSymbol
            };
            graphic.SetZIndex(1);
            graphicsLayer.Graphics.Add(graphic);

            GeometryService geometryService =
            new GeometryService("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Geometry/GeometryServer");
            geometryService.BufferCompleted += GeometryService_BufferCompleted;
            geometryService.Failed += GeometryService_Failed;

            // Use a projection appropriate for your area of interest
            BufferParameters bufferParams = new BufferParameters()
            {
                Unit = LinearUnit.StatuteMile,
                BufferSpatialReference = new SpatialReference(32611),
                OutSpatialReference = MyMap.SpatialReference
            };
            bufferParams.Features.Add(graphic);
            bufferParams.Distances.Add(10);

            geometryService.BufferAsync(bufferParams);
        }

        void GeometryService_BufferCompleted(object sender, GraphicsEventArgs args)
        {
            IList<Graphic> results = args.Results;
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            foreach (Graphic graphic in results)
            {
                graphic.Symbol = DefaultBufferSymbol;
                graphicsLayer.Graphics.Add(graphic);
            }
        }

        private void GeometryService_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Geometry Service error: " + e.Error);
        }
    }
}
