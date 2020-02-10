using System;
using System.Windows;
using System.Windows.Controls;
using ESRI.ArcGIS.Client;

namespace ArcGISSilverlightSDK
{
    public partial class SwitchMap : UserControl
    {
        public SwitchMap()
        {
            InitializeComponent();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the Tag from the selected radio button.  The Tag value
            // cooresponds to the index value of a layer in the map.  
            int index = Convert.ToInt32(((RadioButton)sender).Tag);

            ArcGISTiledMapServiceLayer arcgisLayer = MyMap.Layers["AGOLayer"] as ArcGISTiledMapServiceLayer;

            switch (index)
            {
                case 0:
                    arcgisLayer.Url = "http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_StreetMap_World_2D/MapServer";
                    break;
                case 1:
                    arcgisLayer.Url = "http://server.arcgisonline.com/ArcGIS/rest/services/NGS_Topo_US_2D/MapServer";
                    break;
                case 2:
                    arcgisLayer.Url = "http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_Imagery_World_2D/MapServer";
                    break;
            }
        }
    }
}
