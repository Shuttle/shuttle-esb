using Shuttle.ESB.Msmq;
using Shuttle.ESB.SqlServer;

namespace Shuttle.ESB.Test.Integration
{
	public class SqlServerSectionFixture
	{
		protected SqlServerSection GetSqlServerSection(string file)
		{
			return SqlServerSection.Open(string.Format(@".\ConfigurationSections\SqlServer\files\{0}", file));
		}
	}
}