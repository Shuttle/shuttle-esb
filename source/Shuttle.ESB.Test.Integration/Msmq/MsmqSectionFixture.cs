using Shuttle.ESB.Msmq;

namespace Shuttle.ESB.Test.Integration.Msmq
{
	public class MsmqSectionFixture
	{
		protected MsmqSection GetMsmqSection(string file)
		{
			return MsmqSection.Open(string.Format(@".\Msmq\files\{0}", file));
		}
	}
}