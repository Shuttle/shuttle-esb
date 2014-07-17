using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	[TestFixture]
	public class SqlServerSectionTest : SqlServerSectionFixture
	{
		[Test]
		[TestCase("SqlServer.config")]
		[TestCase("SqlServer-Grouped.config")]
		public void Should_be_able_to_load_a_full_configuration(string file)
		{
			var section = GetSqlServerSection(file);

			Assert.IsNotNull(section);

			Assert.AreEqual("subscription-connection-string-name", section.SubscriptionManagerConnectionStringName);
			Assert.AreEqual(".\\custom-script-folder", section.ScriptFolder);
		}
	}
}