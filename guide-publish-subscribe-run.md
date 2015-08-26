---
title: Publish / Subscribe Guide - Run
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-publish-subscribe.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-publish-subscribe-run'</script>
# Run

> Set both the client and server projects as the startup.

![Publish/Subscribe Startup]({{ site.baseurl }}/assets/images/guide-publish-subscribe-startup.png "Publish/Subscribe Startup")

## Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

![Publish/Subscribe Run - Client]({{ site.baseurl }}/assets/images/guide-publish-subscribe-run-client.png "Publish/Subscribe Run - Client")

<div class='alert alert-info'>You will observe that the <strong>server</strong> application has processed the message.</div>

![Publish/Subscribe Run - Server]({{ site.baseurl }}/assets/images/guide-publish-subscribe-run-server.png "Publish/Subscribe Run - Server")

<div class='alert alert-info'>The <strong>subscriber</strong> application will then process the event published by the <strong>server</strong>.</div>

![Publish/Subscribe Run - Subscriber]({{ site.baseurl }}/assets/images/guide-publish-subscribe-run-subscriber.png "Publish/Subscribe Run - Subscriber")

You have now completed a full publish / subscribe call chain.

Previous: [Subscriber][previous]

[previous]: {{ site.baseurl }}/guide-publish-subscribe-subscriber
