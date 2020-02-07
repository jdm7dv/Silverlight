using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Toolkit;

namespace ArcGISSilverlightSDK
{
    public partial class DrawGraphics : UserControl
    {
        private Draw MyDrawObject;
        private Symbol _activeSymbol = null;

        public DrawGraphics()
        {
            InitializeComponent();

            MyDrawObject = new Draw(MyMap)
            {
                LineSymbol = LayoutRoot.Resources["DrawLineSymbol"] as LineSymbol,
                FillSymbol = LayoutRoot.Resources["DrawFillSymbol"] as FillSymbol
            };

            MyDrawObject.DrawComplete += MyDrawObject_DrawComplete;
        }

        private void MyToolbar_ToolbarItemClicked(object sender, ESRI.ArcGIS.Client.Toolkit.SelectedToolbarItemArgs e)
        {
            switch (e.Index)
            {
                case 0: // Point
                    MyDrawObject.DrawMode = DrawMode.Point;
                    _activeSymbol = LayoutRoot.Resources["DefaultMarkerSymbol"] as Symbol;
                    break;
                case 1: // Polyline
                    MyDrawObject.DrawMode = DrawMode.Polyline;
                    _activeSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as Symbol;
                    break;
                case 2: // Polygon
                    MyDrawObject.DrawMode = DrawMode.Polygon;
                    _activeSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as Symbol;
                    break;
                case 3: // Rectangle
                    MyDrawObject.DrawMode = DrawMode.Rectangle;
                    _activeSymbol = LayoutRoot.Resources["DefaultFillSymbol"] as Symbol;
                    break;
                case 4: // Freehand
                    MyDrawObject.DrawMode = DrawMode.Freehand;
                    _activeSymbol = LayoutRoot.Resources["DefaultLineSymbol"] as Symbol;
                    break;
                default: // Clear Graphics
                    MyDrawObject.DrawMode = DrawMode.None;
                    GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
                    graphicsLayer.ClearGraphics();
                    break;
            }
            MyDrawObject.IsEnabled = (MyDrawObject.DrawMode != DrawMode.None);
        }

        private void MyToolbar_ToolbarIndexChanged(object sender, ESRI.ArcGIS.Client.Toolkit.SelectedToolbarItemArgs e)
        {
            StatusTextBlock.Text = e.Item.Text;
        }


        private void MyDrawObject_DrawComplete(object sender, ESRI.ArcGIS.Client.DrawEventArgs args)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            ESRI.ArcGIS.Client.Graphic graphic = new ESRI.ArcGIS.Client.Graphic()
            {
                Geometry = args.Geometry,
                Symbol = _activeSymbol,
            };
            graphicsLayer.Graphics.Add(graphic);
        }
    }
}
