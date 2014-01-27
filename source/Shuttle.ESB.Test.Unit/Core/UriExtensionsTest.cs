using System;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Core
{
	public class UriExtensionsTest : UnitFixture
	{
		[Test]
		public void Should_be_able_to_secure_a_uri()
		{
			Assert.AreEqual("uri://the-host/path", new Uri("uri://username:password@the-host/path").Secured().ToString());
		}
	}
}