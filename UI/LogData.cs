using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteerLoggerUser
{
    public class LogData
    {
        public List<string> rawheaders = new List<string>();
        public List<string> convheaders = new List<string>();
        public List<DateTime> timestamp = new List<DateTime>();
        public List<double> time = new List<double>();
        public List<List<double>> rawData = new List<List<double>>();
        public List<List<double>> convData = new List<List<double>>();

        // Initialises rawData and convData lists by populating them with correct number of empty lists
        public void InitRawConv(int pinNum)
        {
            for (int i = 0; i < pinNum; i++)
            {
                this.rawData.Add(new List<double>());
                this.convData.Add(new List<double>());
            }
        }
        
        // Adds a list of values to the corresponding rawData columns
        public void AddRawData(List<double> values)
        {
            int i = 0;
            foreach (double value in values)
            {
                this.rawData[i].Add(value);
                i += 1;
            }
        }

        // Adds a list of values to the corresponding convData columns
        public void AddConvData(List<double> values)
        {
            int i = 0;
            foreach (double value in values)
            {
                this.convData[i].Add(value);
                i += 1;
            }
        }
    }
}
