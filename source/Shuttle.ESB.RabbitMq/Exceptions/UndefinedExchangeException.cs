using System;

namespace Shuttle.ESB.RabbitMq.Exceptions
{
	public class UndefinedExchangeException : Exception
	{
		public UndefinedExchangeException(string message)
			: base(message)
		{
		}

		public UndefinedExchangeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}