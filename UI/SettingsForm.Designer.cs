
namespace SteerLoggerUser
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.lblActivate = new System.Windows.Forms.Label();
            this.ofdFindActivate = new System.Windows.Forms.OpenFileDialog();
            this.cmdFindActivate = new System.Windows.Forms.Button();
            this.lblUnits = new System.Windows.Forms.Label();
            this.cmbUnits = new System.Windows.Forms.ComboBox();
            this.cmdAddUnit = new System.Windows.Forms.Button();
            this.txtAddUnit = new System.Windows.Forms.TextBox();
            this.lblInputTypes = new System.Windows.Forms.Label();
            this.cmbInputTypes = new System.Windows.Forms.ComboBox();
            this.lblBottomVolt = new System.Windows.Forms.Label();
            this.txtBottomVolt = new System.Windows.Forms.TextBox();
            this.lblTopVolt = new System.Windows.Forms.Label();
            this.txtTopVolt = new System.Windows.Forms.TextBox();
            this.cmdAddInputType = new System.Windows.Forms.Button();
            this.cmbGains = new System.Windows.Forms.ComboBox();
            this.lblGains = new System.Windows.Forms.Label();
            this.lblLoggers = new System.Windows.Forms.Label();
            this.cmbLoggers = new System.Windows.Forms.ComboBox();
            this.txtLogger = new System.Windows.Forms.TextBox();
            this.cmdAddLogger = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtActivateLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvPresets = new System.Windows.Forms.DataGridView();
            this.Sensor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Variation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Gain = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ScaleMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScaleMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmdExportDatabase = new System.Windows.Forms.Button();
            this.sfdSaveDatabase = new System.Windows.Forms.SaveFileDialog();
            this.cmdCopyPiData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPresets)).BeginInit();
            this.SuspendLayout();
            // 
            // lblActivate
            // 
            this.lblActivate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblActivate.AutoSize = true;
            this.lblActivate.Location = new System.Drawing.Point(12, 16);
            this.lblActivate.Name = "lblActivate";
            this.lblActivate.Size = new System.Drawing.Size(158, 13);
            this.lblActivate.TabIndex = 0;
            this.lblActivate.Text = "Anaconda activate.bat location:";
            // 
            // cmdFindActivate
            // 
            this.cmdFindActivate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdFindActivate.Location = new System.Drawing.Point(176, 12);
            this.cmdFindActivate.Name = "cmdFindActivate";
            this.cmdFindActivate.Size = new System.Drawing.Size(130, 23);
            this.cmdFindActivate.TabIndex = 2;
            this.cmdFindActivate.Text = "Find activate.bat";
            this.cmdFindActivate.UseVisualStyleBackColor = true;
            this.cmdFindActivate.Click += new System.EventHandler(this.cmdFindActivate_Click);
            // 
            // lblUnits
            // 
            this.lblUnits.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblUnits.AutoSize = true;
            this.lblUnits.Location = new System.Drawing.Point(12, 71);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new System.Drawing.Size(34, 13);
            this.lblUnits.TabIndex = 3;
            this.lblUnits.Text = "Units:";
            // 
            // cmbUnits
            // 
            this.cmbUnits.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            this.cmbUnits.Location = new System.Drawing.Point(57, 68);
            this.cmbUnits.Name = "cmbUnits";
            this.cmbUnits.Size = new System.Drawing.Size(249, 21);
            this.cmbUnits.TabIndex = 4;
            // 
            // cmdAddUnit
            // 
            this.cmdAddUnit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdAddUnit.Location = new System.Drawing.Point(235, 95);
            this.cmdAddUnit.Name = "cmdAddUnit";
            this.cmdAddUnit.Size = new System.Drawing.Size(71, 23);
            this.cmdAddUnit.TabIndex = 5;
            this.cmdAddUnit.Text = "Add Unit";
            this.cmdAddUnit.UseVisualStyleBackColor = true;
            this.cmdAddUnit.Click += new System.EventHandler(this.cmdAddUnit_Click);
            // 
            // txtAddUnit
            // 
            this.txtAddUnit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtAddUnit.Location = new System.Drawing.Point(57, 97);
            this.txtAddUnit.Name = "txtAddUnit";
            this.txtAddUnit.Size = new System.Drawing.Size(172, 20);
            this.txtAddUnit.TabIndex = 6;
            // 
            // lblInputTypes
            // 
            this.lblInputTypes.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInputTypes.AutoSize = true;
            this.lblInputTypes.Location = new System.Drawing.Point(12, 127);
            this.lblInputTypes.Name = "lblInputTypes";
            this.lblInputTypes.Size = new System.Drawing.Size(66, 13);
            this.lblInputTypes.TabIndex = 7;
            this.lblInputTypes.Text = "Input Types:";
            // 
            // cmbInputTypes
            // 
            this.cmbInputTypes.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbInputTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInputTypes.FormattingEnabled = true;
            this.cmbInputTypes.Location = new System.Drawing.Point(84, 124);
            this.cmbInputTypes.Name = "cmbInputTypes";
            this.cmbInputTypes.Size = new System.Drawing.Size(222, 21);
            this.cmbInputTypes.TabIndex = 8;
            // 
            // lblBottomVolt
            // 
            this.lblBottomVolt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBottomVolt.AutoSize = true;
            this.lblBottomVolt.Location = new System.Drawing.Point(81, 180);
            this.lblBottomVolt.Name = "lblBottomVolt";
            this.lblBottomVolt.Size = new System.Drawing.Size(64, 13);
            this.lblBottomVolt.TabIndex = 9;
            this.lblBottomVolt.Text = "Bottom Volt:";
            // 
            // txtBottomVolt
            // 
            this.txtBottomVolt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtBottomVolt.Location = new System.Drawing.Point(154, 177);
            this.txtBottomVolt.Name = "txtBottomVolt";
            this.txtBottomVolt.Size = new System.Drawing.Size(152, 20);
            this.txtBottomVolt.TabIndex = 10;
            // 
            // lblTopVolt
            // 
            this.lblTopVolt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTopVolt.AutoSize = true;
            this.lblTopVolt.Location = new System.Drawing.Point(81, 206);
            this.lblTopVolt.Name = "lblTopVolt";
            this.lblTopVolt.Size = new System.Drawing.Size(50, 13);
            this.lblTopVolt.TabIndex = 11;
            this.lblTopVolt.Text = "Top Volt:";
            // 
            // txtTopVolt
            // 
            this.txtTopVolt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTopVolt.Location = new System.Drawing.Point(154, 203);
            this.txtTopVolt.Name = "txtTopVolt";
            this.txtTopVolt.Size = new System.Drawing.Size(152, 20);
            this.txtTopVolt.TabIndex = 12;
            // 
            // cmdAddInputType
            // 
            this.cmdAddInputType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdAddInputType.Location = new System.Drawing.Point(84, 229);
            this.cmdAddInputType.Name = "cmdAddInputType";
            this.cmdAddInputType.Size = new System.Drawing.Size(222, 23);
            this.cmdAddInputType.TabIndex = 13;
            this.cmdAddInputType.Text = "Add Input Type";
            this.cmdAddInputType.UseVisualStyleBackColor = true;
            this.cmdAddInputType.Click += new System.EventHandler(this.cmdAddInputType_Click);
            // 
            // cmbGains
            // 
            this.cmbGains.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbGains.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGains.FormattingEnabled = true;
            this.cmbGains.Location = new System.Drawing.Point(55, 260);
            this.cmbGains.Name = "cmbGains";
            this.cmbGains.Size = new System.Drawing.Size(251, 21);
            this.cmbGains.TabIndex = 14;
            // 
            // lblGains
            // 
            this.lblGains.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblGains.AutoSize = true;
            this.lblGains.Location = new System.Drawing.Point(12, 262);
            this.lblGains.Name = "lblGains";
            this.lblGains.Size = new System.Drawing.Size(37, 13);
            this.lblGains.TabIndex = 15;
            this.lblGains.Text = "Gains:";
            // 
            // lblLoggers
            // 
            this.lblLoggers.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLoggers.AutoSize = true;
            this.lblLoggers.Location = new System.Drawing.Point(12, 290);
            this.lblLoggers.Name = "lblLoggers";
            this.lblLoggers.Size = new System.Drawing.Size(48, 13);
            this.lblLoggers.TabIndex = 16;
            this.lblLoggers.Text = "Loggers:";
            // 
            // cmbLoggers
            // 
            this.cmbLoggers.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbLoggers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoggers.FormattingEnabled = true;
            this.cmbLoggers.Location = new System.Drawing.Point(66, 287);
            this.cmbLoggers.Name = "cmbLoggers";
            this.cmbLoggers.Size = new System.Drawing.Size(240, 21);
            this.cmbLoggers.TabIndex = 17;
            // 
            // txtLogger
            // 
            this.txtLogger.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLogger.Location = new System.Drawing.Point(66, 316);
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.Size = new System.Drawing.Size(159, 20);
            this.txtLogger.TabIndex = 18;
            // 
            // cmdAddLogger
            // 
            this.cmdAddLogger.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdAddLogger.Location = new System.Drawing.Point(231, 314);
            this.cmdAddLogger.Name = "cmdAddLogger";
            this.cmdAddLogger.Size = new System.Drawing.Size(75, 23);
            this.cmdAddLogger.TabIndex = 19;
            this.cmdAddLogger.Text = "Add Logger";
            this.cmdAddLogger.UseVisualStyleBackColor = true;
            this.cmdAddLogger.Click += new System.EventHandler(this.cmdAddLogger_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSave.Location = new System.Drawing.Point(12, 598);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(143, 23);
            this.cmdSave.TabIndex = 20;
            this.cmdSave.Text = "Save Changes";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(173, 598);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(133, 23);
            this.cmdCancel.TabIndex = 21;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtName.Location = new System.Drawing.Point(154, 151);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(152, 20);
            this.txtName.TabIndex = 22;
            // 
            // lblName
            // 
            this.lblName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(81, 154);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 23;
            this.lblName.Text = "Name:";
            // 
            // txtActivateLocation
            // 
            this.txtActivateLocation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtActivateLocation.Location = new System.Drawing.Point(15, 41);
            this.txtActivateLocation.Name = "txtActivateLocation";
            this.txtActivateLocation.ReadOnly = true;
            this.txtActivateLocation.Size = new System.Drawing.Size(291, 20);
            this.txtActivateLocation.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 341);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Presets:";
            // 
            // dgvPresets
            // 
            this.dgvPresets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPresets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPresets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sensor,
            this.Variation,
            this.FName,
            this.InputType,
            this.Gain,
            this.ScaleMin,
            this.ScaleMax,
            this.Units});
            this.dgvPresets.Location = new System.Drawing.Point(12, 357);
            this.dgvPresets.Name = "dgvPresets";
            this.dgvPresets.Size = new System.Drawing.Size(294, 171);
            this.dgvPresets.TabIndex = 26;
            // 
            // Sensor
            // 
            this.Sensor.HeaderText = "Sensor";
            this.Sensor.Name = "Sensor";
            // 
            // Variation
            // 
            this.Variation.HeaderText = "Variation";
            this.Variation.Name = "Variation";
            // 
            // FName
            // 
            this.FName.HeaderText = "FName";
            this.FName.Name = "FName";
            // 
            // InputType
            // 
            this.InputType.HeaderText = "InputType";
            this.InputType.Name = "InputType";
            // 
            // Gain
            // 
            this.Gain.HeaderText = "Gain";
            this.Gain.Name = "Gain";
            // 
            // ScaleMin
            // 
            this.ScaleMin.HeaderText = "ScaleMin";
            this.ScaleMin.Name = "ScaleMin";
            // 
            // ScaleMax
            // 
            this.ScaleMax.HeaderText = "ScaleMax";
            this.ScaleMax.Name = "ScaleMax";
            // 
            // Units
            // 
            this.Units.HeaderText = "Units";
            this.Units.Name = "Units";
            // 
            // cmdExportDatabase
            // 
            this.cmdExportDatabase.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdExportDatabase.Location = new System.Drawing.Point(12, 534);
            this.cmdExportDatabase.Name = "cmdExportDatabase";
            this.cmdExportDatabase.Size = new System.Drawing.Size(294, 23);
            this.cmdExportDatabase.TabIndex = 27;
            this.cmdExportDatabase.Text = "Export Copy of Database";
            this.cmdExportDatabase.UseVisualStyleBackColor = true;
            this.cmdExportDatabase.Click += new System.EventHandler(this.cmdExportDatabase_Click);
            // 
            // cmdCopyPiData
            // 
            this.cmdCopyPiData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdCopyPiData.Location = new System.Drawing.Point(12, 563);
            this.cmdCopyPiData.Name = "cmdCopyPiData";
            this.cmdCopyPiData.Size = new System.Drawing.Size(294, 23);
            this.cmdCopyPiData.TabIndex = 28;
            this.cmdCopyPiData.Text = "Export Copy of All Logger Data";
            this.cmdCopyPiData.UseVisualStyleBackColor = true;
            this.cmdCopyPiData.Click += new System.EventHandler(this.cmdCopyPiData_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 633);
            this.Controls.Add(this.cmdCopyPiData);
            this.Controls.Add(this.cmdExportDatabase);
            this.Controls.Add(this.dgvPresets);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtActivateLocation);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdAddLogger);
            this.Controls.Add(this.txtLogger);
            this.Controls.Add(this.cmbLoggers);
            this.Controls.Add(this.lblLoggers);
            this.Controls.Add(this.lblGains);
            this.Controls.Add(this.cmbGains);
            this.Controls.Add(this.cmdAddInputType);
            this.Controls.Add(this.txtTopVolt);
            this.Controls.Add(this.lblTopVolt);
            this.Controls.Add(this.txtBottomVolt);
            this.Controls.Add(this.lblBottomVolt);
            this.Controls.Add(this.cmbInputTypes);
            this.Controls.Add(this.lblInputTypes);
            this.Controls.Add(this.txtAddUnit);
            this.Controls.Add(this.cmdAddUnit);
            this.Controls.Add(this.cmbUnits);
            this.Controls.Add(this.lblUnits);
            this.Controls.Add(this.cmdFindActivate);
            this.Controls.Add(this.lblActivate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(334, 637);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPresets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblActivate;
        private System.Windows.Forms.OpenFileDialog ofdFindActivate;
        private System.Windows.Forms.Button cmdFindActivate;
        private System.Windows.Forms.Label lblUnits;
        private System.Windows.Forms.ComboBox cmbUnits;
        private System.Windows.Forms.Button cmdAddUnit;
        private System.Windows.Forms.TextBox txtAddUnit;
        private System.Windows.Forms.Label lblInputTypes;
        private System.Windows.Forms.ComboBox cmbInputTypes;
        private System.Windows.Forms.Label lblBottomVolt;
        private System.Windows.Forms.TextBox txtBottomVolt;
        private System.Windows.Forms.Label lblTopVolt;
        private System.Windows.Forms.TextBox txtTopVolt;
        private System.Windows.Forms.Button cmdAddInputType;
        private System.Windows.Forms.ComboBox cmbGains;
        private System.Windows.Forms.Label lblGains;
        private System.Windows.Forms.Label lblLoggers;
        private System.Windows.Forms.ComboBox cmbLoggers;
        private System.Windows.Forms.TextBox txtLogger;
        private System.Windows.Forms.Button cmdAddLogger;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtActivateLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPresets;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sensor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variation;
        private System.Windows.Forms.DataGridViewTextBoxColumn FName;
        private System.Windows.Forms.DataGridViewComboBoxColumn InputType;
        private System.Windows.Forms.DataGridViewComboBoxColumn Gain;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScaleMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScaleMax;
        private System.Windows.Forms.DataGridViewComboBoxColumn Units;
        private System.Windows.Forms.Button cmdExportDatabase;
        private System.Windows.Forms.SaveFileDialog sfdSaveDatabase;
        private System.Windows.Forms.Button cmdCopyPiData;
    }
}