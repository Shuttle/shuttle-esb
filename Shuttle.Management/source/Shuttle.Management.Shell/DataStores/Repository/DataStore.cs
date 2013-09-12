using System;
using System.Xml.Serialization;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	[XmlType("dataStore")]
	public class DataStore : IComparable<DataStore>
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("secure")]
		public bool Secure { get; set; }

		[XmlAttribute("connectionString")]
		public string ConnectionString { get; set; }

		[XmlAttribute("providerName")]
		public string ProviderName { get; set; }

		public int CompareTo(DataStore other)
		{
			Guard.AgainstNull(other, "other");

			return Name.CompareTo(other.Name);
		}
	}
}