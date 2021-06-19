
namespace SteerLoggerUser
{
    partial class ExcelForm
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
            this.cmbXAxis = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYAxis = new System.Windows.Forms.ComboBox();
            this.cmdExport = new System.Windows.Forms.Button();
            this.ckbCreateGraph = new System.Windows.Forms.CheckBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFormula = new System.Windows.Forms.TextBox();
            this.cmdAddFormula = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.txtColTitle = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbXAxis
            // 
            this.cmbXAxis.FormattingEnabled = true;
            this.cmbXAxis.Location = new System.Drawing.Point(94, 145);
            this.cmbXAxis.Name = "cmbXAxis";
            this.cmbXAxis.Size = new System.Drawing.Size(146, 21);
            this.cmbXAxis.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "X axis Column:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y axis Column:";
            // 
            // cmbYAxis
            // 
            this.cmbYAxis.FormattingEnabled = true;
            this.cmbYAxis.Location = new System.Drawing.Point(94, 175);
            this.cmbYAxis.Name = "cmbYAxis";
            this.cmbYAxis.Size = new System.Drawing.Size(146, 21);
            this.cmbYAxis.TabIndex = 3;
            // 
            // cmdExport
            // 
            this.cmdExport.Location = new System.Drawing.Point(12, 229);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(228, 23);
            this.cmdExport.TabIndex = 4;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
            // 
            // ckbCreateGraph
            // 
            this.ckbCreateGraph.AutoSize = true;
            this.ckbCreateGraph.Location = new System.Drawing.Point(12, 128);
            this.ckbCreateGraph.Name = "ckbCreateGraph";
            this.ckbCreateGraph.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ckbCreateGraph.Size = new System.Drawing.Size(95, 17);
            this.ckbCreateGraph.TabIndex = 5;
            this.ckbCreateGraph.Text = "?Create Graph";
            this.ckbCreateGraph.UseVisualStyleBackColor = true;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(94, 203);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(146, 20);
            this.txtTitle.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Graph Title:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Formula:";
            // 
            // txtFormula
            // 
            this.txtFormula.Location = new System.Drawing.Point(94, 9);
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.Size = new System.Drawing.Size(146, 20);
            this.txtFormula.TabIndex = 10;
            // 
            // cmdAddFormula
            // 
            this.cmdAddFormula.Location = new System.Drawing.Point(12, 91);
            this.cmdAddFormula.Name = "cmdAddFormula";
            this.cmdAddFormula.Size = new System.Drawing.Size(228, 23);
            this.cmdAddFormula.TabIndex = 11;
            this.cmdAddFormula.Text = "Add Formula Column";
            this.cmdAddFormula.UseVisualStyleBackColor = true;
            this.cmdAddFormula.Click += new System.EventHandler(this.cmdAddFormula_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Location = new System.Drawing.Point(94, 62);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(146, 23);
            this.cmdHelp.TabIndex = 12;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // txtColTitle
            // 
            this.txtColTitle.Location = new System.Drawing.Point(94, 36);
            this.txtColTitle.Name = "txtColTitle";
            this.txtColTitle.Size = new System.Drawing.Size(146, 20);
            this.txtColTitle.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Column Title:";
            // 
            // ExcelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 266);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtColTitle);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdAddFormula);
            this.Controls.Add(this.txtFormula);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.ckbCreateGraph);
            this.Controls.Add(this.cmdExport);
            this.Controls.Add(this.cmbYAxis);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbXAxis);
            this.Name = "ExcelForm";
            this.Text = "ExcelForm";
            this.Load += new System.EventHandler(this.ExcelForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbXAxis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYAxis;
        private System.Windows.Forms.Button cmdExport;
        private System.Windows.Forms.CheckBox ckbCreateGraph;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFormula;
        private System.Windows.Forms.Button cmdAddFormula;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.TextBox txtColTitle;
        private System.Windows.Forms.Label label5;
    }
}