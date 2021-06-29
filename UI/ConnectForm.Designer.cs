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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbLogger
            // 
            this.cmbLogger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogger.FormattingEnabled = true;
            this.cmbLogger.Location = new System.Drawing.Point(92, 12);
            this.cmbLogger.Name = "cmbLogger";
            this.cmbLogger.Size = new System.Drawing.Size(109, 21);
            this.cmbLogger.TabIndex = 0;
            // 
            // cmdSelect
            // 
            this.cmdSelect.Location = new System.Drawing.Point(12, 170);
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.Size = new System.Drawing.Size(189, 23);
            this.cmdSelect.TabIndex = 1;
            this.cmdSelect.Text = "Select Logger";
            this.cmdSelect.UseVisualStyleBackColor = true;
            this.cmdSelect.Click += new System.EventHandler(this.cmdSelect_Click);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(92, 39);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(109, 20);
            this.txtUser.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select Logger:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Username:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 205);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.cmdSelect);
            this.Controls.Add(this.cmbLogger);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Logger Login";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLogger;
        private System.Windows.Forms.Button cmdSelect;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

