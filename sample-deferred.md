---
title: Deferred Messages Sample
layout: api
---

<div class='alert alert-info'>Remember to download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.</div>

# Running

When using Visual Studio 2017 the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a Visual Studio command prompt:

```
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.Deferred
nuget restore
```

Once you have opened the `Shuttle.Deferred.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.Deferred.Client
- Shuttle.Deferred.Server

# Implementation

Deferred messages refer to messages that are not immediately processed when available but are rather set to only process at a given future date.

<div class='alert alert-info'>It is important to note that each endpoint <strong>must</strong> have its own deferred queue.</div>

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.Deferred.Client`
- a **Console Application** called `Shuttle.Deferred.Server`
- a **Class Library** called `Shuttle.Deferred.Messages` that will contain all our message classes

## Messages

> Create a new class library called `Shuttle.Deferred.Messages` with a solution called `Shuttle.Deferred`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.Deferred.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.Deferred.Client`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Autofac` nuget package.

This will provide access to the Autofac dependency container implementation.

> Add a reference to the `Shuttle.Deferred.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Autofac;
using Shuttle.Core.Autofac;
using Shuttle.Deferred.Messages;
using Shuttle.Esb;

namespace Shuttle.Deferred.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var containerBuilder = new ContainerBuilder();
			var registry = new AutofacComponentRegistry(containerBuilder);

			ServiceBus.Register(registry);

			using (var bus = ServiceBus.Create(new AutofacComponentResolver(containerBuilder.Build())).Start())
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
```

The message sent will have its `IgnoreTilleDate` set to 5 seconds into the future.  You can have a look at the [TransportMessage][transport-message] for more information on the message structure.

### App.config

> Create the shuttle configuration as follows:

``` xml
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
</configuration>
```

This tells Shuttle that all messages that are sent and have a type name starting with `Shuttle.Deferred.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

## Server

> Add a new `Console Application` to the solution called `Shuttle.Deferred.Server`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Autofac` nuget package.

This will provide access to the Autofac dependency container implementation.

> Install the `Shuttle.Core.ServiceHost` nuget package.

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.ServiceHost` we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Install the `Shuttle.Core.Log4Net` nuget package.

We are also adding **Log4Net** to demonstrate how to add a third-party logging mechanism to shuttle.

> Add a reference to the `Shuttle.Deferred.Messages` project.

### Program

> Implement the `Program` class as follows:

``` c#
using Shuttle.Core.ServiceHost;

namespace Shuttle.RequestResponse.Server
{
    internal class Program
    {
        private static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }
}
```

### Host

> Rename the default `Class1` file to `Host` and implement the `IHost` interface as follows:

``` c#
using Autofac;
using log4net;
using Shuttle.Core.Autofac;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.ServiceHost;
using Shuttle.Esb;

namespace Shuttle.Deferred.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;

        public void Stop()
        {
            _bus.Dispose();
        }

        public void Start()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            var containerBuilder = new ContainerBuilder();
            var registry = new AutofacComponentRegistry(containerBuilder);

            ServiceBus.Register(registry);

            _bus = ServiceBus.Create(new AutofacComponentResolver(containerBuilder.Build())).Start();
        }
    }
}
```

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
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
```

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using Shuttle.Core.Logging;
using Shuttle.Esb;
using Shuttle.Deferred.Messages;

namespace Shuttle.Deferred.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
	    private readonly ILog _log;

	    public RegisterMemberHandler()
	    {
	        _log = Log.For(this);
	    }

	    public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
		{
		    _log.Trace($"[MEMBER REGISTERED] : user name = '{context.Message.UserName}'");
		}
	}
}
```

This will use Log4Net to write out some information to the console window as well as a file.

## Run

> Set both the client and server projects as the startup.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

<div class='alert alert-info'>After 5 seconds you will observe that the <strong>server</strong> application has processed the message.</div>

You have now implemented deferred message sending.

You will also notice that `Log4Net` has created the log file under **~\Shuttle.Deferred\Shuttle.Deferred.Server\bin\Debug\{framework}\logs**.
