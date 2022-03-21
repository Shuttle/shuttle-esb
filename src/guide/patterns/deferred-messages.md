# Deferred Messages

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

This sample makes use of [Shuttle.Esb.AzureMQ](https://github.com/Shuttle/Shuttle.Esb.AzureMQ) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

Once you have opened the `Shuttle.Deferred.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.Deferred.Client
- Shuttle.Deferred.Server

## Implementation

Deferred messages refer to messages that are not immediately processed when available but are rather set to only process at a given future date.

::: danger
It is important to note that each endpoint instance must have its own deferred queue.
:::

In this guide we'll create the following projects:

- `Shuttle.Deferred.Client` (**Console Application**)
- `Shuttle.Deferred.Server` (**Console Application**)
- `Shuttle.Deferred.Messages` (**Class Library**)

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

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Autofac` nuget package.

This will provide access to the Autofac dependency container implementation.

> Add a reference to the `Shuttle.Deferred.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Autofac;
using Shuttle.Core.Autofac;
using Shuttle.Core.Container;
using Shuttle.Deferred.Messages;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;

namespace Shuttle.Deferred.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var containerBuilder = new ContainerBuilder();
			var registry = new AutofacComponentRegistry(containerBuilder);

			registry.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
			registry.RegisterServiceBus();

			using (var bus = new AutofacComponentResolver(containerBuilder.Build()).Resolve<IServiceBus>().Start())
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

> Create the service bus configuration as follows:

``` xml
<?xml version="1.0" encoding="utf-8"?>

<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb" />
	</configSections>

	<appSettings>
		<add key="azure" value="UseDevelopmentStorage=true;" />
	</appSettings>

	<serviceBus>
		<messageRoutes>
			<messageRoute uri="azuremq://azure/shuttle-server-work">
				<add specification="StartsWith" value="Shuttle.Deferred.Messages" />
			</messageRoute>
		</messageRoutes>
	</serviceBus>
</configuration>
```

This tells the service bus that all messages sent having a type name starting with `Shuttle.Deferred.Messages` should be sent to endpoint `azuremq://azure/shuttle-server-work`.

## Server

> Add a new `Console Application` to the solution called `Shuttle.Deferred.Server`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.WorkerService` nuget package.

This allows a console application to be hosted as a Windows Service or Systemd Unit while running as a normal console application when debugging.

> Install the `Shuttle.Core.Autofac` nuget package.

This will provide access to the Autofac dependency container implementation.

> Install the `Shuttle.Core.Log4Net` nuget package.

We are also adding **Log4Net** to demonstrate how to add a third-party logging mechanism.

> Add a reference to the `Shuttle.Deferred.Messages` project.

### Program

> Implement the `Program` class as follows:

``` c#
using Shuttle.Core.WorkerService;

namespace Shuttle.Deferred.Server
{
    public class Programs
    {
        public static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }
}
```

### Host

> Rename the default `Class1` file to `Host` and implement the `IServiceHost` interface as follows:

``` c#
using System.Text;
using Autofac;
using log4net;
using Shuttle.Core.Autofac;
using Shuttle.Core.Container;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.WorkerService;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            var containerBuilder = new ContainerBuilder();
            var registry = new AutofacComponentRegistry(containerBuilder);

            registry.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
            registry.RegisterServiceBus();

            _bus = new AutofacComponentResolver(containerBuilder.Build()).Resolve<IServiceBus>().Start();
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
		<root>
			<level value="INFO" />
			<appender-ref ref="ConsoleAppender" />
			<appender-ref ref="RollingFileAppender" />
		</root>
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
	</log4net>

	<serviceBus>
		<inbox workQueueUri="azuremq://azure/shuttle-server-work" 
		       deferredQueueUri="azuremq://azure/shuttle-server-deferred"
		       errorQueueUri="azuremq://azure/shuttle-error" />
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
		    _log.Information($"[MEMBER REGISTERED] : user name = '{context.Message.UserName}'");
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

::: info 
After 5 seconds you will observe that the server application has processed the message.
:::

You have now implemented deferred message sending.

You will also notice that `Log4Net` has created the log file under `~\Shuttle.Deferred\Shuttle.Deferred.Server\bin\Debug\{framework}\logs`.
