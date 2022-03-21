# Request / Response

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

This sample makes use of [Shuttle.Esb.AzureMQ](https://github.com/Shuttle/Shuttle.Esb.AzureMQ) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

Once you have opened the `Shuttle.RequestResponse.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.RequestResponse.Client
- Shuttle.RequestResponse.Server

## Implementation

In order to get any processing done in Shuttle.Esb a message will need to be generated and sent to an endpoint for processing.  The idea behind a **command** message is that there is exactly **one** endpoint handling the message.  Since it is an instruction the message absolutely ***has*** to be handled and we also need to have only a single endpoint process the message to ensure a consistent result.

In this guide we'll create the following projects:

- `Shuttle.RequestResponse.Client` (**Console Application**)
- `Shuttle.RequestResponse.Server` (**Console Application**)
- `Shuttle.RequestResponse.Messages` (**Class Library**)

## Messages

> Create a new class library called `Shuttle.RequestResponse.Messages` with a solution called `Shuttle.RequestResponse`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.RequestResponse.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

### MemberRegisteredEvent

> Add a new class called `MemberRegisteredEvent` also with a `UserName` property.

``` c#
namespace Shuttle.RequestResponse.Messages
{
	public class MemberRegisteredEvent
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.RequestResponse.Client`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Castle` nuget package.

This will provide access to the Castle `WindsorContainer` implementation.

> Add a reference to the `Shuttle.RequestResponse.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Castle.Windsor;
using Shuttle.Core.Castle;
using Shuttle.Core.Container;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.RequestResponse.Messages;

namespace Shuttle.RequestResponse.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var container = new WindsorComponentContainer(new WindsorContainer());

			container.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();

			container.RegisterServiceBus();

			using (var bus = container.Resolve<IServiceBus>().Start())
			{
				string userName;

				while (!string.IsNullOrEmpty(userName = Console.ReadLine()))
				{
					bus.Send(new RegisterMemberCommand
					{
						UserName = userName
					}, c => c.WillExpire(DateTime.Now.AddSeconds(5)));
				}
			}
		}
	}
}
```

### App.config

> Create the configuration as follows:

``` xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<configSections>
		<section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
	</configSections>

	<appSettings>
		<add key="azure" value="UseDevelopmentStorage=true;" />
	</appSettings>

	<serviceBus>
		<messageRoutes>
			<messageRoute uri="azuremq://azure/shuttle-server-work">
				<add specification="StartsWith" value="Shuttle.RequestResponse.Messages"/>
			</messageRoute>
		</messageRoutes>

		<inbox workQueueUri="azuremq://azure/shuttle-client-work" errorQueueUri="azuremq://azure/shuttle-error" threadCount="1"/>
	</serviceBus>
</configuration>
```

This tells the service bus that all messages sent having a type name starting with `Shuttle.RequestResponse.Messages` should be routed to endpoint `azuremq://azure/shuttle-server-work`.

### MemberRegisteredHandler

> Create a new class called `MemberRegisteredHandler` that implements the `IMessageHandler<MemberRegisteredEvent>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.RequestResponse.Messages;

namespace Shuttle.RequestResponse.Client
{
	public class MemberRegisteredHandler : IMessageHandler<MemberRegisteredEvent>
	{
		public void ProcessMessage(IHandlerContext<MemberRegisteredEvent> context)
		{
			Console.WriteLine();
			Console.WriteLine("[RESPONSE RECEIVED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();
		}
	}
}
```

## Server

> Add a new `Console Application` to the solution called `Shuttle.RequestResponse.Server`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.WorkerService` nuget package.

This allows a console application to be hosted as a Windows Service or Systemd Unit while running as a normal console application when debugging.

> Install the `Shuttle.Core.Castle` nuget package.

This will provide access to the Castle `WindsorContainer` implementation.

> Install the `Shuttle.Core.Log4Net` nuget package.

This will provide access to the `Log4Net` implementation.

> Add a reference to the `Shuttle.RequestResponse.Messages` project.

### Program

> Implement the `Program` class as follows:

``` c#
using Shuttle.Core.WorkerService;

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

This will simply instance the `Host` class and get it running.

### Host

> Add a `Host` class and implement the `IServiceHost` interface as follows:

``` c#
using Castle.Windsor;
using log4net;
using Shuttle.Core.Castle;
using Shuttle.Core.Container;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.WorkerService;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;

namespace Shuttle.RequestResponse.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;

        public Host()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));
        }

        public void Start()
        {
            var container = new WindsorComponentContainer(new WindsorContainer());

            container.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();

            container.RegisterServiceBus();

            _bus = container.Resolve<IServiceBus>().Start();
        }

        public void Stop()
        {
            _bus.Dispose();
        }
    }
}
```

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
<?xml version="1.0" encoding="utf-8"?>

<configuration>
	<configSections>
		<section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb" />
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
	</configSections>

	<appSettings>
		<add key="azure" value="UseDevelopmentStorage=true;" />
	</appSettings>

	<log4net>
		<appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
			<file value="logs\requestresponse-server" />
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
			<appender-ref ref="RollingFileAppender" />
		</root>
	</log4net>

	<serviceBus>
		<inbox workQueueUri="azuremq://azure/shuttle-server-work" errorQueueUri="azuremq://azure/shuttle-error" />
	</serviceBus>
</configuration>
```

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
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
	}
}
```

## Run

> Set both the client and server projects as startup projects.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

::: info 
You will observe that the server application has processed the message.
:::

::: info 
The client application will then process the response returned by the server.
:::

You have now completed a full request / response call chain.

