using System.Linq;
using NUnit.Framework;
using Shuttle.ESB.Core.EventSubscription;
using Shuttle.ESB.SqlServer;

namespace Shuttle.ESB.Test.Integration.SqlServer
{
    [TestFixture]
    public class SqlSubscriptionManagerTest : TestFixture
    {
        protected override void TestTearDown()
        {
        }

        private ISubscriptionManager manager;

        protected override void TestSetUp()
        {
            manager = new SqlSubscriptionManager();

            manager.Start();
        }

        [Test]
        public void Should_be_able_to_subscribe_and_unsubscribe_a_single_message_type()
        {
            const string messageType = "messagetype";
            var messageTypes = new[] { messageType };

            manager.Subscribe("inboxuri", "backboneinboxuri", messageTypes);

            Assert.AreEqual(1, manager.GetInboxWorkQueueUris(messageType).Count());

            manager.Unsubscribe("inboxuri", messageTypes);

            Assert.AreEqual(0, manager.GetInboxWorkQueueUris(messageType).Count());
        }

        [Test]
        public void Should_be_able_to_subscribe_and_unsubscribe_all_message_types()
        {
            const string messagetype1 = "messagetype1";
            const string messagetype2 = "messagetype2";
            const string messagetype3 = "messagetype3";

            var messageTypes = new[] { messagetype1, messagetype2, messagetype3 };

            manager.Subscribe("inboxuri", "backboneinboxuri", messageTypes);

            Assert.AreEqual(1, manager.GetInboxWorkQueueUris(messagetype1).Count());
            Assert.AreEqual(1, manager.GetInboxWorkQueueUris(messagetype2).Count());
            Assert.AreEqual(1, manager.GetInboxWorkQueueUris(messagetype3).Count());

            manager.Unsubscribe("inboxuri");

            Assert.AreEqual(0, manager.GetInboxWorkQueueUris(messagetype1).Count());
            Assert.AreEqual(0, manager.GetInboxWorkQueueUris(messagetype2).Count());
            Assert.AreEqual(0, manager.GetInboxWorkQueueUris(messagetype3).Count());
        }

        [Test]
        public void Should_be_able_to_get_the_backbone_uris()
        {
            const string messagetype1 = "messagetype1";

            var messageTypes = new[] { messagetype1 };

            manager.Subscribe("inboxuri", "backboneinboxuri", messageTypes);

            Assert.AreEqual("backboneinboxuri", manager.GetBackboneInboxWorkQueueUri("inboxuri"));
            Assert.AreEqual(1, manager.GetBackboneInboxWorkQueueUris().Count());

            manager.Unsubscribe("inboxuri");
        }
    }
}