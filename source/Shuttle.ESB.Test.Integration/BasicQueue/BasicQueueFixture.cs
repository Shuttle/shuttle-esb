using System;
using System.IO;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
	public class BasicQueueFixture : IntegrationFixture
	{
		protected void TestSimpleEnqueueAndGetMessage(string workQueueUriFormat)
		{
			var workQueue = GetWorkQueue(workQueueUriFormat);

			var stream = new MemoryStream();

			stream.WriteByte(100);

			var messageId = Guid.NewGuid();

			workQueue.Enqueue(messageId, stream);

			var receivedMessage = workQueue.GetMessage();

			Assert.AreEqual(100, receivedMessage.Stream.ReadByte());
			Assert.IsNull(workQueue.GetMessage());

			workQueue.Acknowledge(receivedMessage.AcknowledgementToken);

			Assert.IsNull(workQueue.GetMessage());

			workQueue.AttemptDrop();
		}

		protected void TestReleaseMessage(string workQueueUriFormat)
		{
			var workQueue = GetWorkQueue(workQueueUriFormat);

			var messageId = Guid.NewGuid();

			workQueue.Enqueue(messageId, new MemoryStream());

			var receivedMessage = workQueue.GetMessage();

			Assert.IsNotNull(receivedMessage);
			Assert.IsNull(workQueue.GetMessage());

			workQueue.Release(receivedMessage.AcknowledgementToken);

			receivedMessage = workQueue.GetMessage();

			Assert.IsNotNull(receivedMessage);
			Assert.IsNull(workQueue.GetMessage());

			workQueue.Acknowledge(receivedMessage.AcknowledgementToken);

			Assert.IsNull(workQueue.GetMessage());

			workQueue.AttemptDrop();
		}

		protected void TestUnacknowledgedMessage(string workQueueUriFormat)
		{
			var workQueue = GetWorkQueue(workQueueUriFormat);

			var messageId = Guid.NewGuid();

			workQueue.Enqueue(messageId, new MemoryStream());

			Assert.IsNotNull(workQueue.GetMessage());
			Assert.IsNull(workQueue.GetMessage());

			workQueue.AttemptDispose();

			workQueue = GetWorkQueue(workQueueUriFormat, false);

			var receivedMessage = workQueue.GetMessage();

			Assert.IsNotNull(receivedMessage);
			Assert.IsNull(workQueue.GetMessage());

			workQueue.Acknowledge(receivedMessage.AcknowledgementToken);
			workQueue.AttemptDispose();

			workQueue = GetWorkQueue(workQueueUriFormat, false);

			Assert.IsNull(workQueue.GetMessage());

			workQueue.AttemptDrop();
		}

		private IQueue GetWorkQueue(string workQueueUriFormat)
		{
			return GetWorkQueue(workQueueUriFormat, true);
		}

		private IQueue GetWorkQueue(string workQueueUriFormat, bool refresh)
		{
			using (var queueManager = QueueManager.Default())
			{
				var workQueue = queueManager.GetQueue(string.Format(workQueueUriFormat, "test-work"));

				if (refresh)
				{
					workQueue.AttemptDrop();
					workQueue.AttemptCreate();
					workQueue.AttemptPurge();
				}

				return workQueue;
			}
		}
	}
}