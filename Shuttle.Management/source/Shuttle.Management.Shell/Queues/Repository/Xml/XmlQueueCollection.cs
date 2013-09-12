using System.Collections.Generic;
using System.Xml.Serialization;

namespace Shuttle.Management.Shell
{
	[XmlType("queues")]
	public class XmlQueueCollection : List<Queue>
	{
	}
}