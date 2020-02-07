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
    public partial class ProgressBar : UserControl
    {
        Double downloadProgress;
        Double downloadProgressOffset;
        Duration naturalDuration;
        TimeSpan position;

        public ProgressBar()
        {
            // Required to initialize variables
            InitializeComponent();
            downloadProgress = 0;
            downloadProgressOffset = 0;
            naturalDuration = new Duration(TimeSpan.FromSeconds(0));
            position = TimeSpan.FromSeconds(0);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(ProgressBar_MouseLeftButtonUp);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            sliderTrack.Width = finalSize.Width;
            this.UpdateLayoutInternal();
            return finalSize;
        }

        public Duration NaturalDuration
        {
            get
            {
                return this.naturalDuration;
            }

            set
            {
                this.naturalDuration = value;
                this.UpdateLayoutInternal();
            }

        }

        public TimeSpan Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
                this.UpdateLayoutInternal();
            }
        }

        public event TimeSpanEventHandler UserSeeked;

        void ProgressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (UserSeeked != null)
            {
                Double requestedPosition = e.GetPosition(sliderTrack).X;
                Double boundedPosition = Math.Min(Math.Max(0, requestedPosition), sliderTrack.Width);
                Double requestedMiliseconds = (boundedPosition / sliderTrack.Width) * NaturalDuration.TimeSpan.TotalMilliseconds;
                UserSeeked(this, new TimeSpanEventArgs(TimeSpan.FromMilliseconds(requestedMiliseconds)));
            }
        }

        private void UpdateLayoutInternal()
        {
            positionText.Text = this.position.ToString();
            if (positionText.Text.IndexOf('.') > 0)
                positionText.Text = positionText.Text.Substring(0, positionText.Text.IndexOf('.') + 2);
            if (this.NaturalDuration.TimeSpan.TotalSeconds != 0)
                Canvas.SetLeft(sliderThumb, (this.position.TotalSeconds / this.naturalDuration.TimeSpan.TotalSeconds) * sliderTrack.Width);
            else
                Canvas.SetLeft(sliderThumb, 0);
            Canvas.SetLeft(downloadProgressRectangle, this.downloadProgressOffset * sliderTrack.Width);
            downloadProgressRectangle.Width = (this.downloadProgress - this.downloadProgressOffset) * sliderTrack.Width;
        }

        public Double DownloadProgress
        {
            get
            {
                return this.downloadProgress;
            }

            set
            {
                this.downloadProgress = value;
                this.UpdateLayout();
            }
        }

        public Double DownloadProgressOffset
        {
            get
            {
                return this.downloadProgressOffset;
            }

            set
            {
                this.downloadProgressOffset = value;
                this.UpdateLayout();
            }
        }
    }

    public delegate void TimeSpanEventHandler(object sender, TimeSpanEventArgs e);

    public class TimeSpanEventArgs : EventArgs
    {
        private TimeSpan _timeSpan;

        public TimeSpanEventArgs(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
        }

        public TimeSpan TimeSpan
        {
            get
            {
                return _timeSpan;
            }
        }
    }

}
