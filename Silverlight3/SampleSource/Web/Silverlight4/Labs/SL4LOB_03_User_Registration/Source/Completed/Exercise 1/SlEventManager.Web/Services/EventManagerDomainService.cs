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

namespace SlEventManager.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using SlEventManager.Web;


    // Implements application logic using the SlEventManagerEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class EventManagerDomainService : LinqToEntitiesDomainService<SlEventManagerEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with alternate signatures.
        // Alternatively, to mark this as the default parameterless query for this entity type, apply the
        // QueryAttribute custom attribute and set its IsDefault property true.
        public IQueryable<Attendee> GetAttendees()
        {
            return this.ObjectContext.Attendees;
        }

        public void InsertAttendee(Attendee attendee)
        {
            if ((attendee.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(attendee, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Attendees.AddObject(attendee);
            }
        }

        public void UpdateAttendee(Attendee currentAttendee)
        {
            this.ObjectContext.Attendees.AttachAsModified(currentAttendee, this.ChangeSet.GetOriginal(currentAttendee));
        }

        public void DeleteAttendee(Attendee attendee)
        {
            if ((attendee.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Attendees.Attach(attendee);
            }
            this.ObjectContext.Attendees.DeleteObject(attendee);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with alternate signatures.
        // Alternatively, to mark this as the default parameterless query for this entity type, apply the
        // QueryAttribute custom attribute and set its IsDefault property true.
        public IQueryable<AttendeeEvent> GetAttendeeEvents()
        {
            return this.ObjectContext.AttendeeEvents;
        }

        public void InsertAttendeeEvent(AttendeeEvent attendeeEvent)
        {
            if ((attendeeEvent.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(attendeeEvent, EntityState.Added);
            }
            else
            {
                this.ObjectContext.AttendeeEvents.AddObject(attendeeEvent);
            }
        }

        public void UpdateAttendeeEvent(AttendeeEvent currentAttendeeEvent)
        {
            this.ObjectContext.AttendeeEvents.AttachAsModified(currentAttendeeEvent, this.ChangeSet.GetOriginal(currentAttendeeEvent));
        }

        public void DeleteAttendeeEvent(AttendeeEvent attendeeEvent)
        {
            if ((attendeeEvent.EntityState == EntityState.Detached))
            {
                this.ObjectContext.AttendeeEvents.Attach(attendeeEvent);
            }
            this.ObjectContext.AttendeeEvents.DeleteObject(attendeeEvent);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with alternate signatures.
        // Alternatively, to mark this as the default parameterless query for this entity type, apply the
        // QueryAttribute custom attribute and set its IsDefault property true.
        public IQueryable<AttendeeScheduleTalk> GetAttendeeScheduleTalks()
        {
            return this.ObjectContext.AttendeeScheduleTalks;
        }

        public void InsertAttendeeScheduleTalk(AttendeeScheduleTalk attendeeScheduleTalk)
        {
            if ((attendeeScheduleTalk.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(attendeeScheduleTalk, EntityState.Added);
            }
            else
            {
                this.ObjectContext.AttendeeScheduleTalks.AddObject(attendeeScheduleTalk);
            }
        }

        public void UpdateAttendeeScheduleTalk(AttendeeScheduleTalk currentAttendeeScheduleTalk)
        {
            this.ObjectContext.AttendeeScheduleTalks.AttachAsModified(currentAttendeeScheduleTalk, this.ChangeSet.GetOriginal(currentAttendeeScheduleTalk));
        }

        public void DeleteAttendeeScheduleTalk(AttendeeScheduleTalk attendeeScheduleTalk)
        {
            if ((attendeeScheduleTalk.EntityState == EntityState.Detached))
            {
                this.ObjectContext.AttendeeScheduleTalks.Attach(attendeeScheduleTalk);
            }
            this.ObjectContext.AttendeeScheduleTalks.DeleteObject(attendeeScheduleTalk);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with alternate signatures.
        // Alternatively, to mark this as the default parameterless query for this entity type, apply the
        // QueryAttribute custom attribute and set its IsDefault property true.
        public IQueryable<Event> GetEvents()
        {
            return this.ObjectContext.Events;
        }

        public IQueryable<Event> GetEventsWithTracksAndTalks()
        {
            return this.ObjectContext.Events.Include("EventTracks.Talks");
        }


        public void InsertEvent(Event @event)
        {
            if ((@event.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(@event, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Events.AddObject(@event);
            }
        }

        public void UpdateEvent(Event currentEvent)
        {
            this.ObjectContext.Events.AttachAsModified(currentEvent, this.ChangeSet.GetOriginal(currentEvent));
        }

        public void DeleteEvent(Event @event)
        {
            if ((@event.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Events.Attach(@event);
            }
            this.ObjectContext.Events.DeleteObject(@event);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with alternate signatures.
        // Alternatively, to mark this as the default parameterless query for this entity type, apply the
        // QueryAttribute custom attribute and set its IsDefault property true.
        public IQueryable<EventTrack> GetEventTracks()
        {
            return this.ObjectContext.EventTracks;
        }

        public void InsertEventTrack(EventTrack eventTrack)
        {
            if ((eventTrack.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(eventTrack, EntityState.Added);
            }
            else
            {
                this.ObjectContext.EventTracks.AddObject(eventTrack);
            }
        }

        public void UpdateEventTrack(EventTrack currentEventTrack)
        {
            this.ObjectContext.EventTracks.AttachAsModified(currentEventTrack, this.ChangeSet.GetOriginal(currentEventTrack));
        }

        public void DeleteEventTrack(EventTrack eventTrack)
        {
            if ((eventTrack.EntityState == EntityState.Detached))
            {
                this.ObjectContext.EventTracks.Attach(eventTrack);
            }
            this.ObjectContext.EventTracks.DeleteObject(eventTrack);
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with alternate signatures.
        // Alternatively, to mark this as the default parameterless query for this entity type, apply the
        // QueryAttribute custom attribute and set its IsDefault property true.
        public IQueryable<Talk> GetTalks()
        {
            return this.ObjectContext.Talks;
        }

        public void InsertTalk(Talk talk)
        {
            if ((talk.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(talk, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Talks.AddObject(talk);
            }
        }

        public void UpdateTalk(Talk currentTalk)
        {
            this.ObjectContext.Talks.AttachAsModified(currentTalk, this.ChangeSet.GetOriginal(currentTalk));
        }

        public void DeleteTalk(Talk talk)
        {
            if ((talk.EntityState == EntityState.Detached))
            {
                this.ObjectContext.Talks.Attach(talk);
            }
            this.ObjectContext.Talks.DeleteObject(talk);
        }
    }
}