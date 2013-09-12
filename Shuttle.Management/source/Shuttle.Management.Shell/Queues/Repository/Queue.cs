using System;
using System.Xml.Serialization;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	[XmlType("queue")]
    public class Queue : IComparable<Queue>
    {
		[XmlAttribute("uri")]
        public string Uri { get; set; }

		public int CompareTo(Queue other)
		{
			Guard.AgainstNull(other,"other");

			return Uri.CompareTo(other.Uri);
		}
    }
}