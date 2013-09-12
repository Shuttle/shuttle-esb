using System.Collections.Generic;
using System.Xml.Serialization;

namespace Shuttle.Management.Shell
{
	[XmlType("dataStores")]
	public class XmlDataStoreCollection : List<DataStore>
	{
	}
}