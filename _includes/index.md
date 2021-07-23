### The broad strokes

All processing is performed on messages (serialized objects) that are received from a queue and then finding a message handler that can handle the type of the message (`instance.GetType().Name`).  Typically messages are sent to a queue to be processed and this combination of queue and the `ServiceBus` instance that performs the processing is referred to as an *endpoint*:

<div markdown="1" class="image-container">
![Endpoint Image]({{ "/assets/images/endpoint.png" | relative_url }} "Endpoint")
</div>

It is important to note that not every `ServiceBus` instance will process messages from an inbox queue.  This happens when the instance is a producer of messages only.  An example may be a `web-api` that receives integration requests that are then sent to a relevant endpoint queue as a `command` message.

Similarly, not every queue is going to be consumed by a `ServiceBus` instance.  An example of this is the error queue where poison messages are routed to.  These queues have to be managed out-of-band to determine the cause of the failure before moving the messages back to the inbox queue for another round of processing.

<div class="alert alert-success" role="alert">
    Packages currently target <code class="language-plaintext">netstandard2.0</code> and <code class="language-plaintext">netstandard2.1</code> which means that the libraries can be used with the following runtimes:
    <ul>
        <li>.NET Core 2.1+</li>
        <li>.NET Framework 4.6.1+</li>
        <li>.NET 5.0</li>
    </ul>
</div>

### How to get going

Start a new **Console Application** project and select a Shuttle.Esb queue implementation from the supported queues:

```
PM> Install-Package Shuttle.Esb.RabbitMQ
```

Now we'll need select one of the [supported containers](http://shuttle.github.io/shuttle-core/shuttle-core-container#supported):

```
PM> Install-Package Shuttle.Core.Autofac
```

We'll also need to host our endpoint using the [service host](http://shuttle.github.io/shuttle-core/shuttle-core-servicehost):

```
PM> Install-Package Shuttle.Core.ServiceHost
```

Next we'll implement our endpoint in order to start listening on our queue:

``` c#
internal class Program
{
	private static void Main()
	{
		ServiceHost.Run<Host>();
	}
}

public class Host : IServiceHost
{
	private IServiceBus _bus;

	public void Start()
	{
		var containerBuilder = new ContainerBuilder();
		var registry = new AutofacComponentRegistry(containerBuilder);

		ServiceBus.Register(registry);

		var resolver = new AutofacComponentResolver(containerBuilder.Build());

		_bus = ServiceBus.Create(resolver).Start();
	}

	public void Stop()
	{
		_bus.Dispose();
	}
}
```

A bit of configuration is going to be needed to help things along:

``` xml
<configuration>
	<configSections>
		<section name="serviceBus" type="Shuttle.Esb.ServiceBusSection, Shuttle.Esb"/>
	</configSections>

	<serviceBus>
		<inbox 
			workQueueUri="msmq://./shuttle-server-work" 
			deferredQueueUri="msmq://./shuttle-server-deferred" 
			errorQueueUri="msmq://./shuttle-error" />
	</serviceBus>
</configuration>
```

### Send a command message for processing

``` c#
using (var bus = ServiceBus.Create(resolver).Start())
{
	bus.Send(new RegisterMemberCommand
	{
		UserName = "Mr Resistor",
		EMailAddress = "ohm@resistor.domain"
	});
}
```

### Publish an event message when something interesting happens

``` c#
using (var bus = ServiceBus.Create(resolver).Start())
{
	bus.Publish(new MemberRegisteredEvent
	{
		UserName = "Mr Resistor"
	});
}
```

### Subscribe to those interesting events

``` c#
resolver.Resolve<ISubscriptionManager>().Subscribe<MemberRegisteredEvent>();
```

### Handle any messages

``` c#
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
```

``` c#
public class MemberRegisteredHandler : IMessageHandler<MemberRegisteredEvent>
{
	public void ProcessMessage(IHandlerContext<MemberRegisteredEvent> context)
	{
		Console.WriteLine();
		Console.WriteLine("[EVENT RECEIVED] : user name = '{0}'", context.Message.UserName);
		Console.WriteLine();
	}
}
```