# Publish / Subscribe

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

This sample makes use of [Shuttle.Esb.AzureMQ](https://github.com/Shuttle/Shuttle.Esb.AzureMQ) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

Once you have opened the `Shuttle.PublishSubscribe.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.PublishSubscribe.Client
- Shuttle.PublishSubscribe.Server
- Shuttle.PublishSubscribe.Subscriber

You will also need to create and configure a Sql Server database for this sample and remember to update the **App.config** `connectionString` settings to point to your database.  Please reference the **Database** section below.

## Implementation

**Events** are interesting things that happen in our system that other systems may be interested in.  There may be `0..N` number of subscribers for an event.  Typically there should be at least one subscriber to an event.

In this guide we'll create the following projects:

- `Shuttle.PublishSubscribe.Client` (**Console Application**)
- `Shuttle.PublishSubscribe.Server` (**Console Application**)
- `Shuttle.PublishSubscribe.Subscriber` (**Console Application**)
- `Shuttle.PublishSubscribe.Messages` (**Class Library**)

## Messages

> Create a new class library called `Shuttle.PublishSubscribe.Messages` with a solution called `Shuttle.PublishSubscribe`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.PublishSubscribe.Messages
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
namespace Shuttle.PublishSubscribe.Messages
{
	public class MemberRegisteredEvent
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Client`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.StructureMap` nuget package.

This will provide access to the StructureMap dependency injection container.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Shuttle.Core.Container;
using Shuttle.Core.StructureMap;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.PublishSubscribe.Messages;
using StructureMap;

namespace Shuttle.PublishSubscribe.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var registry = new Registry();
			var componentRegistry = new StructureMapComponentRegistry(registry);

			componentRegistry.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
			componentRegistry.RegisterServiceBus();

			using (var bus = new StructureMapComponentResolver(new Container(registry)).Resolve<IServiceBus>().Start())
			{
				string userName;

				while (!string.IsNullOrEmpty(userName = Console.ReadLine()))
				{
					bus.Send(new RegisterMemberCommand
					{
						UserName = userName
					});
				}
			}
		}
	}
}
```

### App.config

> Create the relevant configuration as follows:

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
				<add specification="StartsWith" value="Shuttle.PublishSubscribe.Messages" />
			</messageRoute>
		</messageRoutes>
	</serviceBus>
</configuration>
```

This tells the service bus that all messages sent having a type name starting with `Shuttle.PublishSubscribe.Messages` should be sent to endpoint `azuremq://azure/shuttle-server-work`.

## Server

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Server`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.WorkerService` nuget package.

This allows a console application to be hosted as a Windows Service or Systemd Unit while running as a normal console application when debugging.

> Install the `Shuttle.Core.StructureMap` nuget package.

This will provide access to the StructureMap dependency injection container.

> Install the `Shuttle.Esb.Sql.Subscription` nuget package.

This will provide access to the Sql-based `ISubscriptionService` implementation.

> Install the `Microsoft.Data.SqlClient` nuget package.

This will provide a connection to our Sql Server.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

### Program

Implement the `Program` class as follows:

``` c#
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Shuttle.Core.WorkerService;

namespace Shuttle.PublishSubscribe.Server
{
    public class Program
    {
        public static void Main()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

            ServiceHost.Run<Host>();
        }
    }
}
```

This will simply run the `Host` implementation.

### Host

> Add a `Host` class and implement the `IServiceHost` interface as follows:

``` c#
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.StructureMap;
using Shuttle.Core.WorkerService;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.Esb.Sql.Subscription;
using StructureMap;

namespace Shuttle.PublishSubscribe.Server
{
    public class Host : IServiceHostStart
    {
        private IServiceBus _bus;

        public void Start()
        {
            var registry = new Registry();
            var componentRegistry = new StructureMapComponentRegistry(registry);

            componentRegistry.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
            componentRegistry.RegisterDataAccess();
            componentRegistry.RegisterSubscription();
            componentRegistry.RegisterServiceBus();

            _bus = new StructureMapComponentResolver(new Container(registry)).Resolve<IServiceBus>().Start();
        }

        public void Stop()
        {
            _bus?.Dispose();
        }
    }
}
```

### Database

We need a store for our subscriptions.  In this example we will be using **Sql Server**.  Remember to make any required changges to the relevant connection strings.

When you reference the `Shuttle.Esb.Sql.Subscription` package a `scripts` folder is included in the relevant package folder.  Click on the Nuget referenced assembly in the `References` or `Dependencies` (depending on your project type) and navigate to the package folder to find the `scripts` folder.

The `{version}` bit will be in a `semver` format.

> Create a new database called **Shuttle** and execute the script `{provider}\SubscriptionManagerCreate.sql` in the newly created database.

This will create the required structures that the subscription manager will use to store the subcriptions.

Whenever the `Publish` method is invoked on the `ServiceBus` instance the registered `ISubscriptionService` instance is asked for the subscribers to the published message type.  These are retrieved from the Sql Server database for the implementation we are using.

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
<?xml version="1.0" encoding="utf-8"?>

<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb" />
	</configSections>

	<appSettings>
		<add key="azure" value="UseDevelopmentStorage=true;" />
	</appSettings>

	<connectionStrings>
		<add name="Subscription"
		     connectionString="server=.;database=shuttle;user id=sa;password=Pass!000;TrustServerCertificate=True"
		     providerName="Microsoft.Data.SqlClient" />
	</connectionStrings>

	<serviceBus>
		<inbox
			workQueueUri="azuremq://azure/shuttle-server-work"
			errorQueueUri="azuremq://azure/shuttle-error" />
	</serviceBus>
</configuration>
```

The Sql Server implementation of the `ISubscriptionService` that we are using by default will try to find a connection string with a name of **Subscription**.  However, you can override this.  See the [Sql Server configuration options][sql-server] for details about how to do this.

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();

			context.Publish(new MemberRegisteredEvent
			{
				UserName = context.Message.UserName
			});
		}
	}
}
```

This will write out some information to the console window and publish the `MemberRegisteredEvent` message.

## Subscriber

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Subscriber`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.WorkerService` nuget package.

This allows a console application to be hosted as a Windows Service or Systemd Unit while running as a normal console application when debugging.

> Install the `Shuttle.Core.StructureMap` nuget package.

This will provide access to the StructureMap dependency injection container.

> Install the `Shuttle.Esb.Sql.Subscription` nuget package.

This will provide access to the Sql-based `ISubscriptionService` implementation.

> Install the `Microsoft.Data.SqlClient` nuget package.

This will provide a connection to our Sql Server.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

### Program

Implement the `Program` class as follows:

``` c#
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Shuttle.Core.WorkerService;

namespace Shuttle.PublishSubscribe.Subscriber
{
    public class Program
    {
        public static void Main()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

            ServiceHost.Run<Host>();
        }
    }
}
```

This will simply run the `Host` implementation.

### Host

> Add a `Host` class and implement the `IServiceHost` interface as follows:

``` c#
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.StructureMap;
using Shuttle.Core.WorkerService;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.Esb.Sql.Subscription;
using Shuttle.PublishSubscribe.Messages;
using StructureMap;

namespace Shuttle.PublishSubscribe.Subscriber
{
    public class Host : IServiceHostStart
    {
        private IServiceBus _bus;

        public void Start()
        {
            var registry = new Registry();
            var componentRegistry = new StructureMapComponentRegistry(registry);

            componentRegistry.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
            componentRegistry.RegisterDataAccess();
            componentRegistry.RegisterSubscription();

            services.AddServiceBus(builder =>
            {
                builder.AddSubscription<MemberRegisteredEvent>();
            });

            var resolver = new StructureMapComponentResolver(new Container(registry));

            _bus = resolver.Resolve<IServiceBus>().Start();
        }

        public void Stop()
        {
            _bus.Dispose();
        }
    }
}
```

Here we register the subscription by calling the `AddSubscription` method on the `ServiceBusBuiler`.  Since we are using the Sql Server implementation of the `ISubscriptionService` interface an entry will be created in the **SubscriberMessageType** table associating the inbox work queue uri with the message type.

It is important to note that in a production environment one would not typically register subscriptions in this manner as they may be somewhat more sensitive as we do not want any arbitrary subscriber listening in on the messages being published.  For this reason the connection string should be read-only and the subscription should be registered manually or via a deployment script.  Should the subscription **not** yet exist the creation of the subscription will fail, indicating that the subscription should be registered out-of-band.

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
<?xml version="1.0" encoding="utf-8"?>

<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb" />
		<section name="subscription" type="Shuttle.Esb.Sql.Subscription.SubscriptionSection, Shuttle.Esb.Sql.Subscription" />
	</configSections>

	<appSettings>
		<add key="azure" value="UseDevelopmentStorage=true;" />
	</appSettings>

	<connectionStrings>
		<add name="Subscription"
		     connectionString="server=.;database=shuttle;user id=sa;password=Pass!000;TrustServerCertificate=True"
		     providerName="Microsoft.Data.SqlClient" />
	</connectionStrings>

	<serviceBus>
		<inbox
			workQueueUri="azuremq://azure/shuttle-subscriber-work"
			errorQueueUri="azuremq://azure/shuttle-error" />
	</serviceBus>
</configuration>
```

### MemberRegisteredHandler

> Add a new class called `MemberRegisteredHandler` that implements the `IMessageHandler<MemberRegisteredHandler>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Subscriber
{
	public class MemberRegisteredHandler : IMessageHandler<MemberRegisteredEvent>
	{
		public void ProcessMessage(IHandlerContext<MemberRegisteredEvent> context)
		{
			Console.WriteLine();
			Console.WriteLine("[EVENT RECEIVED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();
		}
	}
}
```

This will write out some information to the console window.

## Run

> Set the client, server, and subscriber projects as startup projects.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

::: info 
You will observe that the server application has processed the message.
:::

::: info
The subscriber application will then process the event published by the server.
:::

You have now completed a full publish / subscribe call chain.
