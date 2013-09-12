using System;
using System.ComponentModel;
using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class InboxElement : ConfigurationElement
    {
        private static readonly ConfigurationProperty workQueueStartupAction =
            new ConfigurationProperty("workQueueStartupAction", typeof (QueueStartupAction), QueueStartupAction.None,
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty workQueueUri =
            new ConfigurationProperty("workQueueUri", typeof(string), string.Empty,
                                      ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty errorQueueUri =
            new ConfigurationProperty("errorQueueUri", typeof (string), string.Empty,
                                      ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty journalQueueUri =
            new ConfigurationProperty("journalQueueUri", typeof (string), string.Empty,
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty threadCount =
            new ConfigurationProperty("threadCount", typeof(int), 5, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty maximumFailureCount =
            new ConfigurationProperty("maximumFailureCount", typeof(int), 5, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty durationToIgnoreOnFailure =
            new ConfigurationProperty("durationToIgnoreOnFailure", typeof(TimeSpan[]), null, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty durationToSleepWhenIdle =
            new ConfigurationProperty("durationToSleepWhenIdle", typeof(TimeSpan[]), null, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty distribute =
            new ConfigurationProperty("distribute", typeof(bool), false, ConfigurationPropertyOptions.None);

        public InboxElement()
        {
            base.Properties.Add(workQueueStartupAction);
            base.Properties.Add(workQueueUri);
            base.Properties.Add(errorQueueUri);
            base.Properties.Add(journalQueueUri);
            base.Properties.Add(threadCount);
            base.Properties.Add(durationToSleepWhenIdle);
            base.Properties.Add(maximumFailureCount);
            base.Properties.Add(durationToIgnoreOnFailure);
            base.Properties.Add(distribute);
        }

        [ConfigurationProperty("workQueueStartupAction", IsRequired = false, DefaultValue = QueueStartupAction.None)]
        public QueueStartupAction WorkQueueStartupAction
        {
            get { return (QueueStartupAction) this[workQueueStartupAction]; }
        }

        [ConfigurationProperty("workQueueUri", IsRequired = true)]
        public string WorkQueueUri
        {
            get { return (string) this[workQueueUri]; }
        }

        [ConfigurationProperty("errorQueueUri", IsRequired = true)]
        public string ErrorQueueUri
        {
            get { return (string) this[errorQueueUri]; }
        }

        [ConfigurationProperty("journalQueueUri", IsRequired = false)]
        public string JournalQueueUri
        {
            get { return (string) this[journalQueueUri]; }
        }

        [ConfigurationProperty("threadCount", IsRequired = false)]
        public int ThreadCount
        {
            get { return (int)this[threadCount]; }
        }

        [TypeConverter(typeof(StringDurationArrayConverter))]
        [ConfigurationProperty("durationToSleepWhenIdle", IsRequired = false)]
        public TimeSpan[] DurationToSleepWhenIdle
        {
            get
            {
                return (TimeSpan[])this[durationToSleepWhenIdle];
            }
        }

        [TypeConverter(typeof(StringDurationArrayConverter))]
        [ConfigurationProperty("durationToIgnoreOnFailure", IsRequired = false)]
        public TimeSpan[] DurationToIgnoreOnFailure
        {
            get
            {
                return (TimeSpan[])this[durationToIgnoreOnFailure];
            }
        }

        [ConfigurationProperty("maximumFailureCount", IsRequired = false)]
        public int MaximumFailureCount
        {
            get
            {
                return (int)this[maximumFailureCount];
            }
        }

        [ConfigurationProperty("distribute", IsRequired = false)]
        public bool Distribute
        {
            get
            {
                return (bool)this[distribute];
            }
        }
    }
}