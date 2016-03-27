---
title: Publish / Subscribe Guide - Client
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-publish-subscribe.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-publish-subscribe-client'</script>
# Client

> Add a new `Console Application` to the solution called `Shuttle.PublishSubscribe.Client`.

![Publish/Subscribe Client]({{ site.baseurl }}/assets/images/guide-publish-subscribe-client.png "Publish/Subscribe Client")

> Install the `Shuttle.Esb.Msmq` nuget package.

![Publish/Subscribe Client - Nuget]({{ site.baseurl }}/assets/images/guide-publish-subscribe-client-nuget.png "Publish/Subscribe Client - Nuget")

This will provide access to the Msmq `IQueue` implementation and also include the required dependencies.

> Add a reference to the `Shuttle.PublishSubscribe.Messages` project.

## Program

> Implement the main client code as follows:

~~~ c#
using System;
using Shuttle.Esb;
using Shuttle.PublishSubscribe.Messages;

namespace Shuttle.PublishSubscribe.Client
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
				<add specification="StartsWith" value="Shuttle.PublishSubscribe.Messages" />
			</messageRoute>
		</messageRoutes>		
	</serviceBus>
	
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>
</configuration>
~~~

This tells shuttle that all messages that are sent and have a type name starting with `Shuttle.PublishSubscribe.Messages` should be sent to endpoint `msmq://./shuttle-server-work`.

Previous: [Messages][previous] | Next: [Server][next]

[previous]: {{ site.baseurl }}/guide-publish-subscribe-messages
[next]: {{ site.baseurl }}/guide-publish-subscribe-server