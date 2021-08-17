
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
            this.pnlTemplate = new System.Windows.Forms.Panel();
            this.cmdLoadTemplate = new System.Windows.Forms.Button();
            this.lblSheet = new System.Windows.Forms.Label();
            this.cmbTemplate = new System.Windows.Forms.ComboBox();
            this.cmdWriteCol = new System.Windows.Forms.Button();
            this.cmbLogCols = new System.Windows.Forms.ComboBox();
            this.txtSelectedCell = new System.Windows.Forms.TextBox();
            this.dgvTemplate = new System.Windows.Forms.DataGridView();
            this.lblName = new System.Windows.Forms.Label();
            this.txtSheetName = new System.Windows.Forms.TextBox();
            this.cmdOpenTemplate = new System.Windows.Forms.Button();
            this.cmdCreateWb = new System.Windows.Forms.Button();
            this.cmdUseTemplate = new System.Windows.Forms.Button();
            this.pnlExportNew = new System.Windows.Forms.Panel();
            this.ofdTemplate = new System.Windows.Forms.OpenFileDialog();
            this.pnlTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTemplate)).BeginInit();
            this.pnlExportNew.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbXAxis
            // 
            this.cmbXAxis.FormattingEnabled = true;
            this.cmbXAxis.Location = new System.Drawing.Point(90, 140);
            this.cmbXAxis.Name = "cmbXAxis";
            this.cmbXAxis.Size = new System.Drawing.Size(218, 21);
            this.cmbXAxis.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "X axis Column:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y axis Column:";
            // 
            // cmbYAxis
            // 
            this.cmbYAxis.FormattingEnabled = true;
            this.cmbYAxis.Location = new System.Drawing.Point(90, 167);
            this.cmbYAxis.Name = "cmbYAxis";
            this.cmbYAxis.Size = new System.Drawing.Size(218, 21);
            this.cmbYAxis.TabIndex = 3;
            // 
            // cmdExport
            // 
            this.cmdExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExport.Location = new System.Drawing.Point(15, 428);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(445, 23);
            this.cmdExport.TabIndex = 4;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
            // 
            // ckbCreateGraph
            // 
            this.ckbCreateGraph.AutoSize = true;
            this.ckbCreateGraph.Location = new System.Drawing.Point(213, 117);
            this.ckbCreateGraph.Name = "ckbCreateGraph";
            this.ckbCreateGraph.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ckbCreateGraph.Size = new System.Drawing.Size(95, 17);
            this.ckbCreateGraph.TabIndex = 5;
            this.ckbCreateGraph.Text = "?Create Graph";
            this.ckbCreateGraph.UseVisualStyleBackColor = true;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(90, 194);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(218, 20);
            this.txtTitle.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 197);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Graph Title:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Formula:";
            // 
            // txtFormula
            // 
            this.txtFormula.Location = new System.Drawing.Point(90, 7);
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.Size = new System.Drawing.Size(218, 20);
            this.txtFormula.TabIndex = 10;
            // 
            // cmdAddFormula
            // 
            this.cmdAddFormula.Location = new System.Drawing.Point(19, 88);
            this.cmdAddFormula.Name = "cmdAddFormula";
            this.cmdAddFormula.Size = new System.Drawing.Size(289, 23);
            this.cmdAddFormula.TabIndex = 11;
            this.cmdAddFormula.Text = "Add Formula Column";
            this.cmdAddFormula.UseVisualStyleBackColor = true;
            this.cmdAddFormula.Click += new System.EventHandler(this.cmdAddFormula_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Location = new System.Drawing.Point(90, 59);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(218, 23);
            this.cmdHelp.TabIndex = 12;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // txtColTitle
            // 
            this.txtColTitle.Location = new System.Drawing.Point(90, 33);
            this.txtColTitle.Name = "txtColTitle";
            this.txtColTitle.Size = new System.Drawing.Size(218, 20);
            this.txtColTitle.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Column Title:";
            // 
            // pnlTemplate
            // 
            this.pnlTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTemplate.Controls.Add(this.cmdLoadTemplate);
            this.pnlTemplate.Controls.Add(this.lblSheet);
            this.pnlTemplate.Controls.Add(this.cmbTemplate);
            this.pnlTemplate.Controls.Add(this.cmdWriteCol);
            this.pnlTemplate.Controls.Add(this.cmbLogCols);
            this.pnlTemplate.Controls.Add(this.txtSelectedCell);
            this.pnlTemplate.Controls.Add(this.dgvTemplate);
            this.pnlTemplate.Controls.Add(this.lblName);
            this.pnlTemplate.Controls.Add(this.txtSheetName);
            this.pnlTemplate.Controls.Add(this.cmdOpenTemplate);
            this.pnlTemplate.Location = new System.Drawing.Point(15, 42);
            this.pnlTemplate.Name = "pnlTemplate";
            this.pnlTemplate.Size = new System.Drawing.Size(445, 380);
            this.pnlTemplate.TabIndex = 15;
            // 
            // cmdLoadTemplate
            // 
            this.cmdLoadTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLoadTemplate.Location = new System.Drawing.Point(268, 39);
            this.cmdLoadTemplate.Name = "cmdLoadTemplate";
            this.cmdLoadTemplate.Size = new System.Drawing.Size(173, 23);
            this.cmdLoadTemplate.TabIndex = 25;
            this.cmdLoadTemplate.Text = "Load Template";
            this.cmdLoadTemplate.UseVisualStyleBackColor = true;
            this.cmdLoadTemplate.Click += new System.EventHandler(this.cmdLoadTemplate_Click);
            // 
            // lblSheet
            // 
            this.lblSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSheet.AutoSize = true;
            this.lblSheet.Location = new System.Drawing.Point(224, 15);
            this.lblSheet.Name = "lblSheet";
            this.lblSheet.Size = new System.Drawing.Size(38, 13);
            this.lblSheet.TabIndex = 24;
            this.lblSheet.Text = "Sheet:";
            // 
            // cmbTemplate
            // 
            this.cmbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTemplate.FormattingEnabled = true;
            this.cmbTemplate.Location = new System.Drawing.Point(268, 12);
            this.cmbTemplate.Name = "cmbTemplate";
            this.cmbTemplate.Size = new System.Drawing.Size(173, 21);
            this.cmbTemplate.TabIndex = 23;
            // 
            // cmdWriteCol
            // 
            this.cmdWriteCol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdWriteCol.Location = new System.Drawing.Point(296, 352);
            this.cmdWriteCol.Name = "cmdWriteCol";
            this.cmdWriteCol.Size = new System.Drawing.Size(145, 23);
            this.cmdWriteCol.TabIndex = 22;
            this.cmdWriteCol.Text = "Write Column";
            this.cmdWriteCol.UseVisualStyleBackColor = true;
            this.cmdWriteCol.Click += new System.EventHandler(this.cmdWriteCol_Click);
            // 
            // cmbLogCols
            // 
            this.cmbLogCols.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmbLogCols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogCols.FormattingEnabled = true;
            this.cmbLogCols.Location = new System.Drawing.Point(144, 354);
            this.cmbLogCols.Name = "cmbLogCols";
            this.cmbLogCols.Size = new System.Drawing.Size(146, 21);
            this.cmbLogCols.TabIndex = 21;
            // 
            // txtSelectedCell
            // 
            this.txtSelectedCell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSelectedCell.Location = new System.Drawing.Point(6, 354);
            this.txtSelectedCell.Name = "txtSelectedCell";
            this.txtSelectedCell.ReadOnly = true;
            this.txtSelectedCell.Size = new System.Drawing.Size(132, 20);
            this.txtSelectedCell.TabIndex = 20;
            // 
            // dgvTemplate
            // 
            this.dgvTemplate.AllowUserToAddRows = false;
            this.dgvTemplate.AllowUserToDeleteRows = false;
            this.dgvTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTemplate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTemplate.Location = new System.Drawing.Point(6, 95);
            this.dgvTemplate.Name = "dgvTemplate";
            this.dgvTemplate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTemplate.Size = new System.Drawing.Size(436, 253);
            this.dgvTemplate.TabIndex = 19;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 72);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(94, 13);
            this.lblName.TabIndex = 18;
            this.lblName.Text = "New Sheet Name:";
            // 
            // txtSheetName
            // 
            this.txtSheetName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSheetName.Location = new System.Drawing.Point(137, 69);
            this.txtSheetName.Name = "txtSheetName";
            this.txtSheetName.Size = new System.Drawing.Size(304, 20);
            this.txtSheetName.TabIndex = 17;
            // 
            // cmdOpenTemplate
            // 
            this.cmdOpenTemplate.Location = new System.Drawing.Point(6, 9);
            this.cmdOpenTemplate.Name = "cmdOpenTemplate";
            this.cmdOpenTemplate.Size = new System.Drawing.Size(212, 24);
            this.cmdOpenTemplate.TabIndex = 16;
            this.cmdOpenTemplate.Text = "Find Template Workbook";
            this.cmdOpenTemplate.UseVisualStyleBackColor = true;
            this.cmdOpenTemplate.Click += new System.EventHandler(this.cmdOpenTemplate_Click);
            // 
            // cmdCreateWb
            // 
            this.cmdCreateWb.Location = new System.Drawing.Point(12, 12);
            this.cmdCreateWb.Name = "cmdCreateWb";
            this.cmdCreateWb.Size = new System.Drawing.Size(221, 24);
            this.cmdCreateWb.TabIndex = 16;
            this.cmdCreateWb.Text = "Create New Workbook";
            this.cmdCreateWb.UseVisualStyleBackColor = true;
            this.cmdCreateWb.Click += new System.EventHandler(this.cmdCreateWb_Click);
            // 
            // cmdUseTemplate
            // 
            this.cmdUseTemplate.Location = new System.Drawing.Point(239, 12);
            this.cmdUseTemplate.Name = "cmdUseTemplate";
            this.cmdUseTemplate.Size = new System.Drawing.Size(221, 24);
            this.cmdUseTemplate.TabIndex = 17;
            this.cmdUseTemplate.Text = "Use Template";
            this.cmdUseTemplate.UseVisualStyleBackColor = true;
            this.cmdUseTemplate.Click += new System.EventHandler(this.cmdUseTemplate_Click);
            // 
            // pnlExportNew
            // 
            this.pnlExportNew.Controls.Add(this.label5);
            this.pnlExportNew.Controls.Add(this.txtTitle);
            this.pnlExportNew.Controls.Add(this.label4);
            this.pnlExportNew.Controls.Add(this.txtFormula);
            this.pnlExportNew.Controls.Add(this.txtColTitle);
            this.pnlExportNew.Controls.Add(this.label3);
            this.pnlExportNew.Controls.Add(this.cmdHelp);
            this.pnlExportNew.Controls.Add(this.cmbYAxis);
            this.pnlExportNew.Controls.Add(this.cmdAddFormula);
            this.pnlExportNew.Controls.Add(this.label2);
            this.pnlExportNew.Controls.Add(this.cmbXAxis);
            this.pnlExportNew.Controls.Add(this.label1);
            this.pnlExportNew.Controls.Add(this.ckbCreateGraph);
            this.pnlExportNew.Location = new System.Drawing.Point(69, 42);
            this.pnlExportNew.Name = "pnlExportNew";
            this.pnlExportNew.Size = new System.Drawing.Size(318, 224);
            this.pnlExportNew.TabIndex = 15;
            // 
            // ofdTemplate
            // 
            this.ofdTemplate.Filter = "Excel files|*.xlsx|All files|*.*";
            // 
            // ExcelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(472, 463);
            this.Controls.Add(this.cmdUseTemplate);
            this.Controls.Add(this.cmdCreateWb);
            this.Controls.Add(this.cmdExport);
            this.Controls.Add(this.pnlTemplate);
            this.Controls.Add(this.pnlExportNew);
            this.MinimumSize = new System.Drawing.Size(488, 502);
            this.Name = "ExcelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExcelForm";
            this.Load += new System.EventHandler(this.ExcelForm_Load);
            this.pnlTemplate.ResumeLayout(false);
            this.pnlTemplate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTemplate)).EndInit();
            this.pnlExportNew.ResumeLayout(false);
            this.pnlExportNew.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Panel pnlTemplate;
        private System.Windows.Forms.Button cmdCreateWb;
        private System.Windows.Forms.Button cmdUseTemplate;
        private System.Windows.Forms.Panel pnlExportNew;
        private System.Windows.Forms.OpenFileDialog ofdTemplate;
        private System.Windows.Forms.Button cmdOpenTemplate;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtSheetName;
        private System.Windows.Forms.DataGridView dgvTemplate;
        private System.Windows.Forms.Button cmdWriteCol;
        private System.Windows.Forms.ComboBox cmbLogCols;
        private System.Windows.Forms.TextBox txtSelectedCell;
        private System.Windows.Forms.Label lblSheet;
        private System.Windows.Forms.ComboBox cmbTemplate;
        private System.Windows.Forms.Button cmdLoadTemplate;
    }
}