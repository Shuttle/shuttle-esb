using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
	public interface IMessageRouteProvider
	{
		IEnumerable<string> GetRouteUris(object message);	
	}
}