namespace Shuttle.ESB.Core
{
    public interface IPipelineFactory
    {
    	TPipeline GetPipeline<TPipeline>(IServiceBus bus) where TPipeline : MessagePipeline;
    	void ReleasePipeline(MessagePipeline messagePipeline);
    }
}