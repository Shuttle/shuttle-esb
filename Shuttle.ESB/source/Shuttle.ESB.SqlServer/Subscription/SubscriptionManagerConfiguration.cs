using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.SqlServer
{
    public class SubscriptionManagerConfiguration : ISubscriptionManagerConfiguration
    {
        public SubscriptionManagerConfiguration()
        {
            Secured = ConfigurationItem<bool>.ReadSetting("SubscriptionManagerSecured", true).GetValue();

            Log.For(this).Warning(string.Format(SqlResources.SubscriptionManagerSecured, Secured));
        }


        public bool Secured { get; private set; }
    }
}