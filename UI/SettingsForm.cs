using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SteerLoggerUser
{
    public partial class SettingsForm : Form
    {
        private ProgConfig config;
        public SettingsForm(ProgConfig currentConfig)
        {
            this.config = currentConfig;
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            txtActivateLocation.Text = config.activatePath;

            foreach (string unit in config.units)
            {
                cmbUnits.Items.Add(unit);
            }
            cmbUnits.SelectedIndex = 0;

            foreach (string inputType in config.inputTypes.Keys)
            {
                cmbInputTypes.Items.Add(inputType + ": ("
                    + config.inputTypes[inputType][0].ToString() + ","
                    + config.inputTypes[inputType][1].ToString() + ")");
            }
            cmbInputTypes.SelectedIndex = 0;

            foreach (int gain in config.gains.Keys)
            {
                cmbGains.Items.Add(gain.ToString());
            }
            cmbGains.SelectedIndex = 0;

            foreach (string logger in config.loggers)
            {
                cmbLoggers.Items.Add(logger);
            }
            cmbLoggers.SelectedIndex = 0;

            // Use progConfig to populate the InputSetup grid view correctly
            foreach (string key in config.inputTypes.Keys)
            {
                ((DataGridViewComboBoxColumn)dgvPresets.Columns["inputType"]).Items.Add(key);
            }

            foreach (int gain in config.gains.Keys)
            {
                ((DataGridViewComboBoxColumn)dgvPresets.Columns["gain"]).Items.Add(gain.ToString());
            }

            foreach (string value in config.units)
            {
                ((DataGridViewComboBoxColumn)dgvPresets.Columns["units"]).Items.Add(value);
            }

            foreach (string pin in config.configPins.Keys)
            {
                object[] row =
                {
                    pin.Split(',')[0],
                    (pin.Split(',')[1] == "") ? "N/A" : pin.Split(',')[1],
                    config.configPins[pin].fName,
                    config.configPins[pin].inputType,
                    config.configPins[pin].gain.ToString(),
                    config.configPins[pin].scaleMin,
                    config.configPins[pin].scaleMax,
                    config.configPins[pin].units
                };
                dgvPresets.Rows.Add(row);
            }
        }

        private void cmdFindActivate_Click(object sender, EventArgs e)
        {
            if (ofdFindActivate.ShowDialog() == DialogResult.OK)
            {
                if (ofdFindActivate.SafeFileName != "activate.bat")
                {
                    MessageBox.Show("The file you selected was not called activate.bat, please check this is correct.");
                }
                config.activatePath = ofdFindActivate.FileName;
                txtActivateLocation.Text = config.activatePath;
            }
        }

        private void cmdAddUnit_Click(object sender, EventArgs e)
        {
            if (txtAddUnit.Text == "")
            {
                MessageBox.Show("Please write a new unit to add in the textbox!");
                return;
            }

            string newUnit = txtAddUnit.Text.Trim();
            config.units.Add(newUnit);
            cmbUnits.Items.Add(newUnit);
            cmbUnits.SelectedItem = newUnit;
        }

        private void cmdAddLogger_Click(object sender, EventArgs e)
        {
            if (txtLogger.Text == "")
            {
                MessageBox.Show("Please write a new logger to add in the textbox!");
                return;
            }

            string newLogger = txtLogger.Text.Trim();
            config.loggers.Add(newLogger);
            cmbLoggers.Items.Add(newLogger);
            cmbLoggers.SelectedItem = newLogger;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdAddInputType_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please write the name for the new input type in the textbox!");
                return;
            }

            if (Double.TryParse(txtBottomVolt.Text, out double bottomVolt) == false)
            {
                MessageBox.Show("Please makes sure your bottom voltage value is an integer or decimal.");
                return;
            }
            if (Double.TryParse(txtTopVolt.Text, out double topVolt) == false)
            {
                MessageBox.Show("Please makes sure your top voltage value is an integer or decimal.");
                return;
            }
            string name = txtName.Text.Trim();
            config.inputTypes.Add(name, new double[] { bottomVolt, topVolt });
            cmbInputTypes.Items.Add(name + ": ("
                        + bottomVolt.ToString() + ","
                        + topVolt.ToString() + ")");
            cmbInputTypes.SelectedItem = name + ": (" + bottomVolt.ToString() + "," + topVolt.ToString() + ")";
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            // Opens config using StreamReader
            using (StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\progConf.ini"))
            {
                string line = "";
                char[] trimChars = new char[] { '\n', ' ' };
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Trim(trimChars);
                    if (line == "" || line[0] == '#' || line[0] == '[')
                    {
                        lines.Add(line);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\progConf.ini"))
            {
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                    int num = 0;
                    switch (line)
                    {
                        case "[unitTypes]":
                            num = 0;
                            foreach (string unit in config.units)
                            {
                                writer.WriteLine(num.ToString() + " = " + unit);
                                num += 1;
                            }
                            break;
                        case "[inputTypes]":
                            foreach (string input in config.inputTypes.Keys)
                            {
                                writer.WriteLine(input + " = (" +
                                                config.inputTypes[input][0].ToString() + "," +
                                                config.inputTypes[input][1].ToString() + ")");
                            }
                            break;
                        case "[gains]":
                            foreach (int gain in config.gains.Keys)
                            {
                                writer.WriteLine(gain.ToString() + " = " + config.gains[gain].ToString());
                            }
                            break;
                        case "[hostnames]":
                            num = 0;
                            foreach (string logger in config.loggers)
                            {
                                writer.WriteLine(num.ToString() + " = " + logger);
                                num += 1;
                            }
                            break;
                        case "[activate]":
                            writer.WriteLine("path = " + config.activatePath);
                            break;
                        default:
                            break;
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\configPresets.csv"))
            {
                string line = "";
                foreach (DataGridViewColumn col in dgvPresets.Columns)
                {
                    line += col.HeaderText + ',';
                }
                writer.WriteLine(line.TrimEnd(','));
                for (int i = 0; i < dgvPresets.Rows.Count - 1; i ++)
                {
                    line = "";
                    foreach (DataGridViewCell cell in dgvPresets.Rows[i].Cells)
                    {
                        if (cell.Value == null)
                        {
                            MessageBox.Show("Make sure all preset cells have values filled in.\n" +
                                            "If there is no variation, please put N/A in the variation cell.");
                            return;
                        }
                        if (cell.Value.ToString() == "N/A")
                        {
                            line += ',';
                        }
                        else
                        {
                            line += cell.Value.ToString() + ',';
                        }
                    }
                    writer.WriteLine(line.TrimEnd(','));
                }
            }
            this.Close();
        }
    }
}
