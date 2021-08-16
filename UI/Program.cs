using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteerLoggerUser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //try
            //{
                Application.Run(new mainForm());
            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show(String.Format("Error occured: {0}",exp.Message),"Error!");
            //    MessageBox.Show(String.Format("Full error: {0}",exp.ToString()));
            //}
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
