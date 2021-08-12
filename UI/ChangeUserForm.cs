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
    public partial class ChangeUserForm : Form
    {
        public string user;
        private Action<string> TcpSend;
        public ChangeUserForm(Action<string> tcpSend)
        {
            this.TcpSend = tcpSend;
            InitializeComponent();
        }

        private void cmdChangeUser_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                DialogResult result = MessageBox.Show("No username input, do you want to continue with no username?"
                                                      ,"No Input",MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            user = txtUser.Text;
            this.TcpSend(user);
            this.Close();
        }
    }
}
