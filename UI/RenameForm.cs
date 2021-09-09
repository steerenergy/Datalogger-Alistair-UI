using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteerLoggerUser
{
    public partial class RenameForm : Form
    {
        public DownloadAndProcess DAP;
        public RenameForm(DownloadAndProcess DAP)
        {
            // Get download and process object form main form
            this.DAP = DAP;
            InitializeComponent();
        }

        private void RenameForm_Load(object sender, EventArgs e)
        {
            // Populate dgvRename with old log names and blank new name entries
            foreach(LogMeta log in DAP.logsProcessing)
            {
                dgvRename.Rows.Add(new object[] { log.name, log.testNumber, "", "" });
            }
        }

        private void cmdRename_Click(object sender, EventArgs e)
        {
            // Iterate through each row in datagrid and through each log being processed
            for (int i = 0; i < dgvRename.Rows.Count; i++)
            {
                // If user has input a new name, change name
                if (dgvRename.Rows[i].Cells[2].Value.ToString() != "")
                {
                    DAP.logsProcessing[i].name = dgvRename.Rows[i].Cells[2].Value.ToString();
                }
                // If user has input a new test number, change test number
                if (dgvRename.Rows[i].Cells[3].Value.ToString() != "")
                {
                    try
                    {
                        DAP.logsProcessing[i].testNumber = Convert.ToInt32(dgvRename.Rows[i].Cells[3].Value);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Cannot convert test number to integer. Please make sure all new test numbers are integers.",
                            "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            this.Close();
        }
    }
}
