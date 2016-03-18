---
title: Publish / Subscribe Guide - Messages
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-publish-subscribe.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-publish-subscribe-messages'</script>
# Messages

> Create a new class library called `Shuttle.PublishSubscribe.Messages` with a solution called `Shuttle.PublishSubscribe`

**Note**: remember to change the *Solution name*.

![New solution]({{ site.baseurl }}/assets/images/guide-publish-subscribe-solution.png "New solution")

## RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

~~~ c#
namespace Shuttle.PublishSubscribe.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
~~~

## MemberRegisteredEvent

> Add a new class called `MemberRegisteredEvent` also with a `UserName` property.

~~~ c#
namespace Shuttle.PublishSubscribe.Messages
{
	public class MemberRegisteredEvent
	{
		public string UserName { get; set; }
	}
}
~~~

Previous: [Overview][previous] | Next: [Client][next]

[previous]: {{ site.baseurl }}/guide-publish-subscribe-overview
[next]: {{ site.baseurl }}/guide-publish-subscribe-client
