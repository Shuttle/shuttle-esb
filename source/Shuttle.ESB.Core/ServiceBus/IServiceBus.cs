using System;

namespace Shuttle.ESB.Core
{
	public interface IServiceBus : 
		IMessageSender, 
		IDisposable
	{
		IServiceBus Start();
		void Stop();

		bool Started { get; }

		IServiceBusConfiguration Configuration { get; }
		IServiceBusEvents Events { get; }
	}
}