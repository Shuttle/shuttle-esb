# Message Idempotence

Idempotence as defined on [WikiPedia](https://en.wikipedia.org/wiki/Idempotence):

> Idempotence is the property of certain operations in mathematics and computer science, that can be applied multiple times without changing the result beyond the initial application

When working with messages this means that no matter how many times we apply a message the outcome should be the same.

Let's use the following message structure:

``` c#
public class DebitAccount
{
    public string AccountNumber { get; set; }
    public decimal Amount { get; set; }
}
```

If we send two messages for account number `A-001` with an amount of `50` the account will be debited twice resulting in a total debit of `100`.  This may not be intended.  We can change the structure to the following:

``` c#
public class SetBalance
{
    public string AccountNumber { get; set; }
    public decimal Amount { get; set; }
}
```

Now if we had the balance before the debit we could apply the change and send the `SetBalance` message.  Having the message processed twice would solve our issue as the balance would be the same.

However, this means that we need to know the balance up front *and* it does not solve the issue of the balance changing between calls as the balance would be overwritten.

We need to find a way to truly make this message idempotent:

``` c#
public class DebitAccount
{
    public Guid TransactionId { get; set; }
    public string AccountNumber { get; set; }
    public decimal Amount { get; set; }
}
```

Adding the `TransactionId` means that we can check whether the transaction has been actioned and, if so, ignore any other messages arriving with the same `TransactionId`.  This means we need to track the `TransactionId` is a persistent data store of sorts.

All messages dispatched with Shuttle.Esb are wrapped in a `TransportMessage` envelope that contains a unique `MessageId`.  Using an implementation of the `IIdempotenceService` will be able to make messages idempotent on a technical level.  Thie means that messages that are duplicated due to edge cases of the `at-least-once` delivery mechanism will not be retried.

Some messages are idempotent by their very nature, as in the case of storing a result rather than applying a change, and if you can design messages to work in this way it would be first prize.  This isn't always possible.  An example of idempotent processing may be the activation of a member on a website.  If you store the `DateActivated` for a member and you receive an `ActivateMember`, perhaps from the member clicking on a link in an activation e-mail, you could set the `DateActivated` to the current date where the `username` is equal to the received value and the `DateActivated` is null.  As soon as the member is activated all subsequent activation requests can be ignored since the member has already been activated.

## Exactly-Once Delivery

This mechanism requires a distributed transaction that includes the queue.  Since not many queues support distrubuted transactions Shuttle.Esb no longer supports this mechanism as from version `3.0.0`.  An issue facing Exactly-Once delivery is that when an endpoint is marked as not requiring/supporting a distributed transaction any messages will be immediately sent

## At-Least-Once Delivery

This the mechanism that Shuttle.Esb uses.  It does, however, imply that in some rare edge cases a technically *duplicate* message can arrive at an endpoint.  You should attempt to design your system in such a way that the message handling remains idempotent.

## At-Most-Once Delivery

This implies that the message will never be processed more than once, but may also not arrive at all.  Shuttle.Esb has never implemented this mechanism.
