using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class TransportMessageExtensions
	{
		public static bool EncryptionEnabled(this TransportMessage transportMessage)
		{
			return !string.IsNullOrEmpty(transportMessage.EncryptionAlgorithm)
				   &&
				   !transportMessage.EncryptionAlgorithm.Equals("none", StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool CompressionEnabled(this TransportMessage transportMessage)
		{
			return !string.IsNullOrEmpty(transportMessage.CompressionAlgorithm)
				   &&
				   !transportMessage.CompressionAlgorithm.Equals("none", StringComparison.InvariantCultureIgnoreCase);
		}

		public static void RegisterFailure(this TransportMessage transportMessage, string message)
		{
			transportMessage.RegisterFailure(message, TimeSpan.FromMilliseconds(0));
		}

		public static void RegisterFailure(this TransportMessage transportMessage, string message, TimeSpan timeSpanToIgnore)
		{
			Guard.AgainstNullOrEmptyString(message, "message");

			transportMessage.FailureMessages.Add(string.Format("[{0}] : {1}", DateTime.Now.ToString(ESBResources.FormatDateFull), message));
			transportMessage.IgnoreTillDate = DateTime.Now.Add(timeSpanToIgnore);
		}

		public static bool IsIgnoring(this TransportMessage transportMessage)
		{
			return DateTime.Now <= transportMessage.IgnoreTillDate;
		}

		public static void StopIgnoring(this TransportMessage transportMessage)
		{
			transportMessage.IgnoreTillDate = DateTime.MinValue;
		}

		public static TransportMessage SetMessage(this TransportMessage transportMessage, ISerializer serializer,
												  object message)
		{
			transportMessage.Message = serializer.Serialize(message).ToBytes();
			transportMessage.MessageType = message.GetType().FullName;
			transportMessage.AssemblyQualifiedName = message.GetType().AssemblyQualifiedName;

			return transportMessage;
		}

		public static bool HasSenderInboxWorkQueueUri(this TransportMessage transportMessage)
		{
			return !string.IsNullOrEmpty(transportMessage.SenderInboxWorkQueueUri);
		}

		public static void AcceptInvariants(this TransportMessage transportMessage)
		{
			Guard.AgainstNull(transportMessage.MessageId, "MessageId");
			Guard.AgainstNullOrEmptyString(transportMessage.PrincipalIdentityName, "PrincipalIdentityName");
			Guard.AgainstNullOrEmptyString(transportMessage.MessageType, "MessageType");
			Guard.AgainstNullOrEmptyString(transportMessage.AssemblyQualifiedName, "AssemblyQualifiedName");
		}

		public static void Merge(this List<TransportHeader> merge, IEnumerable<TransportHeader> headers)
		{
			Guard.AgainstNull(headers, "headers");

			foreach (var header in headers.Where(header => !merge.Contains(header.Key)))
			{
				merge.Add(new TransportHeader
							{
								Key = header.Key,
								Value = header.Value
							});
			}
		}

		public static string GetHeaderValue(this List<TransportHeader> headers, string key)
		{
			if (headers == null)
			{
				return string.Empty;
			}

			var header =
				headers.FirstOrDefault(candidate => candidate.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

			return header == null ? string.Empty : header.Value;
		}

		public static void SetHeaderValue(this List<TransportHeader> headers, string key, string value)
		{
			Guard.AgainstNull(headers, "headers");

			var header =
				headers.FirstOrDefault(candidate => candidate.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

			if (header != null)
			{
				header.Value = value;
			}
			else
			{
				headers.Add(new TransportHeader
								{
									Key = key,
									Value = value
								});
			}
		}

		public static bool Contains(this IEnumerable<TransportHeader> headers, string key)
		{
			Guard.AgainstNull(headers, "headers");

			return headers.Any(header => header.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}