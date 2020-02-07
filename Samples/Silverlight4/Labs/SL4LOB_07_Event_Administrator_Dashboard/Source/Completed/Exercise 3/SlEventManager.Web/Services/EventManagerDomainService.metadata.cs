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

#pragma warning disable 649    // disable compiler warnings about unassigned fields

namespace SlEventManager.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies AttendeeMetadata as the class
    // that carries additional metadata for the Attendee class.
    [MetadataTypeAttribute(typeof(Attendee.AttendeeMetadata))]
    public partial class Attendee
    {

        // This class allows you to attach custom attributes to properties
        // of the Attendee class.
        //
        // For example, the following marks the Xyz property as a
        // required field and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz;
        internal sealed class AttendeeMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private AttendeeMetadata()
            {
            }

            public Guid AspNetUserId { get; set; }

            public EntityCollection<AttendeeEvent> AttendeeEvents { get; set; }

            public int AttendeeID { get; set; }

            public EntityCollection<AttendeeScheduleTalk> AttendeeScheduleTalks { get; set; }

            public byte[] UserPicture { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies AttendeeEventMetadata as the class
    // that carries additional metadata for the AttendeeEvent class.
    [MetadataTypeAttribute(typeof(AttendeeEvent.AttendeeEventMetadata))]
    public partial class AttendeeEvent
    {

        // This class allows you to attach custom attributes to properties
        // of the AttendeeEvent class.
        //
        // For example, the following marks the Xyz property as a
        // required field and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz;
        internal sealed class AttendeeEventMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private AttendeeEventMetadata()
            {
            }

            public Attendee Attendee { get; set; }

            public int AttendeeEventID { get; set; }

            public int AttendeeID { get; set; }

            [Include]
            public Event Event { get; set; }

            public int EventID { get; set; }

            public bool IsAcknowledged { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies AttendeeScheduleTalkMetadata as the class
    // that carries additional metadata for the AttendeeScheduleTalk class.
    [MetadataTypeAttribute(typeof(AttendeeScheduleTalk.AttendeeScheduleTalkMetadata))]
    public partial class AttendeeScheduleTalk
    {

        // This class allows you to attach custom attributes to properties
        // of the AttendeeScheduleTalk class.
        //
        // For example, the following marks the Xyz property as a
        // required field and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz;
        internal sealed class AttendeeScheduleTalkMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private AttendeeScheduleTalkMetadata()
            {
            }

            public Attendee Attendee { get; set; }

            public int AttendeeID { get; set; }

            public int AttendeeScheduleTalkID { get; set; }

            public Talk Talk { get; set; }

            public int TalkID { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies EventMetadata as the class
    // that carries additional metadata for the Event class.
    [MetadataTypeAttribute(typeof(Event.EventMetadata))]
    public partial class Event
    {

        // This class allows you to attach custom attributes to properties
        // of the Event class.
        //
        // For example, the following marks the Xyz property as a
        // required field and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz;
        internal sealed class EventMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private EventMetadata()
            {
            }

            public EntityCollection<AttendeeEvent> AttendeeEvents { get; set; }

            public string EventDescription { get; set; }

            public DateTime EventEndDate { get; set; }

            public int EventID { get; set; }

            public DateTime EventStartDate { get; set; }

            public string EventTitle { get; set; }

            [Include]
            public EntityCollection<EventTrack> EventTracks { get; set; }

            public string EventVenueName { get; set; }

            public bool IsLive { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies EventTrackMetadata as the class
    // that carries additional metadata for the EventTrack class.
    [MetadataTypeAttribute(typeof(EventTrack.EventTrackMetadata))]
    public partial class EventTrack
    {

        // This class allows you to attach custom attributes to properties
        // of the EventTrack class.
        //
        // For example, the following marks the Xyz property as a
        // required field and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz;
        internal sealed class EventTrackMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private EventTrackMetadata()
            {
            }

            public Event Event { get; set; }

            public int EventID { get; set; }

            public int EventTrackID { get; set; }

            public string EventTrackTitle { get; set; }

            [Include]
            public EntityCollection<Talk> Talks { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies TalkMetadata as the class
    // that carries additional metadata for the Talk class.
    [MetadataTypeAttribute(typeof(Talk.TalkMetadata))]
    public partial class Talk
    {

        // This class allows you to attach custom attributes to properties
        // of the Talk class.
        //
        // For example, the following marks the Xyz property as a
        // required field and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz;
        internal sealed class TalkMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TalkMetadata()
            {
            }

            public EntityCollection<AttendeeScheduleTalk> AttendeeScheduleTalks { get; set; }

            public EventTrack EventTrack { get; set; }

            public int EventTrackID { get; set; }

            public string TalkAbstract { get; set; }

            public DateTime TalkEndTime { get; set; }

            public int TalkID { get; set; }

            public DateTime TalkStartTime { get; set; }

            public string TalkTitle { get; set; }
        }
    }
}

#pragma warning restore 649    // re-enable compiler warnings about unassigned fields