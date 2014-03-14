using System;
using System.Collections.Generic;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.MessageHandling;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Integration.Idempotence
{
	internal class IdempotenceMessageHandlerFactory : IMessageHandlerFactory
	{
		private readonly Type _type = typeof(IdempotenceCommand);
		private readonly List<Type> _messageTypesHandled = new List<Type>();
		private readonly IdempotenceCounter _counter = new IdempotenceCounter();

		public void Initialize(IServiceBus bus)
		{
			_messageTypesHandled.Add(_type);
		}

		public IMessageHandler GetHandler(object message)
		{
			if (message.GetType() == _type)
			{
				return new IdempotenceHandler(_counter);
			}

			throw new Exception("Can only handle type of 'IdempotenceCommand'.");
		}

		public void ReleaseHandler(IMessageHandler handler)
		{
		}

		public IEnumerable<Type> MessageTypesHandled {
			get { return _messageTypesHandled; }
		}

		public int ProcessedCount
		{
			get { return _counter.ProcessedCount; }
		}
	}
}