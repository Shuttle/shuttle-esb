---
title: Message Distribution Guide - Worker
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-distribution.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-distribution-worker'</script>
# Worker

> Add a new `Class Library` to the solution called `Shuttle.Distribution.Worker`.

![Distribution Worker]({{ site.baseurl }}/assets/images/guide-distribution-Worker.png "Distribution Worker")

> Install the `shuttle-esb-msmq` nuget package.

![Distribution Worker - Nuget Msmq]({{ site.baseurl }}/assets/images/guide-distribution-worker-nuget-msmq.png "Distribution Worker - Nuget Msmq")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `shuttle-core-host` nuget package.

![Distribution Worker - Nuget Host]({{ site.baseurl }}/assets/images/guide-distribution-worker-nuget-host.png "Distribution Worker - Nuget Host")

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.Distribution.Messages` project.

## Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

~~~ c#
using System;
using Shuttle.Core.Host;
using Shuttle.ESB.Core;

namespace Shuttle.Distribution.Worker
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
		<section name='serviceBus' type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
	</configSections>

	<serviceBus>
		<worker 
			distributorControlWorkQueueUri="msmq://./shuttle-server-control-inbox-work" />
			
		<inbox
			workQueueUri="msmq://./shuttle-worker-work"
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
~~~

This configures the endpoint as a worker and specifies the control inbox of the distributor to notify when a thread is available to perform work.

## RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

~~~ c#
using System;
using Shuttle.ESB.Core;
using Shuttle.Distribution.Messages;

namespace Shuttle.Distribution.Worker
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED --- WORKER] : user name = '{0}'", context.Message.UserName);
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

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the worker project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the project.</div>

![Distribution Worker - Host]({{ site.baseurl }}/assets/images/guide-distribution-worker-host.png "Distribution Worker - Host")

Previous: [Client][previous] | Next: [Run][next]

[previous]: {{ site.baseurl }}/guide-distribution-client
[next]: {{ site.baseurl }}/guide-distribution-run