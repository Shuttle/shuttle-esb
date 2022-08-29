# Message Handling Assessor

An implementation of the `IMessageHandlerAssessor` interface is used to determine whether a message should be processed.

If you do not specify your own implementation of the `IMessageHandlingAssessor` the `MessageHandlingAssessor` will be used.

## Methods

### RegisterAssessor

``` c#
void RegisterAssessor(Func<PipelineEvent, bool> assessor);
void RegisterAssessor(ISpecification<PipelineEvent> specification);
```

Register either a function or a specification that returns `true` when the message should be processed; else `false` to ignore the message.

When the message is not processed the rest of the pipeline will still complete.