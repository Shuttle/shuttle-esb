# Idempotence Options

The `IdempotenceOptions` configured as `ServiceBusOptions.Idempotence` represent common options related to idempotence.

```c#
services.AddDataAccess(builder =>
{
    builder.AddConnectionString("shuttle", "System.Data.SqlClient",
        "server=.;database=shuttle;user id=sa;password=Pass!000");
});

services.AddServiceBus(builder => {
    builder.Options.Idempotence.ConnectionStringName = "shuttle";
});
```

And the JSON configuration structure:

```json
{
  "Shuttle": {
    "ServiceBus": {
      "Idempotence": {
        "ConnectionStringName": "connection-string-name"
      }
    }
  }
}
```