using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SteerLoggerUser
{
    public partial class DownloadForm : Form
    {
        // Store mainForm to use TCPSend and TCPReceive
        private mainForm main;
        // Stores logs available to download
        private List<LogMeta> logs;
        // Store item being downloaded (Config or Logs)
        private string item;
        // Determines whether user can select more that one log
        private bool one;
        // Used to tell mainForm if user exits without selecting anything
        private bool cancelled = true;

        public DownloadForm(mainForm mainForm, List<LogMeta> logsAvailable, string downloadItem, bool onlyOne)
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(DownloadFormClosed);
            main = mainForm;
            logs = logsAvailable;
            item = downloadItem;
            one = onlyOne;
        }

        // Show logs to user and allow them to select which ones to download
        // Objectives 4, 8.3 and 13.2
        private void DownloadForm_Load(object sender, EventArgs e)
        {
            lblDownload.Text = "Select " + item + " to Download";
            cmdDownload.Text = "Download " + item;

            Point pos = new Point(10, 5);
            // Enumerate through logs available
            foreach (LogMeta log in logs)
            {
                // Add new checkbox to form for log
                CheckBox tempCheckBox = new CheckBox();
                tempCheckBox.Location = pos;
                tempCheckBox.Text = "";
                tempCheckBox.Name = "ckb" + log.id;
                // Add new label to form for log
                Label tempLabel = new Label();
                tempLabel.AutoSize = true;
                tempLabel.Location = new Point(pos.X + 30, pos.Y + 5);
                tempLabel.Text = log.id + " " + log.name + " " + log.date;
                tempLabel.Name = "lbl" + log.name;
                panel.Controls.Add(tempLabel);
                panel.Controls.Add(tempCheckBox);
                // Increment position of checkboexs and labels being added
                pos.Y += 25;
            }

            cmdDownload.Width = panel.Width;
        }

        // Sends the selected log ids to the logger
        private void cmdDownload_Click(object sender, EventArgs e)
        {
            cancelled = false;
            string logNames = "";
            int num = 0;
            foreach (CheckBox checkBox in panel.Controls.OfType<CheckBox>())
            {
                // If the log's checkbox is selected, add its ID to logNames
                if (checkBox.Checked == true)
                {
                    logNames += checkBox.Name.Substring(3) + ",";
                    num += 1;
                }
            }
            // If no logs selected, let logger know
            if (logNames == "")
            {
                main.TCPSend(item);
                main.TCPSend("No_Logs_Requested");
            }
            // If more than one log selected when only one can be downloaded, alert user
            else if (one == true && num > 1)
            {
                MessageBox.Show("Please only select one item as only one can be downloaded in this instance.");
                return;
            }
            else
            {
                // Send the item being downloaded to the logger and then the IDs of the logs
                main.TCPSend(item);
                main.TCPSend(logNames.Substring(0, logNames.Length - 1));
            }
            this.Close();
        }

        void DownloadFormClosed(object sender, FormClosedEventArgs e)
        {
            // If form is closed without select being pressed, act as if nothing was selected
            if (cancelled == true)
            {
                main.TCPSend(item);
                main.TCPSend("No_Logs_Requested");
            }
        }
    }
}
