using System;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.SqlServer
{
	public class DeferredMessageQueue :
		IDeferredMessageQueue,
		IRequireInitialization
	{
		private static readonly DataSource DeferredMessageDataSource = new DataSource("DeferredMessage",
		                                                                              new SqlServerDbDataParameterFactory(),
		                                                                              new SqlServerContainsQueryFactory(),
		                                                                              new SqlServerInsertQueryFactory(),
		                                                                              new SqlServerUpdateQueryFactory(),
		                                                                              new SqlServerDeleteQueryFactory(),
		                                                                              new SqlServerSelectQueryFactory());

		private readonly IDatabaseGateway _databaseGateway;
		private readonly IDatabaseConnectionFactory _databaseConnectionFactory;
		private readonly IScriptProvider _scriptProvider;

		private static readonly object _padlock = new object();

		private string _dequeueQueryStatement;
		private string _enqueueQueryStatement;

		private readonly ILog _log;

		public static ISubscriptionManager Default()
		{
			return
				new SubscriptionManager(new ScriptProvider(),
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
			_dequeueQueryStatement = _scriptProvider.GetScript(Script.DeferredMessageQueueDequeue);
			_enqueueQueryStatement = _scriptProvider.GetScript(Script.DeferredMessageQueueEnqueue);
		}

		public void Enqueue(DateTime at, Stream stream)
		{
			try
			{
				using (_databaseConnectionFactory.Create(DeferredMessageDataSource))
				{
					_databaseGateway.ExecuteUsing(
						DeferredMessageDataSource,
						RawQuery.CreateFrom(_enqueueQueryStatement)
						        .AddParameterValue(DeferredMessageManagerColumns.DeferTillDate, at)
						        .AddParameterValue(DeferredMessageManagerColumns.MessageBody, stream.ToBytes()));
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
					var row = _databaseGateway.GetSingleRowUsing(
						DeferredMessageDataSource,
						RawQuery.CreateFrom(_dequeueQueryStatement)
						        .AddParameterValue(DeferredMessageManagerColumns.DeferTillDate, now));

					return row == null ? null : new MemoryStream((byte[]) row["MessageBody"]);
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
					RawQuery.CreateFrom(
						_scriptProvider.GetScript(
							Script.DeferredMessageManagerExists))) != 1)
				{
					throw new DeferredMessageManagerException(SqlResources.DeferredMessageDatabaseNotConfigured);
				}
			}
		}
	}
}