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

namespace SilverBlog.SLFrontend
{
    public partial class Page : UserControl
    {
        BlogService.BlogServiceClient service;

        public Page()
        {
            InitializeComponent();
            service = ServiceFactory.GetClient();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            service.TestServiceCompleted += (sen, args) =>
                {
                    MessageBox.Show(args.Result.Title);
                };
            service.TestServiceAsync();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            service.CreateEntryCompleted += (sen, args) =>
            {
                MessageBox.Show("Done");
            };

            DTO.Entry entry = new SilverBlog.DTO.Entry()
            {
                Title = uxNewTitle.Text,
                Description = uxNewContent.Text
            };
            service.CreateEntryAsync(entry);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            uxHomePanel.Visibility = Visibility.Visible;
            uxCreatePanel.Visibility = Visibility.Collapsed;

            LoadEntries();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            uxHomePanel.Visibility = Visibility.Collapsed;
            uxCreatePanel.Visibility = Visibility.Visible;
        }

        private void LoadEntries()
        {
            service.GetLast10EntriesCompleted += (sen, args) =>
            {
                uxEntryList.ItemsSource = args.Result;
            };
            service.GetLast10EntriesAsync();
        }

    }
}
