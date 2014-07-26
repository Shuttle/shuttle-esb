using System;
using System.IO;
using System.Linq;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.FileMQ
{
	public class FileQueue : IQueue, ICreateQueue, IDropQueue, IPurgeQueue
	{
		private readonly object _padlock = new object();
		private bool _journalInitialized;

		private readonly string _queueFolder;
		private readonly string _journalFolder;
		private const string Extension = ".file";
		private const string ExtensionMask = "*.file";

		public FileQueue(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			Uri = uri;

			_queueFolder = uri.LocalPath;

			if (!string.IsNullOrEmpty(uri.Host) && uri.Host.Equals("."))
			{
				_queueFolder = Path.GetFullPath(string.Concat(".", uri.LocalPath));
			}

			_journalFolder = Path.Combine(_queueFolder, "journal");

			Directory.CreateDirectory(_queueFolder);
			Directory.CreateDirectory(_journalFolder);
		}

		public Uri Uri { get; private set; }

		private void ReturnJournalMessages()
		{
			lock (_padlock)
			{
				if (_journalInitialized)
				{
					return;
				}

				foreach (var journalFile in Directory.GetFiles(_journalFolder, ExtensionMask))
				{
					var queueFile = Path.Combine(_queueFolder, Path.GetFileName(journalFile));

					File.Delete(queueFile);
					File.Move(journalFile, queueFile);
				}

				_journalInitialized = true;
			}
		}

		public bool IsEmpty()
		{
			return !Directory.GetFiles(_queueFolder, ExtensionMask).Any();
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			var buffer = new byte[8 * 1024];
			var streaming = Path.Combine(_queueFolder, string.Concat(messageId, ".stream"));
			var message = Path.Combine(_queueFolder, string.Concat(messageId, Extension));

			File.Delete(message);

			using (var source = stream.Copy())
			using (var fs = new FileStream(streaming, FileMode.Create, FileAccess.Write))
			{
				int length;
				while ((length = source.Read(buffer, 0, buffer.Length)) > 0)
				{
					fs.Write(buffer, 0, length);
				}
				fs.Flush();
			}

			File.Move(streaming, message);
		}

		public ReceivedMessage GetMessage()
		{
			if (!_journalInitialized)
			{
				ReturnJournalMessages();
			}

			lock (_padlock)
			{
				var message = Directory.GetFiles(_queueFolder, ExtensionMask).OrderBy(file => new FileInfo(file).CreationTime).FirstOrDefault();

				if (string.IsNullOrEmpty(message))
				{
					return null;
				}

				ReceivedMessage result;
				var acknowledgementToken = Path.GetFileName(message);

				using (var stream = File.OpenRead(message))
				{
					result = new ReceivedMessage(stream.Copy(), acknowledgementToken);
				}

				File.Move(message, Path.Combine(_journalFolder, acknowledgementToken));

				return result;
			}
		}

		public void Acknowledge(object acknowledgementToken)
		{
			lock (_padlock)
			{
				File.Delete(Path.Combine(_journalFolder, (string) acknowledgementToken));
			}
		}

		public void Release(object acknowledgementToken)
		{
			var fileName = (string)acknowledgementToken;
			var queueMessage = Path.Combine(_queueFolder, fileName);
			var journalMessage = Path.Combine(_journalFolder, fileName);

			if (!File.Exists(journalMessage))
			{
				return;
			}

			lock (_padlock)
			{
				File.Delete(queueMessage);
				File.Move(journalMessage, queueMessage);
				File.SetCreationTime(queueMessage, DateTime.Now);
			}
		}

		public void Create()
		{
			Directory.CreateDirectory(_queueFolder);
			Directory.CreateDirectory(_journalFolder);
		}

		public void Drop()
		{
			if (Directory.Exists(_journalFolder))
			{
				Directory.Delete(_journalFolder, true);
			}

			if (Directory.Exists(_queueFolder))
			{
				Directory.Delete(_queueFolder, true);
			}
		}

		public void Purge()
		{
			Drop();
			Create();
		}
	}
}