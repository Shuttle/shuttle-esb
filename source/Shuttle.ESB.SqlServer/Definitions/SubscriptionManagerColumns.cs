using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.ESB.SqlServer
{
    public class SubscriptionManagerColumns
    {
        public static MappedColumn<string> InboxWorkQueueUri = new MappedColumn<string>("InboxWorkQueueUri", DbType.AnsiString, 265);
        public static MappedColumn<string> MessageType = new MappedColumn<string>("MessageType", DbType.AnsiString, 265);
    }
}