using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class ServiceBusExtensions
	{
		public static void AttemptInitialization(this object o, IServiceBus bus)
		{
			if (o.IsNull() || bus.IsNull())
			{
				return;
			}

			var required = o as IRequireInitialization;

			if (required.IsNull())
			{
				return;
			}

			required.Initialize(bus);
		}
	}
}