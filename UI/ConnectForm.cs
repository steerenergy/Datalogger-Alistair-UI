using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace SteerLoggerUser
{
    public partial class ConnectForm : Form
    {
        private string[] loggers;
        public string logger;
        public string user;
        BackgroundWorker worker;

        // Array of logger names passed as parameter to form
        public ConnectForm(string[] logger_arr, string user)
        {
            loggers = logger_arr;
            this.user = user;
            this.FormClosed += new FormClosedEventHandler(ConnectFormClosed);
            InitializeComponent();
        }


        // When form loads, set progress bar value to 0
        private void Form1_Load(object sender, EventArgs e)
        {
            pbScan.Value = 0;
            txtUser.Text = user;
        }


        // Attempt to connect to selected logger to see if it is online
        private void cmdSelect_Click(object sender, EventArgs e)
        {
            // When select is clicked, set logger and user variables
            // These variables are accessed by mainForm to reconnect to selected logger
            logger = "";
            logger = cmbLogger.Text;
            if (logger != "")
            {
                try
                {
                    Int32 port = 13000;
                    // Attempt to connect to logger
                    TcpClient client = new TcpClient(logger, port);
                    NetworkStream stream = client.GetStream();
                    // Quit connection so main form can be loaded
                    string command = "Quit\u0004";
                    Byte[] data = System.Text.Encoding.UTF8.GetBytes(command);
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    client.Close();

                }
                // Catch exception if logger cannot be accessed
                catch (SocketException)
                {
                    MessageBox.Show("Failed to connect to the logger.\n" +
                                    "Make sure the name is typed correctly or try using scan to find available loggers.",
                                    "Connection Failed",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    logger = "";
                    return;
                }
            }
            user = txtUser.Text;
            // Close form after logger and user are selected
            this.Close();
        }


        // Scan local network for logger hostnames gotten from progConf.ini
        private void cmdScan_Click(object sender, EventArgs e)
        {
            // Setup progress bar
            pbScan.Value = 0;
            pbScan.Enabled = true;
            pbScan.Style = ProgressBarStyle.Continuous;
            // Disable scan button, select button and logger drop down menu until scan is complete
            cmdScan.Enabled = false;
            cmdSelect.Enabled = false;
            cmbLogger.Enabled = false;
            cmbLogger.Items.Clear();
            // Create new BackgroundWorker so scanning doesn't halt GUI
            worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            // Setup worker event handlers
            worker.DoWork += Scan;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += ScanComplete;
            worker.RunWorkerAsync();

        }


        // The function run by the worker
        private void Scan(object sender, DoWorkEventArgs e)
        {
            // Set port to 13000 (the port all logger software uses)
            Int32 port = 13000;
            // Enumerate through known logger names (stored in the program config)
            int num = 0;
            foreach (string logger in loggers)
            {
                num += 1;
                try
                {
                    // Attempt to connect to logger
                    TcpClient client = new TcpClient(logger, port);
                    // If a logger can be connected to, add its name to the drop down menu
                    this.Invoke(new Action(() => { cmbLogger.Items.Add(logger); }));
                    //cmbLogger.Items.Add(logger);
                    NetworkStream stream = client.GetStream();
                    // Quit connection so other loggers can be connected to
                    string command = "Quit\u0004";
                    Byte[] data = System.Text.Encoding.UTF8.GetBytes(command);
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    client.Close();
                }
                // Catch exception if logger cannot be accessed
                catch (SocketException)
                {
                    //Logger not online
                }
                // Check for cancellation
                if (worker.CancellationPending)
                {
                    worker.Dispose();
                    return;
                }
                // Report progress of scan
                worker.ReportProgress(num * 100 / loggers.Length);
            }
            worker.ReportProgress(100);
        }


        // When scan finishes, update GUI
        private void ScanComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            // If no loggers can be found, alert user
            if (cmbLogger.Items.Count == 0)
            {
                MessageBox.Show("No loggers found online.", "No Loggers Online",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {
                this.Invoke(new Action(() => { cmbLogger.SelectedIndex = 0; }));
            }
            // Stop progress bar
            pbScan.Enabled = false;
            pbScan.Style = ProgressBarStyle.Continuous;
            pbScan.Value = pbScan.Minimum;
            // Reenable controls
            cmdScan.Enabled = true;
            cmdSelect.Enabled = true;
            cmbLogger.Enabled = true;
        }


        // Updates the progress bar depending on workers progress
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbScan.Value = e.ProgressPercentage;
        }


        // Cancel worker if connect form is closed during scan
        private void ConnectFormClosed(object sender, FormClosedEventArgs e)
        {
            if (worker != null && worker.IsBusy)
            {
                worker.CancelAsync();
            }   
        }
    }
}
