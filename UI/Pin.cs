using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteerLoggerUser
{
    public class Pin
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        public string fName { get; set; }
        public string inputType { get; set; }
        public int gain { get; set; }
        public double scaleMin { get; set; }
        public double scaleMax { get; set; }
        public string units { get; set; }
        public double m { get; set; }
        public double c { get; set; }

        // Stores data about a single ads1115 pin
        // Used for storing config settings
        public Pin()
        {

        }
    }
}
