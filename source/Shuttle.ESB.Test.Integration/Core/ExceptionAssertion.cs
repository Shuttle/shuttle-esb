using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Test.Integration.Core
{
	internal class ExceptionAssertion
	{
		private bool hasRun;

		public ExceptionAssertion(string name, Action action)
		{
			Guard.AgainstNull(action, "action");

			Action = action;

			Name = name;
		}

		public string Name { get; private set; }
		public Action Action { get; private set; }

		public void MarkAsRun()
		{
			hasRun = true;
		}

		public bool HasRun
		{
			get 
			{
				return hasRun;
			}
		}
	}
}