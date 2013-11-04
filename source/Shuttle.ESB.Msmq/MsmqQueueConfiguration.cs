using System;

namespace Shuttle.ESB.Msmq
{
	public class MsmqQueueConfiguration
	{
		public MsmqQueueConfiguration(Uri uri, bool isTransactional)
		{
			Uri = uri;
			IsTransactional = isTransactional;
		}

		public Uri Uri { get; private set; }
		public bool IsTransactional { get; set; }
	}
}