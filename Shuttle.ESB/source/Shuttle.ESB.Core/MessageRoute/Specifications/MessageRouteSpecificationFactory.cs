namespace Shuttle.ESB.Core
{
    internal class MessageRouteSpecificationFactory
    {
        public IMessageRouteSpecification Create(string specification, string value)
        {
            switch (specification.ToLower())
            {
                case "startswith":
                {
                    return new StartsWithMessageRouteSpecification(value);
                }
                case "typelist":
                {
                    return new TypeListMessageRouteSpecification(value);
                }
                case "regex":
                {
                    return new RegexMessageRouteSpecification(value);
                }
                case "assembly":
                {
                    return new AssemblyMessageRouteSpecification(value);
                }
            }

            throw new MessageRouteSpecificationException(string.Format(ESBResources.UnknownMessageRouteSpecification, specification));
        }
    }
}