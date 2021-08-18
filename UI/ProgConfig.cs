using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteerLoggerUser
{
    public class ProgConfig
    {
        public string activatePath;
        public List<string> units;
        public Dictionary<string, double[]> inputTypes;
        public Dictionary<int, double> gains;
        public List<string> loggers;
        public Dictionary<string,Pin> configPins;
        public Dictionary<string, List<string>> variationDict;

        // Stores program config settings
        public ProgConfig()
        {
            this.units = new List<string>();
            this.inputTypes = new Dictionary<string, double[]>();
            this.gains = new Dictionary<int, double>();
            this.loggers = new List<string>();
            this.configPins = new Dictionary<string, Pin>();
            this.variationDict = new Dictionary<string, List<string>>();
        }
    }
}
