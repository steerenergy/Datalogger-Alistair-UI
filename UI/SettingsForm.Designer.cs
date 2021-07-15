﻿
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
            this.SuspendLayout();
            // 
            // lblActivate
            // 
            this.lblActivate.AutoSize = true;
            this.lblActivate.Location = new System.Drawing.Point(12, 16);
            this.lblActivate.Name = "lblActivate";
            this.lblActivate.Size = new System.Drawing.Size(158, 13);
            this.lblActivate.TabIndex = 0;
            this.lblActivate.Text = "Anaconda activate.bat location:";
            // 
            // cmdFindActivate
            // 
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
            this.lblUnits.AutoSize = true;
            this.lblUnits.Location = new System.Drawing.Point(12, 71);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new System.Drawing.Size(34, 13);
            this.lblUnits.TabIndex = 3;
            this.lblUnits.Text = "Units:";
            // 
            // cmbUnits
            // 
            this.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            this.cmbUnits.Location = new System.Drawing.Point(57, 68);
            this.cmbUnits.Name = "cmbUnits";
            this.cmbUnits.Size = new System.Drawing.Size(249, 21);
            this.cmbUnits.TabIndex = 4;
            // 
            // cmdAddUnit
            // 
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
            this.txtAddUnit.Location = new System.Drawing.Point(57, 97);
            this.txtAddUnit.Name = "txtAddUnit";
            this.txtAddUnit.Size = new System.Drawing.Size(172, 20);
            this.txtAddUnit.TabIndex = 6;
            // 
            // lblInputTypes
            // 
            this.lblInputTypes.AutoSize = true;
            this.lblInputTypes.Location = new System.Drawing.Point(12, 127);
            this.lblInputTypes.Name = "lblInputTypes";
            this.lblInputTypes.Size = new System.Drawing.Size(66, 13);
            this.lblInputTypes.TabIndex = 7;
            this.lblInputTypes.Text = "Input Types:";
            // 
            // cmbInputTypes
            // 
            this.cmbInputTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInputTypes.FormattingEnabled = true;
            this.cmbInputTypes.Location = new System.Drawing.Point(84, 124);
            this.cmbInputTypes.Name = "cmbInputTypes";
            this.cmbInputTypes.Size = new System.Drawing.Size(222, 21);
            this.cmbInputTypes.TabIndex = 8;
            // 
            // lblBottomVolt
            // 
            this.lblBottomVolt.AutoSize = true;
            this.lblBottomVolt.Location = new System.Drawing.Point(81, 180);
            this.lblBottomVolt.Name = "lblBottomVolt";
            this.lblBottomVolt.Size = new System.Drawing.Size(64, 13);
            this.lblBottomVolt.TabIndex = 9;
            this.lblBottomVolt.Text = "Bottom Volt:";
            // 
            // txtBottomVolt
            // 
            this.txtBottomVolt.Location = new System.Drawing.Point(154, 177);
            this.txtBottomVolt.Name = "txtBottomVolt";
            this.txtBottomVolt.Size = new System.Drawing.Size(152, 20);
            this.txtBottomVolt.TabIndex = 10;
            // 
            // lblTopVolt
            // 
            this.lblTopVolt.AutoSize = true;
            this.lblTopVolt.Location = new System.Drawing.Point(81, 206);
            this.lblTopVolt.Name = "lblTopVolt";
            this.lblTopVolt.Size = new System.Drawing.Size(50, 13);
            this.lblTopVolt.TabIndex = 11;
            this.lblTopVolt.Text = "Top Volt:";
            // 
            // txtTopVolt
            // 
            this.txtTopVolt.Location = new System.Drawing.Point(154, 203);
            this.txtTopVolt.Name = "txtTopVolt";
            this.txtTopVolt.Size = new System.Drawing.Size(152, 20);
            this.txtTopVolt.TabIndex = 12;
            // 
            // cmdAddInputType
            // 
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
            this.cmbGains.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGains.FormattingEnabled = true;
            this.cmbGains.Location = new System.Drawing.Point(55, 260);
            this.cmbGains.Name = "cmbGains";
            this.cmbGains.Size = new System.Drawing.Size(251, 21);
            this.cmbGains.TabIndex = 14;
            // 
            // lblGains
            // 
            this.lblGains.AutoSize = true;
            this.lblGains.Location = new System.Drawing.Point(12, 262);
            this.lblGains.Name = "lblGains";
            this.lblGains.Size = new System.Drawing.Size(37, 13);
            this.lblGains.TabIndex = 15;
            this.lblGains.Text = "Gains:";
            // 
            // lblLoggers
            // 
            this.lblLoggers.AutoSize = true;
            this.lblLoggers.Location = new System.Drawing.Point(12, 290);
            this.lblLoggers.Name = "lblLoggers";
            this.lblLoggers.Size = new System.Drawing.Size(48, 13);
            this.lblLoggers.TabIndex = 16;
            this.lblLoggers.Text = "Loggers:";
            // 
            // cmbLoggers
            // 
            this.cmbLoggers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoggers.FormattingEnabled = true;
            this.cmbLoggers.Location = new System.Drawing.Point(66, 287);
            this.cmbLoggers.Name = "cmbLoggers";
            this.cmbLoggers.Size = new System.Drawing.Size(240, 21);
            this.cmbLoggers.TabIndex = 17;
            // 
            // txtLogger
            // 
            this.txtLogger.Location = new System.Drawing.Point(66, 316);
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.Size = new System.Drawing.Size(159, 20);
            this.txtLogger.TabIndex = 18;
            // 
            // cmdAddLogger
            // 
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
            this.cmdSave.Location = new System.Drawing.Point(12, 371);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(143, 23);
            this.cmdSave.TabIndex = 20;
            this.cmdSave.Text = "Save Changes";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(173, 371);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(133, 23);
            this.cmdCancel.TabIndex = 21;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(154, 151);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(152, 20);
            this.txtName.TabIndex = 22;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(81, 154);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 23;
            this.lblName.Text = "Name:";
            // 
            // txtActivateLocation
            // 
            this.txtActivateLocation.Location = new System.Drawing.Point(15, 41);
            this.txtActivateLocation.Name = "txtActivateLocation";
            this.txtActivateLocation.ReadOnly = true;
            this.txtActivateLocation.Size = new System.Drawing.Size(290, 20);
            this.txtActivateLocation.TabIndex = 24;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 406);
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
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
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
    }
}