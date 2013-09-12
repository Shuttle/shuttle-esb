using System;

namespace Shuttle.ESB.Core
{
	[Serializable]
	public class TransportHeader
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}
}