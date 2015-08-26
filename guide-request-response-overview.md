---
title: Request / Response Guide - Overview
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-request-response.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-request-response-overview'</script>
# Overview

In order to get any processing done in shuttle-esb a message will need to be generated and sent to an endpoint for processing.  The idea behind a **command** message is that there is exactly **one** endpoint handling the message.  Since it is an instruction the message absolutely ***has*** to be handled and we also need to have only a single endpoint process the message to ensure a consistent result.

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.RequestResponse.Client`
- a **Class Library** called `Shuttle.RequestResponse.Server`
- and another **Class Library** called `Shuttle.RequestResponse.Messages` that will contain all our message classes

Next: [Messages][next]

[next]: {{ site.baseurl }}/guide-request-response-messages
