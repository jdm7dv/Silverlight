using System;
using System.Windows;
using System.Windows.Controls;

namespace ArcGISSilverlightSDK
{
    public partial class MapExtent : UserControl
    {
        public MapExtent()
        {
            InitializeComponent();
        }

        void MyMap_ExtentChange(object sender, ESRI.ArcGIS.Client.ExtentEventArgs args)
        {
            setExtentText(args.NewExtent);
        }
      
        private void setExtentText(ESRI.ArcGIS.Client.Geometry.Envelope newExtent)
        {
            ExtentTextBlock.Text = string.Format("MinX: {0}\nMinY: {1}\nMaxX: {2}\nMaxY: {3}",
                newExtent.XMin, newExtent.YMin, newExtent.XMax, newExtent.YMax);
        }
    }
}