using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.SqlServer
{
	public class SqlQueue : IQueue, ICreate, IDrop, IPurge, ICount, IQueueReader
	{
		private class UnacknowledgedMessage
		{
			public UnacknowledgedMessage(Guid messageId, int sequenceId)
			{
				SequenceId = sequenceId;
				MessageId = messageId;
			}

			public int SequenceId { get; private set; }
			public Guid MessageId { get; private set; }
		}

		private readonly DataSource _dataSource;
		private readonly IDatabaseGateway _databaseGateway;

		private readonly object _padlock = new object();
		private readonly List<UnacknowledgedMessage> _unacknowledgedMessages = new List<UnacknowledgedMessage>();

		private readonly List<Guid> _emptyMessageIds = new List<Guid>
			{
				Guid.Empty
			};

		private readonly IScriptProvider _scriptProvider;

		private readonly string _tableName;
		private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

		private IQuery _countQuery;
		private IQuery _createQuery;
		private IQuery _dropQuery;
		private IQuery _existsQuery;
		private IQuery _purgeQuery;

		private string _removeQueryStatement;
		private string _enqueueQueryStatement;
		private string _dequeueIdQueryStatement;

		private readonly ILog _log;

		private readonly SqlUriParser parser;

		public SqlQueue(Uri uri)
			: this(uri,
			       new ScriptProvider(),
			       DatabaseConnectionFactory.Default(),
			       DatabaseGateway.Default())
		{
		}

		public SqlQueue(Uri uri,
		                IScriptProvider scriptProvider,
		                IDatabaseConnectionFactory databaseConnectionFactory,
		                IDatabaseGateway databaseGateway)
		{
			Guard.AgainstNull(uri, "uri");
			Guard.AgainstNull(scriptProvider, "scriptProvider");
			Guard.AgainstNull(databaseConnectionFactory, "databaseConnectionFactory");
			Guard.AgainstNull(databaseGateway, "databaseGateway");

			_scriptProvider = scriptProvider;
			_databaseConnectionFactory = databaseConnectionFactory;
			_databaseGateway = databaseGateway;

			_log = Log.For(this);

			Uri = uri;

			parser = new SqlUriParser(uri);

			_dataSource = new DataSource(parser.ConnectionName, new SqlDbDataParameterFactory());
			_tableName = parser.TableName;

			BuildQueries();
		}

		public int Count
		{
			get
			{
				try
				{
					using (_databaseConnectionFactory.Create(_dataSource))
					{
						return _databaseGateway.GetScalarUsing<int>(_dataSource, _countQuery);
					}
				}
				catch (Exception ex)
				{
					_log.Error(string.Format(SqlResources.CountError, Uri, ex.Message, _countQuery));

					return 0;
				}
			}
		}

		public void Create()
		{
			if (Exists())
			{
				return;
			}

			try
			{
				using (_databaseConnectionFactory.Create(_dataSource))
				{
					_databaseGateway.ExecuteUsing(_dataSource, _createQuery);
				}
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(SqlResources.CreateError, Uri, ex.Message, _createQuery));

				throw;
			}
		}

		public void Drop()
		{
			if (!Exists())
			{
				return;
			}

			try
			{
				lock (_padlock)
				{
					using (_databaseConnectionFactory.Create(_dataSource))
					{
						_databaseGateway.ExecuteUsing(_dataSource, _dropQuery);
					}

					ResetUnacknowledgedMessageIds();
				}
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(SqlResources.DropError, Uri, ex.Message, _dropQuery));

				throw;
			}
		}

		public void Purge()
		{
			if (!Exists())
			{
				return;
			}

			try
			{
				lock (_padlock)
				{
					using (_databaseConnectionFactory.Create(_dataSource))
					{
						_databaseGateway.ExecuteUsing(_dataSource, _purgeQuery);
					}

					ResetUnacknowledgedMessageIds();
				}
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(SqlResources.PurgeError, Uri, ex.Message, _purgeQuery));

				throw;
			}
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			try
			{
				using (_databaseConnectionFactory.Create(_dataSource))
				{
					_databaseGateway.ExecuteUsing(
						_dataSource,
						RawQuery.Create(_enqueueQueryStatement)
						        .AddParameterValue(QueueColumns.MessageId, messageId)
						        .AddParameterValue(QueueColumns.MessageBody, stream.ToBytes()));
				}
			}
			catch (Exception ex)
			{
				_log.Error(
					string.Format(SqlResources.EnqueueError, messageId, Uri, ex.Message));

				throw;
			}
		}

		public Uri Uri { get; private set; }

		private bool Exists()
		{
			try
			{
				using (_databaseConnectionFactory.Create(_dataSource))
				{
					return _databaseGateway.GetScalarUsing<int>(_dataSource, _existsQuery) == 1;
				}
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(SqlResources.ExistsError, Uri, ex.Message, _existsQuery));

				throw;
			}
		}

		public bool IsEmpty()
		{
			return Count == 0;
		}

		public Stream Dequeue()
		{
			lock (_padlock)
			{
				try
				{
					using (_databaseConnectionFactory.Create(_dataSource))
					{
						var messageIds = _unacknowledgedMessages.Count > 0
							                 ? _unacknowledgedMessages.Select(unacknowledgedMessage => unacknowledgedMessage.MessageId)
							                 : _emptyMessageIds;

						var row = _databaseGateway.GetSingleRowUsing(
							_dataSource,
							RawQuery.Create(
								_scriptProvider.GetScript(
									Script.QueueDequeue,
									_tableName,
									string.Join(",", messageIds.Select(messageId => string.Format("'{0}'", messageId)).ToArray()))));

						if (row == null)
						{
							return null;
						}

						var result = new MemoryStream((byte[]) row["MessageBody"]);

						MessageIdAcknowledgementRequired((int) row["SequenceId"], new Guid(row["MessageId"].ToString()));

						return result;
					}
				}
				catch (Exception ex)
				{
					_log.Error(string.Format(SqlResources.DequeueError, Uri, ex.Message, _createQuery));

					throw;
				}
			}
		}

		private void MessageIdAcknowledgementRequired(int sequenceId, Guid messageId)
		{
			_unacknowledgedMessages.Add(new UnacknowledgedMessage(messageId, sequenceId));
		}

		public Stream Dequeue(Guid messageId)
		{
			lock (_padlock)
			{
				try
				{
					using (_databaseConnectionFactory.Create(_dataSource))
					{
						var row = _databaseGateway.GetSingleRowUsing(
							_dataSource,
							RawQuery.Create(_dequeueIdQueryStatement).AddParameterValue(QueueColumns.MessageId, messageId));

						if (row == null)
						{
							return null;
						}

						var result = new MemoryStream((byte[]) row["MessageBody"]);

						MessageIdAcknowledgementRequired((int) row["SequenceId"], messageId);

						return result;
					}
				}
				catch (Exception ex)
				{
					_log.Error(string.Format(SqlResources.DequeueIdError, Uri, ex.Message, _dequeueIdQueryStatement));

					throw;
				}
			}
		}

		private void BuildQueries()
		{
			_existsQuery = RawQuery.Create(_scriptProvider.GetScript(Script.QueueExists, _tableName));
			_createQuery = RawQuery.Create(_scriptProvider.GetScript(Script.QueueCreate, _tableName));
			_dropQuery = RawQuery.Create(_scriptProvider.GetScript(Script.QueueDrop, _tableName));
			_purgeQuery = RawQuery.Create(_scriptProvider.GetScript(Script.QueuePurge, _tableName));
			_countQuery = RawQuery.Create(_scriptProvider.GetScript(Script.QueueCount, _tableName));
			_enqueueQueryStatement = _scriptProvider.GetScript(Script.QueueEnqueue, _tableName);
			_removeQueryStatement = _scriptProvider.GetScript(Script.QueueRemove, _tableName);
			_dequeueIdQueryStatement = _scriptProvider.GetScript(Script.QueueDequeueId, _tableName);
		}

		public IEnumerable<Stream> Read(int top)
		{
			try
			{
				var result = new List<Stream>();

				using (_databaseConnectionFactory.Create(_dataSource))
				{
					using (var reader = _databaseGateway.GetReaderUsing(
						_dataSource,
						RawQuery.Create(_scriptProvider.GetScript(Script.QueueRead, top.ToString(CultureInfo.InvariantCulture), _tableName)))
						)
					{
						while (reader.Read())
						{
							result.Add(new MemoryStream((byte[]) reader["MessageBody"]));
						}
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(SqlResources.ReadError, top, Uri, ex.Message, _createQuery));

				throw;
			}
		}

		public void Acknowledge(Guid messageId)
		{
			lock (_padlock)
			{
				try
				{
					lock (_padlock)
					{
						using (_databaseConnectionFactory.Create(_dataSource))
						{
							_databaseGateway.ExecuteUsing(_dataSource,
							                              RawQuery.Create(_removeQueryStatement)
							                                      .AddParameterValue(QueueColumns.SequenceId, SequenceId(messageId)));

							MessageIdAcknowledged(messageId);
						}
					}
				}
				catch (Exception ex)
				{
					_log.Error(string.Format(SqlResources.RemoveError, Uri, ex.Message, _removeQueryStatement));

					throw;
				}
			}
		}

		private void MessageIdAcknowledged(Guid messageId)
		{
			_unacknowledgedMessages.RemoveAll(unacknowledgedMessage => unacknowledgedMessage.MessageId.Equals(messageId));
		}

		private int SequenceId(Guid messageId)
		{
			var unacknowledgedMessage = _unacknowledgedMessages.Find(candidate => candidate.MessageId.Equals(messageId));

			return unacknowledgedMessage != null
				       ? unacknowledgedMessage.SequenceId
				       : 0;
		}

		private void ResetUnacknowledgedMessageIds()
		{
			_unacknowledgedMessages.Clear();
		}
	}
}