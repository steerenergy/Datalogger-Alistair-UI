
namespace SteerLoggerUser
{
    partial class DatabaseSearchForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtLoggedBy = new System.Windows.Forms.TextBox();
            this.cmdSearch = new System.Windows.Forms.Button();
            this.ckbDate = new System.Windows.Forms.CheckBox();
            this.lblProject = new System.Windows.Forms.Label();
            this.nudProject = new System.Windows.Forms.NumericUpDown();
            this.nudWorkPack = new System.Windows.Forms.NumericUpDown();
            this.lblWorkPack = new System.Windows.Forms.Label();
            this.nudJobSheet = new System.Windows.Forms.NumericUpDown();
            this.lblJobSheet = new System.Windows.Forms.Label();
            this.ckbProject = new System.Windows.Forms.CheckBox();
            this.ckbWorkPack = new System.Windows.Forms.CheckBox();
            this.ckbJobSheet = new System.Windows.Forms.CheckBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.ckbNotDownloaded = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWorkPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJobSheet)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Search Terms Below:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Log Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date of Log:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Logged By:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(85, 45);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(187, 20);
            this.txtName.TabIndex = 4;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(112, 73);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(160, 20);
            this.dtpDate.TabIndex = 5;
            // 
            // txtLoggedBy
            // 
            this.txtLoggedBy.Location = new System.Drawing.Point(85, 99);
            this.txtLoggedBy.Name = "txtLoggedBy";
            this.txtLoggedBy.Size = new System.Drawing.Size(187, 20);
            this.txtLoggedBy.TabIndex = 6;
            // 
            // cmdSearch
            // 
            this.cmdSearch.Location = new System.Drawing.Point(16, 254);
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Size = new System.Drawing.Size(259, 23);
            this.cmdSearch.TabIndex = 7;
            this.cmdSearch.Text = "Search";
            this.cmdSearch.UseVisualStyleBackColor = true;
            this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
            // 
            // ckbDate
            // 
            this.ckbDate.AutoSize = true;
            this.ckbDate.Location = new System.Drawing.Point(85, 79);
            this.ckbDate.Name = "ckbDate";
            this.ckbDate.Size = new System.Drawing.Size(15, 14);
            this.ckbDate.TabIndex = 8;
            this.ckbDate.UseVisualStyleBackColor = true;
            this.ckbDate.CheckedChanged += new System.EventHandler(this.ckbDate_CheckedChanged);
            // 
            // lblProject
            // 
            this.lblProject.AutoSize = true;
            this.lblProject.Location = new System.Drawing.Point(13, 127);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(43, 13);
            this.lblProject.TabIndex = 9;
            this.lblProject.Text = "Project:";
            // 
            // nudProject
            // 
            this.nudProject.Location = new System.Drawing.Point(112, 125);
            this.nudProject.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudProject.Name = "nudProject";
            this.nudProject.Size = new System.Drawing.Size(160, 20);
            this.nudProject.TabIndex = 10;
            // 
            // nudWorkPack
            // 
            this.nudWorkPack.Location = new System.Drawing.Point(112, 152);
            this.nudWorkPack.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudWorkPack.Name = "nudWorkPack";
            this.nudWorkPack.Size = new System.Drawing.Size(160, 20);
            this.nudWorkPack.TabIndex = 11;
            // 
            // lblWorkPack
            // 
            this.lblWorkPack.AutoSize = true;
            this.lblWorkPack.Location = new System.Drawing.Point(13, 154);
            this.lblWorkPack.Name = "lblWorkPack";
            this.lblWorkPack.Size = new System.Drawing.Size(64, 13);
            this.lblWorkPack.TabIndex = 12;
            this.lblWorkPack.Text = "Work Pack:";
            // 
            // nudJobSheet
            // 
            this.nudJobSheet.Location = new System.Drawing.Point(112, 179);
            this.nudJobSheet.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudJobSheet.Name = "nudJobSheet";
            this.nudJobSheet.Size = new System.Drawing.Size(160, 20);
            this.nudJobSheet.TabIndex = 13;
            // 
            // lblJobSheet
            // 
            this.lblJobSheet.AutoSize = true;
            this.lblJobSheet.Location = new System.Drawing.Point(13, 181);
            this.lblJobSheet.Name = "lblJobSheet";
            this.lblJobSheet.Size = new System.Drawing.Size(58, 13);
            this.lblJobSheet.TabIndex = 14;
            this.lblJobSheet.Text = "Job Sheet:";
            // 
            // ckbProject
            // 
            this.ckbProject.AutoSize = true;
            this.ckbProject.Location = new System.Drawing.Point(85, 127);
            this.ckbProject.Name = "ckbProject";
            this.ckbProject.Size = new System.Drawing.Size(15, 14);
            this.ckbProject.TabIndex = 15;
            this.ckbProject.UseVisualStyleBackColor = true;
            this.ckbProject.CheckedChanged += new System.EventHandler(this.ckbProject_CheckedChanged);
            // 
            // ckbWorkPack
            // 
            this.ckbWorkPack.AutoSize = true;
            this.ckbWorkPack.Location = new System.Drawing.Point(85, 154);
            this.ckbWorkPack.Name = "ckbWorkPack";
            this.ckbWorkPack.Size = new System.Drawing.Size(15, 14);
            this.ckbWorkPack.TabIndex = 16;
            this.ckbWorkPack.UseVisualStyleBackColor = true;
            this.ckbWorkPack.CheckedChanged += new System.EventHandler(this.ckbWorkPack_CheckedChanged);
            // 
            // ckbJobSheet
            // 
            this.ckbJobSheet.AutoSize = true;
            this.ckbJobSheet.Location = new System.Drawing.Point(85, 181);
            this.ckbJobSheet.Name = "ckbJobSheet";
            this.ckbJobSheet.Size = new System.Drawing.Size(15, 14);
            this.ckbJobSheet.TabIndex = 17;
            this.ckbJobSheet.UseVisualStyleBackColor = true;
            this.ckbJobSheet.CheckedChanged += new System.EventHandler(this.ckbJobSheet_CheckedChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(85, 205);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(187, 20);
            this.txtDescription.TabIndex = 18;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(13, 208);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 19;
            this.lblDescription.Text = "Description:";
            // 
            // ckbNotDownloaded
            // 
            this.ckbNotDownloaded.AutoSize = true;
            this.ckbNotDownloaded.Location = new System.Drawing.Point(85, 231);
            this.ckbNotDownloaded.Name = "ckbNotDownloaded";
            this.ckbNotDownloaded.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckbNotDownloaded.Size = new System.Drawing.Size(145, 17);
            this.ckbNotDownloaded.TabIndex = 20;
            this.ckbNotDownloaded.Text = "Not Downloaded by User";
            this.ckbNotDownloaded.UseVisualStyleBackColor = true;
            // 
            // DatabaseSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 292);
            this.Controls.Add(this.ckbNotDownloaded);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.ckbJobSheet);
            this.Controls.Add(this.ckbWorkPack);
            this.Controls.Add(this.ckbProject);
            this.Controls.Add(this.lblJobSheet);
            this.Controls.Add(this.nudJobSheet);
            this.Controls.Add(this.lblWorkPack);
            this.Controls.Add(this.nudWorkPack);
            this.Controls.Add(this.nudProject);
            this.Controls.Add(this.lblProject);
            this.Controls.Add(this.ckbDate);
            this.Controls.Add(this.cmdSearch);
            this.Controls.Add(this.txtLoggedBy);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DatabaseSearchForm";
            this.Text = "Search Pi Database";
            this.Load += new System.EventHandler(this.DatabaseSearchForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWorkPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJobSheet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtLoggedBy;
        private System.Windows.Forms.Button cmdSearch;
        private System.Windows.Forms.CheckBox ckbDate;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.NumericUpDown nudProject;
        private System.Windows.Forms.NumericUpDown nudWorkPack;
        private System.Windows.Forms.Label lblWorkPack;
        private System.Windows.Forms.NumericUpDown nudJobSheet;
        private System.Windows.Forms.Label lblJobSheet;
        private System.Windows.Forms.CheckBox ckbProject;
        private System.Windows.Forms.CheckBox ckbWorkPack;
        private System.Windows.Forms.CheckBox ckbJobSheet;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox ckbNotDownloaded;
    }
}