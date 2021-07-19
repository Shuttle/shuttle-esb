---
title: Throttle Module
layout: api
---
# Throttle Module

```
PM> Install-Package Shuttle.Esb.Module.Throttle
```

The Throttle module for Shuttle.Esb aborts pipeline processing when the CPU usage exceeds given percentage.

The module will attach the `ThrottleObserver` to the `OnPipelineStarting` event of all pipelines except the `StartupPipeline` and abort the pipeline if the SPU usage is exceeds the given percentage.

```xml
<configuration>
    <configSections>
        <section name="throttle" type="Shuttle.Esb.Module.Throttle.ThrottleSection, Shuttle.Esb.Module.Throttle"/>
    </configSections>

  <throttle 
    cpuUsagePercentage="65"
    abortCycleCount="5"
    performanceCounterReadInterval="1000"
    durationToSleepOnAbort="1s" />
</configuration>
```

| Attribute                        | Default     | Description    | 
| ---                            | ---        | ---            | 
| cpuUsagePercentage            | 65        | The CPU usage percentage to start throttling the endpoint pipelines. |
| abortCycleCount                | 5        | The number of times a pipeline will be aborted before running at least once. |
| performanceCounterReadInterval                | 1000        | The number of milliseconds between reading the CPU usage performance counter.  Minimun of 1000 allowed. |
| durationToSleepOnAbort    | 1s        | The duration(s) to sleep when aborting a pipeline.  Cannot be incremented for each abort. |

The module will register/resolve itself using [Shuttle.Core container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#bootstrapping).