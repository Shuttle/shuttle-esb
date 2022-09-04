# Message Route Options

```c#
var configuration = 
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();

services.AddServiceBus(builder => 
{
    builder.Options.MessageRoutes = new List<MessageRouteOptions>
    {
        new()
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
        },
        new()
        {
            Uri = "queue://configuration/inbox-work-b",
            Specifications = new List<MessageRouteOptions.SpecificationOptions>
            {
                new()
                {
                    Name = "TypeList",
                    Value = "DoSomethingCommand"
                }
            }
        }
        },
        new()
        {
            Uri = "queue://configuration/inbox-work-c",
            Specifications = new List<MessageRouteOptions.SpecificationOptions>
            {
                new()
                {
                    Name = "Regex",
                    Value = ".+[Cc]ommand.+"
                }
            }
        }
        },
        new()
        {
            Uri = "queue://configuration/inbox-work-d",
            Specifications = new List<MessageRouteOptions.SpecificationOptions>
            {
                new()
                {
                    Name = "Assembly",
                    Value = "TheAssemblyName"
                }
            }
        }
    };
    
    // or bind from configuration
    configuration
        .GetSection(ServiceBusOptions.SectionName)
        .Bind(builder.Options);
})
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "MessageRoutes": [
        {
          "Uri": "queue://configuration/inbox-work-a",
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
          "Uri": "queue://configuration/inbox-work-b",
          "Specifications": [
            {
              "Name": "TypeList",
              "Value": "DoSomethingCommand"
            }
          ]
        }
        },
        {
          "Uri": "queue://configuration/inbox-work-c",
          "Specifications": [
            {
              "Name": "Regex",
              "Value": ".+[Cc]ommand.+"
            }
          ]
        }
        },
        {
          "Uri": "queue://configuration/inbox-work-d",
          "Specifications": [
            {
              "Name": "Assembly",
              "Value": "TheAssemblyName"
            }
          ]
        }
      ]
    }
  }
}
```
