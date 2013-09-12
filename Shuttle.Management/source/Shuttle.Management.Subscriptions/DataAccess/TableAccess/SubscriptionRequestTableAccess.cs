using System;
using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
    public static class SubscriptionRequestTableAccess
    {
        public const string TableName = "SubscriberMessageTypeRequest";

        public static IQuery Remove(string inboxWorkQueueUri, string messageType)
        {
            return DeleteBuilder
                .Where(SubscriptionRequestColumns.InboxWorkQueueUri).EqualTo(inboxWorkQueueUri)
                .And(SubscriptionRequestColumns.MessageType).EqualTo(messageType)
                .From(TableName);
        }

        public static IQuery Exists(string inboxWorkQueueUri, string messageType)
        {
            return ContainsBuilder
                .Where(SubscriptionRequestColumns.InboxWorkQueueUri).EqualTo(inboxWorkQueueUri)
                .And(SubscriptionRequestColumns.MessageType).EqualTo(messageType)
                .In(TableName);
        }

        public static IQuery Decline(string inboxWorkQueueUri, string messageType, string declinedBy, DateTime declinedDate, string declainedReason)
        {
            return RawQuery.CreateFrom(@"
                    update dbo.{0} set
                        Declined = 1,
                        DeclinedBy = @DeclinedBy,
                        DeclinedReason = @DeclinedReason,
                        DeclinedDate = @DeclinedDate
                    where
                        InboxWorkQueueUri = @InboxWorkQueueUri
                    and
                        MessageType = @MessageType
                ", TableName)
                .AddParameterValue(SubscriptionRequestColumns.DeclinedBy, declinedBy)
                .AddParameterValue(SubscriptionRequestColumns.DeclinedDate, declinedDate)
                .AddParameterValue(SubscriptionRequestColumns.DeclinedReason, declainedReason)
                .AddParameterValue(SubscriptionRequestColumns.InboxWorkQueueUri, inboxWorkQueueUri)
                .AddParameterValue(SubscriptionRequestColumns.MessageType, messageType);
        }
    }
}