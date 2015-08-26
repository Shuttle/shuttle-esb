---
title: Dependency Injection Guide - Overview
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-dependencyinjection.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-dependencyinjection-overview'</script>
# Overview

By default shuttle-esb does not require a dependency injection container.  Shuttle makes use of an `IMessageHandlerFactory` implementation to create message handlers.  If no dependency injection container is required one could stick with the `DefaultMessageHandlerFactory` instantiated by default.

The `DefaultMessageHandlerFactory` requires message handlers that have a default (parameterless) constructor; else the instantiation of the handler will fail.  In this guide we will use the `WindsorContainer` that is part of the [Castle Project](https://github.com/castleproject/Windsor/blob/master/docs/README.md).

In this guide we'll create the following projects:

- a **Console Application** called `Shuttle.DependencyInjection.Client`
- a **Class Library** called `Shuttle.DependencyInjection.Server`
- another **Class Library** called `Shuttle.DependencyInjection.EMail` that will contain a fake e-mail service implementation
- and another **Class Library** called `Shuttle.DependencyInjection.Messages` that will contain all our message classes

Next: [Messages][next]

[next]: {{ site.baseurl }}/guide-dependencyinjection-messages
