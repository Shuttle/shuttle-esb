namespace Shuttle.ESB.Test.Shared
{
	public class IdempotenceCounter
	{
		private readonly object padlock = new object();

		public int ProcessedCount { get; private set; }

		public void Processed()
		{
			lock (padlock)
			{
				ProcessedCount++;
			}
		}
	}
}