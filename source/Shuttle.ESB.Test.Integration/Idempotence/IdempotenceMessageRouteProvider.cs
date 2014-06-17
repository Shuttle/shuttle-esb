using System.Collections.Generic;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration.Idempotence
{
	public class IdempotenceMessageRouteProvider : IMessageRouteProvider
	{
		public IEnumerable<string> GetRouteUris(string messageType)
		{
			return new List<string> { "memory://./idempotence-inbox-work" };
		}
	}
}