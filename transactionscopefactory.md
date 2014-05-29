---
title: TransactionScopeFactory
layout: api
---
# IServiceBusTransactionScopeFactory

An implementation of the `IServiceBusTransactionScopeFactory` interface is used in the `ReceiveMessagePipeline` by the `TransactionScopeObserver` to create a transaction on around the `OnHandleMessage` pipeline event.

This allows messages to be optionally handled within a `TransactionScope`.

If you do not want to make use of a `TransactionScope` you can disable it through configuration:

``` xml
    <transactionScope
      enabled="true|false"
      isolationLevel="ReadCommitted"
      timeoutSeconds="30" />
```

It is the responsibility of the `IServiceBusTransactionScopeFactory` implementation to return a `NullServiceBusTransactionScope` should the transaction scope not be required.  An example can be viewed in the source for the `DefaultServiceBusTransactionScopeFactory`.


