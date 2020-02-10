// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

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
using SlEventManager.Web.Services;
using SlEventManager.ViewModels;

namespace SlEventManager
{
    public partial class OobUi : UserControl
    {
        OobUiViewModel viewModel = new OobUiViewModel();

        public OobUi()
        {
            InitializeComponent();

            Application.Current.CheckAndDownloadUpdateCompleted += new CheckAndDownloadUpdateCompletedEventHandler(Current_CheckAndDownloadUpdateCompleted);
            Application.Current.CheckAndDownloadUpdateAsync();

            this.DataContext = viewModel;
            this.Loaded += new RoutedEventHandler(OobUi_Loaded);
        }

        void OobUi_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.Load(ShowNotification);
        }

        void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                MessageBox.Show("An update is available for this application. Please close the application and restart it.");
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow w = new LoginWindow();
            w.Show();
        }

        private void getButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OnGet();
        }

        NotificationWindow nw = new NotificationWindow();
        void ShowNotification(int count)
        {
            if (nw.Visibility == System.Windows.Visibility.Visible)
            {
                nw.Close();
            }
            nw.Content = new NotificationContent();
            nw.Content.DataContext = count;
            nw.Width = nw.Content.Width + 2;
            nw.Height = nw.Content.Height + 2;
            nw.Show(5000);
        }

        private void showToastButton_Click(object sender, RoutedEventArgs e)
        {
            ShowNotification(42);
        }

    }
}