---
title: Idempotence Sample
layout: api
---
# Running

When using Visual Studio 2015+ the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a Visual Studio command prompt:

~~~
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.Idempotence
nuget restore
~~~

Once you have opened the `Shuttle.Idempotence.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.Idempotence.Client
- Shuttle.Idempotence.Server

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project for the **Shuttle.Idempotence.Server** project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the solution.</div>

You will also need to create and configure a Sql Server database for this sample and remember to update the **App.config** `connectionString` settings to point to your database.  Please reference the **Database** section below.

# Implementation

When operations, or in our case messages, can be applied multiple with the same result they are said to be **idempotent**.  Idempotence is something you should strive to implement directly on your endpoint by keeping track of some unique property of each message and whether the operation has been completed for that unique property.

An `IIdempotenceService` implementation can assist with this from a technical point-of-view by allowing a particular message id to be handled only once.  This works fine for our ***at-least-once*** delivery mechanism where, in some edge case, we may receive the same message again.  However, it will not aid us when two messages are going to be sent, each with its own message id, but they have contain the same data.

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.Idempotence.Client`
- a **Class Library** called `Shuttle.Idempotence.Server`
- and another **Class Library** called `Shuttle.Idempotence.Messages` that will contain all our message classes

## Messages

> Create a new class library called `Shuttle.Idempotence.Messages` with a solution called `Shuttle.Idempotence`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

~~~ c#
namespace Shuttle.Idempotence.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
~~~

## Client

> Add a new `Console Application` to the solution called `Shuttle.Idempotence.Client`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Add a reference to the `Shuttle.Idempotence.Messages` project.

### Program

> Implement the main client code as follows:

~~~ c#
using System;
using Shuttle.Esb;
using Shuttle.Idempotence.Messages;

namespace Shuttle.Idempotence.Client
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
					var command = new RegisterMemberCommand
					{
						UserName = userName
					};

					var transportMessage = bus.CreateTransportMessage(command, null);

					for (var i = 0; i < 5; i++)
					{
						bus.Dispatch(transportMessage); // will be processed once since message id is the same
					}

					bus.Send(command); // will be processed since it has a new message id
					bus.Send(command); // will also be processed since it also has a new message id
				}
			}
		}
	}
}
~~~

Keep in mind that the when you `Send` a message a `TransportMessage` envelope is created with a unique message id (`Guid`).  In the above code we first manually create a `TransportMessage` so that we can send technically identical messages.

The next two `Send` operations do not use the `TransportMessage` but rather send individual messages.  These will each have a `TransportMessage` envelope and, therefore, each have its own unique message id.

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
				<add specification="StartsWith" value="Shuttle.Idempotence.Messages" />
			</messageRoute>
		</messageRoutes>		
	</serviceBus>
	
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>
</configuration>
~~~

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.Idempotence.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

## Server

> Add a new `Class Library` to the solution called `Shuttle.Idempotence.Server`.

> Install noth the `Shuttle.Esb.Msmq` and `Shuttle.Esb.SqlServer` nuget packages.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

We will also have access to the Sql Server implementation of the `IIdempotenceService`.

> Install the `Shuttle.Core.Host` nuget package.

The default mechanism used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.Host` executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.Idempotence.Messages` project.

### Host

> Rename the default `Class1` file to `Host` and implement the `IHost` and `IDisposabe` interfaces as follows:

~~~ c#
using System;
using Shuttle.Core.Host;
using Shuttle.Esb;
using Shuttle.Esb.SqlServer.Idempotence;

namespace Shuttle.Idempotence.Server
{
	public class Host : IHost, IDisposable
	{
		private IServiceBus _bus;

		public void Start()
		{
			_bus = ServiceBus.Create(c=>c.IdempotenceService(IdempotenceService.Default())).Start();
		}

		public void Dispose()
		{
			_bus.Dispose();
		}
	}
}
~~~

### Database

We need a store for our idempotence tracking.  In this example we will be using **Sql Server**.  If you use the express version remember to change the `data source` value to `.\sqlexpress` from the standard `.`.

When you reference the `Shuttle.Esb.SqlServer` package a number of scripts are included in the relevant package folder:

- `.\Shuttle.PublishSubscribe\packages\Shuttle.Esb.SqlServer.{version}\scripts`

The `{version}` bit will be in a `semver` format.

> Create a new database called **Shuttle** and execute script `IdempotenceServiceCreate.sql` in the newly created database.

Whenever `Publish` is called the registered `ISubscriptionManager` instance is asked for the subscribers to the published message type.  These are retrieved from the Sql Server database for the implementation we are using.

### App.config

> Add an `Application Configuration File` item to create the `App.config` and populate as follows:

~~~ xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
	</configSections>

	<connectionStrings>
		<add name="Idempotence"
			 connectionString="Data Source=.;Initial Catalog=shuttle;Integrated Security=SSPI;"
			 providerName="System.Data.SqlClient"/>
	</connectionStrings>

	<serviceBus>
		 <inbox
			workQueueUri="msmq://./shuttle-server-work"
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
~~~

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

~~~ c#
using System;
using Shuttle.Esb;
using Shuttle.Idempotence.Messages;

namespace Shuttle.Idempotence.Server
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
		{
			Console.WriteLine();
			Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}' / message id = '{1}'",
				context.Message.UserName,
				context.TransportMessage.MessageId);
			Console.WriteLine();
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
~~~

This will write out some information to the console window.

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the solution.</div>

## Run

> Set both the client and server projects as the startup.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

<div class='alert alert-info'>You will need to scroll through the message but you will observe that the <strong>server</strong> application has processed the three messages.</div>

You have now implemented message idempotence.

[transport-message]: {{ site.baseurl }}/transport-message
