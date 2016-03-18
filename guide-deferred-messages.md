---
title: Deferred Messages Guide - Messages
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-deferred.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-deferred-messages'</script>
# Messages

> Create a new class library called `Shuttle.Deferred.Messages` with a solution called `Shuttle.Deferred`

**Note**: remember to change the *Solution name*.

![New solution]({{ site.baseurl }}/assets/images/guide-deferred-solution.png "New solution")

## RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

~~~ c#
namespace Shuttle.Deferred.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
~~~

Previous: [Overview][previous] | Next: [Client][next]

[previous]: {{ site.baseurl }}/guide-deferred-overview
[next]: {{ site.baseurl }}/guide-deferred-client
