# Process Management

::: info
Remember that you can download the samples from the <a href="https://github.com/Shuttle/Shuttle.Esb.Samples" target="_blank">GitHub repository</a>.
:::

Our sample case is a fictitious online book store where you can order books.  An order allows you 20 seconds to cancel the order before it is accepted.

The process management sample represents something closer to a real-world scenario where one would have a front-end integrating with a web-api.  The web-api issues commands, via messaging/queues, that are processed by the process managers.

In this sample the front-end is a static [Vue.js](https://vuejs.org/).  The REST API is a `dotnet` web-api.  There are three physical implementations of the same logical process manager to demonstrate the various options, and the read-model is kept updated using CQRS with system messages.  For the event-sourcing side one could just as easily use event processing to update the read model but since the system event messages are being processed it is re-used for the event sourcing implementation also.

Once you have opened the `Shuttle.ProcessManagement.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.EMailSender.Server
- Shuttle.Invoicing.Server
- Shuttle.Ordering.Server
- Shuttle.Process.Custom.Server
- Shuttle.Process.CustomES.Server
- Shuttle.Process.ESModule.Server
- Shuttle.Process.QueryServer
- Shuttle.ProcessManagement.WebApi

Since the subscribers register the relevant subscriptions the publishers need to know about these.  After starting up all the projects please stop everything and re-start in order for the publishers to pick up the subscriptions.

## Front-End / Shuttle.ProcessManagement.Vue

In order to run the `Vue` front-end the simplest would be to open up the folder in `vscode` then running `npm install` in a terminal window follewed by `npm run dev`.

You will also need to create and configure a Sql Server database for this sample and remember to all the `ConnectionStrings` settings in the `appsettings.json` files to point to your database as the default `ConnectionStrings` is typically as follows:

```json
{
  "ConnectionStrings": {
    "azure": "UseDevelopmentStorage=true;",
    "ProcessManagement": "server=.;database=ProcessManagement;user id=sa;password=Pass!000"
  }
}
```

## Database

We need a store for our subscriptions.  In this example we will be using **Sql Server**.  If you are using docker you can quickly get up-and-running with the following:

```
docker run -d --name sql -h sql -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Pass!000" -e "MSSQL_PID=Express" -p 1433:1433 -v C:\SQLServer.Data\:/var/opt/mssql/data mcr.microsoft.com/mssql/server:2019-latest
```

When you reference the `Shuttle.Esb.Sql.Subscription` package a `scripts` folder is included in the relevant package folder.  Click on the NuGet referenced assembly in the `Dependencies` and navigate to the package folder (in the `Path` property) to find the `scripts` folder.

The `{version}` bit will be in a `semver` format.

> Create a new database called **ProcessManagement** and execute the script `{provider}\SubscriptionManagerCreate.sql` in the newly created database.

This will create the required structures that the subscription manager will use to store the subcriptions.  However, this step is optional as the `SqlSubscriptionService` implementation will create any required structures.  In many cases one would need to create the structures manually, such as in production environments, so the script execution process is included for completeness.

Similarly, the `Shuttle.Recall.Sql.Storage` and `Shuttle.Recall.Sql.EventProcessing` packages contain the following scripts that you should execute against the `ProcessManagement` database (not optional):

- `%UserProfile%\.nuget\packages\shuttle.recall.sql.storage\{version}\scripts\{provider}\EventStoreCreate.sql`
- `%UserProfile%\.nuget\packages\shuttle.recall.sql.eventprocessing\{version}\scripts\{provider}\ProjectionCreate.sql`

This will create the relevant structures used by the `Shuttle.Recall` event-sourcing and projection mechanisms.

In addition to this the actual process management application also requires some structures that need to be created:

> Execute script '{extraction-folder}\Shuttle.Esb.Samples\Shuttle.ProcessManagement\.scripts\process-management.sql' in the `ProcessManagement` database.

You should now be able to run the application.

## Process

Once you add books to your order you can place the order using any of the following processes:

- Custom
	* A hand-rolled process manager storing the process state in custom tables.  This is a more traditional approach to data access.
- Custom / EventSource
	* A hand-rolled process manager that stored the process state using the `Shuttle.Recall` event sourcing mechanism directly.
- EventSource / Module
	* This uses the `Shuttle.Esb.Process` module to handle the process state storage for you.
	
Once you register an order by clicking on the button the `RegisterOrderProcess` command is immediately sent to the relevant endpoint for processing.  An `AcceptOrderProcess` command is then sent locally (to the same endpoint) but it is deferred for 20 seconds.  This allows you time to cancel the order.

The handler processing the `AcceptOrderProcess` will simply ignore the message should the process/order have been cancelled in the meantime; else it sends a `CreateOrder` command to the order processing endpoint.  Once the order has been created the process manager receives an `OrderCreated` event and it then sends a `CreateInvoice` command.

When the process manager receives the `InvoiceCreated` event an e-mail needs to be sent to the customer so a `SendEMail` command is sent to the e-mail server endpoint.  Once the `EMailSent` event is recevied by the process manager a `CompleteOrderProcess` command is sent locally that then results in an `OrderProcessCompleted` event.

After the process has been completed it is possible to `Archive` the process.  In our sample it simply deletes the order.

As you can see from all the messages, the process manager is responsible for the interaction between the various business and infrastructure endpoints to ensure that your use-case runs to completion.  None of the systems know about the inner working of another and may be re-used in different processes.


