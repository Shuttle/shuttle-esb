using System.Collections;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ModuleCollection : IEnumerable<IModule>
	{
		private readonly List<IModule> modules = new List<IModule>();

		public IEnumerator<IModule> GetEnumerator()
		{
			return modules.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(IModule module)
		{
			Guard.AgainstNull(module, "module");

			modules.Add(module);
		}
	}
}