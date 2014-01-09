using System;
using Shuttle.ESB.Modules;
using log4net;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Log4Net;
using Shuttle.ESB.Core;

namespace RequestResponse.Server
{
	public class ServiceBusHost : IHost, IDisposable
	{
		private static IServiceBus bus;

		public void Start()
		{
			Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(ServiceBusHost))));

			bus = ServiceBus
				.Create()
				.AddEnryptionAlgorithm(new TripleDesEncryptionAlgorithm())
				.AddCompressionAlgorithm(new GZipCompressionAlgorithm())
				.AddModule(new ActiveTimeRangeModule())
				.Start();
		}

		public void Dispose()
		{
			bus.Dispose();

			LogManager.Shutdown();
		}
	}
}