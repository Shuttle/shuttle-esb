using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
	public class SubscriptionQuery : ISubscriptionQuery
	{
		public IDatabaseGateway DatabaseGateway { get; set; }
		public IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }

		public DataTable All(DataSource source)
		{
			return DatabaseGateway.GetDataTableFor(source, SubscriptionQueries.All());
		}

		public DataTable AllUris(DataSource source)
		{
			return DatabaseGateway.GetDataTableFor(source, SubscriptionQueries.AllUris());
		}

		public DataTable MessageTypes(DataSource source, string uri)
		{
			return DatabaseGateway.GetDataTableFor(source, SubscriptionQueries.MessageTypes(uri));
		}

		public bool HasSubscriptionStructures(DataSource source)
		{
			using (DatabaseConnectionFactory.Create(source))
			{
				return DatabaseGateway.GetScalarUsing<int>(source, SubscriptionQueries.HasSubscriptionStructures()) == 1;
			}
		}
	}
}