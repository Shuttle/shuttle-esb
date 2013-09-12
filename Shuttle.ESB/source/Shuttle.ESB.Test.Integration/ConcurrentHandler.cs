using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
    public class ConcurrentHandler : IMessageHandler<ConcurrentCommand>
    {
        public void ProcessMessage(HandlerContext<ConcurrentCommand> context)
        {
            Log.Debug(string.Format("[processing message] : index = {0}", context.Message.MessageIndex));

            System.Threading.Thread.Sleep(500);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}