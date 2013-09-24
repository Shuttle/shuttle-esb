using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration.ConfigurationFile
{
    public class ConfigurationTestFixture
    {
        protected ServiceBusSection GetServiceBusSection(string file)
        {
            return ServiceBusSection.Open(string.Format(@"..\..\ConfigurationFile\files\{0}", file));
        }
    }
}