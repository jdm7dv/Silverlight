using System;
using System.Windows;
using System.Windows.Controls;

namespace DiggSample
{
    public partial class StoryDetailsView : UserControl
    {
        public StoryDetailsView()
        {
            InitializeComponent();
        }

        void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}


