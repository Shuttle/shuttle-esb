using System;
using System.Messaging;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqReleaseMessageObserver :
		IPipelineObserver<OnReleaseMessage>,
		IPipelineObserver<OnStart>
	{
		private readonly MessagePropertyFilter _messagePropertyFilter;
		private readonly ILog _log;

		public MsmqReleaseMessageObserver()
		{
			_log = Log.For(this);

			_messagePropertyFilter = new MessagePropertyFilter();
			_messagePropertyFilter.SetAll();
		}

		public void Execute(OnStart pipelineEvent)
		{
			var parser = pipelineEvent.Pipeline.State.Get<MsmqUriParser>();

			pipelineEvent.Pipeline.State.Add("queue", new MessageQueue(parser.Path)
				{
					MessageReadPropertyFilter = _messagePropertyFilter
				});

			pipelineEvent.Pipeline.State.Add("journalQueue", new MessageQueue(parser.JournalPath)
				{
					MessageReadPropertyFilter = _messagePropertyFilter
				});
		}

		public void Execute(OnReleaseMessage pipelineEvent)
		{
			var parser = pipelineEvent.Pipeline.State.Get<MsmqUriParser>();
			var tx = pipelineEvent.Pipeline.State.Get<MessageQueueTransaction>();
			var queue = pipelineEvent.Pipeline.State.Get<MessageQueue>("queue");
			var journalQueue = pipelineEvent.Pipeline.State.Get<MessageQueue>("journalQueue");
			var timeout = pipelineEvent.Pipeline.State.Get<TimeSpan>("timeout");

			try
			{
				var journalMessage = ReceiveMessage(pipelineEvent.Pipeline.State.Get<Guid>("messageId"), tx, journalQueue, timeout);

				if (journalMessage == null)
				{
					return;
				}

				var message = new Message
					{
						Recoverable = true,
						Label = journalMessage.Label,
						CorrelationId = string.Format(@"{0}\1", journalMessage.Label),
						BodyStream = journalMessage.BodyStream.Copy()
					};

				queue.Send(message, tx);
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					MsmqQueue.AccessDenied(_log, parser.Path);
				}

				_log.Error(string.Format(MsmqResources.GetMessageError, parser.Path, ex.Message));

				throw;
			}
		}

		private Message ReceiveMessage(Guid messageId, MessageQueueTransaction tx, MessageQueue journalQueue, TimeSpan timeout)
		{
			try
			{
				return journalQueue.ReceiveByCorrelationId(string.Format(@"{0}\1", messageId), timeout, tx);
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
				{
					return null;
				}

				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					MsmqQueue.AccessDenied(_log, journalQueue.Path);
				}

				_log.Error(string.Format(MsmqResources.GetMessageError, journalQueue.Path, ex.Message));

				throw;
			}
		}
	}
}