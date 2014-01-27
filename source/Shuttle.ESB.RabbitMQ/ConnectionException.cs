using System;

namespace Shuttle.ESB.RabbitMQ
{
	internal class ConnectionException : Exception
	{
		public ConnectionException(string message) : base(message)
		{
		}
	}
}