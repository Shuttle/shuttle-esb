using System;
using System.IO;

namespace Shuttle.ESB.Core
{
	public interface IDeferredMessageManager
	{
		void Register(DateTime at, Stream transportMessage);
	}
}