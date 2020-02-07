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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SlEventManager.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
            WebContext.Current.Authentication.LoggedIn += (s, e) => UpdateForUserRole();
            WebContext.Current.Authentication.LoggedOut += (s, e) => UpdateForUserRole();
            UpdateForUserRole();
        }

        
        private Visibility _adminButtonsVisibility;
        public Visibility AdminButtonsVisibility
        {
            get { return _adminButtonsVisibility; }
            set
            {
                if (_adminButtonsVisibility != value)
                {
                    _adminButtonsVisibility = value;
                    OnPropertyChanged("AdminButtonsVisibility");
                }
            }
        }


        private Visibility _attendeeButtonsVisibility;
        public Visibility AttendeeButtonsVisibility
        {
            get { return _attendeeButtonsVisibility; }
            set
            {
                if (_attendeeButtonsVisibility != value)
                {
                    _attendeeButtonsVisibility = value;
                    OnPropertyChanged("AttendeeButtonsVisibility");
                }
            }
        }


        private void UpdateForUserRole()
        {
            bool isLoggedIn = WebContext.Current.User.IsAuthenticated;
            bool isAdmin = isLoggedIn &&
                           WebContext.Current.User.IsInRole("Event Administrators");

            AdminButtonsVisibility = isAdmin ?
                Visibility.Visible : Visibility.Collapsed;
            AttendeeButtonsVisibility = (isLoggedIn && !isAdmin) ?
                Visibility.Visible : Visibility.Collapsed;
        }

    }
}