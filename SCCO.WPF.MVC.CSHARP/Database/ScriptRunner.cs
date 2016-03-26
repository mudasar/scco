using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Properties;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Database
{
    public static class ScriptRunner
    {
        private const string SCRIPT_STAMP_LABEL = "ScriptStamp";
        private static volatile string _fileVersionInfo;

        public static Result Perform()
        {
            InitializeMigrationStamp();
            var scripts = GetScriptFiles();
            try
            {
                var conn = DatabaseController.SharedDbConnection;
                foreach (var fileInfo in scripts)
                {
                    if (!fileInfo.Exists) continue;
                    var script = new MySqlScript(conn, File.ReadAllText(fileInfo.FullName)) {Delimiter = "$$"};
                    script.Execute();
                }
                GlobalSettings.Update(SCRIPT_STAMP_LABEL, _fileVersionInfo);
                return new Result(true, "Migration execution successful.");
            }
            catch (Exception exception)
            {
                Logger.SaveToDatabase(exception.Message);
                return new Result(false, exception.Message);
            }
        }

        public static bool IsMigrationPending()
        {
            InitializeMigrationStamp();
            if (!GetScriptFiles().Any())
            {
                return false;
            }
            var scriptStamp = GlobalVariable.FindByKeyword(SCRIPT_STAMP_LABEL);
            return _fileVersionInfo != scriptStamp.CurrentValue;
        }

        private static IEnumerable<FileInfo> GetScriptFiles()
        {
            var scriptsFolder = GetScriptsFolder();
            if (!Directory.Exists(scriptsFolder)) return new FileInfo[0];

            return Directory.EnumerateFiles(scriptsFolder, "*.sql", SearchOption.AllDirectories)
                            .Select(file => new FileInfo(file));
        }

        private static string GetScriptsFolder()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                "Scripts",
                                _fileVersionInfo,
                                Settings.Default.BranchName);
        }

        private static void InitializeMigrationStamp()
        {
            if (_fileVersionInfo != null) return;
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            _fileVersionInfo = string.Format("{0}", fileVersionInfo.FileVersion);
        }
    }
}