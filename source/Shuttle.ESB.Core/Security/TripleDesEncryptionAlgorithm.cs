using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class TripleDesEncryptionAlgorithm : IEncryptionAlgorithm
	{
		private string key;
		private TripleDESCryptoServiceProvider _provider;

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

			_provider = new TripleDESCryptoServiceProvider
				{
					IV = new byte[8],
					Key =
						new PasswordDeriveBytes(key, new byte[0]).CryptDeriveKey("RC2", "MD5", 128, new byte[8])
				};
		}

		public string Name
		{
			get { return "3DES"; }
		}

		public byte[] Encrypt(byte[] bytes)
		{
			Guard.AgainstNull(bytes, "stream");

			return TripleDESEncrypt(bytes);
		}

		public byte[] Decrypt(byte[] bytes)
		{
			Guard.AgainstNull(bytes, "stream");

			return TripleDESDecrypt(bytes);
		}

		private byte[] TripleDESEncrypt(byte[] plain)
		{
			return GetEncryptedBytes(plain.Length, plain);
		}

		private byte[] GetEncryptedBytes(int plainLength, byte[] plainBytes)
		{
			byte[] encryptedBytes;

			using (var ms = new MemoryStream((plainLength*2) - 1))
			using (var cs = new CryptoStream(ms, _provider.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cs.Write(plainBytes, 0, plainBytes.Length);

				cs.FlushFinalBlock();

				encryptedBytes = new byte[(int) ms.Length];

				ms.Position = 0;

				ms.Read(encryptedBytes, 0, (int) ms.Length);
			}

			return encryptedBytes;
		}

		private byte[] TripleDESDecrypt(byte[] encrypted)
		{
			return GetPlainBytes(encrypted.Length, encrypted);
		}

		private byte[] GetPlainBytes(int secureLength, byte[] encryptedBytes)
		{
			byte[] plainBytes;

			using (var ms = new MemoryStream(secureLength))
			using (var cs = new CryptoStream(ms, _provider.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cs.Write(encryptedBytes, 0, encryptedBytes.Length);

				cs.FlushFinalBlock();

				plainBytes = new byte[(int) ms.Length];

				ms.Position = 0;

				ms.Read(plainBytes, 0, (int) ms.Length);
			}

			return plainBytes;
		}
	}
}