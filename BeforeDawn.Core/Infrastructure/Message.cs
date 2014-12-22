using System;

namespace BeforeDawn.Core.Infrastructure
{
    public class Message
    {
        private static readonly IMessageBus MessageBus;

        static Message()
        {
            MessageBus = new MessageBus();
        }

        public static void Publish<TMessage>(TMessage message)
        {
            MessageBus.Publish(message);
        }

        public static void Subscribe<TMessage>(Action<TMessage> message)
        {
            MessageBus.Subscribe(message);
        }

        public static void UnSubscribe<TMessage>(Action<TMessage> message)
        {
            MessageBus.UnSubscribe(message);
        }

        public static void ClearAllSubscriptionsOfType<TMessage>()
        {
            MessageBus.ClearAllSubscriptionsOfType<TMessage>();
        }
    }
}
