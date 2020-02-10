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
using System.Windows.Data;
using ESRI.ArcGIS.Client.ValueConverters;

namespace ArcGISSilverlightSDK
{
    public partial class AttributeQuery : UserControl
    {
        public AttributeQuery()
        {
            InitializeComponent();

            QueryTask queryTask = new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Demographics/ESRI_Census_USA/MapServer/5");
            queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            queryTask.Failed += QueryTask_Failed;

            ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query();

            // Specify fields to return from initial query
            query.OutFields.AddRange(new string[] { "STATE_NAME" });

            // Return all features
            query.Where = "1=1";
            queryTask.ExecuteAsync(query);
        }

        private void QueryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = QueryComboBox.SelectedIndex;

            if (index > 0)
            {
                QueryTask queryTask = new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                    "Demographics/ESRI_Census_USA/MapServer/5");
                queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
                queryTask.Failed += QueryTask_Failed;

                ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query();
                query.OutFields.Add("*");
                query.Text = QueryComboBox.SelectedItem.ToString();

                queryTask.ExecuteAsync(query);

                // Remove the first entry ("Select...")
                QueryComboBox.Items.RemoveAt(0);
            }
        }

        private void QueryTask_ExecuteCompleted(object sender, ESRI.ArcGIS.Client.Tasks.QueryEventArgs args)
        {
            FeatureSet featureSet = args.FeatureSet;

            // If initial query to populate states combobox
            if (featureSet.Features[0].Attributes.Count == 1)
            {
                // Just show on initial load
                QueryComboBox.Items.Add("Select...");

                foreach (Graphic graphic in args.FeatureSet.Features)
                {
                    QueryComboBox.Items.Add(graphic.Attributes["STATE_NAME"].ToString());
                }

                QueryComboBox.SelectedIndex = 0;
                return;
            }

            // If an item has been selected            
            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.ClearGraphics();

            if (featureSet != null && featureSet.Features.Count > 0)
            {
                // Show selected feature attributes in DataGrid
                Graphic selectedFeature = featureSet.Features[0];

                QueryDetailsDataGrid.ItemsSource = selectedFeature.Attributes;

                // Hightlight selected feature
                selectedFeature.Symbol = DefaultFillSymbol;
                graphicsLayer.Graphics.Add(selectedFeature);

                // Zoom to selected feature (define expand percentage)
                ESRI.ArcGIS.Client.Geometry.Envelope selectedFeatureExtent = selectedFeature.Geometry.Extent;

                double expandPercentage = 30;

                double widthExpand = selectedFeatureExtent.Width * (expandPercentage / 100);
                double heightExpand = selectedFeatureExtent.Height * (expandPercentage / 100);

                ESRI.ArcGIS.Client.Geometry.Envelope displayExtent = new ESRI.ArcGIS.Client.Geometry.Envelope(
                selectedFeatureExtent.XMin - (widthExpand / 2),
                selectedFeatureExtent.YMin - (heightExpand / 2),
                selectedFeatureExtent.XMax + (widthExpand / 2),
                selectedFeatureExtent.YMax + (heightExpand / 2));

                MyMap.ZoomTo(displayExtent);

                // If DataGrid not visible (initial load), show it
                if (DataGridScrollViewer.Visibility == Visibility.Collapsed)
                {
                    DataGridScrollViewer.Visibility = Visibility.Visible;
                    QueryGrid.Height = Double.NaN;
                    QueryGrid.UpdateLayout();
                }
            }
            else
            {
                QueryDetailsDataGrid.ItemsSource = null;
                DataGridScrollViewer.Visibility = Visibility.Collapsed;
                QueryGrid.Height = Double.NaN;
                QueryGrid.UpdateLayout();
            }
        }

        private void QueryTask_Failed(object sender, TaskFailedEventArgs args)
        {
            MessageBox.Show("Query failed: " + args.Error);
        }
    }
}
