---
title: Process Management Sample
layout: api
---
# Overview

Our sample case is a fictitious online book store where you can order books.  An order allows you 20 seconds to cancel the order before it is accepted.

The process management sample represents something closer to a real-world scenario where one would have a front-end integrating with a web-api.  The web-api issues commands that are processed by the process managers.

In this sample the front-end is a static site implemented using (CanJS)[https://canjs.com/].  The rest api is an asp.net web-api.  There are three physical implementations of the same logical process manager to demonstrate the various options and the read-model is kept updated using CQRS with system messages.  For the event-sourcing side one could just as easily use event processing to update the read model but since the system event messages are being processed it is re-used for the event sourcing implementation also.

When using Visual Studio 2015+ the NuGet packages should be restored automatically.  If you find that they do not or if you are using an older version of Visual Studio please execute the following in a powershell command prompt:

~~~
cd {extraction-folder}\Shuttle.Esb.Samples\Shuttle.ProcessManagement
nuget-restore.ps1
~~~

Once you have opened the `Shuttle.ProcessManagement.sln` solution in Visual Studio set the following projects as startup projects:

- Shuttle.EMailSender.Server
- Shuttle.Invoicing.Server
- Shuttle.Ordering.Server
- Shuttle.Process.Custom.Server
- Shuttle.Process.CustomES.Server
- Shuttle.Process.ESModule.Server
- Shuttle.Process.QueryServer
- Shuttle.ProcessManagement.WebApi
- site

> Set `Shuttle.Core.Host.exe` as the **Start external program** option by navigating to the **bin\debug** folder of each of the server projects.

<div class='alert alert-info'>Before the reference <strong>Shuttle.Core.Host.exe</strong> will be available in the <strong>bin\debug</strong> folder you may need to build the solution.</div>

You will also need to create and configure a Sql Server database for this sample and remember to update the **App.config** `connectionString` settings to point to your database.  Please reference the **Database** section below.

# Database

> Create a new database called **ProcessManagement**.

We need a store for our subscriptions.  In this example we will be using **Sql Server**.  If you use the express version remember to change the `data source` value to `.\sqlexpress` from the standard `.`.

The `Shuttle.Esb.SqlServer` package contains a number of scripts in the following folder:

- `.\Shuttle.PublishSubscribe\packages\Shuttle.Esb.SqlServer.{version}\scripts`

The `{version}` bit will be in a `semver` format.

> Execute script `SubscriptionManagerCreate.sql` in the newly created database.

This will create the required structures that the subscription manager will use.

The `Shuttle.Recall.SqlServer` package contains a the following script folder:

- `.\Shuttle.PublishSubscribe\packages\Shuttle.Recall.SqlServer.{version}\scripts`

The `{version}` bit will be in a `semver` format.

> Execute script `EventStoreCreate.sql` in the `ProcessManagement` database.

This will create the relevant structures used by the `Shuttle.Recall` event-sourcing mechanism.

In addition to this the actual process management application also requires some structures that need to be created:

> Execute script '{extraction-folder}\Shuttle.Esb.Samples\Shuttle.ProcessManagement\.scripts\process-management.sql' in the `ProcessManagement` database.

You should now be able to run the application.

# Process

Once you add books to your order you can place the order using any of the following processes:

- Custom
	* A hand-rolled process manager storing the process state in custom tables.  This is a more traditional approach to data access.
- Custom / EventSource
	* A hand-rolled process manager that stored the process state using the `Shuttle.Recall` event sourcing mechanism directly.
- EventSource / Module
	* This uses the `Shuttle.Esb.Process` module to handle the process state storage for you.
	
Once you register an order by clicking on the button the `RegisterOrderProcessCommand` is immeditely sent to the relevant endpoint for processing.  An `AcceptOrderProcessCommand` is then sent locally (to the same endpoint) but it is deferred for 20 seconds.  This allows you time to cancel the order.

The handler processing the `AcceptOrderProcessCommand` will simply ignore the message should the process/order have been cancelled in the meantime; else it sends a `CreateOrderCommand` to the order processing endpoint.  Once the order has been created the process manager receives an `OrderCreatedEvent` and it then sends a `CreateInvoiceCommand` command.

When the process manager receives the `InvoiceCreatedEvent` an e-mail needs to be sent to the customer so a `SendEMailCommand` is sent to the e-mail server endpoint.  Once the `EMailSentEvent` is recevied by the process manager a `CompleteOrderProcessCommand` is sent locally that then results in an `OrderProcessCompletedEvent` event.

After the process has been completed it is possible to `Archive` the process.  In our sample it simply deletes the order.

As you can see from all the messages the process manager is responsible for the interaction between the various business and infrastructure endpoints to ensure that your use-case runs to completion.  None of the system know about the inner working of the other and can be re-used in different processes.


