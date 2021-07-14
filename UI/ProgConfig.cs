using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteerLoggerUser
{
    public class ProgConfig
    {
        public Dictionary<int, string> units;
        public Dictionary<string, double[]> inputTypes;
        public Dictionary<int, double> gains;
        public Dictionary<int, string> loggers;

        public ProgConfig()
        {
            units = new Dictionary<int, string>();
            inputTypes = new Dictionary<string, double[]>();
            gains = new Dictionary<int, double>();
            loggers = new Dictionary<int, string>();
        }
    }
}
