namespace Shuttle.ESB.Core
{
	public class NullEncryptionAlgorithm : IEncryptionAlgorithm
	{
		public string Name
		{
			get { return "null"; }
		}

		public byte[] Encrypt(byte[] bytes)
		{
			return bytes;
		}

		public byte[] Decrypt(byte[] bytes)
		{
			return bytes;
		}
	}
}