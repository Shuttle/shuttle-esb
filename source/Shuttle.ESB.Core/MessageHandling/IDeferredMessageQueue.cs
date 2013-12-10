using System;
using System.IO;

namespace Shuttle.ESB.Core
{
	public interface IDeferredMessageQueue
	{
		void Enqueue(DateTime at, Stream stream);
		Stream Dequeue(DateTime now);
	}
}