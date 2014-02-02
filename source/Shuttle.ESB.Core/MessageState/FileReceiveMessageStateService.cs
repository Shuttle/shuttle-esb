using System;
using System.Collections.Generic;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class FileReceiveMessageStateService : IReceiveMessageStateService, IRequireInitialization
	{
		private readonly string _stateFolder;
		private readonly ILog _log;
		private IServiceBus _bus;

		public FileReceiveMessageStateService()
		{
			_stateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "state");

			_log = Log.For(this);

			_log.Information(string.Format(ESBResources.InformationStateFolder, _stateFolder));
		}

		public bool HasMessageBeenHandled(TransportMessage transportMessage)
		{
			return new TransportMessageStateFile(_stateFolder, transportMessage, _bus.Configuration.Serializer).HasMessageBeenHandled;
		}

		public void HandleMessage(TransportMessage transportMessage)
		{
			var file = new TransportMessageStateFile(_stateFolder, transportMessage, _bus.Configuration.Serializer);

			using (file.BeginTransaction())
			{
				file.RemoveSendRegistrations();

				file.SaveJournal();

				file.CommitTransaction();
			}
		}

		public void AcknowledgeMessage(TransportMessage transportMessage)
		{
			var file = new TransportMessageStateFile(_stateFolder, transportMessage, _bus.Configuration.Serializer);

			using (file.BeginTransaction())
			{
				file.AcknowledgeMessage();

				file.SaveJournal();

				file.CommitTransaction();
			}
		}

		public IEnumerable<TransportMessage> GetMessagesToSend(TransportMessage transportMessage)
		{
			return new TransportMessageStateFile(_stateFolder, transportMessage, _bus.Configuration.Serializer).MessagesToSend;
		}

		public void RegisterMessageToSend(TransportMessage transportMessage, TransportMessage transportMessageToSend)
		{
			var file = new TransportMessageStateFile(_stateFolder, transportMessage, _bus.Configuration.Serializer);

			using (file.BeginTransaction())
			{
				file.RegisterMessageToSend(transportMessageToSend);

				file.SaveJournal();

				file.CommitTransaction();
			}
		}

		public void RegisterSent(TransportMessage transportMessage, TransportMessage transportMessageSent)
		{
			var file = new TransportMessageStateFile(_stateFolder, transportMessage, _bus.Configuration.Serializer);

			using (file.BeginTransaction())
			{
				file.MessageSent(transportMessageSent);

				file.SaveJournal();

				file.CommitTransaction();
			}
		}

		public void Initialize(IServiceBus bus)
		{
			_bus = bus;
		}
	}
}