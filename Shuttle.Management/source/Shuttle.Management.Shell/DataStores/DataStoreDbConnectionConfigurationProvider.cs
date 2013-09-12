using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public class DataStoreDbConnectionConfigurationProvider : IDbConnectionConfigurationProvider
	{
		private readonly IManagementConfiguration managementConfiguration;

		public DataStoreDbConnectionConfigurationProvider(IManagementConfiguration managementConfiguration)
		{
			Guard.AgainstNull(managementConfiguration, "managementConfiguration");

			this.managementConfiguration = managementConfiguration;
		}

		public IDbConnectionConfiguration Get(DataSource source)
		{
			try
			{
				var store = managementConfiguration.DataStoreRepository().Get(source.Name);

				return store == null
				       	? null
				       	: new DbConnectionConfiguration(source,
				       	                                store.ProviderName,
				       	                                store.ConnectionString);
			}
			catch
			{
				return null;
			}
		}
	}
}