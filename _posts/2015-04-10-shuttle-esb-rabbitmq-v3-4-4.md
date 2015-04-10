---
title: shuttle-esb-rabbitmq v3.4.4
layout: post
---

# shuttle-esb-rabbitmq v3.4.4

A new release of the shuttle-esb-rabbitmq nuget package is available.

Connection failures should be correctly retried with this version.

# .NET 3.5 support ended

During this refactoring process it came to light that the latest RabbitMQ.Client (v3.5.1) no longer support .NET 3.5 which means that the shuttle-esb-rabbitmq implementation also no longer can support .NET 3.5.

Hopefully this will not affect too many folks.  The older versions are still available, though.