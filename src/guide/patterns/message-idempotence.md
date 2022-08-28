# Idempotence

This sample makes use of [Shuttle.Esb.AzureStorageQueues](https://github.com/Shuttle/Shuttle.Esb.AzureStorageQueues) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

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

### RegisterMember

> Rename the `Class1` default file to `RegisterMember` and add a `UserName` property.

``` c#
namespace Shuttle.Idempotence.Messages
{
	public class RegisterMember
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.Idempotence.Client`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

> Add a reference to the `Shuttle.Idempotence.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Pipelines;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;
using Shuttle.Idempotence.Messages;

namespace Shuttle.Idempotence.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var services = new ServiceCollection();

			var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

			services.AddSingleton<IConfiguration>(configuration);

			services.AddServiceBus(builder =>
			{
				configuration.GetSection(ServiceBusOptions.SectionName).Bind(builder.Options);
			});

			services.AddAzureStorageQueues(builder =>
			{
				builder.AddOptions("azure", new AzureStorageQueueOptions
				{
					ConnectionString = configuration.GetConnectionString("azure")
				});
			});

			Console.WriteLine("Type some characters and then press [enter] to submit; an empty line submission stops execution:");
			Console.WriteLine();

			var serviceProvider = services.BuildServiceProvider();
			var pipelineFactory = serviceProvider.GetRequiredService<IPipelineFactory>();
			var messageSender = serviceProvider.GetRequiredService<IMessageSender>();
			var transportMessagePipeline = pipelineFactory.GetPipeline<TransportMessagePipeline>();

			using (var bus = serviceProvider.GetRequiredService<IServiceBus>().Start())
			{
				string userName;

				while (!string.IsNullOrEmpty(userName = Console.ReadLine()))
				{
					var command = new RegisterMember
					{
						UserName = userName
					};

					transportMessagePipeline.Execute(command, null, null);

					var transportMessage = transportMessagePipeline.State.GetTransportMessage();

					for (var i = 0; i < 5; i++)
					{
						messageSender.Dispatch(transportMessage, null); // will be processed only once since message id is the same
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

### Client configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "ConnectionStrings": {
    "azure": "UseDevelopmentStorage=true;"
  },
  "Shuttle": {
    "ServiceBus": {
      "MessageRoutes": [
        {
          "Uri": "azuresq://azure/shuttle-server-work",
          "Specifications": [
            {
              "Name": "StartsWith",
              "Value": "Shuttle.Idempotence.Messages"
            }
          ]
        }
      ]
    }
  }
}
```

This tells the service bus that all messages sent having a type name starting with `Shuttle.Idempotence.Messages` should be sent to endpoint `azuresq://azure/shuttle-server-work`.

## Server

> Add a new `Console Application` to the solution called `Shuttle.Idempotence.Server`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Hosting` nuget package.

This allows a console application to be hosted using the .NET generic host.

> Install the `Microsoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;
using Shuttle.Esb.Sql.Idempotence;

namespace Shuttle.Idempotence.Server
{
    public class Program
    {
        public static void Main()
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                    services.AddSingleton<IConfiguration>(configuration);

                    services.AddDataAccess(builder =>
                    {
                        builder.AddConnectionString("Idempotence", "Microsoft.Data.SqlClient");
                    });

                    services.AddServiceBus(builder =>
                    {
                        configuration.GetSection(ServiceBusOptions.SectionName).Bind(builder.Options);
                    });

                    services.AddSqlIdempotence();

                    services.AddAzureStorageQueues(builder =>
                    {
                        builder.AddOptions("azure", new AzureStorageQueueOptions
                        {
                            ConnectionString = configuration.GetConnectionString("azure")
                        });
                    });
                })
                .Build()
                .Run();
        }
    }
}
```

### Database

We need a store for our idempotence tracking.  In this example we will be using **Sql Server**.  If you use the express version remember to change the `data source` value to `.\sqlexpress` from the standard `.`.

When you reference the `Shuttle.Esb.Sql.Idempotence` package a `scripts` folder is included in the relevant package folder.  Click on the NuGet referenced assembly in the `Dependencies` and navigate to the package folder (in the `Path` property) to find the `scripts` folder.

The `{version}` bit will be in a `semver` format.

> Create a new database called **Shuttle** and execute the script `{provider}\IdempotenceServiceCreate.sql` in the newly created database.

### Server configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "ConnectionStrings": {
    "azure": "UseDevelopmentStorage=true;",
    "Idempotence": "server=.;database=shuttle;user id=sa;password=Pass!000;TrustServerCertificate=True"
  },
  "Shuttle": {
    "ServiceBus": {
      "Inbox": {
        "WorkQueueUri": "azuresq://azure/shuttle-server-work",
        "DeferredQueueUri": "azuresq://azure/shuttle-server-deferre",
        "ErrorQueueUri": "azuresq://azure/shuttle-error"
      }
    }
  }
}
```

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMember>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.Idempotence.Messages;

namespace Shuttle.Idempotence.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMember>
	{
		public void ProcessMessage(IHandlerContext<RegisterMember> context)
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
