namespace Shuttle.Management.Shell
{
    partial class QueueView
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
			this.label1 = new System.Windows.Forms.Label();
			this.Uri = new System.Windows.Forms.TextBox();
			this.QueueToolStrip = new System.Windows.Forms.ToolStrip();
			this.QueueList = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 408);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(20, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Uri";
			// 
			// Uri
			// 
			this.Uri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.Uri.Location = new System.Drawing.Point(8, 424);
			this.Uri.Name = "Uri";
			this.Uri.Size = new System.Drawing.Size(584, 20);
			this.Uri.TabIndex = 2;
			this.Uri.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Uri_KeyUp);
			// 
			// QueueToolStrip
			// 
			this.QueueToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.QueueToolStrip.Location = new System.Drawing.Point(0, 459);
			this.QueueToolStrip.Name = "QueueToolStrip";
			this.QueueToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.QueueToolStrip.Size = new System.Drawing.Size(603, 25);
			this.QueueToolStrip.TabIndex = 3;
			this.QueueToolStrip.Text = "toolStrip1";
			// 
			// QueueList
			// 
			this.QueueList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.QueueList.CheckBoxes = true;
			this.QueueList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.QueueList.FullRowSelect = true;
			this.QueueList.GridLines = true;
			this.QueueList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.QueueList.HideSelection = false;
			this.QueueList.Location = new System.Drawing.Point(8, 8);
			this.QueueList.Name = "QueueList";
			this.QueueList.Size = new System.Drawing.Size(584, 384);
			this.QueueList.TabIndex = 0;
			this.QueueList.TabStop = false;
			this.QueueList.UseCompatibleStateImageBehavior = false;
			this.QueueList.View = System.Windows.Forms.View.Details;
			this.QueueList.SelectedIndexChanged += new System.EventHandler(this.QueueList_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Uri";
			this.columnHeader1.Width = 555;
			// 
			// QueueView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.QueueList);
			this.Controls.Add(this.QueueToolStrip);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Uri);
			this.Name = "QueueView";
			this.Size = new System.Drawing.Size(603, 484);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Uri;
		private System.Windows.Forms.ToolStrip QueueToolStrip;
		private System.Windows.Forms.ListView QueueList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
