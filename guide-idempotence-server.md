---
title: Idempotence Guide - Server
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-idempotence.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-idempotence-server'</script>
# Server

> Add a new `Class Library` to the solution called `Shuttle.Idempotence.Server`.

![Idempotence Server]({{ site.baseurl }}/assets/images/guide-idempotence-Server.png "Idempotence Server")

> Install noth the `shuttle-esb-msmq` and `shuttle-esb-sqlserver` nuget packages.

![Idempotence Server - Nuget Msmq/SqlServer]({{ site.baseurl }}/assets/images/guide-idempotence-server-nuget-msmq-sqlserver.png "Idempotence Server - Nuget Msmq/SqlServer")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

We will also have access to the Sql Server implementation of the `IIdempotenceService`.

> Install the `shuttle-core-host` nuget package.

![Idempotence Server - Nuget Host]({{ site.baseurl }}/assets/images/guide-idempotence-server-nuget-host.png "Idempotence Server - Nuget Host")

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.Idempotence.Messages` project.

## Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

~~~ c#
using System;
using Shuttle.Core.Host;
using Shuttle.ESB.Core;
using Shuttle.ESB.SqlServer.Idempotence;

namespace Shuttle.Idempotence.Server
{
	public class Host : IHost, IDisposable
	{
		private IServiceBus _bus;

		public void Start()
		{
			_bus = ServiceBus.Create(c=>c.IdempotenceService(IdempotenceService.Default())).Start();
		}

		public void Dispose()
		{
			_bus.Dispose();
		}
	}
}
~~~

## Database

We need a store for our idempotence tracking.  In this example we will be using **Sql Server**.  If you use the express version remember to change the `data source` value to `.\sqlexpress` from the standard `.`.

When you reference the `shuttle-esb-sqlserver` package a number of scripts are included in the relevant package folder:

- `.\Shuttle.PublishSubscribe\packages\shuttle-esb-sqlserver.{version}\scripts`

The `{version}` bit will be in a `semver` format.

> Create a new database called **Shuttle** and execute script `IdempotenceServiceCreate.sql` in the newly created database.

Whenever `Publish` is called the registered `ISubscriptionManager` instance is asked for the subscribers to the published message type.  These are retrieved from the Sql Server database for the implementation we are using.

## App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

~~~ xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
	</configSections>

	<connectionStrings>
		<add name="Idempotence"
			 connectionString="Data Source=.;Initial Catalog=shuttle;Integrated Security=SSPI;"
			 providerName="System.Data.SqlClient"/>
	</connectionStrings>

	<serviceBus>
		 <inbox
			workQueueUri="msmq://./shuttle-server-work"
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
~~~

## RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

~~~ c#
using System;
using Shuttle.ESB.Core;
using Shuttle.Idempotence.Messages;

namespace Shuttle.Idempotence.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}' / message id = '{1}'",
				context.Message.UserName,
				context.TransportMessage.MessageId);
			Console.WriteLine();
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
~~~

This will write out some information to the console window.

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the project.</div>

![Idempotence Server - Host]({{ site.baseurl }}/assets/images/guide-idempotence-server-host.png "Idempotence Server - Host")

Previous: [Client][previous] | Next: [Run][next]

[previous]: {{ site.baseurl }}/guide-idempotence-client
[next]: {{ site.baseurl }}/guide-idempotence-run