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
    public partial class mainForm : Form
    {
        // Define public/private variables to be used within the form 
        public string logger;
        public string user;
        private ProgConfig progConfig = new ProgConfig();
        public TcpClient client;
        public NetworkStream stream;
        DownloadAndProcess DAP = new DownloadAndProcess();
        public ConcurrentQueue<string> tcpQueue = new ConcurrentQueue<string>();
        private Thread listener;
        public int pbValue;
        public ConcurrentQueue<string> dataQueue = new ConcurrentQueue<string>();
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
                MessageBox.Show("You need to be connected to a logger to do that!");
                // Maybe change this to InvalidDataException to differentiate from connection error
                throw new SocketException();
            }
            try
            {
                // Send data to logger
                Byte[] data = Encoding.UTF8.GetBytes(command + "\u0004");
                stream.Write(data, 0, data.Length);
            }
            // If there is an error, IOException is thrown
            // Close connection and then throw SocketException which is caught by code calling TCPSend
            catch (IOException)
            {
                stream.Close();
                client.Close();
                listener.Abort();
                while (tcpQueue.IsEmpty == false)
                {
                    tcpQueue.TryDequeue(out string result);
                }
                throw new SocketException();
            }
        }

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


        public void TCPListen()
        {
            try
            {
                string buffer = "";
                while (true)
                {
                    if (!IsConnected)
                    {
                        throw new SocketException();
                    }
                    List<string> data = new List<string>();
                    Byte[] byteData = new Byte[2048];
                    Int32 bytes = stream.Read(byteData, 0, byteData.Length);
                    string response = Encoding.UTF8.GetString(byteData, 0, bytes);
                    foreach (string line in response.Split('\u0004'))
                    {
                        data.Add(line);
                    }
                    if (buffer != "")
                    {
                        data[0] = buffer + data[0];
                        buffer = "";
                    }
                    for (int i = 0; i < data.Count - 1; i++)
                    {
                        string line = data[i].TrimEnd('\u0004');
                        if (line == null)
                        {
                            throw new Exception();
                        }
                        else if (line == "Close")
                        {
                            throw new IOException();
                        }
                        tcpQueue.Enqueue(line);
                    }
                    if (data.Last() != "")
                    {
                        buffer = data.Last();
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.");
                stream.Close();
                client.Close();
                while (tcpQueue.IsEmpty == false)
                {
                    tcpQueue.TryDequeue(out string result);
                }
                logger = "";
                try
                {
                    this.Invoke(new Action(() => { lblConnection.Text = "You're not connected to a logger."; }));
                }
                catch (InvalidOperationException)
                {
                    // This occurs if the main form is closed by user while message box is showing
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.");
                stream.Close();
                client.Close();
                while (tcpQueue.IsEmpty == false)
                {
                    tcpQueue.TryDequeue(out string result);
                }
                logger = "";
                try
                {
                    this.Invoke(new Action(() => { lblConnection.Text = "You're not connected to a logger."; }));
                }
                catch (InvalidOperationException)
                {
                    // This occurs if the main form is closed by user while message box is showing
                }
            }
            return;
        }

        // Used to receive data from the logger sent using TCP
        public string TCPReceive()
        {
            if (!IsConnected)
            {
                listener.Abort();
                stream.Close();
                client.Close();
                while (tcpQueue.IsEmpty == false)
                {
                    tcpQueue.TryDequeue(out string result);
                }
                logger = "";
                lblConnection.Text = "You're not connected to a logger.";
                throw new SocketException();
            }
            string response;
            while (tcpQueue.TryDequeue(out response) == false) { }
            return response;

        }

        // Reads program config
        // Test commit
        // Objective 1
        private void ReadProgConfig()
        {
            progConfig = new ProgConfig();
            // Opens config using StreamReader
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
                        // Set which section is being read from using section headers
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
                                        if (data.Length == 1)
                                        {
                                            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\anaconda3\Scripts\activate.bat"))
                                            {
                                                progConfig.activatePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\anaconda3\Scripts\activate.bat";
                                            }
                                            else
                                            {
                                                MessageBox.Show("Cannot find activate.bat for Anaconda, please edit settings and give the location of activate.bat.");
                                                progConfig.activatePath = "";
                                            }
                                        }
                                        else
                                        {
                                            if (File.Exists(data[1]))
                                            {
                                                progConfig.activatePath = data[1];
                                            }
                                            else
                                            {
                                                MessageBox.Show("Cannot find activate.bat for Anaconda, please edit settings and give the location of activate.bat.");
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
            // Read in simple config pins
            using (StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\configPresets.csv"))
            {
                // Read header line and ignore
                reader.ReadLine();
                char[] trimChars = new char[] { '\n', ' ' };
                string line = "";
                string prevSensor = "";
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Trim(trimChars);
                    string[] pinData = line.Split(',');
                    // Add sensor name and variation to variationDict (used for populating cmb menus)
                    if (pinData[0] != prevSensor && pinData[1] != "")
                    {
                        progConfig.variationDict.Add(pinData[0], new List<string> { pinData[1] });
                    }
                    else if (pinData[1] != "")
                    {
                        progConfig.variationDict[pinData[0]].Add(pinData[1]);
                    }
                    else if (pinData[0] != prevSensor)
                    {
                        progConfig.variationDict.Add(pinData[0], new List<string> { "N/A" });
                    }

                    // Add preset to dict of preset pins
                    string presetName = pinData[0] + ',' + pinData[1];
                    Pin tempPin = new Pin();
                    tempPin.fName = pinData[2];
                    tempPin.inputType = pinData[3];
                    tempPin.gain = Convert.ToInt32(pinData[4]);
                    tempPin.scaleMin = Convert.ToDouble(pinData[5]);
                    tempPin.scaleMax = Convert.ToDouble(pinData[6]);
                    tempPin.units = pinData[7];

                    progConfig.configPins.Add(presetName, tempPin);
                    prevSensor = pinData[0];
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


        private void InitialiseAppData()
        {
            // If SteerLogger directory doesn't exist in appData, crete it
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // If progConf.ini and configPresets.csv are not in the appData directory, add them
            string[] files = { "progConf.ini", "configPresets.csv" };
            foreach (string filename in files)
            {
                string output = dirPath + @"\" + filename;
                string file = Application.StartupPath + @"\" + filename;
                if (!File.Exists(output))
                {
                    File.Copy(filename, output);
                }
            }

            // If SteerLogger directory doesn't exist in appData, crete it
            dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\pythonScripts";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Add python files to appData directory
            string[] filePaths = Directory.GetFiles(Application.StartupPath + @"\pythonScripts");
            foreach (var filename in filePaths)
            {
                string file = filename.ToString();

                //Do your job with "file"  
                string str = dirPath + file.ToString().Replace(Application.StartupPath + @"\pythonScripts", "");
                if (!File.Exists(str))
                {
                    File.Copy(file, str);
                }
            }
        }


        // Loads the mainForm
        private void mainForm_Load(object sender, EventArgs e)
        {
            // Initialises appData and copies required files there
            InitialiseAppData();

            // Reads the program config file
            // Objective 1
            ReadProgConfig();
            SetupSimpleConf();
            // Starts the connect form, which searches for loggers and allows user to connect to one
            // Objective 2 and 3
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

            if (logger == "")
            {
                lblConnection.Text = "You are not connected to a logger.";
            }
            else
            {
                lblConnection.Text = String.Format("You are connected to {0} as {1}", logger, user);
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

            foreach (string value in progConfig.units)
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
                    //client.ReceiveTimeout = 100;
                    stream = client.GetStream();

                    listener = new Thread(TCPListen);
                    listener.Start();
                    TCPSend(user);
                    // Get the most recently used config settings from the logger
                    // Objective 5
                    GetRecentConfig();
                    // Get a list of logs that user hasn't downloaded
                    // Objective 4
                    RequestRecentLogs();
                }
                // If there is an issue connecting to Pi, catch error and continue without connection
                catch (SocketException)
                {
                    MessageBox.Show("An error occured in the connection, please reconnect.");
                    stream.Close();
                    client.Close();
                    while (tcpQueue.IsEmpty == false)
                    {
                        tcpQueue.TryDequeue(out string result);
                    }
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
        }

        // Used to get the logs the user hasn't downloaded
        // Objective 4
        private void RequestRecentLogs()
        {
            // Send command and username to logger
            TCPSend("Recent_Logs_To_Download");
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
                string[] data = response.Split('\u001f');
                LogMeta newLog = new LogMeta
                {
                    id = Convert.ToInt32(data[0]),
                    name = data[1],
                    testNumber = Convert.ToInt32(data[2]),
                    date = data[3],
                    project = Convert.ToInt32(data[4]),
                    workPack = Convert.ToInt32(data[5]),
                    jobSheet = Convert.ToInt32(data[6]),
                    size = (data[7] == "None") ? 0 : Convert.ToInt32(data[7])
                };
                logsAvailable.Add(newLog);
                response = TCPReceive();
            }
            // Show DownloadForm which allows user to select which logs to download
            DownloadForm download = new DownloadForm(this, logsAvailable, "Logs", false);
            download.ShowDialog();
            int numlogs = download.num;
            // Clear up resources
            download.Dispose();
            ReceiveProgressForm progressForm;
            dataQueue = new ConcurrentQueue<string>();
            progressForm = new ReceiveProgressForm();
            pbValue = 0;
            progressForm.Show();
            // progressForm.Show();
            // Receive the selected logs using TCP
            // Objective 4.1
            //ReceiveLog(false, progressForm);
            //ReceiveLog(false);
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += (s, args) => ReceiveLog(s,args,numlogs);
            worker.ProgressChanged += (s, e) => ProgressChanged(s,e,progressForm);
            worker.RunWorkerAsync();
        }

        // Receive a full log from the logger
        // Objectives 4.1 and 13.3
        private void ReceiveLog(object sender, EventArgs e, int numLogs)
        {
            try
            {
                int current = 0;
                BackgroundWorker worker = sender as BackgroundWorker;

                //progressForm.UpdateTextBox("Converting data on Pi...");
                //progressForm.UpdateProgressBar();
                string received = TCPReceive();
                // Continue receiving logs until all have been sent
                while (received != "All_Sent")
                {
                    prev = 0;
                    // Create a tempoary LogMeta to store log while its being received
                    LogMeta tempLog = new LogMeta();
                    current = CalcPercent(5, numLogs, current);
                    worker.ReportProgress(current, "Receiving Metadata...");
                    //progressForm.UpdateTextBox("Receiving meta data...");
                    // Receive meta data of log and set LogMeta variables 
                    while (received != "EoMeta")
                    {
                        string[] metaData = received.Split('\u001f');
                        tempLog.id = int.Parse(metaData[0]);
                        tempLog.project = int.Parse(metaData[1]);
                        tempLog.workPack = int.Parse(metaData[2]);
                        tempLog.jobSheet = int.Parse(metaData[3]);
                        tempLog.name = metaData[4];
                        tempLog.testNumber = int.Parse(metaData[5]);
                        tempLog.date = metaData[6];
                        tempLog.time = decimal.Parse(metaData[7]);
                        tempLog.loggedBy = metaData[8];
                        tempLog.downloadedBy = metaData[9];
                        tempLog.description = metaData[10];
                        received = TCPReceive();
                    }


                    current = CalcPercent(10, numLogs, current);
                    worker.ReportProgress(current, "Receiving Metadata...");
                    received = TCPReceive();
                    //progressForm.UpdateTextBox("Receiving config data...");
                    // Receive config settings of log and write to ConfigFile object
                    tempLog.config = new ConfigFile();
                    while (received != "EoConfig")
                    {
                        string[] pinData = received.Split('\u001f');
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
                        tempLog.config.pinList.Add(tempPin);
                        received = TCPReceive();
                    }

                    current = CalcPercent(20, numLogs, current);
                    worker.ReportProgress(current, "Receiving log data...");
                    // progressForm.UpdateTextBox("Receiving log data...");
                    received = TCPReceive();
                    // Set up rawheaders and convheaders for LogData object
                    //tempLog.logData = new LogData();
                    //tempLog.logData.rawheaders = new List<string> { "Date/Time", "Time (seconds)" };
                    //tempLog.logData.convheaders = new List<string> ();
                    int pinNum = 0;
                    //List<string> rawHeaders = new List<string>();
                    List<string> convHeaders = new List<string> { "Date/Time", "Time (seconds)" };
                    // Use config file to write pin headers to rawheaders and convheaders
                    foreach (Pin pin in tempLog.config.pinList)
                    {
                        if (pin.enabled == true)
                        {
                            pinNum += 1;
                            //rawHeaders.Add(pin.name);
                            convHeaders.Add(pin.fName + " | " + pin.units);
                        }
                    }
                    //tempLog.logData.rawheaders.AddRange(rawHeaders);
                    //tempLog.logData.convheaders.AddRange(convHeaders);
                    //tempLog.logData.InitRawConv(pinNum);
                    // AddHeaderDataViewProc(tempLog.logData.convheaders);
                    List<Pin> pins = new List<Pin>();
                    foreach (Pin pin in tempLog.config.pinList)
                    {
                        if (pin.enabled)
                        {
                            pins.Add(pin);
                        }
                    }

                    string host = logger;
                    string user = "pi";
                    string password = "raspberry";

                    SftpClient sftpclient = new SftpClient(host, 22, user, password);
                    // Need to catch error when computer refuses connection
                    try
                    {
                        sftpclient.Connect();
                    }
                    catch (SocketException)
                    {
                        dataQueue.Enqueue("Error occurred. Aborting!");
                        MessageBox.Show("Failed to download, check that Pi has FTP/SSH enabled.");
                        TCPSend("Quit");
                        stream.Close();
                        client.Close();
                        while (tcpQueue.IsEmpty == false)
                        {
                            tcpQueue.TryDequeue(out string result);
                        }
                        logger = "";
                        this.Invoke(new Action(() => { lblConnection.Text = "You're not connected to a logger."; }));
                        return;
                    }

                    // If SteerLogger directory doesn't exist in appData, crete it
                    string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    string path = @"/home/pi/Github/Datalogger-Alistair-Pi/" + received;
                    string temp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger\" + Path.GetFileName(received);

                    current = CalcPercent(40, numLogs, current);
                    worker.ReportProgress(current, "Downloading raw data...");
                    using (FileStream stream = new FileStream(temp, FileMode.Create))
                    {
                        sftpclient.DownloadFile(path, stream);
                    }
                    sftpclient.Dispose();

                    tempLog.raw = temp;

                    current = CalcPercent(60, numLogs, current);
                    worker.ReportProgress(current, "Converting data...");


                    // Read from the file selected
                    using (StreamReader reader = new StreamReader(temp))
                    {
                        using (StreamWriter writer = new StreamWriter(temp.Replace("raw", "converted")))
                        {
                            // Read over header line
                            reader.ReadLine();
                            writer.WriteLine(string.Join(",",convHeaders));
                            while (!reader.EndOfStream)
                            {
                                // Read each line and store the data in the logData object
                                string[] line = reader.ReadLine().Split(',');
                                string output = String.Format("{0},{1}",line[0],line[1]);

                                for (int i = 2; i < line.Length; i++)
                                {
                                    output += String.Format(",{0}", double.Parse(line[i]) * pins[i - 2].m + pins[i - 2].c);
                                }
                                writer.WriteLine(output);
                            }
                        }
                    }

                    tempLog.conv = tempLog.raw.Replace("raw", "converted");

                    current = CalcPercent(80, numLogs, current);
                    worker.ReportProgress(current, "Finalising download...");
                    // If there is already a log being processed, ask user if they want to merge logs
                    if (DAP.processing == true)
                    {
                        DialogResult dialogResult = MessageBox.Show("Would you like to merge the imported log with the current log?\n" +
                                                    "Otherwise the imported log will be added to the queue.",
                                                    "Merge Logs?", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            // If they want to merge, receive the log with merge argument set to true
                            DAP.logsProcessing.Add(tempLog);
                            LogProc tempProc = new LogProc();
                            tempProc.CreateProcFromConv(tempLog.conv);
                            DAP.MergeLogs(tempProc);
                            this.Invoke(new Action(() => { lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber; }));
                            this.Invoke(new Action(() => { PopulateDataViewProc(DAP.logProc); }));
                        }
                        else
                        {
                            // Receive log with merge argument set to false
                            DAP.logsToProc.Enqueue(tempLog);
                        }
                    }
                    // If no log to merge with, receive and add to queue
                    else
                    {
                        DAP.logsToProc.Enqueue(tempLog);
                        // Dequeue next log and display
                        if (DAP.logsToProc.Count > 0)
                        {
                            this.Invoke(new Action(() => { pnlCtrlConf.Hide(); }));
                            this.Invoke(new Action(() => { pnlDataProc.Show(); }));
                            DAP.logsProcessing.Clear();
                            DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                            DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].conv);
                            this.Invoke(new Action(() => { lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber; }));
                            this.Invoke(new Action(() => { PopulateDataViewProc(DAP.logProc); }));
                            DAP.processing = true;
                        }
                    }
                    received = TCPReceive();

                }
                worker.ReportProgress(100, "Download finished...");
                worker.Dispose();
            }
            catch (SocketException)
            {
                MessageBox.Show("Error occured in connection, please reconnect.");
                dataQueue.Enqueue("Error occurred. Aborting!");
                MessageBox.Show("Failed to download, check that Pi has FTP/SSH enabled.");
                TCPSend("Quit");
                stream.Close();
                client.Close();
                while (tcpQueue.IsEmpty == false)
                {
                    tcpQueue.TryDequeue(out string result);
                }
                logger = "";
                this.Invoke(new Action(() => { lblConnection.Text = "You're not connected to a logger."; }));
                return;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                MessageBox.Show(exp.ToString());
                dataQueue.Enqueue("Error occurred. Aborting!");
                MessageBox.Show("Failed to download, check that Pi has FTP/SSH enabled.");
                TCPSend("Quit");
                stream.Close();
                client.Close();
                while (tcpQueue.IsEmpty == false)
                {
                    tcpQueue.TryDequeue(out string result);
                }
                logger = "";
                this.Invoke(new Action(() => { lblConnection.Text = "You're not connected to a logger."; }));
                return;
            }
            
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

        int prev = 0;
        private int CalcPercent(int value, int div, int current)
        {
            int percent = Convert.ToInt32(Convert.ToDouble(current) + Convert.ToDouble(value - prev) * (1d / Convert.ToDouble(div)));
            prev = value;
            return percent;
        }


        // Display logProc in DataProc grid
        // Objectives 4.2 and 15.2
        DataTable table;
        private void PopulateDataViewProc(LogProc logToShow)
        {
            if (table != null)
            {
                table.Dispose();
            }
            table = new DataTable();
            foreach (string header in logToShow.procheaders)
            {
                if (header == "Date/Time")
                {
                    table.Columns.Add(header, typeof(DateTime));
                }
                else 
                {
                    table.Columns.Add(header, typeof(double));
                }
            }

            // Enumerate through logProc and add data to grid
            for (int i = 0; i < logToShow.timestamp.Count; i++)
            {
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
            dgvDataProc.DataSource = table;
            dgvDataProc.Columns[0].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss.fff";



            /*
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
                foreach (List<double> column in logToShow.procData)
                {
                    newRow.Add(column[i].ToString());
                }
                dgvDataProc.Rows.Add(newRow.ToArray());
            }*/
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
            received = TCPReceive();
            // Receive pin data until all data has been sent
            while (received != "EoConfig")
            {
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
                received = TCPReceive();
            }
            SetupSimpleConf();
        }

        // Populates InputSetup grid with default values if recent config can't be gotten
        private void LoadDefaultConfig()
        {
            txtLogName.Text = "";
            nudInterval.Value = 1.0M;
            txtDescription.Text = "";
            nudProject.Value = 0;
            nudWorkPack.Value = 0;
            nudJobSheet.Value = 0;
            int number = 1;
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
                return;
            }
            // If the data there is being processed, ask if user wants to save before clearing
            DialogResult dialogResult = MessageBox.Show("Do you want to save data before clearing?", "Clear Data", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                cmdDwnldCsv.PerformClick();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            foreach (LogMeta log in DAP.logsProcessing)
            {
                File.Delete(log.raw);
                File.Delete(log.conv);
            }

            // If there is a log in the processing queue, display that log
            if (DAP.logsToProc.Count > 0)
            {
                DAP.logsProcessing.Clear();
                DAP.logsProcessing.Add(DAP.logsToProc.Dequeue());
                DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].conv);
                lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
                PopulateDataViewProc(DAP.logProc);
                DAP.processing = true;
            }
            else
            {
                DAP.logsProcessing.Clear();
                lblLogDisplay.Text = "No Log Displaying";
                DAP.processing = false;
                dgvDataProc.DataSource = null;
                dgvDataProc.Rows.Clear();
                dgvDataProc.Columns.Clear();
            }

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        // Import config from Pi
        // Objective 8
        private void cmdImportConf_Click(object sender, EventArgs e)
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
                string[] data = response.Split('\u001f');
                LogMeta newLog = new LogMeta
                {
                    id = Convert.ToInt32(data[0]),
                    name = data[1],
                    testNumber = Convert.ToInt32(data[2]),
                    date = data[3],
                    project = Convert.ToInt32(data[4]),
                    workPack = Convert.ToInt32(data[5]),
                    jobSheet = Convert.ToInt32(data[6]),
                    size = (data[7] == "None") ? 0 : Convert.ToInt32(data[7])
                };
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
            txtDescription.Text = response.Replace(";","\r\n");
            response = TCPReceive();
            txtLogName.Text = response;
            response = TCPReceive();
            nudProject.Value = Convert.ToInt32(response);
            response = TCPReceive();
            nudWorkPack.Value = Convert.ToInt32(response);
            response = TCPReceive();
            nudJobSheet.Value = Convert.ToInt32(response);
            response = TCPReceive();

            // Recevie data for each pin until all pins have been received
            // Objective 8.4
            while (response != "Config_Sent")
            {
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
                response = TCPReceive();
            }
            SetupSimpleConf();
        }


        // Imports a config from a config file
        // Objective 8.1
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
                                general = false;
                                // Increase pin number by one to move one row down on the grid
                                pinNumber += 1;
                                // Reset cell number to start in first column on grid
                                cellNumber = 0;
                                dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = pinNumber + 1;
                                // Cell number increased by one each time a value is changed to change column
                                cellNumber += 1;

                                string pinName = line.Replace("[", "").Replace("]", "");
                                dgvInputSetup.Rows[pinNumber].Cells[cellNumber].Value = pinName;
                                cellNumber += 1;
                            }
                            else if (general == true)
                            {
                                string[] data = line.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                                // Only time interval is imported from general settings
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
                SetupSimpleConf();
            }
        }

        // Save config
        private void cmdSave_Click(object sender, EventArgs e)
        {
            // Validate config settings
            if (ValidateConfig())
            {
                try
                {
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
                    // Invalid scalemin or scalemax data
                    return;
                }
            }
        }


        // Uploads config settings to Pi
        // Objective 10
        private void cmdSaveUpload_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate config settings
                if (ValidateConfig())
                {
                    UploadConfig(CreateConfig());
                }
            }
            // Catch errors when uploading config to Pi
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.");
                return;
            }
            catch (InvalidDataException)
            {
                return;
            }
        }

        // Validates the config settings
        private bool ValidateConfig()
        {
            // Make sure user has given the log a name
            if (txtLogName.Text == "")
            {
                MessageBox.Show("Please input a value for the log name.");
                return false;
            }
            char[] invalidFileChars = Path.GetInvalidFileNameChars();
            foreach (char invalid in invalidFileChars)
            {
                if (txtLogName.Text.Contains(invalid))
                {
                    MessageBox.Show(String.Format("The log name contains invalid character: {0}",invalid));
                    return false;
                }
            }
            // If user hasn't added description, ask if they want to add one
            if (txtDescription.Text == "")
            {
                if (MessageBox.Show("No description. Press OK to continue without description or cancel to cancel upload and add description.","No Description!",MessageBoxButtons.OKCancel) != DialogResult.OK)
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

            if (!(nudProject.Value % 1 == 0))
            {
                MessageBox.Show("Project number must be a whole number.");
                return false;
            }

            if (!(nudWorkPack.Value % 1 == 0))
            {
                MessageBox.Show("Work pack number must be a whole number.");
                return false;
            }

            if (!(nudJobSheet.Value % 1 == 0))
            {
                MessageBox.Show("Job sheet number must be a whole number.");
                return false;
            }

            return true;
        }


        private LogMeta CreateConfig()
        {
            // Create LogMeta to store settings
            LogMeta newLog = new LogMeta();
            newLog.project = Convert.ToInt32(nudProject.Value);
            newLog.workPack = Convert.ToInt32(nudWorkPack.Value);
            newLog.jobSheet = Convert.ToInt32(nudJobSheet.Value);
            newLog.name = txtLogName.Text;
            newLog.time = nudInterval.Value;
            newLog.loggedBy = user;
            string description = txtDescription.Text;
            description = string.Join(";", description.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            newLog.description = description;

            int enabled = 0;
            ConfigFile newConfig = new ConfigFile();
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                // Create a new Pin from each InputSetup row
                Pin newPin = new Pin();
                newPin.id = Convert.ToInt32(row.Cells[0].Value);
                newPin.name = row.Cells[1].Value.ToString();
                newPin.enabled = Convert.ToBoolean(row.Cells[2].Value);
                newPin.fName = row.Cells[3].Value.ToString();
                newPin.inputType = row.Cells[4].Value.ToString();
                newPin.gain = Convert.ToInt32(row.Cells[5].Value);
                double scaleMin;
                // Make sure that scaleMin is a decimal value, not a string
                if (double.TryParse(row.Cells[6].Value.ToString(), out scaleMin))
                {
                    newPin.scaleMin = scaleMin;
                }
                else
                {
                    MessageBox.Show("Please check that all Scale Min values are deciamls.");
                    throw new InvalidDataException();
                }
                double scaleMax;
                // Make sure that scaleMax is a decimal value, not a string
                if (double.TryParse(row.Cells[7].Value.ToString(), out scaleMax))
                {
                    newPin.scaleMax = scaleMax;
                }
                else
                {
                    MessageBox.Show("Please check that all Scale Max values are deciamls.");
                    throw new InvalidDataException();
                }
                newPin.units = row.Cells[8].Value.ToString();
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
                newConfig.pinList.Add(newPin);
            }
            if (enabled == 0)
            {
                MessageBox.Show("Please set at least one pin to enabled.", "No Pins Enabled!");
                throw new InvalidDataException();
            }
            newLog.config = newConfig;
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
        // Objective 10.1
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
        }

        // Uploads a new log config to the logger
        // Objective 10.2
        private void UploadConfig(LogMeta newLog)
        {
            // Send command to logger so it can receive the config
            TCPSend("Upload_Config");
            // Send metadata to the logger
            string metadata = "";
            metadata += newLog.project + "\u001f";
            metadata += newLog.workPack + "\u001f";
            metadata += newLog.jobSheet + "\u001f";
            metadata += newLog.name + "\u001f";
            metadata += newLog.date + "\u001f";
            metadata += newLog.time + "\u001f";
            metadata += newLog.loggedBy + "\u001f";
            metadata += newLog.downloadedBy + "\u001f";
            metadata += newLog.description;
            TCPSend(metadata);
            // Enumerate through pinList and send settings for each Pin to logger
            foreach (Pin pin in newLog.config.pinList)
            {
                string pindata = "";
                pindata += pin.id + "\u001f";
                pindata += pin.name + "\u001f";
                pindata += pin.enabled + "\u001f";
                pindata += pin.fName + "\u001f";
                pindata += pin.inputType + "\u001f";
                pindata += pin.gain + "\u001f";
                pindata += pin.scaleMin + "\u001f";
                pindata += pin.scaleMax + "\u001f";
                pindata += pin.units + "\u001f";
                pindata += pin.m + "\u001f";
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
                MessageBox.Show("An error occured in the connection, or you are not connected. Please reconnect.");
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
                MessageBox.Show("An error occured in the connection, or you are not connected. Please reconnect.");
            }
        }

        // Reset InputSetup grid to default values
        private void cmdResetConfig_Click(object sender, EventArgs e)
        {
            txtLogPins.Text = "";
            dgvInputSetup.Rows.Clear();
            LoadDefaultConfig();
            SetupSimpleConf();
        }

        // Allows user to reconnect to logger or connect to a different one
        private void cmdConnect_Click(object sender, EventArgs e)
        {
            if (listener != null && listener.IsAlive)
            {
                TCPSend("Quit");
                listener.Abort();
            }
            // Close current TCP connection
            if (stream != null)
            {
                stream.Close();
                client.Close();
                while (tcpQueue.IsEmpty == false)
                {
                    tcpQueue.TryDequeue(out string result);
                }
            }
            logger = "";
            lblConnection.Text = "You're not connected to a logger.";
            // Create new connectForm to search for available loggers
            ConnectForm connectForm = new ConnectForm(progConfig.loggers.ToArray());
            connectForm.user = user;
            connectForm.ShowDialog();
            logger = connectForm.logger;
            user = connectForm.user;
            // Try to connect to logger selected by user
            if (logger != null && logger != "")
            {
                try
                {
                    Int32 port = 13000;
                    client = new TcpClient(logger, port);
                    stream = client.GetStream();
                    listener = new Thread(TCPListen);
                    listener.Start();
                    TCPSend(user);
                }
                catch (SocketException)
                {
                    MessageBox.Show("An error occured in the connection, please reconnect.");
                    logger = "";
                    return;
                }
                // Update display to show user is connected
                lblConnection.Text = String.Format("You are connected to: {0} as {1}",logger,user);
            }
            else
            {
                logger = "";
                lblConnection.Text = "You're not connected to a logger.";
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
                    listener.Abort();
                    TCPSend("Quit");
                    stream.Close();
                    client.Close();
                    while (tcpQueue.IsEmpty == false)
                    {
                        tcpQueue.TryDequeue(out string result);
                    }
                }
            }
            // If an error occurs, close stream and client anyway as program is exiting, don't need to alert user
            catch (SocketException)
            {
                stream.Close();
                client.Close();
                while (tcpQueue.IsEmpty == false)
                {
                    tcpQueue.TryDequeue(out string result);
                }
            }

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
                        // Ignore errors
                    }
                }
            }
        }

        // Imports log data from a csv file
        // Objective 12
        private void cmdImportLogFile_Click(object sender, EventArgs e)
        {
            // Create new logMeta and logData to hold log
            LogMeta logMeta = new LogMeta();
            //logMeta.logData = new LogData();
            if (ofdLog.ShowDialog() == DialogResult.OK)
            {
                // Set log name to name of file imported
                logMeta.name = ofdLog.SafeFileName.Replace("converted-", "");
                logMeta.name = logMeta.name.Replace(".csv", "");
                logMeta.conv = ofdLog.FileName;
                //// Read from the file selected
                //using (StreamReader reader = new StreamReader(ofdLog.OpenFile()))
                //{
                //    // Read the headerline and set convheaders
                //    logMeta.logData.convheaders.AddRange(reader.ReadLine().Split(','));
                //    // Initialise convData using the number of headers
                //    logMeta.logData.InitRawConv(logMeta.logData.convheaders.Count - 2);
                //    while (!reader.EndOfStream)
                //    {
                //        // Read each line and store the data in the logData object
                //        string[] line = reader.ReadLine().Split(',');
                //        logMeta.logData.timestamp.Add(Convert.ToDateTime(line[0]));
                //        logMeta.logData.time.Add(Convert.ToDouble(line[1]));
                //        List<double> convData = new List<double>();
                //        for (int i = 2; i < line.Length; i++)
                //        {
                //            convData.Add(double.Parse(line[i]));
                //        }
                //        logMeta.logData.AddConvData(convData);
                //    }
                //}
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
                        tempProc.CreateProcFromConv(logMeta.conv);
                        // Merge logs together
                        DAP.MergeLogs(tempProc);
                        //dgvDataProc.Columns.Clear();
                        // Update data display
                        lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
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
                        DAP.logProc.CreateProcFromConv(DAP.logsProcessing[0].conv);
                        lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
                        PopulateDataViewProc(DAP.logProc);
                        DAP.processing = true;
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
        // Objective 13
        private void cmdImportLogPi_Click(object sender, EventArgs e)
        {
            try
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
                    string[] data = response.Split('\u001f');
                    LogMeta newLog = new LogMeta
                    {
                        id = Convert.ToInt32(data[0]),
                        name = data[1],
                        testNumber = Convert.ToInt32(data[2]),
                        date = data[3],
                        project = Convert.ToInt32(data[4]),
                        workPack = Convert.ToInt32(data[5]),
                        jobSheet = Convert.ToInt32(data[6]),
                        size = (data[7] == "None") ? 0 : Convert.ToInt32(data[7])
                    };
                    logsAvailable.Add(newLog);
                    response = TCPReceive();
                }
                // Create new DownloadForm so user can select logs to download
                // Objective 13.2
                DownloadForm download = new DownloadForm(this, logsAvailable, "Logs", false);
                download.ShowDialog();
                int numLogs = download.num;
                download.Dispose();
                ReceiveProgressForm progressForm;
                progressForm = new ReceiveProgressForm();
                progressForm.Show();
                pbValue = 0;

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += (s,args) => ReceiveLog(s,args,numLogs);
                worker.ProgressChanged += (s, args) => ProgressChanged(s, args, progressForm);
                worker.RunWorkerAsync();

            }
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, or you are not connected. Please reconnect.");
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
                    sfdConfig.FileName = "logConf-" + log.name + "-" + log.testNumber + "-" + log.date + ".ini";
                    sfdConfig.DefaultExt = "ini";
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
                    sfdLog.FileName = "raw-" + log.name + "-" + log.testNumber + "-" + log.date + ".csv";
                    sfdLog.DefaultExt = "csv";
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
                    sfdLog.FileName = "converted-" + log.name + "-" + log.testNumber + "-" + log.date + ".csv";
                    sfdLog.DefaultExt = "csv";
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
                sfdLog.FileName = "processed-" + DAP.logsProcessing[0].name + "-" + DAP.logsProcessing[0].testNumber + "-" + DAP.logsProcessing[0].date + ".csv";
                sfdLog.DefaultExt = "csv";
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


        private void SaveCsv(string temp, string output)
        {
            if (File.Exists(temp))
            {
                File.Copy(temp, output);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        /*
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
                    foreach (List<double> column in log.rawData)
                    {
                        line += column[i] + ",";
                    }
                    writer.WriteLine(line.Trim(','));
                }
            }
        }
        */

        /*
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
                    foreach (List<double> column in log.convData)
                    {
                        line += column[i] + ",";
                    }
                    writer.WriteLine(line.Trim(','));
                }
            }
        }
        */

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
                    foreach (List<double> column in logProc.procData)
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
                    SaveConfig(log, confPath);
                }

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

            sfdLog.FileName = DAP.logsProcessing[0].name + ".zip";

            if (sfdLog.ShowDialog() == DialogResult.OK)
            {
                // Create a zip archive from the temporary zip directory
                // Save zip archive to path specified by user
                ZipFile.CreateFromDirectory(dirPath, sfdLog.FileName);
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
                ExcelForm excelForm = new ExcelForm(DAP.logProc, excel);
                try
                {
                    excelForm.ShowDialog();
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Excel closed before form closed
                }
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

            // Make sure scripts and files are in appData
            InitialiseAppData();
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
            ofdPythonScript.InitialDirectory = dirPath + @"\pythonScripts";

            // Save data to temporary csv in appData directory
            SaveProcCsv(DAP.logProc, dirPath + @"\temp.csv");

            string script = "";
            if (ofdPythonScript.ShowDialog() == DialogResult.OK)
            {
                // Set script to user selected python script
                script = ofdPythonScript.FileName;
                // Get path to activate.bat
                string condaPath = progConfig.activatePath;
                if (File.Exists(condaPath) == false)
                {
                    MessageBox.Show("activate.bat is not the the location expected. Please change the settings to correct the activate.bat location."
                                    + Environment.NewLine + "Expected location: " + condaPath);
                    return;
                }
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
                lblLogDisplay.Text = "Displaying: " + DAP.logsProcessing[0].name + " " + DAP.logsProcessing[0].testNumber;
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
        // Objective 16
        private void cmdPythonGraph_Click(object sender, EventArgs e)
        {
            // Make sure there is data to be graphed
            if (DAP.logsProcessing.Count == 0)
            {
                MessageBox.Show("No log data to process, please import log data and try again.");
                return;
            }

            // Make sure scripts and files are in appData
            InitialiseAppData();
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SteerLogger";
            ofdPythonScript.InitialDirectory = dirPath + @"\pythonScripts";

            // Save data to temporary csv in python script directory
            SaveProcCsv(DAP.logProc, dirPath + @"\temp.csv");

            string script = "";
            if (ofdPythonScript.ShowDialog() == DialogResult.OK)
            {
                // Set script to user selected python script
                script = ofdPythonScript.FileName;
                // Get the path to activate .bat
                string condaPath = progConfig.activatePath;
                if (File.Exists(condaPath) == false)
                {
                    MessageBox.Show("activate.bat is not the the location expected. Please change the settings to correct the activate.bat location."
                                    + Environment.NewLine + "Expected location: " + condaPath);
                    return;
                }
                // Construct the argument to pass to the command shell
                string cmdArguments = "/c \"chdir " + dirPath + "\\ && call " + condaPath + " && python " + script + " " + dirPath + "\"";

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
            SettingsForm settings = new SettingsForm(progConfig);
            settings.ShowDialog();
            ReadProgConfig();
            SetupSimpleConf();
        }

        private void cmdAbt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Epic new logger!!", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmdConfigSwitch_Click(object sender, EventArgs e)
        {
            if (dgvInputSetup.Visible == false)
            {
                //cmbPin.Visible = false;
                //cmbVar.Visible = false;
                //cmbSensor.Visible = false;
                //cmdAddPin.Visible = false;
                //txtLogPins.Visible = false;
                pnlSimpleConfig.Visible = false;
                cmdConfigSwitch.Text = "Simple Config";
                dgvInputSetup.Visible = true;
            }
            else
            {
                dgvInputSetup.Visible = false;
                //cmbPin.Visible = true;
                //cmbVar.Visible = true;
                //cmbSensor.Visible = true;
                //cmdAddPin.Visible = true;
                //txtLogPins.Visible = true;
                pnlSimpleConfig.Visible = true;
                SetupSimpleConf();
                cmdConfigSwitch.Text = "Advanced Config";
            }
        }


        private void SetupSimpleConf()
        {
            cmbPin.Items.Clear();
            for (int i = 1; i <= 16; i++)
            {
                cmbPin.Items.Add(i.ToString());
            }
            cmbPin.SelectedIndex = 0;

            cmbSensor.Items.Clear();
            foreach (string sensor in progConfig.variationDict.Keys)
            {
                cmbSensor.Items.Add(sensor);
            }
            cmbSensor.SelectedIndex = 0;

            cmbVar.Items.Clear();
            cmbVar.Enabled = true;
            foreach (string variation in progConfig.variationDict[cmbSensor.SelectedItem.ToString()])
            {
                cmbVar.Items.Add(variation);
            }
            cmbVar.SelectedIndex = 0;

            if (cmbVar.SelectedItem.ToString() == "N/A")
            {
                cmbVar.Enabled = false;
            }
            txtLogPins.Text = "";
            foreach (DataGridViewRow row in dgvInputSetup.Rows)
            {
                if (Convert.ToBoolean(row.Cells[2].Value) == true)
                {
                    txtLogPins.Text += "Pin " + row.Cells[0].Value + " is set to log " 
                                       + row.Cells[3].Value + "." + Environment.NewLine;
                }
            }
        }

        private void cmbSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbVar.Items.Clear();
            cmbVar.Enabled = true;
            foreach (string variation in progConfig.variationDict[cmbSensor.SelectedItem.ToString()])
            {
                cmbVar.Items.Add(variation);
            }
            cmbVar.SelectedIndex = 0;

            if (cmbVar.SelectedItem.ToString() == "N/A")
            {
                cmbVar.Enabled = false;
            }
        }

        private void cmdAddPin_Click(object sender, EventArgs e)
        {
            string preset = cmbSensor.SelectedItem.ToString() + ',';
            if (cmbVar.Enabled)
            {
                preset += cmbVar.SelectedItem.ToString();
            }
            Pin pin = progConfig.configPins[preset];
            int row = cmbPin.SelectedIndex;
            dgvInputSetup.Rows[row].Cells[2].Value = true;
            dgvInputSetup.Rows[row].Cells[3].Value = pin.fName;
            dgvInputSetup.Rows[row].Cells[4].Value = pin.inputType;
            dgvInputSetup.Rows[row].Cells[5].Value = pin.gain.ToString();
            dgvInputSetup.Rows[row].Cells[6].Value = pin.scaleMin.ToString();
            dgvInputSetup.Rows[row].Cells[7].Value = pin.scaleMax.ToString();
            dgvInputSetup.Rows[row].Cells[8].Value = pin.units;

            txtLogPins.Text += "Added " + cmbSensor.SelectedItem.ToString() +
                               " " + (cmbVar.SelectedItem.ToString() == "N/A" ? "" : cmbVar.SelectedItem.ToString()) + 
                               " to log." + Environment.NewLine;
        }

        private void cmdRemovePin_Click(object sender, EventArgs e)
        {
            int row = cmbPin.SelectedIndex;
            dgvInputSetup.Rows[row].Cells[2].Value = false;
            txtLogPins.Text += "Removed pin " + (row + 1).ToString() + ": " 
                + dgvInputSetup.Rows[row].Cells[3].Value + " from log." + Environment.NewLine;
        }
    }
}
