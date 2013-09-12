using Shuttle.ESB.Core;

namespace Shuttle.Management.Messages
{
	public interface IMessageConfiguration
	{
		string SerializerType { get; }

		ISerializer GetSerializer();
	}
}