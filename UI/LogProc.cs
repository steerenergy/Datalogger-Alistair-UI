using System;
using System.Collections.Generic;
using System.Linq;
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


        // Sets the LogProc values to match the values for converted data of LogData
        public void CreateProcFromConv(LogData logData)
        {
            this.procheaders = logData.convheaders;
            this.timestamp = logData.timestamp;
            this.time = logData.time;
            this.procData = logData.convData;
        }

        // Initalises the procData list by populating with correct number of empty lists
        public void InitProc(int pinNum)
        {
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
