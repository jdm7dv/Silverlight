using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VideoPlayer
{
	public partial class Marker : UserControl
	{
        private TimelineMarker _timelineMarker;
        public TimelineMarker TimelineMarker
        {
            get { return _timelineMarker; }
            set { _timelineMarker = value; }
        }
        public delegate void MarkerEventHandler(Object sender, TimelineMarker m);
        public event MarkerEventHandler Clicked;

		public Marker()
		{
			// Required to initialize variables
			InitializeComponent();
            this.Loaded += new RoutedEventHandler(Marker_Loaded);
		}

        void Marker_Loaded(object sender, RoutedEventArgs e)
        {
            Arrow.MouseLeftButtonUp += new MouseButtonEventHandler(Arrow_MouseLeftButtonUp);    
        }

        void Arrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnMarkerClicked();
        }

        private void OnMarkerClicked()
        {
            if (Clicked != null)
            {
                Clicked(this, _timelineMarker);
            }
        }
	}
}