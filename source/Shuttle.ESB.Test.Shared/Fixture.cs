using System;
using NUnit.Framework;
using Rhino.Mocks;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Shared
{
    [TestFixture]
    public abstract class Fixture
    {
        [SetUp]
        public void SetUpTest()
        {
            Mocks = new MockRepository();

            Serializer = new DefaultSerializer();

            TestSetUp();
        }

        [TearDown]
        protected virtual void TearDownTest()
        {
        }

        protected MockRepository Mocks { get; private set; }

        public ISerializer Serializer { get; private set; }

        public ServiceBusConfiguration CreateMemoryConfiguration()
        {
            MemoryQueue.Clear();

            return new ServiceBusConfiguration
                                                  {
                                                      Inbox =
                                                          new InboxQueueConfiguration
                                                              {
                                                                  WorkQueue = CreateMemoryInboxWork(),
                                                                  JournalQueue = CreateMemoryInboxJournal(),
                                                                  ErrorQueue = CreateMemoryInboxError(),
                                                                  DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
                                                                  ThreadCount = 1
                                                              },
                                                      ControlInbox =
                                                          new ControlInboxQueueConfiguration
                                                              {
                                                                  WorkQueue = CreateMemoryControlInboxWork(),
                                                                  JournalQueue = CreateMemoryControlInboxJournal(),
                                                                  ErrorQueue = CreateMemoryControlInboxError(),
                                                                  DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
                                                                  ThreadCount = 1
                                                              },
                                                      //Outbox =
                                                      //    new OutboxQueueConfiguration
                                                      //        {
                                                      //            WorkQueue = CreateMemoryOutboxWork(),
                                                      //            ErrorQueue = CreateMemoryOutboxError(),
                                                      //            DurationToSleepWhenIdle = new[] { TimeSpan.FromMilliseconds(5) },
                                                      //            ThreadCount = 1
                                                      //        },
                                                      Serializer = new DefaultSerializer(),
                                                      MessageHandlerFactory = new DefaultMessageHandlerFactory(),
                                                      PipelineFactory = new DefaultPipelineFactory(),
                                                      TransactionScopeFactory = new DefaultServiceBusTransactionScopeFactory(),
                                                      ThreadActivityFactory = new DefaultThreadActivityFactory()
                                                  };
        }

        public IQueue CreateMemoryInboxWork()
        {
            return MemoryQueue.From("memory://./inbox_work");
        }

        public IQueue CreateMemoryInboxJournal()
        {
            return MemoryQueue.From("memory://./inbox_journal");
        }

        public IQueue CreateMemoryInboxError()
        {
            return MemoryQueue.From("memory://./inbox_error");
        }

        public IQueue CreateMemoryControlInboxWork()
        {
            return MemoryQueue.From("memory://./control-inbox-work");
        }

        public IQueue CreateMemoryControlInboxJournal()
        {
            return MemoryQueue.From("memory://./control-inbox-journal");
        }

        public IQueue CreateMemoryControlInboxError()
        {
            return MemoryQueue.From("memory://./control-inbox-error");
        }

        public IQueue CreateMemoryOutboxWork()
        {
            return MemoryQueue.From("memory://./outbox_work");
        }

        public IQueue CreateMemoryOutboxError()
        {
            return MemoryQueue.From("memory://./outbox_error");
        }

        [TestFixtureSetUp]
        protected virtual void FixtureSetUp()
        {
        }

        protected virtual void TestSetUp()
        {
        }

        protected T Mock<T>() where T : class
        {
            return MockRepository.GenerateMock<T>();
        }

        protected T Stub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }

        [TestFixtureTearDown]
        protected virtual void FixtureTearDown()
        {
        }
    }
}