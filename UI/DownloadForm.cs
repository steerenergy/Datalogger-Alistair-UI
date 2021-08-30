using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace SteerLoggerUser
{
    public partial class DownloadForm : Form
    {
        // Store item being downloaded (Config or Logs)
        private string item;
        // Determines whether user can select more that one log
        private bool one;
        // Stores TCPReceive function from mainForm
        private Func<string> TCPReceive;
        // Stores TCPSend function from mainForm
        private Action<string> TCPSend;
        // Stores TCP TearDown function from mainForm
        private Action TCPTearDown;
        // Stores number of logs that matched criteria
        private int numLogs;
        // Used to tell mainForm if user exits without selecting anything
        private bool cancelled = true;

        public DownloadForm(string item, bool one, Func<string> TCPReceive, int numLogs, Action<string> TCPSend, Action TCPTearDown)
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(DownloadFormClosed);
            this.item = item;
            this.one = one;
            this.TCPReceive = TCPReceive;
            this.numLogs = numLogs;
            this.TCPSend = TCPSend;
            this.TCPTearDown = TCPTearDown;
        }


        // Show logs to user and allow them to select which ones to download
        private void DownloadForm_Load(object sender, EventArgs e)
        {
            // Setup labels
            lblDownload.Text = "Select " + item + " to Download";
            cmdDownload.Text = "Download " + item;
            // Add logs to data grid view
            for (int i = 0; i < numLogs; i++)
            {
                try
                {
                    string[] response = TCPReceive().Split('\u001f');
                    try
                    {
                        object[] rowData = new object[]
                        {
                        false,
                        Convert.ToUInt32(response[0]),
                        response[1],
                        Convert.ToUInt32(response[2]),
                        ParseDateTime(response[3]),
                        Convert.ToUInt32(response[4]),
                        Convert.ToUInt32(response[5]),
                        Convert.ToUInt32(response[6]),
                        response[7],
                        (response[8] == "None") ? 0 : Convert.ToUInt32(response[8])
                        };
                        dgvDownload.Rows.Add(rowData);
                    }
                    // Catch errors from invalid/corrupt data
                    catch (FormatException)
                    {
                        MessageBox.Show("Received unexcepted data, make sure both programs are up to date and try again.",
                            "Incorrect Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TCPTearDown();
                        Close();
                        return;
                    }
                }
                // Catch connection errors
                catch (SocketException)
                {
                    MessageBox.Show("An error occured in the connection, please reconnect.", "Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("Connection timed out, please reconnect.", "Timeout Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
            }
            cmdDownload.Width = dgvDownload.Width;
        }


        // Parses date string to look nice
        private DateTime ParseDateTime(string date)
        {
            date = date.Replace("-", "");
            StringBuilder dateTime = new StringBuilder();
            // Array for number of characters in each part of the DateTime
            int[] counters = { 4, 2, 2, 2, 2, 2 };
            // Array for separators between each part of the DateTime
            char[] separators = { '/', '/', ' ', ':', ':'};
            int pos = 0;
            int sepPos = 0;
            // Iterate through date string and add characters and separators in order
            foreach (int counter in counters)
            {
                for (int i = 0; i < counter; i++)
                {
                    dateTime.Append(date[pos]);
                    pos += 1;
                }

                if (pos < date.Length - 1)
                {
                    dateTime.Append(separators[sepPos]);
                    sepPos += 1;
                }
            }
            return Convert.ToDateTime(dateTime.ToString());
        }

        // Sends the selected log ids to the logger
        private void cmdDownload_Click(object sender, EventArgs e)
        {
            try
            {
                this.cancelled = false;
                string logNames = "";
                int num = 0;
                // Add selected log id's to a string of logNames
                foreach (DataGridViewRow row in dgvDownload.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        logNames += row.Cells[1].Value + "\u001f";
                        num += 1;
                    }
                }
                // If no logs selected, let logger know
                if (logNames == "")
                {
                    TCPSend(item);
                    TCPSend("No_Logs_Requested");
                }
                // If more than one log selected when only one can be downloaded, alert user
                else if (this.one == true && num > 1)
                {
                    MessageBox.Show("Please only select one item as only one can be downloaded in this instance.",
                                    "Only Select One", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    // Send the item being downloaded to the logger and then the IDs of the logs
                    TCPSend(item);
                    TCPSend(logNames.Substring(0, logNames.Length - 1));
                }
                this.Close();
            }
            catch (SocketException)
            {
                MessageBox.Show("An error occured in the connection, please reconnect.", "Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancelled = true;
                Close();
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("You need to be connected to a logger to do that!",
                                "Connect to a Logger", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                cancelled = true;
                Close();
            }
        }

        void DownloadFormClosed(object sender, FormClosedEventArgs e)
        {
            // If form is closed without select being pressed, act as if nothing was selected
            if (cancelled == true)
            {
                try
                {
                    TCPSend(item);
                    TCPSend("No_Logs_Requested");
                }
                catch (InvalidDataException)
                {
                    return;
                }
            }
        }
    }
}
