using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shuttle.ESB.Core;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
	public partial class MessageManagementView : UserControl, IMessageManagementView
	{
		private readonly MessageManagementPresenter presenter;

		public MessageManagementView(MessageManagementPresenter presenter)
		{
			InitializeComponent();

			this.presenter = presenter;

			MessageToolStrip.AddItem(ManagementResources.Text_Remove,
			                         ManagementResources.Image_Remove,
			                         delegate { presenter.Remove(); });
			MessageToolStrip.AddItem(MessageResources.Text_StopIgnoring,
									 MessageResources.Image_StopIgnoring,
									 delegate { presenter.StopIgnoring(); });
			MessageToolStrip.AddItem(ManagementResources.Text_Move,
			                         ManagementResources.Image_ArrowDown,
			                         delegate { presenter.Move(); });
			MessageToolStrip.AddItem(MessageResources.Text_ReturnToSourceQueue,
			                         MessageResources.Image_ArrowBack,
			                         delegate { presenter.ReturnToSourceQueue(); });
			MessageToolStrip.AddItem(ManagementResources.Text_CheckAll,
			                         delegate { presenter.CheckAll(); });

			MessageToolStrip.AddItem(ManagementResources.Text_InvertChecks,
			                         delegate { presenter.InvertChecks(); });
			MessageToolStrip.AddItem(MessageResources.Text_RefreshQueues,
			                         ManagementResources.Image_Queues,
			                         delegate { presenter.RefreshQueues(); });
			MessageToolStrip.AddItem(MessageResources.Text_RefreshMessages,
			                         ManagementResources.Image_RefreshSmall,
			                         delegate { presenter.RefreshQueue(); });
		}

		protected override void OnLoad(EventArgs e)
		{
			presenter.OnViewReady();
		}

		public string SourceQueueUriValue
		{
			get { return SourceQueueUri.Text; }
			set { SourceQueueUri.Text = value; }
		}

		public string DestinationQueueUriValue
		{
			get { return DestinationQueueUri.Text; }
			set { DestinationQueueUri.Text = value; }
		}

		public int FetchCountValue
		{
			get
			{
				int count;

				if (!int.TryParse(FetchCount.Text, out count))
				{
					count = 0;
				}

				return count;
			}
			set { FetchCount.Text = Convert.ToString(value); }
		}

		public bool HasSelectedMessages
		{
			get { return MessageList.SelectedItems.Count > 0 || MessageList.CheckedItems.Count > 0; }
		}

		public IEnumerable<TransportMessage> SelectedMessages
		{
			get
			{
				var result = new List<TransportMessage>();

				this.Invoke(() =>
				            	{
				            		if (MessageList.CheckedItems.Count > 0)
				            		{
				            			result.AddRange(from ListViewItem item in MessageList.CheckedItems
				            			                select (TransportMessage) item.Tag);
				            		}
				            		else
				            		{
				            			result.AddRange(from ListViewItem item in MessageList.SelectedItems
				            			                select (TransportMessage) item.Tag);
				            		}
				            	});

				return result;
			}
		}

		public void PopulateQueues(IEnumerable<Queue> queues)
		{
			this.Invoke(() =>
			            	{
			            		SourceQueueUri.Clear();
			            		DestinationQueueUri.Clear();

			            		foreach (var queue in queues)
			            		{
                                    SourceQueueUri.AddQueue(queue.Uri);
			            			DestinationQueueUri.AddQueue(queue.Uri);
			            		}
			            	});
		}

		public void AddFetchCount(int count)
		{
			FetchCount.Items.Add(count);
		}

		public void PopulateMessages(IEnumerable<TransportMessage> messages)
		{
			this.Invoke(() =>
			            	{
			            		ClearMessages();

			            		if (messages == null)
			            		{
			            			return;
			            		}

			            		foreach (var message in messages)
			            		{
			            			var li = MessageList.Items.Add(message.MessageId.ToString());

			            			li.SubItems.Add(message.AssemblyQualifiedName);

			            			li.Tag = message;
			            		}
			            	});
		}

		public void ClearMessages()
		{
			this.Invoke(() =>
			            	{
			            		MessageList.Items.Clear();

			            		MessageView.Clear();
			            	});
		}

		public TransportMessage SelectedTransportMessage()
		{
			if (!HasSelectedMessages)
			{
				return null;
			}

			return (TransportMessage) MessageList.SelectedItems[0].Tag;
		}

		public void ShowMessage(TransportMessage transportMessage, object message)
		{
			this.Invoke(() => MessageView.Show(transportMessage, message));
		}

		public void ClearMessageView()
		{
			MessageView.Clear();
		}

		public void CheckAll()
		{
			this.Invoke(() =>
			            	{
			            		foreach (ListViewItem item in MessageList.Items)
			            		{
			            			item.Checked = true;
			            		}
			            	});
		}

		public void InvertChecks()
		{
			this.Invoke(() =>
			            	{
			            		foreach (ListViewItem item in MessageList.Items)
			            		{
			            			item.Checked = !item.Checked;
			            		}
			            	});
		}

		private void MessageList_SelectedIndexChanged(object sender, EventArgs e)
		{
			presenter.MessageSelected();
		}

        private void SourceQueueUri_QueueSelected(object sender, QueueSelectedEventArgs e)
        {
            presenter.RefreshQueue();
        }

        private void SourceQueueUri_KeyUp(object sender, KeyEventArgs e)
        {
            e.OnEnterPressed(presenter.RefreshQueue);
        }
	}
}