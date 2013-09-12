using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.Core.Host;

namespace RequestResponse.Worker
{
    public class ServiceBusHost : IHost, IDisposable
    {
        private IServiceBus bus;

        public void Dispose()
        {
            bus.Dispose();
        }

        public void Start()
        {
			Log.Assign(new ConsoleLog(typeof(ServiceBusHost)) { LogLevel = LogLevel.Trace });

            bus = ServiceBus
				.Create()
                .AddEnryptionAlgorithm(new TripleDesEncryptionAlgorithm())
                .AddCompressionAlgorithm(new GZipCompressionAlgorithm())
				.Start();
        }
    }
}