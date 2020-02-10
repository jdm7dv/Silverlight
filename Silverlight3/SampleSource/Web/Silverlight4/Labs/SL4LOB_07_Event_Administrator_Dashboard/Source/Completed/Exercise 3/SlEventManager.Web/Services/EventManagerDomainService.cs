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
    using System.Web.Security;
    using System.Web.Profile;
    using SlEventManager.Web.Models;


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

        [RequiresRole("Event Administrators")]
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


        [Invoke]
        public IEnumerable<int> FetchEventsForWhichCurrentUserIsRegistered()
        {
            MembershipUser mu = Membership.GetUser();
            if (mu == null)
            {
                return new int[0];
            }
            var q = from attendeeEvent in this.ObjectContext.AttendeeEvents
                    where attendeeEvent.Attendee.AspNetUserId == (Guid)mu.ProviderUserKey
                    select attendeeEvent.EventID;
            return q;
        }

        [Invoke]
        public void RegisterCurrentUserForEvent(int eventId)
        {
            Attendee attendee = GetOrCreateAttendeeForCurrentUser();
            if (!attendee.AttendeeEvents.Any(ev => ev.EventID == eventId))
            {
                attendee.AttendeeEvents.Add(new AttendeeEvent { EventID = eventId });
            }

            this.ObjectContext.SaveChanges();
        }
        [Invoke]
        public void UnregisterCurrentUserForEvent(int eventId)
        {
            Attendee attendee = GetOrCreateAttendeeForCurrentUser();
            AttendeeEvent av = attendee.AttendeeEvents.SingleOrDefault(
                                   x => x.EventID == eventId);
            if (av != null)
            {
                attendee.AttendeeEvents.Remove(av);
            }

            this.ObjectContext.SaveChanges();
        }
        private Attendee GetOrCreateAttendeeForCurrentUser()
        {
            MembershipUser mu = Membership.GetUser();
            if (mu == null)
            {
                throw new InvalidOperationException("User not logged in");
            }
            Attendee at = this.ObjectContext.Attendees.FirstOrDefault(
                              x => x.AspNetUserId == (Guid)mu.ProviderUserKey);
            if (at == null)
            {
                at = new Attendee
                {
                    AspNetUserId = (Guid)mu.ProviderUserKey
                };
                this.ObjectContext.AddToAttendees(at);
            }
            return at;
        }

        [Invoke]
        public void SetCurrentUserImage(byte[] imageBytes)
        {
            Attendee att = GetOrCreateAttendeeForCurrentUser();
            att.UserPicture = imageBytes;
            ObjectContext.SaveChanges();
        }

        [Invoke]
        public byte[] GetCurrentUserImage()
        {
            Attendee att = GetOrCreateAttendeeForCurrentUser();
            return att.UserPicture;
        }

        [Invoke]
        public IEnumerable<int> FetchTalksUserHasInScheduleForEvent(int eventID)
        {
            MembershipUser mu = Membership.GetUser();
            if (mu == null)
            {
                return new int[0];
            }
            var q = from atSchTalk in this.ObjectContext.AttendeeScheduleTalks
                    where atSchTalk.Attendee.AspNetUserId == (Guid)mu.ProviderUserKey
                    select atSchTalk.TalkID;
            return q;
        }

        [Invoke]
        public void AddTalkToUserSchedule(int talkID)
        {
            Attendee attendee = GetOrCreateAttendeeForCurrentUser();
            AttendeeScheduleTalk ast = new AttendeeScheduleTalk { TalkID = talkID };
            attendee.AttendeeScheduleTalks.Add(ast);
            this.ObjectContext.SaveChanges();
        }

        [Invoke]
        public void RemoveTalkFromUserSchedule(int talkID)
        {
            Attendee attendee = GetOrCreateAttendeeForCurrentUser();
            var astQuery = from entry in attendee.AttendeeScheduleTalks
                           where entry.TalkID == talkID
                           select entry;

            AttendeeScheduleTalk ast = astQuery.SingleOrDefault();
            if (ast != null)
            {
                this.ObjectContext.DeleteObject(ast);
                this.ObjectContext.SaveChanges();
            }
        }


        [RequiresRole("Event Administrators")]
        public IQueryable<AttendeeEvent> GetUnacknowledgedAttendeeEventsWithEvents()
        {
            return from attendeeEvent in this.ObjectContext.AttendeeEvents.Include("Event")
                   where !attendeeEvent.IsAcknowledged
                   select attendeeEvent;
        }


        [RequiresRole("Event Administrators")]
        public IEnumerable<UserDisplayDetails> GetUserDisplayDetails(int[] attendeeIds)
        {
            var dbAttendeeQuery = from attendee in this.ObjectContext.Attendees
                                  where attendeeIds.Contains(attendee.AttendeeID)
                                  select new
                                  {
                                      attendee.AttendeeID,
                                      attendee.AspNetUserId
                                  };
            var allUsers = Membership.GetAllUsers().OfType<MembershipUser>();

            var results = from attendeeInfo in dbAttendeeQuery.ToList()
                          let aspNetUser = allUsers.FirstOrDefault(mu =>
                                ((Guid)mu.ProviderUserKey) == attendeeInfo.AspNetUserId)
                          where aspNetUser != null
                          let prof = ProfileBase.Create(aspNetUser.UserName)
                          where prof != null
                          select new UserDisplayDetails
                          {
                              AttendeeID = attendeeInfo.AttendeeID,
                              Email = aspNetUser.Email,
                              FriendlyName = prof.GetPropertyValue("FriendlyName") as string
                          };


            return results;
        }

    }
}