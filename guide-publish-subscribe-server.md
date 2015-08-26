---
title: Publish / Subscribe Guide - Server
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-publish-subscribe.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-publish-subscribe-server'</script>
# Server

> Add a new `Class Library` to the solution called `Shuttle.PublishSubscribe.Server`.

![Publish/Subscribe Server]({{ site.baseurl }}/assets/images/guide-publish-subscribe-server.png "Publish/Subscribe Server")

> Install **both** the `shuttle-esb-msmq` and `shuttle-esb-sqlserver` nuget packages.

![Publish/Subscribe Server - Nuget Msmq/Sql]({{ site.baseurl }}/assets/images/guide-publish-subscribe-server-nuget-msmq-sql.png "Publish/Subscribe Server - Nuget Msmq/Sql")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.  You are also including the **SqlServer** implementation for the `ISubscriptionManager`.

> Install the `shuttle-core-host` nuget package.

![Publish/Subscribe Server - Nuget Host]({{ site.baseurl }}/assets/images/guide-publish-subscribe-server-nuget-host.png "Publish/Subscribe Server - Nuget Host")

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

## Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

``` c#
using System;
using Shuttle.Core.Host;
using Shuttle.ESB.Core;

namespace Shuttle.PublishSubscribe.Subscriber
{
	public class Host : IHost, IDisposable
	{
		private IServiceBus _bus;

		public void Start()
		{
			_bus = ServiceBus.Create(c => c.SubscriptionManager(SubscriptionManager.Default())).Start();
		}

		public void Dispose()
		{
			_bus.Dispose();
		}
	}
}
```

## Database

We need a store for our subscriptions.  In this example we will be using **Sql Server**.  If you use the express version remember to change the `data source` value to `.\sqlexpress` from the standard `.`.

When you reference the `shuttle-esb-sqlserver` package a number of scripts are included in the relevant package folder:

- `.\Shuttle.PublishSubscribe\packages\shuttle-esb-sqlserver.{version}\scripts`

The `{version}` bit will be in a `semver` format.

> Create a new database called **Shuttle** and execute script `SubscriptionManagerCreate.sql` in the newly created database.

This will create the required structures that the idempotence service can use to keep track of message processing.

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
			workQueueUri="msmq://./shuttle-server-work"
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

The Sql Server implementation of the `ISubscriptionManager` that we are using by default will try to find a connection string with a name of **Subscription**.  However, you can override this.  See the [Sql Server configuration options][sql-server] for details about how to do this.

## RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.ESB.Core;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Subscriber
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(HandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();

			context.Publish(new MemberRegisteredEvent
			{
				UserName = context.Message.UserName
			});
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
```

This will write out some information to the console window and publish the `MemberRegisteredEvent` message.

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the project.</div>

![Publish/Subscribe Server - Host]({{ site.baseurl }}/assets/images/guide-publish-subscribe-server-host.png "Publish/Subscribe Server - Host")

Previous: [Client][previous] | Next: [Subscriber][next]

[previous]: {{ site.baseurl }}/guide-publish-subscribe-client
[next]: {{ site.baseurl }}/guide-publish-subscribe-subscriber
[sql-server]: {{ site.baseurl }}/extensions-sql-server
