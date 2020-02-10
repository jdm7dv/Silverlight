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
using SlEventManager.Web;
using System.Collections.Generic;
using SlEventManager.Web.Services;
using SlEventManager.Helpers;

namespace SlEventManager.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
            _registerCommand = new RelayCommand(OnRegister);
            _unregisterCommand = new RelayCommand(OnUnregister);
            _plannerCommand = new RelayCommand(OnPlanner);

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
        
            if (isLoggedIn)
            {
                var ctx = new EventManagerDomainContext();
                ctx.FetchEventsForWhichCurrentUserIsRegistered((op) =>
                {
                    if (!op.HasError)
                    {
                        var items = op.Value;
                        _currentUserRegisteredEventIds = new HashSet<int>(items);
                        UpdateRegistrationButtons();
                    }
                }, null);
            }
            else
            {
                _currentUserRegisteredEventIds = null;
                UpdateRegistrationButtons();
            }
        }


        private void OnRegister()
        {
            if (SelectedEvent != null)
            {
                var ctx = new EventManagerDomainContext();
                ctx.RegisterCurrentUserForEvent(SelectedEvent.EventID, (op) =>
                {
                    UpdateForUserRole();
                }, null);
            }
        }

        private void OnUnregister()
        {
            if (SelectedEvent != null)
            {
                var ctx = new EventManagerDomainContext();
                ctx.UnregisterCurrentUserForEvent(SelectedEvent.EventID, (op) =>
                {
                    UpdateForUserRole();
                }, null);
            }
        }

        private readonly RelayCommand _registerCommand;
        public ICommand RegisterCommand { get { return _registerCommand; } }
        private readonly RelayCommand _unregisterCommand;
        public ICommand UnregisterCommand { get { return _unregisterCommand; } }


        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                _selectedEvent = value;
                UpdateRegistrationButtons();
            }
        }

        private void UpdateRegistrationButtons()
        {
            _registerCommand.IsEnabled = _currentUserRegisteredEventIds != null &&
                SelectedEvent != null &&
                !_currentUserRegisteredEventIds.Contains(SelectedEvent.EventID);

            _unregisterCommand.IsEnabled = _currentUserRegisteredEventIds != null &&
                 SelectedEvent != null &&
                _currentUserRegisteredEventIds.Contains(SelectedEvent.EventID);
            
            _plannerCommand.IsEnabled = _unregisterCommand.IsEnabled;
        }


        private HashSet<int> _currentUserRegisteredEventIds;


        public event EventHandler<NavigateToEntityEventArgs> NavigateToSchedulePlanner;
        private readonly RelayCommand _plannerCommand;
        public ICommand PlannerCommand { get { return _plannerCommand; } }
        private void OnPlanner()
        {
            if (SelectedEvent != null && NavigateToSchedulePlanner != null)
            {
                NavigateToSchedulePlanner(this,
                    new NavigateToEntityEventArgs(SelectedEvent.EventID));
            }
        }
    }
}