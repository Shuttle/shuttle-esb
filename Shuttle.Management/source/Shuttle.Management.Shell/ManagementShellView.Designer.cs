namespace Shuttle.Management.Shell
{
    partial class ManagementShellView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagementShellView));
			this.PresenterStrip = new System.Windows.Forms.ToolStrip();
			this.SplitManagementView = new System.Windows.Forms.SplitContainer();
			this.LogContainer = new System.Windows.Forms.RichTextBox();
			this.SplitManagementView.Panel2.SuspendLayout();
			this.SplitManagementView.SuspendLayout();
			this.SuspendLayout();
			// 
			// PresenterStrip
			// 
			this.PresenterStrip.Location = new System.Drawing.Point(0, 0);
			this.PresenterStrip.Name = "PresenterStrip";
			this.PresenterStrip.Size = new System.Drawing.Size(682, 25);
			this.PresenterStrip.TabIndex = 0;
			this.PresenterStrip.Text = "toolStrip1";
			// 
			// SplitManagementView
			// 
			this.SplitManagementView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplitManagementView.Location = new System.Drawing.Point(0, 25);
			this.SplitManagementView.Name = "SplitManagementView";
			this.SplitManagementView.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// SplitManagementView.Panel2
			// 
			this.SplitManagementView.Panel2.Controls.Add(this.LogContainer);
			this.SplitManagementView.Size = new System.Drawing.Size(682, 479);
			this.SplitManagementView.SplitterDistance = 374;
			this.SplitManagementView.TabIndex = 1;
			// 
			// LogContainer
			// 
			this.LogContainer.BackColor = System.Drawing.Color.Black;
			this.LogContainer.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.LogContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LogContainer.Location = new System.Drawing.Point(0, 0);
			this.LogContainer.Name = "LogContainer";
			this.LogContainer.ReadOnly = true;
			this.LogContainer.Size = new System.Drawing.Size(682, 101);
			this.LogContainer.TabIndex = 0;
			this.LogContainer.TabStop = false;
			this.LogContainer.Text = "";
			// 
			// ManagementShellView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(682, 504);
			this.Controls.Add(this.SplitManagementView);
			this.Controls.Add(this.PresenterStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ManagementShellView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Shuttle Management Shell";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManagementShellView_FormClosing);
			this.Load += new System.EventHandler(this.ManagementShellView_Load);
			this.SplitManagementView.Panel2.ResumeLayout(false);
			this.SplitManagementView.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip PresenterStrip;
		private System.Windows.Forms.SplitContainer SplitManagementView;
		private System.Windows.Forms.RichTextBox LogContainer;
    }
}

