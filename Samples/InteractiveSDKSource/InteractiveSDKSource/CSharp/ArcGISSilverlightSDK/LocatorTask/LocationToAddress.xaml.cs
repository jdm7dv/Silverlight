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
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;

namespace ArcGISSilverlightSDK
{
    public partial class LocationToAddress : UserControl
    {
        public LocationToAddress()
        {
            InitializeComponent();
        }

        private void MyMap_MouseClick(object sender, ESRI.ArcGIS.Client.Map.MouseEventArgs e)
        {
            Locator locatorTask = new Locator("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Locators/" +
                "ESRI_Geocode_USA/GeocodeServer");
            locatorTask.LocationToAddressCompleted += LocatorTask_LocationToAddressCompleted;
            locatorTask.Failed += LocatorTask_Failed;

            // Tolerance (distance) specified in meters
            double tolerance = 20;
            locatorTask.LocationToAddressAsync(e.MapPoint, tolerance);
        }

        private void LocatorTask_LocationToAddressCompleted(object sender, AddressEventArgs args)
        {
            Address address = args.Address;
            Dictionary<string, object> attributes = address.Attributes;

            Graphic graphic = new Graphic()
            {
                Symbol = DefaultMarkerSymbol,
                Geometry = address.Location,
            };

            string latlon = String.Format("{0}, {1}", address.Location.X, address.Location.Y);
            string address1 = attributes["Address"].ToString();
            string address2 = String.Format("{0}, {1} {2}", attributes["City"], attributes["State"], attributes["Zip"]);

            graphic.Attributes.Add("LatLon", latlon);
            graphic.Attributes.Add("Address1", address1);
            graphic.Attributes.Add("Address2", address2);

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.Graphics.Add(graphic);
        }

        private void LocatorTask_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Locator service failed: " + e.Error);
        }
    }
}
