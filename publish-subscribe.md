---
title: Publish/Subscribe
layout: api
---
# Concepts

When you `Send` a *command* shuttle needs to be able to determine the relevant endpoint.  The same applies to publishing an event.  Shuttle would need to know where to `Publish` the event to.  When sending a command there should be 1, and exactly 1, endpoint that receives the command.  When publishing, however, there can be 0 to any number of subscribers.

![Publish/Subscribe Image]({{ site.baseurl }}/assets/images/Architecture-PublishSubscribe.png "Publish/Subscribe")

In order to register an endpoint as a subscriber you can either manually configure the subscription store, as recommended for production, or register the subscription using the `ISubscriptionManager` implementation:

``` c#
    var subscriptionManager = SubscriptionManager.Default();

	// using type
    subscriptionManager.Subscribe(typeof(Event1));
    subscriptionManager.Subscribe(typeof(Event2));
	
	// using a list of types
    subscriptionManager.Subscribe(new[] { typeof(Event1), typeof(Event2) });
	
	// using a full type name
    subscriptionManager.Subscribe(typeof(Event1).FullName);
    subscriptionManager.Subscribe(typeof(Event2).FullName);
	
	// using a list of full type names
    subscriptionManager.Subscribe(new[] { typeof(Event1).FullName, typeof(Event2).FullName });
	
	// using a generic
	subscriptionManager.Subscribe<Event1>();
	subscriptionManager.Subscribe<Event2>();

	var bus = ServiceBus
		.Create(c => c.SubscriptionManager(subscriptionManager))
		.Start();
```

In a production environment it is recommended that the subscription store be maintained manually using an elevated identity.  For the above one could use an identity that has **read-only** permissions.  The `Subscribe` method will fail if the subscription does not exist.  In this way one can ensure that the subscription is not missing from the relevant store.

# Shuttle Configuration

All endpoints that belong to the same physical pub/sub store should connect to the same store.  

You would have a store for your development environment, perhaps even locally on your own machine.  You would have a separate store for your QA, UAT, and production environments.

## Publishing from a web-site

Typically you would publish events from some processing endpoint, as opposed to a web-site.

That being said, there is nothing stopping you from publishing a message from a web-site.  The idea behind publishing a message is that the message represents an event that is typically produced by some *processing* endpoint.  Since a web application should not really be processing anything but rather be *sending* commands off to a processing endpoint it would stand to reason that a web application should not be publishing events.

So if you find yourself in a situation where it seems to make sense to publish from your web application it indicates that you need to make a design decision: either the design is not optimal (so the web application is performing processing) *or* you are constrained in some way the prohibits the implementation of a processing endpoint (maybe a shared hosting environment).
