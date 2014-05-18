using System.Configuration;

namespace Shuttle.ESB.SqlServer
{
	public class SqlServerSection : ConfigurationSection
	{
		public static SqlServerSection Open(string file)
		{
			return ConfigurationManager
					   .OpenMappedMachineConfiguration(new ConfigurationFileMap(file))
					   .GetSection("sqlServer") as SqlServerSection;
		}

		[ConfigurationProperty("subscriptionManagerConnectionStringName", IsRequired = false, DefaultValue = "Subscription")]
		public string SubscriptionManagerConnectionStringName
		{
			get
			{
				return (string)this["subscriptionManagerConnectionStringName"];
			}
		}

		[ConfigurationProperty("scriptFolder", IsRequired = false, DefaultValue = null)]
		public string ScriptFolder
		{
			get
			{
				return (string)this["scriptFolder"];
			}
		}
	}
}