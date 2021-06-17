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
        public List<decimal> time = new List<decimal>();
        public List<List<decimal>> rawData = new List<List<decimal>>();
        public List<List<decimal>> convData = new List<List<decimal>>();

        // Initialises rawData and convData lists by populating them with correct number of empty lists
        public void InitRawConv(int pinNum)
        {
            for (int i = 0; i < pinNum; i++)
            {
                this.rawData.Add(new List<decimal>());
                this.convData.Add(new List<decimal>());
            }
        }
        
        // Adds a list of values to the corresponding rawData columns
        public void AddRawData(List<decimal> values)
        {
            int i = 0;
            foreach (decimal value in values)
            {
                this.rawData[i].Add(value);
                i += 1;
            }
        }

        // Adds a list of values to the corresponding convData columns
        public void AddConvData(List<decimal> values)
        {
            int i = 0;
            foreach (decimal value in values)
            {
                this.convData[i].Add(value);
                i += 1;
            }
        }
    }
}
