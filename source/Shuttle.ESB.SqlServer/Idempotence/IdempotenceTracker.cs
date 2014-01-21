using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.SqlServer.Idempotence
{
    public class IdempotenceTracker :
        IIdempotenceTracker,
        IRequireInitialization
    {
        private static readonly DataSource IdempotenceDataSource = new DataSource("Idempotence", new SqlDbDataParameterFactory());

        private readonly IDatabaseGateway databaseGateway;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;
        private readonly IScriptProvider scriptProvider;

        public static IIdempotenceTracker Default()
        {
            return
                new IdempotenceTracker(new ScriptProvider(),
                                       DatabaseConnectionFactory.Default(),
                                       DatabaseGateway.Default());
        }


        public IdempotenceTracker(
            IScriptProvider scriptProvider,
            IDatabaseConnectionFactory databaseConnectionFactory,
            IDatabaseGateway databaseGateway)
        {
            Guard.AgainstNull(scriptProvider, "scriptProvider");
            Guard.AgainstNull(databaseConnectionFactory, "databaseConnectionFactory");
            Guard.AgainstNull(databaseGateway, "databaseGateway");

            this.scriptProvider = scriptProvider;
            this.databaseConnectionFactory = databaseConnectionFactory;
            this.databaseGateway = databaseGateway;
        }

        public bool Contains(TransportMessage transportMessage)
        {
            using (databaseConnectionFactory.Create(IdempotenceDataSource))
            {
                return databaseGateway.GetScalarUsing<int>(
                    IdempotenceDataSource,
                    RawQuery.Create(
                        scriptProvider.GetScript(
                            Script.IdempotenceTrackerContains))
                            .AddParameterValue(IdempotenceTrackerColumns.MessageId, transportMessage.MessageId)) == 1;
            }
        }

        public void Add(TransportMessage transportMessage)
        {
            using (databaseConnectionFactory.Create(IdempotenceDataSource))
            {
                databaseGateway.ExecuteUsing(
                    IdempotenceDataSource,
                    RawQuery.Create(
                        scriptProvider.GetScript(Script.IdempotenceTrackerAdd))
                            .AddParameterValue(IdempotenceTrackerColumns.MessageId, transportMessage.MessageId));
            }
        }

        public void Remove(TransportMessage transportMessage)
        {
            using (databaseConnectionFactory.Create(IdempotenceDataSource))
            {
                databaseGateway.ExecuteUsing(
                    IdempotenceDataSource,
                    RawQuery.Create(
                        scriptProvider.GetScript(Script.IdempotenceTrackerRemove))
                            .AddParameterValue(IdempotenceTrackerColumns.MessageId, transportMessage.MessageId));
            }
        }

        public void Initialize(IServiceBus bus)
        {
            using (databaseConnectionFactory.Create(IdempotenceDataSource))
            {
                if (databaseGateway.GetScalarUsing<int>(
                    IdempotenceDataSource,
                    RawQuery.Create(
                        scriptProvider.GetScript(
                            Script.IdempotenceTrackerExists))) != 1)
                {
                    throw new IdempotenceTrackerException(SqlResources.IdempotenceTrackerDatabaseNotConfigured);
                }
            }
        }
    }
}