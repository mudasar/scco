using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.AboutProject
{
    class Controller
    {
        private static string _rootFolder;
        private static string _fileServer;
        private static string _applicationName;

        public static Controllers.Result CheckUpdates()
        {
            try
            {
                _rootFolder = GetRootFolder();

                var xmlFile = Path.Combine(_rootFolder, "VersionManager.xml");
                if (!File.Exists(xmlFile))
                {
                    return new Controllers.Result(false, string.Format("File is missing: '{0}'", xmlFile));
                }
                XElement xElement = XElement.Load(xmlFile);
                var xeFileServer = xElement.Element("FileServer");
                var xeApplicationName = xElement.Element("ApplicationName");

                _fileServer = _rootFolder;
                _applicationName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);

                if (xeFileServer != null) _fileServer = xeFileServer.Value;
                if (xeApplicationName != null) _applicationName = xeApplicationName.Value;


                var currentVersion = string.Format("{0} v.{1}", Path.GetFileNameWithoutExtension(_applicationName),
                                                   FileVersionInfo.GetVersionInfo(Path.Combine(_rootFolder, _applicationName))
                                                                  .FileVersion);
                var latestVersion = string.Format("{0} v.{1}", Path.GetFileNameWithoutExtension(_applicationName),
                                                   FileVersionInfo.GetVersionInfo(Path.Combine(_fileServer, _applicationName))
                                                                  .FileVersion);


                if (currentVersion != latestVersion)
                {
                    return new Controllers.Result(true, "Latest version is " + latestVersion);
                }
                return new Controllers.Result(false, "Project is up-to-date.");
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(typeof(Controller), exception);
                return new Controllers.Result(false, exception.Message);
            }
        }

        public static void UpdateProject()
        {
            var exeFilePath = Path.Combine(GetRootFolder(), "updater.exe");
            try
            {
                Process.Start(exeFilePath);
                Environment.Exit(0);
            }
            catch (Exception exception)
            {
                File.AppendAllText(Path.ChangeExtension(exeFilePath,"log"), exception.ToString());
            }
        }

        public static string GetRootFolder()
        {
            // get the root folder
            string rootFolder = Environment.CommandLine;
            rootFolder = rootFolder.Substring(1, rootFolder.Length - 3);
            rootFolder = Path.GetDirectoryName(rootFolder);
            return rootFolder;
        }
    }
}
