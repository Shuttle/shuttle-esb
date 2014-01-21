using System;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.SqlServer
{
	public class DeferredMessageQueue :
		IDeferredMessageQueue,
		IRequireInitialization,
		IPurge,
		ICount
	{
		private static readonly DataSource DeferredMessageDataSource = new DataSource("DeferredMessage", new SqlDbDataParameterFactory());

		private readonly IDatabaseGateway _databaseGateway;
		private readonly IDatabaseConnectionFactory _databaseConnectionFactory;
		private readonly IScriptProvider _scriptProvider;

		private static readonly object _padlock = new object();

		private string _dequeueQueryStatement;
		private string _enqueueQueryStatement;

		private IQuery _countQuery;
		private IQuery _purgeQuery;


		private readonly ILog _log;

		public static IDeferredMessageQueue Default()
		{
			return
				new DeferredMessageQueue(new ScriptProvider(),
				                        DatabaseConnectionFactory.Default(),
				                        DatabaseGateway.Default());
		}


		public DeferredMessageQueue(IScriptProvider scriptProvider, IDatabaseConnectionFactory databaseConnectionFactory,
		                            IDatabaseGateway databaseGateway)
		{
			Guard.AgainstNull(scriptProvider, "scriptProvider");
			Guard.AgainstNull(databaseConnectionFactory, "databaseConnectionFactory");
			Guard.AgainstNull(databaseGateway, "databaseGateway");

			_scriptProvider = scriptProvider;
			_databaseConnectionFactory = databaseConnectionFactory;
			_databaseGateway = databaseGateway;

			_log = Log.For(this);

			BuildQueries();
		}

		private void BuildQueries()
		{
			_dequeueQueryStatement = _scriptProvider.GetScript(Script.DeferredMessageDequeue);
			_enqueueQueryStatement = _scriptProvider.GetScript(Script.DeferredMessageEnqueue);
			_purgeQuery = RawQuery.Create(_scriptProvider.GetScript(Script.DeferredMessagePurge));
			_countQuery = RawQuery.Create(_scriptProvider.GetScript(Script.DeferredMessageCount));
		}

		public void Enqueue(DateTime at, Stream stream)
		{
			try
			{
				using (_databaseConnectionFactory.Create(DeferredMessageDataSource))
				{
					_databaseGateway.ExecuteUsing(
						DeferredMessageDataSource,
						RawQuery.Create(_enqueueQueryStatement)
						        .AddParameterValue(DeferredMessageColumns.DeferTillDate, at)
						        .AddParameterValue(DeferredMessageColumns.MessageBody, stream.ToBytes()));
				}
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(SqlResources.DeferredMessageEnqueueError, ex.Message, _enqueueQueryStatement));

				throw;
			}
		}

		public Stream Dequeue(DateTime now)
		{
			lock (_padlock)
			{
				try
				{
					using (_databaseConnectionFactory.Create(DeferredMessageDataSource))
					{
						var row = _databaseGateway.GetSingleRowUsing(
							DeferredMessageDataSource,
							RawQuery.Create(_dequeueQueryStatement)
							        .AddParameterValue(DeferredMessageColumns.DeferTillDate, now));

						return row == null ? null : new MemoryStream((byte[]) row["MessageBody"]);
					}
				}
				catch (Exception ex)
				{
					_log.Error(string.Format(SqlResources.DeferredMessageDequeueError, ex.Message, _dequeueQueryStatement));

					throw;
				}
			}
		}

		public void Initialize(IServiceBus bus)
		{
			using (_databaseConnectionFactory.Create(DeferredMessageDataSource))
			{
				if (_databaseGateway.GetScalarUsing<int>(
					DeferredMessageDataSource,
					RawQuery.Create(
						_scriptProvider.GetScript(
							Script.DeferredMessageExists))) != 1)
				{
					throw new DeferredMessageException(SqlResources.DeferredMessageDatabaseNotConfigured);
				}
			}
		}

		public void Purge()
		{
			try
			{
				using (_databaseConnectionFactory.Create(DeferredMessageDataSource))
				{
					_databaseGateway.ExecuteUsing(DeferredMessageDataSource, _purgeQuery);
				}
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(SqlResources.DeferredMessagePurgeError, ex.Message, _purgeQuery));

				throw;
			}
		}

		public int Count
		{
			get
			{
				try
				{
					using (_databaseConnectionFactory.Create(DeferredMessageDataSource))
					{
						return _databaseGateway.GetScalarUsing<int>(DeferredMessageDataSource, _countQuery);
					}
				}
				catch (Exception ex)
				{
					_log.Error(string.Format(SqlResources.DeferredMessageCountError, ex.Message, _countQuery));

					return 0;
				}
			}
		}
	}
}