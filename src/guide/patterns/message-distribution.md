# Message Distribution

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

This sample makes use of [Shuttle.Esb.azuresq](https://github.com/Shuttle/Shuttle.Esb.azuresq) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

Once you have opened the `Shuttle.Distribution.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.Distribution.Client
- Shuttle.Distribution.Server
- Shuttle.Distribution.Worker

## Implementation

When you find that a single endpoint, even with many threads, cannot keep up with the required processing and is falling behind you can opt for message distribution.

::: info
When using a broker architecture (such as RabbitMQ, Azure Storage Queues, or Amazon SQS) you do not need to use message distribution as workers can all access the same inbox work queue.  In this case you could simply scale horizontally.
:::

::: warning
Keep in mind that deferred queues are required for each endpoint instance and cannot be shared.
:::

Plesae note that the project structure here is used as a sample to facilitate the execution of the solution.  In a real-world scenario the endpoint project would not be separated into a distributor and a worker; rather, there would be a single implementation and you would simply install the service multiple times on, possibly, multiple machines and then configure the workers and distributor as such.  When using the distribution mechanism there is always a **1 to *N*** relationship between the distribution endpoint and the worker(s).

However, for a broker-style queueing mechanism you do not need to use any distribution as the broker would have a consumer for each thread irrespective of where it originates from.

In this guide we'll create the following projects:

- `Shuttle.Distribution.Client` (**Console Application**)
- `Shuttle.Distribution.Server` (**Console Application**)
- `Shuttle.Distribution.Worker` (**Console Application**)
- `Shuttle.Distribution.Messages` (**Class Library**)

## Messages

> Create a new class library called `Shuttle.Distribution.Messages` with a solution called `Shuttle.Distribution`

**Note**: remember to change the *Solution name*.

### RegisterMember

> Rename the `Class1` default file to `RegisterMember` and add a `UserName` property.

``` c#
namespace Shuttle.Distribution.Messages
{
	public class RegisterMember
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.Distribution.Client`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `ShuttleMicrosoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

> Add a reference to the `Shuttle.Distribution.Messages` project.

### Program

> Implement the main client code as follows:

```c#
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Distribution.Messages;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;

namespace Shuttle.Distribution.Client
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
              "Value": "Shuttle.Distribution.Messages"
            }
          ]
        }
      ]
    }
  }
}
```

This tells the service bus that all messages sent having a type name starting with `Shuttle.Distribution.Messages` should be sent to endpoint `azuresq://azure/shuttle-server-work`.

## Server

> Add a new `Console Application` to the solution called `Shuttle.Distribution.Server`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Hosting` nuget package.

This allows a console application to be hosted using the .NET generic host.

> Install the `ShuttleMicrosoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

### Program

> Implement the `Program` class as follows:

```c#
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;

namespace Shuttle.Distribution.Server
{
    public class Program
    {
        public static void Main()
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
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
                })
                .Build()
                .Run();
        }
    }
}
```

### Server configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "ConnectionStrings": {
    "azure": "UseDevelopmentStorage=true;"
  },
  "Shuttle": {
    "ServiceBus": {
      "ControlInbox": {
        "WorkQueueUri": "azuresq://azure/shuttle-server-control-inbox-work",
        "ErrorQueueUri": "azuresq://azure/shuttle-error"
      },
      "Inbox": {
        "Distribute": true,
        "WorkQueueUri": "azuresq://azure/shuttle-server-work",
        "ErrorQueueUri": "azuresq://azure/shuttle-error"
      }
    }
  }
}
```

This will instruct the endpoint to ***only** distribute messages since the `distribute` attribute is set to `true`.  If it is set to `false` then the endpoint will process incoming messages if a worker thread is not available.

It also configures the control inbox that the endpoint will use to process administrative messages.

## Worker

> Add a new `Console Application` to the solution called `Shuttle.Distribution.Worker`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Hosting` nuget package.

This allows a console application to be hosted using the .NET generic host.

> Install the `ShuttleMicrosoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

> Add a reference to the `Shuttle.Distribution.Messages` project.

### Program

> Implement the `Program` class as follows:

``` c#
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;

namespace Shuttle.Distribution.Worker
{
    public class Program
    {
        public static void Main()
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
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
                })
                .Build()
                .Run();
        }
    }
}
```

### Server configuration file

> Add an `appsettings.json` file as follows:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "Inbox": {
        "WorkQueueUri": "azuresq://azure/shuttle-worker-work",
        "ErrorQueueUri": "azuresq://azure/shuttle-error"
      },
      "Worker": {
        "DistributorControlInboxWorkQueueUri": "azuresq://azure/shuttle-server-control-inbox-work"
      } 
    }
  }
}
```

This configures the endpoint as a worker and specifies the control inbox of the distributor that will be notified when a thread is available to perform work.

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMember>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.Distribution.Messages;

namespace Shuttle.Distribution.Worker
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMember>
	{
		public void ProcessMessage(IHandlerContext<RegisterMember> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED --- WORKER] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();
		}
	}
}
```

This will write out some information to the console window.

## Run

> Set both the client, server, and worker projects as startup projects.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

::: info
You will observe that the server application forwards the message to the worker.
:::

::: info
The worker application will perform the actual processing.
:::

You have now implemented message distribution.
