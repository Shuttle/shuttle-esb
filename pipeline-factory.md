---
title: Pipeline Factory
layout: api
---
# IPipelineFactory

An implementation of the `IPipelineFactory` interface is used to obtain an instance of a pipeline derived from `MessagePipeline`.

Pipelines are re-used and as soon as a `MessagePipeline` is obtained the internal state of the pipeline is cleared.

Although the pipeline factory may be replaced with your own implementation the `DefaultPipelineFactory` implementation should suffice.

## Methods

### GetPipeline

~~~ c#
MessagePipeline GetPipeline<TPipeline>(IServiceBus bus) where TPipeline : MessagePipeline;
~~~

The method will return a new instance of the pipeline if one is not available; else it will return one from the pool of release instances.

### ReleasePipeline

~~~ c#
void ReleasePipeline(MessagePipeline messagePipeline);
~~~

This method inform the factory that the pipeline execution is complete and may be released into the pool of available of instances.