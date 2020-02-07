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
    public partial class VideoPanelEntry : UserControl
    {
        private bool _isLoaded = false;
        public VideoPanelEntry(String title, String imageUri, String videoUri)
        {
            InitializeComponent();
            EnableMoving();
            Title = title;
            ImageUri = imageUri;
            VideoUri = videoUri;

            entryImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imageUri, UriKind.RelativeOrAbsolute));
            titleText.Text = title;
            titleTextShadow.Text = title;

            _isLoaded = true;
            Storyboard.SetTarget(HorizontalScrollStoryboard, this);
            Storyboard.SetTarget(VerticalScrollStoryboard, this);
        }

        void entryImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //entryImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("http://zemrashqiptare.net/images/articles/2007_04/283/u1_cameron-diaz-big.jpg"));
        }

        public String Title;
        public String ImageUri;
        public String VideoUri;

        public event EventHandler DragStarted;
        public event MouseEventHandler DragMoved;
        public event MouseButtonEventHandler Dropped;

        private VideoPanelEntryDragAndDropState _dragAndDropState = VideoPanelEntryDragAndDropState.NoDrop;
        public VideoPanelEntryDragAndDropState DragAndDropState
        {
            get
            {
                return _dragAndDropState;
            }

            set
            {
                _dragAndDropState = value;
                if (_dragAndDropState == VideoPanelEntryDragAndDropState.NoDrop)
                {
                    HideDragAndDropButtonStoryboard.Begin();
                }
                else
                {
                    ShowDragAndDropButtonStoryboard.Begin();
                    if (_dragAndDropState == VideoPanelEntryDragAndDropState.Add)
                    {
                        AddDragAndDropButtonFaceStoryboard.Begin();
                    }
                    else
                    {
                        MoveDragAndDropButtonFaceStoryboard.Begin();
                    }
                }
            }
        }

        public void ScrollHorizontallyTo(Double newPosition)
        {
            if (Canvas.GetLeft(this) == newPosition)
                return;

            if ((_isLoaded))
            {
                HorizontalScrollStoryboard.Stop();
                HorizontalScrollKeyFrame.Value = newPosition;
                HorizontalScrollStoryboard.Begin();
            }
            else
            {
                Canvas.SetLeft(this, newPosition);
            }
        }
        public void ScrollVerticallyTo(Double newPosition)
        {
            if (Canvas.GetTop(this) == newPosition)
                return;

            if (_isLoaded)
            {
                //this.HorizontalScrollStoryboard.stop();
                VerticalScrollKeyFrame.Value = newPosition;
                VerticalScrollStoryboard.Begin();
            }
            else
            {
                Canvas.SetLeft(this, newPosition);
            }
        }

        #region Enable Moving Code

        void EnableMoving()
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(VideoPanelEntry_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(VideoPanelEntry_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(VideoPanelEntry_MouseLeftButtonUp);
            this.MouseEnter += new MouseEventHandler(VideoPanelEntry_MouseEnter);
            this.MouseLeave += new MouseEventHandler(VideoPanelEntry_MouseLeave);
        }

        void VideoPanelEntry_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseLeaveStoryboard.Begin();
        }

        void VideoPanelEntry_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnterStoryboard.Begin();
        }

        bool _isMoving = false;
        Point _moveOrigin = new Point(0, 0);

        void VideoPanelEntry_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.CaptureMouse();
            this._isMoving = true;
            this._moveOrigin = e.GetPosition(null);
            e.Handled = true;
            if (DragStarted != null)
                DragStarted(this, new EventArgs());
            MouseDownStoryboard.Begin();
        }

        void VideoPanelEntry_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMoving)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.GetPosition(null).X - _moveOrigin.X);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.GetPosition(null).Y - _moveOrigin.Y);
                _moveOrigin = e.GetPosition(null);
                if (DragMoved != null)
                    DragMoved(this, e);
            }
        }

        void VideoPanelEntry_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseUpStoryboard.Begin();
            this.ReleaseMouseCapture();
            if (_isMoving)
            {
                this._isMoving = false;
                if (Dropped != null)
                    Dropped(this, e);
            }
        }

        #endregion

    }

    public enum VideoPanelEntryDragAndDropState
    {
        NoDrop,
        Add,
        Move
    }
}
