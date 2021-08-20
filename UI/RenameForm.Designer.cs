
namespace SteerLoggerUser
{
    partial class RenameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameForm));
            this.cmdRename = new System.Windows.Forms.Button();
            this.dgvRename = new System.Windows.Forms.DataGridView();
            this.oldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRename = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRename)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdRename
            // 
            this.cmdRename.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdRename.Location = new System.Drawing.Point(13, 177);
            this.cmdRename.Name = "cmdRename";
            this.cmdRename.Size = new System.Drawing.Size(276, 23);
            this.cmdRename.TabIndex = 0;
            this.cmdRename.Text = "Rename";
            this.cmdRename.UseVisualStyleBackColor = true;
            this.cmdRename.Click += new System.EventHandler(this.cmdRename_Click);
            // 
            // dgvRename
            // 
            this.dgvRename.AllowUserToAddRows = false;
            this.dgvRename.AllowUserToDeleteRows = false;
            this.dgvRename.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRename.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRename.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRename.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.oldName,
            this.newName});
            this.dgvRename.Location = new System.Drawing.Point(13, 30);
            this.dgvRename.Name = "dgvRename";
            this.dgvRename.RowHeadersVisible = false;
            this.dgvRename.Size = new System.Drawing.Size(276, 141);
            this.dgvRename.TabIndex = 1;
            // 
            // oldName
            // 
            this.oldName.HeaderText = "Old Name";
            this.oldName.Name = "oldName";
            this.oldName.ReadOnly = true;
            // 
            // newName
            // 
            this.newName.HeaderText = "New Name";
            this.newName.Name = "newName";
            // 
            // lblRename
            // 
            this.lblRename.AutoSize = true;
            this.lblRename.Location = new System.Drawing.Point(13, 11);
            this.lblRename.Name = "lblRename";
            this.lblRename.Size = new System.Drawing.Size(280, 13);
            this.lblRename.TabIndex = 2;
            this.lblRename.Text = "Input new name in column, leave blank to keep old name.";
            // 
            // RenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 212);
            this.Controls.Add(this.lblRename);
            this.Controls.Add(this.dgvRename);
            this.Controls.Add(this.cmdRename);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RenameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RenameForm";
            this.Load += new System.EventHandler(this.RenameForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRename)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdRename;
        private System.Windows.Forms.DataGridView dgvRename;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn newName;
        private System.Windows.Forms.Label lblRename;
    }
}