using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Tasks;

namespace ArcGISSilverlightSDK
{
    public partial class GraphicsMapTip : UserControl
    {
        public GraphicsMapTip()
        {
            InitializeComponent();
        }

        void MyGraphics_Initialized(object sender, EventArgs args)
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

        void QueryTask_ExecuteCompleted(object sender, ESRI.ArcGIS.Client.Tasks.QueryEventArgs queryArgs)
        {
            if (queryArgs.FeatureSet == null)
                return;

            FeatureSet resultFeatureSet = queryArgs.FeatureSet;
            ESRI.ArcGIS.Client.GraphicsLayer graphicsLayer =
            MyMap.Layers["MyGraphicsLayer"] as ESRI.ArcGIS.Client.GraphicsLayer;

            if (resultFeatureSet != null && resultFeatureSet.Features.Count > 0)
            {
                foreach (ESRI.ArcGIS.Client.Graphic graphicFeature in resultFeatureSet.Features)
                {
                    graphicFeature.Symbol = DefaultFillSymbol;
                    graphicsLayer.Graphics.Add(graphicFeature);
                }
            }
        }
    }
}
