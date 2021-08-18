using System;
using Renci.SshNet;
using Excel = Microsoft.Office.Interop.Excel;
using Renci.SshNet.Sftp;
using System.Net.Sockets;
using System.Data;
using System.Net;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Runtime;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace SteerLoggerUser
{
    // This code controls the main functions of the UI program
    // It controls the creation of all other forms used in the UI
    public partial class mainForm : Form
    {
        // Define public/private variables to be used within the form 
        public string logger;
        public string user;
        private ProgConfig progConfig = new ProgConfig();
        public TcpClient client;
        public NetworkStream stream;
        private DownloadAndProcess DAP = new DownloadAndProcess();
        private ConcurrentQueue<string> tcpQueue = new ConcurrentQueue<string>();
        private Thread listener;
        private bool listenerExit = false;
        public Excel.Application excel = null;

        // Initialises the form
        public mainForm()
        {
            // Add custom function that is run when the form is closed to close TCP connection
            this.FormClosed += new FormClosedEventHandler(MainFormClosed);
            InitializeComponent();
        }


        // Used to send a command/data to the logger using TCP
        public void TCPSend(string command)
        {
            // Make sure connected to logger before trying to send
            if (logger == "")
            {
                // Throw exception which will be caught by the calling function
                throw new InvalidDataException();
            }
            try
            {
                // Send data to logger
                Byte[] data = Encoding.UTF8.GetBytes(command + "\u0004");
                stream.Write(data, 0, data.Length);
            }
            // If there is an error, IOException is thrown
            // Close connection and then throw SocketException which is caught by the calling function
            catch (IOException)
            {
                TCPTearDown();
                throw new SocketException();
            }
            catch (SocketException)
            {
                TCPTearDown();
                throw new SocketException();
            }
        }


        // Returns whether TCP connection is still active
        // Used in error handling in TCPListen and TCPReceive
        private bool IsConnected
        {
            get
            {
                try
                {
                    if (client != null && client.Client != null && client.Client.Connected)
                    {
                        /* pear to the documentation on Poll:
                         * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                         * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                         * -or- true if data is available for reading; 
                         * -or- true if the connection has been closed, reset, or terminated; 
                         * otherwise, returns false
                         */

                        // Detect if client disconnected
                        if (client.Client.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (client.Client.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                // Client disconnected
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }


        // Receives data from logger and populates tcpQueue
        // Allows data to be received parallel to processing in mainForm
        public void TCPListen()
        {
            try
            {
                string buffer = "";
                // While connection is not set to be closed, listen for incoming data
                while (!listenerExit)
                {
                    // If not connected, throw socket exception
                    if (!IsConnected)
                    {
                        throw new SocketException();
                    }
                    // Receive data
                    List<string> data = new List<string>();
                    Byte[] byteData = new Byte[2048];
                    Int32 bytes = stream.Read(byteData, 0, byteData.Length);
                    string response = Encoding.UTF8.GetString(byteData, 0, bytes);
                    // Connection returns b'' when it closes
                    if (response == "")
                    {
                        // Connection closed
                        throw new SocketException();
                    }
                    // Split separate packets by '\u0004' delimiter
                    foreach (string line in response.Split('\u0004'))
                    {
                        data.Add(line);
                    }
                    // If there is data in the buffer, prepend to data
                    if (buffer != "")
                    {
                        data[0] = buffer + data[0];
                        buffer = "";
                    }
                    // Add all but last packet to dataQueue
                    for (int i = 0; i < data.Count - 1; i++)
                    {
                        string line = data[i].TrimEnd('\u0004');
                        // If server sends Close, close connection
                        if (line == "Close")
                        {
                            listenerExit = true;
                            throw new IOException();
                        }
                        tcpQueue.Enqueue(line);
                    }
                    // If the last position contains data (split packet), add to buffer
                    if (data.Last() != "")
                    {
                        buffer = data.Last();
                    }
                }
            }
            catch (IOException)
            {
                // Close listener and tcp connection
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();

                }
                if (client != null)
                {
                    client.Close();
                    client.Dispose();
                }

                tcpQueue = null;
                logger = "";
                if (listenerExit == false)
                {
                    this.BeginInvoke(new Action(() => { lblConnection.Text = "You're not connected to a logger."; }));
                    MessageBox.Show("An error occured in the connection, please reconnect.","Connection Error",
                                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                // Close listener and tcp connection
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();

                }
                if (client != null)
                {
                    client.Close();
                    client.Dispose();
                }

                tcpQueue = null;
                logger = "";
                if (listenerExit == false)
                {
                    this.BeginInvoke(new Action(() => { lblConnection.Text = "You're not connected to a logger."; }));
                    MessageBox.Show("An error occured in the connection, please reconnect.","Connection Error",
                                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            return;
        }


        // Used to dequeue first item in tcpQueue
        // tcpQueue contains data sent from logger in order
        public string TCPReceive()
        {
            // If not connected to server, close connection
            if (!IsConnected)
            {
                TCPTearDown();
                throw new SocketException();
            }
            // Use DateTime to calculate timeout
            DateTime start = DateTime.Now;
            try
            {
                string response;
                // Dequeue response tcpQueue
                while (tcpQueue.TryDequeue(out response) == false)
                {
                    // Timeout set to 30 seconds
                    if (DateTime.Now.Subtract(start).TotalSeconds > 30)
                    {
                        // If no response in 30 seconds, close connection
                        TCPTearDown();
                        throw new TimeoutException();
                    }
                }
                return response;
            }
            // If tcpQueue is set to null due to closed connection, catch and throw SocketException
            catch (NullReferenceException)
            {
                throw new SocketException();
            }
        }


        // Initiate TCP connection to logger
        public void TCPStartUp()
        {
            // Setup TCP connection and stream
            // Loggers all use port 13000
            Int32 port = 13000;
            client = new TcpClient(logger, port);
            stream = client.GetStream();
            // Reset tcpQueue for new connection
            tcpQueue = new ConcurrentQueue<string>();
            // Start listener thread so data can be received in parallel
            listenerExit = false;
            listener = new Thread(TCPListen);
            listener.Start();
            TCPSend(user);
            // Update lblConnection to show new connection to logger
            lblConnection.Text = String.Format("You are connected to {0} as {1}", logger, user);
        }


        // Close TCP connection with logger
        public void TCPTearDown()
        {
            // Close listener and tcp connection
            listenerExit = true;
            if (stream != null)
            {
                stream.Close();
            }
            listener.Join();
            if (client != null)
            {
                client.Close();
            }
            // Dispose to free resources
            stream.Dispose();
            client.Dispose();
            // Set tcpQueue to null
            tcpQueue = null;
            logger = "";
            // Update lblConnection to show no connection
            this.BeginInvoke(new Action(() => { lblConnection.Text = "You're not connected to a logger."; }));
        }


        // Reads program config and presets
        private void ReadProgConfig()
        {
            progConfig = new ProgConfig();
            // Opens program config using StreamReader
            using (StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\progConf.ini"))
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
                        // Set which section is being read using section headers
                        switch (line)
                        {
                            case "[unitTypes]":
                                headerNum = 0;
                                break;
                            case "[inputTypes]":
                                headerNum = 1;
                                break;
                            case "[gains]":
                                headerNum = 2;
                                break;
                            case "[hostnames]":
                                headerNum = 3;
                                break;
                            case "[activate]":
                                headerNum = 4;
                                break;
                            default:
                                // Store data in progConfig in correct variable depending on the section being read
                                string[] data = line.Split(" = ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                switch (headerNum)
                                {
                                    case 0:
                                        progConfig.units.Add(data[1]);
                                        break;
                                    case 1:
                                        // For input types, parse the string and extract the bottom and top volt
                                        string[] decimalData = data[1].Trim(new char[] { '(', ')' }).Split(',');
                                        progConfig.inputTypes.Add(data[0], decimalData.Select(i => Convert.ToDouble(i)).ToArray());
                                        break;
                                    case 2:
                                        progConfig.gains.Add(Convert.ToInt32(data[0]), Convert.ToDouble(data[1]));
                                        break;
                                    case 3:
                                        progConfig.loggers.Add(data[1]);
                                        break;
                                    case 4:
                                        // If there is no set anaconda path, try to find in usual install location
                                        if (data.Length == 1)
                                        {
                                            // If found in usual location, set activatePath
                                            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\anaconda3\Scripts\activate.bat"))
                                            {
                                                progConfig.activatePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) 
                                                                          + @"\anaconda3\Scripts\activate.bat";
                                            }
                                            // If no in usual location, alert user and continue
                                            else
                                            {
                                                MessageBox.Show("Cannot find activate.bat for Anaconda," 
                                                                + "please edit settings and give the location of activate.bat.","Cannot Find activate.bat",
                                                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                                                progConfig.activatePath = "";
                                            }
                                        }
                                        // If there is set anaconda path, check that activate.bat exists there
                                        else
                                        {
                                            if (File.Exists(data[1]))
                                            {
                                                progConfig.activatePath = data[1];
                                            }
                                            // If activate.bat not in set location, alert user and continue
                                            else
                                            {
                                                MessageBox.Show("Cannot find activate.bat for Anaconda, please edit settings and give the location of activate.bat.",
                                                                "Cannot Find activate.bat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                progConfig.activatePath = "";
                                            }
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            // Read in simple config pins from configPresets.csv
            using (StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) 
                                                          + @"\SteerLogger\configPresets.csv"))
            {
                // Read header line and ignore
                reader.ReadLine();
                char[] trimChars = new char[] { '\n', ' ' };
                string line = "";
                // Read and process lines until at the end of the file
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Trim(trimChars);
                    string[] pinData = line.Split(',');
                    // Add sensor name and variation to variationDict (used for populating cmb menus)
                    // If sensor not already in the dictionary and it has a variation, add new sensor and variation to dictionary
                    if (!progConfig.variationDict.Keys.Contains(pinData[0]) && pinData[1] != "")
                    {
                        progConfig.variationDict.Add(pinData[0], new List<string> { pinData[1] });
                    }
                    // If sensor is in dictionary and has a variation, add new variation to sensor
                    else if (pinData[1] != "")
                    {
                        progConfig.variationDict[pinData[0]].Add(pinData[1]);
                    }
                    // If sensor isn't in dictionary and has no variation, add sensor and N/A variation to dictionary
                    else if (!progConfig.variationDict.Keys.Contains(pinData[0]))
                    {
                        progConfig.variationDict.Add(pinData[0], new List<string> { "N/A" });
                    }

                    // Add preset to dict of preset pins
                    string presetName = pinData[0] + ',' + pinData[1];
                    Pin tempPin = new Pin
                    {
                        fName = pinData[2],
                        inputType = pinData[3],
                        gain = Convert.ToInt32(pinData[4]),
                        scaleMin = Convert.ToDouble(pinData[5]),
                        scaleMax = Convert.ToDouble(pinData[6]),
                        units = pinData[7]
                    };
                    progConfig.configPins.Add(presetName, tempPin);
                }
            }

            // If configPresets have been deleted, retrieve version from programFiles
            if (progConfig.configPins.Count == 0)
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\configPresets.csv");
                InitialiseAppData();
                ReadProgConfig();
            }
        }


        // Sets up the appData directory and copies files from programFiles directory
        private void InitialiseAppData()
        {
            // If SteerLogger directory doesn't exist in appData, create it
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // If progConf.ini and configPresets.csv are not in the appData directory, copy them from program files directory
            string[] files = { "progConf.ini", "configPresets.csv" };
            foreach (string filename in files)
            {
                string output = dirPath + @"\" + filename;
                string file = Application.StartupPath + @"\" + filename;
                if (!File.Exists(output))
                {
                    File.Copy(file, output);
                }
            }

            // If SteerLogger\pythonScripts directory doesn't exist in appData, create it
            dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\pythonScripts";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Find files in pythonScripts directory
            string[] filePaths = Directory.GetFiles(Application.StartupPath + @"\pythonScripts");
            // Add python files to appData directory
            foreach (var filename in filePaths)
            {
                string file = filename.ToString();
                string output = dirPath + file.ToString().Replace(Application.StartupPath + @"\pythonScripts", "");
                if (!File.Exists(output))
                {
                    File.Copy(file, output);
                }
            }
        }


        // Loads the mainForm of the UI
        private void mainForm_Load(object sender, EventArgs e)
        {
            // Initialises appData and copies required files there
            InitialiseAppData();
            // Reads the program config file
            ReadProgConfig();
            SetupSimpleConf();
            // Starts the connect form, which searches for loggers and allows user to connect to one
            ConnectForm connectForm = new ConnectForm(progConfig.loggers.ToArray());
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
            // Clear up resources
            connectForm.Dispose();

            // Use progConfig to populate the InputSetup grid view dropdown menus correctly
            foreach (string key in progConfig.inputTypes.Keys)
            {
                ((DataGridViewComboBoxColumn)dgvInputSetup.Columns["inputType"]).Items.Add(key);
            }
            foreach (int gain in progConfig.gains.Keys)
            {
                ((DataGridViewComboBoxColumn)dgvInputSetup.Columns["gain"]).Items.Add(gain.ToString());
            }
            foreach (string value in progConfig.units)
            {
                ((DataGridViewComboBoxColumn)dgvInputSetup.Columns["units"]).Items.Add(value);
            }

            // If a logger has been found and selected, reconnect
            if (logger != "")
            {
                try
                {
                    // Reconnect to logger
                    TCPStartUp();
                    // Get the most recently used config settings from the logger
                    GetRecentConfig();
                    // Get a list of logs that user hasn't downloaded
                    RequestRecentLogs();
                }
                // If there is an issue connecting to Pi, catch error and continue without connection
                catch (SocketException)
                {
                    TCPTearDown();
                    MessageBox.Show("An error occured in the connection, please reconnect.","Connection Error",
                                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                    // Loads InputSetup grid with default values as cannot retrieve recent config
                    LoadDefaultConfig();
                    return;
                }
                catch (TimeoutException)
                {
                    TCPTearDown();
                    MessageBox.Show("Connection timed out, please reconnect.","Timeout Error",
                                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                    // Loads InputSetup grid with default values as cannot retrieve recent config
                    LoadDefaultConfig();
                    return;
                }
            }
            else
            {
                // Update lblConnection to reflect connection status
                lblConnection.Text = "You are not connected to a logger.";
                // Loads InputSetup grid with default values as cannot retrieve recent config
                LoadDefaultConfig();
            }

            // If no logs are shown in processing grid, show CtrlConf panel
            if (DAP.logsProcessing.Count == 0)
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
        }


        // Gets the logs the user hasn't downloaded
        private void RequestRecentLogs()
        {
            // Send Search_Log command and username to logger
            // Logger will search database for logs which haven't been downloaded by username
            TCPSend("Search_Log");
            TCPSend(new string('\u001f',7) + user);
            string response = TCPReceive();
            // If no logs to download, return
            if (response == "No Logs Match Criteria")
            {
                MessageBox.Show("No new logs to download.","No Logs to Download",
                                MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            // Get number of logs which match criteria
            int numLogs = Convert.ToInt16(response);
            // Show DownloadForm which allows user to select which logs to download
            DownloadForm download = new DownloadForm("Logs", false, TCPReceive, numLogs, TCPSend);
            download.ShowDialog();
            // Clear up resources
            download.Dispose();

            // Create BackgroundWorker to download logs in background
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            // Create progressForm to dispay download progress
            ReceiveProgressForm progressForm;
            progressForm = new ReceiveProgressForm(worker);
            progressForm.Show();
            // Setup DoWork and ProgressChanged event handlers
            worker.DoWork += (s, args) => ReceiveLog(s,args);
            worker.ProgressChanged += (s, e) => ProgressChanged(s,e,progressForm);
            // Run worker asynchronously
            worker.RunWorkerAsync();
        }


        // Receive a full log from the logger, this is the DoWork event for the Background worker
        private void ReceiveLog(object sender, EventArgs e)
        {
            // Get the instance of the BackgroundWorker
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                int current = 0;
                int numLogs = Convert.ToInt16(TCPReceive());
                // Download the set number of logs being sent from Logger
                for (int i = 0; i < numLogs; i ++)
                {
                    string received = TCPReceive();
                    prev = 0;
                    // Create a tempoary LogMeta to store log while its being received
                    LogMeta tempLog;
                    // Check for pending cancellation
                    if (worker.CancellationPending)
                    {
                        worker.Dispose();
                        return;
                    }
                    // Calculate progress percentage and report progress
                    current = CalcPercent(5, numLogs, current);
                    worker.ReportProgress(current, "Receiving metadata...");
                    // Receive meta data of log and set LogMeta variables
                    string[] metaData = received.Split('\u001f');
                    tempLog = new LogMeta
                    {
                        id = int.Parse(metaData[0]),
                        project = int.Parse(metaData[1]),
                        workPack = int.Parse(metaData[2]),
                        jobSheet = int.Parse(metaData[3]),
                        name = metaData[4],
                        testNumber = int.Parse(metaData[5]),
                        date = metaData[6],
                        time = decimal.Parse(metaData[7]),
                        loggedBy = metaData[8],
                        raw = metaData[9],
                        description = metaData[10],
                    };
                    // Check for pending cancellation
                    if (worker.CancellationPending)
                    {
                        worker.Dispose();
                        return;
                    }
                    // Calculate progress percentage and report progress
                    current = CalcPercent(10, numLogs, current);
                    worker.ReportProgress(current, "Receiving config data...");
                    // Receive config settings of log and write to Pin array
                    tempLog.config = new Pin[16]; 
                    for (int j = 0; j < 16; j++)
                    {
                        received = TCPReceive();
                        string[] pinData = received.Split('\u001f');
                        // Create a Pin object to store pinData in
                        Pin tempPin = new Pin
                        {
                            id = int.Parse(pinData[0]),
                            name = pinData[1],
                            enabled = (pinData[2] == "True") ? true : false,
                            fName = pinData[3],
                            inputType = pinData[4],
                            gain = int.Parse(pinData[5]),
                            scaleMin = double.Parse(pinData[6]),
                            scaleMax = double.Parse(pinData[7]),
                            units = pinData[8],
                            m = double.Parse(pinData[9]),
                            c = double.Parse(pinData[10])
                        };
                        // Add pin to Pin array
                        tempLog.config[j] = tempPin;
                    }
                    // Check for pending cancellation
                    if (worker.CancellationPending)
                    {
                        worker.Dispose();
                        return;
                    }
                    // Calculate progress percentage and report progress
                    current = CalcPercent(20, numLogs, current);
                    worker.ReportProgress(current, "Receiving log data...");
                    // Set up convheaders for converted file
                    List<string> convHeaders = new List<string> { "Date/Time", "Time (seconds)" };
                    // Use Pin array to write pin headers to convheaders
                    List<Pin> pins = new List<Pin>();
                    foreach (Pin pin in tempLog.config)
                    {
                        if (pin.enabled)
                        {
                            pins.Add(pin);
                            convHeaders.Add(pin.fName + " | " + pin.units);
                        }
                    }

                    // Setup SFTP client
                    string host = logger;
                    string user = "pi";
                    string password = "raspberry";
                    SftpClient sftpclient = new SftpClient(host, 22, user, password);
                    // Catch error if logger refuses connection
                    try
                    {
                        sftpclient.Connect();
                    }
                    catch (SocketException)
                    {
                        TCPSend("Quit");
                        TCPTearDown();
                        worker.ReportProgress(100, "Error occurred, aborting!");
                        MessageBox.Show("Failed to download, check that Pi has FTP/SSH enabled.","Failed to Download",
                                        MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }
                    // If SteerLogger directory doesn't exist in appData, create it
                    string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
                    if (!Directory.Exists(dirPath))
                    {
                        InitialiseAppData();
                    }
                    // Get path of file on Pi and set path for file to be downloaded onto computer
                    string path = @"/home/pi/Github/Datalogger-Alistair-Pi/" + tempLog.raw;
                    string temp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\" + Path.GetFileName(tempLog.raw);
                    // Check for cancellation and report progress percentage
                    if (worker.CancellationPending)
                    {
                        worker.Dispose();
                        return;
                    }
                    current = CalcPercent(40, numLogs, current);
                    worker.ReportProgress(current, "Downloading raw data...");
                    // Create file stream and stream raw data file
                    using (FileStream stream = new FileStream(temp, FileMode.Create))
                    {
                        sftpclient.DownloadFile(path, stream);
                    }
                    // Clear up resources
                    sftpclient.Dispose();
                    // Set raw path to the location of downloaded file of local machine
                    tempLog.raw = temp;
                    // Check for cancellation and report progress percentage
                    if (worker.CancellationPending)
                    {
                        worker.Dispose();
                        return;
                    }
                    current = CalcPercent(60, numLogs, current);
                    worker.ReportProgress(current, "Converting data...");
                    // min and max used for calculating whether logs can be merged or not
                    DateTime min;
                    DateTime max;
                    // Read from the downloaded raw data, convert it and save
                    using (StreamReader reader = new StreamReader(temp))
                    {
                        using (StreamWriter writer = new StreamWriter(temp.Replace("raw", "converted")))
                        {
                            // Read over header line
                            reader.ReadLine();
                            // Write converted headers to file
                            writer.WriteLine(string.Join(",",convHeaders));
                            // Read each line and store the data in the logData object
                            string[] line = reader.ReadLine().Split(',');
                            // Set min to earliest DateTime
                            min = Convert.ToDateTime(line[0]);
                            while (!reader.EndOfStream)
                            {
                                // Add DateTime and Time interval to output
                                StringBuilder output = new StringBuilder(String.Format("{0},{1}",line[0],line[1]));
                                // Read in raw data and convert using pins downloaded from config
                                for (int j = 2; j < line.Length; j++)
                                {
                                    output.AppendFormat(",{0}", double.Parse(line[j]) * pins[j - 2].m + pins[j - 2].c);
                                }
                                // Write converted data to converted data file
                                writer.WriteLine(output.ToString());
                                line = reader.ReadLine().Split(',');
                            }
                            // Set max to last DateTime
                            max = Convert.ToDateTime(line[0]);
                        }
                    }
                    // Set conv file path to location of converted file
                    tempLog.conv = tempLog.raw.Replace("raw", "converted");
                    // Check for cancellation and report progress percentage
                    if (worker.CancellationPending)
                    {
                        worker.Dispose();
                        return;
                    }
                    current = CalcPercent(80, numLogs, current);
                    worker.ReportProgress(current, "Finalising download...");
                    // If there is already a log being processed, check if logs can be merged
                    if (dgvDataProc.DataSource != null)
                    {
                        if (DAP.TestMerge(min, max))
                        {
                            // If logs can be merged, ask user
                            DialogResult dialogResult = MessageBox.Show("Would you like to merge the imported log with the current log?\n" +
                                                    "Otherwise the imported log will be added to the queue.",
                                                    "Merge Logs?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                // If they want to merge, add log to the currently processing logs
                                DAP.logsProcessing.Add(tempLog);
                                // Create tempProc from log downloaded and merge new tempProc
                                LogProc tempProc = new LogProc();
                                tempProc.CreateProcFromConv(tempLog.conv);
                                DAP.MergeLogs(tempProc, tempLog.name);
                                // Update lblLogDisplay and re-populate dgvDataProc
                                this.Invoke(new Action(() => { lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber; }));
                                this.Invoke(new Action(() => { PopulateDataViewProc(DAP.logProc); }));
                            }
                            else
                            {
                                // If they don't want to merge, enqueue the log to logsToProc
                                DAP.logsToProc.Enqueue(tempLog);
                            }
                        }
                        else
                        {
                            // If logs can't be merged, enqueue the log to logsToProc
                            DAP.logsToProc.Enqueue(tempLog);
                        }
                    }
                    // If no logs in dgvDataProc, enqueue log and immediately display next log
                    else
                    {
                        DAP.logsToProc.Enqueue(tempLog);
                        // Dequeue next log and display
                        if (DAP.logsToProc.Count > 0)
                        {
                            // Show DataProc panel
                            this.Invoke(new Action(() => { pnlCtrlConf.Hide(); }));
                            this.Invoke(new Action(() => { pnlDataProc.Show(); }));
                            // Dequeue and display next log
                            DAP.logsProcessing.Clear();
                            DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                            DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].conv);
                            // Update lblLogDisplay and re-populate dgvDataProc
                            this.Invoke(new Action(() => { lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber; }));
                            this.Invoke(new Action(() => { PopulateDataViewProc(DAP.logProc); }));
                        }
                    }
                }
                // Check for cancellation and report progress percentage
                if (worker.CancellationPending)
                {
                    worker.Dispose();
                    return;
                }
                worker.ReportProgress(100, "Download finished...");
                worker.Dispose();
            }
            // If there is a connection error, alert user
            catch (SocketException)
            {
                TCPTearDown();
                worker.ReportProgress(100, "Error occurred, aborting!");
                MessageBox.Show("Error occurred in connection, please reconnect.",
                                "Connection Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                MessageBox.Show("Failed to download, check that Pi has FTP/SSH enabled.",
                                "Download Failed",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            // If there is any other error, tell user error message
            // This is necessary as BackgroundWorker will silently close on error otherwise
            catch (Exception exp)
            {
                TCPTearDown();
                worker.ReportProgress(100, "Error occurred, aborting!");
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, exp.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, exp.Source);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                MessageBox.Show(String.Format("Full error: {0}",exp.ToString()),"Full Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }  
        }


        // Function runs when worker report progress
        private void ProgressChanged(object sender, ProgressChangedEventArgs e, ReceiveProgressForm progressForm)
        {  
            // If there is no message, set message to "" to avoid NullReferenceException
            if (e.UserState == null)
            {
                progressForm.UpdateProgressBar(e.ProgressPercentage, "");
            }
            // Update progress bar and send progress message to be displayed in textbox
            else
            {
                progressForm.UpdateProgressBar(e.ProgressPercentage, e.UserState.ToString());
            }
        }


        // Calculates percentage of work done
        private int prev = 0;
        private int CalcPercent(int value, int div, int current)
        {
            // Add percentage of work done since previous calculation to current percentage
            int percent = Convert.ToInt32(Convert.ToDouble(current) + Convert.ToDouble(value - prev) * (1d / Convert.ToDouble(div)));
            prev = value;
            return percent;
        }


        // Display logProc in DataProc grid
        DataTable table;
        private void PopulateDataViewProc(LogProc logToShow)
        {
            // If table is already got data, dispose before recreation
            if (table != null)
            {
                table.Dispose();
            }
            // Create new DataTable()
            table = new DataTable();
            // Create datatable columns from LogProc headers
            foreach (string header in logToShow.procheaders)
            {
                if (header == "Date/Time")
                {
                    table.Columns.Add(header, typeof(DateTime));
                }
                else 
                {
                    try
                    {
                        table.Columns.Add(header, typeof(double));
                    }
                    // If table already has a header with the name, make a new unique header
                    catch (DuplicateNameException)
                    {
                        table.Columns.Add(header + " 1", typeof(double));
                    }
                }
            }
            // Enumerate through logProc and add data to DataTable
            for (int i = 0; i < logToShow.timestamp.Count; i++)
            {
                // Create each row from logProc object
                object[] row = new object[table.Columns.Count];
                row[0] = logToShow.timestamp[i];
                row[1] = logToShow.time[i];
                int len = table.Columns.Count - 2;
                for (int j = 0; j < len; j ++)
                {
                    row[j + 2] = logToShow.procData[j][i];
                }
                table.Rows.Add(row);
            }
            // Set data source of dgvDataProc to datatable
            // Populating the grid this way is a lot more efficient
            dgvDataProc.DataSource = table;
            dgvDataProc.Columns[0].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss.fff";
            // Set saved to false as new data is in grid
            DAP.saved = false;
        }


        // Gets the most recently used config from the logger
        // Displays config in InputSetup grid
        private void GetRecentConfig()
        {
            // Send command to get most recent config
            TCPSend("Request_Recent_Config");
            string received = TCPReceive();
            // If no configs are stored in the logger, load a default config
            if (received == "No Config Found")
            {
                LoadDefaultConfig();
                MessageBox.Show("No recent config found, loading a default config.",
                                "No Recent Config", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            // Receive metadata
            nudInterval.Value = Convert.ToDecimal(received);
            received = TCPReceive();
            txtDescription.Text = received.Replace(";", "\r\n");
            received = TCPReceive();
            txtLogName.Text = received;
            received = TCPReceive();
            nudProject.Value = Convert.ToInt32(received);
            received = TCPReceive();
            nudWorkPack.Value = Convert.ToInt32(received);
            received = TCPReceive();
            nudJobSheet.Value = Convert.ToInt32(received);

            // Receive pin data for all 16 pins
            for (int i = 0; i < 16; i++)
            {
                received = TCPReceive();
                string[] pinData = received.Split('\u001f');
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
            }
            // Setup simple config menu
            SetupSimpleConf();
        }


        // Populates InputSetup grid with default values if recent config can't be gotten
        private void LoadDefaultConfig()
        {
            // Set all metadata input controls to default
            txtLogName.Text = "";
            nudInterval.Value = 1.0M;
            txtDescription.Text = "";
            nudProject.Value = 0;
            nudWorkPack.Value = 0;
            nudJobSheet.Value = 0;
            int number = 1;
            // Create 16 default pins and add to dgvInputSetup
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


        // Switch from DataProc panel to ControlConfig panel
        private void cmdCtrlConf_Click(object sender, EventArgs e)
        {
            // Hide data processing panel and show control/config panel
            pnlDataProc.Hide();
            pnlCtrlConf.Show();
            // Automatically adjust height of InputSetup rows to fit nicely
            int height = dgvInputSetup.Height - dgvInputSetup.ColumnHeadersHeight - 1;
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                row.Height = height / (dgvInputSetup.Rows.Count);
            }
        }


        // Switch from ControlConfig panel to DataProc panel
        private void cmdDataProc_Click(object sender, EventArgs e)
        {
            // Show control/config panel and hide data processing panel
            pnlCtrlConf.Hide();
            pnlDataProc.Show();
        }


        // Clear data in the DataProc view
        private void cmdClearData_Click(object sender, EventArgs e)
        {
            // If there is no data, return
            if (dgvDataProc.DataSource == null)
            {
                return;
            }
            // Check if data in dgvDataProc has been save or not
            if (DAP.saved == false)
            {
                // If the data hasn't been saved, ask user if they want to save before clearing
                DialogResult dialogResult = MessageBox.Show("Do you want to save data before clearing?", "Clear Data", 
                                                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    // If they want to save, run DwnldCsv button click event
                    cmdDwnldCsv.PerformClick();
                }
                // If they cancel, return with clearing
                else if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }
            }
            // Delete temporary files from appData directory
            foreach (LogMeta log in DAP.logsProcessing)
            {
                if (log.raw != null)
                {
                    File.Delete(log.raw);
                }
                if (log.conv != null)
                {
                    File.Delete(log.conv);
                }                
            }
            DAP.saved = false;
            DAP.processing = false;
            // If there is a log in the processing queue, display that log
            if (DAP.logsToProc.Count > 0)
            {
                // Clear current log from DAP.logsProcessing
                DAP.logsProcessing.Clear();
                // Display new log in dgvDataProc
                DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].conv);
                lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
                PopulateDataViewProc(DAP.logProc);
            }
            // If there is no log in the processing queue, clear grid and don't repopulate
            else
            {
                DAP.logsProcessing.Clear();
                lblLogDisplay.Text = "No Log Displaying";
                DAP.processing = false;
                dgvDataProc.DataSource = null;
                dgvDataProc.Rows.Clear();
                dgvDataProc.Columns.Clear();
            }
        }


        // Import config from Pi
        private void cmdImportConf_Click(object sender, EventArgs e)
        {
            try
            {
                // Open new DatabaseSearchForm to allow user to search for logs
                DatabaseSearchForm databaseSearch = new DatabaseSearchForm(this);
                databaseSearch.ShowDialog();
                if (databaseSearch.cancelled)
                {
                    return;
                }
                string response = TCPReceive();
                // If no logs match search criteria, alert user
                if (response == "No Logs Match Criteria")
                {
                    MessageBox.Show("No logs match search criteria, try using fewer criteria.",
                                    "No Logs Match Criteria",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                // Receive number of logs that match criteria
                int numLogs = Convert.ToInt16(response);
                // Open new DownloadForm to allow user to download config from available logs
                DownloadForm download = new DownloadForm("Config", true, TCPReceive, numLogs, TCPSend);
                download.ShowDialog();
                download.Dispose();
                // If no logs selected to downloaded, return
                response = TCPReceive();
                if (response == "Config_Sent")
                {
                    return;
                }

                // Clear InputSetup grid
                dgvInputSetup.Rows.Clear();
                // Receive config metadata
                nudInterval.Value = Convert.ToDecimal(response);
                response = TCPReceive();
                txtDescription.Text = response.Replace(";", "\r\n");
                response = TCPReceive();
                txtLogName.Text = response;
                response = TCPReceive();
                nudProject.Value = Convert.ToInt32(response);
                response = TCPReceive();
                nudWorkPack.Value = Convert.ToInt32(response);
                response = TCPReceive();
                nudJobSheet.Value = Convert.ToInt32(response);
                // Receive data for all 16 pins
                for (int i = 0; i < 16; i++)
                {
                    response = TCPReceive();
                    string[] pinData = response.Split('\u001f');
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
                }
                // Setup simple config menu to reflect pins enabled
                SetupSimpleConf();
            }
            // Catch errors which can occur during the connection
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.",
                                "Connection Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("You need to be connected to a logger to do that!",
                                "Connect to a Logger",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Connection timed out, please reconnect.",
                                "Timeout Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }


        // Imports a config from a config file
        private void cmdImportConfFile_Click(object sender, EventArgs e)
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
                                // Increase pin number by one to move one row down on the grid
                                pinNumber += 1;
                                general = false;
                                // Reset cell number to start in first column on grid
                                cellNumber = 0;
                                dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = pinNumber + 1;
                                // Cell number increased by one each time a value is changed to change column
                                cellNumber += 1;
                                // Get pin name from pin header
                                string pinName = line.Replace("[", "").Replace("]", "");
                                dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = pinName;
                                cellNumber += 1;
                            }
                            // If general is true, import config metadata
                            else if (general == true)
                            {
                                string[] data = line.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                                // Import config metadata depending on variable
                                switch (data[0])
                                {
                                    case "name":
                                        txtLogName.Text = data[1];
                                        break;
                                    case "timeinterval":
                                        nudInterval.Value = Convert.ToDecimal(data[1]);
                                        break;
                                    case "description":
                                        txtDescription.Text = data[1].Replace(";", "\r\n");
                                        break;
                                    case "project":
                                        nudProject.Value = Convert.ToInt16(data[1]);
                                        break;
                                    case "workpack":
                                        nudWorkPack.Value = Convert.ToInt16(data[1]);
                                        break;
                                    case "jobsheet":
                                        nudJobSheet.Value = Convert.ToInt16(data[1]);
                                        break;
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
                                // Values are not set, set to default
                                else if (data[0] == "inputtype" && data[1] == "Edit Me")
                                {
                                    dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = "4-20mA";
                                    cellNumber += 1;
                                }
                                // Values are not set, set to default
                                else if (data[0] == "unit" && data[1] == "Edit Me")
                                {
                                    dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = "V";
                                    cellNumber += 1;
                                }
                                // All other values are imported as are, m and c values are ignored
                                else if (data[0] != "m" && data[0] != "c")
                                {
                                    dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = data[1];
                                    cellNumber += 1;
                                }
                            }
                        }
                    }
                }
                // Setup simple config menu to reflect pins enabled
                SetupSimpleConf();
            }
        }


        // Save config to local machine
        private void cmdSave_Click(object sender, EventArgs e)
        {
            // Validate config settings
            if (ValidateConfig())
            {
                try
                {
                    // Create config from validated settings
                    LogMeta newLog = CreateConfig();
                    sfdConfig.FileName = "logConf-" + newLog.name + ".ini";
                    // Allow user to save validated config to local machine
                    if (sfdConfig.ShowDialog() == DialogResult.OK)
                    {
                        SaveConfig(newLog, sfdConfig.FileName);
                    }
                }
                catch (InvalidDataException)
                {
                    // No pins enabled
                    MessageBox.Show("Please set at least one pin to enabled.", "No Pins Enabled!",
                                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
            }
        }


        // Uploads config settings to Pi
        private void cmdSaveUpload_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate config settings
                if (ValidateConfig())
                {
                    // Create and upload config to Logger
                    UploadConfig(CreateConfig());
                }
            }
            // Catch errors when uploading config to Logger
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.","Connection Error",
                                MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            catch (InvalidDataException exp)
            {
                // Show different message box depending on error message
                if (exp.Message != "" && exp.Message == "No pins enabled")
                {
                    MessageBox.Show("Please set at least one pin to enabled.", "No Pins Enabled!",
                                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("You need to be connected to a logger to do that!", "Connect to a Logger",
                                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
        }


        // Validates the config settings
        private bool ValidateConfig()
        {
            // Make sure user has given the log a name
            if (txtLogName.Text == "")
            {
                MessageBox.Show("Please input a value for the log name.","No Log Name",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return false;
            }
            // Make sure user hasn't used invalid file name characters in the log name
            char[] invalidFileChars = Path.GetInvalidFileNameChars();
            foreach (char invalid in invalidFileChars)
            {
                if (txtLogName.Text.Contains(invalid))
                {
                    MessageBox.Show(String.Format("The log name contains invalid character: {0}",invalid), 
                        "Invalid Characters",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return false;
                }
            }
            // If user hasn't added description, ask if they want to add one
            if (txtDescription.Text == "")
            {
                if (MessageBox.Show("No description. Continue without description?"
                                    ,"No Description!",MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return false;
                }
            }
            // Make sure time interval is > 0.1 seconds
            if (nudInterval.Value < Convert.ToDecimal(0.1))
            {
                MessageBox.Show("Time interval must be at least 0.1 seconds", "Time Interval Too Low", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Make sure project, workpack and jobsheet are integers
            if (!(nudProject.Value % 1 == 0))
            {
                MessageBox.Show("Project number must be a whole number.","Project Not Integer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(nudWorkPack.Value % 1 == 0))
            {
                MessageBox.Show("Work pack number must be a whole number.", "Work Pack Not Integer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!(nudJobSheet.Value % 1 == 0))
            {
                MessageBox.Show("Job sheet number must be a whole number.", "Job Sheet Not Integer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Make sure scale min and scale max values are all decimals
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                if (!double.TryParse(row.Cells[6].Value.ToString(), out _))
                {
                    MessageBox.Show("Please check that all Scale Min values are decimals.","Scale Min Not Decimal",
                                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return false;
                }
                if (!double.TryParse(row.Cells[7].Value.ToString(), out _))
                {
                    MessageBox.Show("Please check that all Scale Min values are decimals.","Scale Max Not Decimal",
                                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return false;
                }
            }
            // Return true if all validation checks passed
            return true;
        }


        // Create a config from dgvInputSetup
        private LogMeta CreateConfig()
        {
            // Create LogMeta to store settings
            LogMeta newLog = new LogMeta
            {
                project = Convert.ToInt32(nudProject.Value),
                workPack = Convert.ToInt32(nudWorkPack.Value),
                jobSheet = Convert.ToInt32(nudJobSheet.Value),
                name = txtLogName.Text,
                time = nudInterval.Value,
                loggedBy = user,
                description = txtDescription.Text.Replace("\r\n",";")
            };
            // Setup variables for populating new Pin array
            int enabled = 0;
            newLog.config = new Pin[16];
            int index = 0;
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                // Create a new Pin from each InputSetup row
                Pin newPin = new Pin
                {
                    id = Convert.ToInt32(row.Cells[0].Value),
                    name = row.Cells[1].Value.ToString(),
                    enabled = Convert.ToBoolean(row.Cells[2].Value),
                    fName = row.Cells[3].Value.ToString(),
                    inputType = row.Cells[4].Value.ToString(),
                    gain = Convert.ToInt32(row.Cells[5].Value),
                    scaleMin = Convert.ToDouble(row.Cells[6].Value),
                    scaleMax = Convert.ToDouble(row.Cells[7].Value),
                    units = row.Cells[8].Value.ToString()
                };
                if (newPin.enabled == true)
                {
                    // If pin is enabled, calculate m and c values for pin
                    newPin = CalculateMandC(newPin);
                    enabled += 1;
                }
                else
                {
                    newPin.m = 0;
                    newPin.c = 0;
                }
                // Add pin to pin array
                newLog.config[index] = newPin;
                index += 1;
            }
            if (enabled == 0)
            {
                throw new InvalidDataException("No pins enabled");
            }
            return newLog;
        }


        // Calculates m and c values for a Pin using inputType, gain, scaleMin and scaleMax
        private Pin CalculateMandC(Pin pin)
        {
            // Retrieve corresponding voltage pair from Pin inputType
            double inputLow = progConfig.inputTypes[pin.inputType][0];
            double inputHigh = progConfig.inputTypes[pin.inputType][1];
            // Calculate gradient using change in y / change in x
            double m = (pin.scaleMax - pin.scaleMin) / (inputHigh - inputLow);
            // Calculate c from gradient
            pin.c = pin.scaleMax - m * inputHigh;
            // Multiply m by gain scale factor to get 'x' in volts
            pin.m = m * progConfig.gains[pin.gain] / 32767.0;
            return pin;
        }


        // Writes config to specified location on users computer
        private void SaveConfig(LogMeta newLog, string path)
        {
            // Create new StreamWriter to write file
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write general settings
                writer.WriteLine("[General]");
                writer.WriteLine("timeinterval = " + newLog.time);
                writer.WriteLine("name = " + newLog.name);
                writer.WriteLine("description = " + newLog.description);
                writer.WriteLine("project = " + newLog.project);
                writer.WriteLine("workpack = " + newLog.workPack);
                writer.WriteLine("jobsheet = " + newLog.jobSheet);
                writer.WriteLine();

                // Enumerate through Pins and write each one to file
                foreach (Pin pin in newLog.config)
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
        }


        // Uploads a new log config to the logger
        private void UploadConfig(LogMeta newLog)
        {
            // Send command to logger so it can receive the config
            TCPSend("Upload_Config");
            // Send metadata to the logger
            StringBuilder metadata = new StringBuilder();
            metadata.Append(newLog.project + "\u001f");
            metadata.Append(newLog.workPack + "\u001f");
            metadata.Append(newLog.jobSheet + "\u001f");
            metadata.Append(newLog.name + "\u001f");
            metadata.Append(newLog.time + "\u001f");
            metadata.Append(newLog.loggedBy + "\u001f");
            metadata.Append(newLog.description);
            TCPSend(metadata.ToString());
            // Enumerate through pin array and send settings for each Pin to logger
            foreach (Pin pin in newLog.config)
            {
                StringBuilder pindata = new StringBuilder();
                pindata.Append(pin.id + "\u001f");
                pindata.Append(pin.name + "\u001f");
                pindata.Append(pin.enabled + "\u001f");
                pindata.Append(pin.fName + "\u001f");
                pindata.Append(pin.inputType + "\u001f");
                pindata.Append(pin.gain + "\u001f");
                pindata.Append(pin.scaleMin + "\u001f");
                pindata.Append(pin.scaleMax + "\u001f");
                pindata.Append(pin.units + "\u001f");
                pindata.Append(pin.m + "\u001f");
                pindata.Append(pin.c);
                TCPSend(pindata.ToString());
            }
        }


        // Send start command to logger
        private void cmdStartLog_Click(object sender, EventArgs e)
        {
            try
            {
                TCPSend("Start_Log");
                // Show response (either logger started, or logger already running)
                MessageBox.Show(TCPReceive(),"Logger State",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            // Catch connection errors
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, or you are not connected. Please reconnect.",
                                "Connection Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("You need to be connected to a logger to do that!","Not Connected",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Connection timed out, please reconnect.", "Timeout Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        // Send stop command to logger
        private void cmdStopLog_Click(object sender, EventArgs e)
        {
            try
            {
                TCPSend("Stop_Log");
                // Show response (either logger stopped, or logger already stopped)
                MessageBox.Show(TCPReceive(), "Logger State", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Catch connection errors
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, or you are not connected. Please reconnect.",
                                "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("You need to be connected to a logger to do that!","Not Connected",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Connection timed out, please reconnect.","Timeout Error",
                                MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
        }


        // Reset InputSetup grid to default values
        private void cmdResetConfig_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will clear all config data in the Simple/Advanced config menus. Continue?", 
                                                  "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                // Clear simple config textbox and clear advanced setup grid
                txtLogPins.Text = "";
                dgvInputSetup.Rows.Clear();
                // Repopulate grid with default config and setup simple config menu
                LoadDefaultConfig();
                SetupSimpleConf();
            }
        }


        // Allows user to reconnect to logger or connect to a different one
        private void cmdConnect_Click(object sender, EventArgs e)
        {
            // If there is a connection currently, close that connection
            if (listener != null && listener.IsAlive)
            {
                TCPSend("Quit");
                TCPTearDown();
            }
            // Create new connectForm to search for available loggers
            ConnectForm connectForm = new ConnectForm(progConfig.loggers.ToArray())
            {
                user = user
            };
            connectForm.ShowDialog();
            // Get logger and user from connectForm
            logger = connectForm.logger;
            user = connectForm.user;
            // Try to connect to logger selected by user
            if (logger != null && logger != "")
            {
                try
                {
                    TCPStartUp();
                }
                catch (SocketException)
                {
                    MessageBox.Show("An error occured in the connection, please reconnect.","Connection Error",
                                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                // If logger is null, set logger to ""
                logger = "";
                //lblConnection will have already been set to Not Connected in TCPTearDown
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
                    TCPTearDown();
                }
            }
            // If an error occurs, close stream and client anyway as program is exiting, don't need to alert user
            catch (SocketException)
            {
                TCPTearDown();
            }

            // Delete any temporary raw/converted data files from SteerLogger appData directory
            string[] filePaths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger");
            foreach (string filename in filePaths)
            {
                if (filename.Contains("raw") || filename.Contains("converted"))
                {
                    try
                    {
                        File.Delete(filename);
                    }
                    catch
                    {
                        // Ignore errors as Form closing and this operation is non essential
                    }
                }
            }
        }


        // Imports log data from a csv file
        private void cmdImportLogFile_Click(object sender, EventArgs e)
        {
            // Create new logMeta hold log and to add to DAP.logsToProc
            LogMeta logMeta;
            if (ofdLog.ShowDialog() == DialogResult.OK)
            {
                // Set log name to name of file imported, removing "raw-", "converted-" and ".csv" from filename
                // Set logMeta.conv to location of file
                logMeta = new LogMeta
                {
                    name = ofdLog.SafeFileName.Replace("raw-","").Replace("converted-", "").Replace(".csv", ""),
                    conv = ofdLog.FileName
                };
                // If there is already a log being processed, allow user to merge logs
                if (dgvDataProc.DataSource != null)
                {
                    DialogResult dialogResult = MessageBox.Show("Would you like to merge the imported log with the current log?\n" +
                                                                "Otherwise the imported log will be added to the queue.",
                                                                "Merge Logs?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // If select to merge, create new logProc from imported log
                        DAP.logsProcessing.Add(logMeta);
                        LogProc tempProc = new LogProc();
                        tempProc.CreateProcFromConv(logMeta.conv);
                        // Merge logs together
                        DAP.MergeLogs(tempProc, logMeta.name);
                        // Update data display
                        lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
                        PopulateDataViewProc(DAP.logProc);
                    }
                    else
                    {
                        // If not selected to merge, enqueue imported log
                        DAP.logsToProc.Enqueue(logMeta);
                    }
                }
                // If there is no log being processed, display new log
                else
                {
                    // Enqueue imported log
                    DAP.logsToProc.Enqueue(logMeta);
                    // Dequeue next log and display it
                    if (DAP.logsToProc.Count > 0)
                    {
                        DAP.logsProcessing.Clear();
                        DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                        DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].conv);
                        // Update data display
                        lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
                        PopulateDataViewProc(DAP.logProc);
                    }
                }
            }
            // If user cancels, return
            else
            {
                return;
            }
        }


        // Imports log from Pi
        private void cmdImportLogPi_Click(object sender, EventArgs e)
        {
            try
            {
                // Create new DatabaseSearchForm so user can search for logs
                DatabaseSearchForm databaseSearch = new DatabaseSearchForm(this);
                databaseSearch.ShowDialog();
                if (databaseSearch.cancelled)
                {
                    return;
                }
                string response = TCPReceive();
                // If no logs match search criteria, tell user
                if (response == "No Logs Match Criteria")
                {
                    MessageBox.Show("No logs match search criteria, trying searching with fewer criteria.",
                                    "No Logs Match Criteria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                // Get number of logs which match criteria
                int numLogs = Convert.ToUInt16(response);
                // Create new DownloadForm so user can select logs to download
                DownloadForm download = new DownloadForm("Logs", false, TCPReceive, numLogs, TCPSend);
                download.ShowDialog();
                download.Dispose();
                // Create new BackgroundWorker to download logs in the background
                BackgroundWorker worker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                // Create new progress form to display download progress
                ReceiveProgressForm progressForm;
                progressForm = new ReceiveProgressForm(worker);
                progressForm.Show();
                // Setup DoWork and ProgressChanged event handlers for worker
                worker.DoWork += (s,args) => ReceiveLog(s,args);
                worker.ProgressChanged += (s, args) => ProgressChanged(s, args, progressForm);
                // Run worker
                worker.RunWorkerAsync();
            }
            // Catch connection errors
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, or you are not connected. Please reconnect.",
                                "Connection Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("You need to be connected to a logger to do that!", "Not Connected",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Connection timed out, please reconnect.","Timeout Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Dowload log in separate CSV files
        private void cmdDwnldCsv_Click(object sender, EventArgs e)
        {
            // Enumerate through logs that are being processed
            foreach (LogMeta log in DAP.logsProcessing)
            {
                // If a log config exists, allow user to save it on local machine
                if (log.config != null)
                {
                    // Create filename from logMeta holding config
                    sfdConfig.FileName = "logConf-" + log.name +
                                        (log.testNumber == 0 ? "" : String.Format("-{0}", log.testNumber)) +
                                        (log.date == null ? "" : String.Format("-{0}", log.date)) + ".ini";
                    sfdConfig.DefaultExt = "ini";
                    sfdConfig.Filter = "Ini files (*.ini)|*.ini|All files (*.*)|*.*";
                    if (sfdConfig.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Write the config to location specified by user
                            SaveConfig(log, sfdConfig.FileName);
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
                if (log.raw != null || log.raw != "")
                {
                    // Create filename from logMeta
                    sfdLog.FileName = "raw-" + log.name +
                                        (log.testNumber == 0 ? "" : String.Format("-{0}", log.testNumber)) +
                                        (log.date == null ? "" : String.Format("-{0}", log.date)) + ".csv";
                    sfdLog.DefaultExt = "csv";
                    sfdLog.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";
                    if (sfdLog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Write raw data csv to specified location
                            SaveCsv(log.raw, sfdLog.FileName);
                        }
                        catch (FileNotFoundException)
                        {
                            MessageBox.Show("Raw csv file does not exist in appData folder. Redownload log and try again.",
                                            "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (log.conv != null || log.conv != "")
                {
                    // Create filename from logMeta
                    sfdLog.FileName = "converted-" + log.name + 
                                      (log.testNumber == 0 ? "" : String.Format("-{0}",log.testNumber)) + 
                                      (log.date == null ? "" : String.Format("-{0}",log.date)) + ".csv";
                    sfdLog.DefaultExt = "csv";
                    sfdLog.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";
                    if (sfdLog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Write conv data csv to specified location
                            SaveCsv(log.conv, sfdLog.FileName);
                        }
                        catch (FileNotFoundException)
                        {
                            MessageBox.Show("Raw csv file does not exist in appData folder. Redownload log and try again.",
                                            "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // If the log is being processed, allow user to save processed data (data in data display)
            if (DAP.processing == true)
            {
                // Create filename from first log in DAP.logsProcessing
                sfdLog.FileName = "processed-" + DAP.logsProcessing[0].name +
                                    (DAP.logsProcessing[0].testNumber == 0 ? "" : String.Format("-{0}", DAP.logsProcessing[0].testNumber)) +
                                    (DAP.logsProcessing[0].date == null ? "" : String.Format("-{0}", DAP.logsProcessing[0].date)) + ".csv";
                sfdLog.DefaultExt = "csv";
                sfdLog.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";
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
            // Set save to true as log has been saved
            DAP.saved = true;
        }


        // Saves csv to local machine
        private void SaveCsv(string temp, string output)
        {
            // File is copied from application directory to location on local machine specified by user
            if (File.Exists(temp))
            {
                File.Copy(temp, output);
            }
            // If file doesn't exist in appData, throw FileNotFoundException
            else
            {
                throw new FileNotFoundException();
            }
        }
      

        // Write processed csv to location on local machine
        private void SaveProcCsv(LogProc logProc, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write headers of csv file
                StringBuilder heading = new StringBuilder();
                foreach (string header in logProc.procheaders)
                {
                    heading.Append(header + ",");
                }
                writer.WriteLine(heading.ToString().Trim(','));

                // Iterate through logProc and write each row to csv file
                for (int i = 0; i < logProc.timestamp.Count; i++)
                {
                    StringBuilder line = new StringBuilder();
                    line.Append(logProc.timestamp[i].ToString("yyyy/MM/dd HH:mm:ss.fff") + ",");
                    line.Append(logProc.time[i] + ",");
                    foreach (List<double> column in logProc.procData)
                    {
                        line.Append(column[i] + ",");
                    }
                    writer.WriteLine(line.ToString().Trim(','));
                }
            }
        }


        // Download logs being processed in a zip file
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
            // Save files for each log to zipDir in appData
            foreach (LogMeta log in DAP.logsProcessing)
            {
                // If config file exists for log, add to zipDir in appData
                if (log.config != null)
                {
                    string confPath = dirPath + @"\logConf-" + log.name + ".ini";
                    SaveConfig(log, confPath);
                }
                // If raw file exists, add to zipDir
                if (log.raw == null || log.raw == "")
                {
                    string rawPath = dirPath + @"\raw-" + log.name + ".csv";
                    try
                    {
                        SaveCsv(log.raw, rawPath);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("Raw csv file does not exist in appData folder. Redownload log and try again.",
                                        "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // If conv file exists, add to zipDir
                if (log.conv == null || log.conv == "")
                {
                    string convPath = dirPath + @"\converted-" + log.name + ".csv";
                    try
                    {
                        SaveCsv(log.conv, convPath);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("Raw csv file does not exist in appData folder. Redownload log and try again.",
                                        "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            // Write processed data csv to zipDir if data has been processed
            if (DAP.processing == true)
            {
                string procPath = dirPath + @"\processed-" + DAP.logsProcessing[0].name + ".csv";
                SaveProcCsv(DAP.logProc, procPath);
            }
            // Set zip filename based on log name and setup sfdLog settings for zip files
            sfdLog.FileName = DAP.logsProcessing[0].name + ".zip";
            sfdLog.DefaultExt = "zip";
            sfdLog.Filter = "Zip file (*.zip)|*.zip|All files (*.*)|*.*";
            if (sfdLog.ShowDialog() == DialogResult.OK)
            {
                // Create a zip archive from the temporary zip directory
                // Save zip archive to path specified by user
                ZipFile.CreateFromDirectory(dirPath, sfdLog.FileName);
                MessageBox.Show("Files zipped successfully.","Zip Successful",
                                MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            // Set save to true as logs have been saved
            DAP.saved = true;
        }


        // Import data into an Excel spreadsheet
        private void cmdExpExcel_Click(object sender, EventArgs e)
        {
            // Check if there is data to export
            if (dgvDataProc.DataSource != null)
            {
                // Create new excelForm to allow user to select how to export data
                ExcelForm excelForm = new ExcelForm(DAP.logProc, excel);
                try
                {
                    excelForm.ShowDialog();
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Excel closed before form closed
                    // Not a fatal error and doesn't need an alert
                }
            }
            // If there is no data to export, tell user
            else
            {
                MessageBox.Show("No data to export, please import data into the processing view!",
                                "No Data", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }


        // Process data in display using a python script
        private void cmdPythonScript_Click(object sender, EventArgs e)
        {
            // Make sure there is data to process
            if (dgvDataProc.DataSource == null)
            {
                MessageBox.Show("No log data to process, please import log data and try again.",
                                "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Make sure scripts and files are in appData
            InitialiseAppData();
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
            ofdPythonScript.InitialDirectory = dirPath + @"\pythonScripts";

            // Save data to temporary csv in appData directory
            SaveProcCsv(DAP.logProc, dirPath + @"\temp.csv");
            if (ofdPythonScript.ShowDialog() == DialogResult.OK)
            {
                // Set script to user selected python script
                string script = ofdPythonScript.FileName;
                // Get path to activate.bat
                string condaPath = progConfig.activatePath;
                // If activate.bat cannot be found, alert user and return
                if (File.Exists(condaPath) == false)
                {
                    MessageBox.Show("activate.bat is not at the location expected. Please change the settings to correct the activate.bat location."
                                    + Environment.NewLine + "Expected location: " + condaPath, "Cannot Find activate.bat",
                                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                // Construct the argument to pass to the command shell
                string cmdArguments = "/c \"chdir " + dirPath + "\\ && call " + condaPath + " && python " + script + " " + dirPath + "\"";
                ProcessStartInfo startCmd = new ProcessStartInfo
                {
                    // Setup Process parameters
                    FileName = @"C:\Windows\System32\cmd.exe",
                    Arguments = cmdArguments,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };
                // Start process
                using (Process process = Process.Start(startCmd))
                {
                    MessageBox.Show("Python script starting.","Python Script Starting",
                                    MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            // If user doesn't select a script, return
            else { return; }
            try
            {
                // Read processed data output by python script
                LogProc tempLogProc = ReadProcCsv(dirPath + @"\proc.csv");
                // Allow user to merge processed data with current data or overwrite data in the display
                DialogResult dialogResult = MessageBox.Show("Combine processed data with data in the grid?", "Combine Data?",
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    DAP.MergeLogs(tempLogProc, "Processed");
                }
                else
                {
                    DAP.logProc = tempLogProc;
                }
                // Update data display
                lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
                PopulateDataViewProc(DAP.logProc);
                // Set processing to true as data has been processed
                DAP.processing = true;
                // Delete temporary files from appData
                File.Delete(dirPath + @"\temp.csv");
                File.Delete(dirPath + @"\proc.csv");
            }
            // Catch exception if proc.csv cannot be found, usually means script failed
            catch (FileNotFoundException)
            {
                MessageBox.Show("Processing failed. Make sure your Python script outputs a proc.csv file.\n" +
                                "Also make sure that activate.bat is stored at C:\\Users\\<Your_User>\\anaconda3\\Scripts\\activate.bat", 
                                "Processing Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete(dirPath + @"\temp.csv");
            }
        }


        // Reads processed data output from python script
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
                    tempLogProc.time.Add(Convert.ToDouble(line[1]));
                    List<double> procData = new List<double>();
                    for (int i = 2; i < line.Length; i++)
                    {
                        tempLogProc.procData[i - 2].Add(double.Parse(line[i]));
                    }
                    tempLogProc.AddProcData(procData);
                }
            }
            return tempLogProc;
        }


        // Allow user to execute python graphing script
        private void cmdPythonGraph_Click(object sender, EventArgs e)
        {
            // Make sure there is data to be graphed
            if (DAP.logsProcessing.Count == 0)
            {
                MessageBox.Show("No log data to process, please import log data and try again.", "No Data",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            // Make sure scripts and files are in appData
            InitialiseAppData();
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
            ofdPythonScript.InitialDirectory = dirPath + @"\pythonScripts";

            // Save data to temporary csv in python script directory
            SaveProcCsv(DAP.logProc, dirPath + @"\temp.csv");
            if (ofdPythonScript.ShowDialog() == DialogResult.OK)
            {
                // Set script to user selected python script
                string script = ofdPythonScript.FileName;
                // Get the path to activate .bat
                string condaPath = progConfig.activatePath;
                // If activate.bat cannot be found, alert user and return
                if (File.Exists(condaPath) == false)
                {
                    MessageBox.Show("activate.bat is not the the location expected. Please change the settings to correct the activate.bat location."
                                    + Environment.NewLine + "Expected location: " + condaPath, "Cannot Find activate.bat",
                                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                // Construct the argument to pass to the command shell
                string cmdArguments = "/c \"chdir " + dirPath + "\\ && call " + condaPath + " && python " + script + " " + dirPath + "\"";
                ProcessStartInfo startCmd = new ProcessStartInfo
                {
                    // Set process arguments
                    FileName = @"C:\Windows\System32\cmd.exe",
                    Arguments = cmdArguments,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };
                // Start process
                using (Process process = Process.Start(startCmd))
                {
                    MessageBox.Show("Python script starting.","Python Script Starting",
                                    MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                // No data needs to be returned as nothing is processed
                // Python script handles displaying the graph and allowing user to save it
                // Delete temporary files from appData
                File.Delete(dirPath + @"\temp.csv");
            }
            else
            {
                return;
            }
        }


        // Handles resizing of dgvInputSetup rows when mainForm is resized
        private void dgvInputSetup_SizeChanged(object sender, EventArgs e)
        {
            // Automatically adjust height of rows to fit nicely
            int height = dgvInputSetup.Height - dgvInputSetup.ColumnHeadersHeight - 1;
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                row.Height = height / (dgvInputSetup.Rows.Count);
            }
        }


        // Shows Settings form and changes settings
        private void cmdSettings_Click(object sender, EventArgs e)
        {
            // Create and show new settings form
            SettingsForm settings = new SettingsForm(progConfig, TCPSend, TCPReceive, logger);
            settings.ShowDialog();
            settings.Dispose();
            // Re-read settings files to make sure new changes are reflected
            ReadProgConfig();
            SetupSimpleConf();
        }


        // Shows the about menu
        private void cmdAbt_Click(object sender, EventArgs e)
        {
            // This is still a Work in Progress
            MessageBox.Show("Epic new logger!!", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // Switch from simple config to advanced config or vice versa
        private void cmdConfigSwitch_Click(object sender, EventArgs e)
        {
            // Check which config menu is showing
            if (dgvInputSetup.Visible == false)
            {
                // Hide simple config and show advanced config grid
                pnlSimpleConfig.Visible = false;
                cmdConfigSwitch.Text = "Simple Config";
                dgvInputSetup.Visible = true;
            }
            else
            {
                // Hide advanced config grid and show simple config panel
                dgvInputSetup.Visible = false;
                pnlSimpleConfig.Visible = true;
                SetupSimpleConf();
                cmdConfigSwitch.Text = "Advanced Config";
            }
        }


        // Populates simple config menu using program config
        private void SetupSimpleConf()
        {
            // Clear and repopulate pin combo box
            cmbPin.Items.Clear();
            for (int i = 1; i <= 16; i++)
            {
                cmbPin.Items.Add(i.ToString());
            }
            cmbPin.SelectedIndex = 0;
            // Clear and repopulate sensor combo box
            cmbSensor.Items.Clear();
            foreach (string sensor in progConfig.variationDict.Keys)
            {
                cmbSensor.Items.Add(sensor);
            }
            cmbSensor.SelectedIndex = 0;
            // Clear and repopulate variation combo box
            cmbVar.Items.Clear();
            cmbVar.Enabled = true;
            foreach (string variation in progConfig.variationDict[cmbSensor.SelectedItem.ToString()])
            {
                cmbVar.Items.Add(variation);
            }
            cmbVar.SelectedIndex = 0;
            // If sensor has no variation, disable variation combo box
            if (cmbVar.SelectedItem.ToString() == "N/A")
            {
                cmbVar.Enabled = false;
            }
            // Clear textbox
            txtLogPins.Text = "";
            // Go through input setup grid and list pins that are enabled in textbox
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                if (Convert.ToBoolean(row.Cells[2].Value) == true)
                {
                    txtLogPins.AppendText("Pin " + row.Cells[0].Value + " is set to log " 
                                       + row.Cells[3].Value + "." + Environment.NewLine);
                }
            }
        }


        // Changes variations in cmbVar to match the sensor currently selected
        private void cmbSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear variations
            cmbVar.Items.Clear();
            cmbVar.Enabled = true;
            // Repopulate with variations for the sensor
            foreach (string variation in progConfig.variationDict[cmbSensor.SelectedItem.ToString()])
            {
                cmbVar.Items.Add(variation);
            }
            cmbVar.SelectedIndex = 0;
            // If sensor has no variations, don't disable cmbVar
            if (cmbVar.SelectedItem.ToString() == "N/A")
            {
                cmbVar.Enabled = false;
            }
        }


        // Sets a pin to log in the simple config menu
        private void cmdAddPin_Click(object sender, EventArgs e)
        {
            // Get preset for configPins dictionary
            StringBuilder preset = new StringBuilder(cmbSensor.SelectedItem.ToString() + ',');
            if (cmbVar.Enabled)
            {
                preset.Append(cmbVar.SelectedItem.ToString());
            }
            // Get pin settings for preset from configPins dictionary
            Pin pin = progConfig.configPins[preset.ToString()];
            int row = cmbPin.SelectedIndex;
            // Add pin settings to dgvInputSetup
            dgvInputSetup.Rows[row].Cells[2].Value = true;
            dgvInputSetup.Rows[row].Cells[3].Value = pin.fName;
            dgvInputSetup.Rows[row].Cells[4].Value = pin.inputType;
            dgvInputSetup.Rows[row].Cells[5].Value = pin.gain.ToString();
            dgvInputSetup.Rows[row].Cells[6].Value = pin.scaleMin.ToString();
            dgvInputSetup.Rows[row].Cells[7].Value = pin.scaleMax.ToString();
            dgvInputSetup.Rows[row].Cells[8].Value = pin.units;
            // Verify in textbox that pin has been added
            txtLogPins.AppendText("Added " + cmbSensor.SelectedItem.ToString() +
                                  " " + (cmbVar.SelectedItem.ToString() == "N/A" ? "" : cmbVar.SelectedItem.ToString()) + 
                                  " to log." + Environment.NewLine);
        }


        // Remove a pin from log using simple config setup
        private void cmdRemovePin_Click(object sender, EventArgs e)
        {
            // Get the row to disable from cmbPin
            int row = cmbPin.SelectedIndex;
            dgvInputSetup.Rows[row].Cells[2].Value = false;
            // Update textbox to show pin was removed
            txtLogPins.AppendText("Removed pin " + (row + 1).ToString() + ": " 
                                  + dgvInputSetup.Rows[row].Cells[3].Value + " from log." + Environment.NewLine);
        }


        // Allows user to change their username during a session
        private void cmdChangeUser_Click(object sender, EventArgs e)
        {
            try
            {
                TCPSend("Change_User");
                // Show change user form
                ChangeUserForm changeUserForm = new ChangeUserForm(TCPSend);
                changeUserForm.ShowDialog();
                // Update user variable and update lblConnection
                user = changeUserForm.user;
                lblConnection.Text = String.Format("You are connected to {0} as {1}", logger, user);
                // Clear resources
                changeUserForm.Dispose();
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
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // Resize lblConnection when text changed so it will resize to as much text as possible
        private void lblConnection_TextChanged(object sender, EventArgs e)
        {
            lblConnection.Width = cmdChangeUser.Location.X - (cmdAbt.Location.X + cmdAbt.Width) - 10;
        }


        // Allows user to reconvert raw data on local machine using config on local machine
        // Means if config settings are input incorrectly for a log, it can be easily fixed
        private void cmdReconvert_Click(object sender, EventArgs e)
        {
            // Create new logMeta to hold log
            LogMeta logMeta;
            ofdLog.Title = "Open Log Data";
            // Allow user to select raw data file
            if (ofdLog.ShowDialog() == DialogResult.OK)
            {
                // Set log name to name of file imported
                logMeta = new LogMeta
                {
                    name = ofdLog.SafeFileName.Replace("raw-","").Replace(".csv","") + "-Reconverted",
                    raw = ofdLog.FileName
                };
            }
            // If user cancels, return
            else
            {
                return;
            }
            List<Pin> enabled = new List<Pin>();
            // Import the config to use to convert raw data
            ofdConfig.Title = "Open Config Data";
            if (ofdConfig.ShowDialog() == DialogResult.OK)
            {
                // Create stream object to use in StreamReader creation
                Stream fileStream = ofdConfig.OpenFile();
                bool general = false;
                // pinNumber used to index logMeta.config
                int pinNumber = -1;
                logMeta.config = new Pin[16];
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
                                // Increase pin number by one
                                pinNumber += 1;
                                logMeta.config[pinNumber] = new Pin();
                                general = false;
                                logMeta.config[pinNumber].id = pinNumber;
                                logMeta.config[pinNumber].name = line.Replace("[", "").Replace("]", "");
                            }
                            else if (general == true)
                            {
                                string[] data = line.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                                // Only time interval is imported from general settings
                                switch (data[0])
                                {
                                    case "timeinterval":
                                        logMeta.time = Convert.ToDecimal(data[1]);
                                        break;
                                    case "description":
                                        logMeta.description = data[1].Replace(";", "\r\n");
                                        break;
                                    case "project":
                                        logMeta.project = Convert.ToInt16(data[1]);
                                        break;
                                    case "workpack":
                                        logMeta.workPack = Convert.ToInt16(data[1]);
                                        break;
                                    case "jobsheet":
                                        logMeta.jobSheet = Convert.ToInt16(data[1]);
                                        break;
                                }
                            }
                            else
                            {
                                string[] data = line.Split(new string[] { " = " }, StringSplitOptions.None);
                                // Set pin settings from config file
                                switch (data[0])
                                {
                                    case "enabled":
                                        logMeta.config[pinNumber].enabled = data[1] == "True";
                                        break;
                                    case "friendlyname":
                                        logMeta.config[pinNumber].fName = data[1];
                                        break;
                                    case "inputtype":
                                        logMeta.config[pinNumber].inputType = data[1] == "Edit Me" ? "4-20mA" : data[1];
                                        break;
                                    case "gain":
                                        logMeta.config[pinNumber].gain = Convert.ToInt16(data[1]);
                                        break;
                                    case "scalelow":
                                        logMeta.config[pinNumber].scaleMin = Convert.ToDouble(data[1]);
                                        break;
                                    case "scalehigh":
                                        logMeta.config[pinNumber].scaleMax = Convert.ToDouble(data[1]);
                                        break;
                                    case "unit":
                                        logMeta.config[pinNumber].units = data[1] == "Edit Me" ? "V" : data[1];
                                        break;
                                    case "m":
                                        logMeta.config[pinNumber].m = Convert.ToDouble(data[1]);
                                        break;
                                    case "c":
                                        logMeta.config[pinNumber].c = Convert.ToDouble(data[1]);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return;
            }

            // Read from the raw file selected
            using (StreamReader reader = new StreamReader(logMeta.raw))
            {
                logMeta.conv = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + String.Format(@"\SteerLogger\converted-{0}.csv", logMeta.name);
                using (StreamWriter writer = new StreamWriter(logMeta.conv))
                {
                    // Read over header line
                    reader.ReadLine();
                    StringBuilder header = new StringBuilder("Date/Time,Time (seconds),");
                    foreach (Pin pin in enabled)
                    {
                        header.Append(pin.fName + " | " + pin.units + ",");
                    }
                    writer.WriteLine(header.ToString().TrimEnd(','));
                    // Read each line from raw data, convert and write to config data
                    while (!reader.EndOfStream)
                    {
                        string[] line = reader.ReadLine().Split(',');
                        StringBuilder output = new StringBuilder(String.Format("{0},{1}", line[0], line[1]));

                        for (int j = 2; j < line.Length; j++)
                        {
                            output.AppendFormat(",{0}", double.Parse(line[j]) * enabled[j - 2].m + enabled[j - 2].c);
                        }
                        writer.WriteLine(output.ToString());
                    }
                }
            }
            // Enqueue converted data, display if no logs being processed
            if (dgvDataProc.DataSource != null)
            {
                DAP.logsToProc.Enqueue(logMeta);
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
                    DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].conv);
                    lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
                    PopulateDataViewProc(DAP.logProc);
                }
            }
        }


        // Allows user to rename a log locally if naming error has been made
        private void cmdRename_Click(object sender, EventArgs e)
        {
            // Show rename form
            RenameForm renameForm = new RenameForm(DAP);
            renameForm.ShowDialog();
            // Set DAP to updated DAP from renameForm
            DAP = renameForm.DAP;
            renameForm.Dispose();
            // Update display to reflect new name
            lblLogDisplay.Text = String.Format("Displaying: {0} {1}", DAP.logsProcessing[0].name, DAP.logsProcessing[0].testNumber);
        }
    }
}
