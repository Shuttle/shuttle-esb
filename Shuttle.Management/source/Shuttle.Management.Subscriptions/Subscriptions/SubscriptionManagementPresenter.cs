using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
    public class SubscriptionManagementPresenter : ManagementModulePresenter, ISubscriptionManagementPresenter
    {
        private readonly IDatabaseGateway databaseGateway;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;
        private readonly ISubscriptionQuery subscriptionQuery;

        private readonly ISubscriptionManagementView view;

        public SubscriptionManagementPresenter(IDatabaseGateway databaseGateway, IDatabaseConnectionFactory databaseConnectionFactory, ISubscriptionQuery subscriptionQuery)
        {
            view = new SubscriptionManagementView(this);

            this.databaseGateway = databaseGateway;
            this.databaseConnectionFactory = databaseConnectionFactory;
            this.subscriptionQuery = subscriptionQuery;
        }

        public void CheckAllSubscriptions()
        {
            view.CheckAllSubscriptions();
        }

        public void InvertSubscriptionChecks()
        {
            view.InvertSubscriptionChecks();
        }

        public void RefreshSubscriptions()
        {
            var dataStoreName = view.DataStoreValue;

            if (string.IsNullOrEmpty(dataStoreName))
            {
                Log.Warning(ManagementResources.NoDataStoreSelected);

                return;
            }

            var inboxWorkQueueUriValue = view.InboxWorkQueueUriValue;

            if (string.IsNullOrEmpty(inboxWorkQueueUriValue))
            {
                Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.InboxWorkQueueUri));

                return;
            }

            QueueTask("RefreshSubscriptions",
                      () =>
                      {
                          view.ClearSubscriptions();

                          var dataSource = DataSourceFactory.Create(dataStoreName);

                          using (databaseConnectionFactory.Create(dataSource))
                              foreach (DataRow row in
                                  subscriptionQuery.MessageTypes(dataSource, inboxWorkQueueUriValue).Rows)
                              {
                                  view.AddSubscription(
                                      SubscriptionColumns.MessageType.MapFrom(row),
                                      SubscriptionColumns.AcceptedBy.MapFrom(row),
                                      SubscriptionColumns.AcceptedDate.MapFrom(row));
                              }
                      });
        }

        public override void OnViewReady()
        {
            base.OnViewReady();

            RefreshDataStores();
            RefreshSubscribers();
        }

        public void RefreshDataStores()
        {
            QueueTask("RefreshDataStores", () => view.PopulateDataStores(ManagementConfiguration.DataStoreRepository().All()));
        }

        public void AddSubscriptions()
        {
            var dataStoreName = view.DataStoreValue;

            if (string.IsNullOrEmpty(dataStoreName))
            {
                Log.Warning(ManagementResources.NoDataStoreSelected);

                return;
            }

            var inboxWorkQueueUri = view.InboxWorkQueueUriValue;

            if (string.IsNullOrEmpty(inboxWorkQueueUri))
            {
                Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.InboxWorkQueueUri));

                return;
            }

            QueueTask("AddSubscriptions",
                      () =>
                      {
                          var connectionName = DataSourceFactory.Create(dataStoreName);

                          using (databaseConnectionFactory.Create(connectionName))
                          {
                              foreach (var messageType in view.SelectedMessageTypes)
                              {
                                  databaseGateway.ExecuteUsing(
                                      connectionName,
                                      SubscriptionRequestTableAccess.Remove(inboxWorkQueueUri, messageType));

                                  if (databaseGateway.GetScalarUsing<int>(
                                      connectionName,
                                      SubscriptionTableAccess.Exists(inboxWorkQueueUri, messageType)) != 0)
                                  {
                                      continue;
                                  }

                                  databaseGateway.ExecuteUsing(
                                      connectionName,
                                      SubscriptionTableAccess.Add(inboxWorkQueueUri, messageType, EnvironmentExtensions.FullDomainUser()));
                              }
                          }
                      });

            RefreshSubscriptions();
        }

        public void CheckAllEventMessageTypes()
        {
            view.CheckAllEventMessageTypes();
        }

        public void InvertEventMessageTypeChecks()
        {
            view.InvertEventMessageTypeChecks();
        }

        public void GetAssemblyEventMessageTypes()
        {
            view.GetAssemblyFileName();
        }

        public void RefreshSubscribers()
        {
            var dataStoreName = view.DataStoreValue;

            QueueTask("RefreshSubscribers",
                      () =>
                      {
                          var uris = new List<string>();

                          if (!string.IsNullOrEmpty(dataStoreName))
                          {
                              var dataSource = DataSourceFactory.Create(dataStoreName);

                              using (databaseConnectionFactory.Create(dataSource))
                              {
                                  uris.AddRange(from DataRow row in subscriptionQuery.AllUris(dataSource).Rows
                                                select SubscriptionColumns.InboxWorkQueueUri.MapFrom(row));
                              }
                          }

                          uris.AddRange(from Queue queue in ManagementConfiguration.QueueRepository().All()
                                        where !uris.Contains(queue.Uri)
                                        select queue.Uri);

                          uris.Sort();

                          view.PopulateSubscriberUris(uris);
                      });

            RefreshSubscriptions();
        }

        public void RemoveSubscriptions()
        {
            var dataStoreName = view.DataStoreValue;

            if (string.IsNullOrEmpty(dataStoreName))
            {
                Log.Warning(ManagementResources.NoDataStoreSelected);

                return;
            }

            var inboxWorkQueueUri = view.InboxWorkQueueUriValue;

            if (string.IsNullOrEmpty(inboxWorkQueueUri))
            {
                Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.InboxWorkQueueUri));

                return;
            }

            if (MessageBox.Show(string.Format(ManagementResources.ConfirmRemoval, SubscriptionResources.Text_Subscriptions), ManagementResources.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            QueueTask("RemoveSubscriptions",
                      () =>
                      {
                          var connectionName = DataSourceFactory.Create(dataStoreName);

                          using (databaseConnectionFactory.Create(connectionName))
                          {
                              foreach (var messageType in view.SelectedSubscriptions)
                              {
                                  databaseGateway.ExecuteUsing(
                                      connectionName,
                                      SubscriptionTableAccess.Remove(inboxWorkQueueUri, messageType));
                              }
                          }
                      }
                );

            RefreshSubscriptions();
        }

        public void DataStoreChanged()
        {
            var dataSource = DataSourceFactory.Create(view.DataStoreValue);

            using (databaseConnectionFactory.Create(dataSource))
            {
                if (!subscriptionQuery.HasSubscriptionStructures(dataSource))
                {
                    Log.Error(
                        string.Format(
                            "Data store '{0}' does not contain the required structures for subscription handling.  Please execute the relevant creation script against the data store.",
                            view.DataStoreValue));
                }
            }

            RefreshSubscribers();
        }

        public override string Text
        {
            get { return SubscriptionResources.Text_Subscriptions; }
        }

        public override Image Image
        {
            get { return SubscriptionResources.Image_Subscriptions; }
        }

        public override UserControl ViewUserControl
        {
            get
            {
                var control = (UserControl)view;

                control.Enabled = ManagementConfiguration.HasDataStoreRepository;

                return control;
            }
        }
    }
}