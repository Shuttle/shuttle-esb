using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
    partial class MessageManagementView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SplitInput = new System.Windows.Forms.SplitContainer();
            this.DestinationQueueUri = new Shuttle.Management.Shell.QueueHierarchyView();
            this.SourceQueueUri = new Shuttle.Management.Shell.QueueHierarchyView();
            this.label3 = new System.Windows.Forms.Label();
            this.FetchCount = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MessageToolStrip = new System.Windows.Forms.ToolStrip();
            this.MessageList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MessageView = new Shuttle.Management.Messages.MessageView();
            this.SplitInput.Panel1.SuspendLayout();
            this.SplitInput.Panel2.SuspendLayout();
            this.SplitInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitInput
            // 
            this.SplitInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitInput.Location = new System.Drawing.Point(0, 0);
            this.SplitInput.Name = "SplitInput";
            this.SplitInput.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitInput.Panel1
            // 
            this.SplitInput.Panel1.Controls.Add(this.DestinationQueueUri);
            this.SplitInput.Panel1.Controls.Add(this.SourceQueueUri);
            this.SplitInput.Panel1.Controls.Add(this.label3);
            this.SplitInput.Panel1.Controls.Add(this.FetchCount);
            this.SplitInput.Panel1.Controls.Add(this.label5);
            this.SplitInput.Panel1.Controls.Add(this.label4);
            this.SplitInput.Panel1.Controls.Add(this.MessageToolStrip);
            this.SplitInput.Panel1.Controls.Add(this.MessageList);
            // 
            // SplitInput.Panel2
            // 
            this.SplitInput.Panel2.Controls.Add(this.MessageView);
            this.SplitInput.Size = new System.Drawing.Size(719, 560);
            this.SplitInput.SplitterDistance = 293;
            this.SplitInput.TabIndex = 0;
            // 
            // DestinationQueueUri
            // 
            this.DestinationQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationQueueUri.Location = new System.Drawing.Point(8, 234);
            this.DestinationQueueUri.Name = "DestinationQueueUri";
            this.DestinationQueueUri.ShowQueueButtonPosition = Shuttle.Management.Shell.ShowQueueButtonPosition.Left;
            this.DestinationQueueUri.Size = new System.Drawing.Size(697, 20);
            this.DestinationQueueUri.TabIndex = 6;
            // 
            // SourceQueueUri
            // 
            this.SourceQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceQueueUri.Location = new System.Drawing.Point(11, 24);
            this.SourceQueueUri.Name = "SourceQueueUri";
            this.SourceQueueUri.ShowQueueButtonPosition = Shuttle.Management.Shell.ShowQueueButtonPosition.Left;
            this.SourceQueueUri.Size = new System.Drawing.Size(596, 20);
            this.SourceQueueUri.TabIndex = 1;
            this.SourceQueueUri.QueueSelected += new System.EventHandler<Shuttle.Management.Shell.QueueSelectedEventArgs>(this.SourceQueueUri_QueueSelected);
            this.SourceQueueUri.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SourceQueueUri_KeyUp);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Destination queue uri";
            // 
            // FetchCount
            // 
            this.FetchCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FetchCount.DisplayMember = "Uri";
            this.FetchCount.FormattingEnabled = true;
            this.FetchCount.Location = new System.Drawing.Point(613, 24);
            this.FetchCount.Name = "FetchCount";
            this.FetchCount.Size = new System.Drawing.Size(92, 21);
            this.FetchCount.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(613, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Fetch count";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Source queue uri";
            // 
            // MessageToolStrip
            // 
            this.MessageToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MessageToolStrip.Location = new System.Drawing.Point(0, 268);
            this.MessageToolStrip.Name = "MessageToolStrip";
            this.MessageToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MessageToolStrip.Size = new System.Drawing.Size(719, 25);
            this.MessageToolStrip.TabIndex = 7;
            this.MessageToolStrip.Text = "toolStrip1";
            // 
            // MessageList
            // 
            this.MessageList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageList.CheckBoxes = true;
            this.MessageList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.MessageList.FullRowSelect = true;
            this.MessageList.GridLines = true;
            this.MessageList.HideSelection = false;
            this.MessageList.Location = new System.Drawing.Point(8, 56);
            this.MessageList.Name = "MessageList";
            this.MessageList.Size = new System.Drawing.Size(697, 157);
            this.MessageList.TabIndex = 4;
            this.MessageList.UseCompatibleStateImageBehavior = false;
            this.MessageList.View = System.Windows.Forms.View.Details;
            this.MessageList.SelectedIndexChanged += new System.EventHandler(this.MessageList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Message id";
            this.columnHeader1.Width = 308;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Assembly Qualified Name";
            this.columnHeader2.Width = 674;
            // 
            // MessageView
            // 
            this.MessageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageView.Location = new System.Drawing.Point(0, 0);
            this.MessageView.Name = "MessageView";
            this.MessageView.Size = new System.Drawing.Size(719, 263);
            this.MessageView.TabIndex = 0;
            // 
            // MessageManagementView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SplitInput);
            this.Name = "MessageManagementView";
            this.Size = new System.Drawing.Size(719, 560);
            this.SplitInput.Panel1.ResumeLayout(false);
            this.SplitInput.Panel1.PerformLayout();
            this.SplitInput.Panel2.ResumeLayout(false);
            this.SplitInput.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox FetchCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStrip MessageToolStrip;
        private System.Windows.Forms.ListView MessageList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private MessageView MessageView;
        private QueueHierarchyView SourceQueueUri;
        private QueueHierarchyView DestinationQueueUri;
    }
}

