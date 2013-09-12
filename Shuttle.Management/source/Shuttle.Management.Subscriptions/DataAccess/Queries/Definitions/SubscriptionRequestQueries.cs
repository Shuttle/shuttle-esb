using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
	public static class SubscriptionRequestQueries
	{
		public const string TableName = "SubscriberMessageTypeRequest";

		public static IQuery All()
		{
			return SelectBuilder
				.Select(SubscriptionRequestColumns.InboxWorkQueueUri)
				.With(SubscriptionRequestColumns.MessageType)
				.With(SubscriptionRequestColumns.Declined)
				.With(SubscriptionRequestColumns.DeclinedBy)
				.With(SubscriptionRequestColumns.DeclinedDate)
				.With(SubscriptionRequestColumns.DeclinedReason)
				.OrderBy(SubscriptionRequestColumns.InboxWorkQueueUri).Ascending()
				.From(TableName);
		}

		public static IQuery AllUris()
		{
			return RawQuery.CreateFrom("select distinct InboxWorkQueueUri from {0}", TableName);
		}

		public static IQuery MessageTypes(string uri)
		{
			return SelectBuilder
				.Select(SubscriptionRequestColumns.MessageType)
				.With(SubscriptionRequestColumns.Declined)
				.With(SubscriptionRequestColumns.DeclinedBy)
				.With(SubscriptionRequestColumns.DeclinedDate)
				.With(SubscriptionRequestColumns.DeclinedReason)
				.Where(SubscriptionRequestColumns.InboxWorkQueueUri).EqualTo(uri)
				.OrderBy(SubscriptionRequestColumns.MessageType).Ascending()
				.From(TableName);
		}

		public static IQuery HasSubscriptionRequestStructures()
		{
			return
				RawQuery.CreateFrom(
					"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'SubscriberMessageRequestType') select 1 ELSE select 0");
		}
	}
}