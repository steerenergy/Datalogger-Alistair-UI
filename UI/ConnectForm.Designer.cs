namespace SteerLoggerUser
{
    partial class ConnectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectForm));
            this.cmbLogger = new System.Windows.Forms.ComboBox();
            this.cmdSelect = new System.Windows.Forms.Button();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblLogger = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdScan = new System.Windows.Forms.Button();
            this.pbScan = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // cmbLogger
            // 
            this.cmbLogger.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLogger.FormattingEnabled = true;
            this.cmbLogger.Location = new System.Drawing.Point(92, 44);
            this.cmbLogger.Name = "cmbLogger";
            this.cmbLogger.Size = new System.Drawing.Size(109, 21);
            this.cmbLogger.TabIndex = 0;
            // 
            // cmdSelect
            // 
            this.cmdSelect.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdSelect.Location = new System.Drawing.Point(12, 141);
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.Size = new System.Drawing.Size(189, 23);
            this.cmdSelect.TabIndex = 1;
            this.cmdSelect.Text = "Select Logger";
            this.cmdSelect.UseVisualStyleBackColor = true;
            this.cmdSelect.Click += new System.EventHandler(this.cmdSelect_Click);
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Location = new System.Drawing.Point(92, 115);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(109, 20);
            this.txtUser.TabIndex = 2;
            // 
            // lblLogger
            // 
            this.lblLogger.AutoSize = true;
            this.lblLogger.Location = new System.Drawing.Point(12, 47);
            this.lblLogger.Name = "lblLogger";
            this.lblLogger.Size = new System.Drawing.Size(43, 13);
            this.lblLogger.TabIndex = 3;
            this.lblLogger.Text = "Logger:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Username:";
            // 
            // cmdScan
            // 
            this.cmdScan.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdScan.Location = new System.Drawing.Point(12, 12);
            this.cmdScan.Name = "cmdScan";
            this.cmdScan.Size = new System.Drawing.Size(189, 23);
            this.cmdScan.TabIndex = 5;
            this.cmdScan.Text = "Scan for Loggers";
            this.cmdScan.UseVisualStyleBackColor = true;
            this.cmdScan.Click += new System.EventHandler(this.cmdScan_Click);
            // 
            // pbScan
            // 
            this.pbScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbScan.Enabled = false;
            this.pbScan.Location = new System.Drawing.Point(12, 170);
            this.pbScan.Name = "pbScan";
            this.pbScan.Size = new System.Drawing.Size(189, 23);
            this.pbScan.TabIndex = 6;
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 205);
            this.Controls.Add(this.pbScan);
            this.Controls.Add(this.cmdScan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblLogger);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.cmdSelect);
            this.Controls.Add(this.cmbLogger);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(229, 244);
            this.Name = "ConnectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Logger Login";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLogger;
        private System.Windows.Forms.Button cmdSelect;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblLogger;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdScan;
        private System.Windows.Forms.ProgressBar pbScan;
    }
}

