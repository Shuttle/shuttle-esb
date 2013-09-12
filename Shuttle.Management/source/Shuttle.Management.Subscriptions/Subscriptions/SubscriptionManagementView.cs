using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
	public partial class SubscriptionManagementView : UserControl, ISubscriptionManagementView
	{
		private readonly ISubscriptionManagementPresenter presenter;

		public SubscriptionManagementView(ISubscriptionManagementPresenter presenter)
		{
			this.presenter = presenter;

			InitializeComponent();

			SubscriptionToolStrip.AddItem(ManagementResources.Text_Remove,
			                              ManagementResources.Image_Remove,
			                              delegate { presenter.RemoveSubscriptions(); });
			SubscriptionToolStrip.AddItem(ManagementResources.Text_CheckAll,
			                              delegate { presenter.CheckAllSubscriptions(); });
			SubscriptionToolStrip.AddItem(ManagementResources.Text_InvertChecks,
			                              delegate { presenter.InvertSubscriptionChecks(); });
			SubscriptionToolStrip.AddItem(SubscriptionResources.Text_RefreshSubscriptions,
			                              ManagementResources.Image_RefreshSmall,
			                              delegate { presenter.RefreshSubscriptions(); });
			SubscriptionToolStrip.AddItem(SubscriptionResources.Text_RefreshSubscribers,
			                              ManagementResources.Image_RefreshSmall,
			                              delegate { presenter.RefreshSubscribers(); });
			SubscriptionToolStrip.AddItem(ManagementResources.Text_RefreshDataStores,
			                              ManagementResources.Image_RefreshSmall,
			                              delegate { presenter.RefreshDataStores(); });

			EventMessageTypeToolStrip.AddItem(ManagementResources.Text_Add,
			                                  ManagementResources.Image_Add,
			                                  delegate { presenter.AddSubscriptions(); });
			EventMessageTypeToolStrip.AddItem(ManagementResources.Text_CheckAll,
			                                  delegate { presenter.CheckAllEventMessageTypes(); });
			EventMessageTypeToolStrip.AddItem(ManagementResources.Text_InvertChecks,
			                                  delegate { presenter.InvertEventMessageTypeChecks(); });
			EventMessageTypeToolStrip.AddItem(SubscriptionResources.Text_GetAssemblyEventMessageTypes,
			                                  ManagementResources.Image_DriveMagnify,
			                                  delegate { presenter.GetAssemblyEventMessageTypes(); });
		}

		public void PopulateSubscriberUris(IEnumerable<string> uris)
		{
			this.Invoke(() =>
			            	{
								InboxWorkQueueUri.Clear();

			            		uris.ForEach(uri => InboxWorkQueueUri.AddQueue(uri));
			            	});
		}

		public void ClearSubscriptions()
		{
			this.Invoke(() => SubscriptionList.Items.Clear());
		}

		public string InboxWorkQueueUriValue
		{
			get { return InboxWorkQueueUri.Text; }
		}

		public string DataStoreValue
		{
			get { return DataStore.Text; }
		}

		public void AddSubscription(string messageType, string acceptedBy, DateTime acceptedDate)
		{
			this.Invoke(() =>
			            	{
			            		if (SubscriptionAlreadyPopulated(messageType))
			            		{
			            			return;
			            		}

			            		var item = SubscriptionList.Items.Add(messageType);

			            		item.SubItems.Add(acceptedBy);
			            		item.SubItems.Add(acceptedDate.ToString(ManagementResources.FormatDateTime));
			            	});
		}

		private bool SubscriptionAlreadyPopulated(string messageType)
		{
			return
				SubscriptionList.Items.Cast<ListViewItem>().Any(
					item => item.Text.Equals(messageType, StringComparison.InvariantCultureIgnoreCase));
		}

		public List<string> SelectedMessageTypes
		{
			get
			{
				var result = new List<string>();

				this.Invoke(() =>
				{
					if (EventMessageTypeList.CheckedItems.Count > 0)
					{
						result.AddRange(from ListViewItem item in EventMessageTypeList.CheckedItems
										select item.Text);
					}
					else
					{
						result.AddRange(from ListViewItem item in EventMessageTypeList.SelectedItems
										select item.Text);
					}
				});

				return result;
			}
		}

		public IEnumerable<string> SelectedSubscriptions
		{
			get
			{
				var result = new List<string>();

				this.Invoke(() =>
				{
					if (SubscriptionList.CheckedItems.Count > 0)
					{
						result.AddRange(from ListViewItem item in SubscriptionList.CheckedItems
										select item.Text);
					}
					else
					{
						result.AddRange(from ListViewItem item in SubscriptionList.SelectedItems
										select item.Text);
					}
				});

				return result;
			}
		}

		public void AddSubscription(string inboxWorkQueueUri, string messageType)
		{
			this.Invoke(() =>
			            	{
			            		if (!InboxWorkQueueUri.ContainsQueue(inboxWorkQueueUri))
			            		{
			            			InboxWorkQueueUri.AddQueue(inboxWorkQueueUri);
			            		}

			            		if (!InboxWorkQueueUriValue.Equals(inboxWorkQueueUri)
			            		    ||
			            		    SubscriptionAlreadyPopulated(messageType))
			            		{
			            			return;
			            		}

			            		SubscriptionList.Items.Add(messageType);
			            	});
		}

		public void CheckAllSubscriptions()
		{
			this.Invoke(() =>
			            	{
			            		foreach (ListViewItem item in SubscriptionList.Items)
			            		{
			            			item.Checked = true;
			            		}
			            	}
				);
		}

		public void InvertSubscriptionChecks()
		{
			this.Invoke(() =>
			            	{
			            		foreach (ListViewItem item in SubscriptionList.Items)
			            		{
			            			item.Checked = !item.Checked;
			            		}
			            	}
				);
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

		public void PopulateEventMessageTypes(IEnumerable<Type> list)
		{
			this.Invoke(
				() =>
					{
						EventMessageTypeList.Items.Clear();

						foreach (var type in list)
						{
							EventMessageTypeList.Items.Add(type.FullName);
						}
					});
		}

		public void CheckAllEventMessageTypes()
		{
			this.Invoke(
				() =>
					{
						foreach (ListViewItem item in EventMessageTypeList.Items)
						{
							item.Checked = true;
						}
					});
		}

		public void InvertEventMessageTypeChecks()
		{
			this.Invoke(
				() =>
					{
						foreach (ListViewItem item in EventMessageTypeList.Items)
						{
							item.Checked = !item.Checked;
						}
					});
		}

		public void GetAssemblyFileName()
		{
			if (ofd.ShowDialog() != DialogResult.OK)
			{
				return;
			}
		}

		private void DataStore_SelectedIndexChanged(object sender, EventArgs e)
		{
			presenter.DataStoreChanged();
		}

		protected override void OnLoad(EventArgs e)
		{
			presenter.OnViewReady();
		}

        private void InboxWorkQueueUri_KeyUp(object sender, KeyEventArgs e)
        {
            e.OnEnterPressed(presenter.RefreshSubscriptions);
        }

        private void InboxWorkQueueUri_QueueSelected(object sender, QueueSelectedEventArgs e)
        {
            presenter.RefreshSubscriptions();
        }

	}
}