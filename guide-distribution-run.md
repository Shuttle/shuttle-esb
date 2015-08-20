---
title: Message Distribution Guide
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-distribution.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-distribution-run'</script>
# Run

> Set both the client and server projects as the startup.

![Distribution Startup]({{ site.baseurl }}/assets/images/guide-distribution-startup.png "Distribution Startup")

## Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

![Distribution Run - Client]({{ site.baseurl }}/assets/images/guide-distribution-run-client.png "Distribution Run - Client")

<div class='alert alert-info'>You will observe that the <strong>server</strong> application forwards the message to the worker.</div>

![Distribution Run - Server]({{ site.baseurl }}/assets/images/guide-distribution-run-server.png "Distribution Run - Server")

<div class='alert alert-info'>The <strong>worker</strong> application will perform the actual processing.</div>

![Distribution Run - Worker]({{ site.baseurl }}/assets/images/guide-distribution-run-worker.png "Distribution Run - Worker")

You have now implemented message distribution.

Previous: [Worker][previous]

[previous]: {{ site.baseurl }}/guide-distribution-worker
