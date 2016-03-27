---
title: Request / Response Guide - Server
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-request-response.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-request-response-server'</script>
# Server

> Add a new `Class Library` to the solution called `Shuttle.RequestResponse.Server`.

![Request/Response Server]({{ site.baseurl }}/assets/images/guide-request-response-Server.png "Request/Response Server")

> Install the `Shuttle.Esb.Msmq` nuget package.

![Request/Response Server - Nuget Msmq]({{ site.baseurl }}/assets/images/guide-request-response-server-nuget-msmq.png "Request/Response Server - Nuget Msmq")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Host` nuget package.

![Request/Response Server - Nuget Host]({{ site.baseurl }}/assets/images/guide-request-response-server-nuget-host.png "Request/Response Server - Nuget Host")

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.RequestResponse.Messages` project.

## Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

~~~ c#
using System;
using Shuttle.Core.Host;
using Shuttle.Esb;

namespace Shuttle.RequestResponse.Server
{
	public class Host : IHost, IDisposable
	{
		private IServiceBus _bus;

		public void Start()
		{
			_bus = ServiceBus.Create().Start();
		}

		public void Dispose()
		{
			_bus.Dispose();
		}
	}
}
~~~

## App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

~~~ xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
	</configSections>

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
using Shuttle.Esb;
using Shuttle.RequestResponse.Messages;

namespace Shuttle.RequestResponse.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();

			context.Send(new MemberRegisteredEvent
			{
				UserName = context.Message.UserName
			}, c => c.Reply());
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
~~~

This will write out some information to the console window and send a response back to the sender (client).

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the project.</div>

![Request/Response Server - Host]({{ site.baseurl }}/assets/images/guide-request-response-server-host.png "Request/Response Server - Host")

Previous: [Client][previous] | Next: [Run][next]

[previous]: {{ site.baseurl }}/guide-request-response-client
[next]: {{ site.baseurl }}/guide-request-response-run