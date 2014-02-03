using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
	[Serializable]
	public class TransportMessage
	{
		public TransportMessage()
		{
			MessageId = Guid.NewGuid();
			SendDate = DateTime.MinValue;
			IgnoreTillDate = DateTime.MinValue;

			FailureMessages = new List<string>();
			Headers = new List<TransportHeader>();
		}

		public byte[] Message { get; set; }

		public Guid MessageReceivedId { get; set; }
		public Guid MessageId { get; set; }
		public string CorrelationId { get; set; }
		public string SenderInboxWorkQueueUri { get; set; }
		public string RecipientInboxWorkQueueUri { get; set; }
		public string PrincipalIdentityName { get; set; }
		public DateTime IgnoreTillDate { get; set; }
		public DateTime SendDate { get; set; }
		public List<string> FailureMessages { get; set; }
		public string MessageType { get; set; }
		public string AssemblyQualifiedName { get; set; }
		public string EncryptionAlgorithm { get; set; }
		public string CompressionAlgorithm { get; set; }
		public List<TransportHeader> Headers { get; set; }
	}
}