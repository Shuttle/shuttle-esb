---
title: Request / Response Guide - Client
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-request-response.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-request-response-client'</script>
# Client

> Add a new `Console Application` to the solution called `Shuttle.RequestResponse.Client`.

![Request/Response Client]({{ site.baseurl }}/assets/images/guide-request-response-client.png "Request/Response Client")

> Install the `Shuttle.Esb.Msmq` nuget package.

![Request/Response Client - Nuget]({{ site.baseurl }}/assets/images/guide-request-response-client-nuget.png "Request/Response Client - Nuget")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Add a reference to the `Shuttle.RequestResponse.Messages` project.

## Program

> Implement the main client code as follows:

~~~ c#
using System;
using Shuttle.Esb;
using Shuttle.RequestResponse.Messages;

namespace Shuttle.RequestResponse.Client
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

## App.config

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
				<add specification="StartsWith" value="Shuttle.RequestResponse.Messages" />
			</messageRoute>
		</messageRoutes>		

		<inbox
		   workQueueUri="msmq://./shuttle-client-work"
		   errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
	
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>
</configuration>
~~~

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.RequestResponse.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

## MemberRegisteredHandler

> Create a new class called `MemberRegisteredHandler` that implements the `IMessageHandler<MemberRegisteredEvent>` interface as follows:

~~~ c#
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

		public bool IsReusable {
			get { return true; } 
		}
	}
}
~~~

Previous: [Messages][previous] | Next: [Server][next]

[previous]: {{ site.baseurl }}/guide-request-response-messages
[next]: {{ site.baseurl }}/guide-request-response-server