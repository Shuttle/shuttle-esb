namespace Shuttle.Management.Subscriptions
{
    partial class SubscriptionManagementView
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
            this.SubscriptionSplit = new System.Windows.Forms.SplitContainer();
            this.InboxWorkQueueUri = new Shuttle.Management.Shell.QueueHierarchyView();
            this.DataStore = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SubscriptionToolStrip = new System.Windows.Forms.ToolStrip();
            this.SubscriptionList = new System.Windows.Forms.ListView();
            this.MessageTypeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AcceptedByColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AcceptedDateColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.EventMessageTypeToolStrip = new System.Windows.Forms.ToolStrip();
            this.EventMessageTypeList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.SubscriptionSplit.Panel1.SuspendLayout();
            this.SubscriptionSplit.Panel2.SuspendLayout();
            this.SubscriptionSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // SubscriptionSplit
            // 
            this.SubscriptionSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubscriptionSplit.Location = new System.Drawing.Point(0, 0);
            this.SubscriptionSplit.Name = "SubscriptionSplit";
            this.SubscriptionSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SubscriptionSplit.Panel1
            // 
            this.SubscriptionSplit.Panel1.Controls.Add(this.InboxWorkQueueUri);
            this.SubscriptionSplit.Panel1.Controls.Add(this.DataStore);
            this.SubscriptionSplit.Panel1.Controls.Add(this.label1);
            this.SubscriptionSplit.Panel1.Controls.Add(this.SubscriptionToolStrip);
            this.SubscriptionSplit.Panel1.Controls.Add(this.SubscriptionList);
            this.SubscriptionSplit.Panel1.Controls.Add(this.label4);
            // 
            // SubscriptionSplit.Panel2
            // 
            this.SubscriptionSplit.Panel2.Controls.Add(this.EventMessageTypeToolStrip);
            this.SubscriptionSplit.Panel2.Controls.Add(this.EventMessageTypeList);
            this.SubscriptionSplit.Size = new System.Drawing.Size(749, 567);
            this.SubscriptionSplit.SplitterDistance = 349;
            this.SubscriptionSplit.TabIndex = 0;
            // 
            // InboxWorkQueueUri
            // 
            this.InboxWorkQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InboxWorkQueueUri.Location = new System.Drawing.Point(8, 72);
            this.InboxWorkQueueUri.Name = "InboxWorkQueueUri";
            this.InboxWorkQueueUri.ShowQueueButtonPosition = Shuttle.Management.Shell.ShowQueueButtonPosition.Left;
            this.InboxWorkQueueUri.Size = new System.Drawing.Size(733, 20);
            this.InboxWorkQueueUri.TabIndex = 3;
            this.InboxWorkQueueUri.QueueSelected += new System.EventHandler<Shuttle.Management.Shell.QueueSelectedEventArgs>(this.InboxWorkQueueUri_QueueSelected);
            this.InboxWorkQueueUri.KeyUp += new System.Windows.Forms.KeyEventHandler(this.InboxWorkQueueUri_KeyUp);
            // 
            // DataStore
            // 
            this.DataStore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataStore.FormattingEnabled = true;
            this.DataStore.Location = new System.Drawing.Point(8, 24);
            this.DataStore.Name = "DataStore";
            this.DataStore.Size = new System.Drawing.Size(733, 21);
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
            // SubscriptionToolStrip
            // 
            this.SubscriptionToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SubscriptionToolStrip.Location = new System.Drawing.Point(0, 324);
            this.SubscriptionToolStrip.Name = "SubscriptionToolStrip";
            this.SubscriptionToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SubscriptionToolStrip.Size = new System.Drawing.Size(749, 25);
            this.SubscriptionToolStrip.TabIndex = 5;
            this.SubscriptionToolStrip.Text = "toolStrip2";
            // 
            // SubscriptionList
            // 
            this.SubscriptionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SubscriptionList.CheckBoxes = true;
            this.SubscriptionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MessageTypeColumn,
            this.AcceptedByColumn,
            this.AcceptedDateColumn});
            this.SubscriptionList.FullRowSelect = true;
            this.SubscriptionList.GridLines = true;
            this.SubscriptionList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SubscriptionList.HideSelection = false;
            this.SubscriptionList.Location = new System.Drawing.Point(8, 104);
            this.SubscriptionList.MultiSelect = false;
            this.SubscriptionList.Name = "SubscriptionList";
            this.SubscriptionList.Size = new System.Drawing.Size(733, 208);
            this.SubscriptionList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.SubscriptionList.TabIndex = 4;
            this.SubscriptionList.UseCompatibleStateImageBehavior = false;
            this.SubscriptionList.View = System.Windows.Forms.View.Details;
            // 
            // MessageTypeColumn
            // 
            this.MessageTypeColumn.Text = "Event message types subscribed to";
            this.MessageTypeColumn.Width = 463;
            // 
            // AcceptedByColumn
            // 
            this.AcceptedByColumn.DisplayIndex = 2;
            this.AcceptedByColumn.Text = "Accepted by";
            this.AcceptedByColumn.Width = 99;
            // 
            // AcceptedDateColumn
            // 
            this.AcceptedDateColumn.DisplayIndex = 1;
            this.AcceptedDateColumn.Text = "Accepted date";
            this.AcceptedDateColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AcceptedDateColumn.Width = 125;
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
            // EventMessageTypeToolStrip
            // 
            this.EventMessageTypeToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.EventMessageTypeToolStrip.Location = new System.Drawing.Point(0, 189);
            this.EventMessageTypeToolStrip.Name = "EventMessageTypeToolStrip";
            this.EventMessageTypeToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.EventMessageTypeToolStrip.Size = new System.Drawing.Size(749, 25);
            this.EventMessageTypeToolStrip.TabIndex = 1;
            this.EventMessageTypeToolStrip.Text = "toolStrip1";
            // 
            // EventMessageTypeList
            // 
            this.EventMessageTypeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EventMessageTypeList.CheckBoxes = true;
            this.EventMessageTypeList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.EventMessageTypeList.FullRowSelect = true;
            this.EventMessageTypeList.GridLines = true;
            this.EventMessageTypeList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.EventMessageTypeList.HideSelection = false;
            this.EventMessageTypeList.Location = new System.Drawing.Point(8, 8);
            this.EventMessageTypeList.MultiSelect = false;
            this.EventMessageTypeList.Name = "EventMessageTypeList";
            this.EventMessageTypeList.Size = new System.Drawing.Size(733, 170);
            this.EventMessageTypeList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.EventMessageTypeList.TabIndex = 0;
            this.EventMessageTypeList.UseCompatibleStateImageBehavior = false;
            this.EventMessageTypeList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Event message types to add";
            this.columnHeader1.Width = 677;
            // 
            // ofd
            // 
            this.ofd.DefaultExt = "dll";
            this.ofd.Filter = "Assembly Files (*.dll; *.exe)|*.dll;*.exe|All Files (*.*)|*.*";
            // 
            // SubscriptionManagementView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SubscriptionSplit);
            this.Name = "SubscriptionManagementView";
            this.Size = new System.Drawing.Size(749, 567);
            this.SubscriptionSplit.Panel1.ResumeLayout(false);
            this.SubscriptionSplit.Panel1.PerformLayout();
            this.SubscriptionSplit.Panel2.ResumeLayout(false);
            this.SubscriptionSplit.Panel2.PerformLayout();
            this.SubscriptionSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SubscriptionSplit;
		private System.Windows.Forms.ListView SubscriptionList;
		private System.Windows.Forms.ColumnHeader MessageTypeColumn;
		private System.Windows.Forms.ColumnHeader AcceptedByColumn;
		private System.Windows.Forms.ColumnHeader AcceptedDateColumn;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListView EventMessageTypeList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ToolStrip SubscriptionToolStrip;
		private System.Windows.Forms.ToolStrip EventMessageTypeToolStrip;
		private System.Windows.Forms.ComboBox DataStore;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.OpenFileDialog ofd;
        private Shell.QueueHierarchyView InboxWorkQueueUri;
    }
}