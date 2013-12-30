shuttle-esb
===========

[Have a look at our YouTube channel](http://www.youtube.com/shuttleservicebus)

We still have a lot of information on our [old wiki](http://servicebus.co.za/) but we will be migrating that as time goes by.

getting started with the source
===============================

A free .NET open-source enterprise service bus.

Once you have downloaded the source you need to initialize your environment by executing the `initialize-environment.msbuild` file.

> Open a Visual Studio Command Prompt

> cd {extract-folder}\build\\{relevant-version}\

> msbuild initialize-environment.msbuild

You will be prompted for your sql data source.  If you wish to keep the default (.\sqlexpress) or you you would want to check this out later simply press enter.


This makes changes w.r.t. the paths in some pertinent files will also call an msbuild task for `complete-build.debug.msbuild` to build the debug development environment.

