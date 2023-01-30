using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockLivelock.Utils
{
    public class Logger
    {
        private static StringBuilder _logs
            = new StringBuilder();

        public static string CurrentLogs
        {
            get { return _logs.ToString(); }
        }

        public static void Log(string logData, bool newLine = true)
        {
            if (newLine)
                _logs.AppendLine(logData);
            else
                _logs.Append(logData);
        }

        public static void DeleteLogs() => _logs.Clear();
    }
}
