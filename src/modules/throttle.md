# Throttle

```
PM> Install-Package Shuttle.Esb.Module.Throttle
```

The Throttle module for Shuttle.Esb aborts pipeline processing when the CPU usage exceeds given percentage.

The module will attach the `ThrottleObserver` to the `OnPipelineStarting` event of the `InboxMessagePipeline` and abort the pipeline if the CPU usage exceeds the given percentage.

## Configuration

```c#
services.AddThrottleModule(builder => 
{
	builder.Options.CpuUsagePercentage = 65;
	builder.Options.AbortCycleCount = 5;
	builder.Options.DurationToSleepOnAbort = new List<TimeSpan> { TimeSpan.FromSeconds(1) };
})
```

## Options

| Option | Default 	| Description	| 
| --- | --- | --- | 
| `CpuUsagePercentage` | 65 | The CPU usage percentage threshold to start throttling. |
| `AbortCycleCount` | 5 | The number of times a pipeline will be aborted before running at least once. |
| `DurationToSleepOnAbort`	| "00:00:01" | The duration(s) to sleep when aborting a pipeline.  Can be incremented for each abort. |
