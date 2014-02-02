using System.Collections.Generic;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class TransportMessageStateFile
	{
		private readonly ISerializer _serializer;
		private readonly string _stateFilePath;
		private readonly string _stateJournalFilePath;
		private readonly string _stateCompleteFilePath;
		private readonly TransportMessageState _state;

		public TransportMessageStateFile(string stateFolder, TransportMessage transportMessage, ISerializer serializer)
		{
			Guard.AgainstNullOrEmptyString(stateFolder, "stateFolder");
			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(transportMessage.MessageId, "transportMessage.MessageId");
			Guard.AgainstNull(serializer, "serializer");

			_serializer = serializer;

			_stateFilePath = StateFilePath(stateFolder, transportMessage);
			_stateJournalFilePath = Path.Combine(stateFolder, string.Format("{0}.journal", transportMessage.MessageId));
			_stateCompleteFilePath = Path.Combine(stateFolder, string.Format("{0}.complete", transportMessage.MessageId));

			ProcessTransaction();

			if (Exists())
			{
				using (var stream = new MemoryStream(File.ReadAllBytes(_stateFilePath)))
				{
					_state = (TransportMessageState) serializer.Deserialize(typeof (TransportMessageState), stream);
				}
			}
			else
			{
				_state = new TransportMessageState();
			}
		}

		public static string StateFilePath(string stateFolder, TransportMessage transportMessage)
		{
			Guard.AgainstNullOrEmptyString(stateFolder, "stateFolder");
			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(transportMessage.MessageId, "transportMessage.MessageId");

			return Path.Combine(stateFolder, string.Format("{0}.state", transportMessage.MessageId));
		}

		public TransportMessageStateFileTransaction BeginTransaction()
		{
			return new TransportMessageStateFileTransaction(this);
		}

		public void RollbackTransaction()
		{
			if (!File.Exists(_stateCompleteFilePath))
			{
				return;
			}

			File.Delete(_stateCompleteFilePath);
			ProcessTransaction();
		}

		public void CommitTransaction()
		{
			if (File.Exists(_stateCompleteFilePath))
			{
				return;
			}

			File.WriteAllText(_stateCompleteFilePath, string.Empty);
			ProcessTransaction();
		}

		public void ProcessTransaction()
		{
			if (HasComplete())
			{
				File.Copy(_stateJournalFilePath, _stateFilePath, true);
				File.Delete(_stateJournalFilePath);
				File.Delete(_stateCompleteFilePath);
			}
			else
			{
				if (File.Exists(_stateJournalFilePath))
				{
					File.Delete(_stateJournalFilePath);
				}
			}
		}

		public bool Exists()
		{
			return File.Exists(_stateFilePath);
		}

		public bool HasComplete()
		{
			return File.Exists(_stateCompleteFilePath);
		}

		public void SaveJournal()
		{
			using (var stream = _serializer.Serialize(_state))
			{
				File.WriteAllBytes(_stateJournalFilePath, stream.ToBytes());
			}
		}

		public bool HasMessageBeenHandled
		{
			get { return _state.HasMessageBeenHandled; }
		}

		public IEnumerable<TransportMessage> MessagesToSend
		{
			get { return _state.MessagesToSend; }
		}

		public void AcknowledgeMessage()
		{
			_state.HasMessageBeenHandled = true;
		}

		public void RemoveSendRegistrations()
		{
			_state.MessagesToSend.Clear();
		}

		public void RegisterMessageToSend(TransportMessage transportMessageToSend)
		{
			_state.MessagesToSend.Add(transportMessageToSend);
		}

		public void MessageSent(TransportMessage transportMessageSent)
		{
			_state.MessagesToSend.RemoveAll(message => message.MessageId.Equals(transportMessageSent.MessageId));
		}
	}
}