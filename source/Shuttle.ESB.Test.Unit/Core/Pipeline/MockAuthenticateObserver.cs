using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Core
{
	public class MockAuthenticateObserver :
		IPipelineObserver<MockPipelineEvent1>, 
		IPipelineObserver<MockPipelineEvent2>,
		IPipelineObserver<MockPipelineEvent3>
	{
		private string callSequence = string.Empty;

		public void Execute(MockPipelineEvent1 pipelineEvent)
		{
			Console.WriteLine("MockAuthenticateObserver.Execute() called for event '{0}'.", pipelineEvent.Name);

			callSequence += "1";
		}

		public void Execute(MockPipelineEvent2 pipelineEvent)
		{
			Console.WriteLine("MockAuthenticateObserver.Execute() called for event '{0}'.", pipelineEvent.Name);

			callSequence += "2";
		}

		public void Execute(MockPipelineEvent3 pipelineEvent)
		{
			Console.WriteLine("MockAuthenticateObserver.Execute() called for event '{0}'.", pipelineEvent.Name);

			callSequence += "3";
		}

		public string CallSequence
		{
			get { return callSequence; }
		}
	}
}