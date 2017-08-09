---
title: Message Distribution Sample
layout: api
---

<div class='alert alert-info'>Remember to download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.</div>

# Running

When using Visual Studio 2015+ the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a Visual Studio command prompt:

```
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.Distribution
nuget restore
```

Once you have opened the `Shuttle.Distribution.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.Distribution.Client
- Shuttle.Distribution.Server
- Shuttle.Distribution.Worker

# Implementation

When you find that a single endpoint, even with ample threads, cannot keep up with the required processing and is falling behind you can opt for message distribution.

<div class='alert alert-info'>When using a broker architecture (such as RabbitMQ) you do not need to use message distribution as workers can all access the same inbox work queue.</div>

Plesae note that the project structure here is used as a sample to facilitate the execution of the solution.  In a real-world scenario the endpoint project would not be separated into a distributor and a worker; rather, there would be a single implementation and you would simply install the service multiple times on, possibly, multiple machines and then configure the workers and distributor as such.  When using shuttle as the distribution mechanism there is always a **1 to *N*** relationship between the distribution endpoint and the worker(s).

However, for a broker-style queueing mechanism such as *RabbitMQ* you do not need to use shuttle to perform any distribution as RabbitMQ would have a consumer for each thread irrespective of where it originates from.

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.Distribution.Client`
- a **Class Library** called `Shuttle.Distribution.Server`
- another **Class Library** called `Shuttle.Distribution.Worker`
- and another **Class Library** called `Shuttle.Distribution.Messages` that will contain all our message classes

## Messages

> Create a new class library called `Shuttle.Distribution.Messages` with a solution called `Shuttle.Distribution`

**Note**: remember to change the *Solution name*.

### RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.Distribution.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

## Client

> Add a new `Console Application` to the solution called `Shuttle.Distribution.Client`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Unity` nuget package.

This will add the [Unity](https://github.com/unitycontainer/unity/) implementation of the [component container](http://shuttle.github.io/shuttle-core/overview-container/) interfaces.

> Add a reference to the `Shuttle.Distribution.Messages` project.

### Program

> Implement the main client code as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.Distribution.Messages;

namespace Shuttle.Distribution.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var container = new UnityComponentContainer(new UnityContainer());

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
				<add specification="StartsWith" value="Shuttle.Distribution.Messages" />
			</messageRoute>
		</messageRoutes>		
	</serviceBus>
	
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>
</configuration>
```

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.Distribution.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

## Server

> Add a new `Class Library` to the solution called `Shuttle.Distribution.Server`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Unity` nuget package.

This will add the [Unity](https://github.com/unitycontainer/unity/) implementation of the [component container](http://shuttle.github.io/shuttle-core/overview-container/) interfaces.

> Install the `Shuttle.Core.ServiceHost` nuget package.

The [default mechanism](http://shuttle.github.io/shuttle-core/overview-service-host/) used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.ServiceHost` in our console executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.Distribution.Messages` project.

### Host

> Rename the default `Class1` file to `Host` and implement the `IServiceHost` interface as follows:

``` c#
using Microsoft.Practices.Unity;
using Shuttle.Core.ServiceHost;
using Shuttle.Core.Unity;
using Shuttle.Esb;

namespace Shuttle.Distribution.Server
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;

        public void Start()
        {
            var container = new UnityComponentContainer(new UnityContainer());

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
		<control 
			workQueueUri="msmq://./shuttle-server-control-inbox-work" 
			errorQueueUri="msmq://./shuttle-samples-error" />

		<inbox
		   distribute="true"
		   workQueueUri="msmq://./shuttle-server-work"
		   errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

This will instruct the endpoint to ***only** distribute messages since the `distribute` attribute is set to `true`.  If it is set to `false` then the endpoint will process an incoming message if a worker thread is not available.

It also configures the control inbox that the endpoint will use to process administrative messages.

## Worker

> Add a new `Class Library` to the solution called `Shuttle.Distribution.Worker`.

> Install the `Shuttle.Esb.Msmq` nuget package.

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Install the `Shuttle.Core.Unity` nuget package.

This will add the [Unity](https://github.com/unitycontainer/unity/) implementation of the [component container](http://shuttle.github.io/shuttle-core/overview-container/) interfaces.

> Install the `Shuttle.Core.ServiceHost` nuget package.

The [default mechanism](http://shuttle.github.io/shuttle-core/overview-service-host/) used to host an endpoint is by using a Windows service.  However, by using the `Shuttle.Core.ServiceHost` in our console executable we are able to run the endpoint as a console application or register it as a Windows service for deployment.

> Add a reference to the `Shuttle.Distribution.Messages` project.

### Host

> Rename the default `Class1` file to `Host` and implement the `IServiceHost` interface as follows:

``` c#
using Microsoft.Practices.Unity;
using Shuttle.Core.ServiceHost;
using Shuttle.Core.Unity;
using Shuttle.Esb;

namespace Shuttle.Distribution.Worker
{
    public class Host : IServiceHost
    {
        private IServiceBus _bus;

        public void Start()
        {
            var container = new UnityComponentContainer(new UnityContainer());

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
		<worker 
			distributorControlWorkQueueUri="msmq://./shuttle-server-control-inbox-work" />
			
		<inbox
			workQueueUri="msmq://./shuttle-worker-work"
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

This configures the endpoint as a worker and specifies the control inbox of the distributor to notify when a thread is available to perform work.

### RegisterMemberHandler

> Add a new class called `RegisterMemberHandler` that implements the `IMessageHandler<RegisterMemberCommand>` interface as follows:

``` c#
using System;
using Shuttle.Esb;
using Shuttle.Distribution.Messages;

namespace Shuttle.Distribution.Worker
{
	public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
	{
		public void ProcessMessage(IHandlerContext<RegisterMemberCommand> context)
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

> Set both the client and server projects as the startup.

### Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

<div class='alert alert-info'>You will observe that the <strong>server</strong> application forwards the message to the worker.</div>

<div class='alert alert-info'>The <strong>worker</strong> application will perform the actual processing.</div>

You have now implemented message distribution.

[transport-message]: {{ site.baseurl }}/transport-message
