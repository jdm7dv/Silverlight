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
using SlEventManager.Web;
using SlEventManager.Web.Services;
using System.ServiceModel.DomainServices.Client;


namespace SlEventManager.ViewModels
{
    public class SchedulePlannerViewModel : ViewModelBase
    {
        private EventManagerDomainContext _context;

        public void Load(int eventId)
        {
            _context = new EventManagerDomainContext();

            var eventQuery = from ev in _context.GetEventsWithTracksAndTalksQuery()
                             where ev.EventID == eventId
                             select ev;

            _context.Load(eventQuery, (loadOp) =>
            {
                var talkQuery = from ev in loadOp.Entities
                                from track in ev.EventTracks
                                from talk in track.Talks
                                select talk;

                AllTalks = talkQuery.ToList();

                EventTitle = loadOp.Entities.Select(ev => ev.EventTitle).FirstOrDefault();
            }, null);
        }


        private IList<Talk> _allTalks;
        public IList<Talk> AllTalks
        {
            get { return _allTalks; }
            set
            {
                if (value != _allTalks)
                {
                    _allTalks = value;
                    OnPropertyChanged("AllTalks");
                }
            }
        }

        private string _eventTitle;
        public string EventTitle
        {
            get { return _eventTitle; }
            set
            {
                if (value != _eventTitle)
                {
                    _eventTitle = value;
                    OnPropertyChanged("EventTitle");
                }
            }
        }

    }
}