using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace SteerLoggerUser
{
    public partial class ReceiveProgressForm : Form
    {
        private DateTime start;
        private BackgroundWorker worker;

        public ReceiveProgressForm(BackgroundWorker worker)
        {
            this.FormClosing += new FormClosingEventHandler(ProgressFormClosed);
            this.worker = worker;
            InitializeComponent();
        }

        private void ReceiveProgressForm_Load(object sender, EventArgs e)
        {
            // Start timer, setup progress bar and set the start time for time calculations
            timer.Enabled = true;
            pbDownload.Value = 0;
            pbDownload.Maximum = 100;
            start = DateTime.Now;
        }

        // Updates progress on form
        public void UpdateProgressBar(int value, string line)
        {
            // Update the progress bar
            pbDownload.Value = value;
            // Append any messages to the textbox
            txtOuput.AppendText(line + Environment.NewLine);
            if (line == "Error occurred, aborting!")
            {
                // If an error occurs, close
                this.Close();
            }
            // If the operation is complete, close form
            if (value == pbDownload.Maximum)
            {
                this.Close();
            }
        }

        // Calculate the time remaining when the timer ticks
        private void timer_Tick(object sender, EventArgs e)
        {
            int value = pbDownload.Value;
            if (value != 0)
            {
                // Get time remaining from time taken divided by work done mutlipled by work left to do 
                double timeSecs = (DateTime.Now.Subtract(start).TotalSeconds / value) * (pbDownload.Maximum - value);
                TimeSpan estimate = TimeSpan.FromSeconds(timeSecs);
                lblTime.Text = estimate.ToString("d'd 'h'h 'm'm 's's'");
            }
        }


        // Allow user to cancel operation and close form
        private void ProgressFormClosed(object sender, FormClosingEventArgs e)
        {
            if (pbDownload.Value != pbDownload.Maximum)
            {
                // Double check user wants to end download
                DialogResult dialogResult = MessageBox.Show("Downloading not finished, do you want to cancel download?", "Cancel Download?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    // If yes, cancel worker
                    worker.CancelAsync();
                }
                else if (dialogResult == DialogResult.No)
                {
                    // If no, cancel progress form close
                    e.Cancel = true;
                }
            }
        }
    }
}
