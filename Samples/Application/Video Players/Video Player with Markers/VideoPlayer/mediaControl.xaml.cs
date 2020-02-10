using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.IO;
using System.Text;
using System.Windows.Markup;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Windows.Threading;
using System.Windows.Browser; 

namespace VideoPlayer
{
	public partial class mediaControl : UserControl
	{
        public delegate TimeSpan TimeLine(TimeSpan position);
        private DispatcherTimer _playTimer = new DispatcherTimer();
        private TimeSpan _duration;

        private Marker _activeMarker;

        private Dictionary<TimeSpan, Marker> _markers = new Dictionary<TimeSpan, Marker>();
        private Dictionary<TimeSpan, Marker> _cCs = new Dictionary<TimeSpan, Marker>();
        private List<MarkerData> _externalMarkerData = new List<MarkerData>();

        public delegate void EventHandler(Object sender, EventArgs e);
        public delegate void MarkerEventHandler(Object sender, TimelineMarker m);
        public delegate void SoundEventHandler(Object sender, bool isMuted);

        public event EventHandler PlayClicked;
        public event SoundEventHandler SoundChanged;
        public event EventHandler PauseClicked;
        public event EventHandler StopClicked;
        public event EventHandler FullScreenClicked;
        public event EventHandler BufferingStart;
        public event EventHandler BufferingEnd;
        public event EventHandler MediaCompleted;
        public event MarkerEventHandler MarkerReached;

        private MediaElement _media = null;
        private bool _buffering = false;
        private bool _timeRemaining = false;
        private double _lastVolume = 0;
        private bool _seekable = true;

        public void CenterPlay()
        {
            btnPlay.IsChecked = true;
            btnPlay_Checked(this, null);
        }

        public MediaElement Media
        {
            set
            {
                _media = value;
                if (_media != null)
                {
                    _media.MediaOpened += new RoutedEventHandler(_media_MediaOpened);
                    
                }
            }
        }
        private bool _EnableMarkers = true;
        public bool EnableMarkers
        {
            set
            {
                _EnableMarkers = value;
            }
            get
            {
                return _EnableMarkers;
            }
        }
        private bool _EnableCC = false;
        public bool EnableCC
        {
            set
            {
                _EnableCC = value;
            }
            get
            {
                return _EnableCC;
            }
        }
        private Uri _MarkerPath;
        public Uri MarkerPath
        {
            set
            {
                _MarkerPath = value;
            }
            get
            {
                return _MarkerPath;
            }
        }
        public double Volume
        {
            set
            {
                sliderVolume.Value = value;
            }
        }


        public mediaControl()
        {
            // Required to initialize variables
            InitializeComponent();

            spinner.Visibility = Visibility.Collapsed;

            this.btnPlay.Checked += new RoutedEventHandler(btnPlay_Checked);
            this.btnPlay.Unchecked += new RoutedEventHandler(btnPlay_Unchecked);

            this.btnSpeaker.Checked += new RoutedEventHandler(btnSpeaker_Checked);
            this.btnSpeaker.Unchecked += new RoutedEventHandler(btnSpeaker_Unchecked);

            this.btnFullScreen.Click += new RoutedEventHandler(btnFullScreen_Click);

            this.tbCurrentTime.MouseLeftButtonUp += new MouseButtonEventHandler(tbCurrentTime_MouseLeftButtonUp);

            this.sliderTimeline.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderTimeline_ValueChanged);
            this.sliderTimeline.LargeChange = .1;
            this.sliderTimeline.SmallChange = .05;

            this.sliderVolume.LargeChange = .125;
            this.sliderVolume.SmallChange = .125;

            this.sliderTimeline.Maximum = 1;

            this.sliderVolume.Value = .5;
            this.sliderVolume.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderVolume_ValueChanged);

            //attach to the sliders sizechanged event to replace the markers since there position is relative
            sliderTimeline.SizeChanged += new SizeChangedEventHandler(sliderTimeline_SizeChanged);

            //this will be used to update the time textblock and the slider
            _playTimer.Interval = TimeSpan.FromMilliseconds(50);
            _playTimer.Tick += new System.EventHandler(_playTimer_Tick);
            _lastVolume = sliderVolume.Value;
        }

        #region Handlers
        void sliderTimeline_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (KeyValuePair<TimeSpan, Marker> marker in _markers)
            {
                double sliderWidth = sliderTimeline.ActualWidth;
                double positionOfArrow = CalculateMarkerPosition(marker.Value.TimelineMarker, sliderWidth);
                marker.Value.Arrow.Margin = new Thickness((positionOfArrow / 2) + 2, 4.5, 0, 0);
                marker.Value.Margin = new Thickness((positionOfArrow / 2), 0, 0, 0);
                marker.Value.markerGrid.Margin = new Thickness(0, -75, 0, 0);
            }
        }
        void _media_MediaOpened(object sender, RoutedEventArgs e)
        {
            _duration = _media.NaturalDuration.TimeSpan;
            tbTotalTime.Text = string.Format("{0:00}:{1:00}", (_duration.Hours * 60) + _duration.Minutes, _duration.Seconds);

            _media.CurrentStateChanged += new RoutedEventHandler(_media_CurrentStateChanged);
            _media.DownloadProgressChanged += new RoutedEventHandler(_media_DownloadProgressChanged);
            _media.MediaEnded += new RoutedEventHandler(_media_MediaEnded);

            // need to check seek state
            _seekable = _media.CanSeek;

            if (!_seekable)
            {
                sliderTimeline.IsHitTestVisible = _seekable;
            }

            if (_media.AutoPlay)
            {
                _playTimer.Start();
                btnPlay.IsChecked = true;
            }

            if (_EnableMarkers)
            {
                if (_MarkerPath != null)
                {
                    // Create an HttpWebRequest object to the desired URI. 
                    WebRequest request = HttpWebRequest.Create(_MarkerPath);
                    // Start the asynchronous request.
                    request.BeginGetResponse(new AsyncCallback(this.OpenStreamCompleted), request);
                }
                else
                {
                    //using the collection of media.Markers insert our marker control to visualize the popup
                    PlaceMarkers();
                    //set a handler to control how the marker popup gets displayed
                    _media.MarkerReached += new TimelineMarkerRoutedEventHandler(_media_MarkerReached);
                } 
            }
        }

        void OpenStreamCompleted(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            WebResponse response = request.EndGetResponse(ar);

            // Read the response into a Stream object.
            Stream responseStream = response.GetResponseStream();

            // Create a new StreamReader instance using the specified Stream object.
            using (StreamReader streamreader = new StreamReader(responseStream))
            {
                XElement document = XElement.Load(streamreader); 
                _externalMarkerData = (from el in document.Elements() select GetMarkerData(el)).ToList();
            }
            Dispatcher.BeginInvoke(ProcessParsedMarkerCollection);
        }

        void marker_Clicked(object sender, TimelineMarker m)
        {
            double percentOfSeconds = m.Time.TotalSeconds / _duration.TotalSeconds;
            sliderTimeline.Value = percentOfSeconds;
            _media.Play();
            OnPlayClicked();
        }

        void _media_MarkerReached(object sender, TimelineMarkerRoutedEventArgs e)
        {
            OnMarkerReached(e.Marker);
            if (string.Compare(e.Marker.Type, "marker", StringComparison.InvariantCultureIgnoreCase) <= 0)
            {
                if (_activeMarker != null)
                {
                    _activeMarker.AnimateIn.Stop();
                }
                
                Marker marker;
                _markers.TryGetValue(e.Marker.Time, out marker);
                if (marker != null)
                {
                    _activeMarker = marker;
                    marker.AnimateIn.Begin();
                }
            }
        }
        
        void _media_DownloadProgressChanged(object sender, RoutedEventArgs e)
        {
            // get the scale transform
            Rectangle progress = sliderTimeline.ProgressBar;
            progress.RenderTransform.SetValue(ScaleTransform.ScaleXProperty, _media.DownloadProgress);
        }

        void _media_MediaEnded(object sender, RoutedEventArgs e)
        {
            _playTimer.Stop();
            btnPlay.IsChecked = false;
            tbCurrentTime.Text = "00:00";
            this.sliderTimeline.Value = 0;

            if (MediaCompleted != null)
            {
                MediaCompleted(this, null);
            }
        }

        void _media_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (_media.CurrentState == MediaElementState.Playing)
            {
                _playTimer.Start();
                spinner.Visibility = Visibility.Collapsed;
                if (BufferingEnd != null)
                {
                    BufferingEnd(sender, e);
                }
            }
            else if (_media.CurrentState == MediaElementState.Buffering)
            {
                if (spinner.Visibility == Visibility.Collapsed)
                {
                    spinner.Visibility = Visibility.Visible;
                    spinner.Animation.Begin();
                }
                if (BufferingStart != null)
                {
                    BufferingStart(sender, e);
                }
            }
            else
            {
                spinner.Visibility = Visibility.Collapsed;
                if (BufferingEnd != null)
                {
                    BufferingEnd(sender, e);
                }
            }
        }

        void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            OnFullScreenClick();
        }

        void tbCurrentTime_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetCurrentTime();
            if (_timeRemaining)
                _timeRemaining = false;
            else
                _timeRemaining = true;
        }

        void btnSpeaker_Unchecked(object sender, RoutedEventArgs e)
        {
            _media.IsMuted = true;
            _lastVolume = sliderVolume.Value;
            sliderVolume.Value = 0.0;
            OnSoundChanged();
        }

        void btnSpeaker_Checked(object sender, RoutedEventArgs e)
        {
            _media.IsMuted = false;
            sliderVolume.Value = _lastVolume;
            OnSoundChanged();
        }

        void _media_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            if (_buffering && _media.BufferingProgress == 1.0)
            {
                spinner.Visibility = Visibility.Collapsed;
                spinner.Animation.Stop();
                _buffering = false;
            }
            else if (!_buffering && _media.BufferingProgress > 0 && _media.BufferingProgress < 1.0)
            {
                spinner.Visibility = Visibility.Visible;
                spinner.Animation.Begin();
                _buffering = true;
            }

        }

        void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
             _media.Volume = e.NewValue;
        }

        void sliderTimeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double tValue = (_media.Position.TotalSeconds/_duration.TotalSeconds);
            if (e.NewValue != tValue)
            {
                _media.Position = TimeSpan.FromSeconds(_duration.TotalSeconds * e.NewValue);
            }
        }

        void _playTimer_Tick(object sender, EventArgs e)
        {
            SetCurrentTime();
            if (_duration.TotalSeconds > 0)
                this.sliderTimeline.Value = (_media.Position.TotalSeconds / _duration.TotalSeconds);
        }

        void btnPlay_Checked(object sender, RoutedEventArgs e)
        {
            _media.Play();
            OnPlayClicked();
        }
        void btnPlay_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_media.CanPause)
            {
                _media.Pause();
                OnPauseClicked();
            }
            else
            {
                _media.Stop();
            }
        }
        #endregion

        #region Event helpers
        private void OnSoundChanged()
        {
            if (SoundChanged != null)
            {
                SoundChanged(this,_media.IsMuted);
            }
        }
        private void OnFullScreenClick()
        {
            if (FullScreenClicked != null)
            {
                FullScreenClicked(this, null);
            }
        }
        private void OnBufferingStart()
        {
            if (BufferingStart != null)
            {
                BufferingStart(this, null);
            }
        }
        private void OnBufferingEnd()
        {
            if (BufferingEnd != null)
            {
                BufferingEnd(this, null);
            }
        }

        private void OnMediaCompleted()
        {
            if (MediaCompleted != null)
            {
                MediaCompleted(this, null);
            }
        }

        private void OnStopClicked()
        {
            if (StopClicked != null)
            {
                StopClicked(this, null);
            }
        }
       
        private void OnPlayClicked()
        {
            if (PlayClicked != null)
            {
                PlayClicked(this, null);
            }
        }
        
        private void OnPauseClicked()
        {
            if (PauseClicked != null)
            {
                PauseClicked(this, null);
            }
        }
        private void OnMarkerReached(TimelineMarker marker)
        {
            if (MarkerReached != null)
            {
                MarkerReached(this, marker);
            }
        }
        #endregion

        #region Private methods
        public void ProcessParsedMarkerCollection()
        {
            if (_externalMarkerData.Count > 0)
            {
                foreach (MarkerData data in _externalMarkerData)
                {
                    if (data.Enabled)
                    {
                        TimelineMarker timeLineMarker1 = new TimelineMarker();
                        timeLineMarker1.Text = data.Text;
                        timeLineMarker1.Time = data.Time;
                        //NOTE: in SL2 Beta 2 if type is not set the app crashed
                        timeLineMarker1.Type = data.Type;
                        _media.Markers.Add(timeLineMarker1);
                    }
                }
                PlaceMarkers();
                //set a handler to control how the marker popup gets displayed
                _media.MarkerReached += new TimelineMarkerRoutedEventHandler(_media_MarkerReached);

            }
        }
        private MarkerData GetMarkerData(XElement el)
        {
            MarkerData m = new MarkerData();
            string time = el.Attribute("Time").Value;
            TimeSpan tsTime = TimeSpan.Parse(time);
            m.Time = tsTime;
            m.Type = el.Attribute("Type").Value;
            m.Enabled = Convert.ToBoolean(el.Attribute("Enabled").Value);
            XAttribute xaValue = el.Attribute("Value");
            if (xaValue != null)
                m.Text = xaValue.Value.ToString();
            else
                m.Text = el.Value;
            return m;
        }
        private void SetCurrentTime()
        {
            if (!_timeRemaining)
            {
                tbCurrentTime.Text = string.Format("{0:00}:{1:00}", (_media.Position.Hours * 60) + _media.Position.Minutes, _media.Position.Seconds);
            }
            else
            {
                TimeSpan remaining = _duration.Subtract(_media.Position);
                tbCurrentTime.Text = string.Format("{0:00}:{1:00}", (remaining.Hours * 60) + remaining.Minutes, remaining.Seconds);
            }
        }

        private void PlaceMarkers()
        {
            foreach (TimelineMarker timeLineMarker1 in _media.Markers)
            {
                if (string.Compare(timeLineMarker1.Type, "marker", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    //place a marker in relation to timeline
                    Marker marker = new Marker();
                    marker.TimelineMarker = timeLineMarker1;
                    marker.tbMarker.Text = timeLineMarker1.Text;
                    marker.HorizontalAlignment = HorizontalAlignment.Left;

                    marker.Clicked += new Marker.MarkerEventHandler(marker_Clicked);
                    //set width based on percentage of text

                    marker.VerticalAlignment = VerticalAlignment.Top;
                    //marker.SetValue(Grid.ColumnProperty, 0);

                    double positionOfArrow = CalculateMarkerPosition(timeLineMarker1, sliderTimeline.ActualWidth);

                    marker.Arrow.Margin = new Thickness((positionOfArrow / 2) + 2, 4.5, 0, 0);
                    marker.Margin = new Thickness((positionOfArrow / 2), 0, 0, 0);
                    marker.markerGrid.Margin = new Thickness(0, -75, 0, 0);

                    gridCol2.Children.Add(marker);

                    _markers.Add(timeLineMarker1.Time, marker);
                }
            }
        }
        private double CalculateMarkerPosition(TimelineMarker timeLineMarker1, double sliderWidth)
        {
            //adjust the placement of the arrow
            double percentOfSeconds = timeLineMarker1.Time.TotalSeconds / _duration.TotalSeconds;
            double positionOfArrow = Math.Ceiling(sliderWidth * percentOfSeconds);
            return positionOfArrow;
        }
        #endregion

        #region Public Methods
        public void Seek(TimeSpan time)
        {
            if (time.TotalSeconds < _duration.TotalSeconds)
            {
                double percentOfSeconds = time.TotalSeconds / _duration.TotalSeconds;
                sliderTimeline.Value = percentOfSeconds;
                
                _media.Play();
                OnPlayClicked();
            }
        }
        #endregion
    }
    internal class MarkerData
    {
        public TimeSpan Time { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public bool Enabled { get; set; }
    }
}