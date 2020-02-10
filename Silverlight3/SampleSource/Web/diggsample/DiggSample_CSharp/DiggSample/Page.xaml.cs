using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Xml.Linq;

namespace DiggSample
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve Topic to Search for from WaterMarkTextBox
            string topic = txtSearchTopic.Text;

            // Construct Digg REST URL
            string diggUrl = String.Format("http://services.digg.com/stories/topic/{0}?count=20&appkey=http%3A%2F%2Fscottgu.com", topic);

            // Initiate Async Network call to Digg
            WebClient diggService = new WebClient();
            diggService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DiggService_DownloadStoriesCompleted);
            diggService.DownloadStringAsync(new Uri(diggUrl));
        }

        void DiggService_DownloadStoriesCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                DisplayStories(e.Result);
            }
        }

        void DisplayStories(string xmlContent)
        {
            XDocument xmlStories = XDocument.Parse(xmlContent);

            var stories = from story in xmlStories.Descendants("story")
                          where story.Element("thumbnail") != null && 
                                !story.Element("thumbnail").Attribute("src").Value.EndsWith(".gif")
                          select new DiggStory
                          {
                              Id = (int)story.Attribute("id"),
                              Title = ((string)story.Element("title")).Trim(),
                              Description = ((string)story.Element("description")).Trim(),
                              ThumbNail = (string)story.Element("thumbnail").Attribute("src").Value,
                              HrefLink = new Uri((string)story.Attribute("link")),
                              NumDiggs = (int)story.Attribute("diggs"),
                              UserName = (string)story.Element("user").Attribute("name").Value,
                          };

            StoriesList.SelectedIndex = -1;
            StoriesList.ItemsSource = stories;
        }

        void StoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DiggStory story = (DiggStory) StoriesList.SelectedItem;

            if (story != null)
            {
                DetailsView.DataContext = story;
                DetailsView.Visibility = Visibility.Visible;
            }
        }
    }
}

