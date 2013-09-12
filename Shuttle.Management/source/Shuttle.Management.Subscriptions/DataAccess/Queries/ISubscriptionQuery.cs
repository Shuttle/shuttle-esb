using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
    public interface ISubscriptionQuery
    {
        DataTable All(DataSource source);
		DataTable AllUris(DataSource source);
		DataTable MessageTypes(DataSource source, string uri);
		bool HasSubscriptionStructures(DataSource source);
	}
}