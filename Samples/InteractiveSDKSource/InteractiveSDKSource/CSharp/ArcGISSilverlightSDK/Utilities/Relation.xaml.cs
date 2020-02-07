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
using System.Text;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Tasks;

namespace ArcGISSilverlightSDK
{
    public partial class Relation : UserControl
    {
        public Relation()
        {
            InitializeComponent();

            MyMap.Layers.LayersInitialized += Layers_LayersInitialized;
        }

        void Layers_LayersInitialized(object sender, EventArgs args)
        {
            GraphicsLayer polygonLayer = MyMap.Layers["MyPolygonGraphicsLayer"] as GraphicsLayer;

            for (int i = 0; i < polygonLayer.Graphics.Count; i++)
            {
                Graphic graphic = polygonLayer.Graphics[i];

                if (!graphic.Attributes.ContainsKey("Name"))
                {
                    graphic.Attributes.Add("Name", String.Format("Polygon_{0}", i));
                    graphic.Attributes.Add("Relation", null);
                }
            }
        }

        private void MyMap_MouseClick(object sender, ESRI.ArcGIS.Client.Map.MouseEventArgs e)
        {
            GraphicsLayer pointLayer = MyMap.Layers["MyPointGraphicsLayer"] as GraphicsLayer;
            string name = String.Format("Point_{0}", pointLayer.Graphics.Count);

            MapPoint mapPoint = e.MapPoint;
            mapPoint.SpatialReference = MyMap.SpatialReference;

            Graphic graphic = new Graphic()
            {
                Symbol = DefaultPointMarkerSymbol,
                Geometry = mapPoint,
            };

            graphic.Attributes.Add("Name", name);
            graphic.Attributes.Add("Relation", null);

            pointLayer.Graphics.Add(graphic);
        }

        private void ExecuteRelationButton_Click(object sender, RoutedEventArgs e)
        {
            GeometryService geometryService =
            new GeometryService("http://sampleserver2.arcgisonline.com/ArcGIS/rest/services/" +
                "Geometry/GeometryServer");
            geometryService.RelationCompleted += GeometryService_RelationCompleted;
            geometryService.Failed += GeometryService_Failed;

            GraphicsLayer pointLayer = MyMap.Layers["MyPointGraphicsLayer"] as GraphicsLayer;
            if (pointLayer.Graphics.Count < 1)
            {
                MessageBox.Show("No points available");
                return;
            }

            foreach (Graphic graphic in pointLayer.Graphics)
                graphic.Attributes["Relation"] = null;

            GraphicsLayer polygonLayer = MyMap.Layers["MyPolygonGraphicsLayer"] as GraphicsLayer;
            foreach (Graphic graphic in polygonLayer.Graphics)
                graphic.Attributes["Relation"] = null;

            ExecuteRelationButton.Visibility = Visibility.Collapsed;

            geometryService.RelationAsync(
            pointLayer.Graphics.ToList(),
            polygonLayer.Graphics.ToList(),
            GeometryRelation.esriGeometryRelationWithin, null);
        }

        private void GeometryService_RelationCompleted(object sender, RelationEventArgs args)
        {
            GraphicsLayer pointLayer = MyMap.Layers["MyPointGraphicsLayer"] as GraphicsLayer;
            GraphicsLayer polygonLayer = MyMap.Layers["MyPolygonGraphicsLayer"] as GraphicsLayer;

            List<GeometryRelationPair> results = args.Results;
            foreach (GeometryRelationPair pair in results)
            {
                if (pointLayer.Graphics[pair.Graphic1Index].Attributes["Relation"] == null)
                {
                    pointLayer.Graphics[pair.Graphic1Index].Attributes["Relation"] =
                    string.Format("Within Polygon {0}", pair.Graphic2Index);
                }
                else
                {
                    pointLayer.Graphics[pair.Graphic1Index].Attributes["Relation"] +=
                    "," + pair.Graphic2Index.ToString();
                }

                if (polygonLayer.Graphics[pair.Graphic2Index].Attributes["Relation"] == null)
                {
                    polygonLayer.Graphics[pair.Graphic2Index].Attributes["Relation"] =
                    string.Format("Contains Point {0}", pair.Graphic1Index);
                }
                else
                {
                    polygonLayer.Graphics[pair.Graphic2Index].Attributes["Relation"] +=
                    "," + pair.Graphic1Index.ToString();
                }
            }

            ExecuteRelationButton.Visibility = Visibility.Visible;
        }

        private void GeometryService_Failed(object sender, TaskFailedEventArgs args)
        {
            MessageBox.Show("Geometry Service error: " + args.Error);
        }
    }
}
