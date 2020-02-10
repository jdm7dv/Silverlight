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
    public partial class Scalebar : UserControl
    {
        public Scalebar()
        {
            InitializeComponent();
        }

        private void MyScaleBar_Loaded(object sender, RoutedEventArgs e)
        {
            ESRI.ArcGIS.Client.ScaleBar scaleBar = sender as ESRI.ArcGIS.Client.ScaleBar;
            if (scaleBar != null)
                scaleBar.Map = MyMap;
        }
    }
}
