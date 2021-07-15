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
    public partial class ReceiveProgessFrom : Form
    {
        private mainForm main;
        private int maxNum;
        private int prev = 0;
        private DateTime start;

        public ReceiveProgessFrom(mainForm mainForm, int max)
        {
            this.main = mainForm;
            this.maxNum = 100;
            InitializeComponent();
        }

        private void ReceiveProgessFrom_Load(object sender, EventArgs e)
        {
            timer.Enabled = true;
            pbDownload.Maximum = maxNum;
            start = DateTime.Now;
        }

        public void UpdateProgressBar()
        {
            DateTime now = DateTime.Now;
            int value = main.pbValue;
            pbDownload.Value = value;
            if (value == maxNum)
            {
                this.Close();
            }
            if ((value - prev) != 0)
            {
                double timeSecs = now.Subtract(start).TotalSeconds / (main.pbValue) * (maxNum - main.pbValue);
                TimeSpan estimate = TimeSpan.FromSeconds(timeSecs);
                lblTime.Text = estimate.ToString("d'd 'h'h 'm'm 's's'");
            }
            prev = value;
            this.Update();
        }

        public void UpdateTextBox(string line)
        {
            txtOuput.Text += line + Environment.NewLine;
            this.Update();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (main.dataQueue.TryDequeue(out string line))
            {
                txtOuput.Text += line + Environment.NewLine;
                if (line == "Error occurred. Aborting!")
                {
                    this.Close();
                }
            }
            int value = main.pbValue;
            pbDownload.Value = value;
            if (value == maxNum)
            {
                this.Close();
            }
            if ((value - prev) != 0)
            {
                double timeSecs = 0.1 / (value - prev) * (maxNum - main.pbValue);
                TimeSpan estimate = TimeSpan.FromSeconds(timeSecs);
                lblTime.Text = estimate.ToString("d'd 'h'h 'm'm 's's'");
            }
            prev = value;
        }
    }
}
