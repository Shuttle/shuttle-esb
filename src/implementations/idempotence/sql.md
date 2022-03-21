# SQL

```
PM> Install-Package Shuttle.Esb.Sql.Idempotence
```

Contains a sql-based `IIdempotenceService` implementation.

## Registration

The required components may be registered by calling `ComponentRegistryExtensions.RegisterIdempotence(IComponentRegistry)`.

## Supported providers

Currently only the `System.Data.SqlClient` and `Microsoft.Data.SqlClient` provider names are supported but this can easily be extended.  Feel free to give it a bash and please send a pull request if you *do* go this route.  You are welcome to create an issue and assistance will be provided where able.

## Configuration

``` xml
<configuration>
  <configSections>
    <section name="idempotence" type="Shuttle.Esb.Sql.Idempotence.IdempotenceSection, Shuttle.Esb.Sql.Idempotence" />
  </configSections>

  <idempotence
    connectionStringName="connection-string-name" />
  .
  .
  .
<configuration>
```