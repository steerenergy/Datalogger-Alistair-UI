
namespace SteerLoggerUser
{
    partial class ChangeUserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeUserForm));
            this.txtUser = new System.Windows.Forms.TextBox();
            this.cmdChangeUser = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Location = new System.Drawing.Point(50, 14);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(140, 20);
            this.txtUser.TabIndex = 0;
            // 
            // cmdChangeUser
            // 
            this.cmdChangeUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdChangeUser.Location = new System.Drawing.Point(13, 40);
            this.cmdChangeUser.Name = "cmdChangeUser";
            this.cmdChangeUser.Size = new System.Drawing.Size(177, 23);
            this.cmdChangeUser.TabIndex = 1;
            this.cmdChangeUser.Text = "Change";
            this.cmdChangeUser.UseVisualStyleBackColor = true;
            this.cmdChangeUser.Click += new System.EventHandler(this.cmdChangeUser_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(12, 17);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(32, 13);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "User:";
            // 
            // ChangeUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 83);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.cmdChangeUser);
            this.Controls.Add(this.txtUser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(222, 122);
            this.Name = "ChangeUserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change User";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Button cmdChangeUser;
        private System.Windows.Forms.Label lblUser;
    }
}