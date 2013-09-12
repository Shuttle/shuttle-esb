using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class TypeListMessageRouteSpecification : IMessageRouteSpecification
    {
        protected readonly List<Type> types = new List<Type>();

        public TypeListMessageRouteSpecification(params Type[] types)
            : this((IEnumerable<Type>)types)
        {
        }

        public TypeListMessageRouteSpecification(IEnumerable<Type> types)
        {
            Guard.AgainstNull(types, "types");

            this.types.AddRange(types);
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

                types.Add(type);
            }
        }

        public bool IsSatisfiedBy(object message)
        {
            Guard.AgainstNull(message, "message");

            return types.Contains(message.GetType());
        }
    }
}