using System;
using System.Configuration;
using System.Transactions;

namespace Shuttle.ESB.Core
{
	public class TransactionScopeElement : ConfigurationElement
	{
		private const int DefaultTimeoutSeconds = 30;
		private const IsolationLevel DefaultIsolationLevel = IsolationLevel.ReadCommitted;

		private static readonly ConfigurationProperty enabled =
			new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty isolationLevel =
			new ConfigurationProperty("isolationLevel", typeof(IsolationLevel), DefaultIsolationLevel,
									  ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty timeoutSeconds =
			new ConfigurationProperty("timeoutSeconds", typeof(int), DefaultTimeoutSeconds,
									  ConfigurationPropertyOptions.None);

		public TransactionScopeElement()
		{
			base.Properties.Add(enabled);
			base.Properties.Add(isolationLevel);
			base.Properties.Add(timeoutSeconds);
		}

		[ConfigurationProperty("enabled", IsRequired = false)]
		public bool Enabled
		{
			get
			{
				return (bool)this[enabled];
			}
		}

		[ConfigurationProperty("isolationLevel", IsRequired = false, DefaultValue = DefaultIsolationLevel)]
		public IsolationLevel IsolationLevel
		{
			get
			{
				var value = this[isolationLevel];

				if (value == null || string.IsNullOrEmpty(value.ToString()))
				{
					return DefaultIsolationLevel;
				}

				try
				{
					return (IsolationLevel)Enum.Parse(typeof(IsolationLevel), value.ToString());
				}
				catch
				{
					return DefaultIsolationLevel;
				}
			}
		}

		[ConfigurationProperty("timeoutSeconds", IsRequired = false, DefaultValue = "30")]
		public int TimeoutSeconds
		{
			get
			{
				var value = this[timeoutSeconds].ToString();

				if (string.IsNullOrEmpty(value))
				{
					return DefaultTimeoutSeconds;
				}

				int result;

				return int.TryParse(value, out result) ? result : DefaultTimeoutSeconds;
			}
		}
	}
}