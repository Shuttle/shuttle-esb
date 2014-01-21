using System.Collections.Generic;
using System.Data;
using System.Linq;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.SqlServer
{
    public class SubscriptionManager :
        ISubscriptionManager,
        IRequireInitialization
    {
        private static readonly DataSource SubscriptionDataSource = new DataSource("Subscription", new SqlDbDataParameterFactory());

        private readonly List<string> deferredSubscriptions = new List<string>();

        private readonly IDatabaseGateway databaseGateway;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;
        private readonly IScriptProvider scriptProvider;

        private IServiceBusConfiguration serviceBusConfiguration;

        private readonly Dictionary<string, List<string>> subscribers = new Dictionary<string, List<string>>();

        private static readonly object padlock = new object();

        public static ISubscriptionManager Default()
        {
            return
                new SubscriptionManager(new ScriptProvider(),
                                        DatabaseConnectionFactory.Default(),
                                        DatabaseGateway.Default());
        }


        public SubscriptionManager(IScriptProvider scriptProvider, IDatabaseConnectionFactory databaseConnectionFactory,
                                   IDatabaseGateway databaseGateway)
        {
            Guard.AgainstNull(scriptProvider, "scriptProvider");
            Guard.AgainstNull(databaseConnectionFactory, "databaseConnectionFactory");
            Guard.AgainstNull(databaseGateway, "databaseGateway");

            this.scriptProvider = scriptProvider;
            this.databaseConnectionFactory = databaseConnectionFactory;
            this.databaseGateway = databaseGateway;
        }

        protected bool HasDeferredSubscriptions
        {
            get { return deferredSubscriptions.Count > 0; }
        }

        protected bool Started
        {
            get { return serviceBusConfiguration != null; }
        }

        public void Initialize(IServiceBus bus)
        {
            serviceBusConfiguration = bus.Configuration;

            using (databaseConnectionFactory.Create(SubscriptionDataSource))
            {
                if (databaseGateway.GetScalarUsing<int>(
                    SubscriptionDataSource,
                    RawQuery.Create(
                        scriptProvider.GetScript(
                            Script.SubscriptionManagerExists))) != 1)
                {
                    throw new SubscriptionManagerException(SqlResources.SubscriptionManagerDatabaseNotConfigured);
                }
            }

            if (HasDeferredSubscriptions)
            {
                Subscribe(deferredSubscriptions);
            }
        }

        public void Subscribe(IEnumerable<string> messageTypes)
        {
            if (!Started)
            {
                deferredSubscriptions.AddRange(messageTypes);

                return;
            }

            using (databaseConnectionFactory.Create(SubscriptionDataSource))
            {
                foreach (var messageType in messageTypes)
                {
                    databaseGateway.ExecuteUsing(
                        SubscriptionDataSource,
                        RawQuery.Create(
                            scriptProvider.GetScript(Script.SubscriptionManagerSubscribe))
                                .AddParameterValue(SubscriptionManagerColumns.InboxWorkQueueUri,
                                                   serviceBusConfiguration.Inbox.WorkQueue.Uri.ToString())
                                .AddParameterValue(SubscriptionManagerColumns.MessageType, messageType));
                }
            }
        }

        public IEnumerable<string> GetSubscribedUris(object message)
        {
            Guard.AgainstNull(message, "message");

            var messageType = message.GetType().FullName;

            if (!subscribers.ContainsKey(messageType))
            {
                lock (padlock)
                {
                    if (!subscribers.ContainsKey(messageType))
                    {
                        DataTable table;

                        using (databaseConnectionFactory.Create(SubscriptionDataSource))
                        {
                            table = databaseGateway.GetDataTableFor(
                                SubscriptionDataSource,
                                RawQuery.Create(
                                    scriptProvider.GetScript(
                                        Script.SubscriptionManagerInboxWorkQueueUris))
                                        .AddParameterValue(SubscriptionManagerColumns.MessageType, messageType));
                        }

                        subscribers.Add(messageType, (from DataRow row in table.Rows
                                                      select SubscriptionManagerColumns.InboxWorkQueueUri.MapFrom(row))
                                                         .ToList());
                    }
                }
            }

            return subscribers[messageType];
        }
    }
}