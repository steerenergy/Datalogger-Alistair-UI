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
        public ConfigFile config { get; set; }
        public string raw { get; set; }
        public string conv { get; set; }
        public int size { get; set; }
        public string description { get; set; }

        public LogMeta()
        {
        }

        public LogMeta(int id, 
                       int project, 
                       int workpack,
                       int jobSheet,
                       string name,
                       int testNumber,
                       string date,
                       decimal time,
                       string loggedBy,
                       string downloadedBy,
                       string raw,
                       string conv,
                       int size,
                       string description)
        {
            this.id = id;
            this.project = project;
            this.workPack = workpack;
            this.jobSheet = jobSheet;
            this.name = name;
            this.testNumber = testNumber;
            this.date = date;
            this.time = time;
            this.loggedBy = loggedBy;
            this.downloadedBy = downloadedBy;
            this.raw = raw;
            this.conv = conv;
            this.size = size;
            this.description = description;
        }
    }
}
