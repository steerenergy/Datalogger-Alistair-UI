
namespace SteerLoggerUser
{
    partial class ColumnTextForm
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
            this.dgvHeaders = new System.Windows.Forms.DataGridView();
            this.cmdChangeHeaders = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHeaders
            // 
            this.dgvHeaders.AllowUserToAddRows = false;
            this.dgvHeaders.AllowUserToDeleteRows = false;
            this.dgvHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHeaders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHeaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHeaders.Location = new System.Drawing.Point(12, 37);
            this.dgvHeaders.MinimumSize = new System.Drawing.Size(300, 372);
            this.dgvHeaders.Name = "dgvHeaders";
            this.dgvHeaders.RowHeadersVisible = false;
            this.dgvHeaders.Size = new System.Drawing.Size(300, 372);
            this.dgvHeaders.TabIndex = 0;
            // 
            // cmdChangeHeaders
            // 
            this.cmdChangeHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdChangeHeaders.Location = new System.Drawing.Point(12, 415);
            this.cmdChangeHeaders.Name = "cmdChangeHeaders";
            this.cmdChangeHeaders.Size = new System.Drawing.Size(300, 23);
            this.cmdChangeHeaders.TabIndex = 1;
            this.cmdChangeHeaders.Text = "Change Headers";
            this.cmdChangeHeaders.UseVisualStyleBackColor = true;
            this.cmdChangeHeaders.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(12, 18);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(309, 13);
            this.lblInstructions.TabIndex = 2;
            this.lblInstructions.Text = "Input new header for each column, leave blank to keep old text.";
            // 
            // ColumnTextForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 450);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.cmdChangeHeaders);
            this.Controls.Add(this.dgvHeaders);
            this.Name = "ColumnTextForm";
            this.Text = "ColumnTextForm";
            this.Load += new System.EventHandler(this.ColumnTextForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHeaders;
        private System.Windows.Forms.Button cmdChangeHeaders;
        private System.Windows.Forms.Label lblInstructions;
    }
}