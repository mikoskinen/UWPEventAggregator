using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace UWPEventAggregator
{
    public class EventAggregator
    {
        private static List<Tuple<CoreDispatcher, object>> subscribers = new List<Tuple<CoreDispatcher, object>>();

        public void Subscribe<TMessage>(ISubscriber<TMessage> subscriber)
        {
            subscribers.Add(new Tuple<CoreDispatcher, object>(Window.Current.Dispatcher, subscriber));
        }

        public void Publish<TMessage>(TMessage message)
        {
            var messageType = GetEventType(message);

            foreach (var subscriber in subscribers)
            {
                var handler = subscriber.Item2;

                if (messageType.IsInstanceOfType(handler))
                {
                    var dispatcher = subscriber.Item1;
                    dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        ((ISubscriber<TMessage>)handler).HandleMessage(message);
                    });
                }
            }
        }

        private static Type GetEventType<T>(T args)
        {
            return typeof(ISubscriber<>).MakeGenericType(args.GetType());
        }
    }
}
