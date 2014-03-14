using System;
using System.Diagnostics;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class IdempotenceServiceExtensions
	{
		public static void AccessException(this IIdempotenceService service, ILog log, Exception ex, ObservablePipeline pipeline)
		{
			Guard.AgainstNull(service, "service");
			Guard.AgainstNull(log, "log");
			Guard.AgainstNull(ex, "ex");
			Guard.AgainstNull(pipeline, "pipeline");

			log.Fatal(string.Format( ESBResources.FatalIdempotenceServiceException, service.GetType().FullName, ex.CompactMessages()));

			pipeline.Abort();
		}
	}
}