using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface ILogger
    {
        TrackInfo TrackInfo
        {
            get;
            set;
        }
        void Log(string message);
        void Log(string message, LogType logType);
        void Log(string message, LogType logType, Exception exception);
        void Log(string logName, string message);
        void Log(string logName, string message, LogType logType);
        void Log(string logName, string message, LogType logType, Exception exception);
    }
}
