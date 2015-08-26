---
title: Request / Response Guide - Run
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-request-response.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-request-response-run'</script>
# Run

> Set both the client and server projects as the startup.

![Request/Response Startup]({{ site.baseurl }}/assets/images/guide-request-response-startup.png "Request/Response Startup")

## Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

![Request/Response Run - Client]({{ site.baseurl }}/assets/images/guide-request-response-run-client.png "Request/Response Run - Client")

<div class='alert alert-info'>You will observe that the <strong>server</strong> application has processed the message.</div>

![Request/Response Run - Server]({{ site.baseurl }}/assets/images/guide-request-response-run-server.png "Request/Response Run - Server")

<div class='alert alert-info'>The <strong>client</strong> application will then process the response returned by the <strong>server</strong>.</div>

![Request/Response Run - Client Response]({{ site.baseurl }}/assets/images/guide-request-response-run-client-response.png "Request/Response Run - Client Response")

You have now completed a full request / response call chain.

Previous: [Server][previous]

[previous]: {{ site.baseurl }}/guide-request-response-server
