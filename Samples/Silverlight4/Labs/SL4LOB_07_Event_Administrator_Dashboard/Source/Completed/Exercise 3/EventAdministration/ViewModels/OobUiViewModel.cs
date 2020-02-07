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
using System.Collections.Generic;
using System.Linq;
using SlEventManager.Web.Services;
using System.Windows.Threading;

namespace SlEventManager.ViewModels
{
    public class OobUiViewModel : ViewModelBase
    {
        private IList<UnacknowledgedRegistrationViewModel> _unacknowledgedRegistrations;
        public IList<UnacknowledgedRegistrationViewModel> UnacknowledgedRegistrations
        {
            get { return _unacknowledgedRegistrations; }
            set
            {
                if (_unacknowledgedRegistrations != value)
                {
                    _unacknowledgedRegistrations = value;
                    OnPropertyChanged("UnacknowledgedRegistrations");
                }
            }
        }


        private EventManagerDomainContext ctx = new EventManagerDomainContext();

        public void OnGet()
        {
            ctx.Load(ctx.GetUnacknowledgedAttendeeEventsWithEventsQuery(), loadUnackOp =>
            {
                if (loadUnackOp.HasError) { loadUnackOp.MarkErrorAsHandled(); return; }

                int[] attendeeIds = (from atev in loadUnackOp.Entities
                                     select atev.AttendeeID).Distinct().ToArray();

                ctx.Load(ctx.GetUserDisplayDetailsQuery(attendeeIds), loadDetailsOp =>
                {
                    if (loadDetailsOp.HasError)
                    { loadDetailsOp.MarkErrorAsHandled(); return; }

                    var attendeeDetails = loadDetailsOp.Entities.ToDictionary(d => d.AttendeeID);

                    var registrations = from atev in loadUnackOp.Entities
                                        let details = attendeeDetails[atev.AttendeeID]
                                        select new UnacknowledgedRegistrationViewModel
                                        {
                                            EventTitle = atev.Event.EventTitle,
                                            UserDisplayName = details.FriendlyName,
                                            AttendeeEventID = atev.AttendeeEventID,
                                            UserEmail = details.Email
                                        };

                    if (UnacknowledgedRegistrations != null)
                    {
                        int[] oldAttendeeEventIds = UnacknowledgedRegistrations.Select(
                            e => e.AttendeeEventID).Distinct().ToArray();
                        int[] newAttendeeEventIds = loadUnackOp.Entities.Select(
                            e => e.AttendeeEventID).Distinct().ToArray();

                        if (newAttendeeEventIds.Any(id => !oldAttendeeEventIds.Contains(id))
                            && showNotification != null)
                        {
                            showNotification(newAttendeeEventIds.Length);
                        }
                    }

                    UnacknowledgedRegistrations = registrations.ToList();
                }, null);
            }, null);
        }

        private DispatcherTimer dt;
        private Action<int> showNotification;

        public void Load(Action<int> notifyCallback)
        {
            showNotification = notifyCallback;
            if (dt == null)
            {
                dt = new DispatcherTimer();
                dt.Tick += NotifyTick;
                dt.Interval = TimeSpan.FromSeconds(10);
            }
            dt.Start();
        }

        void NotifyTick(object sender, EventArgs e)
        {
            OnGet();
        }

        public void Unload()
        {
            dt.Stop();
            showNotification = null;
        }

    }
}