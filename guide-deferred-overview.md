---
title: Deferred Messages Guide - Overview
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-deferred.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-deferred-overview'</script>
# Overview

Deferred messages refer to messages that are not immediately processed when available but are rather set to only process at a given future date.

<div class='alert alert-info'>It is important to note that each endpoint <strong>must</strong> have its own deferred queue.</div>

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.Deferred.Client`
- a **Class Library** called `Shuttle.Deferred.Server`
- and another **Class Library** called `Shuttle.Deferred.Messages` that will contain all our message classes

Next: [Messages][next]

[next]: {{ site.baseurl }}/guide-deferred-messages
