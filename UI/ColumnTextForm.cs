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
    public partial class ColumnTextForm : Form
    {
        public List<string> headers;
        public ColumnTextForm(List<string> headers)
        {
            // Get current headers
            this.headers = headers;
            InitializeComponent();
        }

        private void ColumnTextForm_Load(object sender, EventArgs e)
        {
            // Populate datagridview with current headers
            DataTable table = new DataTable();
            table.Columns.AddRange(new DataColumn[] { new DataColumn("Old Name",typeof(string)), new DataColumn("New Name",typeof(string)) });
            foreach (string header in headers)
            {
                table.Rows.Add(new object[] { header, "" });
            }
            dgvHeaders.DataSource = table;
            dgvHeaders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Iterate through current headers
            for (int i = 0; i < headers.Count; i++)
            {
                // If grid has a new value input, update header to new value
                if (dgvHeaders.Rows[i].Cells[1].Value.ToString() != "")
                {
                    headers[i] = dgvHeaders.Rows[i].Cells[1].Value.ToString();
                }
            }
            this.Close();
        }
    }
}
