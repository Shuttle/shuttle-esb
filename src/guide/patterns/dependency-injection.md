# Dependency Injection

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

This sample makes use of [Shuttle.Esb.AzureMQ](https://github.com/Shuttle/Shuttle.Esb.AzureMQ) for the message queues.  Local Azure Storage Queues should be provided by [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio).

Once you have opened the `Shuttle.DependencyInjection.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.DependencyInjection.Client
- Shuttle.DependencyInjection.Server

## Implementation

By default Shuttle.Esb does not require a dependency injection container.  Shuttle makes use of an `IMessageHandlerFactory` implementation to create message handlers.  If no dependency injection container is required one could stick with the `DefaultMessageHandlerFactory` instantiated by default.

The `DefaultMessageHandlerFactory` requires message handlers that have a default (parameterless) constructor; else the instantiation of the handler will fail.  In this guide we will use the `WindsorContainer` that is part of the [Castle Project](https://github.com/castleproject/Windsor/blob/master/docs/README.md).

In this guide we'll create the following projects:

- `Shuttle.DependencyInjection.Client` (**Console Application**)
- `Shuttle.DependencyInjection.Server` (**Console Application**)
- `Shuttle.DependencyInjection.EMail` (**Class Library**)
- `Shuttle.DependencyInjection.Messages` (**Class Library**)

## Messages

> Create a new class library called `Shuttle.DependencyInjection.Messages` with a solution called `Shuttle.DependencyInjection`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.DependencyInjection.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.DependencyInjection.Client`.

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Ninject` nuget package.

This will provide access to the Ninject implementation.

> Add a reference to the `Shuttle.DependencyInjection.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Ninject;
using Shuttle.Core.Container;
using Shuttle.Core.Ninject;
using Shuttle.DependencyInjection.Messages;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;

namespace Shuttle.DependencyInjection.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = new NinjectComponentContainer(new StandardKernel());

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
                    });
                }
            }
        }
    }
}
```

### App.config

> Create the service bus configuration as follows:

``` xml
<?xml version="1.0" encoding="utf-8"?>

<configuration>
	<configSections>
		<section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb" />
	</configSections>

	<appSettings>
		<add key="azure" value="UseDevelopmentStorage=true;" />
	</appSettings>

	<serviceBus>
		<messageRoutes>
			<messageRoute uri="azuremq://azure/shuttle-server-work">
				<add specification="StartsWith" value="Shuttle.DependencyInjection.Messages" />
			</messageRoute>
		</messageRoutes>
	</serviceBus>
</configuration>```

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.DependencyInjection.Messages` should be sent to endpoint `azuremq://azure/shuttle-server-work`.

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

> Install the `Shuttle.Esb.AzureMQ` nuget package.

This will provide access to the Azure Storage Queues `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.WorkerService` nuget package.

This allows a console application to be hosted as a Windows Service or Systemd Unit while running as a normal console application when debugging.

> Install the `Shuttle.Core.Ninject` nuget package.

This will provide access to the Ninject implementation.

> Add references to both the `Shuttle.DependencyInjection.Messages` and `Shuttle.DependencyInjection.EMail` projects.

### Program

> Implement the `Program` class as follows:

``` c#
using Shuttle.Core.WorkerService;

namespace Shuttle.DependencyInjection.Server
{
    public class Program
    {
        public static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }
}
```

This simply executes the `Host` class implementation.

### Host

> Add a `Host` class and implement the `IServiceHost` interface as follows:

``` c#
using Ninject;
using Shuttle.Core.Container;
using Shuttle.Core.Ninject;
using Shuttle.Core.WorkerService;
using Shuttle.DependencyInjection.EMail;
using Shuttle.Esb;
using Shuttle.Esb.AzureMQ;

namespace Shuttle.DependencyInjection.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;
        private StandardKernel _kernel;

        public void Stop()
        {
            _kernel.Dispose();
            _bus.Dispose();
        }

        public void Start()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IEMailService>().To<EMailService>();

            var container = new NinjectComponentContainer(_kernel);

            container.Register<IAzureStorageConfiguration, DefaultAzureStorageConfiguration>();
            container.RegisterServiceBus();

            _bus = container.Resolve<IServiceBus>().Start();
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
	</configSections>

	<appSettings>
		<add key="azure" value="UseDevelopmentStorage=true;" />
	</appSettings>

	<serviceBus>
		<inbox workQueueUri="azuremq://azure/shuttle-server-work" errorQueueUri="azuremq://azure/shuttle-error" />
	</serviceBus>
</configuration>
```

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.Core.Contract;
using Shuttle.DependencyInjection.EMail;
using Shuttle.Esb;
using Shuttle.DependencyInjection.Messages;

namespace Shuttle.DependencyInjection.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		private readonly IEMailService _emailService;

		public RegisterMemberHandler(IEMailService emailService)
		{
			Guard.AgainstNull(emailService, "emailService");

			_emailService = emailService;
		}

		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
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
