using System.Collections.Generic;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration.Idempotence
{
	public class IdempotenceMessageRouteProvider : IMessageRouteProvider
	{
		public IEnumerable<string> GetRouteUris(object message)
		{
			return new List<string> { "memory://./idempotence-inbox-work" };
		}
	}
}