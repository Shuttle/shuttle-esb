using System;
using System.ComponentModel;
using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class OutboxElement : ConfigurationElement
    {
        [ConfigurationProperty("workQueueUri", IsRequired = true)]
        public string WorkQueueUri
        {
			get { return (string)this["workQueueUri"]; }
        }

        [ConfigurationProperty("errorQueueUri", IsRequired = true)]
        public string ErrorQueueUri
        {
			get { return (string)this["errorQueueUri"]; }
        }

        [TypeConverter(typeof(StringDurationArrayConverter))]
        [ConfigurationProperty("durationToSleepWhenIdle", IsRequired = false)]
        public TimeSpan[] DurationToSleepWhenIdle
        {
            get
            {
				return (TimeSpan[])this["durationToSleepWhenIdle"];
            }
        }

        [TypeConverter(typeof(StringDurationArrayConverter))]
        [ConfigurationProperty("durationToIgnoreOnFailure", IsRequired = false)]
        public TimeSpan[] DurationToIgnoreOnFailure
        {
            get
            {
				return (TimeSpan[])this["durationToIgnoreOnFailure"];
            }
        }

        [ConfigurationProperty("maximumFailureCount", IsRequired = false)]
        public int MaximumFailureCount
        {
            get
            {
				return (int)this["maximumFailureCount"];
            }
        }

		[ConfigurationProperty("threadCount", IsRequired = false)]
		public int ThreadCount
		{
			get { return (int)this["threadCount"]; }
		}
	}
}