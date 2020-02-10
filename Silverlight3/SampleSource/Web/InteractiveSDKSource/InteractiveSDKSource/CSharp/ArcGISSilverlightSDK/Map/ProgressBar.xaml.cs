using System;
using System.Windows;
using System.Windows.Controls;

namespace ArcGISSilverlightSDK
{
    public partial class ProgressBar : UserControl
    {
        public ProgressBar()
        {
            InitializeComponent();
        }

        private void MyMap_Progress(object sender, ESRI.ArcGIS.Client.ProgressEventArgs args)
        {
            if (args.Progress < 100)
            {
                progressGrid.Visibility = Visibility.Visible;
                MyProgressBar.Value = args.Progress;
                ProgressValueTextBlock.Text = String.Format("{0}%", args.Progress);
            }
            else
            {
                progressGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
