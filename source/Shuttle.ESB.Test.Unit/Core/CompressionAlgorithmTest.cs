using System;
using System.Text;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Core
{
    public class CompressionAlgorithmTest : UnitFixture
    {
        [Test]
        public void Should_be_able_to_compress_and_decompress_using_gzip()
        {
            var algorithm = new GZipCompressionAlgorithm();

            const string text = "gzip compression algortihm|gzip compression algortihm|gzip compression algortihm|gzip compression algortihm|gzip compression algortihm|gzip compression algortihm|gzip compression algortihm";

            Assert.AreEqual(text, Encoding.UTF8.GetString(algorithm.Decompress(algorithm.Compress(Encoding.UTF8.GetBytes(text)))));
        }

        [Test]
        public void Should_be_able_to_compress_and_decompress_using_deflate()
        {
            var algorithm = new DeflateCompressionAlgorithm();

            const string text = "deflate compression algortihm|deflate compression algortihm|deflate compression algortihm|deflate compression algortihm|deflate compression algortihm|deflate compression algortihm|deflate compression algortihm";

            Assert.AreEqual(text, Encoding.UTF8.GetString(algorithm.Decompress(algorithm.Compress(Encoding.UTF8.GetBytes(text)))));
        }
    }
}

