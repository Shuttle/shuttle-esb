---
title: Shuttle-ESB API
layout: api
---
# ServiceBus

There are basically three ways to communicate from one endpoint to another:

- sending **command** messages for processing
- sending **command** messages for *deferred* processing
- publishing **event** messages

A **command** message is only ever sent to **one** endpoint and at least **one** endpoint is required for the message to be successfully sent.  This requirement stems from a **command** being an instruction that will result in an action so it is something that is still going to take place.  If there is no endpoint to send a **command** to the instruction will not be executed whilst there is an expectation that the action would be executed.

An **event**, on the other hand, is something that has happened typically as a result of a **command** but not always.  There may be some other state that is noticed by the system that may require an **event** being published.  An event may have **zero** or **more** subscribers since there is no requirement for anyone to be interested in the event.  Typically when a business **event** is defined there should be at least one interested subscriber; else the **event** would not exist.  There may be some infrastructure-related eventst that may not always have a subscriber but it may be worthwhile publising the event anyway since it is something interesting that has occurred.

Some examples of *commands* and *events*:

| Command 					| Event 					|
| ---						| ---						|
| `SendEMailCommand`		| `EMailSentEvent`			|
| `CreateCustomerCommand`	| `CustomerCreatedEvent`	|
| `PlaceOrderCommand`		| `OrderPlacedEvent`		|
| `CancelOrderCommand`		| `OrderCancelledEvent`		|

## Sending **command** messages


## Sending deferred **command** messages

## Publishing **event** messages

# ServiceBusConfiguration

TBD