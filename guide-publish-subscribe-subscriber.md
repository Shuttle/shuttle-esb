---
title: Publish / Subscribe Guide - Subscriber
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-publish-subscribe.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-publish-subscribe-subscriber'</script>
# Subscriber

> Add a new `Class Library` to the solution called `Shuttle.PublishSubscribe.Subscriber`.

![Publish/Subscribe Subscriber]({{ site.baseurl }}/assets/images/guide-publish-subscribe-subscriber.png "Publish/Subscribe Subscriber")

> Install **both** the `shuttle-esb-msmq` and `shuttle-esb-sqlsubscriber` nuget packages.

![Publish/Subscribe Subscriber - Nuget Msmq/Sql]({{ site.baseurl }}/assets/images/guide-publish-subscribe-subscriber-nuget-msmq-sql.png "Publish/Subscribe Subscriber - Nuget Msmq/Sql")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.  You are also including the **SqlSubscriber** implementation for the `ISubscriptionManager`.

> Install the `shuttle-core-host` nuget package.

![Publish/Subscribe Subscriber - Nuget Host]({{ site.baseurl }}/assets/images/guide-publish-subscribe-subscriber-nuget-host.png "Publish/Subscribe Subscriber - Nuget Host")

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

## Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

``` c#
using System;
using Shuttle.Core.Host;
using Shuttle.ESB.Core;
using Shuttle.ESB.SqlServer;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Subscriber
{
	public class Host : IHost, IDisposable
	{
		private IServiceBus _bus;

		public void Start()
		{
			var subscriptionManager = SubscriptionManager.Default();

			subscriptionManager.Subscribe<MemberRegisteredEvent>();

			_bus = ServiceBus.Create(c => c.SubscriptionManager(subscriptionManager)).Start();
		}

		public void Dispose()
		{
			_bus.Dispose();
		}
	}
}
```

Here we register the subscription by calling the `subscriptionManager.Subscribe<MemberRegisteredEvent>();` method.  Since we a re using the Sql Server implementation of the `ISubscriptionManager` interface an entry will be created in the **SubscriberMessageType** table associating the inbox work queue uri with the message type.

It is important to note that in a production environment one would not typically register subscriptions in this manner as they may be somewhat more sensitive as we do not want any arbitrary subscriber listening in on the messages being published.  For this reason the connection string should be read-only and the subscription should be registered manually or via a deployment script.  Should the subscription **not** yet exist the creation of the subscription will fail, indicating that there is work to be done.

## App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
	</configSections>

	<connectionStrings>
		<add name="Subscription"
			 connectionString="Data Source=.;Initial Catalog=shuttle;Integrated Security=SSPI;"
			 providerName="System.Data.SqlClient"/>
	</connectionStrings>

	<serviceBus>
		 <inbox
			workQueueUri="msmq://./shuttle-subscriber-work"
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

## MemberRegisteredHandler

> Add a new class called `MemberRegisteredHandler` that implements the `IMessageHandler<MemberRegisteredHandler>` interface as follows:

``` c#
using System;
using Shuttle.ESB.Core;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Server
{
	public class MemberRegisteredHandler : IMessageHandler<MemberRegisteredEvent>
	{
		public void ProcessMessage(HandlerContext<MemberRegisteredEvent> context)
		{
			Console.WriteLine();
			Console.WriteLine("[EVENT RECEIVED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
```

This will write out some information to the console window.

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the subscriber project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the project.</div>

![Publish/Subscribe Subscriber - Host]({{ site.baseurl }}/assets/images/guide-publish-subscribe-subscriber-host.png "Publish/Subscribe Subscriber - Host")

Previous: [Server][previous] | Next: [Run][next]

[previous]: {{ site.baseurl }}/guide-publish-subscribe-server
[next]: {{ site.baseurl }}/guide-publish-subscribe-run