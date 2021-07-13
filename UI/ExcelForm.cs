using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;

namespace SteerLoggerUser
{
    public partial class ExcelForm : Form
    {
        // Stores data to export to Excel
        private LogProc logProc;
        // Stores a queue of Excel formulas that user has added
        private Queue<string[]> formulaQueue = new Queue<string[]>();

        public ExcelForm(LogProc logToExp)
        {
            InitializeComponent();
            logProc = logToExp;
        }

        // Populate graph drop down menus with column headers
        private void ExcelForm_Load(object sender, EventArgs e)
        {
            foreach (string column in logProc.procheaders)
            {
                cmbXAxis.Items.Add(column);
                cmbYAxis.Items.Add(column);
            }
            cmbXAxis.SelectedIndex = 1;
            cmbYAxis.SelectedIndex = 0;
        }

        // Exports data to Excel
        // Objective 14.3
        private void cmdExport_Click(object sender, EventArgs e)
        {
            try 
            {
                // Open Excel and create a new workbook and worksheet
                Excel.Application excel = new Excel.Application();
                Excel._Workbook excelWb = (Excel._Workbook)(excel.Workbooks.Add(Missing.Value));
                Excel._Worksheet excelSheet = (Excel._Worksheet)excelWb.ActiveSheet;

                // Write the column headers to the first row on the sheet
                for (int i = 0; i < logProc.procheaders.Count; i++)
                {
                    excelSheet.Cells[1, i + 1] = logProc.procheaders[i];
                }
                
                // Change the format of the timestamp column to display timestamps nicely
                Excel.Range range = excelSheet.get_Range("A2", "A" + (logProc.timestamp.Count + 1));
                range.NumberFormat = "yyyy/mm/dd hh:mm:ss.000";

                // Enumerate through data and add each row to the spreadsheet
                for (int i = 0; i < logProc.timestamp.Count; i++)
                {

                    excelSheet.Cells[i + 2, 1] = logProc.timestamp[i].ToString("yyyy/MM/dd HH:mm:ss.fff");
                    excelSheet.Cells[i + 2, 2] = logProc.time[i];
                    for (int j = 0; j < logProc.procData.Count; j++)
                    {
                        excelSheet.Cells[i + 2, j + 3] = logProc.procData[j][i];
                    }
                }

                // Store the next available column for writing formula columns
                int availableCol = logProc.procheaders.Count;
                // Enumerate through formulaQueue and create a column for each formula used
                // Objective 14.3.1
                while (formulaQueue.Count > 0)
                {
                    string[] formulaCol = formulaQueue.Dequeue();
                    // Get cells in the column
                    string[] rangeCells = {GetColumn(availableCol) + "2",
                                           GetColumn(availableCol) + (logProc.timestamp.Count + 1)};
                    Excel.Range colRange = excelSheet.get_Range(rangeCells[0], rangeCells[1]);
                    // Set formula of the cells to the user formula
                    colRange.Formula = formulaCol[0];
                    // Set the title of the column to the user title
                    excelSheet.Cells[1, availableCol + 1] = formulaCol[1];
                    // Increment the available column
                    availableCol += 1;
                }

                // Autofit all the columns so sheet looks nicer
                range = excelSheet.get_Range("A1", GetColumn(availableCol) + "1");
                range.EntireColumn.AutoFit();

                // If user has selected to create graph, create one
                if (ckbCreateGraph.Checked == true)
                {
                    CreateGraph(excelSheet, excelWb);
                }

                // Allow user to see spreadsheet and control it
                excel.UserControl = true;
                excel.Visible = true;

                MessageBox.Show("Exported Successfully!");
                this.Close();
            }
            // Catch any exceptions and report to user
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
                MessageBox.Show(theException.ToString());
            }
        }

        // Creates a graph from two of the columns
        // Objective 14.3.2
        private void CreateGraph(Excel._Worksheet excelSheet, Excel._Workbook excelWb)
        {
            // Create new Excel chart        
            Excel._Chart chart = (Excel._Chart)excelWb.Charts.Add(Missing.Value, Missing.Value,
            Missing.Value, Missing.Value);

            // Get cell range for X axis column
            int X = cmbXAxis.SelectedIndex;
            string[] Xrange = { (GetColumn(X) + "2"), GetColumn(X) + (logProc.timestamp.Count + 1) };

            // Get cell range for Y axis column
            int Y = cmbYAxis.SelectedIndex;
            string[] Yrange = { (GetColumn(Y) + "2"), GetColumn(Y) + (logProc.timestamp.Count + 1) };

            // Select column for Y axis
            Excel.Range dataRange = excelSheet.get_Range(Yrange[0], Yrange[1]);
            // Create line graph from Y axis column
            chart.ChartWizard(dataRange,
                Excel.XlChartType.xlLine, 
                2,
                Excel.XlRowCol.xlColumns, 
                0, 
                0, 
                false,
                txtTitle.Text, 
                cmbXAxis.Text, 
                cmbYAxis.Text, 
                "");

            // Select X axis values and add to chart
            Excel.Series series = (Excel.Series)chart.SeriesCollection(1);
            series.XValues = excelSheet.get_Range(Xrange[0],Xrange[1]);

            // Place chart on sheet
            chart.Location(Excel.XlChartLocation.xlLocationAsObject, excelSheet.Name);
            
            //Move the chart so as not to cover your data.
            dataRange = (Excel.Range)excelSheet.Rows.get_Item(4, Missing.Value);
            excelSheet.Shapes.Item("Chart 1").Top = (float)(double)dataRange.Top;
            dataRange = (Excel.Range)excelSheet.Columns.get_Item(cmbXAxis.Items.Count + 2, Missing.Value);
            excelSheet.Shapes.Item("Chart 1").Left = (float)(double)dataRange.Left;
        }

        // Used to get an Excel column name from a number
        private string GetColumn(int colNum)
        {
            int dividend = colNum + 1;
            string colName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                colName = Convert.ToChar(65 + modulo).ToString() + colName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return colName;
        }

        // Displays help to user for creating formula columns
        private void cmdHelp_Click(object sender, EventArgs e)
        {
            string message = "To append a column with an Excel formula, write the formula " +
                             "in the box on this form as you would in Excel then click add formula column." + Environment.NewLine +
                             Environment.NewLine + "To reference a data column in the formula, use the list below: " + Environment.NewLine;
            int colNum = 0;
            foreach (string procheader in logProc.procheaders)
            {
                message += procheader + ": " + GetColumn(colNum) + Environment.NewLine;
                colNum += 1;
            }

            MessageBox.Show(message);
        }

        // Adds a formula to the formula queue
        // Objective 14.3.1
        private void cmdAddFormula_Click(object sender, EventArgs e)
        {
            // Make sure user has written a formula and it is written correctly
            if (txtFormula.Text == "")
            {
                MessageBox.Show("Please write a formula into the formula box.");
                return;
            }
            else if (txtFormula.Text[0] != '=')
            {
                MessageBox.Show("Please make sure your formula starts with an \'=\' sign.");
                return;
            }

            // Add formula to queue
            formulaQueue.Enqueue(new string[] { txtFormula.Text, txtColTitle.Text});
            // Allow formula column to be used for graphing
            cmbXAxis.Items.Add(txtColTitle.Text);
            cmbYAxis.Items.Add(txtColTitle.Text);
            MessageBox.Show("Column added successfully!");
            // Reset formula controls
            txtFormula.Text = "";
            txtColTitle.Text = "";
        }
    }
}
