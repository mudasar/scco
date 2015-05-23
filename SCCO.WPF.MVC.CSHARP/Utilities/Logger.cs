using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SCCO.WPF.MVC.CS.Utilities
{
    public class Logger
    {
        private static volatile object _lockObject = new object();
        public static void ExceptionLogger(object classname, Exception exception)
        {
            lock (_lockObject)
            {
                var logBuilder = new StringBuilder();
                logBuilder.AppendFormat(">>> ExceptionLogger: {0:f}", DateTime.Now);
                logBuilder.AppendLine("");
                logBuilder.AppendFormat(">>> ClassName: {0}", classname.GetType());
                logBuilder.AppendLine("");
                logBuilder.AppendFormat(">>> TargetSite: {0}", exception.TargetSite);
                logBuilder.AppendLine("");
                logBuilder.AppendFormat(">>> Message: " + exception.Message);
                logBuilder.AppendLine("");
                logBuilder.AppendLine(">>> StackTrace: ");
                logBuilder.AppendLine(exception.StackTrace);
                logBuilder.AppendLine(new string('=', 80));

                Debug.WriteLine(logBuilder.ToString());
                string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                string logFile = string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                File.AppendAllText(Path.Combine(logDirectory, logFile), logBuilder.ToString());
            }
        }

        public static void ExceptionLogger(Type classname, Exception exception)
        {
            lock (_lockObject)
            {
                var logBuilder = new StringBuilder();
                logBuilder.AppendFormat(">>> ExceptionLogger: {0:f}", DateTime.Now); 
                logBuilder.AppendLine("");
                logBuilder.AppendFormat(">>> ClassName: {0}", classname);
                logBuilder.AppendLine("");
                logBuilder.AppendFormat(">>> TargetSite: {0}", exception.TargetSite);
                logBuilder.AppendLine("");
                logBuilder.AppendFormat(">>> Message: " + exception.Message);
                logBuilder.AppendLine("");
                logBuilder.AppendLine(">>> StackTrace: ");
                logBuilder.AppendLine(exception.StackTrace);
                logBuilder.AppendLine(new string('=', 80));

                Debug.WriteLine(logBuilder.ToString());
                string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                string logFile = string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                File.AppendAllText(Path.Combine(logDirectory, logFile), logBuilder.ToString());
            }
        }
    }
}
