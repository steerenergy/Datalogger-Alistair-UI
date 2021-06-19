using System;
using System.Windows.Forms;

namespace SteerLoggerUser
{
    public partial class DatabaseSearchForm : Form
    {
        // Store mainForm to use TCPSend and TCPReceive
        private mainForm main;
        public bool cancelled = true;
        public DatabaseSearchForm(mainForm mainForm)
        {
            InitializeComponent();
            main = mainForm;
        }

        // Sends search criteria to logger so database can be searched
        // Objectives 8.2 and 13.1
        private void cmdSearch_Click(object sender, EventArgs e)
        {
            cancelled = false;
            // Send command to logger
            main.TCPSend("Search_Log");
            
            // Get variables from Form controls
            string name = txtName.Text;
            string date = dtpDate.Value.ToString("yyyyMMdd");
            if (cmbIgnoreDate.Checked == true)
            {
                date = "";
            }
            string loggedBy = txtLoggedBy.Text;
            string values = name + ',' + date  + ',' + loggedBy;
            // Send values to logger
            main.TCPSend(values);
            this.Close();
        }
    }
}
