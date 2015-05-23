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

        public static Result Restorex(string backupFile)
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

        public static Result Restore(string backupFile)
        {
            if(!File.Exists(backupFile)) return new Result(false, "Backup file does not exists!");

            try
            {
                var myProcess = new Process();
                myProcess.StartInfo.FileName = "cmd.exe";
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(MySqlExePath);
                myProcess.StartInfo.RedirectStandardInput = false;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();

                var myStreamWriter = myProcess.StandardInput;
                var mystreamreader = myProcess.StandardOutput;
                var args = string.Format("mysql -h {0} -u {1} -p{2} {3} < {4}",
                                         Settings.Default.DatabaseServer,
                                         Settings.Default.DatabaseUser,
                                         Password.Decrypt(Settings.Default.DatabasePassword),
                                         GetDatabaseName(),
                                         Backup());

                myStreamWriter.WriteLine(args);
                myStreamWriter.Close();
                myProcess.WaitForExit();
                myProcess.Close();
                return new Result(true, "Database restore successful");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }

            }


        public static bool ExecuteSvnCommandWithFileInput(string command, string arguments, string filePath, out string result, out string errors)
        {
            bool retval = false;
            string output = string.Empty;
            string errorLines = string.Empty;
            Process svnCommand = null;
            var psi = new ProcessStartInfo(command);

            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            try
            {
                Process.Start(psi);
                psi.Arguments = arguments;
                svnCommand = Process.Start(psi);

                var file = new FileInfo(filePath);
                StreamReader reader = file.OpenText();
                string fileContents = reader.ReadToEnd();
                reader.Close();

                StreamWriter myWriter = svnCommand.StandardInput;
                StreamReader myOutput = svnCommand.StandardOutput;
                StreamReader myErrors = svnCommand.StandardError;

                myWriter.AutoFlush = true;
                myWriter.Write(fileContents);
                myWriter.Close();

                output = myOutput.ReadToEnd();
                errorLines = myErrors.ReadToEnd();

                // Check for errors
                if (errorLines.Trim().Length == 0)
                {
                    retval = true;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                errorLines += Environment.NewLine + msg;
            }
            finally
            {
                if (svnCommand != null)
                {
                    svnCommand.Close();
                }
            }

            result = output;
            errors = errorLines;

            return retval;
        }
    }
}



