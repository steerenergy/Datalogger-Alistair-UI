
namespace SteerLoggerUser
{
    partial class DownloadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadForm));
            this.cmdDownload = new System.Windows.Forms.Button();
            this.lblDownload = new System.Windows.Forms.Label();
            this.dgvDownload = new System.Windows.Forms.DataGridView();
            this.Download = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDownload)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdDownload
            // 
            this.cmdDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDownload.Location = new System.Drawing.Point(13, 345);
            this.cmdDownload.Name = "cmdDownload";
            this.cmdDownload.Size = new System.Drawing.Size(258, 23);
            this.cmdDownload.TabIndex = 1;
            this.cmdDownload.Text = "Download";
            this.cmdDownload.UseVisualStyleBackColor = true;
            this.cmdDownload.Click += new System.EventHandler(this.cmdDownload_Click);
            // 
            // lblDownload
            // 
            this.lblDownload.AutoSize = true;
            this.lblDownload.Location = new System.Drawing.Point(13, 10);
            this.lblDownload.Name = "lblDownload";
            this.lblDownload.Size = new System.Drawing.Size(115, 13);
            this.lblDownload.TabIndex = 2;
            this.lblDownload.Text = "Select ... to Download:";
            // 
            // dgvDownload
            // 
            this.dgvDownload.AllowUserToAddRows = false;
            this.dgvDownload.AllowUserToDeleteRows = false;
            this.dgvDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDownload.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Download,
            this.Id,
            this.LogName,
            this.Date,
            this.LogSize});
            this.dgvDownload.Location = new System.Drawing.Point(13, 26);
            this.dgvDownload.Name = "dgvDownload";
            this.dgvDownload.RowHeadersVisible = false;
            this.dgvDownload.Size = new System.Drawing.Size(258, 312);
            this.dgvDownload.TabIndex = 3;
            // 
            // Download
            // 
            this.Download.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Download.HeaderText = "Download";
            this.Download.Name = "Download";
            this.Download.Width = 61;
            // 
            // Id
            // 
            this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Id.HeaderText = "ID";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Width = 43;
            // 
            // LogName
            // 
            this.LogName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LogName.HeaderText = "Name";
            this.LogName.Name = "LogName";
            this.LogName.ReadOnly = true;
            this.LogName.Width = 60;
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 55;
            // 
            // LogSize
            // 
            this.LogSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.LogSize.HeaderText = "Size";
            this.LogSize.Name = "LogSize";
            this.LogSize.ReadOnly = true;
            this.LogSize.Width = 52;
            // 
            // DownloadForm
            // 
            this.AcceptButton = this.cmdDownload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 381);
            this.Controls.Add(this.dgvDownload);
            this.Controls.Add(this.lblDownload);
            this.Controls.Add(this.cmdDownload);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 420);
            this.Name = "DownloadForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "DownloadForm";
            this.Load += new System.EventHandler(this.DownloadForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDownload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cmdDownload;
        private System.Windows.Forms.Label lblDownload;
        private System.Windows.Forms.DataGridView dgvDownload;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Download;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogSize;
    }
}