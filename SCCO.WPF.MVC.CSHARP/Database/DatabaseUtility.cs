using System;
using System.Diagnostics;
using System.IO;
using SCCO.WPF.MVC.CS.Properties;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Database
{
    public class DatabaseUtility
    {
        //private Database.DatabaseController _databaseController;
        public static string FolderLocation { get; set; }

        private static string GetDatabaseName()
        {
            var year = MainController.LoggedUser.TransactionDate.Year;
            var branch = Settings.Default.BranchName;
            var environment = Settings.Default.DatabaseEnvironment;
            return string.Format("{0}_{1}_{2}", branch, year, environment);
        }

        private static string BackupFilePath
        {
            get
            {
                var datetime = DateTime.Now;
                var year = datetime.Year;
                var month = datetime.Month;
                var day = datetime.Day;
                var hour = datetime.Hour;
                var minute = datetime.Minute;
                var second = datetime.Second;
                var millisecond = datetime.Millisecond;
                return FolderLocation + "\\" + "MySqlBackup" + Convert.ToString(year) + "-" + Convert.ToString(month) +
                       "-" + Convert.ToString(day) + "-" + Convert.ToString(hour) + "-" + Convert.ToString(minute) + "-" +
                       Convert.ToString(second) + "-" + Convert.ToString(millisecond) + ".sql";
            }
        }

        private static string MySqlDumpExePath 
        {
            get
            {
                var filePath = Path.Combine(Settings.Default.MySQLServerPath, "db", "bin", "mysqldump.exe");
                if (!File.Exists(filePath))
                    filePath = Path.Combine(Settings.Default.MySQLServerPath, "bin", "mysqldump.exe");
                
                return filePath;
            }
        }

        private static string MySqlExePath
        {
            get
            {
                var filePath = Path.Combine(Settings.Default.MySQLServerPath, "db", "bin", "mysql.exe");
                if (!File.Exists(filePath))
                    filePath = Path.Combine(Settings.Default.MySQLServerPath, "bin", "mysql.exe");

                return filePath;
            }
        }

        public static Result Backup()
        {
            try
            {
                var myProcess = new Process
                    {
                        StartInfo =
                            {
                                FileName = MySqlDumpExePath,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                Arguments = "-h localhost --user=" + Settings.Default.DatabaseUser + " --password=" +
                                            Password.Decrypt(Settings.Default.DatabasePassword) +
                                            " --databases " + GetDatabaseName() + " --result-file " +
                                            BackupFilePath
                            }
                    };
                myProcess.Start();
                string output = myProcess.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                return new Result(true, @"Database backup successful.");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        public static Result Restore(string backupFile)
        {
            try
            {
                var myProcess = new Process
                    {
                        StartInfo =
                            {
                                FileName = MySqlExePath,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true
                            }
                    };
                //var args = string.Format("mysql -u [uname] -p[pass] [db_to_restore] < [backupfile.sql]");
                var args = string.Format("-h {0} -u {1} -p{2} {3} < {4}",
                    Settings.Default.DatabaseServer,
                    Settings.Default.DatabaseUser,
                    Password.Decrypt(Settings.Default.DatabasePassword),
                    GetDatabaseName(),
                    Backup());
                myProcess.StartInfo.Arguments = args;
                myProcess.Start();
                string output = myProcess.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                return new Result(true, @"Database restore successful.");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }

        }
    }
}



