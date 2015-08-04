---
title: Request / Response Guide
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-request-response.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-request-response-overview'</script>
# Overview

In order to get any processing done in shuttle-esb a message will need to be generated and sent to an endpoint for processing.  In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.Client`
- a **Class Library** called `Shuttle.Server`
- and another **Class Library** called `Shuttle.Messages` that will contain all our message classes

We will create a `RegisterMemberCommand` message that the client application will send to the server for processing.

Next: [Messages][next]

[next]: {{ site.baseurl }}/guide-request-response-messages
