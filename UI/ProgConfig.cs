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

        public ProgConfig()
        {
            units = new List<string>();
            inputTypes = new Dictionary<string, double[]>();
            gains = new Dictionary<int, double>();
            loggers = new List<string>();
        }
    }
}
