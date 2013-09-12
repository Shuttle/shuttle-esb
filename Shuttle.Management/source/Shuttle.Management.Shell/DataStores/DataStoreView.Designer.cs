namespace Shuttle.Management.Shell
{
    partial class DataStoreView
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
			this.StoreName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ConnectionString = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.ProviderName = new System.Windows.Forms.ComboBox();
			this.DataStoreToolStrip = new System.Windows.Forms.ToolStrip();
			this.DataStoreList = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 328);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Name";
			// 
			// StoreName
			// 
			this.StoreName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.StoreName.Location = new System.Drawing.Point(8, 344);
			this.StoreName.Name = "StoreName";
			this.StoreName.Size = new System.Drawing.Size(608, 20);
			this.StoreName.TabIndex = 2;
			this.StoreName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Save);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 376);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Connection string";
			// 
			// ConnectionString
			// 
			this.ConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectionString.Location = new System.Drawing.Point(8, 392);
			this.ConnectionString.Name = "ConnectionString";
			this.ConnectionString.Size = new System.Drawing.Size(608, 20);
			this.ConnectionString.TabIndex = 4;
			this.ConnectionString.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Save);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 424);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(75, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Provider name";
			// 
			// ProviderName
			// 
			this.ProviderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ProviderName.FormattingEnabled = true;
			this.ProviderName.Location = new System.Drawing.Point(8, 440);
			this.ProviderName.Name = "ProviderName";
			this.ProviderName.Size = new System.Drawing.Size(608, 21);
			this.ProviderName.TabIndex = 6;
			this.ProviderName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Save);
			// 
			// DataStoreToolStrip
			// 
			this.DataStoreToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.DataStoreToolStrip.Location = new System.Drawing.Point(0, 474);
			this.DataStoreToolStrip.Name = "DataStoreToolStrip";
			this.DataStoreToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.DataStoreToolStrip.Size = new System.Drawing.Size(627, 25);
			this.DataStoreToolStrip.TabIndex = 7;
			this.DataStoreToolStrip.Text = "toolStrip1";
			// 
			// DataStoreList
			// 
			this.DataStoreList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.DataStoreList.CheckBoxes = true;
			this.DataStoreList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.DataStoreList.FullRowSelect = true;
			this.DataStoreList.GridLines = true;
			this.DataStoreList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.DataStoreList.HideSelection = false;
			this.DataStoreList.Location = new System.Drawing.Point(8, 8);
			this.DataStoreList.Name = "DataStoreList";
			this.DataStoreList.Size = new System.Drawing.Size(608, 304);
			this.DataStoreList.TabIndex = 0;
			this.DataStoreList.TabStop = false;
			this.DataStoreList.UseCompatibleStateImageBehavior = false;
			this.DataStoreList.View = System.Windows.Forms.View.Details;
			this.DataStoreList.SelectedIndexChanged += new System.EventHandler(this.DataStoreList_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 120;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Connection string";
			this.columnHeader2.Width = 300;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Provider name";
			this.columnHeader3.Width = 150;
			// 
			// DataStoreView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DataStoreList);
			this.Controls.Add(this.DataStoreToolStrip);
			this.Controls.Add(this.ProviderName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ConnectionString);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.StoreName);
			this.Name = "DataStoreView";
			this.Size = new System.Drawing.Size(627, 499);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox StoreName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ConnectionString;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ProviderName;
		private System.Windows.Forms.ToolStrip DataStoreToolStrip;
		private System.Windows.Forms.ListView DataStoreList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}