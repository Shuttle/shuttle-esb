using System.Configuration;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class TripleDesEncryptionAlgorithm : IEncryptionAlgorithm
    {
        private string key;
        private readonly ICryptographyService cryptographyService = new CryptographyService();

        public TripleDesEncryptionAlgorithm(string key)
        {
            Guard.AgainstNullOrEmptyString(key, "key");

            this.key = key;
        }

        public TripleDesEncryptionAlgorithm()
        {
            ReadConfiguration();
        }

        private void ReadConfiguration()
        {
            var section = ConfigurationManager.GetSection("tripleDES") as TripleDESSection;

            if (section == null)
            {
                throw new ConfigurationErrorsException(ESBResources.TripleDESSectionMissing);
            }

            key = section.Key;

            if (string.IsNullOrEmpty(key))
            {
                throw new ConfigurationErrorsException(ESBResources.TripleDESKeyMissing);
            }
        }

    	public string Name
    	{
    		get { return "3DES"; }
    	}

    	public byte[] Encrypt(byte[] bytes)
        {
            Guard.AgainstNull(bytes, "stream");

            return cryptographyService.TripleDESEncrypt(bytes, key);
        }

        public byte[] Decrypt(byte[] bytes)
        {
            Guard.AgainstNull(bytes, "stream");

            return cryptographyService.TripleDESDecrypt(bytes, key);
        }
    }
}