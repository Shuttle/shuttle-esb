# Publish / Subscribe

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

This sample makes use of [Shuttle.Esb.AzureStorageQueues](https://github.com/Shuttle/Shuttle.Esb.AzureStorageQueues) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

Once you have opened the `Shuttle.PublishSubscribe.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.PublishSubscribe.Client
- Shuttle.PublishSubscribe.Server
- Shuttle.PublishSubscribe.Subscriber

You will also need to create and configure a Sql Server database for this sample and remember to update the **App.config** `connectionString` settings to point to your database.  Please reference the **Database** section below.

## Implementation

**Events** are interesting things that happen in our system that other systems may be interested in.  There may be `0..n` number of subscribers for an event.  Typically there should be at least one subscriber to an event.

In this guide we'll create the following projects:

- `Shuttle.PublishSubscribe.Client` (**Console Application**)
- `Shuttle.PublishSubscribe.Server` (**Console Application**)
- `Shuttle.PublishSubscribe.Subscriber` (**Console Application**)
- `Shuttle.PublishSubscribe.Messages` (**Class Library**)

## Messages

> Create a new class library called `Shuttle.PublishSubscribe.Messages` with a solution called `Shuttle.PublishSubscribe`

**Note**: remember to change the *Solution name*.

### RegisterMember

> Rename the `Class1` default file to `RegisterMember` and add a `UserName` property.

``` c#
namespace Shuttle.PublishSubscribe.Messages
{
	public class RegisterMember
	{
		public string UserName { get; set; }
	}
}
```

### MemberRegistered

> Add a new class called `MemberRegistered` also with a `UserName` property.

``` c#
namespace Shuttle.PublishSubscribe.Messages
{
	public class MemberRegistered
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Client`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `ShuttleMicrosoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Client
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
                    ConnectionString = "UseDevelopmentStorage=true;"
                });
            });

            Console.WriteLine("Type some characters and then press [enter] to submit; an empty line submission stops execution:");
            Console.WriteLine();

            using (var bus = services.BuildServiceProvider().GetRequiredService<IServiceBus>().Start())
            {
                string userName;

                while (!string.IsNullOrEmpty(userName = Console.ReadLine()))
                {
                    bus.Send(new RegisterMember
                    {
                        UserName = userName
                    });
                }
            }
        }
    }
}
```

### Client configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "MessageRoutes": [
        {
          "Uri": "azuresq://azure/shuttle-server-work",
          "Specifications": [
            {
              "Name": "StartsWith",
              "Value": "Shuttle.PublishSubscribe.Messages"
            }
          ]
        }
      ]
    }
  }
}
```

This tells the service bus that all messages sent having a type name starting with `Shuttle.PublishSubscribe.Messages` should be sent to endpoint `azuresq://azure/shuttle-server-work`.

## Server

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Server`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Hosting` nuget package.

This allows a console application to be hosted using the .NET generic host.

> Install the `ShuttleMicrosoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;
using Shuttle.Esb.Sql.Subscription;

namespace Shuttle.PublishSubscribe.Server
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
                        builder.AddConnectionString("Subscription", "Microsoft.Data.SqlClient");
                    });

                    services.AddSqlSubscription();

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
                })
                .Build()
                .Run();
        }
    }
}
```

### Database

We need a store for our subscriptions.  In this example we will be using **Sql Server**.  Remember to make any required changges to the relevant connection strings.

When you reference the `Shuttle.Esb.Sql.Subscription` package a `scripts` folder is included in the relevant package folder.  Click on the Nuget referenced assembly in the `Dependencies` and navigate to the package folder (in the `Path` property)  to find the `scripts` folder.

The `{version}` bit will be in a `semver` format.

> Create a new database called **Shuttle** and execute the script `{provider}\SubscriptionManagerCreate.sql` in the newly created database.

This will create the required structures that the subscription manager will use to store the subcriptions.  However, this step is optional as the `SqlSubscriptionService` implementation will create any required structures.  In many cases one would need to create the structures manually, such as in production environments, so the script execution process is included for completeness.

Whenever the `Publish` method is invoked on the `ServiceBus` instance the registered `ISubscriptionService` instance is asked for the subscribers to the published message type.  These are retrieved from the Sql Server database for the implementation we are using.

### Server configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "ConnectionStrings": {
    "azure": "UseDevelopmentStorage=true;",
    "Subscription": "server=.;database=shuttle;user id=sa;password=Pass!000;TrustServerCertificate=True"
  },
  "Shuttle": {
    "ServiceBus": {
      "Inbox": {
        "WorkQueueUri": "azuresq://azure/shuttle-server-work",
        "ErrorQueueUri": "azuresq://azure/shuttle-error"
      }
    }
  }
}
```

The Sql Server implementation of the `ISubscriptionService` that we are using by default will try to find a connection string with a name of **Subscription**.  However, you can override this.  See the [documentation](https://shuttle.github.io/shuttle-esb/implementations/subscription/sql.html) for details about how to do this.

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMember>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMember>
	{
		public void ProcessMessage(IHandlerContext<RegisterMember> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();

			context.Publish(new MemberRegistered
			{
				UserName = context.Message.UserName
			});
		}
	}
}
```

This will write out some information to the console window and publish the `MemberRegistered` event message.

## Subscriber

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Subscriber`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Hosting` nuget package.

This allows a console application to be hosted using the .NET generic host.

> Install the `ShuttleMicrosoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;
using Shuttle.Esb.Sql.Subscription;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Subscriber
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
                        builder.AddConnectionString("Subscription", "Microsoft.Data.SqlClient");
                    });

                    services.AddSqlSubscription();

                    services.AddServiceBus(builder =>
                    {
                        configuration.GetSection(ServiceBusOptions.SectionName).Bind(builder.Options);

                        builder.AddSubscription<MemberRegistered>();
                    });

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

Here we add the subscription by calling the `ServiceBusBuilder.AddSubscription<T>` method.  Since we are using the Sql Server implementation of the `ISubscriptionService` interface an entry will be created in the **SubscriberMessageType** table associating the inbox work queue uri with the message type.

It is important to note that in a production environment one would not typically have the subscriber register subscriptions in this manner as we do not want any arbitrary subscriber listening in on the messages being published.  For this reason the connection string should be read-only and the subscription should be registered manually or via a deployment script.  Should the subscription **not** yet exist the creation of the subscription will fail, indicating that the subscription should be registered out-of-band.

### Subscriber configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "ConnectionStrings": {
    "azure": "UseDevelopmentStorage=true;",
    "Subscription": "server=.;database=shuttle;user id=sa;password=Pass!000;TrustServerCertificate=True"
  },
  "Shuttle": {
    "ServiceBus": {
      "Inbox": {
        "WorkQueueUri": "azuresq://azure/shuttle-subscriber-work",
        "ErrorQueueUri": "azuresq://azure/shuttle-error"
      }
    }
  }
}
```

### MemberRegisteredHandler

> Add a new class called `MemberRegisteredHandler` that implements the `IMessageHandler<MemberRegisteredHandler>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Subscriber
{
	public class MemberRegisteredHandler : IMessageHandler<MemberRegistered>
	{
		public void ProcessMessage(IHandlerContext<MemberRegistered> context)
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
