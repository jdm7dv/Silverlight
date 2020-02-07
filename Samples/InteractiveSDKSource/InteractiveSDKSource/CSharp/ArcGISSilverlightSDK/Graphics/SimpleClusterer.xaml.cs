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
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client;

namespace ArcGISSilverlightSDK
{
    public partial class SimpleClusterer : UserControl
    {
        public SimpleClusterer()
        {
            InitializeComponent();
        }

        private void LoadGraphics()
        {
            QueryTask queryTask = 
                new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Specialty/ESRI_StatesCitiesRivers_USA/MapServer/0");
            queryTask.ExecuteCompleted += queryTask_ExecuteCompleted;

            Query query = new ESRI.ArcGIS.Client.Tasks.Query();
            query.OutSpatialReferenceWKID = MyMap.SpatialReference.WKID;
            query.ReturnGeometry = true;
            query.Where = "1=1";
            queryTask.ExecuteAsync(query);
        }

        void queryTask_ExecuteCompleted(object sender, QueryEventArgs args)
        {
            FeatureSet featureSet = args.FeatureSet;

            if (featureSet == null || featureSet.Features.Count < 1)
            {
                MessageBox.Show("No features retured from query");
                return;
            }

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            
            foreach (Graphic graphic in featureSet.Features)
            {
                graphic.Symbol = MediumMarkerSymbol;
                graphicsLayer.Graphics.Add(graphic);
            }
        }

        private void MyMap_ExtentChanged(object sender, ExtentEventArgs e)
        {
            if (e.OldExtent == null)
            LoadGraphics();
        }    
    }
}
