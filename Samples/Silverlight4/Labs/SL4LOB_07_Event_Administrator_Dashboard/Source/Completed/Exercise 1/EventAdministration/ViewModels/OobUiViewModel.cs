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
                int[] attendeeIds = (from atev in loadUnackOp.Entities
                                     select atev.AttendeeID).Distinct().ToArray();

                ctx.Load(ctx.GetUserDisplayDetailsQuery(attendeeIds), loadDetailsOp =>
                {
                    var attendeeDetails = loadDetailsOp.Entities.ToDictionary(d => d.AttendeeID);

                    var registrations = from atev in loadUnackOp.Entities
                                        let details = attendeeDetails[atev.AttendeeID]
                                        select new UnacknowledgedRegistrationViewModel
                                        {
                                            EventTitle = atev.Event.EventTitle,
                                            UserDisplayName = details.FriendlyName,
                                            UserEmail = details.Email
                                        };
                    UnacknowledgedRegistrations = registrations.ToList();
                }, null);
            }, null);
        }

    }
}