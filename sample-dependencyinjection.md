---
title: Dependency Injection Sample
layout: api
---
# Running

When using Visual Studio 2015+ the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a Visual Studio command prompt:

~~~
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.DependencyInjection
nuget restore
~~~

Once you have opened the `Shuttle.DependencyInjection.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.DependencyInjection.Client
- Shuttle.DependencyInjection.Server

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project for the **Shuttle.DependencyInjection.Server** project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the solution.</div>

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

~~~ c#
namespace Shuttle.DependencyInjection.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
~~~

## Client

> Add a new `Console Application` to the solution called `Shuttle.DependencyInjection.Client`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Add a reference to the `Shuttle.DependencyInjection.Messages` project.

### Program

> Implement the main client code as follows:

~~~ c#
using System;
using Shuttle.Esb;
using Shuttle.DependencyInjection.Messages;

namespace Shuttle.DependencyInjection.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var bus = ServiceBus.Create().Start())
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
~~~

The message sent will have its `IgnoreTilleDate` set to 5 seconds into the future.  You can have a look at the [TransportMessage][transport-message] for more information on the message structure.

### App.config

> Create the shuttle configuration as follows:

~~~ xml
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
~~~

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.DependencyInjection.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

## E-Mail

To demonstrate the dependency injection we will create a fake e-mail service that we intend using in the server endpoint.

> Add a new `Class Library` to the solution called `Shuttle.DependencyInjection.EMail`.

### IEMailService

> Add an interface called `IEMailService` and implement it as follows:

~~~ c#
namespace Shuttle.DependencyInjection.EMail
{
	public interface IEMailService
	{
		void Send(string name);
	}
}
~~~

### EMailService

> Rename the default `Class1` file to `EMailService` and implement the `IEMailService` interfaces as follows:

~~~ c#
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
~~~

## Server

> Add a new `Class Library` to the solution called `Shuttle.DependencyInjection.Server`.

> Install both the `Shuttle.Esb.Msmq` and `Shuttle.Esb.Castle` nuget packages.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

It will also include the `WindsorContainer` implementation of the `IMessageHandlerFactory`.

> Install the `Shuttle.Core.Host` nuget package.

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add references to both the `Shuttle.DependencyInjection.Messages` and `Shuttle.DependencyInjection.EMail` projects.

### Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

~~~ c#
using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shuttle.Core.Host;
using Shuttle.DependencyInjection.EMail;
using Shuttle.Esb.Castle;
using Shuttle.Esb;

namespace Shuttle.DependencyInjection.Server
{
	public class Host : IHost, IDisposable
	{
		private IServiceBus _bus;
		private WindsorContainer _container;

		public void Start()
		{
			_container = new WindsorContainer();

			_container.Register(Component.For<IEMailService>().ImplementedBy<EMailService>());

			// register all the message handler in this assembly
			_container.Register(
				Classes.FromThisAssembly()
				.BasedOn(typeof(IMessageHandler<>))
				.WithServiceFromInterface(typeof(IMessageHandler<>))
				.LifestyleTransient()
				);

			_bus = ServiceBus.Create(
				c => c.MessageHandlerFactory(new CastleMessageHandlerFactory(_container))
				).Start();
		}

		public void Dispose()
		{
			_bus.Dispose();
		}
	}
}
~~~

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

~~~ xml
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
~~~

## RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

~~~ c#
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

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
~~~

This will write out some information to the console window.  The injected e-mail service will also be invoked and you'll see the result in the console window.

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the solution.</div>

## Run

> Set both the client and server projects as the startup.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

<div class='alert alert-info'>You will notice that the <strong>server</strong> application has processed the message and simulate sending an e-mail though the <strong>IEMailService</strong> implementation.</div>

You have now implemented dependency injection for message handlers.

[transport-message]: {{ site.baseurl }}/transport-message
