---
title: Message Distribution Guide - Messages
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-distribution.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-distribution-messages'</script>
# Messages

> Create a new class library called `Shuttle.Distribution.Messages` with a solution called `Shuttle.Distribution`

**Note**: remember to change the *Solution name*.

![New solution]({{ site.baseurl }}/assets/images/guide-distribution-solution.png "New solution")

## RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.Distribution.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

Previous: [Overview][previous] | Next: [Client][next]

[previous]: {{ site.baseurl }}/guide-distribution-overview
[next]: {{ site.baseurl }}/guide-distribution-client
