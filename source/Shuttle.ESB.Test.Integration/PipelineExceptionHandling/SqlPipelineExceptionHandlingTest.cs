using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration.Core
{
	public class SqlPipelineExceptionHandlingTest : PipelineExceptionFixture
	{
		[Test]
		public void Should_be_able_to_handle_exceptions_in_receive_stage_of_receive_pipeline()
		{
			TestExceptionHandling("sql://shuttle/{0}");
		}
	}
}