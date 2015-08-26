---
title: Dependency Injection Guide - Run
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-dependencyinjection.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-dependencyinjection-run'</script>
# Run

> Set both the client and server projects as the startup.

![Dependency Injection Startup]({{ site.baseurl }}/assets/images/guide-dependencyinjection-startup.png "Dependency Injection Startup")

## Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

![DependencyInjection Run - Client]({{ site.baseurl }}/assets/images/guide-dependencyinjection-run-client.png "Dependency Injection Run - Client")

<div class='alert alert-info'>You will notice that the <strong>server</strong> application has processed the message and simulate sending an e-mail though the <strong>IEMailService</strong> implementation.</div>

![Dependency Injection Run - Server]({{ site.baseurl }}/assets/images/guide-dependencyinjection-run-server.png "Dependency Injection Run - Server")

You have now implemented dependency injection for message handlers.

Previous: [Server][previous]

[previous]: {{ site.baseurl }}/guide-dependencyinjection-server
