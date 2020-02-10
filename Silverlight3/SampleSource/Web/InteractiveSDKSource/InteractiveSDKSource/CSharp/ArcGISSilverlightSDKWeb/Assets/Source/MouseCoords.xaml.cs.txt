using System;
using System.Windows;
using System.Windows.Controls;

namespace ArcGISSilverlightSDK
{
    public partial class MouseCoords : UserControl
    {
        public MouseCoords()
        {
            InitializeComponent();
        }

        private void StreetMap_Initialized(object sender, EventArgs args)
        {
            MyMap.MouseMove += new System.Windows.Input.MouseEventHandler(MyMap_MouseMove); 
        }

        private void MyMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs args)
        {
            if (MyMap.Extent != null)
            {
                System.Windows.Point screenPoint = args.GetPosition(MyMap);
                ScreenCoordsTextBlock.Text = string.Format("Screen Coords: X = {0}, Y = {1}", 
                    screenPoint.X, screenPoint.Y);

                ESRI.ArcGIS.Client.Geometry.MapPoint mapPoint = MyMap.ScreenToMap(screenPoint);
                MapCoordsTextBlock.Text = string.Format("Map Coords: X = {0}, Y = {1}",
                    Math.Round(mapPoint.X, 4), Math.Round(mapPoint.Y, 4));
            }
        }
    }
}