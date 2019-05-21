---
title: Serialization
layout: api
---
# Serialization

Shuttle.Esb makes use of the serialization components provided by [Shuttle.Core.Serialization](http://shuttle.github.io/shuttle-core/shuttle-core-serialization).

In order to make use of an `ISerializer` implementation other than the `DefaultSerializer` you can either register the serializer explicitly in your [container](http://shuttle.github.io/shuttle-core/shuttle-core-container#supported) of choice or make use of a [bootstrapped](http://shuttle.github.io/shuttle-core/shuttle-core-container#bootstrapping) component such as the one provided by [Shuttle.Core.Json](http://shuttle.github.io/shuttle-core/shuttle-core-json).