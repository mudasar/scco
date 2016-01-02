using System;
using System.IO;
using System.Windows.Forms;
using SCCO.WPF.MVC.CS.Views;
using System.Linq;
namespace SCCO.WPF.MVC.CS.Database
{
    /// <summary>
    /// Interaction logic for BackUpDatabaseWindow.xaml
    /// </summary>
    public partial class RestoreFromBackupWindow
    {
        public RestoreFromBackupWindow()
        {
            InitializeComponent();

            FilePickerButton.Click += (sender, args) => SelectBackupFile();
            RestoreButton.Click += (sender, args) => Restore();
        }

        private void SelectBackupFile()
        {
            var ofd = new OpenFileDialog
                {
                    Filter = @"SQL File | *.sql",
                    InitialDirectory = DriveInfo.GetDrives()
                                                .OrderBy(t => t.Name)
                                                .Last(t => t.DriveType == DriveType.Fixed)
                                                .Name,
                    Title = @"Select backup file"
                };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BackupFileTextBox.Text = ofd.FileName;
            }
        }

        private void Restore()
        {
            var backupFile = BackupFileTextBox.Text;
            if (String.IsNullOrEmpty(backupFile))
            {
                MessageWindow.ShowAlertMessage("Please select a backup file.");
                return;
            }

            if (!File.Exists(backupFile))
            {
                MessageWindow.ShowAlertMessage("Backup file does not exist.");
                return;
            }

            Controllers.Result result = DatabaseUtility.Restore(backupFile);
            if (result.Success)
                MessageWindow.ShowNotifyMessage(result.Message);
            else
                MessageWindow.ShowAlertMessage(result.Message);
        }
    }
}
