---
title: Request / Response Guide
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-publish-subscribe.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-publish-subscribe-overview'</script>
# Overview

**Events** are interesting things that happen in our system that other systems may be interested in.  There may be **0..*****N*** number of subscribers for an event.  Typically there should be at least one subscriber for an event else it isn't really carrying its own weight.

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.PublishSubscribe.Client`
- a **Class Library** called `Shuttle.PublishSubscribe.Server`
- another **Class Library** called `Shuttle.PublishSubscribe.Messages` that will contain all our message classes
- and, lastly, another **Class Library** called `Shuttle.PublishSubscribe.Subscriber` that will represent a subscriber of our event message

Next: [Messages][next]

[next]: {{ site.baseurl }}/guide-publish-subscribe-messages

