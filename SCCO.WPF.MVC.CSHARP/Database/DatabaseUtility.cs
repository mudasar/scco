using System;
using System.IO;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Database
{
    public class DatabaseUtility
    {
        public static string FolderLocation { get; set; }

        private static string BackupFilePath
        {
            get
            {
                string fileName = string.Format("{0}_{1:yyyyMMddHHmmss}.sql", CurrentDatabase(), DateTime.Now);
                return Path.Combine(FolderLocation, fileName);
            }
        }

        private static string CurrentDatabase()
        {
            int year = MainController.LoggedUser.TransactionDate.Year;
            return DatabaseController.GetDatabaseByYear(year);
        }

        public static Result Backup()
        {
            try
            {
                DatabaseController.Backup(CurrentDatabase(), BackupFilePath);
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
                DatabaseController.Restore(CurrentDatabase(), dumpFile);
                return new Result(true, "Restore successful.");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }
    }
}