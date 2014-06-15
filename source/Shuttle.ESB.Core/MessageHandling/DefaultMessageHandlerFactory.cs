using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DefaultMessageHandlerFactory : MessageHandlerFactory
    {
        private readonly Dictionary<Type, Type> _messageHandlerTypes = new Dictionary<Type, Type>();

        private readonly ILog _log;

        public DefaultMessageHandlerFactory()
        {
            _log = Log.For(this);
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
            if (_messageHandlerTypes.ContainsKey(messageType))
            {
                return;
            }

            _messageHandlerTypes.Add(messageType, messageHandlerType);

            _log.Information(string.Format("[add message handler] : message type = '{0}' / handler type = '{1}' ", messageType.FullName, messageHandlerType.FullName));
        }

        public override IMessageHandler CreateHandler(object message)
        {
            var messageType = message.GetType();

            if (_messageHandlerTypes.ContainsKey(messageType))
            {
                return (IMessageHandler)Activator.CreateInstance(_messageHandlerTypes[messageType]);
            }

            return null;
        }

        public override IEnumerable<Type> MessageTypesHandled
        {
            get { return _messageHandlerTypes.Keys; }
        }

        public override void Initialize(IServiceBus bus)
        {
            _messageHandlerTypes.Clear();

	        var reflectionService = new ReflectionService();

	        foreach (var type in reflectionService.GetTypes(typeof(IMessageHandler<>)))
	        {
				if (type.GetConstructor(Type.EmptyTypes) != null)
				{
					AddMessageHandlerType(type);
				}
				else
				{
					_log.Warning(string.Format(ESBResources.DefaultMessageHandlerFactoryNoDefaultConstructor, type.FullName));
				}
			}
        }
    }
}