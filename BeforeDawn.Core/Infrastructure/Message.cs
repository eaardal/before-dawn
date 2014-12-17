using System;

namespace BeforeDawn.Core.Infrastructure
{
    public delegate void MessageDistributorEventHandler<in TMessage>(TMessage e);

    public class Message
    {
        public static void Publish<TMessage>(TMessage message)
        {
            MessageDistributor<TMessage>.Publish(message);
        }

        public static void Subscribe<TMessage>(Action<TMessage> message)
        {
            MessageDistributor<TMessage>.MessageSent += message.Invoke;
        }

        public static void UnSubscribe<TMessage>(Action<TMessage> message)
        {
            MessageDistributor<TMessage>.MessageSent -= message.Invoke;
        }

        public static void ClearAllSubscriptionsOfType<TMessage>()
        {
            MessageDistributor<TMessage>.ClearAllSubscriptions();
        }

        private class MessageDistributor<TMessage>
        {
            public static event MessageDistributorEventHandler<TMessage> MessageSent;

            public static void ClearAllSubscriptions()
            {
                MessageSent = null;
            }

            public static void Publish(TMessage message)
            {
                if (MessageSent != null)
                    MessageSent(message);
            }
        }
    }
}
