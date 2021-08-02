using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SteerLoggerUser
{
    public partial class ExcelForm : Form
    {

        // TO-DO close workbook nicely on crash
        
        // Global variables used to interface with Excel
        private Excel.Application excel;
        private Excel._Workbook workbook;

        // Stores data to export to Excel
        private LogProc logProc;
        // Stores a queue of Excel formulas that user has added
        private Queue<string[]> formulaQueue = new Queue<string[]>();
        // Stores path of template workbook
        private string path = "";
        // Stores whether using template
        private bool template = false;
        // Stores arrays of data to export to template
        private Dictionary<int[],string[]> exportingArrays = new Dictionary<int[],string[]>();

        public ExcelForm(LogProc logToExp, Excel.Application excelApp)
        {
            InitializeComponent();
            logProc = logToExp;
            excel = excelApp;
        }

        // Populate graph drop down menus with column headers
        private void ExcelForm_Load(object sender, EventArgs e)
        {
            if (excel == null)
            {
                excel = new Excel.Application();
            }
            dgvTemplate.SelectionChanged += new EventHandler(dgvTemplate_SelectionChanged);
            this.FormClosed += new FormClosedEventHandler(ExcelFormClosed);

            pnlTemplate.Hide();
            pnlExportNew.Show();
            template = false;
            ofdTemplate.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            foreach (string column in logProc.procheaders)
            {
                cmbXAxis.Items.Add(column);
                cmbYAxis.Items.Add(column);
                cmbLogCols.Items.Add(column);
            }
            cmbXAxis.SelectedIndex = 1;
            cmbYAxis.SelectedIndex = 0;
            cmbLogCols.SelectedIndex = 0;
            cmbTemplate.Items.Add("No workbook loaded");
            cmbTemplate.SelectedIndex = 0;
        }

        // Exports data to Excel
        // Objective 14.3
        private void cmdExport_Click(object sender, EventArgs e)
        {
            if (template == true)
            {
                ExportTemplate();
            }
            else
            {
                ExportNew();
            }
        }


        // Export data to Excel by creating new workbook
        private void ExportNew()
        {
            try
            {
                // Open Excel and create a new workbook and worksheet
                workbook = (Excel._Workbook)(excel.Workbooks.Add(Missing.Value));
                Excel._Worksheet excelSheet = (Excel._Worksheet)workbook.ActiveSheet;

                // Write the column headers to the first row on the sheet
                for (int i = 0; i < logProc.procheaders.Count; i++)
                {
                    excelSheet.Cells[1, i + 1] = logProc.procheaders[i];
                }

                // Change the format of the timestamp column to display timestamps nicely
                Excel.Range range = excelSheet.get_Range("A2", "A" + (logProc.timestamp.Count + 1));
                range.NumberFormat = "yyyy/mm/dd hh:mm:ss.000";


                Excel.Range valueRange = excelSheet.get_Range("A2", GetColumn(logProc.procheaders.Count - 1) + (logProc.timestamp.Count + 1));
                object[,] values = valueRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);
                // Enumerate through data and add each row to the spreadsheet
                for (int i = 0; i < logProc.timestamp.Count; i++)
                {

                    //excelSheet.Cells[i + 2, 1] = logProc.timestamp[i].ToString("yyyy/MM/dd HH:mm:ss.fff");
                    //excelSheet.Cells[i + 2, 2] = logProc.time[i];
                    //for (int j = 0; j < logProc.procData.Count; j++)
                    //{
                    //   excelSheet.Cells[i + 2, j + 3] = logProc.procData[j][i];
                    //}
                    values[i + 1, 1] = logProc.timestamp[i].ToString("yyyy/MM/dd HH:mm:ss.fff");
                    values[i + 1, 2] = logProc.time[i];
                    for (int j = 0; j < logProc.procData.Count; j++)
                    {
                        values[i + 1, j + 3] = logProc.procData[j][i];
                    }
                }
                valueRange.Value = values;

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
                    CreateGraph(excelSheet, workbook);
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
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
            }  
        }

        // Creates a graph from two of the columns
        // Objective 14.3.2
        private void CreateGraph(Excel._Worksheet excelSheet, Excel._Workbook workbook)
        {
            // Create new Excel chart        
            Excel._Chart chart = (Excel._Chart)workbook.Charts.Add(Missing.Value, Missing.Value,
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

        private void cmdCreateWb_Click(object sender, EventArgs e)
        {
            pnlTemplate.Hide();
            pnlExportNew.Show();
            template = false;
        }

        private void cmdUseTemplate_Click(object sender, EventArgs e)
        {
            pnlExportNew.Hide();
            pnlTemplate.Show();
            template = true;
        }

        private void cmdOpenTemplate_Click(object sender, EventArgs e)
        {
            if (ofdTemplate.ShowDialog() == DialogResult.OK)
            {
                path = ofdTemplate.FileName;
                if (Path.GetExtension(path) != ".xlsx")
                {
                    MessageBox.Show("File input doesn't appear to be an Excel file. Please double check file extension");
                }
            }
            else
            {
                return;
            }

            try
            {
                workbook = (Excel._Workbook)(excel.Workbooks.Open(path));
                Excel._Worksheet template = new Excel.Worksheet();
                cmbTemplate.Items.Clear();
                // Enumerate sheets in workbook
                foreach (Excel._Worksheet worksheet in workbook.Sheets)
                {
                    cmbTemplate.Items.Add(worksheet.Name);
                }
                cmbTemplate.SelectedIndex = 0;
            }
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
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
        }

        private void dgvTemplate_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Update the txtCellSelected to reflect changes to the selection.
                txtSelectedCell.Text = "Selected cell: "
                                        + GetColumn(dgvTemplate.SelectedCells[0].ColumnIndex)
                                        + (dgvTemplate.SelectedCells[0].RowIndex + 1).ToString();
            }
            catch
            {
                txtSelectedCell.Text = "Selected cell:";
            }
            
        }

        private void cmdWriteCol_Click(object sender, EventArgs e)
        {
            int col = dgvTemplate.SelectedCells[0].ColumnIndex;
            int row = dgvTemplate.SelectedCells[0].RowIndex;

            int logCol = 0;
            int i = 0;
            foreach (string header in logProc.procheaders)
            {
                if (header == cmbLogCols.SelectedItem.ToString())
                {
                    logCol = i; 
                }
                i++;
            }

            dgvTemplate.Rows[row].Cells[col].Value = logProc.procheaders[logCol];


            // Gets array of data depending on column selected
            string[] data;
            if (logCol == 0)
            {
                data = Array.ConvertAll(logProc.timestamp.ToArray(), x => x.ToString("yyyy/MM/dd HH:mm:ss.fff"));
            }
            else if (logCol == 1)
            {
                data = Array.ConvertAll(logProc.time.ToArray(), x => x.ToString());
            }
            else
            {
                data = Array.ConvertAll(logProc.procData[logCol - 2].ToArray(), x => x.ToString());
            }

            int[] cell = { dgvTemplate.SelectedCells[0].ColumnIndex, dgvTemplate.SelectedCells[0].RowIndex };
            string[] name = { logProc.procheaders[logCol] };
            exportingArrays.Add(cell, name.Concat(data).ToArray());

            row++;
            // Adds extra rows if needed
            while (data.Length > dgvTemplate.Rows.Count - row)
            {
                i = dgvTemplate.Rows.Count;
                dgvTemplate.Rows.Add(new DataGridViewRow());
                dgvTemplate.Rows[i].HeaderCell.Value = i.ToString();
            }

            // Writes data to dgvTemplate
            for (i = row; i < logProc.timestamp.Count + row; i++)
            {
                dgvTemplate.Rows[i].Cells[col].Value = data[i - row];
            }
        }


        private void ExportTemplate()
        {
            string name = txtSheetName.Text;
            if (name == "")
            {
                MessageBox.Show("Please input a name for the new sheet.");
                return;
            }
            try
            {
                workbook = (Excel._Workbook)(excel.Workbooks.Open(path));
                Excel._Worksheet template = new Excel.Worksheet();
                // Enumerate sheets in workbook
                foreach (Excel._Worksheet worksheet in workbook.Sheets)
                {
                    if (worksheet.Name == cmbTemplate.SelectedItem.ToString())
                    {
                        template = worksheet;
                    }

                    if (worksheet.Name == name)
                    {
                        MessageBox.Show("There is already a sheet with that name.");
                        workbook.Close();
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbook);
                        //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
                        return;
                    }
                }

                // Create copy of template sheet
                Excel.Range excelRange = template.UsedRange;
                template.Copy(template);

                // Rename template sheet to user defined name
                Excel._Worksheet excelSheet = workbook.ActiveSheet;
                excelSheet.Name = name;

                foreach (int[] key in exportingArrays.Keys)
                {
                    Excel.Range valueRange = excelSheet.get_Range(GetColumn(key[0]) + (key[1] + 1).ToString(), GetColumn(key[0]) + (exportingArrays[key].Length + key[1] + 1).ToString());
                    object[,] values = valueRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);
                    // Enumerate through data and add each row to the spreadsheet
                    for (int i = 0; i < exportingArrays[key].Length; i++)
                    {
                        values[i + 1, 1] = exportingArrays[key][i];
                    }
                    valueRange.Value = values;
                }
                
                excel.Visible = true;
                excel.UserControl = true;

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
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
            }
        }

        private void cmdLoadTemplate_Click(object sender, EventArgs e)
        {
            // Read data into dgvtemplate
            try
            {
                workbook = (Excel._Workbook)(excel.Workbooks.Open(path));
                Excel._Worksheet template = new Excel.Worksheet();
                // Enumerate sheets in workbook
                foreach (Excel._Worksheet worksheet in workbook.Sheets)
                {
                    if (worksheet.Name == cmbTemplate.SelectedItem.ToString())
                    {
                        template = worksheet;
                    }
                }

                if (template == new Excel.Worksheet())
                {
                    MessageBox.Show("Cannot find a sheet called " + cmbTemplate.SelectedItem.ToString() 
                                 +  "  in workbook, make sure there is a template sheet called Template.");
                }

                // Create copy of template sheet
                Excel.Range excelRange = template.UsedRange;
                object[,] valueArray = excelRange.get_Value(Excel.XlRangeValueDataType.xlRangeValueDefault);

                dgvTemplate.Rows.Clear();
                dgvTemplate.Columns.Clear();

                for (int i = 1; i <= valueArray.GetLength(1); i++)
                {
                    DataGridViewColumn tempColumn = new DataGridViewColumn();
                    tempColumn.Name = GetColumn(i - 1);
                    tempColumn.HeaderText = GetColumn(i - 1);
                    tempColumn.CellTemplate = new DataGridViewTextBoxCell();
                    dgvTemplate.Columns.Add(tempColumn);
                }

                for (int i = 1; i <= valueArray.GetLength(0); i++)
                {
                    List<string> rowData = new List<string>();
                    for (int j = 1; j <= valueArray.GetLength(1); j++)
                    {
                        rowData.Add((valueArray[i, j] == null) ? "" : valueArray[i, j].ToString());
                    }
                    dgvTemplate.Rows.Add(rowData.ToArray());
                    dgvTemplate.Rows[i - 1].HeaderCell.Value = i.ToString();
                }

                workbook.Close();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbook);
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
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
            //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
        }

        private void ExcelFormClosed(object sender, FormClosedEventArgs e)
        {
            // Close instance of Excel application
            //try
            //{
            //    workbook.Close(true);
            //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbook);
            //    excel.Quit();
            //   System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
            //}
            //catch (System.Runtime.InteropServices.COMException)
            //{
            //    // Excel closed before form
            //}
        }
    }
}
