namespace Shuttle.Management.Messages
{
    partial class MessageView
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
			this.PropertyValueList = new System.Windows.Forms.ListView();
			this.PropertyColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ValueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// PropertyValueList
			// 
			this.PropertyValueList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PropertyColumn,
            this.ValueColumn});
			this.PropertyValueList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PropertyValueList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PropertyValueList.FullRowSelect = true;
			this.PropertyValueList.GridLines = true;
			this.PropertyValueList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.PropertyValueList.Location = new System.Drawing.Point(0, 0);
			this.PropertyValueList.Name = "PropertyValueList";
			this.PropertyValueList.Size = new System.Drawing.Size(487, 429);
			this.PropertyValueList.TabIndex = 0;
			this.PropertyValueList.UseCompatibleStateImageBehavior = false;
			this.PropertyValueList.View = System.Windows.Forms.View.Details;
			this.PropertyValueList.SelectedIndexChanged += new System.EventHandler(this.PropertyValueList_SelectedIndexChanged);
			// 
			// PropertyColumn
			// 
			this.PropertyColumn.Text = "Property";
			this.PropertyColumn.Width = 246;
			// 
			// ValueColumn
			// 
			this.ValueColumn.Text = "Value";
			this.ValueColumn.Width = 241;
			// 
			// MessageView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PropertyValueList);
			this.Name = "MessageView";
			this.Size = new System.Drawing.Size(487, 429);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView PropertyValueList;
        private System.Windows.Forms.ColumnHeader PropertyColumn;
        private System.Windows.Forms.ColumnHeader ValueColumn;

    }
}