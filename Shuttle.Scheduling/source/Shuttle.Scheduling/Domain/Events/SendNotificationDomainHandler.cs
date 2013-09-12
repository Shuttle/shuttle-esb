using System;
using Shuttle.Core.Data;
using Shuttle.Core.Domain;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.Scheduling
{
	public class SendNotificationDomainHandler : IDomainEventHandler<SendNotification>
	{
		private readonly IScheduleRepository scheduleRepository;

		public SendNotificationDomainHandler(IScheduleRepository scheduleRepository)
		{
			Guard.AgainstNull(scheduleRepository, "scheduleRepository");

			this.scheduleRepository = scheduleRepository;
		}

		public IServiceBus Bus { get; set; }
		public IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }

		public void Handle(SendNotification args)
		{
			using (DatabaseConnectionFactory.Create(SchedulerData.Source))
			{
				var message = new RunScheduleCommand
								{
									Name = args.Schedule.Id,
									DateDue = args.Due,
									DateSent = DateTime.Now,
									ServerName = Environment.MachineName
								};

				scheduleRepository.SaveNextNotification(args.Schedule);

				Bus.Send(message, args.Schedule.InboxWorkQueueUri);

				Log.For(this).Debug(string.Format("RunScheduleCommand '{0}' sent to {1}", message.Name, args.Schedule.InboxWorkQueueUri));
			}
		}
	}
}