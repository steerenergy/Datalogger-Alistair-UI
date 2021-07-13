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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.cmdClearData = new System.Windows.Forms.Button();
            this.cmdDwnldCsv = new System.Windows.Forms.Button();
            this.cmdExpExcel = new System.Windows.Forms.Button();
            this.cmdDwnldZip = new System.Windows.Forms.Button();
            this.cmdImportLogFile = new System.Windows.Forms.Button();
            this.cmdImportLogPi = new System.Windows.Forms.Button();
            this.cmdPythonGraph = new System.Windows.Forms.Button();
            this.cmdPythonScript = new System.Windows.Forms.Button();
            this.dgvDataProc = new System.Windows.Forms.DataGridView();
            this.sfdConfig = new System.Windows.Forms.SaveFileDialog();
            this.ofdConfig = new System.Windows.Forms.OpenFileDialog();
            this.lblConnection = new System.Windows.Forms.Label();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.ofdLog = new System.Windows.Forms.OpenFileDialog();
            this.sfdLog = new System.Windows.Forms.SaveFileDialog();
            this.ofdPythonScript = new System.Windows.Forms.OpenFileDialog();
            this.pnlCtrlConf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputSetup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            this.pnlDataProc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataProc)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCtrlConf
            // 
            this.pnlCtrlConf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.pnlCtrlConf.Location = new System.Drawing.Point(13, 40);
            this.pnlCtrlConf.Name = "pnlCtrlConf";
            this.pnlCtrlConf.Size = new System.Drawing.Size(828, 391);
            this.pnlCtrlConf.TabIndex = 0;
            // 
            // cmdResetConfig
            // 
            this.cmdResetConfig.AutoSize = true;
            this.cmdResetConfig.Location = new System.Drawing.Point(18, 94);
            this.cmdResetConfig.Name = "cmdResetConfig";
            this.cmdResetConfig.Size = new System.Drawing.Size(186, 25);
            this.cmdResetConfig.TabIndex = 10;
            this.cmdResetConfig.Text = "Reset Config";
            this.cmdResetConfig.UseVisualStyleBackColor = true;
            this.cmdResetConfig.Click += new System.EventHandler(this.cmdResetConfig_Click);
            // 
            // cmdStopLog
            // 
            this.cmdStopLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdStopLog.Location = new System.Drawing.Point(118, 231);
            this.cmdStopLog.Name = "cmdStopLog";
            this.cmdStopLog.Size = new System.Drawing.Size(86, 33);
            this.cmdStopLog.TabIndex = 9;
            this.cmdStopLog.Text = "Stop Log";
            this.cmdStopLog.UseVisualStyleBackColor = true;
            this.cmdStopLog.Click += new System.EventHandler(this.cmdStopLog_Click);
            // 
            // cmdStartLog
            // 
            this.cmdStartLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdStartLog.AutoEllipsis = true;
            this.cmdStartLog.Location = new System.Drawing.Point(18, 231);
            this.cmdStartLog.Name = "cmdStartLog";
            this.cmdStartLog.Size = new System.Drawing.Size(94, 33);
            this.cmdStartLog.TabIndex = 8;
            this.cmdStartLog.Text = "Start Log";
            this.cmdStartLog.UseVisualStyleBackColor = true;
            this.cmdStartLog.Click += new System.EventHandler(this.cmdStartLog_Click);
            // 
            // cmdImportConf
            // 
            this.cmdImportConf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdImportConf.AutoEllipsis = true;
            this.cmdImportConf.Location = new System.Drawing.Point(18, 347);
            this.cmdImportConf.Name = "cmdImportConf";
            this.cmdImportConf.Size = new System.Drawing.Size(186, 33);
            this.cmdImportConf.TabIndex = 7;
            this.cmdImportConf.Text = "Import Stored Config";
            this.cmdImportConf.UseVisualStyleBackColor = true;
            this.cmdImportConf.Click += new System.EventHandler(this.cmdImportConf_Click);
            // 
            // cmdSaveUpload
            // 
            this.cmdSaveUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSaveUpload.AutoEllipsis = true;
            this.cmdSaveUpload.Location = new System.Drawing.Point(18, 308);
            this.cmdSaveUpload.Name = "cmdSaveUpload";
            this.cmdSaveUpload.Size = new System.Drawing.Size(186, 33);
            this.cmdSaveUpload.TabIndex = 6;
            this.cmdSaveUpload.Text = "Save and Upload";
            this.cmdSaveUpload.UseVisualStyleBackColor = true;
            this.cmdSaveUpload.Click += new System.EventHandler(this.cmdSaveUpload_Click);
            // 
            // dgvInputSetup
            // 
            this.dgvInputSetup.AllowUserToAddRows = false;
            this.dgvInputSetup.AllowUserToDeleteRows = false;
            this.dgvInputSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInputSetup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInputSetup.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvInputSetup.ColumnHeadersHeight = 42;
            this.dgvInputSetup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
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
            this.dgvInputSetup.Location = new System.Drawing.Point(214, 0);
            this.dgvInputSetup.Name = "dgvInputSetup";
            this.dgvInputSetup.RowHeadersVisible = false;
            this.dgvInputSetup.RowHeadersWidth = 51;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInputSetup.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInputSetup.Size = new System.Drawing.Size(614, 391);
            this.dgvInputSetup.TabIndex = 5;
            this.dgvInputSetup.SizeChanged += new System.EventHandler(this.dgvInputSetup_SizeChanged);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.id.HeaderText = "Number";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 85;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // enabled
            // 
            this.enabled.HeaderText = "Enabled";
            this.enabled.MinimumWidth = 6;
            this.enabled.Name = "enabled";
            this.enabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.enabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // fName
            // 
            this.fName.HeaderText = "Friendly Name";
            this.fName.MinimumWidth = 6;
            this.fName.Name = "fName";
            // 
            // inputType
            // 
            this.inputType.HeaderText = "Input Type";
            this.inputType.MinimumWidth = 6;
            this.inputType.Name = "inputType";
            this.inputType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.inputType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // gain
            // 
            this.gain.HeaderText = "Gain";
            this.gain.MinimumWidth = 6;
            this.gain.Name = "gain";
            this.gain.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // scaleMin
            // 
            this.scaleMin.HeaderText = "Scale Minimum";
            this.scaleMin.MinimumWidth = 6;
            this.scaleMin.Name = "scaleMin";
            // 
            // scaleMax
            // 
            this.scaleMax.HeaderText = "Scale Maximum";
            this.scaleMax.MinimumWidth = 6;
            this.scaleMax.Name = "scaleMax";
            // 
            // units
            // 
            this.units.HeaderText = "Units";
            this.units.MinimumWidth = 6;
            this.units.Name = "units";
            this.units.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.units.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSave.Location = new System.Drawing.Point(18, 270);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(186, 33);
            this.cmdSave.TabIndex = 4;
            this.cmdSave.Text = "Save and Don\'t Upload";
            this.cmdSave.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Time Interval:";
            // 
            // nudInterval
            // 
            this.nudInterval.AutoSize = true;
            this.nudInterval.DecimalPlaces = 1;
            this.nudInterval.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudInterval.Location = new System.Drawing.Point(98, 66);
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
            this.nudInterval.Size = new System.Drawing.Size(106, 20);
            this.nudInterval.TabIndex = 2;
            this.nudInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Log Name:";
            // 
            // txtLogName
            // 
            this.txtLogName.Location = new System.Drawing.Point(97, 35);
            this.txtLogName.Name = "txtLogName";
            this.txtLogName.Size = new System.Drawing.Size(106, 20);
            this.txtLogName.TabIndex = 0;
            // 
            // cmdCtrlConf
            // 
            this.cmdCtrlConf.Location = new System.Drawing.Point(13, 12);
            this.cmdCtrlConf.Name = "cmdCtrlConf";
            this.cmdCtrlConf.Size = new System.Drawing.Size(125, 22);
            this.cmdCtrlConf.TabIndex = 1;
            this.cmdCtrlConf.Text = "Control/Config";
            this.cmdCtrlConf.UseVisualStyleBackColor = true;
            this.cmdCtrlConf.Click += new System.EventHandler(this.cmdCtrlConf_Click);
            // 
            // cmdDataProc
            // 
            this.cmdDataProc.Location = new System.Drawing.Point(144, 12);
            this.cmdDataProc.Name = "cmdDataProc";
            this.cmdDataProc.Size = new System.Drawing.Size(130, 22);
            this.cmdDataProc.TabIndex = 2;
            this.cmdDataProc.Text = "Download/Process Data";
            this.cmdDataProc.UseVisualStyleBackColor = true;
            this.cmdDataProc.Click += new System.EventHandler(this.cmdDataProc_Click);
            // 
            // cmdSettings
            // 
            this.cmdSettings.Location = new System.Drawing.Point(281, 12);
            this.cmdSettings.Name = "cmdSettings";
            this.cmdSettings.Size = new System.Drawing.Size(112, 22);
            this.cmdSettings.TabIndex = 3;
            this.cmdSettings.Text = "Settings";
            this.cmdSettings.UseVisualStyleBackColor = true;
            this.cmdSettings.Click += new System.EventHandler(this.cmdSettings_Click);
            // 
            // cmdAbt
            // 
            this.cmdAbt.Location = new System.Drawing.Point(399, 12);
            this.cmdAbt.Name = "cmdAbt";
            this.cmdAbt.Size = new System.Drawing.Size(100, 22);
            this.cmdAbt.TabIndex = 4;
            this.cmdAbt.Text = "About/Help";
            this.cmdAbt.UseVisualStyleBackColor = true;
            this.cmdAbt.Click += new System.EventHandler(this.cmdAbt_Click);
            // 
            // pnlDataProc
            // 
            this.pnlDataProc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDataProc.Controls.Add(this.cmdClearData);
            this.pnlDataProc.Controls.Add(this.cmdDwnldCsv);
            this.pnlDataProc.Controls.Add(this.cmdExpExcel);
            this.pnlDataProc.Controls.Add(this.cmdDwnldZip);
            this.pnlDataProc.Controls.Add(this.cmdImportLogFile);
            this.pnlDataProc.Controls.Add(this.cmdImportLogPi);
            this.pnlDataProc.Controls.Add(this.cmdPythonGraph);
            this.pnlDataProc.Controls.Add(this.cmdPythonScript);
            this.pnlDataProc.Controls.Add(this.dgvDataProc);
            this.pnlDataProc.Location = new System.Drawing.Point(13, 40);
            this.pnlDataProc.Name = "pnlDataProc";
            this.pnlDataProc.Size = new System.Drawing.Size(828, 391);
            this.pnlDataProc.TabIndex = 5;
            // 
            // cmdClearData
            // 
            this.cmdClearData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdClearData.Location = new System.Drawing.Point(17, 352);
            this.cmdClearData.Name = "cmdClearData";
            this.cmdClearData.Size = new System.Drawing.Size(186, 29);
            this.cmdClearData.TabIndex = 4;
            this.cmdClearData.Text = "Clear Data View";
            this.cmdClearData.UseVisualStyleBackColor = true;
            this.cmdClearData.Click += new System.EventHandler(this.cmdClearData_Click);
            // 
            // cmdDwnldCsv
            // 
            this.cmdDwnldCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdDwnldCsv.Location = new System.Drawing.Point(17, 246);
            this.cmdDwnldCsv.Name = "cmdDwnldCsv";
            this.cmdDwnldCsv.Size = new System.Drawing.Size(186, 29);
            this.cmdDwnldCsv.TabIndex = 0;
            this.cmdDwnldCsv.Text = "Save as CSV/s";
            this.cmdDwnldCsv.UseVisualStyleBackColor = true;
            this.cmdDwnldCsv.Click += new System.EventHandler(this.cmdDwnldCsv_Click);
            // 
            // cmdExpExcel
            // 
            this.cmdExpExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdExpExcel.Location = new System.Drawing.Point(17, 317);
            this.cmdExpExcel.Name = "cmdExpExcel";
            this.cmdExpExcel.Size = new System.Drawing.Size(186, 29);
            this.cmdExpExcel.TabIndex = 2;
            this.cmdExpExcel.Text = "Export to Excel";
            this.cmdExpExcel.UseVisualStyleBackColor = true;
            this.cmdExpExcel.Click += new System.EventHandler(this.cmdExpExcel_Click);
            // 
            // cmdDwnldZip
            // 
            this.cmdDwnldZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdDwnldZip.Location = new System.Drawing.Point(17, 282);
            this.cmdDwnldZip.Name = "cmdDwnldZip";
            this.cmdDwnldZip.Size = new System.Drawing.Size(186, 29);
            this.cmdDwnldZip.TabIndex = 1;
            this.cmdDwnldZip.Text = "Save as CSV/s in Zip";
            this.cmdDwnldZip.UseVisualStyleBackColor = true;
            this.cmdDwnldZip.Click += new System.EventHandler(this.cmdDwnldZip_Click);
            // 
            // cmdImportLogFile
            // 
            this.cmdImportLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdImportLogFile.Location = new System.Drawing.Point(17, 212);
            this.cmdImportLogFile.Name = "cmdImportLogFile";
            this.cmdImportLogFile.Size = new System.Drawing.Size(186, 29);
            this.cmdImportLogFile.TabIndex = 5;
            this.cmdImportLogFile.Text = "Import Log From File";
            this.cmdImportLogFile.UseVisualStyleBackColor = true;
            this.cmdImportLogFile.Click += new System.EventHandler(this.cmdImportLogFile_Click);
            // 
            // cmdImportLogPi
            // 
            this.cmdImportLogPi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdImportLogPi.Location = new System.Drawing.Point(17, 177);
            this.cmdImportLogPi.Name = "cmdImportLogPi";
            this.cmdImportLogPi.Size = new System.Drawing.Size(186, 29);
            this.cmdImportLogPi.TabIndex = 6;
            this.cmdImportLogPi.Text = "Import Log From Pi";
            this.cmdImportLogPi.UseVisualStyleBackColor = true;
            this.cmdImportLogPi.Click += new System.EventHandler(this.cmdImportLogPi_Click);
            // 
            // cmdPythonGraph
            // 
            this.cmdPythonGraph.Location = new System.Drawing.Point(17, 42);
            this.cmdPythonGraph.Name = "cmdPythonGraph";
            this.cmdPythonGraph.Size = new System.Drawing.Size(186, 29);
            this.cmdPythonGraph.TabIndex = 3;
            this.cmdPythonGraph.Text = "Graph using Python Script";
            this.cmdPythonGraph.UseVisualStyleBackColor = true;
            this.cmdPythonGraph.Click += new System.EventHandler(this.cmdPythonGraph_Click);
            // 
            // cmdPythonScript
            // 
            this.cmdPythonScript.Location = new System.Drawing.Point(17, 6);
            this.cmdPythonScript.Name = "cmdPythonScript";
            this.cmdPythonScript.Size = new System.Drawing.Size(186, 29);
            this.cmdPythonScript.TabIndex = 2;
            this.cmdPythonScript.Text = "Process using Python Script";
            this.cmdPythonScript.UseVisualStyleBackColor = true;
            this.cmdPythonScript.Click += new System.EventHandler(this.cmdPythonScript_Click);
            // 
            // dgvDataProc
            // 
            this.dgvDataProc.AllowUserToAddRows = false;
            this.dgvDataProc.AllowUserToDeleteRows = false;
            this.dgvDataProc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataProc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataProc.Location = new System.Drawing.Point(210, 3);
            this.dgvDataProc.Name = "dgvDataProc";
            this.dgvDataProc.ReadOnly = true;
            this.dgvDataProc.RowHeadersVisible = false;
            this.dgvDataProc.RowHeadersWidth = 51;
            this.dgvDataProc.Size = new System.Drawing.Size(614, 385);
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
            this.lblConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(519, 17);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(119, 13);
            this.lblConnection.TabIndex = 6;
            this.lblConnection.Text = " You are connected to: ";
            // 
            // cmdConnect
            // 
            this.cmdConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdConnect.Location = new System.Drawing.Point(758, 12);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(80, 22);
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
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(855, 450);
            this.Controls.Add(this.cmdConnect);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.cmdAbt);
            this.Controls.Add(this.cmdSettings);
            this.Controls.Add(this.cmdDataProc);
            this.Controls.Add(this.cmdCtrlConf);
            this.Controls.Add(this.pnlCtrlConf);
            this.Controls.Add(this.pnlDataProc);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(871, 489);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn fName;
        private System.Windows.Forms.DataGridViewComboBoxColumn inputType;
        private System.Windows.Forms.DataGridViewComboBoxColumn gain;
        private System.Windows.Forms.DataGridViewTextBoxColumn scaleMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn scaleMax;
        private System.Windows.Forms.DataGridViewComboBoxColumn units;
    }
}