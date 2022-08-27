# SQL

```
PM> Install-Package Shuttle.Esb.Sql.Queue
```

Sql RDBMS implementation of the `IQueue` interface for use with Shuttle.Esb which creates a table for each required queue.

## Supported providers

Currently only the `System.Data.SqlClient` and `Microsoft.Data.SqlClient` provider names are supported but this can easily be extended.  Feel free to give it a bash and please send a pull request if you *do* go this route.  You are welcome to create an issue and assistance will be provided where able.

## Configuration

TThe URI structure is `sql://configuration-name/queue-name`.

```c#
services.AddDataAccess(builder =>
{
	builder.AddConnectionString("shuttle", "System.Data.SqlClient", "server=.;database=shuttle;user id=sa;password=Pass!000");
});

services.AddSqlQueue(builder =>
{
	builder.AddOptions("shuttle", new SqlQueueOptions
	{
		ConnectionStringName = "shuttle"
	});
});
```

The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
    "SqlQueue": {
      "ConnectionStringName": "connection-string-name"
    }
  }
}
``` 

## Options

| Option | Default	| Description |
| --- | --- | --- | 
| `ConnectionStringName` | | The name of the connection string to use.  This package makes use of [Shuttle.Core.Data](https://shuttle.github.io/shuttle-core/data/shuttle-core-data.html) for data access. |
