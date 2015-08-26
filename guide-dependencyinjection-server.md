---
title: Dependency Injection Guide - Server
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-dependencyinjection.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-dependencyinjection-server'</script>
# Server

> Add a new `Class Library` to the solution called `Shuttle.DependencyInjection.Server`.

![Dependency Injection Server]({{ site.baseurl }}/assets/images/guide-dependencyinjection-Server.png "Dependency Injection Server")

> Install both the `shuttle-esb-msmq` and `shuttle-esb-castle` nuget packages.

![Dependency Injection Server - Nuget Msmq]({{ site.baseurl }}/assets/images/guide-dependencyinjection-server-nuget-esb.png "Dependency Injection Server - Nuget Msmq")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

It will also include the `WindsorContainer` implementation of the `IMessageHandlerFactory`.

> Install the `shuttle-core-host` nuget package.

![Dependency Injection Server - Nuget Host]({{ site.baseurl }}/assets/images/guide-dependencyinjection-server-nuget-core.png "Dependency Injection Server - Nuget Host")

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add references to both the `Shuttle.DependencyInjection.Messages` and `Shuttle.DependencyInjection.EMail` projects.

## Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

``` c#
using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shuttle.Core.Host;
using Shuttle.DependencyInjection.EMail;
using Shuttle.ESB.Castle;
using Shuttle.ESB.Core;

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
```

## App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
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
using Shuttle.ESB.Core;
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

		public void ProcessMessage(HandlerContext<RegisterMemberCommand> context)
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
```

This will write out some information to the console window.  The injected e-mail service will also be invoked and you'll see the result in the console window.

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the project.</div>

![Dependency Injection Server - Host]({{ site.baseurl }}/assets/images/guide-dependencyinjection-server-host.png "Dependency Injection Server - Host")

Previous: [Client][previous] | Next: [Run][next]

[previous]: {{ site.baseurl }}/guide-dependencyinjection-client
[next]: {{ site.baseurl }}/guide-dependencyinjection-run