using System;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	[TestFixture]
	public class InboxServiceBusSection : ServiceBusSectionFixture
	{
		[Test]
		public void Should_be_able_to_load_a_full_configuration()
		{
			var section = GetServiceBusSection("Inbox-Full.config");

			Assert.IsNotNull(section);

			Assert.AreEqual("msmq://./inbox-work", section.Inbox.WorkQueueUri);
			Assert.AreEqual("msmq://./inbox-error", section.Inbox.ErrorQueueUri);

			Assert.AreEqual(25, section.Inbox.ThreadCount);
			Assert.AreEqual(25, section.Inbox.MaximumFailureCount);

			Assert.AreEqual(TimeSpan.FromMilliseconds(250), section.Inbox.DurationToSleepWhenIdle[0]);
			Assert.AreEqual(TimeSpan.FromSeconds(10), section.Inbox.DurationToSleepWhenIdle[1]);
			Assert.AreEqual(TimeSpan.FromSeconds(30), section.Inbox.DurationToSleepWhenIdle[2]);

			Assert.AreEqual(TimeSpan.FromMinutes(30), section.Inbox.DurationToIgnoreOnFailure[0]);
			Assert.AreEqual(TimeSpan.FromHours(1), section.Inbox.DurationToIgnoreOnFailure[1]);
		}

		[Test]
		public void Should_be_able_to_load_with_only_required_configuration()
		{
			var section = GetServiceBusSection("Inbox-Required.config");

			Assert.IsNotNull(section);

			Assert.AreEqual("msmq://./inbox-work", section.Inbox.WorkQueueUri);
			Assert.AreEqual("msmq://./inbox-error", section.Inbox.ErrorQueueUri);
		}
	}
}