using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
    public class ServiceBusSectionFixture
    {
        protected ServiceBusSection GetServiceBusSection(string file)
        {
			return ServiceBusSection.Open(string.Format(@".\ServiceBusSection\files\{0}", file));
        }
    }
}