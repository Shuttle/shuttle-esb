### The broad strokes

Start a new **Console Application** project and select a Shuttle.Esb queue implementation from the supported queues:

```
PM> Install-Package Shuttle.Esb.RabbitMQ
```
`

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