---
title: Message Distribution Guide
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-distribution.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-distribution-overview'</script>
# Overview

When you find that a single endpoint, even with ample threads, cannot keep up with the required processing and is falling behind you can opt for message distribution.

<div class='alert alert-info'>When using a broker architecture (such as RabbitMQ) you do not need to use message distribution as workers can all access the same inbox work queue.</div>

Plesae note that the project structure here is used as a sample to facilitate the execution of the solution.  In a real-world scenario the endpoint project would not be separated into a distributor and a worker; rather, there would be a single implementation and you would simply install the service multiple times on, possibly, multiple machines and then configure the workers and distributor as such.  When using shuttle as the distribution mechanism there is always a **1 to *N*** relationship between the distribution endpoint and the worker(s).

However, for a broker-style queueing mechanism such as *RabbitMQ* you do not need to use shuttle to perform any distribution as RabbitMQ would have a consumer for each thread irrespective of where it originates from.

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.Distribution.Client`
- a **Class Library** called `Shuttle.Distribution.Server`
- another **Class Library** called `Shuttle.Distribution.Worker`
- and another **Class Library** called `Shuttle.Distribution.Messages` that will contain all our message classes

Next: [Messages][next]

[next]: {{ site.baseurl }}/guide-distribution-messages
