---
title: Dependency Injection Guide - Messages
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-dependencyinjection.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-dependencyinjection-messages'</script>
# Messages

> Create a new class library called `Shuttle.DependencyInjection.Messages` with a solution called `Shuttle.DependencyInjection`

**Note**: remember to change the *Solution name*.

![New solution]({{ site.baseurl }}/assets/images/guide-dependencyinjection-solution.png "New solution")

## RegisterMemberCommand

> Rename the `Class1` default file to `RegisterMemberCommand` and add a `UserName` property.

``` c#
namespace Shuttle.DependencyInjection.Messages
{
	public class RegisterMemberCommand
	{
		public string UserName { get; set; }
	}
}
```

Previous: [Overview][previous] | Next: [Client][next]

[previous]: {{ site.baseurl }}/guide-dependencyinjection-overview
[next]: {{ site.baseurl }}/guide-dependencyinjection-client
