using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Management.Subscriptions
{
    public static class SubscriptionRequestColumns
    {
        public static readonly MappedColumn<string> InboxWorkQueueUri = new MappedColumn<string>("InboxWorkQueueUri",
                                                                                                 DbType.AnsiString, 130);

        public static readonly MappedColumn<string> MessageType = new MappedColumn<string>("MessageType",
                                                                                           DbType.AnsiString, 250);
		public static readonly MappedColumn<int> Declined = new MappedColumn<int>("Declined", DbType.Int32);
		public static readonly MappedColumn<string> DeclinedBy = new MappedColumn<string>("DeclinedBy", DbType.AnsiString, 250);
		public static readonly MappedColumn<string> DeclinedReason = new MappedColumn<string>("DeclinedReason", DbType.AnsiString, 1500);
		public static readonly MappedColumn<DateTime?> DeclinedDate = new MappedColumn<DateTime?>("DeclinedDate", DbType.DateTime);
	}
}