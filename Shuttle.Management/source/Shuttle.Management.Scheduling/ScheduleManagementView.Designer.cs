namespace Shuttle.Management.Scheduling
{
	partial class ScheduleManagementView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleManagementView));
            this.ScheduleList = new System.Windows.Forms.ListView();
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InboxWorkQueueUriColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CronExpressionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NextNotificationColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CronExpression = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ScheduleName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SchedulingToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolTipContainer = new System.Windows.Forms.ToolTip(this.components);
            this.DataStore = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EndpointInboxWorkQueueUri = new Shuttle.Management.Shell.QueueHierarchyView();
            this.SuspendLayout();
            // 
            // ScheduleList
            // 
            this.ScheduleList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScheduleList.CheckBoxes = true;
            this.ScheduleList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.InboxWorkQueueUriColumn,
            this.CronExpressionColumn,
            this.NextNotificationColumn});
            this.ScheduleList.FullRowSelect = true;
            this.ScheduleList.GridLines = true;
            this.ScheduleList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ScheduleList.HideSelection = false;
            this.ScheduleList.Location = new System.Drawing.Point(8, 56);
            this.ScheduleList.MultiSelect = false;
            this.ScheduleList.Name = "ScheduleList";
            this.ScheduleList.Size = new System.Drawing.Size(843, 247);
            this.ScheduleList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ScheduleList.TabIndex = 2;
            this.ScheduleList.UseCompatibleStateImageBehavior = false;
            this.ScheduleList.View = System.Windows.Forms.View.Details;
            this.ScheduleList.SelectedIndexChanged += new System.EventHandler(this.ScheduleList_SelectedIndexChanged);
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Name";
            this.NameColumn.Width = 199;
            // 
            // InboxWorkQueueUriColumn
            // 
            this.InboxWorkQueueUriColumn.DisplayIndex = 2;
            this.InboxWorkQueueUriColumn.Text = "Inbox Work Queue Uri";
            this.InboxWorkQueueUriColumn.Width = 252;
            // 
            // CronExpressionColumn
            // 
            this.CronExpressionColumn.DisplayIndex = 1;
            this.CronExpressionColumn.Text = "Cron Expression";
            this.CronExpressionColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CronExpressionColumn.Width = 159;
            // 
            // NextNotificationColumn
            // 
            this.NextNotificationColumn.Text = "Next Notification";
            this.NextNotificationColumn.Width = 131;
            // 
            // CronExpression
            // 
            this.CronExpression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CronExpression.Location = new System.Drawing.Point(8, 425);
            this.CronExpression.Name = "CronExpression";
            this.CronExpression.Size = new System.Drawing.Size(844, 20);
            this.CronExpression.TabIndex = 8;
            this.ToolTipContainer.SetToolTip(this.CronExpression, resources.GetString("CronExpression.ToolTip"));
            this.CronExpression.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Save);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 409);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Cron expression";
            // 
            // ScheduleName
            // 
            this.ScheduleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScheduleName.Location = new System.Drawing.Point(8, 329);
            this.ScheduleName.Name = "ScheduleName";
            this.ScheduleName.Size = new System.Drawing.Size(844, 20);
            this.ScheduleName.TabIndex = 4;
            this.ScheduleName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Save);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 313);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Schedule name";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 361);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Endpoint inbox work queue uri";
            // 
            // SchedulingToolStrip
            // 
            this.SchedulingToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SchedulingToolStrip.Location = new System.Drawing.Point(0, 453);
            this.SchedulingToolStrip.Name = "SchedulingToolStrip";
            this.SchedulingToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SchedulingToolStrip.Size = new System.Drawing.Size(858, 25);
            this.SchedulingToolStrip.TabIndex = 9;
            this.SchedulingToolStrip.Text = "toolStrip1";
            // 
            // DataStore
            // 
            this.DataStore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataStore.FormattingEnabled = true;
            this.DataStore.Location = new System.Drawing.Point(8, 24);
            this.DataStore.Name = "DataStore";
            this.DataStore.Size = new System.Drawing.Size(844, 21);
            this.DataStore.TabIndex = 1;
            this.DataStore.SelectedIndexChanged += new System.EventHandler(this.DataStore_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Data store";
            // 
            // EndpointInboxWorkQueueUri
            // 
            this.EndpointInboxWorkQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EndpointInboxWorkQueueUri.Location = new System.Drawing.Point(8, 378);
            this.EndpointInboxWorkQueueUri.Name = "EndpointInboxWorkQueueUri";
            this.EndpointInboxWorkQueueUri.ShowQueueButtonPosition = Shuttle.Management.Shell.ShowQueueButtonPosition.Left;
            this.EndpointInboxWorkQueueUri.Size = new System.Drawing.Size(843, 20);
            this.EndpointInboxWorkQueueUri.TabIndex = 6;
            this.EndpointInboxWorkQueueUri.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Save);
            // 
            // ScheduleManagementView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EndpointInboxWorkQueueUri);
            this.Controls.Add(this.DataStore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SchedulingToolStrip);
            this.Controls.Add(this.CronExpression);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ScheduleName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ScheduleList);
            this.Name = "ScheduleManagementView";
            this.Size = new System.Drawing.Size(858, 478);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView ScheduleList;
		private System.Windows.Forms.ColumnHeader NameColumn;
		private System.Windows.Forms.ColumnHeader InboxWorkQueueUriColumn;
		private System.Windows.Forms.ColumnHeader CronExpressionColumn;
		private System.Windows.Forms.ColumnHeader NextNotificationColumn;
		private System.Windows.Forms.TextBox CronExpression;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox ScheduleName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ToolStrip SchedulingToolStrip;
		private System.Windows.Forms.ToolTip ToolTipContainer;
		private System.Windows.Forms.ComboBox DataStore;
        private System.Windows.Forms.Label label3;
        private Shell.QueueHierarchyView EndpointInboxWorkQueueUri;

	}
}
