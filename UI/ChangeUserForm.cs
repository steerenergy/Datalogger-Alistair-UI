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
        private bool cancelled = true;

        public ChangeUserForm(Action<string> tcpSend)
        {
            this.TcpSend = tcpSend;
            this.FormClosed += new FormClosedEventHandler(UserFormClosed);
            InitializeComponent();

        }


        // Send new username to logger
        private void cmdChangeUser_Click(object sender, EventArgs e)
        {
            // If username is empty, check that user wants to set no username
            if (txtUser.Text == "")
            {
                DialogResult result = MessageBox.Show("No username input, do you want to continue with no username?"
                                                      ,"No Input",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            user = txtUser.Text;
            this.TcpSend(user);
            cancelled = false;
            this.Close();
        }


        // If form is closed without selecting ChangeUser, tell logger it was closed and not to change username
        private void UserFormClosed(object sender, FormClosedEventArgs e)
        {
            if (cancelled == true)
            {
                this.TcpSend("Closed");
            }
        }
    }
}
