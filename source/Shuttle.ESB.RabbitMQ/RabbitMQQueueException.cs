using System;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueueException : Exception
	{
		public RabbitMQQueueException(string message) : base(message)
		{
		}
	}
}