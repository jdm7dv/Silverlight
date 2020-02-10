using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ESRI.ArcGIS.Client.ValueConverters;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client;
using System.Collections.Generic;

namespace ArcGISSilverlightSDK
{
    public partial class QueryWithoutMap : UserControl
    {
        public QueryWithoutMap()
        {
            InitializeComponent();
        }

        void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            QueryTask queryTask = new QueryTask("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Demographics/ESRI_Census_USA/MapServer/5");
            queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            queryTask.Failed += QueryTask_Failed;

            ESRI.ArcGIS.Client.Tasks.Query query = new ESRI.ArcGIS.Client.Tasks.Query();
            query.Text = StateNameTextBox.Text;

            query.OutFields.Add("*");
            queryTask.ExecuteAsync(query);
        }

        void QueryTask_ExecuteCompleted(object sender, ESRI.ArcGIS.Client.Tasks.QueryEventArgs args)
        {
            FeatureSet featureSet = args.FeatureSet;

            if (featureSet != null && featureSet.Features.Count > 0)
            {
                List<QueryResultData> resultList = new List<QueryResultData>();
                foreach (Graphic feature in featureSet.Features)
                {
                    resultList.Add(new QueryResultData()
                    {
                        STATE_NAME = feature.Attributes["STATE_NAME"].ToString(),
                        SUB_REGION = feature.Attributes["SUB_REGION"].ToString(),
                        STATE_FIPS = feature.Attributes["STATE_FIPS"].ToString(),
                        STATE_ABBR = feature.Attributes["STATE_ABBR"].ToString(),
                        POP2000 = feature.Attributes["POP2000"].ToString(),
                        POP2007 = feature.Attributes["POP2007"].ToString()
                    });
                }
                QueryDetailsDataGrid.ItemsSource = resultList;
            }
            else
            {
                MessageBox.Show("No features returned from query");
            }
        }


        private void QueryTask_Failed(object sender, TaskFailedEventArgs args)
        {
            MessageBox.Show("Query execute error: " + args.Error);
        }

        public class QueryResultData
        {
            public string STATE_NAME { get; set; }
            public string SUB_REGION { get; set; }
            public string STATE_FIPS { get; set; }
            public string STATE_ABBR { get; set; }
            public string POP2000 { get; set; }
            public string POP2007 { get; set; }
        }
    }
}

