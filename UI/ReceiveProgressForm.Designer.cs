
namespace SteerLoggerUser
{
    partial class ReceiveProgressForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiveProgressForm));
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.txtOuput = new System.Windows.Forms.TextBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pbDownload
            // 
            this.pbDownload.Location = new System.Drawing.Point(12, 178);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(297, 23);
            this.pbDownload.TabIndex = 0;
            // 
            // txtOuput
            // 
            this.txtOuput.Location = new System.Drawing.Point(13, 13);
            this.txtOuput.Multiline = true;
            this.txtOuput.Name = "txtOuput";
            this.txtOuput.ReadOnly = true;
            this.txtOuput.Size = new System.Drawing.Size(363, 159);
            this.txtOuput.TabIndex = 1;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(315, 188);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(65, 13);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "0d 0h 0m 0s";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // ReceiveProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 213);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.txtOuput);
            this.Controls.Add(this.pbDownload);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(404, 252);
            this.MinimumSize = new System.Drawing.Size(404, 252);
            this.Name = "ReceiveProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ReceiveProgressForm";
            this.Load += new System.EventHandler(this.ReceiveProgressForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbDownload;
        private System.Windows.Forms.TextBox txtOuput;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timer;
    }
}