using System;
using System.IO;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	[TestFixture]
	public class SqlServerSectionTest : SqlServerSectionFixture
	{
		[Test]
		public void Should_be_able_to_load_a_full_configuration()
		{
			var section = GetSqlServerSection("SqlServer.config");

			Assert.IsNotNull(section);

			Assert.AreEqual("subscription-connection-string-name", section.SubscriptionManagerConnectionStringName);
			Assert.AreEqual(".\\custom-script-folder", section.ScriptFolder);
		}
	}
}