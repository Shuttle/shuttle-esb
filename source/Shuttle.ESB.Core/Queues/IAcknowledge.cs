using System;

namespace Shuttle.ESB.Core
{
	public interface IAcknowledge
	{
		void Acknowledge(Guid messageId);
	}
}