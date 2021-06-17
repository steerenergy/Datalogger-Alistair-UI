using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace SteerLoggerUser
{
    class DownloadAndProcess
    {
        // Stores logs waiting to be processed
        public Queue<LogMeta> logsToProc = new Queue<LogMeta>();
        // Stores logs currently being processed
        public List<LogMeta> logsProcessing = new List<LogMeta>();
        // Stores processed data displayed in the DataProc grid
        public LogProc logProc = new LogProc();
        // Stores whether a log is being processed or not
        public bool processing = false;

        // Used to merge two logs together
        // Objective 17
        public void MergeLogs(LogProc logToMerge)
        {
            // Create new LogProc object to store merged logs
            LogProc tempProcessLog = new LogProc();
            // Counters is used to store the index of each log
            int[] counters = { 0, 0 };

            // Create headers from current log and imported log
            tempProcessLog.procheaders = logProc.procheaders;
            tempProcessLog.procheaders.AddRange(logToMerge.procheaders.Skip(2));

            // Initialise the procData using the number of columns of logs being merged
            int columnNum = logProc.procData.Count();
            columnNum += logToMerge.procData.Count();
            tempProcessLog.InitProc(columnNum);

            // Set earliest common time as start time
            DateTime start = RoundDateTime(logProc.timestamp[0]);
            if (RoundDateTime(logToMerge.timestamp[0]) > start)
            {
                start = RoundDateTime(logToMerge.timestamp[0]);
            }

            // Set latest common time as finish time
            DateTime finish = RoundDateTime(logProc.timestamp.Last());
            if (RoundDateTime(logToMerge.timestamp.Last()) < finish)
            {
                finish = RoundDateTime(logToMerge.timestamp.Last());
            }

            // largest is used to store the current timestamp being compared between logs
            DateTime largest = start;
            // While the finish time hasn't been reached, find common times and add them
            while (largest < finish)
            {
                // The section below makes sure data is only added to the merged log
                // when a timestamp is common in both logs being merged
                // Objective 17.3
                bool equal = true;
                // Compare timestamp at current position of log 1 to largest
                if (RoundDateTime(logProc.timestamp[counters[0]]) < largest)
                {
                    // If it is less than largest, increment the current position of log 1 by 1
                    counters[0] += 1;
                    equal = false;
                }
                else if (RoundDateTime(logProc.timestamp[counters[0]]) > largest)
                {
                    // If it is greater, do not increment and set largest to the larger timestamp
                    largest = RoundDateTime(logProc.timestamp[counters[0]]);
                    equal = false;
                }

                // Compare timestamp at current position of log 2 to largest
                if (RoundDateTime(logToMerge.timestamp[counters[1]]) < largest)
                {
                    // If it is less than largest, increment the current position of log 2 by 1
                    counters[1] += 1;
                    equal = false;
                }
                else if (RoundDateTime(logToMerge.timestamp[counters[1]]) > largest)
                {
                    // If it is greater, do not increment and set largest to the larger timestamp
                    largest = RoundDateTime(logToMerge.timestamp[counters[1]]);
                    equal = false;
                }

                // If log 1 current timestamp = log 2 current timestamp = largest, add data to merged LogProc
                // Objective 17.3
                if (equal)
                {
                    decimal timeDifference = 0;
                    if (tempProcessLog.timestamp.Count != 0)
                    {
                        // Calute time between start timestamp and largest timestamp
                        timeDifference = Convert.ToDecimal((largest - tempProcessLog.timestamp[0]).TotalSeconds);
                    }
                    // Add timestamp and time to merged log
                    tempProcessLog.timestamp.Add(largest);
                    tempProcessLog.time.Add(timeDifference);

                    // Add data for that row from log 1 and log 2
                    List<decimal> rowdata = new List<decimal>();
                    foreach (List<decimal> column in logProc.procData)
                    {
                        rowdata.Add(column[counters[0]]);
                    }
                    foreach (List<decimal> column in logToMerge.procData)
                    {
                        rowdata.Add(column[counters[1]]);
                    }
                    tempProcessLog.AddProcData(rowdata);
                    // Increment both counters by 1
                    for (int i = 0; i < counters.Count(); i++)
                    {
                        counters[i] += 1;
                    }
                }
            }
            // If no common timestamps are found, do not merge and return
            if (tempProcessLog.timestamp.Count == 0)
            {
                MessageBox.Show("No common timestamps found.");
                return;
            }
            // Set current logProc to the merged logs
            this.logProc = tempProcessLog;
        }

        // Used to round a date time object to the nearest 0.1 seconds (lowest time interval)
        // Means logs can be merged without being in sync to the millisecond
        private DateTime RoundDateTime(DateTime input)
        {
            int millisecond = input.Millisecond;
            // Round millisecond by dividing by 100, rounding, then multiplying by 100
            millisecond = Convert.ToInt32(Math.Round(millisecond / 100M) * 100);
            // If the millisecond is rounded to be greater than 1 second
            // Set millisecond to 0 and add 1 second to the timestamp
            if (millisecond >= 1000)
            {
                millisecond = 0;
                input = input.AddSeconds(1);
            }
            // Return rounded timestamp
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second, millisecond);
        }
    }
}
