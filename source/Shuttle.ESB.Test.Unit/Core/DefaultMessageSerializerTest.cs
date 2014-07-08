using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Unit.Core
{
	public class DefaultMessageSerializerTest : UnitFixture
	{
		private static ISerializer SUT()
		{
			return new DefaultSerializer();
		}

		[Test]
		public void Should_be_able_to_serialize_and_deserialize_a_transport_message()
		{
			var original = new TransportMessage();

			var sut = SUT();

			var stream = sut.Serialize(original);

			var xml = new StreamReader(stream).ReadToEnd();

			Assert.IsTrue(xml.Contains(original.MessageId.ToString()));

			stream.Position = 0;

			Assert.AreEqual(original.MessageId, ((TransportMessage)sut.Deserialize(typeof(TransportMessage), stream)).MessageId);
		}

		[Test]
		public void Should_be_able_to_serialize_and_deserialize_at_least_500_instances_with_a_simple_type_in_a_second()
		{
			var original = new TransportMessage
							   {
								   Message = SUT().Serialize(new SimpleCommand
								                             	{
								                             		Name = "SimpleMessage"
								                             	}).ToBytes()
							   };

			var serializer = SUT();

			var sw = new Stopwatch();

			sw.Start();

			const int REQUIREDCOUNT = 500;

			for (var i = 0; i < REQUIREDCOUNT; i++)
			{
				serializer.Deserialize(typeof(TransportMessage), serializer.Serialize(original));
			}

			sw.Stop();

			Console.WriteLine("Serialized/Deserialized {0} instances in {1} ms", REQUIREDCOUNT, sw.ElapsedMilliseconds);

			Assert.IsTrue(sw.ElapsedMilliseconds < 1000,
						  "Should be able to serialize and deserialize at least {0} instances with a simple type in a second",
						  REQUIREDCOUNT);
		}
	}
}