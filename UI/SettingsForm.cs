using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SteerLoggerUser
{
    public partial class SettingsForm : Form
    {
        private ProgConfig config;
        private Action<string> TCPSend;
        private Func<string> TCPReceive;
        private string logger;

        public SettingsForm(ProgConfig currentConfig, Action<string> TCPSend, Func<string> TCPReceive, string logger)
        {
            this.config = currentConfig;
            this.TCPSend = TCPSend;
            this.TCPReceive = TCPReceive;
            this.logger = logger;
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
                for (int i = 0; i < dgvPresets.Rows.Count - 1; i++)
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

        private void cmdExportDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                sfdSaveDatabase.FileName = String.Format("{0}-Database-{1}.csv", this.logger, DateTime.Now.ToString("yyyymmdd-HHmmss"));
                sfdSaveDatabase.DefaultExt = "csv";
                sfdSaveDatabase.AddExtension = true;
                sfdSaveDatabase.Filter = "Csv files (*.csv)|*.zip|All files (*.*)|*.*";
                if (sfdSaveDatabase.ShowDialog() == DialogResult.OK)
                {
                    TCPSend("Export_Database");
                    using (StreamWriter writer = new StreamWriter(sfdSaveDatabase.FileName))
                    {
                        writer.WriteLine(TCPReceive().Replace('\u001f', ','));
                        int numRows = Convert.ToInt32(TCPReceive());
                        for (int i = 0; i < numRows; i++)
                        {
                            writer.WriteLine(TCPReceive().Replace(',', ';').Replace('\u001f', ','));
                        }
                    }
                    MessageBox.Show("Exported Successfully.");
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("An error occurred in the connection, please reconnect.");
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("You need to be connected to a logger to do that!");
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Connection timed out, please reconnect.");
            }
        }

        private void cmdCopyPiData_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            ReceiveProgressForm progressForm;
            progressForm = new ReceiveProgressForm(worker);
            progressForm.Show();

            worker.DoWork += (s, args) => DoWork(s, args);
            worker.ProgressChanged += (s, args) => ProgressChanged(s, args, progressForm);
            worker.RunWorkerAsync();
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e, ReceiveProgressForm progressForm)
        {
            if (e.UserState == null)
            {
                progressForm.UpdateProgressBar(e.ProgressPercentage, "");
            }
            else
            {
                progressForm.UpdateProgressBar(e.ProgressPercentage, e.UserState.ToString());
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                worker.ReportProgress(0, "Starting download...");
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\zipDir";
                // If the temporary directory exists, delete it
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                }
                // Create temporary directory to write files to and then zip
                Directory.CreateDirectory(dirPath);

                string host = logger;
                string user = "pi";
                string password = "raspberry";

                SftpClient sftpclient = new SftpClient(host, 22, user, password);
                // Need to catch error when computer refuses connection
                sftpclient.Connect();
                int numFiles = CountFiles(sftpclient, @"/home/pi/Github/Datalogger-Alistair-Pi/");
                numDone = 0;
                DownloadDir(sftpclient, @"/home/pi/Github/Datalogger-Alistair-Pi/", dirPath, numFiles, worker);


                Thread thread = new Thread((ThreadStart)(() =>
                {
                    sfdSaveDatabase.FileName = String.Format("{0}-Data-{1}.zip", logger, DateTime.Now.ToString("yyyymmdd-HHmmss"));
                    sfdSaveDatabase.DefaultExt = "zip";
                    sfdSaveDatabase.AddExtension = true;
                    sfdSaveDatabase.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
                    if (sfdSaveDatabase.ShowDialog() == DialogResult.OK)
                    {
                    // Create a zip archive from the temporary zip directory
                    // Save zip archive to path specified by user
                    ZipFile.CreateFromDirectory(dirPath, sfdSaveDatabase.FileName);
                        MessageBox.Show("Files zipped successfully.");
                    }
                }));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();

            }
            catch (SocketException)
            {
                worker.ReportProgress(100, "Error occurred, aborting!");
                MessageBox.Show("Error occurred in connection, please reconnect.");
                MessageBox.Show("Failed to download, check that Pi has FTP/SSH enabled.");
                return;
            }
            catch (Exception exp)
            {
                worker.ReportProgress(100, "Error occurred, aborting!");
                MessageBox.Show(exp.Message);
                MessageBox.Show(exp.ToString());
                MessageBox.Show("Failed to download, check that Pi has FTP/SSH enabled.");
                return;
            }
        }


        private int numDone;
        private void DownloadDir(SftpClient sftpclient, string filepath, string dirPath, int numFiles, BackgroundWorker worker)
        {
            IEnumerable<SftpFile> files = sftpclient.ListDirectory(filepath);

            foreach (SftpFile file in files)
            {
                if (file.IsDirectory)
                {
                    if (file.Name != "." && file.Name != "..")
                    {
                        Directory.CreateDirectory(dirPath + "\\" + file.Name);
                        DownloadDir(sftpclient, file.FullName, dirPath + "\\" + file.Name, numFiles, worker);
                        if (worker == null)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    using (FileStream stream = new FileStream(dirPath + "\\" + file.Name, FileMode.Create, FileAccess.Write))
                    {
                        sftpclient.DownloadFile(file.FullName, stream);
                        numDone += 1;
                        if (worker.CancellationPending)
                        {
                            worker.Dispose();
                            return;
                        }
                        worker.ReportProgress(numDone * 100 / numFiles, String.Format("Downloaded {0}", file.Name));
                    }
                }
            }
        }


        private int CountFiles(SftpClient sftpClient, string filepath)
        {
            int numFiles = 0;
            IEnumerable<SftpFile> files = sftpClient.ListDirectory(filepath);
            foreach (SftpFile file in files)
            {
                if (file.IsDirectory)
                {
                    if (file.Name != "." && file.Name != "..")
                    {
                        numFiles += CountFiles(sftpClient, file.FullName);
                    }
                }
                else
                {
                    numFiles += 1;
                }
            }
            return numFiles;
        }
    }
}
