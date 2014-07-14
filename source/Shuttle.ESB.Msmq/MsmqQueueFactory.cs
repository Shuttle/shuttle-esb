using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqQueueFactory : IQueueFactory
	{
		public IMsmqConfiguration Configuration { get; private set; }

		public MsmqQueueFactory()
			: this(MsmqConfiguration.Default())
		{
		}

		public MsmqQueueFactory(IMsmqConfiguration configuration)
		{
			Configuration = configuration;
		}

		public string Scheme
		{
			get
			{
				return MsmqUriParser.Scheme;
			}
		}

		public IQueue Create(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return new MsmqQueue(uri, Configuration);
		}

		public bool CanCreate(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
