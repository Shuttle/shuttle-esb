namespace Shuttle.Management.Shell
{
    partial class QueueHierarchyView
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
            this.SelectedQueueUri = new System.Windows.Forms.TextBox();
            this.ShowQueuesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectedQueueUri
            // 
            this.SelectedQueueUri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectedQueueUri.Location = new System.Drawing.Point(0, 0);
            this.SelectedQueueUri.Name = "SelectedQueueUri";
            this.SelectedQueueUri.Size = new System.Drawing.Size(251, 20);
            this.SelectedQueueUri.TabIndex = 1;
            this.SelectedQueueUri.Click += new System.EventHandler(this.SelectedQueueUri_Click);
            this.SelectedQueueUri.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SelectedQueueUri_KeyUp);
            // 
            // ShowQueuesButton
            // 
            this.ShowQueuesButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.ShowQueuesButton.Location = new System.Drawing.Point(251, 0);
            this.ShowQueuesButton.Name = "ShowQueuesButton";
            this.ShowQueuesButton.Size = new System.Drawing.Size(28, 20);
            this.ShowQueuesButton.TabIndex = 2;
            this.ShowQueuesButton.Text = "...";
            this.ShowQueuesButton.UseVisualStyleBackColor = true;
            this.ShowQueuesButton.Visible = false;
            // 
            // QueueHierarchyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SelectedQueueUri);
            this.Controls.Add(this.ShowQueuesButton);
            this.Name = "QueueHierarchyView";
            this.Size = new System.Drawing.Size(279, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SelectedQueueUri;
        private System.Windows.Forms.Button ShowQueuesButton;
    }
}
