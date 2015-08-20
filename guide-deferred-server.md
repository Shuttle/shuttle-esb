---
title: Deferred Messages Guide
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-deferred.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-deferred-server'</script>
# Server

> Add a new `Class Library` to the solution called `Shuttle.Deferred.Server`.

![Deferred Server]({{ site.baseurl }}/assets/images/guide-deferred-Server.png "Deferred Server")

> Install the `shuttle-esb-msmq` nuget package.

![Deferred Server - Nuget Msmq]({{ site.baseurl }}/assets/images/guide-deferred-server-nuget-msmq.png "Deferred Server - Nuget Msmq")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install both the `shuttle-core-host` and `shuttle-core-infrastructure-log4net` nuget packages.

![Deferred Server - Nuget Host/Logging]({{ site.baseurl }}/assets/images/guide-deferred-server-nuget-host-logging.png "Deferred Server - Nuget Host/Logging")

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

We are also adding **Log4Net** to demonstrate how to add a third-party logging mechanism to shuttle.

> Add a reference to the `Shuttle.Deferred.Messages` project.

## Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

``` c#
using System;
using log4net;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Log4Net;
using Shuttle.ESB.Core;

namespace Shuttle.Deferred.Server
{
	public class Host : IHost, IDisposable
	{
		private IServiceBus _bus;

		public void Start()
		{
			Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

			_bus = ServiceBus.Create().Start();
		}

		public void Dispose()
		{
			_bus.Dispose();
		}
	}
}
```

## App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
	</configSections>

	<log4net>
		<appender name="ConsoleAppender" type="log4net.Appender.ColoredConsoleAppender">
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%d [%t] %-5p %c - %m%n" />
			</layout>
		</appender>
		<appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
			<file value="logs\deferred-server" />
			<appendToFile value="true" />
			<rollingStyle value="Composite" />
			<maxSizeRollBackups value="10" />
			<maximumFileSize value="100000KB" />
			<datePattern value="-yyyyMMdd.'log'" />
			<param name="StaticLogFileName" value="false" />
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%d [%t] %-5p %c - %m%n" />
			</layout>
		</appender>
		<root>
			<level value="TRACE" />
			<appender-ref ref="ConsoleAppender" />
			<appender-ref ref="RollingFileAppender" />
		</root>
	</log4net>

	<serviceBus>
		<inbox
		   workQueueUri="msmq://./shuttle-server-work"
		   deferredQueueUri="msmq://./shuttle-server-deferred"
		   errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

## RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.ESB.Core;
using Shuttle.Deferred.Messages;

namespace Shuttle.Deferred.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(HandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
```

This will write out some information to the console window and send a response back to the sender (client).

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the project.</div>

![Deferred Server - Host]({{ site.baseurl }}/assets/images/guide-deferred-server-host.png "Deferred Server - Host")

Previous: [Client][previous] | Next: [Run][next]

[previous]: {{ site.baseurl }}/guide-deferred-client
[next]: {{ site.baseurl }}/guide-deferred-run