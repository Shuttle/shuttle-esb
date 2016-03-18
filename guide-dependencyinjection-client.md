---
title: Dependency Injection Guide - Client
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-dependencyinjection.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-dependencyinjection-client'</script>
# Client

> Add a new `Console Application` to the solution called `Shuttle.DependencyInjection.Client`.

![Dependency Injection Client]({{ site.baseurl }}/assets/images/guide-dependencyinjection-client.png "Dependency Injection Client")

> Install the `shuttle-esb-msmq` nuget package.

![Dependency Injection Client - Nuget]({{ site.baseurl }}/assets/images/guide-dependencyinjection-client-nuget.png "Dependency Injection Client - Nuget")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Add a reference to the `Shuttle.DependencyInjection.Messages` project.

## Program

> Implement the main client code as follows:

~~~ c#
using System;
using Shuttle.ESB.Core;
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

## App.config

> Create the shuttle configuration as follows:

~~~ xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
		<section name='serviceBus' type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
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

Previous: [Messages][previous] | Next: [e-mail][next]

[previous]: {{ site.baseurl }}/guide-dependencyinjection-messages
[next]: {{ site.baseurl }}/guide-dependencyinjection-email
[transport-message]: {{ site.baseurl }}/transport-message
