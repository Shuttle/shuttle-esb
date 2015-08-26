---
title: Idempotence Guide - Messages
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-idempotence.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-idempotence-messages'</script>
# Messages

> Create a new class library called `Shuttle.Idempotence.Messages` with a solution called `Shuttle.Idempotence`

**Note**: remember to change the *Solution name*.

![New solution]({{ site.baseurl }}/assets/images/guide-idempotence-solution.png "New solution")

## RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.Idempotence.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

Previous: [Overview][previous] | Next: [Client][next]

[previous]: {{ site.baseurl }}/guide-idempotence-overview
[next]: {{ site.baseurl }}/guide-idempotence-client
