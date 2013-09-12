using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
    public class SubscriptionRequestManagementPresenter : ManagementModulePresenter,
                                                          ISubscriptionRequestManagementPresenter
    {
        private readonly ISubscriptionRequestManagementView view;

        private readonly IDatabaseGateway databaseGateway;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;
        private readonly ISubscriptionRequestQuery subscriptionRequestQuery;

        public SubscriptionRequestManagementPresenter(IDatabaseGateway databaseGateway, IDatabaseConnectionFactory databaseConnectionFactory, ISubscriptionRequestQuery subscriptionRequestQuery)
        {
            view = new SubscriptionRequestManagementView(this);

            this.databaseGateway = databaseGateway;
            this.databaseConnectionFactory = databaseConnectionFactory;
            this.subscriptionRequestQuery = subscriptionRequestQuery;
        }

        public void AcceptRequests()
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

            QueueTask("AcceptRequests",
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

            RefreshRequests();
        }

        public void DeclineRequests()
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

            var declainedReason = view.DeclineReasonValue;

            QueueTask("DeclineRequests",
                      () =>
                      {
                          var connectionName = DataSourceFactory.Create(dataStoreName);

                          var declinedDate = DateTime.Now;

                          using (databaseConnectionFactory.Create(connectionName))
                          {
                              foreach (var messageType in view.SelectedMessageTypes)
                              {
                                  databaseGateway.ExecuteUsing(
                                      connectionName,
                                      SubscriptionRequestTableAccess.Decline(inboxWorkQueueUri, messageType,
                                                                             EnvironmentExtensions.FullDomainUser(), declinedDate,
                                                                             declainedReason));
                              }
                          }
                      });

            RefreshRequests();
        }

        public void CheckAll()
        {
            view.CheckAll();
        }

        public void InvertChecks()
        {
            view.InvertChecks();
        }

        public void RefreshRequests()
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
                          view.ClearRequests();

                          var dataSource = DataSourceFactory.Create(dataStoreName);

                          using (databaseConnectionFactory.Create(dataSource))
                          {
                              foreach (DataRow row in
                                  subscriptionRequestQuery.MessageTypes(dataSource, inboxWorkQueueUriValue).Rows)
                              {
                                  view.AddRequest(
                                      SubscriptionRequestColumns.MessageType.MapFrom(row),
                                      SubscriptionRequestColumns.Declined.MapFrom(row) == 1,
                                      SubscriptionRequestColumns.DeclinedBy.MapFrom(row),
                                      SubscriptionRequestColumns.DeclinedDate.MapFrom(row),
                                      SubscriptionRequestColumns.DeclinedReason.MapFrom(row));
                              }
                          }
                      });
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
                                  uris.AddRange(from DataRow row in subscriptionRequestQuery.AllUris(dataSource).Rows
                                                select SubscriptionColumns.InboxWorkQueueUri.MapFrom(row));
                              }
                          }

                          uris.Sort();

                          view.PopulateSubscriberUris(uris);
                      });

            RefreshRequests();
        }

        public void DataStoreChanged()
        {
            RefreshSubscribers();
        }

        public void RefreshDataStores()
        {
            QueueTask("RefreshDataStores", () => view.PopulateDataStores(ManagementConfiguration.DataStoreRepository().All()));
        }

        public override string Text
        {
            get { return SubscriptionResources.Text_SubscriptionRequests; }
        }

        public override Image Image
        {
            get { return SubscriptionResources.Image_SubscriptionRequests; }
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

        public override void OnViewReady()
        {
            base.OnViewReady();

            RefreshDataStores();
            RefreshSubscribers();
        }
    }
}