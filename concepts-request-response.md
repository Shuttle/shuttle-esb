---
title: Request / Response Concepts
layout: api
---
## Request / Response

For some background on **Request/Response** messaging pattern you can have a look at the [Wikipedia article](http://en.wikipedia.org/wiki/Request-response).

![Request/Response Image]({{ site.baseurl }}/assets/images/Architecture-RequestResponse.png "Request/Response")

To request an endpoint to perform a certain function you send a command message:

~~~c#
    bus.Send(new RequestMessage());
~~~

Although this is a very simple pattern it results in rather tight behavioural coupling.  This is not necessarily a bad thing and in many instances it is definitely required.

Typically the message handler for the command message goes about its business and processes the message.  But there will be times when a response is required.

The response can then be a command message or an event message and you can simply call the **reply** method on the service bus instance:

~~~c#
    bus.Send(new ResponseMessage(), c => c.Reply());
~~~

The response may, of course, be decoupled by publishing an event message but it is up to the implementor to decide the mechanism.  This would then no longer be request/response but rather publish/subscribe.  The advantage of request/response is that it provides the ability to respond to the caller directly whereas publishing a message would result in **all** publishers receiving a copy of the message.

All message sending in Shuttle.Esb is uni-directional.  This means that a message will be displatched to the receipient queue where is will be processed by a message handler.  That message handler can then decide whether to respond by sending another message back (uni-directional) to the sender's work queue, or perhaps publish an event, or even do nothing.

