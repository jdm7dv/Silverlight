using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Mix08
{
  public partial class Page : UserControl
  {
    private string _randomUrl { get { return "http://catalog.video.msn.com/randomVideo.aspx?mk=us&vs=0&ff=99&c=10&random=" + DateTime.Now.Millisecond.ToString(); } }

    public Page()
    {
      InitializeComponent();
      GetVideos(_randomUrl);
    }

    private void GetVideos(string url)
    {
      WebClient wc = new WebClient();
      wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
      wc.DownloadStringAsync(new Uri(url));
    }

    List<VideoSource> _videos;

    void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      this.wait.Visibility = Visibility.Collapsed;
      this.wtspin.Stop();

      _videos = new List<VideoSource>();

      XDocument doc = XDocument.Parse(e.Result);

      foreach (XElement video in doc.Descendants(_NS + "video"))
      {
        _videos.Add(new VideoSource()
        {
          Title = (string)video.Element(_NS + "title"),
          Source = (string)(video.Element(_NS + "source").Attribute("friendlyName")),
          PublishDate = DateTime.Parse((string)video.Element(_NS + "startDate")).ToShortDateString(),
          Description = (string)video.Element(_NS + "description"),
          ImageUrl = GetUriAsset(video.Element(_NS + "files"), "file", "2007"),
          LargeImageUrl = GetUriAsset(video.Element(_NS + "files"), "file", "2009"),
          VideoUrl = GetUriAsset(video.Element(_NS + "videoFiles"), "videoFile", "1002"),
          ViewCount = (string)video.Element(_NS + "usage").Element(_NS + "usageItem").Attribute("totalCount"),
        });
      }

      this.results.ItemsSource = _videos;
      if (_videos.Count > 0)
      {
        this.details.DataContext = _videos[0];
      }

    }

    #region LINQ Helper
    private XNamespace _NS = "urn:schemas-microsoft-com:msnvideo:catalog";
    private string GetUriAsset(XElement element, string nodeName, string formatCode)
    {
      IEnumerable<string> uris = from file in element.Descendants(_NS + nodeName)
                                 where (string)file.Attribute("formatCode") == formatCode
                                 select (string)file.Element(_NS + "uri");
      return ((uris.Count<string>() > 0) ? uris.First<string>() : string.Empty);
    }
    #endregion

    private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      details.DataContext = (sender as StackPanel).DataContext;
    }
    private string _searchUrl = "http://catalog.video.msn.com/search.aspx?mk=us&ff=99&q=";

    private void tb_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter) SearchVideos(tb.Text);

    }

    private void SearchClick(object sender, RoutedEventArgs e)
    {
      SearchVideos(tb.Text);
    }

    private void SearchVideos(string searchText)
    {
      if (searchText != "")
      {
        GetVideos(_searchUrl + searchText);
      }
    }

    private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
    {
      ((Storyboard)(sender as StackPanel).FindName("grow")).Begin();
    }

    private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
    {
      ((Storyboard)(sender as StackPanel).FindName("shrink")).Begin();
    }
  }
}
