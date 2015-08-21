---
title: Idempotence Guide
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-idempotence.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-idempotence-run'</script>
# Run

> Set both the client and server projects as the startup.

![Idempotence Startup]({{ site.baseurl }}/assets/images/guide-idempotence-startup.png "Idempotence Startup")

## Execute

> Execute the application.

> The **client** application will wait for you to input a user name.  For this example enter **my user name** and press enter:

![Idempotence Run - Client]({{ site.baseurl }}/assets/images/guide-idempotence-run-client.png "Idempotence Run - Client")

<div class='alert alert-info'>You will need to scroll through the message but you will observe that the <strong>server</strong> application has processed the three messages.</div>

You have now implemented message idempotence.

Previous: [Server][previous]

[previous]: {{ site.baseurl }}/guide-idempotence-server
