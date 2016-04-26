### Send a command message for processing

~~~ c#
using (var bus = ServiceBus.Create().Start())
{
	bus.Send(new RegisterMemberCommand
	{
		UserName = "Mr Resistor",
		EMailAddress = "ohm@resistor.domain"
	});
}
~~~

### Publish an event message when something interesting happens

~~~ c#
using (var bus = ServiceBus.Create(c => c.SubscriptionManager(SubscriptionManager.Default())).Start())
{
	bus.Publish(new MemberRegisteredEvent
	{
		UserName = "Mr Resistor"
	});
}
~~~

### Subscribe to those interesting events

~~~ c#
SubscriptionManager.Default().Subscribe<MemberRegisteredEvent>();
~~~

### Handle any messages

~~~ c#
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

	public bool IsReusable
	{
		get { return true; }
	}
}
~~~

~~~ c#
public class MemberRegisteredHandler : IMessageHandler<MemberRegisteredEvent>
{
	public void ProcessMessage(IHandlerContext<MemberRegisteredEvent> context)
	{
		Console.WriteLine();
		Console.WriteLine("[EVENT RECEIVED] : user name = '{0}'", context.Message.UserName);
		Console.WriteLine();
	}

	public bool IsReusable
	{
		get { return true; }
	}
}
~~~