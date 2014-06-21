namespace Shuttle.ESB.Core
{
	public interface IWorkQueueConfiguration : IThreadCount
	{
		IQueue WorkQueue { get; }
	}
}