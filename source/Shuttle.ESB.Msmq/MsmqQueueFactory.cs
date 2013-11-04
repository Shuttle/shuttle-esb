using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqQueueFactory : IQueueFactory
	{
		public MsmqConfiguration Configuration { get; private set; }

		public MsmqQueueFactory()
			: this(MsmqConfiguration.Default())
		{
		}

		public MsmqQueueFactory(MsmqConfiguration configuration)
		{
			Configuration = configuration;
		}

		public string Scheme
		{
			get
			{
				return MsmqQueue.SCHEME;
			}
		}

		public IQueue Create(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			var queueConfiguration = Configuration.FindQueueConfiguration(uri);

			return new MsmqQueue(uri, queueConfiguration == null || queueConfiguration.IsTransactional);
		}

		public bool CanCreate(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
