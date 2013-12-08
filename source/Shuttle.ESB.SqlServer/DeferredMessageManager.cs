using System;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.SqlServer
{
	public class DeferredMessageManager : 
		IDeferredMessageManager,
		IRequireInitialization
	{
		private static readonly DataSource DeferredMessageDataSource = new DataSource("DeferredMessage",
																				   new SqlServerDbDataParameterFactory(),
																				   new SqlServerContainsQueryFactory(),
																				   new SqlServerInsertQueryFactory(),
																				   new SqlServerUpdateQueryFactory(),
																				   new SqlServerDeleteQueryFactory(),
																				   new SqlServerSelectQueryFactory());
		private readonly IDatabaseGateway databaseGateway;
		private readonly IDatabaseConnectionFactory databaseConnectionFactory;
		private readonly IScriptProvider scriptProvider;

        public static ISubscriptionManager Default()
        {
            return
                new SubscriptionManager(new ScriptProvider(),
                                        DatabaseConnectionFactory.Default(),
                                        DatabaseGateway.Default());
        }


        public DeferredMessageManager(IScriptProvider scriptProvider, IDatabaseConnectionFactory databaseConnectionFactory,
                                   IDatabaseGateway databaseGateway)
        {
            Guard.AgainstNull(scriptProvider, "scriptProvider");
            Guard.AgainstNull(databaseConnectionFactory, "databaseConnectionFactory");
            Guard.AgainstNull(databaseGateway, "databaseGateway");

            this.scriptProvider = scriptProvider;
            this.databaseConnectionFactory = databaseConnectionFactory;
            this.databaseGateway = databaseGateway;
        }

		public void Register(DateTime at, Stream transportMessage)
		{
			using (databaseConnectionFactory.Create(DeferredMessageDataSource))
			{
					databaseGateway.ExecuteUsing(
						DeferredMessageDataSource,
						RawQuery.CreateFrom(
							scriptProvider.GetScript(Script.DeferredMessageManagerRegister))
								.AddParameterValue(DeferredMessageManagerColumns.DeferTillDate, at)
								.AddParameterValue(DeferredMessageManagerColumns.TransportMessage, transportMessage.ToBytes()));
			}
		}

		public void Initialize(IServiceBus bus)
		{
			using (databaseConnectionFactory.Create(DeferredMessageDataSource))
			{
				if (databaseGateway.GetScalarUsing<int>(
					DeferredMessageDataSource,
					RawQuery.CreateFrom(
						scriptProvider.GetScript(
							Script.DeferredMessageManagerExists))) != 1)
				{
					throw new DeferredMessageManagerException(SqlResources.DeferredMessageManagerDatabaseNotConfigured);
				}
			}
		}
	}
}