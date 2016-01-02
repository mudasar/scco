using System;
using System.Data;
using System.IO;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Properties;

namespace SCCO.WPF.MVC.CS.Database
{
    public class DatabaseUtility
    {
        public static string FolderLocation { get; set; }

        private static string BackupFilePath
        {
            get
            {
                string fileName = string.Format("{0}_{1:yyyyMMddHHmmss}.sql", GetDatabaseName(), DateTime.Now);
                return Path.Combine(FolderLocation, fileName);
            }
        }

        private static string GetDatabaseName()
        {
            int year = MainController.LoggedUser.TransactionDate.Year;
            string branch = Settings.Default.BranchName;
            string environment = Settings.Default.DatabaseEnvironment;
            return string.Format("{0}_{1}_{2}", branch, year, environment);
        }

        public static string BaseDirectory()
        {
            const string query = "SHOW VARIABLES WHERE Variable_Name = 'basedir'";
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                return dataRow[1].ToString();
            }
            return "";
        }

        public static string ProgramDataDirectory()
        {
            const string query = "SHOW VARIABLES WHERE Variable_Name = 'datadir'";
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                return dataRow[1].ToString();
            }
            return "";
        }

        public static Result Backup()
        {
            try
            {
                DatabaseController.Backup(GetDatabaseName(), BackupFilePath);
                return new Result(true, "Backup successful.");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }

        public static Result Restore(string dumpFile)
        {
            try
            {
                DatabaseController.Restore(GetDatabaseName(), dumpFile);
                return new Result(true, "Restore successful.");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }
    }
}