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
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Toolkit;
using ESRI.ArcGIS.Client.Symbols;
using System.Collections.ObjectModel;

namespace ArcGISSilverlightSDK
{
    public partial class MapTipWidget : UserControl
    {
        public MapTipWidget()
        {
            InitializeComponent();
        }

        private void MyGraphicsLayer_Initialized(object sender, EventArgs args)
        {
            ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query()
            {
                Geometry = new ESRI.ArcGIS.Client.Geometry.Envelope(-180, 0, 0, 90)
            };
            query.OutFields.Add("*");
            
            QueryTask queryTask = new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Demographics/ESRI_Census_USA/MapServer/5");
            queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            queryTask.ExecuteAsync(query);
        } 

        private void QueryTask_ExecuteCompleted(object sender, ESRI.ArcGIS.Client.Tasks.QueryEventArgs args)
        {
            FeatureSet featureSet = args.FeatureSet;
  
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            MyMapTip.GraphicsLayer = graphicsLayer;

            if (featureSet != null && featureSet.Features.Count > 0)
            {
                foreach (Graphic feature in featureSet.Features)
                {
                    feature.Symbol = DefaultFillSymbol;
                    graphicsLayer.Graphics.Add(feature);
                }
            }
        }
    }
}
