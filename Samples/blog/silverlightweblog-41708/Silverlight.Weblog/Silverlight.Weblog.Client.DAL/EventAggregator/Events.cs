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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using Microsoft.Practices.Unity;

namespace Silverlight.Weblog.Client.CoreBL.EventAggregator
{
    public class EventService<T>
    {
        public EventService()
        {
            if (!typeof(CompositePresentationEvent<>).IsAssignableFrom(typeof(T)))
                throw new Exception();
        }

        [Dependency]
        public static IEventAggregator EventAggregator { get; set; }

        public static void Subscribe<T, TPayload>(Action<TPayload> callback)
             where T : CompositePresentationEvent<TPayload>
        {
            EventAggregator.GetEvent<T>().Subscribe(callback);
        }

        public static void Unsubscribe<T, TPayload>(Action<TPayload> callback)
             where T : CompositePresentationEvent<TPayload>
        {
            EventAggregator.GetEvent<T>().Unsubscribe(callback);
        }

        public static void Publish<T, TPayload>(TPayload payload)
            where T : CompositePresentationEvent<TPayload>
        {

            EventAggregator.GetEvent<T>().Publish(payload);
        }



    }
}
