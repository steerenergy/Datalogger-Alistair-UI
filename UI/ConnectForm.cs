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
        public ConnectForm(string[] logger_arr)
        {
            loggers = logger_arr;
            this.FormClosed += new FormClosedEventHandler(ConnectFormClosed);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pbScan.Value = 0;
        }


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
                    // Objective 3
                    TcpClient client = new TcpClient(logger, port);
                    // If a logger can be connected to, add its name to the drop down menu
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
                    MessageBox.Show("Failed to connect to the logger.\n" +
                        "Make sure the name is typed correctly or try using scan to find available loggers.");
                    logger = "";
                    return;
                }
            }
            user = txtUser.Text;
            // Close form after logger and user are selected
            this.Close();
        }

        private void cmdScan_Click(object sender, EventArgs e)
        {
            pbScan.Value = 0;
            pbScan.Enabled = true;
            //pbScan.MarqueeAnimationSpeed = 100;
            pbScan.Style = ProgressBarStyle.Continuous;
            cmdScan.Enabled = false;
            cmdSelect.Enabled = false;
            cmbLogger.Enabled = false;
            cmbLogger.Items.Clear();

            worker = new BackgroundWorker();
            worker.DoWork += Scan;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += ScanComplete;
            worker.RunWorkerAsync();

        }


        private void Scan(object sender, DoWorkEventArgs e)
        {
            Int32 port = 13000;
            // Enumerate through known logger names (stored in the program config)
            // Objective 2
            int num = 0;
            foreach (string logger in loggers)
            {
                num += 1;
                try
                {
                    // Attempt to connect to logger
                    // Objective 3
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
                worker.ReportProgress(num * 100 / loggers.Length);
            }
            worker.ReportProgress(100);
        }

        private void ScanComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            // If no loggers can be found, alert user
            if (cmbLogger.Items.Count == 0)
            {
                MessageBox.Show("No loggers found online.");
            }
            else
            {
                this.Invoke(new Action(() => { cmbLogger.SelectedIndex = 0; }));
            }

            if (user != null)
            {
                this.Invoke(new Action(() => { txtUser.Text = user; }));
            }


            pbScan.Enabled = false;
            pbScan.Style = ProgressBarStyle.Continuous;
            pbScan.Value = pbScan.Minimum;
            cmdScan.Enabled = true;
            cmdSelect.Enabled = true;
            cmbLogger.Enabled = true;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbScan.Value = e.ProgressPercentage;
        }

        private void ConnectFormClosed(object sender, FormClosedEventArgs e)
        {
            if (worker != null && worker.IsBusy)
            {
                worker.CancelAsync();
            }   
        }
    }
}
