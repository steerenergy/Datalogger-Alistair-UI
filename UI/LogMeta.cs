using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteerLoggerUser
{
    public class LogMeta
    {
        public int id;
        public int project;
        public int workPack;
        public int jobSheet;
        public string name;
        public int testNumber;
        public string date;
        public decimal time;
        public string loggedBy;
        public string downloadedBy;
        public ConfigFile config;
        //public LogData logData;
        public string raw;
        public string conv;
        public int size;
        public string description;
    }
}
