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

namespace ArcGISSilverlightSDK
{
    public partial class AddLayerDynamically : UserControl
    {
        public AddLayerDynamically()
        {
            InitializeComponent();
        }

        private void AddLayerButton_Click(object sender, RoutedEventArgs e)
        {
            MyMap.Layers.Clear();

            ESRI.ArcGIS.Client.ArcGISTiledMapServiceLayer NewTiledLayer = new ESRI.ArcGIS.Client.ArcGISTiledMapServiceLayer();

            NewTiledLayer.Initialized += (evtsender, args) =>
            {
                MyMap.ZoomTo(NewTiledLayer.InitialExtent);
            };

            NewTiledLayer.Url = UrlTextBox.Text;
            MyMap.Layers.Add(NewTiledLayer);
        }
    }
}
