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
using System.Xml;
using System.Xml.Linq;
using System.Net;

namespace VideoRSSViewer
{
    public partial class VideoPanel : UserControl
    {
        public int NumberOfEntriesDisplayed = 3;
        public Double SpaceBetweenEntries = 10;

        public VideoPanel()
        {
            InitializeComponent();
            EnableMoving();
            //Load(new Uri("channel9screencast.xml", UriKind.RelativeOrAbsolute));
            //GetRandomVideos();
            UpdateLayoutInternal();
            scrollRightButton.MouseLeftButtonDown += new MouseButtonEventHandler(scrollRightButton_MouseLeftButtonDown);
            scrollRightButton.MouseLeftButtonUp += new MouseButtonEventHandler(scrollRightButton_MouseLeftButtonUp);
            scrollLeftButton.MouseLeftButtonDown += new MouseButtonEventHandler(scrollLeftButton_MouseLeftButtonDown);
            scrollLeftButton.MouseLeftButtonUp += new MouseButtonEventHandler(scrollLeftButton_MouseLeftButtonUp);
            refreshButton.MouseLeftButtonDown += new MouseButtonEventHandler(refreshButton_MouseLeftButtonDown);
            refreshButton.MouseLeftButtonUp += new MouseButtonEventHandler(refreshButton_MouseLeftButtonUp);
        }

        void refreshButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void refreshButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetRandomVideos();
            RefreshButtonStoryboard.Begin();
        }

        void scrollLeftButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void scrollRightButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void scrollRightButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LeftMostEntry = Math.Min(entryContainer.Children.Count - NumberOfEntriesDisplayed, LeftMostEntry + NumberOfEntriesDisplayed);
        }

        void scrollLeftButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LeftMostEntry = Math.Max(0, LeftMostEntry - NumberOfEntriesDisplayed);
        }


        private int _leftMostEntry = 0;
        private int LeftMostEntry
        {
            get
            {
                return _leftMostEntry;
            }

            set
            {
                _leftMostEntry = value;
                UpdateLayoutInternal();
            }
        }

        private void UpdateLayoutInternal()
        {
            this.Width = NumberOfEntriesDisplayed * (100 + SpaceBetweenEntries) + SpaceBetweenEntries;
            this.Height = 100 + 2*SpaceBetweenEntries;

            backgroundRectangle.Height = this.Height;
            backgroundRectangle.Width = this.Width;
            entryContainerClip.Clip = new RectangleGeometry();
            (entryContainerClip.Clip as RectangleGeometry).Rect = new Rect(0, 0, this.Width - 20, this.Height - 20);
            Canvas.SetLeft(scrollRightButton, this.Width - 11);
            Canvas.SetLeft(refreshButton, (this.Width - refreshButton.Width) / 2);
            Canvas.SetTop(refreshButton, (this.Height - refreshButton.Height/2));

            Double _currLeft = 0;
            foreach (VideoPanelEntry entry in entryContainer.Children)
            {
                entry.ScrollHorizontallyTo(_currLeft);
                _currLeft += entry.Width + SpaceBetweenEntries;
            }

            if (LeftMostEntry == 0)
            {
                HideScrollLeftButtonStoryboard.Begin();
                scrollLeftButton.IsHitTestVisible = false;
            }
            else
            {
                ShowScrollLeftButtonStoryboard.Begin();
                scrollLeftButton.IsHitTestVisible = true;
            }

            if (LeftMostEntry + NumberOfEntriesDisplayed < entryContainer.Children.Count)
            {
                ShowScrollRightButtonStoryboard.Begin();
                scrollRightButton.IsHitTestVisible = true;
            }
            else
            {
                HideScrollRightButtonStoryboard.Begin();
                scrollRightButton.IsHitTestVisible = false;
            }

            Double _desiredLeftPosition = -LeftMostEntry * (100 + SpaceBetweenEntries);
            if (Canvas.GetLeft(entryContainer) != _desiredLeftPosition)
            {
                HorizontalScrollKeyFrame.Value = _desiredLeftPosition;
                HorizontalScrollStoryboard.Begin();
            }

        }

        #region XML Loading

        bool firstTime = true;
        private void GetRandomVideos()
        {
            string url;
            if (firstTime)
            {   
                firstTime = false;
                url = "http://catalog.video.msn.com/search.aspx?mk=us&ff=99&c=10&q=funny&random=" + DateTime.Now.Millisecond.ToString();
            }
            else
                url = "http://catalog.video.msn.com/randomVideo.aspx?mk=us&vs=0&ff=99&c=10&random=" + DateTime.Now.Millisecond.ToString();

            WebClient wc = new WebClient();

            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.DownloadStringAsync(new Uri(url));
        }

        private XNamespace _NS = "urn:schemas-microsoft-com:msnvideo:catalog";

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            RefreshButtonStoryboard.Stop();
            XDocument doc = XDocument.Parse(e.Result);

            var vs = from video in doc.Descendants(_NS + "video")
                     select new VideoSource
                     {
                         UUID = (string)video.Element(_NS + "uuid"),
                         Title = (string)video.Element(_NS + "title"),
                         Source = (string)(video.Element(_NS + "source").Attribute("friendlyName")),
                         PublishDate = DateTime.Parse((string)video.Element(_NS + "startDate")).ToShortDateString(),
                         Description = (string)video.Element(_NS + "description"),
                         Tags = (from tag in video.Element(_NS + "tags").Descendants() select tag.Value).ToList<string>(),
                         ImageUrl = GetUriAsset(video.Element(_NS + "files"), "file", "2007"),
                         LargeImageUrl = GetUriAsset(video.Element(_NS + "files"), "file", "2009"),
                         VideoUrl = GetUriAsset(video.Element(_NS + "videoFiles"), "videoFile", "1002"),
                         ViewCount = (string)video.Element(_NS + "usage").Element(_NS + "usageItem").Attribute("totalCount"),
                         Related = GetRelatedLinks(video.Element(_NS + "extendedXml"))
                     };

            IEnumerable<VideoSource> results = vs;
            foreach (VideoSource source in results)
            {
                AddEntry(new VideoPanelEntry(source.Title, source.ImageUrl, source.VideoUrl));
            }
        }

            #region Helpers
        private string GetUriAsset(XElement element, string nodeName, string formatCode)
        {
            IEnumerable<string> uris = from file in element.Descendants(_NS + nodeName)
                                       where (string)file.Attribute("formatCode") == formatCode
                                       select (string)file.Element(_NS + "uri");
            return ((uris.Count<string>() > 0) ? uris.First<string>() : string.Empty);
        }

        private List<VideoSource.RelatedLinks> GetRelatedLinks(XElement element)
        {
            List<VideoSource.RelatedLinks> related = null;

            if (null != element)
            {
                var links = from link in element.Descendants(_NS + "link")
                            select new VideoSource.RelatedLinks
                            {
                                Name = link.Value,
                                Uri = (string)link.Attribute("uri")
                            };
                related = links.ToList<VideoSource.RelatedLinks>();
            }

            return related;
        }
        #endregion 

        #endregion

        private void AddEntry(VideoPanelEntry newEntry)
        {
            newEntry.DragStarted += new EventHandler(newEntry_DragStarted);
            Canvas.SetLeft(newEntry, Math.Max(this.Width, entryContainer.Children.Count * (100 + SpaceBetweenEntries)));
            entryContainer.Children.Add(newEntry);
            UpdateLayoutInternal();
        }

        void newEntry_DragStarted(object sender, EventArgs e)
        {
            VideoPanelEntry selectedEntry = sender as VideoPanelEntry;
            GeneralTransform transform = selectedEntry.TransformToVisual((Application.Current.RootVisual as Page).VideoArea);
            Point newOffset = transform.Transform(new Point(0, 0));
            entryContainer.Children.Remove(selectedEntry);

            selectedEntry.DragStarted -= newEntry_DragStarted;
            selectedEntry.DragMoved += new MouseEventHandler(selectedEntry_DragMoved);
            selectedEntry.Dropped += new MouseButtonEventHandler(selectedEntry_Dropped);

            (Application.Current.RootVisual as Page).VideoArea.Children.Add(selectedEntry);
            Canvas.SetLeft(selectedEntry, newOffset.X);
            Canvas.SetTop(selectedEntry, newOffset.Y);
            UpdateLayoutInternal();
        }

        void  selectedEntry_DragMoved(object sender, MouseEventArgs e)
        {
            VideoPanelEntry entry = sender as VideoPanelEntry;
            IEnumerable<UIElement> hitTestResult = VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition((Application.Current.RootVisual as Page).VideoArea), (Application.Current.RootVisual as Page).VideoArea);
            bool _foundVideoPanelEntry = false;
            foreach (UIElement hitElement in hitTestResult)
            {
                if ((!_foundVideoPanelEntry) && (hitElement.Equals(entry)))
                {
                    _foundVideoPanelEntry = true;
                }
                else
                {
                    if (hitElement.Equals(this))
                    {
                        entry.DragAndDropState = VideoPanelEntryDragAndDropState.NoDrop;
                        break;
                    }

                    if (hitElement is VideoPlayer)
                    {
                        entry.DragAndDropState = VideoPanelEntryDragAndDropState.Move;
                        break;
                    }

                    // if we get to the VideoArea Canvas, then nothing else was hit
                    if (hitElement.Equals((Application.Current.RootVisual as Page).VideoArea))
                    {
                        entry.DragAndDropState = VideoPanelEntryDragAndDropState.Add;
                        break;
                    }
                }
            }
        }

        public event EventHandler<VideoPanelEventArgs> EntryDropped;

        void selectedEntry_Dropped(object sender, MouseButtonEventArgs e)
        {
            VideoPanelEntry entry = sender as VideoPanelEntry;
            IEnumerable<UIElement> hitTestResult = VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition((Application.Current.RootVisual as Page).VideoArea), (Application.Current.RootVisual as Page).VideoArea);
            bool _foundVideoPanelEntry = false;
            foreach (UIElement hitElement in hitTestResult)
            {
                if ((!_foundVideoPanelEntry) && (hitElement.Equals(entry)))
                {
                    _foundVideoPanelEntry = true;
                }
                else
                {
                    if (hitElement.Equals(this))
                    {
                        break;
                    }

                    if (hitElement is VideoPlayer)
                    {
                        (hitElement as VideoPlayer).Source = entry.VideoUri;
                        break;
                    }

                    // if we get to the VideoArea Canvas, then nothing else was hit
                    if (hitElement.Equals((Application.Current.RootVisual as Page).VideoArea))
                    {
                        if (EntryDropped != null)
                            EntryDropped(this, new VideoPanelEventArgs(entry.VideoUri, e));

                        break;
                    }
                }
            }

            (Application.Current.RootVisual as Page).VideoArea.Children.Remove(entry);
        }


        #region Enable Moving Code

        void EnableMoving()
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(VideoPanelEntry_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(VideoPanelEntry_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(VideoPanelEntry_MouseLeftButtonUp);
        }

        bool _isMoving = false;
        Point _moveOrigin = new Point(0, 0);

        void VideoPanelEntry_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                this.CaptureMouse();
                this._isMoving = true;
                this._moveOrigin = e.GetPosition(this);
                this.Opacity = 0.7;
                Canvas.SetZIndex(this, 1);
            }
        }

        void VideoPanelEntry_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMoving)
            {
                Canvas.SetLeft(this, Canvas.GetLeft(this) + e.GetPosition(this).X - _moveOrigin.X);
                Canvas.SetTop(this, Canvas.GetTop(this) + e.GetPosition(this).Y - _moveOrigin.Y);
                _moveOrigin = e.GetPosition(this);
            }
        }

        void VideoPanelEntry_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this._isMoving = false;
            this.Opacity = 1;
            Canvas.SetZIndex(this, 0);
        }

        #endregion

    }

    // used to pass info from a drag-and-drop event from VideoPanel
    public class VideoPanelEventArgs : EventArgs
    {
        public VideoPanelEventArgs(String videoUri, MouseButtonEventArgs dropLocationMouseButtonEventArgs)
        {
            _videoUri = videoUri;
            _dropLocationMouseButtonEventArgs = dropLocationMouseButtonEventArgs;
        }

        private String _videoUri;
        public String VideoUri
        {
            get
            {
                return _videoUri;
            }
        }

        private MouseButtonEventArgs _dropLocationMouseButtonEventArgs;
        public MouseButtonEventArgs DropLocationMouseButtonEventArgs
        {
            get
            {
                return _dropLocationMouseButtonEventArgs;
            }
        }
    }

    public class VideoSource
    {
        #region Properties
        public string UUID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }      // 136x102
        public string LargeImageUrl { get; set; } // 400x300
        public string Source { get; set; }
        public string ViewCount { get; set; }
        public string PublishDate { get; set; }
        public List<string> Tags { get; set; }
        public List<RelatedLinks> Related { get; set; }
        public string ShortDescription { get { return ((Description.Length > 163) ? Description.Substring(0, 160) + "..." : Description); } }

        public class RelatedLinks
        {
            public string Uri { get; set; }
            public string Name { get; set; }
        }
        #endregion
    }
}
