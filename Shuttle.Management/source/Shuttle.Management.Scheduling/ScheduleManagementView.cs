using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Scheduling
{
	public partial class ScheduleManagementView : UserControl, IScheduleManagementView
	{
		private readonly IScheduleManagementPresenter presenter;

		public ScheduleManagementView(IScheduleManagementPresenter presenter)
		{
			this.presenter = presenter;

			InitializeComponent();

			SchedulingToolStrip.AddItem(ManagementResources.Text_Remove,
										ManagementResources.Image_Remove,
										delegate { presenter.RemoveSchedule(); });
			SchedulingToolStrip.AddItem(ManagementResources.Text_Save,
									   ManagementResources.Image_Save,
									   delegate { presenter.SaveSchedule(); });
			SchedulingToolStrip.AddItem(ManagementResources.Text_CheckAll,
										delegate { presenter.CheckAllSchedules(); });
			SchedulingToolStrip.AddItem(ManagementResources.Text_InvertChecks,
										delegate { presenter.InvertScheduleChecks(); });
			SchedulingToolStrip.AddItem(SchedulingResources.Text_RefreshSchedules,
										ManagementResources.Image_RefreshSmall,
										delegate { presenter.RefreshSchedules(); });
			SchedulingToolStrip.AddItem(ManagementResources.Text_RefreshDataStores,
										ManagementResources.Image_RefreshSmall,
										delegate { presenter.RefreshDataStores(); });
		}

		protected override void OnLoad(EventArgs e)
		{
			presenter.OnViewReady();
		}

		public void AddSchedule(string name, string inboxWorkQueueUri, string cronExpression, DateTime nextNotification)
		{
			this.Invoke(() =>
							{
								var item = ScheduleList.Items.Add(name);

								item.SubItems.Add(inboxWorkQueueUri);
								item.SubItems.Add(cronExpression);
								item.SubItems.Add(nextNotification.FormatFull());

							});
		}

		public List<string> GetSelectedScheduleNames()
		{
			return (from ListViewItem item in ScheduleList.CheckedItems select item.Text).ToList();
		}

		public void MarkAllSchedules()
		{
			this.Invoke(() =>
							{
								foreach (ListViewItem item in ScheduleList.Items)
								{
									item.Checked = true;
								}
							}
				);
		}

		public void InvertMarkedSchedules()
		{
			this.Invoke(() =>
							{
								foreach (ListViewItem item in ScheduleList.Items)
								{
									item.Checked = !item.Checked;
								}
							}
				);
		}

		public void ClearSchedules()
		{
			this.Invoke(() => ScheduleList.Items.Clear());
		}

		public void PopulateDataStores(IEnumerable<DataStore> list)
		{
			this.Invoke(
				() =>
				{
					DataStore.Items.Clear();

					foreach (var store in list)
					{
						DataStore.Items.Add(store.Name);
					}
				});
		}

		public string DataStoreValue
		{
			get { return DataStore.Text; }
		}

		public void CheckAllSchedules()
		{
			this.Invoke(() =>
							{
								foreach (ListViewItem item in ScheduleList.Items)
								{
									item.Checked = true;
								}
							}
				);
		}

		public void InvertScheduleChecks()
		{
			this.Invoke(() =>
							{
								foreach (ListViewItem item in ScheduleList.Items)
								{
									item.Checked = !item.Checked;
								}
							}
				);
		}

		public string ScheduleNameValue
		{
			get { return ScheduleName.Text; }
			set { ScheduleName.Text = value; }
		}

		public string EndpointInboxWorkQueueUriValue
		{
			get { return EndpointInboxWorkQueueUri.Text; }
			set { EndpointInboxWorkQueueUri.Text = value; }
		}

		public string CronExpressionValue
		{
			get { return CronExpression.Text; }
			set { CronExpression.Text = value; }
		}

		public IEnumerable<string> SelectedScheduleNames
		{
			get
			{
				var result = new List<string>();

				this.Invoke(() =>
				{
					if (ScheduleList.CheckedItems.Count > 0)
					{
						result.AddRange(from ListViewItem item in ScheduleList.CheckedItems
										select item.Text);
					}
					else
					{
						result.AddRange(from ListViewItem item in ScheduleList.SelectedItems
										select item.Text);
					}
				});

				return result;
			}
		}

		public void PopulateInboxWorkQueueUris(IEnumerable<Queue> list)
		{
			this.Invoke(() =>
			{
				EndpointInboxWorkQueueUri.Clear();

				foreach (var queue in list)
				{
					EndpointInboxWorkQueueUri.AddQueue(queue.Uri);
				}
			});
		}

		private void DataStore_SelectedIndexChanged(object sender, EventArgs e)
		{
			presenter.DataStoreChanged();
		}

		private void Save(object sender, KeyEventArgs e)
		{
			e.OnEnterPressed(presenter.SaveSchedule);
		}

		private void ScheduleList_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = GetSelectedSchedule();

			if (item == null)
			{
				ScheduleNameValue = string.Empty;
				CronExpressionValue = string.Empty;
				EndpointInboxWorkQueueUriValue = string.Empty;

				return;
			}

			ScheduleNameValue = item.Text;
			CronExpressionValue = item.SubItems[CronExpressionColumn.Index].Text;
			EndpointInboxWorkQueueUriValue = item.SubItems[InboxWorkQueueUriColumn.Index].Text;
		}

		private ListViewItem GetSelectedSchedule()
		{
			return ScheduleList.SelectedItems.Count == 0 ? null : ScheduleList.SelectedItems[0];
		}
	}
}