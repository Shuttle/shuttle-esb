namespace Shuttle.ESB.Core
{
	public interface IMessageHandler
    {
		bool IsReusable { get; }
    }

	public interface IMessageHandler<T> : IMessageHandler where T : class
	{
		void ProcessMessage(HandlerContext<T> context);
	}
}