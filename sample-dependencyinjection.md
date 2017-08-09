---
title: Dependency Injection Sample
layout: api
---

<div class='alert alert-info'>Remember to download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.</div>

# Running

When using Visual Studio 2015+ the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a Visual Studio command prompt:

```
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.DependencyInjection
nuget restore
```

Once you have opened the `Shuttle.DependencyInjection.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.DependencyInjection.Client
- Shuttle.DependencyInjection.Server

# Implementation

By default Shuttle.Esb does not require a dependency injection container.  Shuttle makes use of an `IMessageHandlerFactory` implementation to create message handlers.  If no dependency injection container is required one could stick with the `DefaultMessageHandlerFactory` instantiated by default.

The `DefaultMessageHandlerFactory` requires message handlers that have a default (parameterless) constructor; else the instantiation of the handler will fail.  In this guide we will use the `WindsorContainer` that is part of the [Castle Project](https://github.com/castleproject/Windsor/blob/master/docs/README.md).

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.DependencyInjection.Client`
- a **Class Library** called `Shuttle.DependencyInjection.Server`
- another **Class Library** called `Shuttle.DependencyInjection.EMail` that will contain a fake e-mail service implementation
- and another **Class Library** called `Shuttle.DependencyInjection.Messages` that will contain all our message classes

## Messages

> Create a new class library called `Shuttle.DependencyInjection.Messages` with a solution called `Shuttle.DependencyInjection`

**Note**: remember to change the *Solution name*.

## RegisterMemberCommand

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

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Ninject` nuget package.

This will add the [Ninject](http://www.ninject.org/) implementation of the [component container](http://shuttle.github.io/shuttle-core/overview-container/) interfaces.

> Add a reference to the `Shuttle.DependencyInjection.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Ninject;
using Shuttle.Core.Ninject;
using Shuttle.DependencyInjection.Messages;
using Shuttle.Esb;

namespace Shuttle.DependencyInjection.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var container = new NinjectComponentContainer(new StandardKernel());

			ServiceBus.Register(container);

			using (var bus = ServiceBus.Create(container).Start())
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
				<add specification="StartsWith" value="Shuttle.DependencyInjection.Messages" />
			</messageRoute>
		</messageRoutes>		
	</serviceBus>
	
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>
</configuration>
```

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.DependencyInjection.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

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

> Add a new `Class Library` to the solution called `Shuttle.DependencyInjection.Server`.

> Install both the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Ninject` nuget package.

This will add the [Ninject](http://www.ninject.org/) implementation of the [component container](http://shuttle.github.io/shuttle-core/overview-container/) interfaces.

> Install the `Shuttle.Core.ServiceHost` nuget package.

The [default mechanism](http://shuttle.github.io/shuttle-core/overview-service-host/) used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.ServiceHost` in our console executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add references to both the `Shuttle.DependencyInjection.Messages` and `Shuttle.DependencyInjection.EMail` projects.

### Host

> Rename the default `Class1` file to `Host` and implement the `IServiceHost` interface as follows:

``` c#
using Ninject;
using Shuttle.Core.Ninject;
using Shuttle.Core.ServiceHost;
using Shuttle.DependencyInjection.EMail;
using Shuttle.Esb;

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

            ServiceBus.Register(container);

            _bus = ServiceBus.Create(container).Start();
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
	</configSections>

	<serviceBus>
		<inbox
		   workQueueUri="msmq://./shuttle-server-work"
		   errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

## RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.Core.Infrastructure;
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

<div class='alert alert-info'>You will notice that the <strong>server</strong> application has processed the message and simulate sending an e-mail though the <strong>IEMailService</strong> implementation.</div>

You have now implemented dependency injection for message handlers.

[transport-message]: {{ site.baseurl }}/transport-message
