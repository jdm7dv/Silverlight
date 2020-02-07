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
using System.Windows.Navigation;
using SlEventManager.ViewModels;
using SlEventManager.Web;

namespace SlEventManager.Views
{
    public partial class SchedulePlanner : Page
    {
        SchedulePlannerViewModel _viewModel = new SchedulePlannerViewModel();

        public SchedulePlanner()
        {
            InitializeComponent();

            this.DataContext = _viewModel;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string eventId = NavigationContext.QueryString["EventID"];

            _viewModel.Load(int.Parse(eventId));
        }

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(popupContainer);
            Canvas.SetLeft(menuPopup, mousePosition.X);
            Canvas.SetTop(menuPopup, mousePosition.Y);

            FrameworkElement itemTemplateInstance = (FrameworkElement)sender;
            menuPopup.DataContext = itemTemplateInstance.DataContext;

            menuPopup.IsOpen = true;
            popupContainer.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void popupContainer_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            CloseContextMenu();
        }

        private void CloseContextMenu()
        {
            popupContainer.Background = null;
            menuPopup.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Talk talk = (Talk)btn.DataContext;
            _viewModel.RemoveTalk(talk);

            CloseContextMenu();
        }

    }
}