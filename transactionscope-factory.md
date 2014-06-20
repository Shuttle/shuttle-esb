---
title: TransactionScope Factory
layout: api
---
# ITransactionScopeFactory

An implementation of the `ITransactionScopeFactory` interface is used in the `ReceiveMessagePipeline` by the `TransactionScopeObserver` to create a transaction on around the `OnHandleMessage` pipeline event.

This allows messages to be optionally handled within a `TransactionScope`.

If you do not want to make use of a `TransactionScope` you can disable it through configuration:

``` xml
    <transactionScope
      enabled="true|false"
      isolationLevel="ReadCommitted"
      timeoutSeconds="30" />
```

It is the responsibility of the `ITransactionScopeFactory` implementation to return a `NullServiceBusTransactionScope` should the transaction scope not be required.  An example can be viewed in the source for the `DefaultServiceBusTransactionScopeFactory`.

## Methods

### Create

``` c#
ITransactionScope Create(PipelineEvent pipelineEvent)
```

The method returns a new `ITransactionScope` instance.

