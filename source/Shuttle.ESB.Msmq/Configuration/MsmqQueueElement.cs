using System.Configuration;

namespace Shuttle.ESB.Msmq
{
	public class MsmqQueueElement: ConfigurationElement
    {
        private static readonly ConfigurationProperty uri =
            new ConfigurationProperty("uri", typeof (string), string.Empty, ConfigurationPropertyOptions.IsRequired);

		private static readonly ConfigurationProperty isTransactional =
			new ConfigurationProperty("isTransactional", typeof(bool), true, ConfigurationPropertyOptions.IsRequired);
		
		public MsmqQueueElement()
        {
            base.Properties.Add(uri);
			base.Properties.Add(isTransactional);
        }

        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get
            {
                return (string)this[uri];
            }
        }

		[ConfigurationProperty("isTransactional", IsRequired = true)]
		public bool IsTransactional
		{
			get
			{
				return (bool)this[isTransactional];
			}
		}
	}
}