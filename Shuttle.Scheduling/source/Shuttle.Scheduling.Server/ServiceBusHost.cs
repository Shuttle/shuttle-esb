using System;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using log4net;
using log4net.Config;
using Shuttle.Core.Data;
using Shuttle.Core.Data.Castle;
using Shuttle.Core.Domain;
using Shuttle.Core.Domain.Castle;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Castle;
using Shuttle.Core.Infrastructure.Log4Net;
using Shuttle.ESB.Core;
using Shuttle.ESB.Modules.ActiveTimeRange;
using Shuttle.ESB.SqlServer;

namespace Shuttle.Scheduling.Server
{
	public class ServiceBusHost : IHost, IDisposable, IActiveState
	{
		private readonly WindsorContainer container = new WindsorContainer();

		private IServiceBus bus;

		private volatile bool running = true;
		private Thread thread;
		private IDatabaseConnectionFactory databaseConnectionFactory;
		private IScheduleRepository repository;

		private readonly int millisecondsBetweenScheduleChecks = ConfigurationItem<int>.ReadSetting("MillisecondsBetweenScheduleChecks", 5000).GetValue();

		public void Dispose()
		{
			running = false;

			bus.Dispose();

			LogManager.Shutdown();
		}

		public void Start()
		{
			XmlConfigurator.Configure();

			Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof (Program))));

            ConnectionStrings.Approve();

            container.RegisterSingleton("Shuttle.Core.Infrastructure", RegexPatterns.EndsWith("Factory"));
			container.RegisterSingleton("Shuttle.Core.Infrastructure", RegexPatterns.EndsWith("Mapper"));

			container.RegisterDataAccessCore();
			container.RegisterDataAccessDefaults();

			container.Register(Component.For<IDatabaseConnectionCache>().ImplementedBy<ThreadStaticDatabaseConnectionCache>());

			container.RegisterDataAccess("Shuttle.Scheduling");
			container.RegisterSingleton("Shuttle.Scheduling", RegexPatterns.EndsWith("Factory"));
			container.RegisterSingleton("Shuttle.Scheduling", RegexPatterns.EndsWith("DomainHandler"));

            DomainEvents.Assign(new DomainEventDispatcher(container));

			bus = ServiceBus
				.Create()
				.SubscriptionManager(SubscriptionManager.Default())
				.AddModule(new ActiveTimeRangeModule())
				.Start();

			container.Register(bus);

			repository = container.Resolve<IScheduleRepository>();
			databaseConnectionFactory = container.Resolve<IDatabaseConnectionFactory>();

			thread = new Thread(ProcessSchedule);

			thread.Start();
		}

		private void ProcessSchedule()
		{
			while (running)
			{
				using (databaseConnectionFactory.Create(SchedulerData.Source))
				{
					repository.All().ForEach(schedule => schedule.CheckNotification());
				}

				ThreadSleep.While(millisecondsBetweenScheduleChecks, this);
			}
		}

		public bool Active
		{
			get { return running; }
		}
	}
}