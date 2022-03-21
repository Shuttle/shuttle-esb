# SQL

```
PM> Install-Package Shuttle.Esb.Sql.Subscription
```

Contains a sql-based `ISubscriptionManager` implementation.  The subscription manager caches all subscriptions forever so should a new subscriber be added be sure to restart the publisher endpoint service.

## Registration

The required components may be registered by calling `ComponentRegistryExtensions.RegisterSubscription(IComponentRegistry)`.

## Supported providers

- `Microsoft.Data.SqlClient`
- `System.Data.SqlClient`
- `Npgsql` / thanks to [hopla](https://github.com/hopla)

If you'd like support for another SQL-based provider please feel free to give it a bash and send a pull request if you *do* go this route.  You are welcome to create an issue and assistance will be provided where possible.

## Configuration

The configuration section is optional as the defaults will be applied when no section is provided.

```xml
<configuration>
	<configSections>
		<section name="subscription" type="Shuttle.Esb.Sql.Subscription.SubscriptionSection, Shuttle.Esb.Sql.Subscription"/>
	</configSections>
  
	<subscription
		connectionStringName="Subscription"
		subscribe="Normal|Ensure|Ignore"
	/>
  .
  .
  .
<configuration>
```

| Attribute | Default	| Description | 
| --- | --- | --- |
| `connectionStringName`	 | Subscription | The name of the `connectionString` to use to connect to the subscription store. |
| `subscribe`	| Normal | Indicates how calls to the `Subscribe` method are dealt with: <br/>- `Normal` is the ***default*** and will subscribe to the given message type(s) if they have not been subscribed to yet.<br/>- `Ensure` will check to see that the subscription exists and if not will throw an `ApplicationException`.<br/>- `Ignore` will simply ignore the subscription request. |

Whenever the endpoint is configured as a worker no new subscriptions will be registered against the endpoint since any published events should be subscribed to only by the distributor endpoint.  When using a broker such as RabbitMQ all the endpoints feed off the same work queue uri and any of the endpoints could create the subscription.

When moving to a non-development environment it is recommended that you make use of the `Ensure` option for the `subscribe` attribute since any change to the work queue uri will result in possible duplicate subscriptions.  
