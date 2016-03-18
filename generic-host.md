---
title: Generic Service Host
layout: api
---
# Simple Service Hosting

The generic service host is implemented by **Shuttle.Core.Host.exe**.  Defined in the assembly is an `IHost` interface:

~~~ c#
namespace Shuttle.Core.Host
{
    public interface IHost
    {
        void Start();
    }
}
~~~

Whenever Shuttle.Core.Host.exe runs it will scan the folder it resides in and find all assemblies that contain a class implementing IHost.  There must be exactly one.  You are also able to specify the specific type to use on the command line.

So to test any projects in Visual Studio that will eventually be hosted as services you simply references Shuttle.Core.Host.exe and implement IHost on a class.  Then go to the project properties.  Under the *Debug* tab you set the *Start Action* to *Start external program* and select the path to Shuttle.Core.Host.exe that should be under you target path for your project.

In order to install a service you execute the generic host with the `/install` argument:

~~~
    Shuttle.Core.Host.exe /install
~~~

There are many options.  To view these execute the generic host with the `/?` argument.
