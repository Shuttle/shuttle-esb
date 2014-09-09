using System;

namespace Shuttle.ESB.Core
{
	public interface IUriResolver
	{
		Uri Get(string forUri);
	}
}