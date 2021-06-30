using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Runtime;

namespace SteerLoggerUser
{
    public partial class mainForm : Form
    {
        // Define public/private variables to be used within the form 
        public string logger;
        public string user;
        private ProgConfig progConfig = new ProgConfig();
        public TcpClient client;
        public NetworkStream stream;
        DownloadAndProcess DAP = new DownloadAndProcess();

        // Initialises the form
        public mainForm()
        {
            MessageBox.Show("Logger UI started. \nSearching for active loggers on your local network. (This will take about 30 seconds)");

            // Add custom function that is run when the form is closed to close TCP connection
            this.FormClosed += new FormClosedEventHandler(MainFormClosed);
            InitializeComponent();
        }

        // Reads program config
        // Test commit
        // Objective 1
        private void ReadProgConfig()
        {
            // Opens config using StreamReader
            using (StreamReader reader = new StreamReader(Application.StartupPath + "\\progConf.ini"))
            {
                string line = "";
                char[] trimChars = new char[] { '\n', ' ' };
                // headerNum used to keep track of which section is being read
                int headerNum = 0;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Trim(trimChars);
                    // Ignore empty lines or comments
                    if (line != "" && line[0] != '#')
                    {
                        // Set which section is being read from using section headers
                        if (line == "[unitTypes]")
                        {
                            headerNum = 0;
                        }
                        else if (line == "[inputTypes]")
                        {
                            headerNum = 1;
                        }
                        else if (line == "[gains]")
                        {
                            headerNum = 2;
                        }
                        else if (line == "[hostnames]")
                        {
                            headerNum = 3;
                        }
                        else
                        {
                            // Store data in progConfig in correct variable depending on the section being read
                            string[] data = line.Split(" = ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (headerNum == 0)
                            {
                                progConfig.units.Add(Convert.ToInt32(data[0]), data[1]);
                            }
                            else if (headerNum == 1)
                            {
                                string[] decimalData = data[1].Trim(new char[] { '(', ')' }).Split(',');
                                progConfig.inputTypes.Add(data[0], decimalData.Select(i => Convert.ToDecimal(i)).ToArray());
                            }
                            else if (headerNum == 2)
                            {
                                progConfig.gains.Add(Convert.ToInt32(data[0]), Convert.ToDecimal(data[1]));
                            }
                            else if (headerNum == 3)
                            {
                                progConfig.loggers.Add(Convert.ToInt32(data[0]), data[1]);
                            }
                        }
                    }
                }
            }
        }

        // Loads the mainForm
        private void mainForm_Load(object sender, EventArgs e)
        {
            // Reads the program config file
            // Objective 1
            ReadProgConfig();
            // Starts the connect form, which searches for loggers and allows user to connect to one
            // Objective 2 and 3
            ConnectForm connectForm = new ConnectForm(progConfig.loggers.Values.ToArray());
            connectForm.ShowDialog();

            // If logger is null, close application as user has closed the connect form
            if (connectForm.logger == null)
            {
                this.Close();
                return;
            }

            // Set logger and name of user
            logger = connectForm.logger;
            user = connectForm.user;

            if (logger == "")
            {
                lblConnection.Text = "You are not connected to a logger.";
            }
            else
            {
                lblConnection.Text += logger;
            }

            // Use progConfig to populate the InputSetup grid view correctly
            foreach (string key in progConfig.inputTypes.Keys)
            {
                ((DataGridViewComboBoxColumn)dgvInputSetup.Columns["inputType"]).Items.Add(key);
            }

            foreach (int gain in progConfig.gains.Keys)
            {
                ((DataGridViewComboBoxColumn)dgvInputSetup.Columns["gain"]).Items.Add(gain.ToString());
            }

            foreach (string value in progConfig.units.Values)
            {
                ((DataGridViewComboBoxColumn)dgvInputSetup.Columns["units"]).Items.Add(value);
            }

            // If a logger has been found and selected, reconnect
            if (logger != "")
            {
                try
                {
                    Int32 port = 13000;
                    client = new TcpClient(logger, port);
                    stream = client.GetStream();
                    // Get the most recently used config settings from the logger
                    // Objective 5
                    GetRecentConfig();
                    // Get a list of logs that user hasn't downloaded
                    // Objective 4
                    RequestRecentLogs();
                    // If a log has been downloaded, show the Download/Process panel instead
                    if (DAP.logsToProc.Count > 0)
                    {
                        pnlCtrlConf.Hide();
                        pnlDataProc.Show();
                        // Dequeue log from queue of logs to be processed
                        DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                        // Set logProc to the log being processed
                        DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].logData);
                        // Display logProc data to user
                        // Objective 4.2
                        PopulateDataViewProc(DAP.logProc);
                        DAP.processing = true;
                    }
                }
                // If there is an issue connecting to Pi, catch error and continue without connection
                catch (SocketException)
                {
                    MessageBox.Show("An error occured in the connection, please reconnect.");
                    stream.Close();
                    client.Close();
                    logger = "";
                    // Loads InputSetup grid with default values as cannot retrieve recent config
                    LoadDefaultConfig();
                    return;
                }
            }
            else
            {
                // Loads InputSetup grid with default values as cannot retrieve recent config
                LoadDefaultConfig();
            }

            if (DAP.processing == false)
            {
                pnlDataProc.Hide();
                pnlCtrlConf.Show();

                // Automatically adjust height of rows to fit nicely
                int height = dgvInputSetup.Height - dgvInputSetup.ColumnHeadersHeight - 1;
                foreach (DataGridViewRow row in dgvInputSetup.Rows)
                {
                    row.Height = height / (dgvInputSetup.Rows.Count);
                }
            }
            else
            {
                pnlCtrlConf.Hide();
                pnlDataProc.Show();
            }
            
        }

        // Used to get the logs the user hasn't downloaded
        // Objective 4
        private void RequestRecentLogs()
        {
            // Send command and username to logger
            TCPSend("Recent_Logs_To_Download");
            string received = TCPReceive();
            while (received != "Send_User")
            {
                TCPSend("Recent_Logs_To_Download");
                received = TCPReceive();
            }
            TCPSend(user);
            List<LogMeta> logsAvailable = new List<LogMeta>();
            string response = TCPReceive();
            // If no logs to download, exit
            if (response == "No Logs To Download")
            {
                MessageBox.Show("No new logs to download.");
                return;
            }
            // Add available logs to list
            while (response != "EoT")
            {
                string[] data = response.Split(',');
                LogMeta newLog = new LogMeta();
                newLog.id = Convert.ToInt32(data[0]);
                newLog.name = data[1];
                newLog.date = data[2];
                logsAvailable.Add(newLog);
                response = TCPReceive();
            }
            // Show DownloadForm which allows user to select which logs to download
            DownloadForm download = new DownloadForm(this, logsAvailable, "Logs", false);
            download.ShowDialog();
            // Receive the selected logs using TCP
            // Objective 4.1
            ReceiveLog(false);
        }

        // Receive a full log from the logger
        // Objectives 4.1 and 13.3
        private void ReceiveLog(bool merge)
        {
            string received = TCPReceive();
            // Continue receiving logs until all have been sent
            while (received != "All_Sent")
            {
                // Create a tempoary LogMeta to store log while its being received
                LogMeta tempLog = new LogMeta();
                // Receive meta data of log and set LogMeta variables 
                while (received != "EoMeta")
                {
                    string[] metaData = received.Split(',');
                    tempLog.id = int.Parse(metaData[0]);
                    tempLog.name = metaData[1];
                    tempLog.date = metaData[2];
                    tempLog.time = decimal.Parse(metaData[3]);
                    tempLog.loggedBy = metaData[4];
                    tempLog.downloadedBy = metaData[5];
                    received = TCPReceive();
                }
                received = TCPReceive();
                // Receive config settings of log and write to ConfigFile object
                tempLog.config = new ConfigFile();
                while (received != "EoConfig")
                {
                    string[] pinData = received.Split(',');
                    Pin tempPin = new Pin();
                    tempPin.id = int.Parse(pinData[0]);
                    tempPin.name = pinData[1];
                    tempPin.enabled = (pinData[2] == "True") ? true : false;
                    tempPin.fName = pinData[3];
                    tempPin.inputType = pinData[4];
                    tempPin.gain = int.Parse(pinData[5]);
                    tempPin.scaleMin = decimal.Parse(pinData[6]);
                    tempPin.scaleMax = decimal.Parse(pinData[7]);
                    tempPin.units = pinData[8];
                    tempPin.m = decimal.Parse(pinData[9]);
                    tempPin.c = decimal.Parse(pinData[10]);
                    tempLog.config.pinList.Add(tempPin);
                    received = TCPReceive();
                }
                received = TCPReceive();
                // Set up rawheaders and convheaders for LogData object
                tempLog.logData = new LogData();
                tempLog.logData.rawheaders = new List<string> { "Date/Time", "Time (seconds)" };
                tempLog.logData.convheaders = new List<string> { "Date/Time", "Time (seconds)" };
                int pinNum = 0;
                List<string> rawHeaders = new List<string>();
                List<string> convHeaders = new List<string>();
                // Use config file to write pin headers to rawheaders and convheaders
                foreach (Pin pin in tempLog.config.pinList)
                {
                    if (pin.enabled == true)
                    {
                        pinNum += 1;
                        rawHeaders.Add(pin.name);
                        convHeaders.Add(pin.fName + "|" + pin.name + "|" + pin.units);
                    }
                }
                tempLog.logData.rawheaders.AddRange(rawHeaders);
                tempLog.logData.convheaders.AddRange(convHeaders);
                tempLog.logData.InitRawConv(pinNum);
                // Receive log data and write to LogData object
                while (received != "EoLog")
                {
                    string[] rowData = received.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    tempLog.logData.timestamp.Add(Convert.ToDateTime(rowData[0]));
                    tempLog.logData.time.Add(decimal.Parse(rowData[1]));
                    List<decimal> rawData = new List<decimal>();
                    // Adds first half of data row (minus timestamp and time) to rawData
                    for (int i = 2; i <= (rowData.Length / 2); i++)
                    {
                        rawData.Add(decimal.Parse(rowData[i]));
                    }
                    tempLog.logData.AddRawData(rawData);
                    List<decimal> convData = new List<decimal>();
                    // Adds second half of data row (minus timestamp and time) to convData
                    for (int i = ((rowData.Length / 2) + 1); i < rowData.Length; i++)
                    {
                        convData.Add(decimal.Parse(rowData[i]));
                    }
                    tempLog.logData.AddConvData(convData);
                    received = TCPReceive();
                }
                // If merge is true, merge the log downloaded with the current logProc
                if (merge)
                {
                    DAP.logsProcessing.Add(tempLog);
                    LogProc tempProc = new LogProc();
                    tempProc.CreateProcFromConv(tempLog.logData);
                    DAP.MergeLogs(tempProc);
                }
                // If merge is not true, add log to logsToProc queue
                else
                {
                    DAP.logsToProc.Enqueue(tempLog);
                }
                received = TCPReceive();
            }
        }

        // Display logProc in DataProc grid
        // Objectives 4.2 and 15.2
        private void PopulateDataViewProc(LogProc logToShow)
        {
            // Clear grid
            dgvDataProc.Rows.Clear();
            dgvDataProc.Columns.Clear();
            // Create columns and set the header text to log headers
            foreach (string header in logToShow.procheaders)
            {
                DataGridViewColumn tempColumn = new DataGridViewColumn();
                tempColumn.Name = header.Split('|')[0];
                tempColumn.HeaderText = header;
                tempColumn.CellTemplate = new DataGridViewTextBoxCell();
                dgvDataProc.Columns.Add(tempColumn);
            }
            // Enumerate through logProc and add data to grid
            for (int i = 0; i < logToShow.timestamp.Count; i++)
            {
                List<string> newRow = new List<string>();
                newRow.Add(logToShow.timestamp[i].ToString("yyyy/MM/dd HH:mm:ss.fff"));
                newRow.Add(logToShow.time[i].ToString());
                foreach (List<decimal> column in logToShow.procData)
                {
                    newRow.Add(column[i].ToString());
                }
                dgvDataProc.Rows.Add(newRow.ToArray());
            }
        }

        // Gets the most recently used config from the logger
        // Displays config in InputSetup grid
        // Objective 5
        private void GetRecentConfig()
        {
            // Send command to get most recent config
            TCPSend("Request_Recent_Config");
            string received = TCPReceive();
            // If no configs are stored in the logger, load a default config
            if (received == "No Config Found")
            {
                LoadDefaultConfig();
                MessageBox.Show("No recent config found, loading a default config.");
                return;
            }
            // Receive time interval
            nudInterval.Value = decimal.Parse(received);
            received = TCPReceive();
            // Receive pin data until all data has been sent
            while (received != "EoConfig")
            {
                string[] pinData = received.Split(',');
                // Create new row from pin data and add to InputSetup grid
                object[] rowData = new object[]
                {
                    pinData[0],
                    pinData[1],
                    (pinData[2] == "True" ) ? true : false,
                    pinData[3],
                    pinData[4],
                    pinData[5],
                    pinData[6],
                    pinData[7],
                    pinData[8]
                };
                if (!((DataGridViewComboBoxColumn)dgvInputSetup.Columns["units"]).Items.Contains(pinData[8]))
                {
                    ((DataGridViewComboBoxColumn)dgvInputSetup.Columns["units"]).Items.Add(pinData[8]);
                }
                dgvInputSetup.Rows.Add(rowData);
                received = TCPReceive();
            }
            
        }

        // Populates InputSetup grid with default values if recent config can't be gotten
        private void LoadDefaultConfig()
        {
            nudInterval.Value = 1.0M;
            int number = 0;
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    object[] rowData = new object[]
                    {
                        number,
                        i.ToString() + 'A' + j.ToString(),
                        false,
                        "Edit Me",
                        "4-20mA",
                        "1",
                        0.0M,
                        0.0M,
                        "V"
                    };
                    dgvInputSetup.Rows.Add(rowData);
                    number += 1;
                }
            }
        }
        
        // Used to send a command/data to the logger using TCP
        public void TCPSend(string command)
        {
            // Make sure connected to logger before trying to send
            if (logger == "")
            {
                MessageBox.Show("You need to be connected to a logger to do that!");
                throw new SocketException();
            }
            string response = "";
            try
            {
                /* 
                // Send data until logger confirms it was received
                while (response != "Received")
                {
                    // Encode data using UTF-8
                    Byte[] data = Encoding.UTF8.GetBytes(command);
                    stream.Write(data, 0, data.Length);
                    data = new Byte[2048];
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    response = Encoding.UTF8.GetString(data, 0, bytes);
                }
                */
                Byte[] data = Encoding.UTF8.GetBytes(command);
                stream.Write(data, 0, data.Length);
            }
            // If there is an error, IOException is thrown
            // Close connection and then throw SocketException which is caught by code calling TCPSend
            catch (IOException)
            {
                stream.Close();
                client.Close();
                throw new SocketException();
            }
        }

        // Used to receive data from the logger sent using TCP
        public string TCPReceive()
        {
            try
            {   /*
                // Receive data and decode using UTF-8 
                Byte[] data = new Byte[2048];
                Int32 bytes = stream.Read(data, 0, data.Length);
                string response = Encoding.UTF8.GetString(data, 0, bytes);
                data = Encoding.UTF8.GetBytes("Received");
                stream.Write(data, 0, data.Length);
                return response;
                */
                Byte[] data = new Byte[2048];
                Int32 bytes = stream.Read(data, 0, data.Length);
                string response = Encoding.UTF8.GetString(data, 0, bytes);
                return response;
            }
            // If there is an error, IOException is thrown
            // Close connection and then throw SocketException which is caught by code calling TCPReceive
            catch (IOException)
            {
                stream.Close();
                client.Close();
                throw new SocketException();
            }
        }

        // Switch from DataProc panel to ControlConfig panel
        private void cmdCtrlConf_Click(object sender, EventArgs e)
        {
            pnlDataProc.Hide();
            pnlCtrlConf.Show();

            // Automatically adjust height of rows to fit nicely
            int height = dgvInputSetup.Height - dgvInputSetup.ColumnHeadersHeight - 1;
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                row.Height = height / (dgvInputSetup.Rows.Count);
            }
        }

        // Switch from ControlConfig panel to DataProc panel
        private void cmdDataProc_Click(object sender, EventArgs e)
        {
            pnlCtrlConf.Hide();
            pnlDataProc.Show();
        }

        // Clear data in the DataProc view
        private void cmdClearData_Click(object sender, EventArgs e)
        {
            if (DAP.processing == false)
            {
                dgvDataProc.Rows.Clear();
                dgvDataProc.Columns.Clear();
                return;
            }
            // If the data there is being processed, ask if user wants to save before clearing
            DialogResult dialogResult = MessageBox.Show("Do you want to save data before clearing?", "Clear Data", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                cmdDwnldCsv.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                dgvDataProc.Rows.Clear();
                dgvDataProc.Columns.Clear();
                // If there is a log in the processing queue, display that log
                if (DAP.logsToProc.Count > 0)
                {
                    DAP.logsProcessing.Clear();
                    DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                    DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].logData);
                    PopulateDataViewProc(DAP.logProc);
                    DAP.processing = true;
                }
                else
                {
                    DAP.logsProcessing.Clear();
                    DAP.processing = false;
                }
            }
        }

        // Import config from Pi or from file
        // Objective 8
        private void cmdImportConf_Click(object sender, EventArgs e)
        {
            // Ask user whether they want to import from Pi or file
            DialogResult dialogResult = MessageBox.Show("Select Yes to import from Pi, select no to import from file on local machine.",
                                                        "Import from Pi?",
                                                        MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                ImportConfigFile();
            }
            else
            {
                if (logger == "")
                {
                    MessageBox.Show("You need to be connected to a logger to do that.");
                    return;
                }
                try
                {
                    ImportConfigPi();
                }
                catch (SocketException)
                {
                    MessageBox.Show("An error occured in the connection, please reconnect.");
                    return;
                }
            }
        }

        // Imports a config from a config file
        // Objective 8.1
        private void ImportConfigFile()
        {
            if (ofdConfig.ShowDialog() == DialogResult.OK)
            {
                // Create stream object to use in StreamReader creation
                var fileStream = ofdConfig.OpenFile();

                bool general = false;
                // Used to select which data grid cell to change
                // pinNumber represents row, cellNumber represents column
                int pinNumber = -1;
                int cellNumber = 0;

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    // Reads lines one at a time until the end of the file
                    while (reader.EndOfStream == false)
                    {
                        string line = reader.ReadLine().Trim();
                        if (line != "")
                        {
                            if (line == "[General]")
                            {
                                general = true;
                            }
                            // Indicates that a new pin header has been reached
                            else if (line[0] == '[')
                            {
                                general = false;
                                // Increase pin number by one to move one row down on the grid
                                pinNumber += 1;
                                // Reset cell number to start in first column on grid
                                cellNumber = 0;
                                dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = pinNumber;
                                // Cell number increased by one each time a value is changed to change column
                                cellNumber += 1;

                                string pinName = line.Replace("[", "").Replace("]", "");
                                dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = pinName;
                                cellNumber += 1;
                            }
                            else if (general == true)
                            {
                                string[] data = line.Split(" = ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                // Only time interval is imported from general settings
                                if (data[0] == "timeinterval")
                                {
                                    nudInterval.Value = Convert.ToDecimal(data[1]);
                                }
                            }
                            else
                            {
                                string[] data = line.Split(new string[] { " = " }, StringSplitOptions.None);
                                // As enabled has to be a bool, a special case for conversion is used
                                if (data[0] == "enabled")
                                {
                                    bool enabled = data[1] == "True";
                                    dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = enabled;
                                    cellNumber += 1;
                                }
                                else if (data[0] == "inputtype" && data[1] == "Edit Me")
                                {
                                    dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = "4-20mA";
                                    cellNumber += 1;
                                }
                                else if (data[0] == "unit" && data[1] == "Edit Me")
                                {
                                    dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = "V";
                                    cellNumber += 1;
                                }
                                else if (data[0] != "m" && data[0] != "c")
                                {
                                    dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = data[1];
                                    cellNumber += 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Imports config from Pi
        // Objectives 8.2, 8.3 and 8.4
        private void ImportConfigPi()
        {
            // Open new DatabaseSearchForm to allow user to search for logs
            // Objective 8.2
            DatabaseSearchForm databaseSearch = new DatabaseSearchForm(this);
            databaseSearch.ShowDialog();
            if (databaseSearch.cancelled)
            {
                return;
            }
            List<LogMeta> logsAvailable = new List<LogMeta>();
            string response = TCPReceive();
            if (response == "No Logs Match Criteria")
            {
                MessageBox.Show("No logs match criteria.");
                return;
            }
            // Receive the logs that match the search criteria 
            while (response != "EoT")
            {
                string[] data = response.Split(',');
                LogMeta newLog = new LogMeta();
                newLog.id = Convert.ToInt32(data[0]);
                newLog.name = data[1];
                newLog.date = data[2];
                logsAvailable.Add(newLog);
                response = TCPReceive();
            }
            // Open new DownloadForm to allow user to download config from available logs
            // Objective 8.3
            DownloadForm download = new DownloadForm(this, logsAvailable, "Config", true);
            download.ShowDialog();
            response = TCPReceive();
            if (response == "Config_Sent")
            {
                return;
            }
            
            // Clear InputSetup grid
            dgvInputSetup.Rows.Clear();
            // Receive time interval and set the nudInterval control's value to the time interval
            // Objective 8.4
            nudInterval.Value = Convert.ToDecimal(response);
            response = TCPReceive();

            // Recevie data for each pin until all pins have been received
            // Objective 8.4
            while (response != "Config_Sent")
            {
                string[] pinData = response.Split(',');
                // Create row from pin data and add to InputSetup grid
                object[] rowData = new object[]
                {
                pinData[0],
                pinData[1],
                (pinData[2] == "True" ) ? true : false,
                pinData[3],
                pinData[4],
                pinData[5],
                pinData[6],
                pinData[7],
                pinData[8]
                };
                if (!((DataGridViewComboBoxColumn)dgvInputSetup.Columns["units"]).Items.Contains(pinData[8]))
                {
                    ((DataGridViewComboBoxColumn)dgvInputSetup.Columns["units"]).Items.Add(pinData[8]);
                }
                dgvInputSetup.Rows.Add(rowData);
                response = TCPReceive();
            }
        }

        // Saves and uploads config settings to Pi
        // Objective 10
        private void cmdSaveUpload_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate config settings
                ValidateConfig(true);
            }
            // Catch errors when uploading config to Pi
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.");
                return;
            }
        }

        // Validates the config settings
        private void ValidateConfig(bool upload)
        {
            // Make sure user has given the log a name
            if (txtLogName.Text == "")
            {
                MessageBox.Show("Please input a value for the log name.");
                return;
            }
            // Check with Pi that the name is unique
            TCPSend("Check_Name");
            TCPSend(txtLogName.Text);
            if (TCPReceive() == "Name exists")
            {
                MessageBox.Show("A log with that name already exists.");
                return;
            }
            // Make sure time interval is > 0.1 seconds
            if (nudInterval.Value < Convert.ToDecimal(0.1))
            {
                MessageBox.Show("Time interval must be at least 0.1 seconds", "Time Interval Too Low", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Create LogMeta to store settings
            LogMeta newLog = new LogMeta();
            newLog.name = txtLogName.Text;
            newLog.time = nudInterval.Value;
            newLog.loggedBy = user;

            ConfigFile newConfig = new ConfigFile();
            foreach(DataGridViewRow row in dgvInputSetup.Rows)
            {
                // Create a new Pin from each InputSetup row
                Pin newPin = new Pin();
                newPin.id = Convert.ToInt32(row.Cells[0].Value);
                newPin.name = row.Cells[1].Value.ToString();
                newPin.enabled = Convert.ToBoolean(row.Cells[2].Value);
                newPin.fName = row.Cells[3].Value.ToString();
                newPin.inputType = row.Cells[4].Value.ToString();
                newPin.gain = Convert.ToInt32(row.Cells[5].Value);
                decimal scaleMin;
                // Make sure that scaleMin is a decimal value, not a string
                if (decimal.TryParse(row.Cells[6].Value.ToString(), out scaleMin))
                {
                    newPin.scaleMin = scaleMin;
                }
                else
                {
                    MessageBox.Show("Please check that all Scale Min values are deciamls.");
                    return;
                }
                decimal scaleMax;
                // Make sure that scaleMax is a decimal value, not a string
                if (decimal.TryParse(row.Cells[7].Value.ToString(), out scaleMax)) 
                {
                    newPin.scaleMax = scaleMax;
                }
                else
                {
                    MessageBox.Show("Please check that all Scale Max values are deciamls.");
                    return;
                }
                newPin.units = row.Cells[8].Value.ToString();
                if (newPin.enabled == true)
                {
                    // If pin is enabled, calculate m and c values for pin
                    newPin = CalculateMandC(newPin);
                }
                else
                {
                    newPin.m = 0;
                    newPin.c = 0;
                }
                newConfig.pinList.Add(newPin);
            }
            newLog.config = newConfig;
            sfdConfig.FileName = "logConf-" + newLog.name + ".ini";
            // Allow user to save validated config to local machine
            if (sfdConfig.ShowDialog() == DialogResult.OK)
            {
                SaveConfig(newLog, upload, sfdConfig.FileName);
            }
        }

        // Calculates m and c values for a Pin using inputType, gain, scaleMin and scaleMax
        private Pin CalculateMandC(Pin pin)
        {
            // Retrieve corresponding voltage pair from Pin inputType
            decimal inputLow = progConfig.inputTypes[pin.inputType][0];
            decimal inputHigh = progConfig.inputTypes[pin.inputType][1];
            // Calculate gradient using change in y / change in x
            decimal m = (pin.scaleMax - pin.scaleMin) / (inputHigh - inputLow);
            // Calculate c from gradient
            pin.c = pin.scaleMax - m * inputHigh;
            // Multiply m by gain scale factor to get 'x' in volts
            pin.m = m * progConfig.gains[pin.gain] / 32767.0M;
            return pin;
        }

        // Writes config to specified location on users computer
        // Objective 10.1
        private void SaveConfig(LogMeta newLog, bool upload, string path)
        {
            // Create new StreamWriter to write file
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write general settings
                writer.WriteLine("[General]");
                writer.WriteLine("timeinterval = " + newLog.time);
                writer.WriteLine("name = " + newLog.name);
                writer.WriteLine();

                // Enumerate through Pins and write each one to file
                foreach (Pin pin in newLog.config.pinList)
                {
                    // Create new heading using Pin name
                    writer.WriteLine("[" + pin.name + "]");
                    writer.WriteLine("enabled = " + pin.enabled);
                    // If Pin is enabled, write settings selected by user
                    if (pin.enabled == true)
                    {
                        writer.WriteLine("friendlyname = " + pin.fName);
                        writer.WriteLine("inputtype = " + pin.inputType);
                        writer.WriteLine("gain = " + pin.gain);
                        writer.WriteLine("scalelow = " + pin.scaleMin);
                        writer.WriteLine("scalehigh = " + pin.scaleMax);
                        writer.WriteLine("unit = " + pin.units);
                        writer.WriteLine("m = " + pin.m);
                        writer.WriteLine("c = " + pin.c);
                    }
                    // If not enabled, write default values (matching how Tom's configs worked)
                    else
                    {
                        writer.WriteLine("friendlyname = Edit Me");
                        writer.WriteLine("inputtype = Edit Me");
                        writer.WriteLine("gain = 1");
                        writer.WriteLine("scalelow = 0.0");
                        writer.WriteLine("scalehigh = 0.0");
                        writer.WriteLine("unit = Edit Me");
                    }
                    writer.WriteLine();
                }
            }

            // If upload is true, upload config to logger
            // Objective 10.2
            if (upload == true)
            {
                UploadConfig(newLog);
            }
        }

        // Uploads a new log config to the logger
        // Objective 10.2
        private void UploadConfig(LogMeta newLog)
        {
            // Send command to logger so it can receive the config
            TCPSend("Upload_Config");
            // Send metadata to the logger
            string metadata = "";
            metadata += newLog.name + ",";
            metadata += newLog.date + ",";
            metadata += newLog.time + ",";
            metadata += newLog.loggedBy + ",";
            metadata += newLog.downloadedBy;
            TCPSend(metadata);
            // Enumerate through pinList and send settings for each Pin to logger
            foreach (Pin pin in newLog.config.pinList)
            {
                string pindata = "";
                pindata += pin.id + ",";
                pindata += pin.name + ",";
                pindata += pin.enabled + ",";
                pindata += pin.fName + ",";
                pindata += pin.inputType + ",";
                pindata += pin.gain + ",";
                pindata += pin.scaleMin + ",";
                pindata += pin.scaleMax + ",";
                pindata += pin.units + ",";
                pindata += pin.m + ",";
                pindata += pin.c;
                TCPSend(pindata);
            }
        }

        // Send start command to logger
        // Objective 9
        private void cmdStartLog_Click(object sender, EventArgs e)
        {
            try
            {
                TCPSend("Start_Log");
                // Show response (either logger started, or logger already running)
                MessageBox.Show(TCPReceive());
            }
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.");
            }
        }

        // Send stop command to logger
        // Objective 9
        private void cmdStopLog_Click(object sender, EventArgs e)
        {
            try
            {
                TCPSend("Stop_Log");
                // Show response (either logger stopped, or logger already stopped)
                MessageBox.Show(TCPReceive());
            }
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.");
            }
        }

        // Reset InputSetup grid to default values
        private void cmdResetConfig_Click(object sender, EventArgs e)
        {
            dgvInputSetup.Rows.Clear();
            LoadDefaultConfig();
        }

        // Allows user to reconnect to logger or connect to a different one
        private void cmdConnect_Click(object sender, EventArgs e)
        {
            // Close current TCP connection
            if (stream != null)
            {
                stream.Close();
                client.Close();
            }
            // Create new connectForm to search for available loggers
            ConnectForm connectForm = new ConnectForm(progConfig.loggers.Values.ToArray());
            connectForm.user = user;
            connectForm.ShowDialog();
            logger = connectForm.logger;
            user = connectForm.user;
            // Try to connect to logger selected by user
            if (logger != null)
            {
                try
                {
                    Int32 port = 13000;
                    client = new TcpClient(logger, port);
                    stream = client.GetStream();
                }
                catch (SocketException)
                {
                    MessageBox.Show("An error occured in the connection, please reconnect.");
                    logger = "";
                    return;
                }
                // Update display to show user is connected
                lblConnection.Text = "You are connected to: " + logger;
            }
        }

        // Close TCP connection when mainForm is closed
        void MainFormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // If user is connected to logger, close TCP stream and client
                if (logger != "" && logger != null)
                {
                    TCPSend("Quit");
                    stream.Close();
                    client.Close();
                }
            }
            // If an error occurs, close stream and client anyway as program is exiting, don't need to alert user
            catch (SocketException)
            {
                stream.Close();
                client.Close();
            }
        }

        // Imports log data from a csv file
        // Objective 12
        private void cmdImportLogFile_Click(object sender, EventArgs e)
        {
            // Create new logMeta and logData to hold log
            LogMeta logMeta = new LogMeta();
            logMeta.logData = new LogData();
            if (ofdLog.ShowDialog() == DialogResult.OK)
            {
                // Set log name to name of file imported
                logMeta.name = ofdLog.SafeFileName.Replace("converted-","");
                logMeta.name = logMeta.name.Replace(".csv", "");
                // Read from the file selected
                using (StreamReader reader = new StreamReader(ofdLog.OpenFile()))
                {
                    // Read the headerline and set convheaders
                    logMeta.logData.convheaders.AddRange(reader.ReadLine().Split(','));
                    // Initialise convData using the number of headers
                    logMeta.logData.InitRawConv(logMeta.logData.convheaders.Count - 2);
                    while (!reader.EndOfStream)
                    {
                        // Read each line and store the data in the logData object
                        string[] line = reader.ReadLine().Split(',');
                        logMeta.logData.timestamp.Add(Convert.ToDateTime(line[0]));
                        logMeta.logData.time.Add(Convert.ToDecimal(line[1]));
                        List<decimal> convData = new List<decimal>();
                        for (int i = 2; i < line.Length; i++)
                        {
                            convData.Add(decimal.Parse(line[i]));
                        }
                        logMeta.logData.AddConvData(convData);
                    }
                }
            }
            // If user cancels, return
            else
            {
                return;
            }
            // If there is already a log being processed, allow user to merge logs
            if (DAP.processing == true)
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to merge the imported log with the current log?\n" +
                                                            "Otherwise the imported log will be added to the queue.", 
                                                            "Merge Logs?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // If select to merge, create new logProc from imported log
                    DAP.logsProcessing.Add(logMeta);
                    LogProc tempProc = new LogProc();
                    tempProc.CreateProcFromConv(logMeta.logData);
                    // Merge logs together
                    dgvDataProc.Columns.Clear();
                    // Update data display
                    PopulateDataViewProc(DAP.logProc);
                }
                else
                {
                    // If not selected to merge, enqueue imported log
                    DAP.logsToProc.Enqueue(logMeta);
                    // Dequeue next log and display it
                    //if (DAP.logsToProc.Count > 0)
                    //{
                    //    DAP.logsProcessing.Clear();
                    //    DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                    //    DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].logData);
                    //    PopulateDataViewProc(DAP.logProc);
                    //}
                }
            }
            else
            {
                // Enqueue imported log
                DAP.logsToProc.Enqueue(logMeta);
                // Dequeue next log and display it
                if (DAP.logsToProc.Count > 0)
                {
                    DAP.logsProcessing.Clear();
                    DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                    DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].logData);
                    PopulateDataViewProc(DAP.logProc);
                    DAP.processing = true;
                }
            }
        }

        // Imports log from Pi
        // Objective 13
        private void cmdImportLogPi_Click(object sender, EventArgs e)
        {
            // Create new DatabaseSearchForm so user can search for logs
            // Objective 13.1
            DatabaseSearchForm databaseSearch = new DatabaseSearchForm(this);
            databaseSearch.ShowDialog();
            if (databaseSearch.cancelled)
            {
                return;
            }
            List<LogMeta> logsAvailable = new List<LogMeta>();
            string response = TCPReceive();
            if (response == "No Logs Match Criteria")
            {
                MessageBox.Show("No logs match criteria.");
                return;
            }
            // Receive list of logs the meet the search criteria.
            while (response != "EoT")
            {
                string[] data = response.Split(',');
                LogMeta newLog = new LogMeta();
                newLog.id = Convert.ToInt32(data[0]);
                newLog.name = data[1];
                newLog.date = data[2];
                logsAvailable.Add(newLog);
                response = TCPReceive();
            }
            // Create new DownloadForm so user can select logs to download
            // Objective 13.2
            DownloadForm download = new DownloadForm(this, logsAvailable, "Logs", false);
            download.ShowDialog();

            // If there is already a log being processed, ask user if they want to merge logs
            if (DAP.processing == true)
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to merge the imported log with the current log?\n" +
                                            "Otherwise the imported log will be added to the queue.",
                                            "Merge Logs?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // If they want to merge, receive the log with merge argument set to true
                    ReceiveLog(true);
                    PopulateDataViewProc(DAP.logProc);
                }
                else
                {
                    // Receive log with merge argument set to false
                    ReceiveLog(false);
                    // Dequeue next log and display it to user
                    //if (DAP.logsToProc.Count > 0)
                    //{
                    //    DAP.logsProcessing.Clear();
                    //    DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                    //    DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].logData);
                    //    PopulateDataViewProc(DAP.logProc);
                    //}
                }
            }
            // If no log to merge with, receive and add to queue
            else
            {
                ReceiveLog(false);
                // Dequeue next log and display
                if (DAP.logsToProc.Count > 0)
                {
                    DAP.logsProcessing.Clear();
                    DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                    DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].logData);
                    PopulateDataViewProc(DAP.logProc);
                    DAP.processing = true;
                }
            }
        }

        // Dowload log CSV files
        // Objective 14.1
        private void cmdDwnldCsv_Click(object sender, EventArgs e)
        {
            // Enumerate through logs that are being processed
            foreach (LogMeta log in DAP.logsProcessing)
            {
                // If a log config exists, allow user to save it on local machine
                if (log.config != null)
                {
                    sfdConfig.FileName = "logConf-" + log.name + ".ini";
                    if (sfdConfig.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Write the config to location specified by user
                            SaveConfig(log, false, sfdConfig.FileName);
                        }
                        // Catch any input/output errors
                        catch (IOException)
                        {
                            MessageBox.Show("Error saving config file. Make sure the file is not being used by another application and try again.",
                                            "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                // If a log has raw data, allow user to save raw data csv on local machine
                if (log.logData.rawData[0].Count != 0)
                {
                    sfdLog.FileName = "raw-" + log.name + ".csv";
                    if (sfdLog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Write raw data csv to specified location
                            SaveRawCsv(log.logData, sfdLog.FileName);
                        }
                        // Catch any input/output errors
                        catch (IOException)
                        {
                            MessageBox.Show("Error saving raw csv file. Make sure the file is not being used by another application and try again.",
                                            "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                // If a log has converted data, allow user to save conv data csv on local machine
                if (log.logData.convData[0].Count != 0)
                {
                    sfdLog.FileName = "converted-" + log.name + ".csv";
                    if (sfdLog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Write conv data csv to specified location
                            SaveConvCsv(log.logData, sfdLog.FileName);
                        }
                        // Cathc any input/output errors
                        catch (IOException)
                        {
                            MessageBox.Show("Error saving converted csv file. Make sure the file is not being used by another application and try again.",
                                            "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            // If the log is being processed, allow user to save processed data (data in data display)
            if (DAP.processing == true)
            {
                sfdLog.FileName = "processed-" + DAP.logsProcessing[0].name + ".csv";
                if (sfdLog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Write processed data csv to specified location
                        SaveProcCsv(DAP.logProc, sfdLog.FileName);
                    }
                    // Catch any input/output errors
                    catch (IOException)
                    {
                        MessageBox.Show("Error saving converted csv file. Make sure the file is not being used by another application and try again.",
                                        "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Write raw csv to location on local machine
        private void SaveRawCsv(LogData log, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write the headers of the csv file
                string heading = "";
                foreach (string header in log.rawheaders)
                {
                    heading += header + ",";
                }
                writer.WriteLine(heading.Trim(','));

                // Iterate through logData and write each row to the csv file
                for (int i = 0; i < log.timestamp.Count; i++)
                {
                    string line = "";
                    line += log.timestamp[i].ToString("yyyy/MM/dd HH:mm:ss.fff") + ",";
                    line += log.time[i] + ",";
                    foreach (List<decimal> column in log.rawData)
                    {
                        line += column[i] + ",";
                    }
                    writer.WriteLine(line.Trim(','));
                }
            }    
        }

        // Write conv csv to location on local machine
        private void SaveConvCsv(LogData log, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write headers of the csv file
                string heading = "";
                foreach (string header in log.convheaders)
                {
                    heading += header + ",";
                }
                writer.WriteLine(heading.Trim(','));

                // Iterate through logData and write each row to the csv file
                for (int i = 0; i < log.timestamp.Count; i++)
                {
                    string line = "";
                    line += log.timestamp[i].ToString("yyyy/MM/dd HH:mm:ss.fff") + ",";
                    line += log.time[i] + ",";
                    foreach (List<decimal> column in log.convData)
                    {
                        line += column[i] + ",";
                    }
                    writer.WriteLine(line.Trim(','));
                }
            }
        }

        // Write processed csv to location on local machine
        private void SaveProcCsv(LogProc logProc, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write headers of csv file
                string heading = "";
                foreach (string header in logProc.procheaders)
                {
                    heading += header + ",";
                }
                writer.WriteLine(heading.Trim(','));

                // Iterate through logProc and write each row to csv file
                for (int i = 0; i < logProc.timestamp.Count; i++)
                {
                    string line = "";
                    line += logProc.timestamp[i].ToString("yyyy/MM/dd HH:mm:ss.fff") + ",";
                    line += logProc.time[i] + ",";
                    foreach (List<decimal> column in logProc.procData)
                    {
                        line += column[i] + ",";
                    }
                    writer.WriteLine(line.Trim(','));
                }
            }

        }

        // Download logs being processed in a zip file
        // Objective 14.2
        private void cmdDwnldZip_Click(object sender, EventArgs e)
        {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\zipDir";
            // If the temporary directory exists, delete it
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
            // Create temporary directory to write files to and then zip
            Directory.CreateDirectory(dirPath);

            // Enumerate through logs being processed
            // Save files for each log to zipDir
            foreach (LogMeta log in DAP.logsProcessing)
            {
                if (log.config != null)
                {
                    string confPath = dirPath + @"\logConf-" + log.name + ".ini";
                    SaveConfig(log, false, confPath);
                }

                if (log.logData.rawData[0].Count != 0)
                {
                    string rawPath = dirPath + @"\raw-" + log.name + ".csv";
                    SaveRawCsv(log.logData, rawPath);
                }

                if (log.logData.convData[0].Count != 0)
                {
                    string convPath = dirPath + @"\converted-" + log.name + ".csv";
                    SaveConvCsv(log.logData, convPath);
                }
            }
            // Write processed data csv to zipDir if data has been processed
            if (DAP.processing == true)
            {
                string procPath = dirPath + @"\processed-" + DAP.logsProcessing[0].name + ".csv";
                SaveProcCsv(DAP.logProc, procPath);
            }

            sfdLog.FileName = DAP.logsProcessing[0].name + ".zip";

            if (sfdLog.ShowDialog() == DialogResult.OK)
            {
                // Create a zip archive from the temporary zip directory
                // Save zip archive to path specified by user
                ZipFile.CreateFromDirectory(dirPath,sfdLog.FileName);
                MessageBox.Show("Files zipped successfully.");
            }
        }

        // Import data into an Excel spreadsheet
        // Objective 14.3
        private void cmdExpExcel_Click(object sender, EventArgs e)
        {
            // Check if there is dat to export
            if (DAP.logProc.timestamp.Count != 0)
            {
                // Create new excelForm to allow user to select how to export data
                ExcelForm excelForm = new ExcelForm(DAP.logProc);
                excelForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No data to export, please import data into the processing view!");
            }
        }

        // Process data in display using a python script
        private void cmdPythonScript_Click(object sender, EventArgs e)
        {
            // Make sure there is data to process
            if (DAP.logsProcessing.Count == 0)
            {
                MessageBox.Show("No log data to process, please import log data and try again.");
                return;
            }

            // If SteerLogger directory doesn't exist in appData, crete it
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Save data to temporary csv in appData directory
            SaveProcCsv(DAP.logProc, dirPath + @"\temp.csv");

            string script = "";
            if (ofdPythonScript.ShowDialog() == DialogResult.OK)
            {
                // Set script to user selected python script
                script = ofdPythonScript.FileName;
                // Get path to activate.bat
                string condaPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\anaconda3\Scripts\activate.bat";
                // Construct the argument to pass to the command shell
                string cmdArguments = "/c \"chdir " + dirPath + "\\ && call " + condaPath + " && python " + script + " " + dirPath + "\"";


                ProcessStartInfo startCmd = new ProcessStartInfo();
                // Setup Process arguements
                startCmd.FileName = @"C:\Windows\System32\cmd.exe";
                startCmd.Arguments = cmdArguments;
                startCmd.UseShellExecute = false;
                startCmd.CreateNoWindow = false;
                // Start process
                // Objective 15.1
                using (Process process = Process.Start(startCmd))
                {
                    MessageBox.Show("Python script starting.");
                }
            }
            else { return; }

            // Read processed data output by python script
            // Objectve 15.2
            try
            {
                LogProc tempLogProc = ReadProcCsv(dirPath + @"\proc.csv");
                // Allow user to merge processed data with current data or overwrite data in the display
                // Objective 15.3
                DialogResult dialogResult = MessageBox.Show("Combine processed data with data in the grid?", "Combine Data?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DAP.MergeLogs(tempLogProc);
                }
                else
                {
                    DAP.logProc = tempLogProc;
                }
                PopulateDataViewProc(DAP.logProc);
                File.Delete(dirPath + @"\temp.csv");
                File.Delete(dirPath + @"\proc.csv");
            }
            // Fires if proc.csv cannot be found, usually means script failed
            catch (FileNotFoundException) 
            {
                MessageBox.Show("Processing failed. Make sure your Python script outputs a proc.csv file.\n" +
                                "Also make sure that activate.bat is stored at C:\\Users\\<Your_User>\\anaconda3\\Scripts\\activate.bat", "Processing Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete(dirPath + @"\temp.csv");
            }
        }

        // Reads processed data output from python script
        // Objective 15.3
        private LogProc ReadProcCsv(string filepath)
        {
            LogProc tempLogProc = new LogProc();
            // Create stream reader to read csv data
            using (StreamReader reader = new StreamReader(filepath))
            {
                // Read headers from csv
                tempLogProc.procheaders.AddRange(reader.ReadLine().Split(','));
                // Initialise procData using header count
                tempLogProc.InitProc(tempLogProc.procheaders.Count - 2);
                // Read each row and add it to logProc
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(',');
                    tempLogProc.timestamp.Add(Convert.ToDateTime(line[0]));
                    tempLogProc.time.Add(Convert.ToDecimal(line[1]));
                    List<decimal> procData = new List<decimal>();
                    for (int i = 2; i < line.Length; i++)
                    {
                        tempLogProc.procData[i - 2].Add(decimal.Parse(line[i]));
                    }
                    tempLogProc.AddProcData(procData);
                }
            }
            return tempLogProc;
        }

        // Allow user to execute python graphing script
        // Objective 16
        private void cmdPythonGraph_Click(object sender, EventArgs e)
        {
            // Make sure there is data to be graphed
            if (DAP.logsProcessing.Count == 0)
            {
                MessageBox.Show("No log data to process, please import log data and try again.");
                return;
            }

            // If SteerLogger directory doesn't exist in appData, crete it
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Save data to temporary csv in python script directory
            SaveProcCsv(DAP.logProc, dirPath + @"\temp.csv");

            string script = "";
            if (ofdPythonScript.ShowDialog() == DialogResult.OK)
            {
                // Set script to user selected python script
                script = ofdPythonScript.FileName;
                // Get the path to activate .bat
                string condaPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\anaconda3\Scripts\activate.bat";
                // Construct the argument to pass to the command shell
                string cmdArguments = "/c \"chdir " + dirPath + "\\ && call " + condaPath +  " && python " + script + " " + dirPath + "\"";

                ProcessStartInfo startCmd = new ProcessStartInfo();
                // Set process arguments
                startCmd.FileName = @"C:\Windows\System32\cmd.exe";
                startCmd.Arguments = cmdArguments;
                startCmd.UseShellExecute = false;
                startCmd.CreateNoWindow = false;
                // Start process
                using (Process process = Process.Start(startCmd))
                {
                    MessageBox.Show("Python script starting.");
                }
                // No data needs to be returned as nothing is processed
                // Python script handles displaying the graph and allowing user to save it
            }
            else
            {
                return;
            }
        }

        private void dgvInputSetup_SizeChanged(object sender, EventArgs e)
        {            
            // Automatically adjust height of rows to fit nicely
            int height = dgvInputSetup.Height - dgvInputSetup.ColumnHeadersHeight - 1;
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                row.Height = height / (dgvInputSetup.Rows.Count);
            }
        }

        private void cmdSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("WIP: Will likely allow progConfig.ini to be changed from here.");
        }

        private void cmdAbt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Epic new logger!!","About",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
