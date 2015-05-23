using System;
using System.Windows;
using System.Windows.Forms;
using SCCO.WPF.MVC.CS.Views;

namespace SCCO.WPF.MVC.CS.Database
{
    /// <summary>
    /// Interaction logic for BackUpDatabaseWindow.xaml
    /// </summary>
    public partial class BackUpDatabaseWindow
    {
        public BackUpDatabaseWindow()
        {
            InitializeComponent();

            BackupFilePicker.Click += (sender, args) =>
                {
                    var myFilePicker = new OpenFileDialog
                        {
                            Filter = @"SQL Dump File (*.sql)|*.sql|All files (*.*)|*.*",
                        };
                    if (myFilePicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        txtBackupFile.Text = myFilePicker.FileName;
                };

            RestoreButton.Click += (sender, args) =>
                {
                    if (String.IsNullOrEmpty(txtBackupFile.Text))
                    {
                        MessageWindow.ShowAlertMessage("Please select a backup file.");
                        return;
                    }

                    Controllers.Result result = DatabaseUtility.Restore(txtBackupFile.Text);
                    if (result.Success)
                        MessageWindow.ShowNotifyMessage(result.Message);
                    else
                        MessageWindow.ShowAlertMessage(result.Message);
                };
        }

        private void BackUp()
        {
            if (String.IsNullOrEmpty(txtBackupFolder.Text))
            {
                MessageWindow.ShowAlertMessage("Please select a folder.");
                return;
            }

            DatabaseUtility.FolderLocation = txtBackupFolder.Text.Trim();
            Controllers.Result result = DatabaseUtility.Backup();
            if (result.Success)
                MessageWindow.ShowNotifyMessage(result.Message);
            else
                MessageWindow.ShowAlertMessage(result.Message);
        }

        private void BackupButtonOnClick(object sender, RoutedEventArgs e)
        {
            BackUp();
        }

        private void BrowseFolder()
        {
            var myFolderBrowser = new FolderBrowserDialog {Description = @"Select a Folder", ShowNewFolderButton = true};
            if (myFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtBackupFolder.Text = myFolderBrowser.SelectedPath;
            }

        private void FolderPickerButtonOnClick(object sender, RoutedEventArgs e)
        {
            BrowseFolder();
        }
    }
}
