
namespace SteerLoggerUser
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.lnkManual = new System.Windows.Forms.LinkLabel();
            this.lblManual = new System.Windows.Forms.Label();
            this.lblIssues = new System.Windows.Forms.Label();
            this.lnkIssues = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lnkManual
            // 
            this.lnkManual.AutoEllipsis = true;
            this.lnkManual.Location = new System.Drawing.Point(12, 34);
            this.lnkManual.Name = "lnkManual";
            this.lnkManual.Size = new System.Drawing.Size(352, 23);
            this.lnkManual.TabIndex = 0;
            this.lnkManual.TabStop = true;
            this.lnkManual.Text = "https://tresor.it/p#0032n8slgqchec1tbqa1vlej/Live%20Projects/X01%20-%20Logging/Al" +
    "istair/Documentation/New%20Datalogger%20User%20Manual.docx";
            this.lnkManual.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkManual_LinkClicked);
            // 
            // lblManual
            // 
            this.lblManual.AutoSize = true;
            this.lblManual.Location = new System.Drawing.Point(12, 9);
            this.lblManual.Name = "lblManual";
            this.lblManual.Size = new System.Drawing.Size(244, 13);
            this.lblManual.TabIndex = 1;
            this.lblManual.Text = "The user manual for the logger can be found here:";
            // 
            // lblIssues
            // 
            this.lblIssues.AutoSize = true;
            this.lblIssues.Location = new System.Drawing.Point(12, 57);
            this.lblIssues.Name = "lblIssues";
            this.lblIssues.Size = new System.Drawing.Size(205, 13);
            this.lblIssues.TabIndex = 2;
            this.lblIssues.Text = "Issues/Suggestions can be reported here:";
            // 
            // lnkIssues
            // 
            this.lnkIssues.AutoSize = true;
            this.lnkIssues.Location = new System.Drawing.Point(12, 80);
            this.lnkIssues.Name = "lnkIssues";
            this.lnkIssues.Size = new System.Drawing.Size(293, 13);
            this.lnkIssues.TabIndex = 3;
            this.lnkIssues.TabStop = true;
            this.lnkIssues.Text = "https://github.com/steerenergy/Datalogger-Alistair-UI/issues";
            this.lnkIssues.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIssues_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 294);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Happy Logging!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SteerLoggerUser.Properties.Resources.steer;
            this.pictureBox1.Location = new System.Drawing.Point(12, 96);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(352, 195);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 316);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkIssues);
            this.Controls.Add(this.lblIssues);
            this.Controls.Add(this.lblManual);
            this.Controls.Add(this.lnkManual);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutForm";
            this.Text = "AboutForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkManual;
        private System.Windows.Forms.Label lblManual;
        private System.Windows.Forms.Label lblIssues;
        private System.Windows.Forms.LinkLabel lnkIssues;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}