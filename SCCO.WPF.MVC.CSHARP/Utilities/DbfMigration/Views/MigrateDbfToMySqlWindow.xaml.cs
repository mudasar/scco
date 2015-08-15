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
            var dbfLocation = DbfLocationTextBox.Text;
            if (String.IsNullOrEmpty(dbfLocation))
            {
                MessageWindow.ShowAlertMessage("Please select a folder.");
                return;
            }

            if (!ConfirmAction()) return;

            if (!TruncateTables()) return;

            var loggedYear = Controllers.MainController.LoggedUser.TransactionDate.Year;

            var queuedTables = new Queue<FileInfo>();
            
            var dataFolderList = GetDataFolderList(DbfLocationTextBox.Text, loggedYear);
            foreach (var folder in dataFolderList)
            {
                if (CashVoucherCheckBox.IsChecked == true)
                {
                    queuedTables.Enqueue(new FileInfo(Path.Combine(folder, "cv.dbf")));
                }

                if (JournalVoucherCheckBox.IsChecked == true)
                {
                    queuedTables.Enqueue(new FileInfo(Path.Combine(folder, "jv.dbf")));
                }

                if (OfficialReceiptCheckBox.IsChecked == true)
                {
                    queuedTables.Enqueue(new FileInfo(Path.Combine(folder, "or.dbf")));
                }
            }
            if (!queuedTables.Any())
            {
                MessageWindow.ShowAlertMessage("No db files to migrate.");
                return;
            }

            var progressWindow = new MigrationProgessWindow(queuedTables);
            progressWindow.ShowDialog();
        }

        private bool TruncateTables()
        {
            if (TruncateTableCheckBox.IsChecked == false) return true;

            if (CashVoucherCheckBox.IsChecked == true)
            {
                if (!MigrationHelper.TruncateTable("cv")) return false;
            }
            if (JournalVoucherCheckBox.IsChecked == true)
            {
                if (!MigrationHelper.TruncateTable("jv")) return false;
            }
            if (OfficialReceiptCheckBox.IsChecked == true)
            {
                if (!MigrationHelper.TruncateTable("or")) return false;
            }
            return true;
        }

        private static IEnumerable<string> GetDataFolderList(string dataFolder, int year)
        {
            var rootFolder = Path.Combine(dataFolder, "data", year.ToString());
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
                DbfLocationTextBox.Text = myFolderBrowser.SelectedPath;
            }

        private void FolderPickerButtonOnClick(object sender, RoutedEventArgs e)
        {
            BrowseFolder();
        }

    }
}
