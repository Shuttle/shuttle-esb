---
title: Request / Response Sample
layout: api
---

<iframe width="560" height="315" src="https://www.youtube.com/embed/ysolQC51YVU" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>

<div class='alert alert-info'>Remember to download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.</div>

# Running

When using Visual Studio 2017 the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a Visual Studio command prompt:

```
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.RequestResponse
nuget restore
```

Once you have opened the `Shuttle.RequestResponse.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.RequestResponse.Client
- Shuttle.RequestResponse.Server

# Implementation

In order to get any processing done in Shuttle.Esb a message will need to be generated and sent to an endpoint for processing.  The idea behind a **command** message is that there is exactly **one** endpoint handling the message.  Since it is an instruction the message absolutely ***has*** to be handled and we also need to have only a single endpoint process the message to ensure a consistent result.

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.RequestResponse.Client`
- a **Console Application** called `Shuttle.RequestResponse.Server`
- a **Class Library** called `Shuttle.RequestResponse.Messages` that will contain all our message classes

## Messages

> Create a new class library called `Shuttle.RequestResponse.Messages` with a solution called `Shuttle.RequestResponse`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.RequestResponse.Messages
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
namespace Shuttle.RequestResponse.Messages
{
	public class MemberRegisteredEvent
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.RequestResponse.Client`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Castle` nuget package.

This will provide access to the Castle `WindsorContainer` implementation.

> Add a reference to the `Shuttle.RequestResponse.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Castle.Windsor;
using Shuttle.Core.Castle;
using Shuttle.Esb;
using Shuttle.RequestResponse.Messages;

namespace Shuttle.RequestResponse.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var container = new WindsorComponentContainer(new WindsorContainer());

			ServiceBus.Register(container);

			using (var bus = ServiceBus.Create(container).Start())
			{
				string userName;

				while (!string.IsNullOrEmpty(userName = Console.ReadLine()))
				{
					bus.Send(new RegisterMemberCommand
					{
						UserName = userName
					}, c => c.WillExpire(DateTime.Now.AddSeconds(5)));
				}
			}
		}
	}
}
```

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
				<add specification="StartsWith" value="Shuttle.RequestResponse.Messages" />
			</messageRoute>
		</messageRoutes>		

		<inbox
		   workQueueUri="msmq://./shuttle-client-work"
		   errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

This tells Shuttle that all messages that are sent and have a type name starting with `Shuttle.RequestResponse.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

### MemberRegisteredHandler

> Create a new class called `MemberRegisteredHandler` that implements the `IMessageHandler<MemberRegisteredEvent>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.RequestResponse.Messages;

namespace Shuttle.RequestResponse.Client
{
	public class MemberRegisteredHandler : IMessageHandler<MemberRegisteredEvent>
	{
		public void ProcessMessage(IHandlerContext<MemberRegisteredEvent> context)
		{
			Console.WriteLine();
			Console.WriteLine("[RESPONSE RECEIVED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();
		}
	}
}
```

## Server

> Add a new `Console Application` to the solution called `Shuttle.RequestResponse.Server`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.ServiceHost` nuget package.

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.ServiceHost` we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Install the `Shuttle.Core.Castle` nuget package.

This will provide access to the Castle `WindsorContainer` implementation.

> Install the `Shuttle.Core.Log4Net` nuget package.

This will provide access to the `Log4Net` implementation.

> Add a reference to the `Shuttle.RequestResponse.Messages` project.

### Host

> Implement the `Program` class as follows:

``` c#
using Shuttle.Core.ServiceHost;

namespace Shuttle.RequestResponse.Server
{
    internal class Program
    {
        private static void Main()
        {
            ServiceHost.Run<Host>();
        }
    }
}
```

This will simply instance the `Host` class and get it running.

> Add a `Host` class and implement the `IServiceHost` and `IDisposabe` interfaces as follows:

``` c#
using Castle.Windsor;
using log4net;
using Shuttle.Core.Castle;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.ServiceHost;
using Shuttle.Esb;

namespace Shuttle.RequestResponse.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;

        public Host()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));
        }

        public void Start()
        {
            var container = new WindsorComponentContainer(new WindsorContainer());

            ServiceBus.Register(container);

            _bus = ServiceBus.Create(container).Start();
        }

        public void Stop()
        {
            _bus.Dispose();
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

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.RequestResponse.Messages;

namespace Shuttle.RequestResponse.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();

			context.Send(new MemberRegisteredEvent
			{
				UserName = context.Message.UserName
			}, c => c.Reply());
		}
	}
}
```

## Run

> Set both the client and server projects as startup projects.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

<div class='alert alert-info'>You will observe that the <strong>server</strong> application has processed the message.</div>

<div class='alert alert-info'>The <strong>client</strong> application will then process the response returned by the <strong>server</strong>.</div>

You have now completed a full request / response call chain.
