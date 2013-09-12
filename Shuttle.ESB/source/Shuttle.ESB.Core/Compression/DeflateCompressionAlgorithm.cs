using System.IO;
using System.IO.Compression;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeflateCompressionAlgorithm : ICompressionAlgorithm
	{
		public string Name
		{
			get { return "Deflate"; }
		}

        public byte[] Compress(byte[] bytes)
        {
            Guard.AgainstNull(bytes, "stream");

            using (var compressed = new MemoryStream())
            {
                using (var gzip = new DeflateStream(compressed, CompressionMode.Compress, true))
                {
                    gzip.Write(bytes, 0, bytes.Length);
                }

                return compressed.ToArray();
            }
        }

        public byte[] Decompress(byte[] bytes)
        {
            Guard.AgainstNull(bytes, "stream");

            using (var gzip = new DeflateStream(new MemoryStream(bytes), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var decompressed = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = gzip.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            decompressed.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return decompressed.ToArray();
                }
            }
        }

	}
}