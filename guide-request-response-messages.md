---
title: Request / Response Guide
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-request-response.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-request-response-messages'</script>
# Messages

> Create a new class library called `Shuttle.RequestResponse.Messages` with a solution called `Shuttle.RequestResponse`

**Note**: remember to change the *Solution name*.

![New solution]({{ site.baseurl }}/assets/images/guide-request-response-solution.png "New solution")

## RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.RequestResponse.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

## MemberRegisteredEvent

> Add a new class called `MemberRegisteredEvent` also with a `UserName` property.

``` c#
namespace Shuttle.RequestResponse.Messages
{
	public class MemberRegisteredEvent
	{
		public string UserName { get; set; }
	}
}
```

Previous: [Overview][previous] | Next: [Client][next]

[previous]: {{ site.baseurl }}/guide-request-response-overview
[next]: {{ site.baseurl }}/guide-request-response-client
