using System;
using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
    public static class SubscriptionTableAccess
    {
        public const string TableName = "SubscriberMessageType";

        public static IQuery Add(string inboxWorkQueueUri, string messageType, string acceptedBy)
        {
            return InsertBuilder.Insert()
                .Add(SubscriptionColumns.InboxWorkQueueUri).WithValue(inboxWorkQueueUri)
                .Add(SubscriptionColumns.MessageType).WithValue(messageType)
                .Add(SubscriptionColumns.AcceptedBy).WithValue(acceptedBy)
                .Add(SubscriptionColumns.AcceptedDate).WithValue(DateTime.Now)
                .Into(TableName);
        }

        public static IQuery Remove(string inboxWorkQueueUri, string messageType)
        {
            return DeleteBuilder
                .Where(SubscriptionColumns.InboxWorkQueueUri).EqualTo(inboxWorkQueueUri)
                .And(SubscriptionColumns.MessageType).EqualTo(messageType)
                .From(TableName);
        }

        public static IQuery Exists(string inboxWorkQueueUri, string messageType)
        {
            return ContainsBuilder
                .Where(SubscriptionColumns.InboxWorkQueueUri).EqualTo(inboxWorkQueueUri)
                .And(SubscriptionColumns.MessageType).EqualTo(messageType)
                .In(TableName);
        }
    }
}