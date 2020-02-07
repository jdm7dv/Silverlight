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
using ESRI.ArcGIS.Client.Toolkit;
using System.Windows.Data;
using ESRI.ArcGIS.Client.ValueConverters;

namespace ArcGISSilverlightSDK
{
    public partial class SpatialQuery : UserControl
    {
        private Symbol _inputSymbol = null;
        private Draw MyDrawSurface;

        public SpatialQuery()
        {
            InitializeComponent();

            MyDrawSurface = new Draw(MyMap)
            {
                LineSymbol = DefaultLineSymbol,
                FillSymbol = DefaultFillSymbol
            };
            MyDrawSurface.DrawComplete += MyDrawSurface_DrawComplete;
        }

        private void esriTools_ToolbarItemClicked(object sender, ESRI.ArcGIS.Client.Toolkit.SelectedToolbarItemArgs e)
        {
            switch (e.Index)
            {
                case 0: // Point
                    MyDrawSurface.DrawMode = DrawMode.Point;
                    _inputSymbol = DefaultMarkerSymbol;
                    break;
                case 1: // Polyline
                    MyDrawSurface.DrawMode = DrawMode.Polyline;
                    _inputSymbol = DefaultLineSymbol;
                    break;
                case 2: // Polygon
                    MyDrawSurface.DrawMode = DrawMode.Polygon;
                    _inputSymbol = DefaultFillSymbol;
                    break;
                case 3: // Rectangle
                    MyDrawSurface.DrawMode = DrawMode.Rectangle;
                    _inputSymbol = DefaultFillSymbol;
                    break;
                default: // Clear
                    MyDrawSurface.DrawMode = DrawMode.None;
                    GraphicsLayer selectionGraphicslayer = MyMap.Layers["MySelectionGraphicsLayer"] as GraphicsLayer;
                    selectionGraphicslayer.ClearGraphics();
                    GraphicsLayer drawGraphicslayer = MyMap.Layers["MyDrawGraphicsLayer"] as GraphicsLayer;
                    drawGraphicslayer.ClearGraphics();
                    QueryDetailsDataGrid.ItemsSource = null;
                    break;
            }
            MyDrawSurface.IsEnabled = (MyDrawSurface.DrawMode != DrawMode.None);
            StatusTextBlock.Text = e.Item.Text;
        }

        private void MyDrawSurface_DrawComplete(object sender, ESRI.ArcGIS.Client.DrawEventArgs args)
        {
            GraphicsLayer selectionGraphicslayer = MyMap.Layers["MySelectionGraphicsLayer"] as GraphicsLayer;
            selectionGraphicslayer.ClearGraphics();

            GraphicsLayer drawGraphicslayer = MyMap.Layers["MyDrawGraphicsLayer"] as GraphicsLayer;
            drawGraphicslayer.ClearGraphics();

            ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
            {
                Geometry = args.Geometry,
                Symbol = _inputSymbol
            };

            drawGraphicslayer.Graphics.Add(graphic);

            QueryTask queryTask = new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Demographics/ESRI_Census_USA/MapServer/5");
            queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            queryTask.Failed += QueryTask_Failed;

            // Bind data grid to query results
            Binding resultFeaturesBinding = new Binding("LastResult.Features");
            resultFeaturesBinding.Source = queryTask;
            QueryDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding);
            Query query = new ESRI.ArcGIS.Client.Tasks.Query();

            // Specify fields to return from query
            query.OutFields.AddRange(new string[] { "STATE_NAME", "SUB_REGION", "STATE_FIPS", "STATE_ABBR", "POP2000", "POP2007" });
            query.Geometry = args.Geometry;

            // Return geometry with result features
            query.ReturnGeometry = true;

            queryTask.ExecuteAsync(query);
        }

        private void QueryTask_ExecuteCompleted(object sender, ESRI.ArcGIS.Client.Tasks.QueryEventArgs args)
        {
            FeatureSet featureSet = args.FeatureSet;

            if (featureSet == null || featureSet.Features.Count < 1)
            {
                MessageBox.Show("No features retured from query");
                return;
            }

            GraphicsLayer graphicsLayer = MyMap.Layers["MySelectionGraphicsLayer"] as GraphicsLayer;

            if (featureSet != null && featureSet.Features.Count > 0)
            {
                foreach (Graphic feature in featureSet.Features)
                {
                    feature.Symbol = ResultsFillSymbol;

                    //graphicsLayer.Graphics.Add(feature);
                    graphicsLayer.Graphics.Insert(0, feature);
                }
            }

            MyDrawSurface.IsEnabled = false;
        }

        private void QueryTask_Failed(object sender, TaskFailedEventArgs args)
        {
            MessageBox.Show("Query failed: " + args.Error);
        }
    }

    // Helper class to simplify XAML data grid column declaration
    public class GraphicAttributeColumn : DataGridTextColumn
    {
        public GraphicAttributeColumn()
        {
            System.Windows.Data.Binding b = new System.Windows.Data.Binding("Attributes");
            b.Converter = new DictionaryConverter();
            this.Binding = b;
        }

        private string attributeName;
        public string AttributeName
        {
            get { return attributeName; }
            set
            {
                if (Binding != null && Binding.Converter is DictionaryConverter)
                    Binding.ConverterParameter = value;
                attributeName = value;
            }
        }
    }
}
