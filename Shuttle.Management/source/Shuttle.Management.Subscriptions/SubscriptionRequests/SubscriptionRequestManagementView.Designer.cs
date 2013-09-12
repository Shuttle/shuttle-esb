namespace Shuttle.Management.Subscriptions
{
	partial class SubscriptionRequestManagementView
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label4 = new System.Windows.Forms.Label();
            this.RequestList = new System.Windows.Forms.ListView();
            this.MessageTypeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.DeclineReason = new System.Windows.Forms.TextBox();
            this.RequestToolStrip = new System.Windows.Forms.ToolStrip();
            this.DataStore = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InboxWorkQueueUri = new Shuttle.Management.Shell.QueueHierarchyView();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Subscriber inbox work queue uri";
            // 
            // RequestList
            // 
            this.RequestList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestList.CheckBoxes = true;
            this.RequestList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MessageTypeColumn,
            this.columnHeader1,
            this.columnHeader2});
            this.RequestList.FullRowSelect = true;
            this.RequestList.GridLines = true;
            this.RequestList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.RequestList.Location = new System.Drawing.Point(8, 104);
            this.RequestList.MultiSelect = false;
            this.RequestList.Name = "RequestList";
            this.RequestList.ShowItemToolTips = true;
            this.RequestList.Size = new System.Drawing.Size(698, 248);
            this.RequestList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.RequestList.TabIndex = 4;
            this.RequestList.UseCompatibleStateImageBehavior = false;
            this.RequestList.View = System.Windows.Forms.View.Details;
            // 
            // MessageTypeColumn
            // 
            this.MessageTypeColumn.Text = "Message Type";
            this.MessageTypeColumn.Width = 405;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Declined by";
            this.columnHeader1.Width = 103;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Declined date";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 140;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 360);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Decline reason";
            // 
            // DeclineReason
            // 
            this.DeclineReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeclineReason.Location = new System.Drawing.Point(8, 376);
            this.DeclineReason.Multiline = true;
            this.DeclineReason.Name = "DeclineReason";
            this.DeclineReason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DeclineReason.Size = new System.Drawing.Size(698, 48);
            this.DeclineReason.TabIndex = 6;
            // 
            // RequestToolStrip
            // 
            this.RequestToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RequestToolStrip.Location = new System.Drawing.Point(0, 438);
            this.RequestToolStrip.Name = "RequestToolStrip";
            this.RequestToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RequestToolStrip.Size = new System.Drawing.Size(714, 25);
            this.RequestToolStrip.TabIndex = 7;
            this.RequestToolStrip.Text = "toolStrip1";
            // 
            // DataStore
            // 
            this.DataStore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataStore.FormattingEnabled = true;
            this.DataStore.Location = new System.Drawing.Point(8, 24);
            this.DataStore.Name = "DataStore";
            this.DataStore.Size = new System.Drawing.Size(698, 21);
            this.DataStore.TabIndex = 1;
            this.DataStore.SelectedIndexChanged += new System.EventHandler(this.DataStore_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data store";
            // 
            // InboxWorkQueueUri
            // 
            this.InboxWorkQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InboxWorkQueueUri.Location = new System.Drawing.Point(8, 73);
            this.InboxWorkQueueUri.Name = "InboxWorkQueueUri";
            this.InboxWorkQueueUri.ShowQueueButtonPosition = Shuttle.Management.Shell.ShowQueueButtonPosition.Left;
            this.InboxWorkQueueUri.Size = new System.Drawing.Size(698, 20);
            this.InboxWorkQueueUri.TabIndex = 3;
            this.InboxWorkQueueUri.QueueSelected += new System.EventHandler<Shuttle.Management.Shell.QueueSelectedEventArgs>(this.InboxWorkQueueUri_QueueSelected);
            this.InboxWorkQueueUri.KeyUp += new System.Windows.Forms.KeyEventHandler(this.InboxWorkQueueUri_KeyUp);
            // 
            // SubscriptionRequestManagementView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.InboxWorkQueueUri);
            this.Controls.Add(this.DataStore);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RequestToolStrip);
            this.Controls.Add(this.DeclineReason);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RequestList);
            this.Controls.Add(this.label4);
            this.Name = "SubscriptionRequestManagementView";
            this.Size = new System.Drawing.Size(714, 463);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListView RequestList;
		private System.Windows.Forms.ColumnHeader MessageTypeColumn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox DeclineReason;
		private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ToolStrip RequestToolStrip;
		private System.Windows.Forms.ComboBox DataStore;
		private System.Windows.Forms.Label label1;
        private Shell.QueueHierarchyView InboxWorkQueueUri;
	}
}