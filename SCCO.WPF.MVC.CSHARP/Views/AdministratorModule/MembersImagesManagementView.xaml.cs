using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Properties;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public partial class MembersImagesManagementView
    {
        private GlobalVariable _sharedFolder;

        public MembersImagesManagementView()
        {
            InitializeComponent();
            RefreshDisplay();
            WireUpEvents();
        }

        private void RefreshDisplay()
        {
            // images folder
            _sharedFolder = GlobalVariable.FindByKeyword(GlobalKeys.SharedFolder);
            ImagesFolderTextBox.Text = _sharedFolder.CurrentValue;

            // restore
            var network = Contact.GetImagesFolder();
            RestoreFolderTextBox.Text = network;

            // backup
            var drives = DriveInfo.GetDrives();
            var rootDrive = @"C:\";

            foreach (
                var drive in
                    drives.OrderBy(drive => drive.Name)
                          .Where(drive => drive.DriveType == DriveType.Fixed || drive.DriveType == DriveType.Removable))
            {
                rootDrive = drive.RootDirectory.ToString();
            }
            BackupFolderTextBox.Text = Path.Combine(rootDrive, "BACKUP", "IMAGES");
        }

        private string BrowseFolder()
        {
            var myFolderBrowser = new FolderBrowserDialog {Description = @"Select a Folder", ShowNewFolderButton = true};
            if (myFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                return myFolderBrowser.SelectedPath;

            return null;
        }

        private void WireUpEvents()
        {
            #region --- Images Folder ---

            ImagesFolderPickerButton.Click += (sender, args) =>
                {
                    var folder = BrowseFolder();
                    if (!string.IsNullOrEmpty(folder))
                    {
                        var di = new DirectoryInfo(folder);
                        ImagesFolderTextBox.Text = di.Name;
                    }
                };
            ApplyButton.Click += (sender, args) => SaveFolder();

            #endregion

            #region --- Backup ---

            BackupFolderPickerButton.Click += (sender, args) =>
                {
                    var folder = BrowseFolder();
                    if (!string.IsNullOrEmpty(folder))
                    {
                        BackupFolderTextBox.Text = folder;
                    }
                };

            BackupButton.Click += (sender, args) => Backup();

            #endregion

            #region --- Restore ---

            RestoreFolderPickerButton.Click += (sender, args) =>
                {
                    var folder = BrowseFolder();
                    if (!string.IsNullOrEmpty(folder))
                    {
                        RestoreFolderTextBox.Text = folder;
                    }
                };

            RestoreButton.Click += (sender, args) => Restore();

            #endregion
        }

        private bool IsValid()
        {
            if (!Directory.Exists(BackupFolderTextBox.Text))
            {
                if (MessageWindow.ShowConfirmMessage("Backup folder does not exist! Create folder?") ==
                    MessageBoxResult.Yes)
                {
                    try
                    {
                        Directory.CreateDirectory(BackupFolderTextBox.Text);
                        return Directory.Exists(BackupFolderTextBox.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageWindow.ShowAlertMessage(ex.Message);
                        return false;
                    }
                }
                return false;
            }

            if (!Directory.Exists(RestoreFolderTextBox.Text))
            {
                MessageWindow.ShowAlertMessage("Restore folder does not exist!");
                return false;
            }
            return true;
        }

        private void Backup()
        {
            if (!IsValid())
            {
                return;
            }
            try
            {
                FileSystem.CopyDirectory(RestoreFolderTextBox.Text, BackupFolderTextBox.Text, UIOption.AllDialogs);
                MessageWindow.ShowNotifyMessage("Backup successful!");
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }

        private void Restore()
        {
            if (!IsValid())
            {
                return;
            }
            try
            {
                FileSystem.CopyDirectory(BackupFolderTextBox.Text, RestoreFolderTextBox.Text, UIOption.AllDialogs);
                MessageWindow.ShowNotifyMessage("Restore successful!");
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }

        private void SaveFolder()
        {
            var folder = ImagesFolderTextBox.Text;
            var network = Path.Combine(@"\\", Settings.Default.DatabaseServer, folder);
            if (!Directory.Exists(network))
            {
                MessageWindow.ShowAlertMessage(folder + " is not a shared folder on the server.");
                return;
            }
            var sharedFolder = GlobalVariable.FindByKeyword(GlobalKeys.SharedFolder);
            sharedFolder.CurrentValue = folder;
            sharedFolder.Description = @"Shared Folder where Members Pictures and Signatures can be stored.";
            sharedFolder.Update();
            Contact.RefreshImagesFolder();
            RefreshDisplay();
        }
    }
}