using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using SCCO.WPF.MVC.CS.Views;


namespace SCCO.WPF.MVC.CS.Utilities.DbfMigration.Views
{
    /// <summary>
    /// Interaction logic for MigrateDbfToMySqlWindow.xaml
    /// </summary>
    public partial class MigrateDbfToMySqlWindow
    {
        public MigrateDbfToMySqlWindow()
        {
            InitializeComponent();
            MigrateDataButton.Click += (sender, args) => MigrateFromDbf();
        }

        private void MigrateFromDbf()
        {
            if (String.IsNullOrEmpty(BackupFolder.Text))
            {
                MessageWindow.ShowAlertMessage("Please select a folder.");
                return;
            }

            if (!ConfirmAction()) return;

            var loggedYear = Controllers.MainController.LoggedUser.TransactionDate.Year;
            var dataFolderList = GetDataFolderList(BackupFolder.Text, loggedYear);
            if (dataFolderList == null) return;
            if (!dataFolderList.Any())
            {
                MessageWindow.ShowAlertMessage("No folders found for db files.");
                return;
            }

            var progressWindow = new MigrationProgessWindow(dataFolderList, GetTablesToMigrate());
            progressWindow.ShowDialog();
        }

        private static List<string> GetDataFolderList(string dataFolder, int year)
        {
            var rootFolder = Path.Combine(dataFolder, year.ToString());
            var foldersList = new List<string>();

            for (int i = 0; i < 12; i++)
            {
                var dateTime = DateTime.Parse(string.Format("{0}/{1}/1", year, i + 1));
                var month = dateTime.ToString("MMM").ToLower();
                var folderPath = Path.Combine(rootFolder, month);
                if (Directory.Exists(folderPath))
                {
                    foldersList.Add(folderPath);
                }
            }

            return foldersList;
        }

        private static bool ConfirmAction()
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.Append("This process will delete records in the server. ");
            messageBuilder.AppendLine("You may backup first before proceeding.");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Do you want to continue?");
            return MessageWindow.ShowConfirmMessage(messageBuilder.ToString()) == MessageBoxResult.Yes;
        }

        private void BrowseFolder()
        {
            var myFolderBrowser = new FolderBrowserDialog {Description = @"Select a Folder", ShowNewFolderButton = true};
            if (myFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                BackupFolder.Text = myFolderBrowser.SelectedPath;
            }

        private void FolderPickerButtonOnClick(object sender, RoutedEventArgs e)
        {
            BrowseFolder();
        }

        private List<string> GetTablesToMigrate()
        {
            var tables = new List<string>();
            if (CashVoucherCheckBox.IsChecked == true)
            {
                tables.Add("cv");
            }

            if (JournalVoucherCheckBox.IsChecked == true)
            {
                tables.Add("jv");
            }

            if (OfficialReceiptCheckBox.IsChecked == true)
            {
                tables.Add("or");
            }

            return tables;
        }
    }
}
