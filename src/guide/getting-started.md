# Getting Started

Start a new **Console Application** project and select a Shuttle.Esb queue implementation from the supported queues:

```
PM> Install-Package Shuttle.Esb.AzureStorageQueues
```

We'll also make use of the [.NET generic host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host):

```
PM> Install-Package Microsoft.Extensions.Hosting
```

Next we'll implement our endpoint in order to start listening on our queue:

``` c#
internal class Program
{
    private static void Main()
    {
        Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddServiceBus(builder =>
                {
                    builder.Options.Inbox.WorkQueueUri = "azuresq://azure/work";
                });

                services.AddAzureStorageQueues(builder =>
                {
                    builder.AddOptions("azure", new AzureStorageQueueOptions
                    {
                        ConnectionString = "UseDevelopmentStorage=true;"
                    });
                });
            })
            .Build()
            .Run();
    }
}
```

Even though the options may be set directly as above, typically one would make use of a configuration provider:

```c#
internal class Program
{
    private static void Main()
    {
        Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                var configuration = 
                    new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

                services.AddSingleton<IConfiguration>(configuration);

                services.AddServiceBus(builder =>
                {
                    configuration
                        .GetSection(ServiceBusOptions.SectionName)
                        .Bind(builder.Options);
                });

                services.AddAzureStorageQueues(builder =>
                {
                    builder.AddOptions("azure", new AzureStorageQueueOptions
                    {
                        ConnectionString = configuration
                            .GetConnectionString("azure")
                    });
                });
            })
            .Build()
            .Run();
    }
}
```

The `appsettings.json` file would be as follows:

```json
{
  "ConnectionStrings": {
    "azure": "UseDevelopmentStorage=true;"
  },
  "Shuttle": {
    "ServiceBus": {
      "Inbox": {
        "WorkQueueUri": "azuresq://azure/work",
      }
    }
  }
}
```

### Send a command message for processing

``` c#
bus.Send(new RegisterMember
{
    UserName = "user-name",
    EMailAddress = "user@domain.com"
});
```

### Publish an event message when something interesting happens

Before publishing an event one would need to register an `ISubscrtiptionService` implementation such as [Shuttle.Esb.Sql.Subscription](/implementations/subscription/sql.md).

``` c#
bus.Publish(new MemberRegistered
{
    UserName = "user-name"
});
```

### Subscribe to those interesting events

``` c#
services.AddServiceBus(builder =>
{
    builder.AddSubscription<MemberRegistered>();
});
```

### Handle any messages

``` c#
public class RegisterMemberHandler : IMessageHandler<RegisterMember>
{
    public RegisterMemberHandler(IDependency dependency)
    {
    }

	public void ProcessMessage(IHandlerContext<RegisterMember> context)
	{
        // perform member registration

		context.Publish(new MemberRegistered
		{
			UserName = context.Message.UserName
		});
	}
}
```

``` c#
public class MemberRegisteredHandler : IMessageHandler<MemberRegistered>
{
	public void ProcessMessage(IHandlerContext<MemberRegistered> context)
	{
        // processing
	}
}
```