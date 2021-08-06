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

        public ReceiveProgressForm()
        {
            InitializeComponent();
        }

        private void ReceiveProgessFrom_Load(object sender, EventArgs e)
        {
            timer.Enabled = true;
            pbDownload.Maximum = 100;
            start = DateTime.Now;
        }

        public void UpdateProgressBar(int value, string line)
        {
            pbDownload.Value = value;
            txtOuput.AppendText(line + Environment.NewLine);
            if (line == "Error occurred. Aborting!")
            {
                this.Close();
            }
            if (value == pbDownload.Maximum)
            {
                this.Close();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int value = pbDownload.Value;
            if (value != 0)
            {
                double timeSecs = (DateTime.Now.Subtract(start).TotalSeconds / value) * (pbDownload.Maximum - value);
                TimeSpan estimate = TimeSpan.FromSeconds(timeSecs);
                lblTime.Text = estimate.ToString("d'd 'h'h 'm'm 's's'");
            }
        }
    }
}
