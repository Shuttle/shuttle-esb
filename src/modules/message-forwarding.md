# Message Forwarding

```
PM> Install-Package Shuttle.Esb.Module.MessageForwarding
```

The MessageForwarding module for Shuttle.Esb will forward any handled messages onto the specified queue(s).

## Configuration

```c#
services.AddMessageForwardingModule(builder => 
{
	builder.Options.ForwardingRoutes.Add(new MessageRouteOptions
	{
		Uri = "queue://configuration/inbox-work-a",
        Specifications = new List<MessageRouteOptions.SpecificationOptions>
        {
            new()
            {
                Name = "StartsWith",
                Value = "Shuttle.Messages.v1"
            },
            new()
            {
                Name = "StartsWith",
                Value = "Shuttle.Messages.v2"
            }
        }
	})
});
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "Modules": {
      "MessageForwarding": {
        "ForwardingRoutes": [
          {
            "Uri": "queue://./inbox-work-a",
            "Specifications": [
              {
                "Name": "StartsWith",
                "Value": "Shuttle.Messages.v1"
              },
              {
                "Name": "StartsWith",
                "Value": "Shuttle.Messages.v2"
              }
            ]
          },
          {
            "Uri": "queue://./inbox-work-b",
            "Specifications": [
              {
                "Name": "TypeList",
                "Value": "DoSomethingCommand"
              }
            ]
          }
        ]
      }
    }
  }
}
```

The `ForwardingRoutes` makes use of the `MessageRouteOptions` as defined in the `Shuttle.Esb` package and the documentation for the `MessageRouteOptions` is applicable to the `ForwardingRoutes` as well.