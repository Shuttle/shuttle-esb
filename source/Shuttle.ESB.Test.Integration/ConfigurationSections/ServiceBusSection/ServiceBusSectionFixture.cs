using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
    public class ServiceBusSectionFixture
    {
        protected ServiceBusSection GetServiceBusSection(string file)
        {
			return ServiceBusSection.Open(string.Format(@".\ConfigurationSections\ServiceBusSection\files\{0}", file));
        }
    }
}