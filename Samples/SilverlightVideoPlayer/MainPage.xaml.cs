using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VideoPlayer
{
    // For best full-screen video performance, set EnableGPUAcceleration="true" on the plug-in.
    // If hosting Silverlight from .aspx, add EnableGPUAcceleration="true" to the 
    // <asp:Silverlight> element.  If hosting Silverlight from .html, add
    // <param name="enableGPUAcceleration" value="true" /> under the <object> tag.

    public partial class MainPage : UserControl
    {
        // mediaElement.Position updates TimelineSlider.Value, and
        // updating TimelineSlider.Value updates mediaElement.Position, 
        // this variable helps us break the infinite loop
        private bool duringTickEvent = false;

        private bool playVideoWhenSliderDragIsOver = false;

        public MainPage()
        {
            InitializeComponent();

            mediaElement.Source = new Uri("file:///C:/Users/Jonathan_Moore/Desktop/Tasks/HDI_ITPro_Silverlight_vcsharp_Silverlight5_OS_Windows/NativeWindowDemo/Media/Russian science_.mp4");

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan duration = mediaElement.NaturalDuration.TimeSpan;
            totalTimeTextBlock.Text = TimeSpanToString(duration);
            UpdateVideoSize();
        }

        #region play button

        private void BigPlayButton_Click(object sender, RoutedEventArgs e)
        {
            playPauseButton.IsChecked = true;
            PlayPauseButton_Click(sender, e);
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            bigPlayButton.Visibility = Visibility.Collapsed;

            // this will be the toggle button state after the click has been processed
            if (playPauseButton.IsChecked == true)
                mediaElement.Play();
            else
                mediaElement.Pause();
        }

        #endregion

        #region timelineSlider

        private void Seek(double percentComplete)
        {
            if (duringTickEvent)
                throw new Exception("Can't call Seek() now, you'll get an infinite loop");

            TimeSpan duration = mediaElement.NaturalDuration.TimeSpan;
            int newPosition = (int)(duration.TotalSeconds * percentComplete);
            mediaElement.Position = new TimeSpan(0, 0, newPosition);

            // let the next CompositionTarget.Rendering take care of updating the text blocks
        }

        private Slider GetSliderParent(object sender)
        {
            FrameworkElement element = (FrameworkElement)sender;
            do
            {
                element = (FrameworkElement)VisualTreeHelper.GetParent(element);
            } while (!(element is Slider));
            return (Slider)element;
        }

        private void LeftTrack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            FrameworkElement lefttrack = (sender as FrameworkElement).FindName("LeftTrack") as FrameworkElement;
            FrameworkElement righttrack = (sender as FrameworkElement).FindName("RightTrack") as FrameworkElement;
            double position = e.GetPosition(lefttrack).X;
            double width = righttrack.TransformToVisual(lefttrack).Transform(new Point(righttrack.ActualWidth, righttrack.ActualHeight)).X;
            double percent = position / width;
            Slider slider = GetSliderParent(sender);
            slider.Value = percent;
        }

        private void HorizontalThumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (GetSliderParent(sender) != timelineSlider) return;

            bool notPlaying = (mediaElement.CurrentState == MediaElementState.Paused
                || mediaElement.CurrentState == MediaElementState.Stopped);

            if (notPlaying)
            {
                playVideoWhenSliderDragIsOver = false;
            }
            else
            {
                playVideoWhenSliderDragIsOver = true;
                mediaElement.Pause();
            }
        }

        private void HorizontalThumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (playVideoWhenSliderDragIsOver)
                mediaElement.Play();
        }

        private void TimelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (duringTickEvent)
                return;

            Seek(timelineSlider.Value);
        }

        #endregion

        #region updating current time

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            duringTickEvent = true;

            TimeSpan duration = mediaElement.NaturalDuration.TimeSpan;
            if (duration.TotalSeconds != 0)
            {
                double percentComplete = (mediaElement.Position.TotalSeconds / duration.TotalSeconds);
                timelineSlider.Value = percentComplete;
                string text = TimeSpanToString(mediaElement.Position);
                if (this.currentTimeTextBlock.Text != text)
                    this.currentTimeTextBlock.Text = text;
            }

            duringTickEvent = false;
        }

        private static string TimeSpanToString(TimeSpan time)
        {
            return string.Format("{0:00}:{1:00}", (time.Hours * 60) + time.Minutes, time.Seconds);
        }
        #endregion

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.IsMuted = (bool)muteButton.IsChecked;
        }

        #region fullscreen mode

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            var content = Application.Current.Host.Content;
            content.IsFullScreen = !content.IsFullScreen;
        }

        private void Content_FullScreenChanged(object sender, EventArgs e)
        {
            UpdateVideoSize();
        }

        private void UpdateVideoSize()
        {
            if (App.Current.Host.Content.IsFullScreen)
            {
                // mediaElement takes all available space
                VideoRow.Height = new GridLength(1, GridUnitType.Star);
                VideoColumn.Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                // mediaElement is only as big as the source video
                VideoRow.Height = new GridLength(1, GridUnitType.Auto);
                VideoColumn.Width = new GridLength(1, GridUnitType.Auto);
            }
        }

        #endregion
    }
}