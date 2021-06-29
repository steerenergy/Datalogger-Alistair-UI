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
        public Dictionary<string, decimal[]> inputTypes;
        public Dictionary<int, decimal> gains;
        public Dictionary<int, string> loggers;

        public ProgConfig()
        {
            units = new Dictionary<int, string>();
            inputTypes = new Dictionary<string, decimal[]>();
            gains = new Dictionary<int, decimal>();
            loggers = new Dictionary<int, string>();
        }
    }
}
