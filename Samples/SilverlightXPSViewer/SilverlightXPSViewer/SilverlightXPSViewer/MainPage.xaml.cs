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
using System.IO;
using System.Windows.Resources;
using System.Xml;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Reflection.Emit;

namespace SilverlightXPSViewer
{
    public partial class MainPage : UserControl
    {
        #region Class Level Variables
        internal delegate void Action(UIElement visual , ref int i);

        protected const double MinimumScale = 0.2;
        protected const double MaximumScale = 3.0;

        private WebClient _webClient;
        protected List<string> _pageNames = new List<string>();
        protected List<string> _imageKey = new List<string>();
        protected Dictionary<string, Stream> _imageSources = new Dictionary<string, Stream>();
        protected Dictionary<string, Stream> _OrignalimageSources = new Dictionary<string, Stream>();
        protected List<string> _viewboxOfImages = new List<string>();
        protected List<string> _viewPortOfImages = new List<string>();
        
        StreamResourceInfo _streamResourceInfo;
        protected int _pageNumber;
        protected double _pageScale;
        protected Point? _lastMousePosition;
        private String Source;
        private double width;
        private int zIndex;
        private Canvas _mainCanvas;
       

        public bool ZoominFeature
        {
            set { SetZoominFeature(value); }

        }
        public bool IsGroupView
        {
            set { SetGroupView(value); }

        }
        public double ControlWidth
        {
            set { this.Width = value; }
            get { return this.Width; }
        }
        public double ControlHeight
        {
            set { this.Height = value; }
            get { return this.Height; }
        }
        #endregion

        public MainPage()
        {
            InitializeComponent();

            // Hook up event handlers
            page.MouseLeftButtonDown += new MouseButtonEventHandler(Page_MouseLeftButtonDown);
            page.MouseMove += new MouseEventHandler(Page_MouseMove);
            page.MouseLeftButtonUp += new MouseButtonEventHandler(Page_MouseLeftButtonUp);
            page.MouseLeave += new MouseEventHandler(Page_MouseLeftButtonUp);

            zoomOut.MouseLeftButtonDown += new MouseButtonEventHandler(zoom_MouseLeftButtonDown);
            zoomIn.MouseLeftButtonDown += new MouseButtonEventHandler(zoom_MouseLeftButtonDown);

            Loaded += delegate
            {
                ForAllChildren(controls, CenterTextBlocks);
            };

            if (Application.Current.IsRunningOutOfBrowser)
            {
                Application.Current.CheckAndDownloadUpdateCompleted += new CheckAndDownloadUpdateCompletedEventHandler(Current_CheckAndDownloadUpdateCompleted);
                Application.Current.CheckAndDownloadUpdateAsync();
            }

        }

        void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if(e.UpdateAvailable)
                MessageBox.Show("Please Close This Application, Application has been Updated.");
        }

        #region Functions
  
        public void SetGroupView(Boolean IsGroupView)
        {
            if (IsGroupView == true) pageControls.Visibility = Visibility.Collapsed;
            else pageControls.Visibility = Visibility.Visible;

        }
        public void SetZoominFeature(Boolean Required)
        {
            if (Required == true) zoomControls.Visibility = Visibility.Collapsed;
            else zoomControls.Visibility = Visibility.Visible;
        }
        private void ShowXPSDoc(string xpsDoc)
        {
            // Download Font Assembly
            string xapName = xpsDoc.Replace(".xps", ".xap");
            //ShowStatusMessage(string.Format("Downloading {0}...", xapName));
            _webClient.OpenReadAsync(new Uri(xapName, UriKind.RelativeOrAbsolute), xpsDoc);
        }
        public double ScrollViewerWidth
        {
            set
            {
                xps_viewer.Width = value;
                //pageparent.Width = xps_viewer.Width ;
                //page.Width = xps_viewer.Width;
            }
            get { return xps_viewer.Width; }
        }
        public double ScrollViewerHeight
        {
            set
            {
                xps_viewer.Height = value;
                //pageparent.Height = xps_viewer.Height;
                //page.Height = xps_viewer.Height;
            }
            get { return xps_viewer.Height; }
        }

        public void ResizeDcoument()
        {
            Application_Current_Host_Content_Resized(null, null);
        }
        public void LoadDocsByUrl(string Source)
        {
            if (!Uri.IsWellFormedUriString(Source, UriKind.Absolute))
            {
                throw new UriFormatException("Invaild Format");
            }
            else
            {
                this.Source = Source;
                string basePath = string.Empty;
                ShowProgress("Loading...");
                pageContents.Children.Clear();

                basePath = Source;
                //basePath = Source.Substring(0, Source.LastIndexOf("/")+1);  // .LastIndexOf("/")   .Replace(_sourceFile,"");1

                CreateWebClientInstance();
                _webClient.OpenReadAsync(new Uri(Source));
            }
        }

        internal void ReadPackagePageContent()
        {
            using (var responseP = Application.GetResourceStream(_streamResourceInfo, ConvertPartName("/_rels/.rels")).Stream)
            {
                var FixedDocSeqPath = string.Empty;
                using (var readerP = XmlReader.Create(responseP))
                {
                    readerP.ReadToDescendant("Relationship");
                    do
                    {
                        readerP.MoveToAttribute("Target");
                        if(readerP.Value.Contains(".fdseq"))
                            FixedDocSeqPath = readerP.ReadContentAsString();
                    } while (readerP.ReadToNextSibling("Relationship"));
                }

                using (var responseF = Application.GetResourceStream(_streamResourceInfo, ConvertPartName(FixedDocSeqPath)).Stream)
                {
                    using (var readerF = XmlReader.Create(responseF))
                    {
                        var FixedDocPath = string.Empty;
                        readerF.ReadToDescendant("DocumentReference");
                        do
                        {
                            readerF.MoveToAttribute("Source");
                            if (readerF.Value.Contains(".fdoc"))
                                FixedDocPath = readerF.ReadContentAsString();
                        } while (readerF.ReadToNextSibling("DocumentReference"));

                        using (var response = Application.GetResourceStream(_streamResourceInfo, ConvertPartName(FixedDocPath)).Stream)
                        {
                            using (var reader = XmlReader.Create(response))
                            {
                                reader.ReadToDescendant("PageContent");
                                do
                                {
                                    reader.MoveToAttribute("Source");

                                    if(reader.Value.Contains(System.IO.Path.GetDirectoryName(FixedDocPath)))
                                        _pageNames.Add(reader.ReadContentAsString());
                                    else
                                        _pageNames.Add(System.IO.Path.Combine(FixedDocPath.Replace(System.IO.Path.GetFileName(FixedDocPath),""), reader.ReadContentAsString()));

                                } while (reader.ReadToNextSibling("PageContent"));
                            }
                        }
                    }
                }
            }
        }
        
        internal void DisplayCurrentPage()
        {
            if (_streamResourceInfo != null)
            {
                // Clear the current page
                pageContents.Children.Clear();
                _imageSources.Clear();
                _viewboxOfImages.Clear();
                _viewPortOfImages.Clear();
                _OrignalimageSources.Clear();
                _imageKey.Clear();
                zIndex = 0;

                zoomControls.Visibility = Visibility.Visible;
                controls.Visibility = Visibility.Visible;
                pageContents.Visibility = Visibility.Visible;
                zoomIn.Visibility = Visibility.Visible;
                zoomOut.Visibility = Visibility.Visible;

                // Parse the current page file
                using (var response = Application.GetResourceStream(_streamResourceInfo, ConvertPartName(_pageNames[_pageNumber])).Stream)
                {
                    using (var reader = XmlReader.Create(response))
                    {
                        reader.ReadToDescendant("FixedPage");
                        // Get the Silverlight-compatible XAML for the page
                        string translatedXaml;
                        using (var translatingReader = new XpsToSilverlightXamlReader(reader, Source, _imageSources, _viewboxOfImages, _viewPortOfImages, _streamResourceInfo))
                        {
                            translatedXaml = translatingReader.ReadOuterXml();
                        }
                        
                        _imageSources.ToList().ForEach(f => _OrignalimageSources.Add(f.Key, f.Value));
                        _imageSources.ToList().ForEach(f => _imageKey.Add(f.Key));

                        // Prepare a border for the XAML
                        translatedXaml = translatedXaml.Replace("xmlns=\"http://schemas.microsoft.com/xps/2005/06\"", "xmlns=\"http://schemas.microsoft.com/client/2007\"");

                        _mainCanvas = null;
                        _mainCanvas = XamlReader.Load(translatedXaml) as Canvas;
                        _mainCanvas.SetValue(Canvas.LeftProperty, 0.0);
                        _mainCanvas.SetValue(Canvas.TopProperty, 0.0);

                        pageContents.Width = _mainCanvas.Width;
                        pageContents.Height = _mainCanvas.Height;

                        ForAllChildren(_mainCanvas, SetTagOfPath);
                        // Set ImageBrush/ImageSource
                        ForAllChildren(_mainCanvas, SetImageBrushSource);
                        // Load the page XAML into the wrapper
                        pageContents.Children.Add(_mainCanvas);

                        // Update the page number display in the controls UI
                        pageInfo.Text = string.Format("{0}/{1}", _pageNumber + 1  , _pageNames.Count);

                        pageInfo.SetValue(Canvas.LeftProperty, 0.0);
                        int i = 0;
                        CenterTextBlocks(pageInfo, ref i);
                        pagePrevious.Opacity = (0 < _pageNumber + 1) ? 1 : 0.5;
                        pageNext.Opacity = (_pageNumber < _pageNames.Count - 1) ? 1 : 0.5;

                        // Simulate a resize to display the page in full-page view
                        Application_Current_Host_Content_Resized(_mainCanvas, null);
                    }
                }
            }
            else
            {
                throw new Exception("StreamResourceInfo cannot be null");
            }
        }
      
        protected void UpdateZoomUI(double scale)
        {
            // Update the zoom portion of the Controls UI
            _pageScale = scale;
            scaler.ScaleX = _pageScale;
            scaler.ScaleY = _pageScale;

            //scaler.CenterX = xps_viewer.Width / 2;
            //scaler.CenterY = xps_viewer.Height / 2;

            page.Width = (_pageScale * pageContents.ActualWidth);
            page.Height = (_pageScale * pageContents.ActualHeight);

            if (page.Width <= xps_viewer.Width && page.Height <= xps_viewer.Height)
            {
                xps_viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                xps_viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
            else
            {
                xps_viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                xps_viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }

            zoomIndicator.Width = Math.Min(Math.Max(((_pageScale - MinimumScale) / (MaximumScale - MinimumScale)), 0), 1) * zoomBar.Width;
            zoomOut.Opacity = (MinimumScale < _pageScale) ? 1 : 0.5;
            zoomIn.Opacity = (_pageScale < MaximumScale) ? 1 : 0.5;
        }
        private void ShowProgress(String progress)
        {
            ShowStatusMessage(progress);
            statusControls.Visibility = Visibility.Visible;
        }
        private void HideProgress()
        {
            ShowStatusMessage(string.Empty);
        }
        internal void ForAllChildren(Panel panel, Action action)
        {
            // Helper function to run a method on all children of an element
            for (int i = 0; i < panel.Children.Count; i++)
            {
                var child = panel.Children[i];
                action(child , ref i);
                var childAsPanel = child as Panel;
                if (null != childAsPanel)
                {
                  ForAllChildren(childAsPanel, action);
                }
            }
        }
        internal void SetTagOfPath(UIElement uiElement, ref int i)
        {
            if (uiElement is System.Windows.Shapes.Path && (uiElement as System.Windows.Shapes.Path).Fill is ImageBrush)
            {
                uiElement.SetValue(Canvas.TagProperty, _imageKey[0]);
                _imageKey.RemoveAt(0);
            }
        }
        internal void SetImageBrushSource(UIElement uiElement , ref int i)
        {
            // Set the ImageBrush/ImageSource to the next value in the list (skipping unsupported TIFF images)
            var path = uiElement as System.Windows.Shapes.Path;
            if (null != path)
            {
                var brush = path.Fill as ImageBrush;
                if (null != brush)
                {
                    brush.Transform = null;
                    if (0 < _imageSources.Count)
                    {
                        var source = Convert.ToString(path.GetValue(Canvas.TagProperty));
                        var count = Convert.ToInt32(source.Split('%')[1]);
                        var tempviewbox = _viewboxOfImages[count].Split(',');
                        var tempviewPort = _viewPortOfImages[count].Split(',');
                        var Iserror = false;
                        if (".TIF" != System.IO.Path.GetExtension(source).ToUpper())
                        {
                            var stream = _OrignalimageSources[Convert.ToString(path.GetValue(Canvas.TagProperty))];
                            var bitmapImage = new BitmapImage();
                            try
                            {
                                bitmapImage.SetSource(stream);
                                brush.ImageSource = bitmapImage;
                               
                            }catch
                            {
                                Iserror = true;
                            }

                            if (!Iserror)
                            {
                                var viewbox = new Rect(Convert.ToDouble(tempviewbox[0]), Convert.ToDouble(tempviewbox[1]),
                                  Convert.ToDouble(tempviewbox[2]), Convert.ToDouble(tempviewbox[3]));
                                var viewport = new Rect(Convert.ToDouble(tempviewPort[0]), Convert.ToDouble(tempviewPort[1]),
                                   Convert.ToDouble(tempviewPort[2]), Convert.ToDouble(tempviewPort[3]));

                                if ((viewbox.Width != bitmapImage.PixelWidth || viewbox.Height != bitmapImage.PixelHeight))
                                {
                                    path.Visibility = Visibility.Collapsed;

                                    var obj = _imageSources.Where(w => w.Value != null && w.Value.Length == stream.Length && w.Key != source).ToList();
                                    var orignalimages = _OrignalimageSources.Where(w => w.Value != null && w.Value.Length == stream.Length && w.Key != source).ToList();

                                    if (obj.Count >= 0 && orignalimages.Count > 0)
                                    {
                                        if (obj.Count == 0)
                                        {
                                            var vp = _viewPortOfImages[Convert.ToInt32(orignalimages[0].Key.Split('%')[1])].Split(',');

                                            Image img = new Image();
                                            img.Source = bitmapImage;
                                            img.Stretch = Stretch.Fill;
                                            img.Width = viewport.Width;

                                            var bitMap = new BitmapImage();
                                            bitMap.SetSource(orignalimages[0].Value);

                                            if (bitMap.PixelWidth == bitmapImage.PixelWidth && bitMap.PixelHeight == bitmapImage.PixelHeight)
                                                img.Height = viewport.Height + Convert.ToDouble(vp[3]);

                                            bitMap = null;

                                            img.SetValue(Canvas.LeftProperty, Convert.ToDouble(vp[0]));
                                            img.SetValue(Canvas.TopProperty, Convert.ToDouble(vp[1]));

                                            var parent = path.Parent as Canvas;
                                            parent.Clip = null;
                                            var index = parent.Children.IndexOf(path);
                                            parent.Children.Insert(index, img);
                                            i++;

                                        }
                                        else
                                        {
                                            var canvas = path.Parent as Canvas;
                                            var changePath = canvas.Children.Where(w => (w is System.Windows.Shapes.Path) && (w != path) &&
                                                (w as System.Windows.Shapes.Path).Data.Bounds.Top > path.Data.Bounds.Top
                                                && (w as System.Windows.Shapes.Path).Data.Bounds.Bottom < path.Data.Bounds.Bottom)
                                                .OrderBy(p => canvas.Children.IndexOf(p)).ToList();

                                            var index = 0;
                                            if (path.Parent != _mainCanvas)
                                            {
                                                index = _mainCanvas.Children.IndexOf(path.Parent as Canvas);

                                                for (int c = 0; c < changePath.Count; c++)
                                                {
                                                    canvas.Children.Remove(changePath[c]);
                                                    _mainCanvas.Children.Insert((index + 1 + c), changePath[c]);
                                                    changePath[c].SetValue(Canvas.ZIndexProperty, ++zIndex);
                                                }
                                            }
                                            else
                                            {
                                                changePath.ForEach(f => f.SetValue(Canvas.ZIndexProperty, ++zIndex));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var img = _OrignalimageSources.Where(w => w.Value != null && w.Value.Length == stream.Length && w.Key != source).ToList();
                                        if (img.Count == 0)
                                            path.Visibility = Visibility.Visible;
                                    }
                                }
                            }
                        }
                        _imageSources.Remove(Convert.ToString(path.GetValue(Canvas.TagProperty)));
                    }
                }
            }
            else
            {
                var glyphs = uiElement as Glyphs;
                if (glyphs != null)
                {
                    var stream = Application.GetResourceStream(_streamResourceInfo, ConvertPartName(glyphs.FontUri.ToString())).Stream;
                    glyphs.FontSource = new FontSource(stream);
                    zIndex++;
                    glyphs.SetValue(Canvas.ZIndexProperty, zIndex);
                }
            }
        }
        internal void CenterTextBlocks(UIElement uiElement , ref int i)
        {
            // Center TextBlocks within their parent (leaving specified Left/Top properties alone)
            var block = uiElement as TextBlock;
            if (null != block)
            {
                var parent = block.Parent as FrameworkElement;
                if (null != parent)
                {
                    if (0 == (double)block.GetValue(Canvas.LeftProperty))
                    {
                        block.SetValue(Canvas.LeftProperty, (parent.Width - block.ActualWidth) / 2);
                    }
                    if (0 == (double)block.GetValue(Canvas.TopProperty))
                    {
                        block.SetValue(Canvas.TopProperty, (parent.Height - block.ActualHeight) / 2);
                    }
                }
            }
        }
        internal void ShowStatusMessage(string message)
        {
            // Show a status message
            //statusText.Text = string.Format("Status: {0}", message);
            if (message.Length > 30)
                statusText.Text = message.Substring(0, 30) + "...";
            else
                statusText.Text = message;
        }
        internal static Uri ConvertPartName(string partName)
        {
            // Remove the leading '/' from part names since Silverlight doesn't seem to like them there
            return new Uri(partName.TrimStart('/'), UriKind.RelativeOrAbsolute);
        }
        #endregion

        #region EventHandlers
        public void zoom_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            // Zoom in/out
            UpdateZoomUI(Math.Max(Math.Min(_pageScale * ((zoomOut == sender) ? 0.9 : 1 / 0.9), MaximumScale), MinimumScale));
        }
        public void zoomBar_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            // Zoom to the specified value
            var point = e.GetPosition(zoomBar);
            UpdateZoomUI(((point.X / zoomBar.Width) * (MaximumScale - MinimumScale)) + MinimumScale);
        }

        void _webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (null == e.Error)
                {

                    HideProgress();
                    _streamResourceInfo = new StreamResourceInfo(e.Result, null);

                    // Reset state and display the first page of the new content
                    _pageNames.Clear();
                    _pageNumber = 0;
                    ReadPackagePageContent();
                    DisplayCurrentPage();

                    // Simulate a resize to display the page in full-page view
                    Application_Current_Host_Content_Resized(null, null);
                }
                else
                {
                    ShowStatusMessage(e.Error.Message);
                }
            }
            else
            {
                HideProgress();
                ShowStatusMessage(string.Empty);
            }
        }
        void CreateWebClientInstance()
        {
            if (_webClient != null)
            {
                _webClient.OpenReadCompleted -= (_webClient_OpenReadCompleted);
                _webClient.DownloadProgressChanged -= (_webClient_DownloadProgressChanged);
            }
            _webClient = null;
            _webClient = new WebClient();
            _webClient.OpenReadCompleted += (_webClient_OpenReadCompleted);
            _webClient.DownloadProgressChanged += (_webClient_DownloadProgressChanged);
        }
        void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ShowProgress(string.Format("Downloading {0:##}%", ((100.0 * e.BytesReceived) / e.TotalBytesToReceive)));
        }


        protected void Application_Current_Host_Content_Resized(object sender, EventArgs e)
        {
            var width = 0.0;
            var height = 0.0;

            width = xps_viewer.Width;
            height = xps_viewer.Height;

            if (!double.IsNaN(pageContents.Width) || !double.IsNaN(pageContents.Height))
            {
                // Zoom to full-page view  
                UpdateZoomUI(Math.Min(width / pageContents.Width, height / pageContents.Height));
            }
            page.SetValue(Canvas.LeftProperty, 0.0);
            page.SetValue(Canvas.TopProperty, 0.0);

            pageContents.SetValue(Canvas.LeftProperty, 0.0);
            pageContents.SetValue(Canvas.TopProperty, 0.0);

            scaler.CenterX = 0;
            scaler.CenterY = 0;
        }
        protected void Page_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            // If the click was not on the controls UI, then begin a drag of the page
            var controlsRect = new Rect((double)controls.GetValue(Canvas.LeftProperty), (double)controls.GetValue(Canvas.TopProperty), controls.Width, controls.Height);
            var position = e.GetPosition(null);
            if (!controlsRect.Contains(position))
            {
                _lastMousePosition = position;
            }
        }
       
        protected void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (null != _lastMousePosition)
            {
                // Compute the delta from the last position (adjusted for current scale)
                var position = e.GetPosition(null);

                var dx = (position.X - _lastMousePosition.Value.X);
                var dy = (position.Y - _lastMousePosition.Value.Y);

                _lastMousePosition = position;

                // Move the page accordingly
                pageContents.SetValue(Canvas.LeftProperty, (double)pageContents.GetValue(Canvas.LeftProperty) + dx);
                pageContents.SetValue(Canvas.TopProperty, (double)pageContents.GetValue(Canvas.TopProperty) + dy);

            }
        }
        protected void Page_MouseLeftButtonUp(object sender, /* Mouse */ EventArgs e)
        {
            _lastMousePosition = null;
        }
        #endregion

        private void parentCanvas_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        private void btnshow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadDocsByUrl(txt_Url.Text);
        }
        
        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_pageNumber > 0)
            {
                _pageNumber -= 1;
                DisplayCurrentPage();
            }
        }

        private void Path_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (_pageNumber < _pageNames.Count - 1)
            {
                _pageNumber += 1;
                DisplayCurrentPage();
            }
        }
    }
}
