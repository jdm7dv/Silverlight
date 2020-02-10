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
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            myVideoPanel.EntryDropped += new EventHandler<VideoPanelEventArgs>(myVideoPanel_EntryDropped);            
        }

        void myVideoPanel_EntryDropped(object sender, VideoPanelEventArgs e)
        {
            VideoPlayer newVideoPlayer = new VideoPlayer(e.VideoUri);

            // Position new player on the screen
            Canvas.SetLeft(newVideoPlayer, e.DropLocationMouseButtonEventArgs.GetPosition(null).X - newVideoPlayer.Width / 2);
            Canvas.SetTop(newVideoPlayer, e.DropLocationMouseButtonEventArgs.GetPosition(null).Y - newVideoPlayer.Height / 2);
            
            VideoArea.Children.Add(newVideoPlayer);
        }

    }
}
