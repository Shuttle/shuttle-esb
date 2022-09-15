# Changelog

## v13.2.0

- Fixed deferred processing.  Although working it would be sluggish.
- Added `DeferredMessageProcessingAdjusted` event to `DeferredMessageProcessor` to indicate when the deferred processing has halted.

## v13.1.0

- Changed all `DateTime.Now` to `DateTime.UtcNow` and the relevant date comparisons use `DateTime.ToUniversalTime()` to ensure the correct timezone.
- Refactored `DeferredMessageProcessor` to enable sharing of deferred queues between endpoints.
- Added `DeferredMessageProcessingHalted` event to `DeferredMessageProcessor` to indicate when the deferred processing has halted.

### Date.UtcNow

Since all `DateTime.Now` calls have been replaced by `DateTime.UtcNow` please test your software before releasing to production to ensure that any bits making use of internal Shuttle.Esb `DateTime` values function as expected.

### DeferredMessageProcessor

Before `v13.1.0` endpoints making use of deferred message processing would require their own deferred queue since each endpoint would track the next time the queue requires processing by checking the smallest `IgnoreTillDate` in each `TransportMessage` in the queue.

From `v13.1.0` this has changed since each endpoint makes use of the `ServiceBusOptions.Inbox.DeferredMessageProcessorResetInterval` TimeSpan value to resume checking the deferred queue.  There is, of course, nothing preventing one from still making use of different deferred queues.