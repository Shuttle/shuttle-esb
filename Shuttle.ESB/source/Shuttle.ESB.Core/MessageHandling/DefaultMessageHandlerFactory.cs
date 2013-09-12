using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DefaultMessageHandlerFactory : MessageHandlerFactory
    {
        private readonly Dictionary<Type, Type> messageHandlerTypes = new Dictionary<Type, Type>();

        private readonly ILog log;

        public DefaultMessageHandlerFactory()
        {
            log = Log.For(this);
        }

        private void AddMessageHandlerType(Type messageHandlerType)
        {
            var messageHandler = typeof(IMessageHandler<>);

            foreach (var type in messageHandlerType.GetInterfaces())
            {
                if (!type.IsGenericType || type.GetGenericTypeDefinition() != messageHandler)
                {
                    continue;
                }

                AddMessageTypeHandler(type.GetGenericArguments()[0], messageHandlerType);
            }
        }

        private void AddMessageTypeHandler(Type messageType, Type messageHandlerType)
        {
            if (messageHandlerTypes.ContainsKey(messageType))
            {
                return;
            }

            messageHandlerTypes.Add(messageType, messageHandlerType);

            log.Information(string.Format("[add message handler] : message type = '{0}' / handler type = '{1}' ", messageType.FullName, messageHandlerType.FullName));
        }

        public override IMessageHandler CreateHandler(object message)
        {
            var messageType = message.GetType();

            if (messageHandlerTypes.ContainsKey(messageType))
            {
                return (IMessageHandler)Activator.CreateInstance(messageHandlerTypes[messageType]);
            }

            return null;
        }

        public override IEnumerable<Type> MessageTypesHandled
        {
            get { return messageHandlerTypes.Keys; }
        }

        public override void Initialize(IServiceBus bus)
        {
            messageHandlerTypes.Clear();

            new ReflectionService().GetTypes(typeof(IMessageHandler<>)).ForEach(
                type =>
                {
                    if (type.HasDefaultConstructor())
                    {
                        AddMessageHandlerType(type);
                    }
                });
        }
    }
}