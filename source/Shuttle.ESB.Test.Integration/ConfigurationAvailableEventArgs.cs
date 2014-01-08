using System;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
	public class ConfigurationAvailableEventArgs : EventArgs
	{
		public ConfigurationAvailableEventArgs(IServiceBusConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IServiceBusConfiguration Configuration { get; private set; }
	}
}