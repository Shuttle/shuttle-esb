using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.SqlServer
{
    public class SqlQueue : IQueue, ICreate, IDrop, IPurge, ICount, IQueueReader
    {
        [ThreadStatic] private static object underlyingMessageData;

        internal const string SCHEME = "sql";
        private readonly DataSource dataSource;
        private readonly IDatabaseGateway databaseGateway;

        private static readonly object padlock = new object();

        private readonly ISubscriptionManagerConfiguration subscriptionManagerConfiguration;
        private readonly IScriptProvider scriptProvider;

        private readonly string tableName;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;

        private IQuery countQuery;
        private IQuery createQuery;
        private IQuery dequeueQuery;
        private IQuery dropQuery;
        private IQuery existsQuery;
        private IQuery purgeQuery;

        private string removeQueryStatement;
        private string enqueueQueryStatement;
        private string dequeueIdQueryStatement;

        private readonly ILog log;

        public SqlQueue(Uri uri)
            : this(uri,
                   new SubscriptionManagerConfiguration(),
                   new ScriptProvider(),
                   DatabaseConnectionFactory.Default(),
                   DatabaseGateway.Default())
        {
        }

        public SqlQueue(Uri uri,
                        ISubscriptionManagerConfiguration subscriptionManagerConfiguration,
                        IScriptProvider scriptProvider,
                        IDatabaseConnectionFactory databaseConnectionFactory,
                        IDatabaseGateway databaseGateway)
        {
            Guard.AgainstNull(uri, "uri");
            Guard.AgainstNull(subscriptionManagerConfiguration, "subscriptionManagerConfiguration");
            Guard.AgainstNull(scriptProvider, "scriptProvider");
            Guard.AgainstNull(databaseConnectionFactory, "databaseConnectionFactory");
            Guard.AgainstNull(databaseGateway, "databaseGateway");

            this.subscriptionManagerConfiguration = subscriptionManagerConfiguration;
            this.scriptProvider = scriptProvider;
            this.databaseConnectionFactory = databaseConnectionFactory;
            this.databaseGateway = databaseGateway;

            if (!uri.Scheme.Equals(SCHEME, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidSchemeException(SCHEME, uri.ToString());
            }

            var builder = new UriBuilder(uri);

            if (uri.LocalPath == "/" || uri.Segments.Length != 2)
            {
                throw new UriFormatException(string.Format(ESBResources.UriFormatException,
                                                           "sql://{{connection-name}}/{{table-name}}",
                                                           uri));
            }

            log = Log.For(this);

            Uri = builder.Uri;

            dataSource = new DataSource(Uri.Host,
                                        new SqlServerDbDataParameterFactory(),
                                        new SqlServerContainsQueryFactory(),
                                        new SqlServerInsertQueryFactory(),
                                        new SqlServerUpdateQueryFactory(),
                                        new SqlServerDeleteQueryFactory(),
                                        new SqlServerSelectQueryFactory());

            using (databaseConnectionFactory.Create(dataSource))
            {
                var host = databaseGateway.GetScalarUsing<string>(dataSource,
                                                                  RawQuery.CreateFrom("select host_name()"));

                IsLocal = (host ?? string.Empty) == Environment.MachineName;
            }

            tableName = Uri.Segments[1];

            IsTransactional = true;

            BuildQueries();
        }

        public int Count
        {
            get
            {
                try
                {
                    using (databaseConnectionFactory.Create(dataSource))
                    {
                        return databaseGateway.GetScalarUsing<int>(dataSource, countQuery);
                    }
                }
                catch
                {
                    log.Debug(string.Format("Could not get count for queue '{0}'.  Query: {1}", Uri, countQuery));

                    return 0;
                }
            }
        }

        public void Create()
        {
            if (Exists() != QueueAvailability.Missing)
            {
                return;
            }

            try
            {
                using (databaseConnectionFactory.Create(dataSource))
                {
                    databaseGateway.ExecuteUsing(dataSource, createQuery);
                }
            }
            catch (Exception ex)
            {
                log.Debug(string.Format("Could not create queue '{0}'.  Exception: {1}.  Query: {2}", Uri,
                                        ex.Message, createQuery));

                throw;
            }
        }

        public void Drop()
        {
            if (Exists() != QueueAvailability.Exists)
            {
                return;
            }

            try
            {
                using (databaseConnectionFactory.Create(dataSource))
                {
                    databaseGateway.ExecuteUsing(dataSource, dropQuery);
                }
            }
            catch (Exception ex)
            {
                log.Debug(string.Format("Could not drop queue '{0}'.  Exception: {1}.  Query: {2}", Uri, ex.Message,
                                        createQuery));

                throw;
            }
        }

        public void Purge()
        {
            if (Exists() != QueueAvailability.Exists)
            {
                return;
            }

            try
            {
                using (databaseConnectionFactory.Create(dataSource))
                {
                    databaseGateway.ExecuteUsing(dataSource, purgeQuery);
                }
            }
            catch (Exception ex)
            {
                log.Debug(string.Format("Could not purge queue '{0}'.  Exception: {1}.  Query: {2}", Uri, ex.Message,
                                        createQuery));

                throw;
            }
        }

        public bool IsTransactional { get; private set; }

        public void Enqueue(object data)
        {
            Guard.AgainstNull(data, "data");

            var row = data as DataRow;

            if (row == null)
            {
                throw new EnqueueMessageDataTypeMismatchException(data.GetType().FullName,
                                                                  Uri.ToString(),
                                                                  typeof (DataRow).FullName);
            }

            var messageId = QueueColumns.MessageId.MapFrom(row);

            try
            {
                using (databaseConnectionFactory.Create(dataSource))
                {
                    databaseGateway.ExecuteUsing(
                        dataSource,
                        RawQuery.CreateFrom(enqueueQueryStatement)
                                .AddParameterValue(QueueColumns.MessageId, messageId)
                                .AddParameterValue(QueueColumns.MessageBody, QueueColumns.MessageBody.MapFrom(row)));
                }
            }
            catch (Exception ex)
            {
                log.Debug(
                    string.Format(
                        "Could not enqueue message data with message id '{0}' on queue '{1}'.  Exception: {2}",
                        messageId, Uri, ex.Message));

                throw;
            }
        }

        public void Enqueue(Guid messageId, Stream stream)
        {
            try
            {
                using (databaseConnectionFactory.Create(dataSource))
                {
                    databaseGateway.ExecuteUsing(
                        dataSource,
                        RawQuery.CreateFrom(enqueueQueryStatement)
                                .AddParameterValue(QueueColumns.MessageId, messageId)
                                .AddParameterValue(QueueColumns.MessageBody, stream.ToBytes()));
                }
            }
            catch (Exception ex)
            {
                log.Debug(
                    string.Format(
                        "Could not enqueue message id '{0}' on queue '{1}'.  Exception: {2}", messageId, Uri, ex.Message));

                throw;
            }
        }

        public bool IsLocal { get; private set; }
        public Uri Uri { get; private set; }

        public QueueAvailability Exists()
        {
            try
            {
                using (databaseConnectionFactory.Create(dataSource))
                {
                    return databaseGateway.GetScalarUsing<int>(dataSource, existsQuery) == 1
                               ? QueueAvailability.Exists
                               : QueueAvailability.Missing;
                }
            }
            catch (Exception ex)
            {
                log.Debug(string.Format("Could not check whether queue '{0}' exists.  Exception: {1}.  Query: {2}", Uri,
                                        ex.Message, existsQuery));

                throw;
            }
        }

        public bool IsEmpty()
        {
            return Count == 0;
        }

        public object UnderlyingMessageData
        {
            get { return underlyingMessageData; }
        }

        private void ResetUnderlyingMessageData()
        {
            underlyingMessageData = null;
        }

        public Stream Dequeue()
        {
            ResetUnderlyingMessageData();

            lock (padlock)
            {
                try
                {
                    using (databaseConnectionFactory.Create(dataSource))
                    {
                        var row = databaseGateway.GetSingleRowUsing(dataSource, dequeueQuery);

                        if (row == null)
                        {
                            return null;
                        }

                        underlyingMessageData = row;

                        return new MemoryStream((byte[]) row["MessageBody"]);
                    }
                }
                catch (Exception ex)
                {
                    log.Debug(string.Format("Could not dequeue message from queue '{0}'.  Exception: {1}.  Query: {2}",
                                            Uri, ex.Message, createQuery));

                    throw;
                }
            }
        }

        public Stream Dequeue(Guid messageId)
        {
            ResetUnderlyingMessageData();

            lock (padlock)
            {
                try
                {
                    using (databaseConnectionFactory.Create(dataSource))
                    {
                        var row = databaseGateway.GetSingleRowUsing(
                            dataSource,
                            RawQuery.CreateFrom(dequeueIdQueryStatement).AddParameterValue(QueueColumns.MessageId,
                                                                                           messageId));

                        if (row == null)
                        {
                            return null;
                        }

                        underlyingMessageData = row;

                        using (var stream = new MemoryStream((byte[]) row["MessageBody"]))
                        {
                            underlyingMessageData = new MemoryStream(stream.ToBytes());

                            return (Stream) underlyingMessageData;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Debug(string.Format("Could not dequeue message from queue '{0}'.  Exception: {1}.  Query: {2}",
                                            Uri, ex.Message, createQuery));

                    throw;
                }
            }
        }

        public bool Remove(Guid messageId)
        {
            ResetUnderlyingMessageData();

            try
            {
                using (databaseConnectionFactory.Create(dataSource))
                {
                    return databaseGateway.ExecuteUsing(
                        dataSource,
                        RawQuery.CreateFrom(removeQueryStatement)
                                .AddParameterValue(QueueColumns.MessageId, messageId)) > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug(string.Format("Could not remove message from queue '{0}'.  Exception: {1}.  Query: {2}",
                                        Uri, ex.Message, createQuery));

                throw;
            }
        }

        private void BuildQueries()
        {
            existsQuery = RawQuery.CreateFrom(scriptProvider.GetScript(Script.QueueExists, tableName));
            createQuery = RawQuery.CreateFrom(scriptProvider.GetScript(Script.QueueCreate, tableName));
            dropQuery = RawQuery.CreateFrom(scriptProvider.GetScript(Script.QueueDrop, tableName));
            purgeQuery = RawQuery.CreateFrom(scriptProvider.GetScript(Script.QueuePurge, tableName));
            dequeueQuery = RawQuery.CreateFrom(scriptProvider.GetScript(Script.QueueDequeue, tableName));
            countQuery = RawQuery.CreateFrom(scriptProvider.GetScript(Script.QueueCount, tableName));
            enqueueQueryStatement = scriptProvider.GetScript(Script.QueueEnqueue, tableName);
            removeQueryStatement = scriptProvider.GetScript(Script.QueueRemove, tableName);
            dequeueIdQueryStatement = scriptProvider.GetScript(Script.QueueDequeueId, tableName);
        }

        public IEnumerable<Stream> Read(int top)
        {
            try
            {
                var result = new List<Stream>();

                using (databaseConnectionFactory.Create(dataSource))
                {
                    using (var reader = databaseGateway.GetReaderUsing(
                        dataSource,
                        RawQuery.CreateFrom(scriptProvider.GetScript(Script.QueueRead,
                                                                     top.ToString(),
                                                                     tableName))))
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
                log.Debug(string.Format(
                    "Could not read top {0} messages from queue '{1}'.  Exception: {2}.  Query: {3}",
                    top, Uri, ex.Message, createQuery));

                throw;
            }
        }
    }
}