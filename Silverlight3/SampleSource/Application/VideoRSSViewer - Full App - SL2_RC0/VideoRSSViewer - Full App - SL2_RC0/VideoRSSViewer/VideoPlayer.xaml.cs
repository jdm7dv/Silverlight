using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VideoRSSViewer
{
    public partial class VideoPlayer : UserControl
    {
        public VideoPlayer()
        {
            InitializeComponent();

            myPlayPauseButton.Clicked += new MouseEventHandler(myPlayPauseButton_Clicked);
            myME.CurrentStateChanged += new RoutedEventHandler(myME_CurrentStateChanged);

            // call initialization methods for each component
            InitializeProgressBar();
            InitializeFullScreenButton();
            InitializeWindowFrame();
            EnableMoving();
        }

        public VideoPlayer(String source): this()
        {
            Source = source;
        }

        public String Source
        {
            get
            {
                return myME.Source.ToString();
            }

            set
            {
                myME.Source = new Uri(value, UriKind.RelativeOrAbsolute);
                bufferingEllipseAnimation.Begin();
            }
        }

        #region PlayPauseButton Code
        void myME_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (myME.CurrentState == MediaElementState.Playing)
                myPlayPauseButton.State = PlayPauseButtonState.Pause;
            else
                myPlayPauseButton.State = PlayPauseButtonState.Play;

            if ((myME.CurrentState == MediaElementState.Buffering) || (myME.CurrentState == MediaElementState.Opening))
            {
                bufferingEllipse.Visibility = Visibility.Visible;
            }
            else
            {
                bufferingEllipse.Visibility = Visibility.Collapsed;
            }
        }

        void myPlayPauseButton_Clicked(object sender, MouseEventArgs e)
        {
            if (myME.CurrentState == MediaElementState.Playing)
                myME.Pause();
            else
                myME.Play();
        }

        #endregion

        #region ProgressBar Code
        // hooks all event handlers related to progress bar.
        void InitializeProgressBar()
        {
            myME.MediaOpened += new RoutedEventHandler(myME_MediaOpened);
            myME.DownloadProgressChanged += new RoutedEventHandler(myME_DownloadProgressChanged);
            myProgressBar.UserSeeked += new TimeSpanEventHandler(myProgressBar_UserSeeked);
        }

        Storyboard timer = null;
        void myME_MediaOpened(object sender, RoutedEventArgs e)
        {
            myProgressBar.NaturalDuration = myME.NaturalDuration;

            // initialize timer if it's not set
            if (timer == null)
            {
                timer = new Storyboard();
                timer.Duration = TimeSpan.FromMilliseconds(20);
                timer.Completed += new EventHandler(timer_Completed);
                this.Resources.Add("myTimer", timer);
            }
            timer.Begin();
        }

        // timer tick event
        void timer_Completed(object sender, EventArgs e)
        {
            myProgressBar.Position = myME.Position;
            timer.Begin();
        }

        // Called whenever more is downloaded from MediaElement
        void myME_DownloadProgressChanged(object sender, RoutedEventArgs e)
        {
            myProgressBar.DownloadProgress = myME.DownloadProgress;
            myProgressBar.DownloadProgressOffset = myME.DownloadProgressOffset;
        }

        // handles seek event from Progress Bar
        void myProgressBar_UserSeeked(object sender, TimeSpanEventArgs e)
        {
            myME.Position = e.TimeSpan;
        }
        #endregion

        #region FullScreenButton Code

        // hook up appropriate events
        void InitializeFullScreenButton()
        {
            myFullScreenButton.Clicked += new MouseEventHandler(myFullScreenButton_Clicked);
        }

        void myFullScreenButton_Clicked(object sender, MouseEventArgs e)
        {
            System.Windows.Interop.SilverlightHost host = Application.Current.Host;
            host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
            host.Content.IsFullScreen = !host.Content.IsFullScreen;
        }

        void Content_FullScreenChanged(object sender, EventArgs e)
        {
            System.Windows.Interop.SilverlightHost host = Application.Current.Host;
            if (host.Content.IsFullScreen)
            {
                (Application.Current.RootVisual as Page).FullScreenRectangle.Height = host.Content.ActualHeight;
                (Application.Current.RootVisual as Page).FullScreenRectangle.Width = host.Content.ActualWidth;
                VideoBrush vb = new VideoBrush();
                vb.SetSource(myME);
                vb.Stretch = Stretch.Uniform;
                (Application.Current.RootVisual as Page).FullScreenRectangle.Fill = vb;
                (Application.Current.RootVisual as Page).FullScreenRectangleBackground.Visibility = Visibility.Visible;
            }
            else
            {
                (Application.Current.RootVisual as Page).FullScreenRectangle.Fill = null;
                (Application.Current.RootVisual as Page).FullScreenRectangleBackground.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region WindowFrame Code
        void InitializeWindowFrame()
        {
            myWindowFrame.Resized += new EventHandler<MouseEventArgs>(myWindowFrame_Resized);
            myWindowFrame.Closed += new EventHandler(myWindowFrame_Closed);
        }

        void myWindowFrame_Closed(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                (this.Parent as Panel).Children.Remove(this);
            }
        }

        void myWindowFrame_Resized(object sender, MouseEventArgs e)
        {
            Double NewWidth = e.GetPosition(this).X + myWindowFrame.ResizeRectangle.ActualWidth / 2;
            Double NewHeight = e.GetPosition(this).Y + myWindowFrame.ResizeRectangle.ActualHeight / 2;

            if (NewWidth >= 250)
                this.Width = NewWidth;
            else
                this.Width = 250;

            if (NewHeight >= 250)
                this.Height = NewHeight;
            else
                this.Height = 250;

        }
        #endregion

        #region Enable Moving Code

        void EnableMoving()
        {
            myME.MouseLeftButtonDown += new MouseButtonEventHandler(VideoPlayer_MouseLeftButtonDown);
            myME.MouseMove += new MouseEventHandler(VideoPlayer_MouseMove);
            myME.MouseLeftButtonUp += new MouseButtonEventHandler(VideoPlayer_MouseLeftButtonUp);
        
            backgroundRectangle.MouseLeftButtonDown += new MouseButtonEventHandler(VideoPlayer_MouseLeftButtonDown);
            backgroundRectangle.MouseMove += new MouseEventHandler(VideoPlayer_MouseMove);
            backgroundRectangle.MouseLeftButtonUp += new MouseButtonEventHandler(VideoPlayer_MouseLeftButtonUp);
        }

        bool _isMoving = false;
        Point _moveOrigin = new Point(0, 0);

        void VideoPlayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as UIElement).CaptureMouse();
            this._isMoving = true;
            this._moveOrigin = e.GetPosition(this);
            this.Opacity = 0.5;
        }

        void VideoPlayer_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMoving)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.GetPosition(this).X - _moveOrigin.X);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.GetPosition(this).Y - _moveOrigin.Y);
                _moveOrigin = e.GetPosition(this);
            }
        }

        void VideoPlayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (sender as UIElement).ReleaseMouseCapture();
            this._isMoving = false;
            this.Opacity = 1;
        }

        #endregion

    }
}
