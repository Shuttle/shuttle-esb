using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
	public partial class SubscriptionRequestManagementView : UserControl, ISubscriptionRequestManagementView
	{
		private readonly ISubscriptionRequestManagementPresenter presenter;

		public SubscriptionRequestManagementView(ISubscriptionRequestManagementPresenter presenter)
		{
			InitializeComponent();

			this.presenter = presenter;

			RequestToolStrip.AddItem(SubscriptionResources.Text_Accept,
									 SubscriptionResources.Image_Accept,
									 delegate { presenter.AcceptRequests(); });
			RequestToolStrip.AddItem(SubscriptionResources.Text_Decline,
									 SubscriptionResources.Image_Decline,
									 delegate { presenter.DeclineRequests(); });
			RequestToolStrip.AddItem(ManagementResources.Text_CheckAll,
									 delegate { presenter.CheckAll(); });
			RequestToolStrip.AddItem(ManagementResources.Text_InvertChecks,
									 delegate { presenter.InvertChecks(); });
			RequestToolStrip.AddItem(SubscriptionResources.Text_RefreshSubscriptions,
									 ManagementResources.Image_RefreshSmall,
									 delegate { presenter.RefreshRequests(); });
			RequestToolStrip.AddItem(SubscriptionResources.Text_RefreshSubscribers,
									 ManagementResources.Image_RefreshSmall,
									 delegate { presenter.RefreshSubscribers(); });
			RequestToolStrip.AddItem(ManagementResources.Text_RefreshDataStores,
									 ManagementResources.Image_RefreshSmall,
									 delegate { presenter.RefreshDataStores(); });
		}

		public void PopulateSubscriberUris(IEnumerable<string> uris)
		{
			this.Invoke(() =>
							{
								InboxWorkQueueUri.Clear();

								uris.ForEach(uri => InboxWorkQueueUri.AddQueue(uri));
							});
		}

		public string InboxWorkQueueUriValue
		{
			get { return InboxWorkQueueUri.Text; }
		}

		public string DeclineReasonValue
		{
			get { return DeclineReason.Text; }
		}

		public void AddRequest(string messageType, bool declined, string declinedBy, DateTime? declinedDate,
							   string declinedReason)
		{
			this.Invoke(() =>
							{
								if (RequestExists(messageType))
								{
									return;
								}

								var color = declined
												? Color.Red
												: SystemColors.ControlText;

								var item = RequestList.Items.Add(messageType);

								item.ForeColor = color;

								item.SubItems.Add(declinedBy).ForeColor = color;
								item.SubItems.Add(declinedDate.HasValue
													? declinedDate.Value.ToString(ManagementResources.FormatDateTime)
													: string.Empty).ForeColor = color;

								item.ToolTipText = declinedReason;
							});
		}

		private bool RequestExists(string messageType)
		{
			return
				RequestList.Items.Cast<ListViewItem>().Any(
					item => item.Text.Equals(messageType, StringComparison.InvariantCultureIgnoreCase));
		}

		private ListViewItem FindRequestItem(string messageType)
		{
			return
				RequestList.Items.Cast<ListViewItem>().FirstOrDefault(
					item => item.Text.Equals(messageType, StringComparison.InvariantCultureIgnoreCase));
		}

		public List<string> SelectedMessageTypes
		{
			get
			{
				var result = new List<string>();

				this.Invoke(() =>
								{
									if (RequestList.CheckedItems.Count > 0)
									{
										result.AddRange(from ListViewItem item in RequestList.CheckedItems
														select item.Text);
									}
									else
									{
										result.AddRange(from ListViewItem item in RequestList.SelectedItems
														select item.Text);
									}
								});

				return result;
			}
		}

		public void InvertChecks()
		{
			this.Invoke(() =>
							{
								foreach (ListViewItem item in RequestList.Items)
								{
									item.Checked = !item.Checked;
								}
							}
				);
		}

		public void RemoveMessageType(string inboxWorkQueueUri, string messageType)
		{
			this.Invoke(() =>
							{
								if (!InboxWorkQueueUriValue.Equals(inboxWorkQueueUri))
								{
									return;
								}

								var item = FindRequestItem(messageType);

								if (item != null)
								{
									RequestList.Items.Remove(item);
								}

								if (RequestList.Items.Count != 0)
								{
									return;
								}

								InboxWorkQueueUri.RemoveQueue(inboxWorkQueueUri);
								InboxWorkQueueUri.Text = "";
							});
		}

		public void DeclineMessageType(string inboxWorkQueueUri, string messageType, string declinedBy, DateTime declinedDate,
									   string declinedReason)
		{
			this.Invoke(() =>
							{
								if (!InboxWorkQueueUriValue.Equals(inboxWorkQueueUri))
								{
									return;
								}

								var item = FindRequestItem(messageType);

								if (item == null)
								{
									return;
								}

								item.SubItems[1].Text = declinedBy;
								item.SubItems[2].Text = declinedDate.ToString(ManagementResources.FormatDateTime);
								item.ToolTipText = declinedReason;

								var color = Color.Red;

								item.ForeColor = color;

								foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
								{
									subItem.ForeColor = color;
								}
							});
		}

		public void CheckAll()
		{
			this.Invoke(() =>
							{
								foreach (ListViewItem item in RequestList.Items)
								{
									item.Checked = true;
								}
							});
		}

		public string DataStoreValue
		{
			get { return DataStore.Text; }
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

		public void ClearRequests()
		{
			this.Invoke(() => RequestList.Items.Clear());
		}

		private void DataStore_SelectedIndexChanged(object sender, EventArgs e)
		{
			presenter.DataStoreChanged();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			presenter.OnViewReady();
		}

        private void InboxWorkQueueUri_KeyUp(object sender, KeyEventArgs e)
        {
            e.OnEnterPressed(presenter.RefreshRequests);
        }

        private void InboxWorkQueueUri_QueueSelected(object sender, QueueSelectedEventArgs e)
        {
            presenter.RefreshRequests();
        }

	}
}