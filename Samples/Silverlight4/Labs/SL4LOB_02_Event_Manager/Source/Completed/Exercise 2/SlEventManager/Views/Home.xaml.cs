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

namespace SlEventManager
{
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using SlEventManager.Resources;
    using System;
    using SlEventManager.Web;

    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        ///     Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void eventDomainDataSource_LoadedData(object sender, System.Windows.Controls.LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void NavigateToEditEvent(int eventId)
        {
            NavigationService.Navigate(new Uri("/EditEvent?EventID=" +
                eventId, UriKind.Relative));
        }

        private void editCurrentButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Event currentEvent = eventDataGrid.SelectedItem as Event;
            if (currentEvent != null)
            {
                NavigateToEditEvent(currentEvent.EventID);
            }

        }

        private void eventDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editCurrentButton.IsEnabled = eventDataGrid.SelectedItem != null;

        }

    }
}