using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;

namespace ArcGISSilverlightSDK
{
    public partial class MapLayersOnOff : UserControl
    {
        public MapLayersOnOff()
        {
            InitializeComponent();
        }

        void DynamicLayer_Initialized(object sender, EventArgs args)
        {
            ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer dynamicServiceLayer =
                sender as ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer;
            if (dynamicServiceLayer.VisibleLayers == null)
                dynamicServiceLayer.VisibleLayers = GetDefaultVisibleLayers(dynamicServiceLayer);
            UpdateLayerList(dynamicServiceLayer);
        }

        private int[] GetDefaultVisibleLayers(ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer dynamicService)
        {
            List<int> visibleLayerIDList = new List<int>();

            ESRI.ArcGIS.Client.LayerInfo[] layerInfoArray = dynamicService.Layers;

            for (int index = 0; index < layerInfoArray.Length; index++)
            {
                if (layerInfoArray[index].DefaultVisibility)
                    visibleLayerIDList.Add(index);
            }

            return visibleLayerIDList.ToArray();
        }

        private void UpdateLayerList(ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer dynamicServiceLayer)
        {
            int[] visibleLayerIDs = dynamicServiceLayer.VisibleLayers;

            if (visibleLayerIDs == null)
                visibleLayerIDs = GetDefaultVisibleLayers(dynamicServiceLayer);

            List<LayerListData> visibleLayerList = new List<LayerListData>();

            ESRI.ArcGIS.Client.LayerInfo[] layerInfoArray = dynamicServiceLayer.Layers;

            for (int index = 0; index < layerInfoArray.Length; index++)
            {
                visibleLayerList.Add(new LayerListData()
                {
                    Visible = visibleLayerIDs.Contains(index),
                    ServiceName = dynamicServiceLayer.ID,
                    LayerName = layerInfoArray[index].Name,
                    LayerIndex = index
                });
            }

            LayerVisibilityListBox.ItemsSource = visibleLayerList;
        }

        void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox tickedCheckBox = sender as CheckBox;

            string serviceName = tickedCheckBox.Tag.ToString();
            bool visible = (bool)tickedCheckBox.IsChecked;

            int layerIndex = Int32.Parse(tickedCheckBox.Name);

            ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer dynamicServiceLayer = MyMap.Layers[serviceName] as
                ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer;

            List<int> visibleLayerList =
                dynamicServiceLayer.VisibleLayers != null
                ? dynamicServiceLayer.VisibleLayers.ToList() : new List<int>();

            if (visible)
            {
                if (!visibleLayerList.Contains(layerIndex))
                    visibleLayerList.Add(layerIndex);
            }
            else
            {
                if (visibleLayerList.Contains(layerIndex))
                    visibleLayerList.Remove(layerIndex);
            }

            dynamicServiceLayer.VisibleLayers = visibleLayerList.ToArray();
        }

        public class LayerListData
        {
            public bool Visible { get; set; }
            public string ServiceName { get; set; }
            public string LayerName { get; set; }
            public int LayerIndex { get; set; }
        }
    }
}
