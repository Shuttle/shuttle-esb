---
title: Publish / Subscribe Sample
layout: api
---

<div class='alert alert-info'>Remember to download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.</div>

# Running

When using Visual Studio 2017 the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a Visual Studio command prompt:

```
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.PublishSubscribe
nuget restore
```

Once you have opened the `Shuttle.PublishSubscribe.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.PublishSubscribe.Client
- Shuttle.PublishSubscribe.Server
- Shuttle.PublishSubscribe.Subscriber

You will also need to create and configure a Sql Server database for this sample and remember to update the **App.config** `connectionString` settings to point to your database.  Please reference the **Database** section below.

# Implementation

**Events** are interesting things that happen in our system that other systems may be interested in.  There may be **0..*****N*** number of subscribers for an event.  Typically there should be at least one subscriber for an event else it isn't really carrying its own weight.

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.PublishSubscribe.Client`
- a **Console Application** called `Shuttle.PublishSubscribe.Server`
- a **Console Application** called `Shuttle.PublishSubscribe.Subscriber` that will represent a subscriber of our event message
- a **Class Library** called `Shuttle.PublishSubscribe.Messages` that will contain all our message classes

## Messages

> Create a new class library called `Shuttle.PublishSubscribe.Messages` with a solution called `Shuttle.PublishSubscribe`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.PublishSubscribe.Messages
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
namespace Shuttle.PublishSubscribe.Messages
{
	public class MemberRegisteredEvent
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Client`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.StructureMap` nuget package.

This will provide access to the StructureMap dependency injection container.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Shuttle.Core.StructureMap;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;
using StructureMap;

namespace Shuttle.PublishSubscribe.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var smRegistry = new Registry();
			var registry = new StructureMapComponentRegistry(smRegistry);

			ServiceBus.Register(registry);

			using (var bus = ServiceBus.Create(new StructureMapComponentResolver(new Container(smRegistry))).Start())
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
				<add specification="StartsWith" value="Shuttle.PublishSubscribe.Messages" />
			</messageRoute>
		</messageRoutes>		
	</serviceBus>
</configuration>
```

This tells Shuttle that all messages that are sent and have a type name starting with `Shuttle.PublishSubscribe.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

## Server

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Server`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.StructureMap` nuget package.

This will provide access to the StructureMap dependency injection container.

> Install the `Shuttle.Esb.Sql.Subscription` nuget package.

This will provide access to the Sql-based `ISubscriptionManager` implementation.

> Install the `Shuttle.Core.ServiceHost` nuget package.

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.ServiceHost` assembly we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

### Program

Implement the `Program` class as follows:

``` c#
using Shuttle.Core.ServiceHost;

namespace Shuttle.PublishSubscribe.Server
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

This will simply run the `Host` implementation.

### Host

> Add a `Host` class and implement the `IServiceHost` interface as follows:

``` c#
using Shuttle.Core.ServiceHost;
using Shuttle.Core.StructureMap;
using Shuttle.Esb;
using StructureMap;

namespace Shuttle.PublishSubscribe.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;

        public void Start()
        {
            var smRegistry = new Registry();
            var registry = new StructureMapComponentRegistry(smRegistry);

            ServiceBus.Register(registry);

            _bus = ServiceBus.Create(new StructureMapComponentResolver(new Container(smRegistry))).Start();
        }

        public void Stop()
        {
            _bus.Dispose();
        }
    }
}
```

### Database

We need a store for our subscriptions.  In this example we will be using **Sql Server**.  If you use the express version remember to change the `data source` value to `.\sqlexpress` from the standard `.`.

When you reference the `Shuttle.Esb.Sql.Subscription` package a `scripts` folder is included in the relevant package folder.  Click on the Nuget referenced assembly in the `References` or `Dependencies` (depending on your project type) and navigate to the package folder to find the `scripts` folder.

The `{version}` bit will be in a `semver` format.

> Create a new database called **Shuttle** and execute the script `System.Data.SqlClient\SubscriptionManagerCreate.sql` in the newly created database.

This will create the required structures that the subscription manager will use to store the subcriptions.

Whenever `Publish` is called the registered `ISubscriptionManager` instance is asked for the subscribers to the published message type.  These are retrieved from the Sql Server database for the implementation we are using.

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
	</configSections>

	<connectionStrings>
		<add name="Subscription"
			 connectionString="Data Source=.;Initial Catalog=shuttle;Integrated Security=SSPI;"
			 providerName="System.Data.SqlClient"/>
	</connectionStrings>

	<serviceBus>
		 <inbox
			workQueueUri="msmq://./shuttle-server-work"
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

The Sql Server implementation of the `ISubscriptionManager` that we are using by default will try to find a connection string with a name of **Subscription**.  However, you can override this.  See the [Sql Server configuration options][sql-server] for details about how to do this.

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName);
			Console.WriteLine();

			context.Publish(new MemberRegisteredEvent
			{
				UserName = context.Message.UserName
			});
		}
	}
}
```

This will write out some information to the console window and publish the `MemberRegisteredEvent` message.

## Subscriber

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Subscriber`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.StructureMap` nuget package.

This will provide access to the StructureMap dependency injection container.

> Install the `Shuttle.Esb.Sql.Subscription` nuget package.

This will provide access to the Sql-based `ISubscriptionManager` implementation.

> Install the `Shuttle.Core.ServiceHost` nuget package.

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.ServiceHost` assembly we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

### Program

Implement the `Program` class as follows:

``` c#
using Shuttle.Core.ServiceHost;

namespace Shuttle.PublishSubscribe.Server
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

This will simply run the `Host` implementation.

### Host

> Add a `Host` class and implement the `IServiceHost` interface as follows:

``` c#
using Shuttle.Core.Container;
using Shuttle.Core.ServiceHost;
using Shuttle.Core.StructureMap;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;
using StructureMap;

namespace Shuttle.PublishSubscribe.Subscriber
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;

        public void Start()
        {
            var structureMapRegistry = new Registry();
            var registry = new StructureMapComponentRegistry(structureMapRegistry);

            ServiceBus.Register(registry);

            var resolver = new StructureMapComponentResolver(new Container(structureMapRegistry));

            resolver.Resolve<ISubscriptionManager>().Subscribe<MemberRegisteredEvent>();

            _bus = ServiceBus.Create(resolver).Start();
        }

        public void Stop()
        {
            _bus.Dispose();
        }
    }
}
```

Here we register the subscription by calling the `ISubscriptionManager` implementation's `Subscribe<MemberRegisteredEvent>();` method.  Since we are using the Sql Server implementation of the `ISubscriptionManager` interface an entry will be created in the **SubscriberMessageType** table associating the inbox work queue uri with the message type.

It is important to note that in a production environment one would not typically register subscriptions in this manner as they may be somewhat more sensitive as we do not want any arbitrary subscriber listening in on the messages being published.  For this reason the connection string should be read-only and the subscription should be registered manually or via a deployment script.  Should the subscription **not** yet exist the creation of the subscription will fail, indicating that there is work to be done.

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
	</configSections>

	<connectionStrings>
		<add name="Subscription"
			 connectionString="Data Source=.;Initial Catalog=shuttle;Integrated Security=SSPI;"
			 providerName="System.Data.SqlClient"/>
	</connectionStrings>

	<serviceBus>
		 <inbox
			workQueueUri="msmq://./shuttle-subscriber-work"
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

### MemberRegisteredHandler

> Add a new class called `MemberRegisteredHandler` that implements the `IMessageHandler<MemberRegisteredHandler>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Server
{
	public class MemberRegisteredHandler : IMessageHandler<MemberRegisteredEvent>
	{
		public void ProcessMessage(IHandlerContext<MemberRegisteredEvent> context)
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

> Set both the client and server projects as the startup.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

<div class='alert alert-info'>You will observe that the <strong>server</strong> application has processed the message.</div>

<div class='alert alert-info'>The <strong>subscriber</strong> application will then process the event published by the <strong>server</strong>.</div>

You have now completed a full publish / subscribe call chain.
