
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
            this.panel = new System.Windows.Forms.Panel();
            this.cmdDownload = new System.Windows.Forms.Button();
            this.lblDownload = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.AutoSize = true;
            this.panel.Location = new System.Drawing.Point(13, 27);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(258, 312);
            this.panel.TabIndex = 0;
            // 
            // cmdDownload
            // 
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
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(284, 381);
            this.Controls.Add(this.lblDownload);
            this.Controls.Add(this.cmdDownload);
            this.Controls.Add(this.panel);
            this.Name = "DownloadForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "DownloadForm";
            this.Load += new System.EventHandler(this.DownloadForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button cmdDownload;
        private System.Windows.Forms.Label lblDownload;
    }
}