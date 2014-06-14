namespace Shuttle.ESB.Core
{
    public interface IPipelineFactory
    {
    	MessagePipeline GetPipeline<TPipeline>(IServiceBus bus) where TPipeline : MessagePipeline;
    	void ReleasePipeline(MessagePipeline messagePipeline);
    }
}