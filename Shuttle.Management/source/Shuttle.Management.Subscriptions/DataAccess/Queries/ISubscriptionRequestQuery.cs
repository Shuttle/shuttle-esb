using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
	public interface ISubscriptionRequestQuery
	{
		DataTable All(DataSource source);
		DataTable AllUris(DataSource source);
		DataTable MessageTypes(DataSource source, string uri);
		bool HasSubscriptionRequestStructures(DataSource source);
	}
}