using System.Configuration;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqSection : ConfigurationSection
	{
		public static MsmqSection Open(string file)
		{
			return ConfigurationManager
					   .OpenMappedMachineConfiguration(new ConfigurationFileMap(file))
					   .GetSection("msmq") as MsmqSection;
		}

		private static readonly ConfigurationProperty queues =
			new ConfigurationProperty("queues", typeof(MsmqQueueElementCollection), null,
									  ConfigurationPropertyOptions.None);

        public MsmqSection()
        {
			base.Properties.Add(queues);
        }

		[ConfigurationProperty("queues", IsRequired = true)]
		public MsmqQueueElementCollection Queues
        {
			get { return (MsmqQueueElementCollection)this[queues]; }
        }
	}
}