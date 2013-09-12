using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
	public class SubscriptionRequestQuery : ISubscriptionRequestQuery
	{
		public IDatabaseGateway DatabaseGateway { get; set; }
		public IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }

		public DataTable All(DataSource source)
		{
			return DatabaseGateway.GetDataTableFor(source, SubscriptionRequestQueries.All());
		}

		public DataTable AllUris(DataSource source)
		{
			return DatabaseGateway.GetDataTableFor(source,
			                                       SubscriptionRequestQueries.AllUris());
		}

		public DataTable MessageTypes(DataSource source, string uri)
		{
			return DatabaseGateway.GetDataTableFor(source,
			                                       SubscriptionRequestQueries.MessageTypes(uri));
		}

		public bool HasSubscriptionRequestStructures(DataSource source)
		{
			using (DatabaseConnectionFactory.Create(source))
			{
				return
					DatabaseGateway.GetScalarUsing<int>(source,
					                                    SubscriptionRequestQueries.HasSubscriptionRequestStructures()) == 1;
			}
		}
	}
}