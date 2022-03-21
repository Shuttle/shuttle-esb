# Service Bus Policy

An implementation of the `IServiceBusPolicy` interface is used to evaluate failures and determine whether the failure should be retried.

## Methods

### EvaluateMessageHandlingFailure

``` c#
MessageFailureAction EvaluateMessageHandlingFailure(OnPipelineException pipelineEvent);
```

### EvaluateMessageDistributionFailure

``` c#
MessageFailureAction EvaluateMessageDistributionFailure(OnPipelineException pipelineEvent);
```

### EvaluateOutboxFailure

``` c#
MessageFailureAction EvaluateOutboxFailure(OnPipelineException pipelineEvent);
```
