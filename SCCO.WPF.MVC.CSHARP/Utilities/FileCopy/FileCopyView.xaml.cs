using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace SCCO.WPF.MVC.CS.Utilities.FileCopy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class FileCopyView
    {
        private readonly FileCopyViewModel _viewModel;

        public FileCopyView(FileCopyViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
            Title = "File " + _viewModel.ProcessLabel;

            btnUpdate.Click += (s, e) => CopyFiles();

            btnCancel.Click += (s, e) => Close();
        }

        private static bool AreTheyEqual(string fileToCopy, string fileToReplace)
        {
            var fileToCopyExt = Path.GetExtension(fileToCopy);
            var fileToReplaceExt = Path.GetExtension(fileToReplace);

            if (fileToCopyExt != fileToReplaceExt) { return false; }
            if (fileToReplaceExt != null && (fileToReplaceExt.ToLower() == ".exe" || fileToReplaceExt.ToLower() == ".dll"))
            {
                var fileToCopyVersionInfo = FileVersionInfo.GetVersionInfo(fileToCopy);
                var fileToReplaceVersionInfo = FileVersionInfo.GetVersionInfo(fileToReplace);
                if (fileToCopyVersionInfo.FileVersion != null && fileToReplaceVersionInfo.FileVersion != null)
                {
                    var fileToCopyVersion = new Version(fileToCopyVersionInfo.FileVersion);
                    var fileToReplaceVersion = new Version(fileToReplaceVersionInfo.FileVersion);
                    return fileToCopyVersion == fileToReplaceVersion;
                }
            }

            byte[] fileToReplaceCheckSum;
            byte[] filesToCopyCheckSum;

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(fileToReplace))
                {
                    fileToReplaceCheckSum = md5.ComputeHash(stream);
                }
                using (var stream = File.OpenRead(fileToCopy))
                {
                    filesToCopyCheckSum = md5.ComputeHash(stream);
                }
            }

            if (fileToReplaceCheckSum.Length != filesToCopyCheckSum.Length) { return false; }
            return !fileToReplaceCheckSum.Where((t, i) => t != filesToCopyCheckSum[i]).Any();
        }

        private delegate void UpdateProgressBar(System.Windows.DependencyProperty dp, object value);
        private delegate void RefreshDisplay(System.Windows.DependencyProperty dp, object value);

        private void CopyFiles()
        {
            try
            {
                //btnUpdate.IsEnabled = false;
                //btnCancel.IsEnabled = false;
                var updateProgressBar = new UpdateProgressBar(ProgressBar1.SetValue);
                var refreshDisplay = new RefreshDisplay(StatusLabel.SetValue);
                var destinationFolder = _viewModel.DestinationFolder;
                var sourceFolder = _viewModel.SourceFolder;

                if (destinationFolder == null) return;

                // copy directory first
                Directory.CreateDirectory(destinationFolder);

                var directories = Directory.GetDirectories(sourceFolder, "*.*", SearchOption.AllDirectories);
                foreach (string directory in directories)
                {
                    Directory.CreateDirectory(directory.Replace(sourceFolder, destinationFolder));
                }

                // copy all (new or modified) files
                var filesToCopy = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);
                var nTotalFiles = filesToCopy.Length;
                for (int i = 0; i < nTotalFiles; i++)
                {
                    var fileToCopy = filesToCopy[i];
                    var fileToReplace = fileToCopy.Replace(sourceFolder, destinationFolder);
                    if (File.Exists(fileToReplace))
                    {
                        if (!AreTheyEqual(fileToCopy, fileToReplace))
                        {
                            File.Copy(fileToCopy, fileToReplace, true);
                        }
                    }
                    else
                    {
                        File.Copy(fileToCopy, fileToReplace, true);
                    }
                    var value = (double) ((Convert.ToDecimal(i + 1)/nTotalFiles)*100);
                    Dispatcher.Invoke(updateProgressBar,
                                      System.Windows.Threading.DispatcherPriority.Background,
                                      new object[] {RangeBase.ValueProperty, value});

                    Dispatcher.Invoke(updateProgressBar,
                                      System.Windows.Threading.DispatcherPriority.Background,
                                      new object[] {RangeBase.ValueProperty, value});

                    var label = string.Format("Copying {0}...", fileToCopy);
                    Dispatcher.Invoke(refreshDisplay,
                                      System.Windows.Threading.DispatcherPriority.Background,
                                      new object[] {RangeBase.ValueProperty, label});
                }
                _viewModel.ProgressStatus = string.Format("{0} successful!", _viewModel.ProcessLabel);
                Dispatcher.Invoke(refreshDisplay,
                                  System.Windows.Threading.DispatcherPriority.Background,
                                  new object[]
                                  {RangeBase.ValueProperty, string.Format("{0} successful!", _viewModel.ProcessLabel)});
            }
            catch (Exception ex)
            {
                _viewModel.ProgressStatus = ex.Message;
            }
            //finally
            //{
            //    btnUpdate.IsEnabled = true;
            //    btnCancel.IsEnabled = true;
            //}
        }
    }
}