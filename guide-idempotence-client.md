---
title: Idempotence Messages Guide
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-idempotence.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-idempotence-client'</script>
# Client

> Add a new `Console Application` to the solution called `Shuttle.Idempotence.Client`.

![Idempotence Client]({{ site.baseurl }}/assets/images/guide-idempotence-client.png "Idempotence Client")

> Install the `shuttle-esb-msmq` nuget package.

![Idempotence Client - Nuget]({{ site.baseurl }}/assets/images/guide-idempotence-client-nuget.png "Idempotence Client - Nuget")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Add a reference to the `Shuttle.Idempotence.Messages` project.

## Program

> Implement the main client code as follows:

``` c#
using System;
using Shuttle.ESB.Core;
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
```

Keep in mind that the when you `Send` a message a `TransportMessage` envelope is created with a unique message id (`Guid`).  In the above code we first manually create a `TransportMessage` so that we can send technically identical messages.

The next two `Send` operations do not use the `TransportMessage` but rather send individual messages.  These will each have a `TransportMessage` envelope and, therefore, each have its own unique message id.

## App.config

> Create the shuttle configuration as follows:

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
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
```

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.Idempotence.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

Previous: [Messages][previous] | Next: [Server][next]

[previous]: {{ site.baseurl }}/guide-idempotence-messages
[next]: {{ site.baseurl }}/guide-idempotence-server
[transport-message]: {{ site.baseurl }}/transport-message
