# SQL

```
PM> Install-Package Shuttle.Esb.Sql.Idempotence
```

Contains a sql-based `IIdempotenceService` implementation.

## Supported providers

Currently only the `System.Data.SqlClient` and `Microsoft.Data.SqlClient` provider names are supported but this can easily be extended.  Feel free to give it a bash and please send a pull request if you *do* go this route.  You are welcome to create an issue and assistance will be provided where able.

## Configuration

```c#
services.AddDataAccess(builder =>
{
    builder.AddConnectionString("shuttle", "System.Data.SqlClient",
        "server=.;database=shuttle;user id=sa;password=Pass!000");
});

services.AddSqlIdempotence(builder =>
{
    builder.Options.ConnectionStringName = "shuttle";
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