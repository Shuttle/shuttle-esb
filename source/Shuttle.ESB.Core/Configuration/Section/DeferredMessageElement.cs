using System;
using System.ComponentModel;
using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class DeferredMessageElement : ConfigurationElement
    {
        private static readonly ConfigurationProperty durationToSleepWhenIdle =
            new ConfigurationProperty("durationToSleepWhenIdle", typeof(TimeSpan[]), null, ConfigurationPropertyOptions.None);

	    public DeferredMessageElement()
        {
            base.Properties.Add(durationToSleepWhenIdle);
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
    }
}