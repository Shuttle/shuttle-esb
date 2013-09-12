namespace Shuttle.ESB.Core
{
	public class NullCompressionAlgorithm : ICompressionAlgorithm
	{
		public string Name
		{
			get { return "null"; }
		}

		public byte[] Compress(byte[] bytes)
		{
			return bytes;
		}

		public byte[] Decompress(byte[] bytes)
		{
			return bytes;
		}
	}
}