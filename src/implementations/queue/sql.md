# SQL

```
PM> Install-Package Shuttle.Esb.Sql.Queue
```

Sql RDBMS implementation of the `IQueue` interface for use with Shuttle.Esb which creates a table for each required queue.

## Registration

The required components may be registered by calling `ComponentRegistryExtensions.RegisterSqlQueue(IComponentRegistry)`.

## Supported providers

Currently only the `System.Data.SqlClient` and `Microsoft.Data.SqlClient` provider names are supported but this can easily be extended.  Feel free to give it a bash and please send a pull request if you *do* go this route.  You are welcome to create an issue and assistance will be provided where able.

## Configuration

The queue configuration is part of the specified uri, e.g.:

``` xml
    <inbox
      workQueueUri="sql://connectionstring-name/table-queue"
	  .
	  .
	  .
    />
```
