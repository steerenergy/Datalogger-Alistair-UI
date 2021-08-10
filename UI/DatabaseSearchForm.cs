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
            try
            {
                cancelled = false;
                // Send command to logger
                main.TCPSend("Search_Log");

                // Get variables from Form controls
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
                string values = name + '\u001f' + date + '\u001f' + loggedBy + '\u001f' + 
                                project + '\u001f' + workPack + '\u001f' + jobSheet;
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

        private void DatabaseSearchForm_Load(object sender, EventArgs e)
        {
            dtpDate.Enabled = false;
            nudProject.Enabled = false;
            nudWorkPack.Enabled = false;
            nudJobSheet.Enabled = false;
        }
    }
}
