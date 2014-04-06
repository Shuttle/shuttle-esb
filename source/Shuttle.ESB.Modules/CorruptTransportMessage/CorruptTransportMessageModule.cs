using System;
using System.IO;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Modules
{
	public class CorruptTransportMessageModule : IModule
	{
		private string _corruptTransportMessageFolder;

		public void Initialize(IServiceBus bus)
		{
			Guard.AgainstNull(bus,"bus");

			_corruptTransportMessageFolder = ConfigurationItem<string>.ReadSetting("CorruptTransportMessageFolder").GetValue();

			bus.Events.TransportMessageDeserializationException += OnTransportMessageDeserializationException;
		}

		private void OnTransportMessageDeserializationException(object sender, DeserializationExceptionEventArgs deserializationExceptionEventArgs)
		{
			var filePath = Path.Combine(_corruptTransportMessageFolder, string.Format("{0}.stm", Guid.NewGuid().ToString()));

			using (Stream file = File.OpenWrite(filePath))
			using (var stream = deserializationExceptionEventArgs.PipelineEvent.Pipeline.State.GetTransportMessageStream().Copy())
			{
				stream.CopyTo(file);
			}
		}
	}
}