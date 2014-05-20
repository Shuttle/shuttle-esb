using System;
using System.Configuration;
using System.IO;

namespace Shuttle.ESB.SqlServer
{
	public class SqlServerConfiguration : ISqlServerConfiguration
	{
		private static SqlServerSection section;

		public SqlServerConfiguration()
		{
			SubscriptionManagerConnectionStringName = "Subscription";
			ScriptFolder = null;

			NormalizeScriptFolder();
		}

		private void NormalizeScriptFolder()
		{
			if (string.IsNullOrEmpty(ScriptFolder))
			{
				ScriptFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");
			}
			else
			{
				if (!Path.IsPathRooted(ScriptFolder))
				{
					ScriptFolder = Path.GetFullPath(ScriptFolder);
				}
			}
		}

		public static SqlServerSection SqlServerSection
		{
			get
			{
				return section ??
				       (section = ConfigurationManager.GetSection("sqlServer") as SqlServerSection);
			}
		}

		public string SubscriptionManagerConnectionStringName { get; set; }
		public string ScriptFolder { get; set; }

		public static SqlServerConfiguration Default()
		{
			var configuration = new SqlServerConfiguration();

			if (SqlServerSection != null)
			{
				configuration.SubscriptionManagerConnectionStringName = section.SubscriptionManagerConnectionStringName;
				configuration.ScriptFolder = section.ScriptFolder;

				configuration.NormalizeScriptFolder();
			}

			return configuration;
		}
	}
}