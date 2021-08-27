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
        private void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                cancelled = false;
                // Send command to logger
                main.TCPSend("Search_Log");

                // Get variables from Form controls
                // If variable hasn't been set, set to ""
                string name = txtName.Text;
                string date = "";
                if (ckbDate.Checked == true)
                {
                    date = dtpDate.Value.ToString("yyyyMMdd");
                }
                string loggedBy = txtLoggedBy.Text;
                string project = "";
                if (ckbProject.Checked == true)
                {
                    project = nudProject.Value.ToString();
                }
                string workPack = "";
                if (ckbWorkPack.Checked == true)
                {
                    workPack = nudWorkPack.Value.ToString();
                }
                string jobSheet = "";
                if (ckbJobSheet.Checked == true)
                {
                    jobSheet = nudJobSheet.Value.ToString();
                }
                string description = txtDescription.Text;
                string notDownloaded = "";
                if (ckbNotDownloaded.Checked)
                {
                    notDownloaded = "true";
                }
                string values = name + '\u001f' + date + '\u001f' + loggedBy + '\u001f' + 
                                project + '\u001f' + workPack + '\u001f' + jobSheet + 
                                '\u001f' + description + '\u001f' + notDownloaded;
                // Send values to logger
                main.TCPSend(values);
            }
            catch (Exception exp)
            {
                cancelled = true;
                throw exp;
            }
            this.Close();
        }


        // Enable or disable nudProject depending on ckbProject checkstate
        private void ckbProject_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbProject.Checked)
            {
                nudProject.Enabled = true;
            }
            else
            {
                nudProject.Enabled = false;
            }
        }

        // Enable or disable nudWorkPack depending on ckbWorkPack checkstate
        private void ckbWorkPack_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbWorkPack.Checked)
            {
                nudWorkPack.Enabled = true;
            }
            else
            {
                nudWorkPack.Enabled = false;
            }
        }

        // Enable or disable nudJobSheet depending on ckbJobSheet checkstate
        private void ckbJobSheet_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbJobSheet.Checked)
            {
                nudJobSheet.Enabled = true;
            }
            else
            {
                nudJobSheet.Enabled = false;
            }
        }

        // Enable or disable dtpDate depending on ckbDate checkstate
        private void ckbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbDate.Checked)
            {
                dtpDate.Enabled = true;
            }
            else
            {
                dtpDate.Enabled = false;
            }
        }

        // When form loads, set controls to disabled and set ckbNotDownloaded text to reflect user
        private void DatabaseSearchForm_Load(object sender, EventArgs e)
        {
            dtpDate.Enabled = false;
            nudProject.Enabled = false;
            nudWorkPack.Enabled = false;
            nudJobSheet.Enabled = false;
            ckbNotDownloaded.Text = String.Format("Not Downloaded by {0}", main.user);
        }
    }
}
