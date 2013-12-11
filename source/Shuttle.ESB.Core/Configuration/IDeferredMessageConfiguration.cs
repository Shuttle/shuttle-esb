using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public interface IDeferredMessageConfiguration : IThreadActivityConfiguration
	{
		IDeferredMessageQueue DeferredMessageQueue { get; set; }
	}
}