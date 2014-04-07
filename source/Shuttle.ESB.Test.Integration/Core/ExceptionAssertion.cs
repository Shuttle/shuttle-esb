namespace Shuttle.ESB.Test.Integration.Core
{
	internal class ExceptionAssertion
	{
		private bool hasRun;

		public ExceptionAssertion(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }

		public void MarkAsRun()
		{
			hasRun = true;
		}

		public bool HasRun
		{
			get { return hasRun; }
		}
	}
}