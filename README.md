shuttle-esb
===========

[Have a look at our YouTube channel](http://www.youtube.com/user/shuttleservicebus)

[Please visit our wiki for all the documentation](https://github.com/eben-roux/shuttle-esb/wiki/Shuttle-ESB)

getting started with the source
===============================

A free .NET open-source enterprise service bus.

Once you have downloaded the source you need to perform the following to get started:

  Execute build\initialize-environment.msbuild

  - if you don't have .msbuild files associated with MSBuild you can open a Visual Studio Command Prompt

    cd {extract-folder}\build\\{relevant-version}\

    msbuild initialize-environment.msbuild

    - this makes changes w.r.t. the paths in some pertinent files

    - it will also call an msbuild task for complete-build.debug.msbuild to build the debug development environment


*** NOTE ***

You will be prompted for your sql data source.

If you wish to keep the default (.\sqlexpress) or you you would want to check this out later simply press enter.



