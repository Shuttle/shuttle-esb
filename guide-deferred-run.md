---
title: Deferred Messages Guide - Run
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-deferred.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-deferred-run'</script>
# Run

> Set both the client and server projects as the startup.

![Deferred Startup]({{ site.baseurl }}/assets/images/guide-deferred-startup.png "Deferred Startup")

## Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

![Deferred Run - Client]({{ site.baseurl }}/assets/images/guide-deferred-run-client.png "Deferred Run - Client")

<div class='alert alert-info'>After 5 seconds you will observe that the <strong>server</strong> application has processed the message.</div>

![Deferred Run - Server]({{ site.baseurl }}/assets/images/guide-deferred-run-server.png "Deferred Run - Server")

You have now implemented deferred message sending.

You will also notice that `Log4Net` has created the log file under **~\Shuttle.Deferred\Shuttle.Deferred.Server\bin\Debug\logs**.

Previous: [Server][previous]

[previous]: {{ site.baseurl }}/guide-deferred-server
