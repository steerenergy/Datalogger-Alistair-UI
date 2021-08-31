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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void lnkManual_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkManual.LinkVisited = true;
            System.Diagnostics.Process.Start(lnkManual.Text);
        }

        private void lnkIssues_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkIssues.LinkVisited = true;
            System.Diagnostics.Process.Start(lnkIssues.Text);
        }

        private void lnkTechDoc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkTechDoc.LinkVisited = true;
            System.Diagnostics.Process.Start(lnkTechDoc.Text);
        }
    }
}
