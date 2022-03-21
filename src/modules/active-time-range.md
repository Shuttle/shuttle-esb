# Active Time Range

```
PM> Install-Package Shuttle.Esb.Module.ActiveTimeRange
```

The ActiveTimeRange module for Shuttle.Esb aborts pipeline processing when the current date is not within a given time range.

The module will attach the `ActiveTimeRangeObserver` to the `OnPipelineStarting` event of all pipelines except the `StartupPipeline` and abort the pipeline if the current time is not within the active time range.

```xml
<configuration>
	<configSections>
		<section name="activeTimeRange" type="Shuttle.Esb.Module.ActiveTimeRange.ActiveTimeRangeSection, Shuttle.Esb.Module.ActiveTimeRange"/>
	</configSections>

  <activeTimeRange from="8:00" to="23:00" />
</configuration>
```

The default value of "\*" ignores the value.  If both `from` and `to` are specified as "\*" no pipeline will be aborted.

## Registration / Activation

The required components may be registered by calling `ComponentRegistryExtensions.RegisterActiveTimeRange(IComponentRegistry)`.

In order for the module to attach to the `IPipelineFactory` you would need to resolve it using `IComponentResolver.Resolve<ActiveTimeRangeModule>()`.
