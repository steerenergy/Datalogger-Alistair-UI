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

        // On form load, populate all controls
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Set ActivateLocation.Text to current path of activate.bat
            txtActivateLocation.Text = config.activatePath;
            // Populate cmbUnits with units from program config
            foreach (string unit in config.units)
            {
                cmbUnits.Items.Add(unit);
            }
            cmbUnits.SelectedIndex = 0;
            // Populate cmbInputTypes with input types from program config
            foreach (string inputType in config.inputTypes.Keys)
            {
                cmbInputTypes.Items.Add(inputType + ": ("
                    + config.inputTypes[inputType][0].ToString() + ","
                    + config.inputTypes[inputType][1].ToString() + ")");
            }
            cmbInputTypes.SelectedIndex = 0;
            // Populate cmbGains with gains from program config
            foreach (int gain in config.gains.Keys)
            {
                cmbGains.Items.Add(gain.ToString());
            }
            cmbGains.SelectedIndex = 0;
            // Populate cmbLoggers with logger hostnames from program config
            foreach (string logger in config.loggers)
            {
                cmbLoggers.Items.Add(logger);
            }
            cmbLoggers.SelectedIndex = 0;

            // Use progConfig to setup the presets grid view correctly
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
            // Populate presets grid with current presets
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


        // Allows user to use open file dialog to point program to activate.bat
        private void cmdFindActivate_Click(object sender, EventArgs e)
        {
            if (ofdFindActivate.ShowDialog() == DialogResult.OK)
            {
                // Alert user if the file they select is not called activate.bat
                if (ofdFindActivate.SafeFileName != "activate.bat")
                {
                    MessageBox.Show("The file you selected was not called activate.bat, please check this is correct.", "Check Filename",
                                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                // Set activate path to the location of activate.bat
                config.activatePath = ofdFindActivate.FileName;
                txtActivateLocation.Text = config.activatePath;
            }
        }


        // Add new unit to config
        private void cmdAddUnit_Click(object sender, EventArgs e)
        {
            // Make sure user inputs a unit to add
            if (txtAddUnit.Text == "")
            {
                MessageBox.Show("Please write a new unit to add in the textbox!","No Unit",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            // Add new unit to program config and update combo box
            string newUnit = txtAddUnit.Text.Trim();
            config.units.Add(newUnit);
            cmbUnits.Items.Add(newUnit);
            cmbUnits.SelectedItem = newUnit;
        }

        // Add new logger hostname to config
        private void cmdAddLogger_Click(object sender, EventArgs e)
        {
            // Make sure user inputs a hostname to add
            if (txtLogger.Text == "")
            {
                MessageBox.Show("Please write a new logger to add in the textbox!","No Logger",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            // Add new logger to program config and update combo box
            string newLogger = txtLogger.Text.Trim();
            config.loggers.Add(newLogger);
            cmbLoggers.Items.Add(newLogger);
            cmbLoggers.SelectedItem = newLogger;
        }

        // Close form without saving if cancel clicked
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Add new input type to config
        private void cmdAddInputType_Click(object sender, EventArgs e)
        {
            // Make sure name, bottom volt and top volt are all valid
            if (txtName.Text == "")
            {
                MessageBox.Show("Please write the name for the new input type in the textbox!","No Input Name",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (Double.TryParse(txtBottomVolt.Text, out double bottomVolt) == false)
            {
                MessageBox.Show("Please makes sure your bottom voltage value is an integer or decimal.",
                                "No Bottom Voltage", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (Double.TryParse(txtTopVolt.Text, out double topVolt) == false)
            {
                MessageBox.Show("Please makes sure your top voltage value is an integer or decimal.",
                                "No Top Voltage",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            // Add to program config and update combo box
            string name = txtName.Text.Trim();
            config.inputTypes.Add(name, new double[] { bottomVolt, topVolt });
            cmbInputTypes.Items.Add(name + ": ("
                        + bottomVolt.ToString() + ","
                        + topVolt.ToString() + ")");
            cmbInputTypes.SelectedItem = name + ": (" + bottomVolt.ToString() + "," + topVolt.ToString() + ")";
        }


        // Reads progConf data for saving changes
        private List<string> ReadConfig()
        {
            List<string> lines = new List<string>();
            // Opens config using StreamReader
            using (StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\progConf.ini"))
            {
                string line = "";
                char[] trimChars = new char[] { '\n', ' ' };
                while (!reader.EndOfStream)
                {
                    // Read in non-setting lines e.g. headers, comments and blank lines
                    line = reader.ReadLine().Trim(trimChars);
                    if (line == "" || line[0] == '#' || line[0] == '[')
                    {
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }

        // Save settings by rewriting progConf.ini and configPresets.csv
        private void cmdSave_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            try
            {
                lines = ReadConfig();
            }
            // If progConf.ini doesn't exist, get from programFiles directory
            catch (FileNotFoundException)
            {
                // If SteerLogger directory doesn't exist in appData, create it
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string output = dirPath + @"\progConf.ini";
                string file = Application.StartupPath + @"\progConf.ini";
                try
                {
                    if (!File.Exists(output))
                    {
                        File.Copy(file, output);
                        lines = ReadConfig();
                    }
                }
                catch (FileNotFoundException exp)
                {
                    MessageBox.Show(String.Format("Error: {0}. " +
                        "\nPlease check your installation and reinstall if files are missing." +
                        "\nThe application will now exit.", exp.Message),
                        "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            catch (DirectoryNotFoundException)
            {
                // If SteerLogger directory doesn't exist in appData, create it
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string output = dirPath + @"\progConf.ini";
                string file = Application.StartupPath + @"\progConf.ini";
                try
                {
                    if (!File.Exists(output))
                    {
                        File.Copy(file, output);
                        lines = ReadConfig();
                    }
                }
                catch (FileNotFoundException exp)
                {
                    MessageBox.Show(String.Format("Error: {0}. " +
                        "\nPlease check your installation and reinstall if files are missing." +
                        "\nThe application will now exit.", exp.Message),
                        "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            // Opens progConf.ini using StreamWriter to write new settings
            using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\progConf.ini"))
            {
                foreach (string line in lines)
                {
                    // Write lines read in and add data from config
                    writer.WriteLine(line);
                    int num = 0;
                    // Write the settings for the relevant heading
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

            // Opens configPresets.csv to write new settings
            using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\configPresets.csv"))
            {
                // Write header line
                string line = "";
                foreach (DataGridViewColumn col in dgvPresets.Columns)
                {
                    line += col.HeaderText + ',';
                }
                writer.WriteLine(line.TrimEnd(','));
                // Write the data in each row
                // Do Rows.Count - 1 to ignore last row which is always empty
                for (int i = 0; i < dgvPresets.Rows.Count - 1; i++)
                {
                    line = "";
                    foreach (DataGridViewCell cell in dgvPresets.Rows[i].Cells)
                    {
                        // Make sure there are no empty cells in the row
                        if (cell.Value == null)
                        {
                            MessageBox.Show("Make sure all preset cells have values filled in.\n" +
                                            "If there is no variation, please put N/A in the variation cell.",
                                            "Unfilled Cells",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }
                        // If value is N/A, add nothing, otherwise add value of cell
                        if (cell.Value.ToString() == "N/A")
                        {
                            line += ',';
                        }
                        else
                        {
                            line += cell.Value.ToString() + ',';
                        }
                    }
                    // Write line to congifPresets.csv
                    writer.WriteLine(line.TrimEnd(','));
                }
            }
            // Close settings form
            this.Close();
        }


        // Exports a csv copy of the database on the logger to the local machine
        private void cmdExportDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                // Setup save file dialog for saving database
                sfdSaveDatabase.FileName = String.Format("{0}-Database-{1}.csv", this.logger, DateTime.Now.ToString("yyyymmdd-HHmmss"));
                sfdSaveDatabase.DefaultExt = "csv";
                sfdSaveDatabase.AddExtension = true;
                sfdSaveDatabase.Filter = "Csv files (*.csv)|*.zip|All files (*.*)|*.*";
                // Let user choose where they save the database export
                if (sfdSaveDatabase.ShowDialog() == DialogResult.OK)
                {
                    // Send command to logger
                    TCPSend("Export_Database");
                    using (StreamWriter writer = new StreamWriter(sfdSaveDatabase.FileName))
                    {
                        // Write header line
                        writer.WriteLine(TCPReceive().Replace('\u001f', ','));
                        int numRows = Convert.ToInt32(TCPReceive());
                        for (int i = 0; i < numRows; i++)
                        {
                            // Write row of data
                            writer.WriteLine(TCPReceive().Replace(',', ';').Replace('\u001f', ','));
                        }
                    }
                    MessageBox.Show("Exported Successfully.", "Exported Successfully",
                                    MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            // Catch file input/output errors
            catch (IOException)
            {
                MessageBox.Show("Error saving database csv file. Make sure the file is not being used by another application and try again.",
                "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Catch invalid format errors for the conversion
            catch (FormatException)
            {
                MessageBox.Show("Received unexcepted data, make sure both programs are up to date and try again.",
                    "Incorrect Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Catch connection errors
            catch (SocketException)
            {
                MessageBox.Show("An error occurred in the connection, please reconnect.","Connection Error",
                                MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("You need to be connected to a logger to do that!","Not Connected",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Connection timed out, please reconnect.","Timeout Error",
                                MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }


        // Exports copy of all data on Pi to users computer
        private void cmdCopyPiData_Click(object sender, EventArgs e)
        {
            // Setup background worker so operation can be done in the background
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            // Create progress form to report progress of download
            ReceiveProgressForm progressForm;
            progressForm = new ReceiveProgressForm(worker);
            progressForm.Show();
            //Setup worker event handlers
            worker.DoWork += (s, args) => DoWork(s, args);
            worker.ProgressChanged += (s, args) => ProgressChanged(s, args, progressForm);
            worker.RunWorkerAsync();
        }

        // Updates progressForm with the progress of the worker
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

        // Operation the BackgroundWorker runs
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            // Get instance of BackgroundWorker
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                worker.ReportProgress(0, "Starting download...");
                // Get location of directory where files will be placed to be zipped
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\zipDir";
                // If the temporary directory exists, delete it
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                }
                // Create temporary directory to write files to and then zip
                Directory.CreateDirectory(dirPath);
                
                // Setup SFTP client
                string host = logger;
                string user = "pi";
                string password = "raspberry";
                SftpClient sftpclient = new SftpClient(host, 22, user, password);
                // Connect to logger, count the number of files to download and then download files
                sftpclient.Connect();
                int numFiles = CountFiles(sftpclient, @"/home/pi/Github/Datalogger-Alistair-Pi/");
                numDone = 0;
                DownloadDir(sftpclient, @"/home/pi/Github/Datalogger-Alistair-Pi/", dirPath, numFiles, worker);
                // Once download has finished, create new thread to zip the temporary directory and then save to user chosen location
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
                        try
                        {
                            ZipFile.CreateFromDirectory(dirPath, sfdSaveDatabase.FileName);
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("Error creating Zip archive, make sure the file is not in use by another applicaiton and try again.",
                                "Error Creating Archive", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        MessageBox.Show("Files zipped successfully.","Zip Successful",
                            MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }));
                // Set apartment state to STA is necessary otherwise error occurs
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                // Wait for thread to complete
                thread.Join();
            }
            // Catch connection errors
            catch (SocketException)
            {
                worker.ReportProgress(100, "Error occurred, aborting!");
                MessageBox.Show("Error occurred in connection, please reconnect.","Connection Error",
                                MessageBoxButtons.OK,MessageBoxIcon.Error);
                MessageBox.Show("Failed to download, check that Pi has FTP/SSH enabled.","Download Failed",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            catch (Exception exp)
            {
                worker.ReportProgress(100, "Error occurred, aborting!");
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, exp.Message);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(String.Format("Full error: {0}", exp.ToString()), "Full Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        // Recursive function to download all files in a directory
        private int numDone;
        private void DownloadDir(SftpClient sftpclient, string filepath, string dirPath, int numFiles, BackgroundWorker worker)
        {
            // Get the files in the directory
            IEnumerable<SftpFile> files = sftpclient.ListDirectory(filepath);
            foreach (SftpFile file in files)
            {
                // Check if file is a directory or not
                if (file.IsDirectory)
                {
                    if (file.Name != "." && file.Name != "..")
                    {
                        // If file is a directory, create new direcotry in the download location
                        Directory.CreateDirectory(dirPath + "\\" + file.Name);
                        // Call DownloadDir to download all files from the direcotry
                        DownloadDir(sftpclient, file.FullName, dirPath + "\\" + file.Name, numFiles, worker);
                        if (worker == null)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    // If the file is not a directory, create a FileStream to download the file
                    using (FileStream stream = new FileStream(dirPath + "\\" + file.Name, FileMode.Create, FileAccess.Write))
                    {
                        // Use sftpclient to download the file
                        sftpclient.DownloadFile(file.FullName, stream);
                        // Increment number of files done and report progress, checking for pending cancellation
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


        // Recursively count all the files that need to be downloaded
        // This is used for progress reporting
        private int CountFiles(SftpClient sftpClient, string filepath)
        {
            int numFiles = 0;
            // Get files in directory
            IEnumerable<SftpFile> files = sftpClient.ListDirectory(filepath);
            foreach (SftpFile file in files)
            {
                if (file.IsDirectory)
                {
                    if (file.Name != "." && file.Name != "..")
                    {
                        // If file is a directory, run CountFiles on directory to count files inside the directory
                        numFiles += CountFiles(sftpClient, file.FullName);
                    }
                }
                else
                {
                    // Increment numFiles for each file
                    numFiles += 1;
                }
            }
            return numFiles;
        }
    }
}
