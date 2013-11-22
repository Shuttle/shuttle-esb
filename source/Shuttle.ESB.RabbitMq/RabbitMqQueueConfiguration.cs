using System;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqQueueConfiguration
	{
		public RabbitMqQueueConfiguration(Uri uri, bool isTransactional, bool isDurable, string exchange = null, bool autoDelete = false, bool isExclusive = false)
		{
			Uri = uri;
			IsTransactional = isTransactional;
			IsDurable = isDurable;
			AutoDelete = autoDelete;
			IsExclusive = isExclusive;
			Exchange = exchange ?? string.Empty;
		}

		public Uri Uri { get; private set; }
		public bool IsTransactional { get; set; }
		public bool IsDurable { get; private set; }
		public bool AutoDelete { get; private set; }
		public bool IsExclusive { get; private set; }
		public string Exchange { get; private set; }
	}
}