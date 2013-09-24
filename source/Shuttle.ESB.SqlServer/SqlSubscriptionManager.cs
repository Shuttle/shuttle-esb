using System;
using System.Collections.Generic;
using System.Data;
using Shuttle.Core.Data;
using Shuttle.ESB.Core;
using Shuttle.ESB.Core.EventSubscription;

namespace Shuttle.ESB.SqlServer
{
    public class SqlSubscriptionManager :
        ISubscriptionManager,
        ICreate
    {
        private readonly IScriptConfiguration scriptConfiguration;

        private string createQuery;
        private string existsQuery;
        private string subscribeQuery;
        private string inboxWorkQueueUrisQuery;
        private string backboneInboxWorkQueueUriQuery;
        private string backboneInboxWorkQueueUrisQuery;
        private string unsubscribeAllQuery;
        private string unsubscribeQuery;

        public SqlSubscriptionManager()
            : this(new ScriptConfiguration(), new ConnectionStringSettingsProvider())
        {
        }

        public SqlSubscriptionManager(IScriptConfiguration scriptConfiguration, IUnitOfWorkProvider unitOfWorkProvider)
        {
            this.scriptConfiguration = scriptConfiguration;
            this.connectionStringSettingsProvider = connectionStringSettingsProvider;
        }

        public void Create()
        {
            if (SubscriptionTableExists())
            {
                return;
            }

            gateway.ExecuteNonQuery(createQuery, scriptConfiguration.BatchSeparator());
        }

        public void Start()
        {
            gateway = new DatabaseGateway("SqlSubscriptionManager", connectionStringSettingsProvider);

            existsQuery = scriptConfiguration.GetScript(Script.SubscriptionManagerExists);
            createQuery = scriptConfiguration.GetScript(Script.SubscriptionManagerCreate);
            subscribeQuery = scriptConfiguration.GetScript(Script.SubscriptionManagerSubscribe);
            unsubscribeQuery = scriptConfiguration.GetScript(Script.SubscriptionManagerUnsubscribe);
            unsubscribeAllQuery = scriptConfiguration.GetScript(Script.SubscriptionManagerUnsubscribeAll);
            inboxWorkQueueUrisQuery = scriptConfiguration.GetScript(Script.SubscriptionManagerInboxWorkQueueUrisQuery);
            backboneInboxWorkQueueUriQuery = scriptConfiguration.GetScript(Script.SubscriptionManagerBackboneInboxWorkQueueUriQuery);
            backboneInboxWorkQueueUrisQuery = scriptConfiguration.GetScript(Script.SubscriptionManagerBackboneInboxWorkQueueUrisQuery);

            Create();
        }

        public void Subscribe(string inboxWorkQueueUri, string backboneInboxWorkQueueUri, IEnumerable<string> messageTypes)
        {
            using (var connection = gateway.OpenConnection())
            {
                foreach (var messageType in messageTypes)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = subscribeQuery;

                        command.Parameters.Add(
                            gateway.CreateParameter(
                                "InboxWorkQueueUri",
                                inboxWorkQueueUri));

                        command.Parameters.Add(
                            gateway.CreateParameter(
                                "BackboneInboxWorkQueueUri",
                                backboneInboxWorkQueueUri));

                        command.Parameters.Add(
                            gateway.CreateParameter(
                                "MessageType",
                                messageType));

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Unsubscribe(string inboxWorkQueueUri, IEnumerable<string> messageTypes)
        {
            using (var connection = gateway.OpenConnection())
            {
                foreach (var messageType in messageTypes)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = unsubscribeQuery;

                        command.Parameters.Add(
                            gateway.CreateParameter(
                                "InboxWorkQueueUri",
                                inboxWorkQueueUri));

                        command.Parameters.Add(
                            gateway.CreateParameter(
                                "MessageType",
                                messageType));

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Unsubscribe(string inboxWorkQueueUri)
        {
            using (var connection = gateway.OpenConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = unsubscribeAllQuery;

                command.Parameters.Add(
                    gateway.CreateParameter(
                        "InboxWorkQueueUri",
                        inboxWorkQueueUri));

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<string> GetInboxWorkQueueUris(string messageType)
        {
            using (var connection = gateway.OpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = inboxWorkQueueUrisQuery;

                    command.Parameters.Add(
                        gateway.CreateParameter(
                            "MessageType",
                            messageType));

                    var list = new List<string>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Convert.ToString(reader["InboxWorkQueueUri"]));
                        }
                    }

                    return list;
                }
            }
        }

        public string GetBackboneInboxWorkQueueUri(string inboxWorkQueueUri)
        {
            using (var connection = gateway.OpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = backboneInboxWorkQueueUriQuery;

                    command.Parameters.Add(
                        gateway.CreateParameter(
                            "InboxWorkQueueUri",
                            inboxWorkQueueUri));

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Read()
                                   ? Convert.ToString(reader["BackboneInboxWorkQueueUri"])
                                   : string.Empty;
                    }
                }
            }
        }

        public IEnumerable<string> GetBackboneInboxWorkQueueUris()
        {
            using (var connection = gateway.OpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = backboneInboxWorkQueueUrisQuery;

                    var list = new List<string>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Convert.ToString(reader["BackboneInboxWorkQueueUri"]));
                        }
                    }

                    return list;
                }
            }
        }

        public void SubscriptionAdded(string inboxWorkQueueUri, string backboneInboxWorkQueueUri, IEnumerable<string> messageTypes)
        {
            throw new NotImplementedException();
        }

        public void SubscriptionRemoved(string inboxWorkQueueUri, string backboneInboxWorkQueueUri, IEnumerable<string> messageTypes)
        {
            throw new NotImplementedException();
        }

        public void SubscriptionRemoved(string inboxWorkQueueUri)
        {
            throw new NotImplementedException();
        }

        private bool SubscriptionTableExists()
        {
            return gateway.ExecuteScalar<int>(existsQuery) == 1;
        }
    }
}