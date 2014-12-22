using System;

namespace BeforeDawn.Core.Infrastructure
{
    public interface IMessageBus
    {
        void Publish<TMessage>(TMessage message);
        void Subscribe<TMessage>(Action<TMessage> message);
        void UnSubscribe<TMessage>(Action<TMessage> message);
        void ClearAllSubscriptionsOfType<TMessage>();
    }

    public class MessageBus : IMessageBus
    {
        public delegate void MessageDistributorEventHandler<in TMessage>(TMessage e);

        public void Publish<TMessage>(TMessage message)
        {
            MessageDistributor<TMessage>.Publish(message);
        }

        public void Subscribe<TMessage>(Action<TMessage> message)
        {
            MessageDistributor<TMessage>.MessageSent += message.Invoke;
        }

        public void UnSubscribe<TMessage>(Action<TMessage> message)
        {
            MessageDistributor<TMessage>.MessageSent -= message.Invoke;
        }

        public void ClearAllSubscriptionsOfType<TMessage>()
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
