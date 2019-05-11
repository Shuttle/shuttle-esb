---
title: Idempotence Concepts
layout: api
---
# Message Idempotence

Idempotence as defined on [WikiPedia](https://en.wikipedia.org/wiki/Idempotence):

> Idempotence is the property of certain operations in mathematics and computer science, that can be applied multiple times without changing the result beyond the initial application

When working with messages this means that no matter how many times we apply a message the outcome should be the same.

Let's use the following message structure:

``` c#
public class DebitAccountCommand
{
    public string AccountNumber { get; set; }
    public decimal Amouunt { get; set; }
}
```

If we send two messages for account number 'A-001' with an amount of '50' the account will be debited with an amount of 100.  This may not be intended.  We can change the structure to the following:

``` c#
public class SetBalanceCommand
{
    public string AccountNumber { get; set; }
    public decimal Amouunt { get; set; }
}
```

Now if we had the balance before the debit we could apply the change and send the `SetBalanceCommand` message.  Having the message processed twice would solve our issue as the balance would be the same.

However, this means that we need to know the balance up front *and* it does not solve the issue of the balance changing between calls.

We need to find a way to truly make this message idempotent:

``` c#
public class DebitAccountCommand
{
    public Guid TransactionId { get; set; }
    public string AccountNumber { get; set; }
    public decimal Amouunt { get; set; }
}
```

Adding the `TransactionId` means that we can check whether the transaction has been actioned and, if so, ignore any axtra messages.  It means that we have to keep track of this content though.

All messages dispatched with Shuttle.Esb are wrapped in a [TransactionMessage] envelope that contains a unique `MessageId`.  Using an implementation of the `IIdempotenceService` will be able to make messages idempotent on a technical level.  Thie means that messages that are duplicated due to edge cases of the `at-least-once` delivery mechanism will not be retried.

Some messages are idempotent by their very nature anbd if you can design messages to work in this way it would be first prize.  It, however, would not always be possible.  Take the example of activating a member on a website.  If you store the `DateActivated` for a member and you receive an `ActivateMemberCommand`, perhaps from the member clicking on a link in an activation e-mail, you could set the `DateActivated` to the current date where the `UserName` is equal to the received value and the `DateActivated` is null.  As soon as the member is activated all subsequent activation request will essentially be ignored.

## Exactly-Once Delivery

This mechanism requires a distributed transaction that includes the queue.  Since not many queues support distrubuted transactions Shuttle.Esb no longer supports this mechanism from version 3.0.0.  An issue facing Exactly-Once delivery is that when an endpoint is marked as not requiring/supporting a transaction any sent messages will be immediately sent:

![Non-Transactional Image]({{ "/assets/images/idempotence-eo-non-txn.png" | resolver_url }} "Non-Transactional")

When using transactions this problem is solved:

![Transactional Image]({{ "/assets/images/idempotence-eo-txn.png" | resolver_url }} "Transactional")

## At-Least-Once Delivery

Shuttle.Esb supports *at-least-once* delivery when not making use of an `IIdempotenceService` implementation.  This means that in some rare edge cases a trachnically duplicate message can arrive at an endpoint.  You should attempt to design you system in such a way that the message handling remains idempotent.

![No Idempotence Service]({{ "/assets/images/idempotence-alo.png" | resolver_url }} "No Idempotence Service")

## At-Most-Once Delivery

Shuttle.Esb supports *at-most-once* delivery when making use of an `IIdempotenceService` implementation.  This means that in some rare edge cases a trachnically duplicate message can arrive at an endpoint.  You should attempt to design you system in such a way that the message handling remains idempotent.

![Idempotence Service]({{ "/assets/images/idempotence-amo.png" | resolver_url }} "Idempotence Service")

In addition to message de-duplication, any messages dispatched from within a message handler will be persisted and only sent after the message has been successfully processed.  Should there be a failure before the deferred messages have been sent but after processing the next time the message is processed the processing (handler) will be skipped and onyl the messages will be sent.

[TransportMessage]: {{ "/transport-message" | resolver_url }}

