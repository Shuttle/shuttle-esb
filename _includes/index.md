### Let's get going!

Start a new **Class Library** project and select a Shuttle.Esb queue implementation from the [supported queues]({{ site.baseurl }}/packages/#queues):

<div class="nuget-badge">
	<p>
		<code>Install-Package Shuttle.Esb.Msmq</code>
	</p>
</div>

Now we'll need select one of the [supported containers]({{ site.baseurl }}/supported-containers):

<div class="nuget-badge">
	<p>
		<code>Install-Package Shuttle.Core.Autofac</code>
	</p>
</div>

We'll also need to host our endpoint within the [generic host](http://shuttle.github.io/shuttle-core/overview-service-host/):

<div class="nuget-badge">
	<p>
		<code>Install-Package Shuttle.Core.Host</code>
	</p>
</div>

Next we'll implement our endpoint in order to start listening on our queue:

``` c#
public class Host : IHost, IDisposable
{
	private IServiceBus _bus;

	public void Start()
	{
		var containerBuilder = new ContainerBuilder();
		var registry = new AutofacComponentRegistry(containerBuilder);

		ServiceBus.Register(registry);

		_bus = ServiceBus.Create(new AutofacComponentResolver(containerBuilder.Build())).Start();
	}

	public void Dispose()
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

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of the server project for the **Shuttle.Deferred.Server** project.

<div class='alert alert-warning'>It may be necessary to build the solution before the <strong>Shuttle.Core.Host.exe</strong> executable will be available in the <strong>bin\debug</strong> folder.</div>

### Send a command message for processing

``` c#
using (var bus = ServiceBus.Create().Start())
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
using (var bus = ServiceBus.Create(c => c.SubscriptionManager(SubscriptionManager.Default())).Start())
{
	bus.Publish(new MemberRegisteredEvent
	{
		UserName = "Mr Resistor"
	});
}
```

### Subscribe to those interesting events

``` c#
SubscriptionManager.Default().Subscribe<MemberRegisteredEvent>();
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