---
title: Deferred Messages Sample
layout: api
---
# Running

When using Visual Studio 2015+ the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a Visual Studio command prompt:

~~~
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.Deferred
nuget restore
~~~

Once you have opened the `Shuttle.Deferred.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.Deferred.Client
- Shuttle.Deferred.Server

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project for the **Shuttle.Deferred.Server** project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bindebug</strong> folder you may need to build the solution.</div>

# Implementation

Deferred messages refer to messages that are not immediately processed when available but are rather set to only process at a given future date.

<div class='alert alert-info'>It is important to note that each endpoint <strong>must</strong> have its own deferred queue.</div>

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.Deferred.Client`
- a **Class Library** called `Shuttle.Deferred.Server`
- and another **Class Library** called `Shuttle.Deferred.Messages` that will contain all our message classes

## Messages

> Create a new class library called `Shuttle.Deferred.Messages` with a solution called `Shuttle.Deferred`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

~~~ c#
namespace Shuttle.Deferred.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
~~~

## Client

> Add a new `Console Application` to the solution called `Shuttle.Deferred.Client`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Add a reference to the `Shuttle.Deferred.Messages` project.

### Program

> Implement the main client code as follows:

~~~ c#
using System;
using Shuttle.Esb;
using Shuttle.Deferred.Messages;

namespace Shuttle.Deferred.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var bus = ServiceBus.Create().Start())
			{
				string userName;

				while (!string.IsNullOrEmpty(userName = Console.ReadLine()))
				{
					bus.Send(new RegisterMemberCommand
					{
						UserName = userName
					}, c => c.Defer(DateTime.Now.AddSeconds(5)));
				}
			}
		}
	}
}
~~~

The message sent will have its `IgnoreTilleDate` set to 5 seconds into the future.  You can have a look at the [TransportMessage][transport-message] for more information on the message structure.

### App.config

> Create the shuttle configuration as follows:

~~~ xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
	</configSections>

	<serviceBus>
		<messageRoutes>
			<messageRoute uri="msmq://./shuttle-server-work">
				<add specification="StartsWith" value="Shuttle.Deferred.Messages" />
			</messageRoute>
		</messageRoutes>		
	</serviceBus>
	
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>
</configuration>
~~~

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.Deferred.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

## Server

> Add a new `Class Library` to the solution called `Shuttle.Deferred.Server`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install both the `Shuttle.Core.Host` and `shuttle-core-infrastructure-log4net` nuget packages.

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

We are also adding **Log4Net** to demonstrate how to add a third-party logging mechanism to shuttle.

> Add a reference to the `Shuttle.Deferred.Messages` project.

### Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

~~~ c#
using System;
using log4net;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Log4Net;
using Shuttle.Esb;

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
~~~

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

~~~ xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
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
~~~

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

~~~ c#
using System;
using Shuttle.Esb;
using Shuttle.Deferred.Messages;

namespace Shuttle.Deferred.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
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
~~~

This will write out some information to the console window and send a response back to the sender (client).

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bindebug</strong> folder you may need to build the solution.</div>

## Run

> Set both the client and server projects as the startup.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

<div class='alert alert-info'>After 5 seconds you will observe that the <strong>server</strong> application has processed the message.</div>

You have now implemented deferred message sending.

You will also notice that `Log4Net` has created the log file under **~\Shuttle.Deferred\Shuttle.Deferred.Server\bin\Debug\logs**.

[transport-message]: {{ site.baseurl }}/transport-message
