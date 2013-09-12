using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
    public static class SubscriptionQueries
    {
        public const string TableName = "SubscriberMessageType";

        public static IQuery All()
        {
            return SelectBuilder
                .Select(SubscriptionColumns.InboxWorkQueueUri)
                .With(SubscriptionColumns.MessageType)
                .With(SubscriptionColumns.AcceptedBy)
                .With(SubscriptionColumns.AcceptedDate)
                .OrderBy(SubscriptionColumns.InboxWorkQueueUri).Ascending()
                .From(TableName);
        }

        public static IQuery AllUris()
        {
            return RawQuery.CreateFrom("select distinct InboxWorkQueueUri from {0}", TableName);
        }

        public static IQuery MessageTypes(string uri)
        {
            return SelectBuilder
                .Select(SubscriptionColumns.MessageType)
				.With(SubscriptionColumns.AcceptedBy)
				.With(SubscriptionColumns.AcceptedDate)
				.Where(SubscriptionColumns.InboxWorkQueueUri).EqualTo(uri)
                .OrderBy(SubscriptionColumns.MessageType).Ascending()
                .From(TableName);
        }

		public static IQuery HasSubscriptionStructures()
		{
			return
				RawQuery.CreateFrom(
					"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'SubscriberMessageType') select 1 ELSE select 0");
		}
	}
}