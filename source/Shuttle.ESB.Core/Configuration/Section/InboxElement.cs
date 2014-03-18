using System;
using System.ComponentModel;
using System.Configuration;

namespace Shuttle.ESB.Core
{
	public class InboxElement : ConfigurationElement
	{
		[ConfigurationProperty("workQueueStartupAction", IsRequired = false, DefaultValue = QueueStartupAction.None)]
		public QueueStartupAction WorkQueueStartupAction
		{
			get { return (QueueStartupAction) this["workQueueStartupAction"]; }
		}

		[ConfigurationProperty("workQueueUri", IsRequired = true)]
		public string WorkQueueUri
		{
			get { return (string) this["workQueueUri"]; }
		}

		[ConfigurationProperty("deferredQueueUri", IsRequired = false, DefaultValue = "")]
		public string DeferredQueueUri
		{
			get { return (string) this["deferredQueueUri"]; }
		}

		[ConfigurationProperty("errorQueueUri", IsRequired = true)]
		public string ErrorQueueUri
		{
			get { return (string) this["errorQueueUri"]; }
		}

		[ConfigurationProperty("threadCount", IsRequired = false, DefaultValue = 5)]
		public int ThreadCount
		{
			get { return (int) this["threadCount"]; }
		}

		[TypeConverter(typeof (StringDurationArrayConverter))]
		[ConfigurationProperty("durationToSleepWhenIdle", IsRequired = false, DefaultValue = null)]
		public TimeSpan[] DurationToSleepWhenIdle
		{
			get { return (TimeSpan[]) this["durationToSleepWhenIdle"]; }
		}

		[TypeConverter(typeof (StringDurationArrayConverter))]
		[ConfigurationProperty("durationToIgnoreOnFailure", IsRequired = false, DefaultValue = null)]
		public TimeSpan[] DurationToIgnoreOnFailure
		{
			get { return (TimeSpan[]) this["durationToIgnoreOnFailure"]; }
		}

		[ConfigurationProperty("maximumFailureCount", IsRequired = false, DefaultValue = 5)]
		public int MaximumFailureCount
		{
			get { return (int) this["maximumFailureCount"]; }
		}

		[ConfigurationProperty("distribute", IsRequired = false, DefaultValue = false)]
		public bool Distribute
		{
			get { return (bool) this["distribute"]; }
		}
	}
}