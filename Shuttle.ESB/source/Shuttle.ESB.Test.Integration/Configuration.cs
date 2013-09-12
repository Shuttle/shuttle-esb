using log4net;
using log4net.Config;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Log4Net;

namespace Shuttle.ESB.Test.Integration
{
	[SetUpFixture]
	public class Configuration
	{
		public Configuration()
		{
			XmlConfigurator.Configure();

			Log.Assign(new Log4NetLog(LogManager.GetLogger(GetType())));

			Log.Information("Logging configured.");
		}
	}
}