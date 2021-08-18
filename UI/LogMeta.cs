using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteerLoggerUser
{
    public class LogMeta
    {
        public int id { get; set; }
        public int project { get; set; }
        public int workPack { get; set; }
        public int jobSheet { get; set; }
        public string name { get; set; }
        public int testNumber { get; set; }
        public string date { get; set; } 
        public decimal time { get; set; } 
        public string loggedBy { get; set; } 
        public string downloadedBy { get; set; }
        public Pin[] config { get; set; }
        public string raw { get; set; }
        public string conv { get; set; }
        public int size { get; set; }
        public string description { get; set; }

        // Stores all metdata surrounding a log
        public LogMeta()
        {
        }
    }
}
