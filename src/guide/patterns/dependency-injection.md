# Dependency Injection

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

This sample makes use of [Shuttle.Esb.AzureStorageQueues](https://github.com/Shuttle/Shuttle.Esb.AzureStorageQueues) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

Once you have opened the `Shuttle.DependencyInjection.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.DependencyInjection.Client
- Shuttle.DependencyInjection.Server

## Implementation

Since .NET provides a production-ready dependency injection framework, this sample will demonstrate a very simple use-case.

In this guide we'll create the following projects:

- `Shuttle.DependencyInjection.Client` (**Console Application**)
- `Shuttle.DependencyInjection.Server` (**Console Application**)
- `Shuttle.DependencyInjection.EMail` (**Class Library**)
- `Shuttle.DependencyInjection.Messages` (**Class Library**)

## Messages

> Create a new class library called `Shuttle.DependencyInjection.Messages` with a solution called `Shuttle.DependencyInjection`

**Note**: remember to change the *Solution name*.

### RegisterMember

> Rename the `Class1` default file to `RegisterMember` and add a `UserName` property.

``` c#
namespace Shuttle.DependencyInjection.Messages
{
	public class RegisterMember
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.DependencyInjection.Client`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

> Add a reference to the `Shuttle.DependencyInjection.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.DependencyInjection.Messages;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;

namespace Shuttle.DependencyInjection.Client
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
              "Value": "Shuttle.DependencyInjection.Messages"
            }
          ]
        }
      ]
    }
  }
}
```

This tells Shuttle.Esb that all messages that are sent and have a type name starting with `Shuttle.DependencyInjection.Messages` should be sent to endpoint `azuresq://azure/shuttle-server-work`.

## E-Mail

To demonstrate the dependency injection we will create a fake e-mail service that we intend using in the server endpoint.

> Add a new `Class Library` to the solution called `Shuttle.DependencyInjection.EMail`.

### IEMailService

> Add an interface called `IEMailService` and implement it as follows:

``` c#
namespace Shuttle.DependencyInjection.EMail
{
	public interface IEMailService
	{
		void Send(string name);
	}
}
```

### EMailService

> Rename the default `Class1` file to `EMailService` and implement the `IEMailService` interfaces as follows:

``` c#
using System;
using System.Threading;

namespace Shuttle.DependencyInjection.EMail
{
	public class EMailService : IEMailService
	{
		public void Send(string name)
		{
			Console.WriteLine();
			Console.WriteLine("[SENDING E-MAIL] : name = '{0}'", name);
			Console.WriteLine();

			Thread.Sleep(3000); // simulate communication wait time

			Console.WriteLine();
			Console.WriteLine("[E-MAIL SENT] : name = '{0}'", name);
			Console.WriteLine();
		}
	}
}
```

## Server

> Add a new `Console Application` to the solution called `Shuttle.DependencyInjection.Server`.

> Install the `Shuttle.Esb.AzureStorageQueues` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Microsoft.Extensions.Hosting` nuget package.

This allows a console application to be hosted using the .NET generic host.

> Install the `Microsoft.Extensions.Configuration.Json` nuget package.

This will provide the ability to read the `appsettings.json` file.

> Add references to both the `Shuttle.DependencyInjection.Messages` and `Shuttle.DependencyInjection.EMail` projects.

### Program

> Implement the `Program` class as follows:

``` c#
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shuttle.DependencyInjection.EMail;
using Shuttle.Esb;
using Shuttle.Esb.AzureStorageQueues;

namespace Shuttle.DependencyInjection.Server
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

                    services.AddSingleton<IEMailService, EMailService>();

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
      "Inbox": {
        "WorkQueueUri": "azuresq://azure/shuttle-server-work",
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
using Shuttle.Core.Contract;
using Shuttle.DependencyInjection.EMail;
using Shuttle.Esb;
using Shuttle.DependencyInjection.Messages;

namespace Shuttle.DependencyInjection.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMember>
	{
		private readonly IEMailService _emailService;

		public RegisterMemberHandler(IEMailService emailService)
		{
			Guard.AgainstNull(emailService, nameof(emailService));

			_emailService = emailService;
		}

		public void ProcessMessage(IHandlerContext<RegisterMember> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();

			_emailService.Send(context.Message.UserName);
		}
	}
}
```

This will write out some information to the console window.  The injected e-mail service will also be invoked and you'll see the result in the console window.

## Run

> Set both the client and server projects as the startup.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

::: info
You will notice that the server application has processed the message and simulated sending an e-mail though the `IEMailService` implementation.
:::

You have now implemented dependency injection for message handlers.
