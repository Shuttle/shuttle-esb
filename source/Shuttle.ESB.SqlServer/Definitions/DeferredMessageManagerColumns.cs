using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.ESB.SqlServer
{
	public class DeferredMessageManagerColumns
    {
		public static MappedColumn<DateTime> DeferTillDate = new MappedColumn<DateTime>("DeferTillDate", DbType.DateTime);
		public static MappedColumn<byte[]> MessageBody = new MappedColumn<byte[]>("MessageBody", DbType.Binary);
	}
}