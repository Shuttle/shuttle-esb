using System;
using System.Collections.Generic;
using System.Configuration;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public class ManagementDbConnectionConfigurationProvider : IDbConnectionConfigurationProvider
	{
		private readonly List<IDbConnectionConfigurationProvider> dbConnectionConfigurationProviders =
			new List<IDbConnectionConfigurationProvider>();

		public void AddProvider(IDbConnectionConfigurationProvider provider)
		{
			Guard.AgainstNull(provider, "provider");

			dbConnectionConfigurationProviders.Add(provider);
		}

		public IDbConnectionConfiguration Get(DataSource source)
		{
			Guard.AgainstNull(source, "source");

			var settings = ConfigurationManager.ConnectionStrings[source.Name];

			if (settings != null)
			{
				return new DbConnectionConfiguration(source, settings.ProviderName, settings.ConnectionString);
			}

			foreach (var provider in dbConnectionConfigurationProviders)
			{
				var configuration = provider.Get(source);

				if (configuration == null)
				{
					continue;
				}

				return configuration;
			}

			throw new InvalidOperationException(
				string.Format(
					"The required connection string with name '{0}' is not specified in the application configuration file and has not been registered with any of the providers.",
					source.Name));
		}
	}
}