---
title: Message Distribution Guide - Server
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-distribution.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-distribution-server'</script>
# Server

> Add a new `Class Library` to the solution called `Shuttle.Distribution.Server`.

![Distribution Server]({{ site.baseurl }}/assets/images/guide-distribution-Server.png "Distribution Server")

> Install the `Shuttle.Esb.Msmq` nuget package.

![Distribution Server - Nuget Msmq]({{ site.baseurl }}/assets/images/guide-distribution-server-nuget-msmq.png "Distribution Server - Nuget Msmq")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Host` nuget package.

![Distribution Server - Nuget Host]({{ site.baseurl }}/assets/images/guide-distribution-server-nuget-host.png "Distribution Server - Nuget Host")

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.Distribution.Messages` project.

## Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

~~~ c#
using System;
using Shuttle.Core.Host;
using Shuttle.Esb;

namespace Shuttle.Distribution.Server
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
		<control 
			workQueueUri="msmq://./shuttle-server-control-inbox-work" 
			errorQueueUri="msmq://./shuttle-samples-error" />

		<inbox
		   distribute="true"
		   workQueueUri="msmq://./shuttle-server-work"
		   errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
~~~

This will instruct the endpoint to ***only** distribute messages since the `distribute` attribute is set to `true`.  If it is set to `false` then the endpoint will process an incoming message if a worker thread is not available.

It also configures the control inbox that the endpoint will use to process administrative messages.

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the project.</div>

![Distribution Server - Host]({{ site.baseurl }}/assets/images/guide-distribution-server-host.png "Distribution Server - Host")

Previous: [Client][previous] | Next: [Worker][next]

[previous]: {{ site.baseurl }}/guide-distribution-client
[next]: {{ site.baseurl }}/guide-distribution-worker