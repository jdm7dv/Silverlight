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
using ESRI.ArcGIS.Client.Services;

namespace ArcGISSilverlightSDK
{
    public partial class MessageInABottle : UserControl
    {
        public MessageInABottle()
        {
            InitializeComponent();
        }

        private void MyMap_MouseClick(object sender, ESRI.ArcGIS.Client.Map.MouseEventArgs e)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            Graphic graphic = new Graphic()
            {
                Symbol = StartMarkerSymbol,
                Geometry = e.MapPoint
            };
            graphicsLayer.Graphics.Add(graphic);

            Geoprocessor geoprocessorTask = new Geoprocessor("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Specialty/ESRI_Currents_World/GPServer/MessageInABottle");
            geoprocessorTask.ExecuteCompleted += GeoprocessorTask_ExecuteCompleted;
            geoprocessorTask.Failed += GeoprocessorTask_Failed;

            List<GPParameter> parameters = new List<GPParameter>();
            parameters.Add(new GPFeatureRecordSetLayer("Input_Point", e.MapPoint));
            parameters.Add(new GPDouble("Days", Convert.ToDouble(DaysTextBox.Text)));

            geoprocessorTask.ExecuteAsync(parameters);
        }

        private void GeoprocessorTask_ExecuteCompleted(object sender, GPExecuteCompleteEventArgs e)
        {
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;

            foreach (GPParameter gpParameter in e.Results.OutParameters)
            {
                if (gpParameter is GPFeatureRecordSetLayer)
                {
                    GPFeatureRecordSetLayer gpLayer = gpParameter as GPFeatureRecordSetLayer;
                    foreach (Graphic graphic in gpLayer.FeatureSet.Features)
                    {
                        graphic.Symbol = PathLineSymbol;
                        graphicsLayer.Graphics.Add(graphic);
                    }
                }
            }
        }

        private void GeoprocessorTask_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Geoprocessor service failed: " + e.Error);
        }
    }
}
