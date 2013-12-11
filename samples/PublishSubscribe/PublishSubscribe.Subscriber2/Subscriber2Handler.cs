using System;
using PublishSubscribe.Messages;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace PublishSubscribe.Subscriber2
{
    public class Subscriber2Handler : IMessageHandler<OrderCompletedEvent>
    {
		public void ProcessMessage(HandlerContext<OrderCompletedEvent> context)
    	{
			var comment = string.Format("Handled OrderCompletedEvent on Subscriber2: {0}", context.Message.OrderId);

			ColoredConsole.WriteLine(ConsoleColor.Blue, comment);

			context.Bus.Publish(new WorkDoneEvent
			{
				Comment = comment
			});
			
			context.Bus.SendDeferredReply(DateTime.Now.AddSeconds(5), new WorkDoneEvent
			{
				Comment = "[DEFERRED / Subscriber2] : order id = " + context.Message.OrderId
			});
		}

    	public bool IsReusable
    	{
    		get { return true; }
    	}
    }
}