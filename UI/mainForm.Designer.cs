namespace SteerLoggerUser
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.pnlCtrlConf = new System.Windows.Forms.Panel();
            this.cmdResetConfig = new System.Windows.Forms.Button();
            this.cmdStopLog = new System.Windows.Forms.Button();
            this.cmdStartLog = new System.Windows.Forms.Button();
            this.cmdImportConf = new System.Windows.Forms.Button();
            this.cmdSaveUpload = new System.Windows.Forms.Button();
            this.dgvInputSetup = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.fName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inputType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gain = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.scaleMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scaleMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.units = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmdSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLogName = new System.Windows.Forms.TextBox();
            this.cmdCtrlConf = new System.Windows.Forms.Button();
            this.cmdDataProc = new System.Windows.Forms.Button();
            this.cmdSettings = new System.Windows.Forms.Button();
            this.cmdAbt = new System.Windows.Forms.Button();
            this.pnlDataProc = new System.Windows.Forms.Panel();
            this.dgvDataProc = new System.Windows.Forms.DataGridView();
            this.sfdConfig = new System.Windows.Forms.SaveFileDialog();
            this.ofdConfig = new System.Windows.Forms.OpenFileDialog();
            this.lblConnection = new System.Windows.Forms.Label();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.ofdLog = new System.Windows.Forms.OpenFileDialog();
            this.sfdLog = new System.Windows.Forms.SaveFileDialog();
            this.ofdPythonScript = new System.Windows.Forms.OpenFileDialog();
            this.cmdPythonScript = new System.Windows.Forms.Button();
            this.cmdPythonGraph = new System.Windows.Forms.Button();
            this.cmdExpExcel = new System.Windows.Forms.Button();
            this.cmdDwnldZip = new System.Windows.Forms.Button();
            this.cmdClearData = new System.Windows.Forms.Button();
            this.cmdDwnldCsv = new System.Windows.Forms.Button();
            this.cmdImportLogFile = new System.Windows.Forms.Button();
            this.cmdImportLogPi = new System.Windows.Forms.Button();
            this.pnlCtrlConf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputSetup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            this.pnlDataProc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataProc)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCtrlConf
            // 
            this.pnlCtrlConf.Controls.Add(this.cmdResetConfig);
            this.pnlCtrlConf.Controls.Add(this.cmdStopLog);
            this.pnlCtrlConf.Controls.Add(this.cmdStartLog);
            this.pnlCtrlConf.Controls.Add(this.cmdImportConf);
            this.pnlCtrlConf.Controls.Add(this.cmdSaveUpload);
            this.pnlCtrlConf.Controls.Add(this.dgvInputSetup);
            this.pnlCtrlConf.Controls.Add(this.cmdSave);
            this.pnlCtrlConf.Controls.Add(this.label2);
            this.pnlCtrlConf.Controls.Add(this.nudInterval);
            this.pnlCtrlConf.Controls.Add(this.label1);
            this.pnlCtrlConf.Controls.Add(this.txtLogName);
            this.pnlCtrlConf.Location = new System.Drawing.Point(12, 41);
            this.pnlCtrlConf.Name = "pnlCtrlConf";
            this.pnlCtrlConf.Size = new System.Drawing.Size(776, 397);
            this.pnlCtrlConf.TabIndex = 0;
            // 
            // cmdResetConfig
            // 
            this.cmdResetConfig.Location = new System.Drawing.Point(17, 95);
            this.cmdResetConfig.Name = "cmdResetConfig";
            this.cmdResetConfig.Size = new System.Drawing.Size(174, 25);
            this.cmdResetConfig.TabIndex = 10;
            this.cmdResetConfig.Text = "Reset Config";
            this.cmdResetConfig.UseVisualStyleBackColor = true;
            this.cmdResetConfig.Click += new System.EventHandler(this.cmdResetConfig_Click);
            // 
            // cmdStopLog
            // 
            this.cmdStopLog.Location = new System.Drawing.Point(111, 235);
            this.cmdStopLog.Name = "cmdStopLog";
            this.cmdStopLog.Size = new System.Drawing.Size(80, 33);
            this.cmdStopLog.TabIndex = 9;
            this.cmdStopLog.Text = "Stop Log";
            this.cmdStopLog.UseVisualStyleBackColor = true;
            this.cmdStopLog.Click += new System.EventHandler(this.cmdStopLog_Click);
            // 
            // cmdStartLog
            // 
            this.cmdStartLog.Location = new System.Drawing.Point(17, 235);
            this.cmdStartLog.Name = "cmdStartLog";
            this.cmdStartLog.Size = new System.Drawing.Size(88, 33);
            this.cmdStartLog.TabIndex = 8;
            this.cmdStartLog.Text = "Start Log";
            this.cmdStartLog.UseVisualStyleBackColor = true;
            this.cmdStartLog.Click += new System.EventHandler(this.cmdStartLog_Click);
            // 
            // cmdImportConf
            // 
            this.cmdImportConf.Location = new System.Drawing.Point(17, 352);
            this.cmdImportConf.Name = "cmdImportConf";
            this.cmdImportConf.Size = new System.Drawing.Size(174, 33);
            this.cmdImportConf.TabIndex = 7;
            this.cmdImportConf.Text = "Import Stored Config";
            this.cmdImportConf.UseVisualStyleBackColor = true;
            this.cmdImportConf.Click += new System.EventHandler(this.cmdImportConf_Click);
            // 
            // cmdSaveUpload
            // 
            this.cmdSaveUpload.Location = new System.Drawing.Point(17, 313);
            this.cmdSaveUpload.Name = "cmdSaveUpload";
            this.cmdSaveUpload.Size = new System.Drawing.Size(174, 33);
            this.cmdSaveUpload.TabIndex = 6;
            this.cmdSaveUpload.Text = "Save and Upload";
            this.cmdSaveUpload.UseVisualStyleBackColor = true;
            this.cmdSaveUpload.Click += new System.EventHandler(this.cmdSaveUpload_Click);
            // 
            // dgvInputSetup
            // 
            this.dgvInputSetup.AllowUserToAddRows = false;
            this.dgvInputSetup.AllowUserToDeleteRows = false;
            this.dgvInputSetup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInputSetup.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvInputSetup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInputSetup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.enabled,
            this.fName,
            this.inputType,
            this.gain,
            this.scaleMin,
            this.scaleMax,
            this.units});
            this.dgvInputSetup.Location = new System.Drawing.Point(197, 3);
            this.dgvInputSetup.Name = "dgvInputSetup";
            this.dgvInputSetup.RowHeadersVisible = false;
            this.dgvInputSetup.Size = new System.Drawing.Size(576, 391);
            this.dgvInputSetup.TabIndex = 5;
            // 
            // id
            // 
            this.id.HeaderText = "Number";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // enabled
            // 
            this.enabled.HeaderText = "Enabled";
            this.enabled.Name = "enabled";
            this.enabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.enabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // fName
            // 
            this.fName.HeaderText = "Friendly Name";
            this.fName.Name = "fName";
            // 
            // inputType
            // 
            this.inputType.HeaderText = "Input Type";
            this.inputType.Name = "inputType";
            this.inputType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.inputType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // gain
            // 
            this.gain.HeaderText = "Gain";
            this.gain.Name = "gain";
            this.gain.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // scaleMin
            // 
            this.scaleMin.HeaderText = "Scale Minimum";
            this.scaleMin.Name = "scaleMin";
            // 
            // scaleMax
            // 
            this.scaleMax.HeaderText = "Scale Maximum";
            this.scaleMax.Name = "scaleMax";
            // 
            // units
            // 
            this.units.HeaderText = "Units";
            this.units.Name = "units";
            this.units.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.units.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(17, 274);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(174, 33);
            this.cmdSave.TabIndex = 4;
            this.cmdSave.Text = "Save and Don\'t Upload";
            this.cmdSave.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Time Interval:";
            // 
            // nudInterval
            // 
            this.nudInterval.DecimalPlaces = 1;
            this.nudInterval.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudInterval.Location = new System.Drawing.Point(91, 67);
            this.nudInterval.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(100, 20);
            this.nudInterval.TabIndex = 2;
            this.nudInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Log Name:";
            // 
            // txtLogName
            // 
            this.txtLogName.Location = new System.Drawing.Point(91, 36);
            this.txtLogName.Name = "txtLogName";
            this.txtLogName.Size = new System.Drawing.Size(100, 20);
            this.txtLogName.TabIndex = 0;
            // 
            // cmdCtrlConf
            // 
            this.cmdCtrlConf.Location = new System.Drawing.Point(12, 12);
            this.cmdCtrlConf.Name = "cmdCtrlConf";
            this.cmdCtrlConf.Size = new System.Drawing.Size(117, 23);
            this.cmdCtrlConf.TabIndex = 1;
            this.cmdCtrlConf.Text = "Control/Config";
            this.cmdCtrlConf.UseVisualStyleBackColor = true;
            this.cmdCtrlConf.Click += new System.EventHandler(this.cmdCtrlConf_Click);
            // 
            // cmdDataProc
            // 
            this.cmdDataProc.Location = new System.Drawing.Point(135, 12);
            this.cmdDataProc.Name = "cmdDataProc";
            this.cmdDataProc.Size = new System.Drawing.Size(122, 23);
            this.cmdDataProc.TabIndex = 2;
            this.cmdDataProc.Text = "Download/Process Data";
            this.cmdDataProc.UseVisualStyleBackColor = true;
            this.cmdDataProc.Click += new System.EventHandler(this.cmdDataProc_Click);
            // 
            // cmdSettings
            // 
            this.cmdSettings.Location = new System.Drawing.Point(263, 12);
            this.cmdSettings.Name = "cmdSettings";
            this.cmdSettings.Size = new System.Drawing.Size(105, 23);
            this.cmdSettings.TabIndex = 3;
            this.cmdSettings.Text = "Settings";
            this.cmdSettings.UseVisualStyleBackColor = true;
            // 
            // cmdAbt
            // 
            this.cmdAbt.Location = new System.Drawing.Point(374, 12);
            this.cmdAbt.Name = "cmdAbt";
            this.cmdAbt.Size = new System.Drawing.Size(94, 23);
            this.cmdAbt.TabIndex = 4;
            this.cmdAbt.Text = "About/Help";
            this.cmdAbt.UseVisualStyleBackColor = true;
            // 
            // pnlDataProc
            // 
            this.pnlDataProc.Controls.Add(this.cmdClearData);
            this.pnlDataProc.Controls.Add(this.cmdDwnldCsv);
            this.pnlDataProc.Controls.Add(this.cmdExpExcel);
            this.pnlDataProc.Controls.Add(this.cmdDwnldZip);
            this.pnlDataProc.Controls.Add(this.cmdImportLogFile);
            this.pnlDataProc.Controls.Add(this.cmdImportLogPi);
            this.pnlDataProc.Controls.Add(this.cmdPythonGraph);
            this.pnlDataProc.Controls.Add(this.cmdPythonScript);
            this.pnlDataProc.Controls.Add(this.dgvDataProc);
            this.pnlDataProc.Location = new System.Drawing.Point(12, 41);
            this.pnlDataProc.Name = "pnlDataProc";
            this.pnlDataProc.Size = new System.Drawing.Size(776, 397);
            this.pnlDataProc.TabIndex = 5;
            // 
            // dgvDataProc
            // 
            this.dgvDataProc.AllowUserToAddRows = false;
            this.dgvDataProc.AllowUserToDeleteRows = false;
            this.dgvDataProc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataProc.Location = new System.Drawing.Point(197, 3);
            this.dgvDataProc.Name = "dgvDataProc";
            this.dgvDataProc.ReadOnly = true;
            this.dgvDataProc.RowHeadersVisible = false;
            this.dgvDataProc.Size = new System.Drawing.Size(576, 391);
            this.dgvDataProc.TabIndex = 3;
            // 
            // sfdConfig
            // 
            this.sfdConfig.DefaultExt = "ini";
            // 
            // ofdConfig
            // 
            this.ofdConfig.DefaultExt = "ini";
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(487, 17);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(119, 13);
            this.lblConnection.TabIndex = 6;
            this.lblConnection.Text = " You are connected to: ";
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(710, 12);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(75, 23);
            this.cmdConnect.TabIndex = 7;
            this.cmdConnect.Text = "Reconnect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // ofdLog
            // 
            this.ofdLog.DefaultExt = "csv";
            this.ofdLog.RestoreDirectory = true;
            // 
            // sfdLog
            // 
            this.sfdLog.DefaultExt = "csv";
            // 
            // ofdPythonScript
            // 
            this.ofdPythonScript.DefaultExt = "py";
            this.ofdPythonScript.InitialDirectory = "..\\..\\pythonScripts";
            // 
            // cmdPythonScript
            // 
            this.cmdPythonScript.Location = new System.Drawing.Point(17, 7);
            this.cmdPythonScript.Name = "cmdPythonScript";
            this.cmdPythonScript.Size = new System.Drawing.Size(174, 29);
            this.cmdPythonScript.TabIndex = 2;
            this.cmdPythonScript.Text = "Process using Python Script";
            this.cmdPythonScript.UseVisualStyleBackColor = true;
            this.cmdPythonScript.Click += new System.EventHandler(this.cmdPythonScript_Click);
            // 
            // cmdPythonGraph
            // 
            this.cmdPythonGraph.Location = new System.Drawing.Point(17, 42);
            this.cmdPythonGraph.Name = "cmdPythonGraph";
            this.cmdPythonGraph.Size = new System.Drawing.Size(174, 29);
            this.cmdPythonGraph.TabIndex = 3;
            this.cmdPythonGraph.Text = "Graph using Python Script";
            this.cmdPythonGraph.UseVisualStyleBackColor = true;
            this.cmdPythonGraph.Click += new System.EventHandler(this.cmdPythonGraph_Click);
            // 
            // cmdExpExcel
            // 
            this.cmdExpExcel.Location = new System.Drawing.Point(17, 321);
            this.cmdExpExcel.Name = "cmdExpExcel";
            this.cmdExpExcel.Size = new System.Drawing.Size(174, 29);
            this.cmdExpExcel.TabIndex = 2;
            this.cmdExpExcel.Text = "Export to Excel";
            this.cmdExpExcel.UseVisualStyleBackColor = true;
            this.cmdExpExcel.Click += new System.EventHandler(this.cmdExpExcel_Click);
            // 
            // cmdDwnldZip
            // 
            this.cmdDwnldZip.Location = new System.Drawing.Point(17, 286);
            this.cmdDwnldZip.Name = "cmdDwnldZip";
            this.cmdDwnldZip.Size = new System.Drawing.Size(174, 29);
            this.cmdDwnldZip.TabIndex = 1;
            this.cmdDwnldZip.Text = "Save as CSV/s in Zip";
            this.cmdDwnldZip.UseVisualStyleBackColor = true;
            this.cmdDwnldZip.Click += new System.EventHandler(this.cmdDwnldZip_Click);
            // 
            // cmdClearData
            // 
            this.cmdClearData.Location = new System.Drawing.Point(17, 356);
            this.cmdClearData.Name = "cmdClearData";
            this.cmdClearData.Size = new System.Drawing.Size(174, 29);
            this.cmdClearData.TabIndex = 4;
            this.cmdClearData.Text = "Clear Data View";
            this.cmdClearData.UseVisualStyleBackColor = true;
            this.cmdClearData.Click += new System.EventHandler(this.cmdClearData_Click);
            // 
            // cmdDwnldCsv
            // 
            this.cmdDwnldCsv.Location = new System.Drawing.Point(17, 251);
            this.cmdDwnldCsv.Name = "cmdDwnldCsv";
            this.cmdDwnldCsv.Size = new System.Drawing.Size(174, 29);
            this.cmdDwnldCsv.TabIndex = 0;
            this.cmdDwnldCsv.Text = "Save as CSV/s";
            this.cmdDwnldCsv.UseVisualStyleBackColor = true;
            this.cmdDwnldCsv.Click += new System.EventHandler(this.cmdDwnldCsv_Click);
            // 
            // cmdImportLogFile
            // 
            this.cmdImportLogFile.Location = new System.Drawing.Point(17, 216);
            this.cmdImportLogFile.Name = "cmdImportLogFile";
            this.cmdImportLogFile.Size = new System.Drawing.Size(174, 29);
            this.cmdImportLogFile.TabIndex = 5;
            this.cmdImportLogFile.Text = "Import Log From File";
            this.cmdImportLogFile.UseVisualStyleBackColor = true;
            this.cmdImportLogFile.Click += new System.EventHandler(this.cmdImportLogFile_Click);
            // 
            // cmdImportLogPi
            // 
            this.cmdImportLogPi.Location = new System.Drawing.Point(17, 181);
            this.cmdImportLogPi.Name = "cmdImportLogPi";
            this.cmdImportLogPi.Size = new System.Drawing.Size(174, 29);
            this.cmdImportLogPi.TabIndex = 6;
            this.cmdImportLogPi.Text = "Import Log From Pi";
            this.cmdImportLogPi.UseVisualStyleBackColor = true;
            this.cmdImportLogPi.Click += new System.EventHandler(this.cmdImportLogPi_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cmdConnect);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.cmdAbt);
            this.Controls.Add(this.cmdSettings);
            this.Controls.Add(this.cmdDataProc);
            this.Controls.Add(this.cmdCtrlConf);
            this.Controls.Add(this.pnlDataProc);
            this.Controls.Add(this.pnlCtrlConf);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainForm";
            this.Text = "mainForm";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.pnlCtrlConf.ResumeLayout(false);
            this.pnlCtrlConf.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputSetup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            this.pnlDataProc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataProc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlCtrlConf;
        private System.Windows.Forms.TextBox txtLogName;
        private System.Windows.Forms.Button cmdCtrlConf;
        private System.Windows.Forms.Button cmdDataProc;
        private System.Windows.Forms.Button cmdSettings;
        private System.Windows.Forms.Button cmdAbt;
        private System.Windows.Forms.Button cmdImportConf;
        private System.Windows.Forms.Button cmdSaveUpload;
        private System.Windows.Forms.DataGridView dgvInputSetup;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlDataProc;
        private System.Windows.Forms.DataGridView dgvDataProc;
        private System.Windows.Forms.SaveFileDialog sfdConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn fName;
        private System.Windows.Forms.DataGridViewComboBoxColumn inputType;
        private System.Windows.Forms.DataGridViewComboBoxColumn gain;
        private System.Windows.Forms.DataGridViewTextBoxColumn scaleMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn scaleMax;
        private System.Windows.Forms.DataGridViewComboBoxColumn units;
        private System.Windows.Forms.Button cmdStopLog;
        private System.Windows.Forms.Button cmdStartLog;
        private System.Windows.Forms.OpenFileDialog ofdConfig;
        private System.Windows.Forms.Button cmdResetConfig;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.OpenFileDialog ofdLog;
        private System.Windows.Forms.SaveFileDialog sfdLog;
        private System.Windows.Forms.OpenFileDialog ofdPythonScript;
        private System.Windows.Forms.Button cmdClearData;
        private System.Windows.Forms.Button cmdDwnldCsv;
        private System.Windows.Forms.Button cmdExpExcel;
        private System.Windows.Forms.Button cmdDwnldZip;
        private System.Windows.Forms.Button cmdImportLogFile;
        private System.Windows.Forms.Button cmdImportLogPi;
        private System.Windows.Forms.Button cmdPythonGraph;
        private System.Windows.Forms.Button cmdPythonScript;
    }
}