---
title: Idempotence Guide - Overview
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-idempotence.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-idempotence-overview'</script>
# Overview

When operations, or in our case messages, can be applied multiple with the same result they are said to be **idempotent**.  Idempotence is something you should strive to implement directly on your endpoint by keeping track of some unique property of each message and whether the operation has been completed for that unique property.

An `IIdempotenceService` implementation can assist with this from a technical point-of-view by allowing a particular message id to be handled only once.  This works fine for our ***at-least-once*** delivery mechanism where, in some edge case, we may receive the same message again.  However, it will not aid us when two messages are going to be sent, each with its own message id, but they have contain the same data.

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.Idempotence.Client`
- a **Class Library** called `Shuttle.Idempotence.Server`
- and another **Class Library** called `Shuttle.Idempotence.Messages` that will contain all our message classes

Next: [Messages][next]

[next]: {{ site.baseurl }}/guide-idempotence-messages
