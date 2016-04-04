---
title: Active Time Range Module
layout: api
---
# Active Time Range Module

The `ActiveTimeRangeModule` may be found in the `Shuttle.Esb.Modules` assembly.  The module will attach the `ActiveTimeRangeObserver` to the `OnPipelineStarting` event of all pipelines except the `StartupPipeline` and abort the pipeline if the current time is not within the active time range.

~~~xml
  <appSettings>
    <add key="ActiveFromTime" value="*"/>
    <add key="ActiveToTime" value="*"/>
  </appSettings>
~~~

The default value of `*` indicates the whole day and your pipelines will never be stopped.

~~~c#
	var bus = ServiceBus
		.Create(c => c.AddModule(new ActiveTimeRangeModule()))
		.Start();
~~~
