# Message Idempotence

This sample makes use of [Shuttle.Esb.AzureMQ](https://github.com/Shuttle/Shuttle.Esb.AzureMQ) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

Once you have opened the `Shuttle.Idempotence.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.Idempotence.Client
- Shuttle.Idempotence.Server

You will also need to create and configure a Sql Server database for this sample and remember to update the **App.config** `connectionString` settings to point to your database.  Please reference the **Database** section below.

## Implementation

When operations, or in our case messages, can be applied multiple times with the same result they are said to be **idempotent**.  Idempotence is something you should strive to implement directly on your endpoint by keeping track of some unique property of each message and whether the operation has been completed for that unique property.

An `IIdempotenceService` implementation can assist with this from a technical point-of-view by allowing a particular message id to be handled only once.  This works fine for our ***at-least-once*** delivery mechanism where, in some edge case, we may receive the same message again.  However, it will not aid us when two messages are going to be sent, each with its own message id, but they contain the same data.

In this guide we'll create the following projects:

- `Shuttle.Idempotence.Client` (**Console Application**)
- `Shuttle.Idempotence.Server` (**Console Application**)
- `Shuttle.Idempotence.Messages` (**Class Library**)

## Messages

> Create a new class library called `Shuttle.Idempotence.Messages` with a solution called `Shuttle.Idempotence`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.Idempotence.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.Idempotence.Client`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.SimpleInjector` nuget package.

This will provide access to the SimpleInjector dependency container.

> Add a reference to the `Shuttle.Idempotence.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Shuttle.Core.Container;
using Shuttle.Core.SimpleInjector;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.Idempotence.Messages;
using SimpleInjector;

namespace Shuttle.Idempotence.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
            var simpleInjectorContainer = new Container();

            simpleInjectorContainer.Options.EnableAutoVerification = false;
            
            var container = new SimpleInjectorComponentContainer(simpleInjectorContainer);

            container.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
			container.RegisterServiceBus();

			var transportMessageFactory = container.Resolve<ITransportMessageFactory>();

			using (var bus = container.Resolve<IServiceBus>().Start())
			{
				string userName;

				while (!string.IsNullOrEmpty(userName = Console.ReadLine()))
				{
					var command = new RegisterMemberCommand
					{
						UserName = userName
					};

					var transportMessage = transportMessageFactory.Create(command, c => { });

					for (var i = 0; i < 5; i++)
					{
						bus.Dispatch(transportMessage); // will be processed once since message id is the same
					}

					bus.Send(command); // will be processed since it has a new message id
					bus.Send(command); // will also be processed since it too has a new message id
				}
			}
		}
	}
}
```

Keep in mind that the when you `Send` a message a `TransportMessage` envelope is created with a unique message id (`Guid`).  In the above code we first manually create a `TransportMessage` so that we can send technically identical messages (with the same message id).

The next two `Send` operations do not use the `TransportMessage` but rather send individual messages.  These will each have a `TransportMessage` envelope and, therefore, each have its own unique message id.

### App.config

> Create the shuttle configuration as follows:

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
				<add specification="StartsWith" value="Shuttle.Idempotence.Messages" />
			</messageRoute>
		</messageRoutes>
	</serviceBus>
</configuration>
```

This tells the service bus that all messages sent having a type name starting with `Shuttle.Idempotence.Messages` should be sent to endpoint `azuremq://azure/shuttle-server-work`.

## Server

> Add a new `Console Application` to the solution called `Shuttle.Idempotence.Server`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.WorkerService` nuget package.

This allows a console application to be hosted as a Windows Service or Systemd Unit while running as a normal console application when debugging.

> Install the `Shuttle.Core.SimpleInjector` nuget package.

This will provide access to the SimpleInjector dependency container.

> Install the `Shuttle.Esb.Sql.Idempotence` package. 

We will also have access to the Sql implementation of the `IIdempotenceService`.

> Install the `Microsoft.Data.SqlClient` nuget package.

This will provide a connection to our Sql Server.

> Add a reference to the `Shuttle.Idempotence.Messages` project.

### Program

Implement the `Program` class as follows:

``` c#
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Shuttle.Core.WorkerService;

namespace Shuttle.Idempotence.Server
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
using Shuttle.Core.SimpleInjector;
using Shuttle.Core.WorkerService;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;
using Shuttle.Esb.Sql.Idempotence;
using SimpleInjector;

namespace Shuttle.Idempotence.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;

        public void Start()
        {
            var container = new SimpleInjectorComponentContainer(new Container());

            container.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
            container.RegisterDataAccess();
            container.RegisterIdempotence();
            container.RegisterServiceBus();

            _bus = container.Resolve<IServiceBus>().Start();
        }

        public void Stop()
        {
            _bus?.Dispose();
        }
    }
}
```

### Database

We need a store for our idempotence tracking.  In this example we will be using **Sql Server**.  If you use the express version remember to change the `data source` value to `.\sqlexpress` from the standard `.`.

When you reference the `Shuttle.Esb.Sql.Idempotence` package a `scripts` folder is included in the relevant package folder.  Click on the Nuget referenced assembly in the `References` or `Dependencies` (depending on your project type) and navigate to the package folder to find the `scripts` folder.

The `{version}` bit will be in a `semver` format.

> Create a new database called **Shuttle** and execute the script `{provider}\IdempotenceServiceCreate.sql` in the newly created database.

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
		<add name="Idempotence"
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

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
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
	}
}
```

This will write out some information to the console window.

## Run

> Set both the client and server projects as startup projects.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

::: info
You will need to scroll through the messages but you will observe that the server application has processed all three messages.
:::

You have now implemented message idempotence.
