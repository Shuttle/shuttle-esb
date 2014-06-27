---
title: Getting Started with Shuttle-ESB
layout: api 
---
# Getting started with Shuttle ESB

## Download Shuttle ESB

The first step is to get the latest binary release from the GitHub project releases page:

> <a href='http://www.nuget.org/packages?q=shuttle-esb' target='_blank'>Show the Nuget shuttle-esb packages</a>

> <a href='https://github.com/Shuttle/shuttle-esb/releases' target='_blank'>Take me to the releases page</a>)

## Quick Start using MSMQ

Since Shuttle ESB requires queues to operate we will use MSMQ for this.  Before we get started ensure that you have MSMQ installed on your version of Windows.

> <a href='http://msdn.microsoft.com/en-us/library/aa967729%28v=vs.110%29.aspx' target='_blank'>Installing MSMQ</a>)

We will create a very simple scenario where we will send a command for processing to a generically hosted server.

**Note**: Be sure to target the same framework for all the project to ensure that `IHost` types can be picked up.

## The Visual Studio solution

Start up Visual Studio  and create a new blank solution:

* Click **New Project** on the start page or **File | New | Project**
* Under **Other Project Types** select **Visual Studio Solutions** and click the **Blank Solution** template
* Enter **QuickStart.Shuttle** for the **Name** of the solution

Click **OK** to create the solution.

## Messages

Since our message is shared we will create a separate assembly to contain it.  Add a new class library project to the solution and call it **QuickStart.Shuttle.Messages**.

You can then rename the default **Class1** file to **WriteBlueMessageCommand** and add an automatic property for the **Message**:

``` c#
namespace QuickStart.Shuttle.Messages
{
	public class WriteBlueMessageCommand
	{
		public string BlueMessage { get; set; }
	}
}
```

## Client

Now add a console application called **QuickStart.Shuttle.Client** to the solution and reference the following:

* QuickStart.Shuttle.Messages (project reference)
* Nuget package: shuttle-esb-msmq

Add the following code to your console implemetation:

``` c#
using System;
using QuickStart.Shuttle.Messages;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace QuickStart.Shuttle.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var bus = ServiceBus.Create().Start(); 

			ColoredConsole.WriteLine(
				ConsoleColor.DarkGray,
				"(to exit press enter on an empty line):");
			ColoredConsole.WriteLine(
				ConsoleColor.DarkGray, 
				"Enter a message to write in blue on the server and press enter:");
			Console.WriteLine();

			var message = Console.ReadLine();

			while (!string.IsNullOrEmpty(message))
			{
				bus.Send(new WriteBlueMessageCommand
				         	{
				         		BlueMessage = message
				         	});

				message = Console.ReadLine();
			}

			bus.Dispose();
		}
	}
}
```

Shuttle ESB will need to know where to send the message.  Since we are using the default message route provider we need to define the route in the application configuration file.  Add one to your client project:

``` xml
<?xml version="1.0"?>
<configuration>
	<configSections>
		<section name="serviceBus" type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
	</configSections>

	<serviceBus>
		<messageRoutes>
			<messageRoute uri="msmq://./quickstart_server_inbox_work">
				<add specification="StartsWith" value="QuickStart"/>
			</messageRoute>
		</messageRoutes>
	</serviceBus>
</configuration>
```

This will tell Shuttle ESB to send all messages that have their full name start with **QuickStart** to endpoint **msmq://./quickstart_server_inbox_work**

## Server

Add a class library project to the solution and call it **QuickStart.Shuttle.Server** reference the following:

* QuickStart.Shuttle.Messages (project reference)
* Nuget package: shuttle-esb-msmq

Rename the **Class1** file to **ServiceBusHost**.  Since we will be hosting our server using the generic host we need an entry point for the generic host.  It searches for classes implementing **IHost** so let's implement this interface on our **ServiceBusHost** class:

``` c#
using System;
using Shuttle.Core.Host;
using Shuttle.ESB.Core;

namespace QuickStart.Shuttle.Server
{
	public class ServiceBusHost : IHost, IDisposable
	{
		private static IServiceBus bus;

		public void Start()
		{
			bus = ServiceBus.Create().Start(); 
		}

		public void Dispose()
		{
			bus.Dispose();
		}
	}
}
```

Our service bus instance needs to process an input queue so let's configure that in our application configuration file (you will first need to add one):

``` xml
<?xml version="1.0"?>
<configuration>
	<configSections>
		<section name="serviceBus" type="Shuttle.ESB.Core.ServiceBusSection, Shuttle.ESB.Core"/>
	</configSections>

	<serviceBus>
		<inbox
			workQueueUri="msmq://./quickstart_server_inbox_work"
			errorQueueUri="msmq://./quickstart_server_inbox_error"/>
	</serviceBus>
</configuration>
```

In order for our endpoint to start up property we need to configure our project.  **Build** your server project to have the referenced assemblies copied locally.  Open the project properties and go to the **Debug** tab.  For the **Start Action** select the **Start external program** option.  Click the ellipsis (...) and navigate to the **bin\debug** folder of your project and select the **Shuttle.Core.Host.exe** as the startup program (if you do not see the **Shuttle.Core.Host.exe** file you may first need to build your project).

For Shuttle ESB to process the received message a message handler for each type received will need to be created.  Let's create one for our **WriteBlueMessageCommand** message.  Add a new class called **WriteBlueMessageHandler** and implement the **IMessageHandler<WriteBlueMessageCommand>** interface:

``` c#
using System;
using QuickStart.Shuttle.Messages;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace QuickStart.Shuttle.Server
{
	public class WriteBlueMessageHandler : IMessageHandler<WriteBlueMessageCommand>
	{
		public void ProcessMessage(HandlerContext<WriteBlueMessageCommand> context)
		{
			ColoredConsole.WriteLine(ConsoleColor.Blue, context.Message.BlueMessage);
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
```

## Running the solution

In order to run the solution correctly right-click on the solution and select **Set StartUp Projects**.  Select the **Multiple startup projects** option and have both the client and the server projects start.  Move the server project so that it starts first since it needs to create its inbox work queue (should it not yet exist).  Click OK to close the dialog.

You can now run and test the solution.

You have created a very basic ESB solution but the gyst of the ideas are present. From here you can investigate the samples and engage with the community to expand your scope.
