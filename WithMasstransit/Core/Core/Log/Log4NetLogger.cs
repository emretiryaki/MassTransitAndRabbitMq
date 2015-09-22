using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Core
{
    public class Log4NetLogger : ILogger
    {
        public TrackInfo TrackInfo
        {
            get;
            set;
        }
        public static void Init(string logfile)
        {
            if (string.IsNullOrEmpty(logfile))
            {
                logfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            }
            XmlConfigurator.Configure(new FileInfo(logfile));
        }
        public void Log(string message)
        {
            this.Log(string.Empty, message, LogType.Information, null);
        }
        public void Log(string message, LogType logType)
        {
            this.Log(string.Empty, message, logType, null);
        }
        public void Log(string message, LogType logType, Exception exception)
        {
            this.Log(string.Empty, message, logType, exception);
        }
        public void Log(string logName, string message)
        {
            this.Log(logName, message, LogType.Information, null);
        }
        public void Log(string logName, string message, LogType logType)
        {
            this.Log(logName, message, logType, null);
        }
        public void Log(string logName, string message, LogType logType, Exception exception)
        {
            ILog log = string.IsNullOrWhiteSpace(logName) ? LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType) : LogManager.GetLogger(logName);
            if (this.TrackInfo != null && !string.IsNullOrWhiteSpace(this.TrackInfo.TrackId))
            {
                message = string.Format("Ref:{0} {1}", this.TrackInfo.TrackId, message);
            }
            switch (logType)
            {
                case LogType.Information:
                    if (log.IsInfoEnabled)
                    {
                        log.Info(message, exception);
                        return;
                    }
                    break;
                case LogType.Warning:
                    if (log.IsWarnEnabled)
                    {
                        log.Warn(message, exception);
                        return;
                    }
                    break;
                case LogType.Error:
                    if (log.IsErrorEnabled)
                    {
                        log.Error(message, exception);
                        return;
                    }
                    break;
                case LogType.Debug:
                    if (log.IsDebugEnabled)
                    {
                        log.Debug(message, exception);
                        return;
                    }
                    break;
                default:
                    if (log.IsDebugEnabled)
                    {
                        log.Debug(message, exception);
                    }
                    break;
            }
        }
    }
}