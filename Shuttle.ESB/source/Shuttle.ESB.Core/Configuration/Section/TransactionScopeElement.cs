using System;
using System.Configuration;
using System.Transactions;

namespace Shuttle.ESB.Core
{
    public class TransactionScopeElement : ConfigurationElement
    {
        private static readonly TimeSpan defaultTimeoutSeconds = TimeSpan.FromSeconds(30);
        private const IsolationLevel defaultIsolationLevel = IsolationLevel.ReadCommitted;

        private static readonly ConfigurationProperty isolationLevel =
            new ConfigurationProperty("isolationLevel", typeof (IsolationLevel), defaultIsolationLevel,
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty timeoutSeconds =
            new ConfigurationProperty("timeoutSeconds", typeof(TimeSpan), defaultTimeoutSeconds,
                                      ConfigurationPropertyOptions.None);

        public TransactionScopeElement()
        {
            base.Properties.Add(isolationLevel);
            base.Properties.Add(timeoutSeconds);
        }

        [ConfigurationProperty("isolationLevel", IsRequired = false, DefaultValue = defaultIsolationLevel)]
        public IsolationLevel IsolationLevel
        {
            get
            {
                var value = this[isolationLevel];

                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultIsolationLevel;
                }

                try
                {
                    return (IsolationLevel)Enum.Parse(typeof(IsolationLevel), value.ToString());
                }
                catch
                {
                    return defaultIsolationLevel;
                }
            }
        }

        [ConfigurationProperty("timeoutSeconds", IsRequired = false, DefaultValue = "30")]
        public TimeSpan TimeoutSeconds
        {
            get
            {
                var value = this[timeoutSeconds];

                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultTimeoutSeconds;
                }

                try
                {
                    return TimeSpan.FromSeconds(int.Parse(value.ToString()));
                }
                catch
                {
                    return defaultTimeoutSeconds;
                }
            }
        }
    }
}