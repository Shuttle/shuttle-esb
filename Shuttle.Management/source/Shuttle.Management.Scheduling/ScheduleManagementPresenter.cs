using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;
using Shuttle.Management.Shell;
using Shuttle.Scheduling;

namespace Shuttle.Management.Scheduling
{
    public class ScheduleManagementPresenter : ManagementModulePresenter, IScheduleManagementPresenter
    {
        private readonly IScheduleQuery scheduleQuery;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;
        private readonly IDatabaseGateway databaseGateway;
        private readonly IScheduleManagementView view;

        public ScheduleManagementPresenter(IScheduleQuery scheduleQuery, IDatabaseConnectionFactory databaseConnectionFactory, IDatabaseGateway databaseGateway)
        {
            view = new ScheduleManagementView(this);

            this.scheduleQuery = scheduleQuery;
            this.databaseConnectionFactory = databaseConnectionFactory;
            this.databaseGateway = databaseGateway;
        }

        public override void OnViewReady()
        {
            base.OnViewReady();

            RefreshDataStores();
            RefreshInboxWorkQueueUris();
        }

        private void RefreshInboxWorkQueueUris()
        {
            QueueTask("RefreshInboxWorkQueueUris",
                      () => view.PopulateInboxWorkQueueUris(ManagementConfiguration.QueueRepository().All()));
        }

        public void RemoveSchedule()
        {
            var dataStoreName = view.DataStoreValue;

            if (string.IsNullOrEmpty(dataStoreName))
            {
                Log.Warning(ManagementResources.NoDataStoreSelected);

                return;
            }

            if (
                MessageBox.Show(string.Format(ManagementResources.ConfirmRemoval, SchedulingResources.Text_Schedules),
                                ManagementResources.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes)
            {
                return;
            }

            QueueTask("RemoveSchedules",
                      () =>
                      {
                          var connectionName = DataSourceFactory.Create(dataStoreName);

                          using (databaseConnectionFactory.Create(connectionName))
                          {
                              foreach (var name in view.SelectedScheduleNames)
                              {
                                  databaseGateway.ExecuteUsing(
                                      connectionName,
                                      ScheduleTableAccess.Remove(name));
                              }
                          }
                      });

            RefreshSchedules();
        }

        public void CheckAllSchedules()
        {
            view.CheckAllSchedules();
        }

        public void InvertScheduleChecks()
        {
            view.InvertScheduleChecks();
        }

        public void SaveSchedule()
        {
            var dataStoreName = view.DataStoreValue;

            if (string.IsNullOrEmpty(dataStoreName))
            {
                Log.Warning(ManagementResources.NoDataStoreSelected);

                return;
            }

            var scheduleName = view.ScheduleNameValue;

            if (string.IsNullOrEmpty(scheduleName))
            {
                Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, SchedulingResources.ScheduleName));

                return;
            }

            var inboxWorkQueueUri = view.EndpointInboxWorkQueueUriValue;

            if (string.IsNullOrEmpty(inboxWorkQueueUri))
            {
                Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.InboxWorkQueueUri));

                return;
            }

            var cronExpressionValue = view.CronExpressionValue;

            if (string.IsNullOrEmpty(cronExpressionValue))
            {
                Log.Warning(string.Format(ManagementResources.ValueMayNotBeEmpty, SchedulingResources.CronExpression));

                return;
            }

            CronExpression cronExpression;

            try
            {
                cronExpression = new CronExpression(cronExpressionValue);
            }
            catch
            {
                Log.Warning(string.Format(SchedulingResources.InvalidCronExpression, cronExpressionValue));

                return;
            }

            QueueTask("AddSchedule",
                      () =>
                      {
                          var connectionName = DataSourceFactory.Create(dataStoreName);

                          using (databaseConnectionFactory.Create(connectionName))
                          {
                              var nextNotification = cronExpression.NextOccurrence(DateTime.Now);

                              databaseGateway.ExecuteUsing(
                                  connectionName,
                                  databaseGateway.GetScalarUsing<int>(connectionName, ScheduleTableAccess.Contains(scheduleName)) == 1
                                      ? ScheduleTableAccess.Save(scheduleName, inboxWorkQueueUri, cronExpressionValue, nextNotification)
                                      : ScheduleTableAccess.Add(scheduleName, inboxWorkQueueUri, cronExpressionValue, nextNotification));
                          }
                      });

            RefreshSchedules();
        }

        public void RefreshDataStores()
        {
            QueueTask("RefreshDataStores", () => view.PopulateDataStores(ManagementConfiguration.DataStoreRepository().All()));
        }

        public void RefreshSchedules()
        {
            var dataStoreName = view.DataStoreValue;

            QueueTask("RefreshSchedules",
                      () =>
                      {
                          view.ClearSchedules();

                          var dataSource = DataSourceFactory.Create(dataStoreName);

                          using (databaseConnectionFactory.Create(dataSource))
                          {
                              foreach (DataRow row in scheduleQuery.All(dataSource).Rows)
                              {
                                  view.AddSchedule(ScheduleColumns.Name.MapFrom(row),
                                                   ScheduleColumns.InboxWorkQueueUri.MapFrom(row),
                                                   ScheduleColumns.CronExpression.MapFrom(row),
                                                   ScheduleColumns.NextNotification.MapFrom(row));
                              }
                          }
                      });
        }

        public void MarkAllSchedules()
        {
            view.MarkAllSchedules();
        }

        public void InvertMarkedSchedules()
        {
            view.InvertMarkedSchedules();
        }

        public override string Text
        {
            get { return SchedulingResources.Text_Schedules; }
        }

        public override Image Image
        {
            get { return SchedulingResources.Image_Schedules; }
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

        public void DataStoreChanged()
        {
            var dataSource = DataSourceFactory.Create(view.DataStoreValue);

            using (databaseConnectionFactory.Create(dataSource))
            {
                if (!scheduleQuery.HasScheduleStructures(dataSource))
                {
                    Log.Error(
                        string.Format(
                            "Data store '{0}' does not contain the required structures for schedule handling.  Please execute the relevant creation script against the data store.",
                            view.DataStoreValue));
                }
            }

            RefreshSchedules();
        }
    }
}