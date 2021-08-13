using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        // Stores number of logs that matched criteria
        private int numLogs;
        // Used to tell mainForm if user exits without selecting anything
        private bool cancelled = true;

        public DownloadForm(string item, bool one, Func<string> TCPReceive, int numLogs, Action<string> TCPSend)
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(DownloadFormClosed);
            this.item = item;
            this.one = one;
            this.TCPReceive = TCPReceive;
            this.numLogs = numLogs;
            this.TCPSend = TCPSend;
        }

        // Show logs to user and allow them to select which ones to download
        // Objectives 4, 8.3 and 13.2
        private void DownloadForm_Load(object sender, EventArgs e)
        {
            lblDownload.Text = "Select " + item + " to Download";
            cmdDownload.Text = "Download " + item;

            // Add available logs to list

            for (int i = 0; i < numLogs; i++)
            {
                string[] response = TCPReceive().Split('\u001f');
                object[] rowData = new object[]
                {
                    false,
                    Convert.ToUInt16(response[0]),
                    response[1],
                    Convert.ToUInt16(response[2]),
                    response[3],
                    Convert.ToUInt16(response[4]),
                    Convert.ToUInt16(response[5]),
                    Convert.ToUInt16(response[6]),
                    response[7],
                    (response[8] == "None") ? 0 : Convert.ToUInt16(response[8])
                };
                dgvDownload.Rows.Add(rowData);
            }

            cmdDownload.Width = dgvDownload.Width;
        }

        // Sends the selected log ids to the logger
        private void cmdDownload_Click(object sender, EventArgs e)
        {
            this.cancelled = false;
            string logNames = "";
            int num = 0;

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
                MessageBox.Show("Please only select one item as only one can be downloaded in this instance.");
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

        void DownloadFormClosed(object sender, FormClosedEventArgs e)
        {
            // If form is closed without select being pressed, act as if nothing was selected
            if (cancelled == true)
            {
                TCPSend(item);
                TCPSend("No_Logs_Requested");
            }
        }
    }
}
