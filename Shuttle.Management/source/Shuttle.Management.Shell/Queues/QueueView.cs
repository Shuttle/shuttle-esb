using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Shuttle.Management.Shell
{
	public partial class QueueView : UserControl, IQueueView
	{
		private readonly IQueuePresenter presenter;

		public QueueView(IQueuePresenter presenter)
		{
			InitializeComponent();

			this.presenter = presenter;

			QueueToolStrip.AddItem(ManagementResources.Text_Remove,
								   ManagementResources.Image_Remove,
								   delegate { presenter.Remove(); });
			QueueToolStrip.AddItem(ManagementResources.Text_Save,
								   ManagementResources.Image_Save,
								   delegate { presenter.Save(); });
			QueueToolStrip.AddItem(ManagementResources.Text_CheckAll,
								   delegate { presenter.CheckAll(); });
			QueueToolStrip.AddItem(ManagementResources.Text_InvertChecks,
								   delegate { presenter.InvertChecks(); });
			QueueToolStrip.AddItem(ManagementResources.Text_Refresh,
								   ManagementResources.Image_RefreshSmall,
								   delegate { presenter.Refresh(); });
		}

		public string UriValue
		{
			get { return Uri.Text; }
			set { Uri.Text = value; }
		}

		public bool HasSelectedQueues
		{
			get { return QueueList.SelectedItems.Count > 0 || QueueList.CheckedItems.Count > 0; }
		}

		public void CheckAll()
		{
			this.Invoke(() =>
							{
								foreach (ListViewItem item in QueueList.Items)
								{
									item.Checked = true;
								}
							});
		}

		public void InvertChecks()
		{
			this.Invoke(() =>
							{
								foreach (ListViewItem item in QueueList.Items)
								{
									item.Checked = !item.Checked;
								}
							});
		}

		public void RefreshQueues(IEnumerable<Queue> list)
		{
			this.Invoke(
				() =>
				{
					QueueList.Items.Clear();

					foreach (var queue in list)
					{
						QueueList.Items.Add(queue.Uri).Tag = queue;
					}
				});
		}

		public IEnumerable<Queue> SelectedQueues()
		{
			var result = new List<Queue>();

			this.Invoke(
				() =>
				{
					if (QueueList.CheckedItems.Count > 0)
					{
						result.AddRange(from ListViewItem item in QueueList.CheckedItems
										select (Queue)item.Tag);
					}
					else
					{
						result.AddRange(from ListViewItem item in QueueList.SelectedItems
										select (Queue)item.Tag);
					}
				});

			return result;
		}

		public string GetSelectedQueueUri()
		{
			if (QueueList.SelectedItems.Count == 0)
			{
				return null;
			}

			return QueueList.SelectedItems[0].Text;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			presenter.OnViewReady();
		}

		private void QueueList_SelectedIndexChanged(object sender, EventArgs e)
		{
			presenter.QueueSelected();
		}

		private void Uri_KeyUp(object sender, KeyEventArgs e)
		{
			e.OnEnterPressed(presenter.Save);
		}
	}
}