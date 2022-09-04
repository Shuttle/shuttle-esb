# Modules

More information on the pipelines infrastructure can be obtained in the [Shuttle.Core.Pipelines](https://shuttle.github.io/shuttle-core/infrastructure/shuttle-core-pipelines.html) documentation.

Shuttle.Esb is, therefore, also extensible via modules.  These plug into a relevant pipeline to perform additional tasks within the pipeline by registering one or more observers that respond to the events raised by the pipeline.

Each pipeline has state that contains various name/value pair items.  You can add state, and there are some extensions on the state that return various well-known items such as `GetTransportMessage()` that returns the `TransportMessage` on the pipeline.  Prior to deserializing the transport message it will, of course, be `null`.

You can reference the [Shuttle.Esb](https://github.com/Shuttle/Shuttle.Esb/tree/master/Shuttle.Esb/Pipeline/Pipelines) code directly to get more information on the available pipelines and the events in those pipelines.
