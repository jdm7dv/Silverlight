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
    public partial class AddressToLocation : UserControl
    {
        List<AddressCandidate> _candidateList;
        private bool _firstZoom = true;
        private int _lastIndex = 0;

        public AddressToLocation()
        {
            InitializeComponent();
        }

        private void FindAddressButton_Click(object sender, RoutedEventArgs e)
        {
            Locator locatorTask = new Locator("http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/" +
                "Locators/ESRI_Geocode_USA/GeocodeServer");
            locatorTask.AddressToLocationsCompleted += LocatorTask_AddressToLocationsCompleted;
            locatorTask.Failed += LocatorTask_Failed;
            AddressToLocationsParameters addressParams = new AddressToLocationsParameters();
            Dictionary<string, string> address = addressParams.Address;

            if (!string.IsNullOrEmpty(State.Text))
                address.Add("Address", Address.Text);
            if (!string.IsNullOrEmpty(City.Text))
                address.Add("City", City.Text);
            if (!string.IsNullOrEmpty(State.Text))
                address.Add("State", State.Text);
            if (!string.IsNullOrEmpty(Zip.Text))
                address.Add("Zip", Zip.Text);

            locatorTask.AddressToLocationsAsync(addressParams);
        }

        private void LocatorTask_AddressToLocationsCompleted(object sender, ESRI.ArcGIS.Client.Tasks.AddressToLocationsEventArgs args)
        {
            List<AddressCandidate> returnedCandidates = args.Results;

            GraphicsLayer graphicsLayer = MyMap.Layers["MyGraphicsLayer"] as GraphicsLayer;
            graphicsLayer.ClearGraphics();

            _candidateList = new List<AddressCandidate>();
            ListBox candidateListBox = new ListBox();

            foreach (AddressCandidate candidate in returnedCandidates)
            {
                if (candidate.Score >= 65)
                {
                    _candidateList.Add(candidate);
                    candidateListBox.Items.Add(candidate.Address);

                    Graphic graphic = new Graphic()
                    {
                        Symbol = DefaultMarkerSymbol,
                        Geometry = candidate.Location
                    };

                    graphic.Attributes.Add("Address", candidate.Address);

                    string latlon = String.Format("{0}, {1}", candidate.Location.X, candidate.Location.Y);
                    graphic.Attributes.Add("LatLon", latlon);

                    graphicsLayer.Graphics.Add(graphic);
                }
            }

            candidateListBox.SelectionChanged += _candidateListBox_SelectionChanged;

            CandidateScrollViewer.Content = candidateListBox;
            CandidatePanelGrid.Visibility = Visibility.Visible;

            MapPoint pt = _candidateList[0].Location;
            if (_firstZoom)
            {
                MyMap.ZoomToResolution(MyMap.Resolution / 4, pt);
                _firstZoom = false;
            }
            else
                MyMap.PanTo(pt);

            _lastIndex = 0;
            candidateListBox.SelectedIndex = 0;
        }

        void _candidateListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            int index = listBox.SelectedIndex;
            if (index >= 0 && _lastIndex != index)
            {
                _lastIndex = index;
                AddressCandidate candidate = _candidateList[index];
                MyMap.PanTo(candidate.Location);
            }
        }

        private void LocatorTask_Failed(object sender, TaskFailedEventArgs e)
        {
            MessageBox.Show("Locator service failed: " + e.Error);
        }
    }
}
