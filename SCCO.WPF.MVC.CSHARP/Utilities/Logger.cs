using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using SCCO.WPF.MVC.CS.Database;
using System.Collections.Generic;

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
                var logFile = string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                Write(logFile, logBuilder.ToString());
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
                var logFile = string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                Write(logFile, logBuilder.ToString());

            }
        }

        public static void Log(string logFile, string message, int level)
        {
            var logMessageBuilder = new StringBuilder();
            if (level > 0)
            {
                if (level == 1)
                    logMessageBuilder.Append(">>> ");
                else
                    logMessageBuilder.Append(new string('-', level*3) + " ");
            }
            logMessageBuilder.Append(message);
            Write(logFile, logMessageBuilder.ToString());
            Console.WriteLine(logMessageBuilder.ToString());
        }

        private static void Write(string logFile, string message)
        {
            lock (_lockObject)
            {
                try
                {
                    var logFolder = CreateLogFolder();
                    var fullPath = Path.Combine(logFolder, Path.ChangeExtension(logFile,".log"));
                    File.AppendAllText(fullPath, string.Format("{0}\t{1}\n", DateTime.Now, message));

                    SaveToDatabase(message);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(@"ALERT: {0}", exception.Message);
                }
            }
        }

        public static void SaveToDatabase(string message)
        {
            try
            {
                if (DatabaseController.IsDatabaseExist("admin"))
                {
                    const string sql = "INSERT INTO admin.`logs` (date, message) VALUES (?date, ?message)";
                    var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("?date", DateTime.Now),
                            new SqlParameter("?message", message)
                        };
                    DatabaseController.ExecuteInsertQuery(sql, parameters.ToArray());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private static string CreateLogFolder()
        {
            var logFolder = GetLogFolder();

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            return logFolder;
        }

        private static string GetLogFolder()
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataFolder, "LOGS");
        }
    }

}
