namespace Shuttle.ESB.Core
{
	public static class ServiceBusExtensions
	{
		public static void AttemptInitialization(this object o, IServiceBus bus)
		{
			if (o == null || bus == null)
			{
				return;
			}

			var required = o as IRequireInitialization;

			if (required == null)
			{
				return;
			}

			required.Initialize(bus);
		}
	}
}