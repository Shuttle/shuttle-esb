---
title: Dependency Injection Guide - e-mail
layout: guide
---
<script src="{{ site.baseurl }}/assets/js/guide-dependencyinjection.js"></script>
<script>shuttle.guideData.selectedItemName = 'guide-dependencyinjection-email'</script>
# E-Mail

To demonstrate the dependency injection we will create a fake e-mail service that we intend using in the server endpoint.

> Add a new `Class Library` to the solution called `Shuttle.DependencyInjection.EMail`.

![Dependency Injection E-Mail]({{ site.baseurl }}/assets/images/guide-dependencyinjection-email.png "Dependency Injection E-Mail")

## IEMailService

> Add an interface called `IEMailService` and implement it as follows:

``` c#
namespace Shuttle.DependencyInjection.EMail
{
	public interface IEMailService
	{
		void Send(string name);
	}
}
```

## EMailService

> Rename the default `Class1` file to `EMailService` and implement the `IEMailService` interfaces as follows:

``` c#
using System;
using System.Threading;

namespace Shuttle.DependencyInjection.EMail
{
	public class EMailService : IEMailService
	{
		public void Send(string name)
		{
			Console.WriteLine();
			Console.WriteLine("[SENDING E-MAIL] : name = '{0}'", name);
			Console.WriteLine();

			Thread.Sleep(3000); // simulate communication wait time

			Console.WriteLine();
			Console.WriteLine("[E-MAIL SENT] : name = '{0}'", name);
			Console.WriteLine();
		}
	}
}
```

Previous: [Client][previous] | Next: [Server][next]

[previous]: {{ site.baseurl }}/guide-dependencyinjection-client
[next]: {{ site.baseurl }}/guide-dependencyinjection-server