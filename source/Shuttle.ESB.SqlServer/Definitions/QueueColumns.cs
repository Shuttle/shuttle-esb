using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.ESB.SqlServer
{
    public class QueueColumns
    {
        public static MappedColumn<int> SequenceId = new MappedColumn<int>("SequenceId", DbType.Int32).AsIdentifier();
        public static MappedColumn<Guid> MessageId = new MappedColumn<Guid>("MessageId", DbType.Guid);
        public static MappedColumn<byte[]> MessageBody = new MappedColumn<byte[]>("MessageBody", DbType.Binary);
    }
}