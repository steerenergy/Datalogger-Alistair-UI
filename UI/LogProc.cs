using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SteerLoggerUser
{
    public class LogProc
    {
        public List<string> procheaders = new List<string>();
        public List<DateTime> timestamp = new List<DateTime>();
        public List<double> time = new List<double>();
        public List<List<double>> procData = new List<List<double>>();

        // Used to store the log data when it is being processed
        public LogProc()
        {
            this.procheaders = new List<string>();
            this.timestamp = new List<DateTime>();
            this.time = new List<double>();
            this.procData = new List<List<double>>();
        }

        // Sets the LogProc values to match the values for converted data of LogData
        public void CreateProcFromConv(string convPath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(convPath))
                {
                    // Read the headerline and set convheaders
                    this.procheaders = reader.ReadLine().Split(',').ToList();
                    // If there are less than 3 headers, the data file is likely incorrect
                    if (procheaders.Count < 3)
                    {
                        throw new InvalidDataException();
                    }
                    this.timestamp = new List<DateTime>();
                    this.time = new List<double>();
                    // Initialise procData using the number of headers
                    this.InitProc(this.procheaders.Count - 2);
                    while (!reader.EndOfStream)
                    {
                        // Read each line and store the data in the logData object
                        string[] line = reader.ReadLine().Split(',');
                        this.timestamp.Add(Convert.ToDateTime(line[0]));
                        this.time.Add(Convert.ToDouble(line[1]));
                        List<double> convData = new List<double>();
                        for (int i = 2; i < line.Length; i++)
                        {
                            convData.Add(double.Parse(line[i]));
                        }
                        this.AddProcData(convData);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw new InvalidDataException();
            }
        }

        // Initalises the procData list by populating with correct number of empty lists
        public void InitProc(int pinNum)
        {
            this.procData = new List<List<double>>();
            for (int i = 0; i < pinNum; i++)
            {
                this.procData.Add(new List<double>());
            }
        }
        
        // Adds a list of values to the corresponding procData columns
        public void AddProcData(List<double> values)
        {
            int i = 0;
            foreach (double value in values)
            {
                this.procData[i].Add(value);
                i += 1;
            }
        }
    }
}
