# Active Time Range

```
PM> Install-Package Shuttle.Esb.Module.ActiveTimeRange
```

The ActiveTimeRange module for Shuttle.Esb aborts pipeline processing when the current date is not within a given time range.

## Configuration

```c#
services.AddActiveTimeRangeModule(builder => {
	builder.Options.ActiveFromTime = "10:00";
	builder.Options.ActiveToTime = "14:00";
});
```
The default JSON settings structure is as follows:

```json
{
  "Shuttle": {
      "Modules": {
        "ActiveTimeRange": {
          "ActiveFromTime": "8:00",
          "ActiveToTime":  "23:00" 
        }
      }
  }
}
```

The default value of "\*" ignores the value.  If both `from` and `to` are specified as "\*" no pipeline will be aborted.