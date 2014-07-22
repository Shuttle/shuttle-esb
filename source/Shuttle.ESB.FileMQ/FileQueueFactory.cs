using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.FileMQ
{
	public class FileQueueFactory : IQueueFactory
	{
		public string Scheme
		{
			get { return "filemq"; }
		}

		public IQueue Create(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return new FileQueue(uri);
		}

		public bool CanCreate(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			var result = Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);

			Guard.Against<NotSupportedException>(result && !string.IsNullOrEmpty(uri.Host) && !uri.Host.Equals("."),
			                                     string.Format(FileMQResources.HostNotPermittedException, uri.Host));

			return result;
		}
	}
}