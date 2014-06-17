using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class TypeListMessageRouteSpecification : IMessageRouteSpecification
    {
        protected readonly List<string> _messageTypes = new List<string>();

        public TypeListMessageRouteSpecification(params string[] messageTypes)
            : this((IEnumerable<string>)messageTypes)
        {
        }

		public TypeListMessageRouteSpecification(IEnumerable<string> messageTypes)
        {
            Guard.AgainstNull(messageTypes, "messageTypes");

            _messageTypes.AddRange(_messageTypes);
        }

        public TypeListMessageRouteSpecification(string value)
        {
            Guard.AgainstNullOrEmptyString(value, "value");

            var typeNames = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var typeName in typeNames)
            {
                Type type = null;

                try
                {
                    type = Type.GetType(typeName);
                }
                catch
                {
                }

                if (type == null)
                {
                    throw new MessageRouteSpecificationException(string.Format(ESBResources.TypeListMessageRouteSpecificationUnknownType, typeName));
                }

                _messageTypes.Add(type.FullName);
            }
        }

        public bool IsSatisfiedBy(string messageType)
        {
            Guard.AgainstNull(messageType, "message");

            return _messageTypes.Contains(messageType);
        }
    }
}