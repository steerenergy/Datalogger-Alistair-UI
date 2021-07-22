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
        public string name;
        public string date;
        public decimal time;
        public string loggedBy;
        public string downloadedBy;
        public ConfigFile config;
        public LogData logData;
        public int size;
        public string description;
    }
}
