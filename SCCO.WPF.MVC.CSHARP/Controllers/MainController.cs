using System;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities.DbfMigration.Views;
using SCCO.WPF.MVC.CS.Views;
using SCCO.WPF.MVC.CS.Views.AccountVerifierModule;
using SCCO.WPF.MVC.CS.Views.AdministratorModule;
using SCCO.WPF.MVC.CS.Views.InitialSetupModule;
using SCCO.WPF.MVC.CS.Views.ReportsModule;
using SCCO.WPF.MVC.CS.Views.SearchModule;
using SCCO.WPF.MVC.CS.Views.UserModule;

namespace SCCO.WPF.MVC.CS.Controllers
{
    internal class MainController
    {
        //internal static User LoggedUser;
        internal static User LoggedUser;

        private static DateTime _userTransactionDate = DateTime.Now;

        internal static DateTime UserTransactionDate
        {
            get { return _userTransactionDate; }
            set
            {
                _userTransactionDate = value;

                _mainWindow.TransactionDateLabel.Content = UserTransactionDate.ToLongDateString();

                System.Windows.Media.Brush redBrush = System.Windows.Media.Brushes.Red;
                System.Windows.Media.Brush whiteBrush = System.Windows.Media.Brushes.White;

                var isReadOnly = UserTransactionDate != GlobalSettings.DateOfOpenTransaction;
                _mainWindow.TransactionDateLabel.Foreground = isReadOnly ? redBrush : whiteBrush;

                DatabaseController.SwitchDatabase(_userTransactionDate.Year);
            }
        }

        private static MainWindow _mainWindow;

        internal static void ShowMainWindow()
        {
            _mainWindow = new MainWindow
                {
                    UserNameLabel = { Content = LoggedUser.UserName }
                };
            UserTransactionDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            // hide backup button
            //if (!Directory.Exists(Properties.Settings.Default.MySQLServerPath))
            //    _mainWindow.DatabaseBackupButton.Visibility = Visibility.Hidden;

            _mainWindow.Show();
        }

        internal static void ShowBackUpWindow()
        {
            if (!LoggedUser.CanAccessDatabaseBackup)
            {
                MessageWindow.ShowAlertMessage("This module is not available for current user.");
                return;
            }
            var backUpDataBaseWindow = new MigrateDbfToMySqlWindow();
            backUpDataBaseWindow.Show();
        }

        internal static void ShowAccountVerifierWindow()
        {
            if (!LoggedUser.CanAccessAccountVerifier)
            {
                MessageWindow.ShowAlertMessage("This module is not available for current user.");
                return;
            }
            var accountVerifierWindow = new AccountVerifierWindow();
            accountVerifierWindow.Show();
        }

        internal static void DisplayError(string error)
        {
            const string title = @"Error";
            const MessageBoxImage icon = MessageBoxImage.Error;
            const MessageBoxButton button = MessageBoxButton.OK;
            string message;
            if (error.EndsWith(".") || error.EndsWith("!"))
            {
                message = error;
            }
            else
            {
                message = error + "!";
            }
            MessageBox.Show(message, title, button, icon);
        }

        internal static void ShowCashVoucherWindow()
        {
            if (!LoggedUser.CanAccessCashVoucher)
            {
                MessageWindow.ShowAlertMessage("This module is not available for current user.");
                return;
            }
            var cashVoucherWindow = new CashVoucherWindow();
            cashVoucherWindow.Show();
        }

        internal static void ShowJournalVoucherWindow()
        {
            if (!LoggedUser.CanAccessJournalVoucher)
            {
                MessageWindow.ShowAlertMessage("This module is not available for current user.");
                return;
            }
            var journalVoucherWindow = new JournalVoucherWindow();
            journalVoucherWindow.Show();
        }

        internal static void ShowOfficialReceiptsWindow()
        {
            if (!LoggedUser.CanAccessOfficialReceipts)
            {
                MessageWindow.ShowAlertMessage("This module is not available for current user.");
                return;
            }
            var officialReceiptsWindow = new OfficialReceiptWindow();
            officialReceiptsWindow.Show();
        }

        internal static void ShowTellerCollectorWindow()
        {
            if (!LoggedUser.CanAccessTellerCollector)
            {
                MessageWindow.ShowAlertMessage("This module is not available for current user.");
                return;
            }
            var tellerCollectorWindow = new TellerCollectorWindow();
            tellerCollectorWindow.Show();
        }

        internal static void ShowGeneralLedgerReportsWindow()
        {
            var generalLedgerReportsWindow = new GeneralLedgerReportsWindow();
            generalLedgerReportsWindow.Show();
        }

        internal static void ShowInitialSetupWindow()
        {
            if (!LoggedUser.CanAccessInitialSetup)
            {
                MessageWindow.ShowAlertMessage("This module is not available for current user.");
                return;
            }
            var initialSetupWindow = new InitialSetupWindow();
            initialSetupWindow.Show();
        }

        internal static void ShowMemberInformationWindow()
        {
            var memberInformationWindow = new MemberInformationWindow();
            memberInformationWindow.Show();
        }

        internal static void ShowReportsWindow()
        {
            var reportsWindow = new ReportsWindow();
            reportsWindow.Show();
        }

        internal static void ShowBranchNameSetupWindow()
        {
            var view = new BranchNameSetupWindow();
            view.ShowDialog();
        }

        //internal static void MoveFocusToNextControlOnEnter(object sender, KeyEventArgs e) {
        //    if (e.Key != Key.Enter) return;
        //    var tRequest = new TraversalRequest(FocusNavigationDirection.Next);
        //    var keyboardFocus = Keyboard.FocusedElement as UIElement;

        //    if (keyboardFocus != null) {
        //        keyboardFocus.MoveFocus(tRequest);
        //    }
        //    e.Handled = true;
        //}

        internal static Nfmb SearchMember()
        {
            var members = Nfmb.GetList();
            var searchItems =
                members.Select(
                    member =>
                    new SearchItem(member.ID, member.MemberName) {ItemCode = member.MemberCode}).
                        ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();


            if (searchByCodeWindow.DialogResult == false) return null;

            var searchItem = new Nfmb();
            searchItem.Find(searchByCodeWindow.SelectedItem.ItemId);
            return searchItem;
        }

        internal static Account SearchAccount()
        {
            var accounts = Account.CollectAll();
            var searchItems =
                accounts.Select(
                    account =>
                    new SearchItem(account.ID, account.AccountTitle)
                        {
                            ItemCode = account.AccountCode
                        }).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();


            if (searchByCodeWindow.DialogResult == false) return null;
            var searchItem = new Account();
            searchItem.Find(searchByCodeWindow.SelectedItem.ItemId);
            return searchItem;
        }

        internal static SearchItem SearchGeneralLedgerAccount()
        {
            var searchItems = new System.Collections.Generic.List<SearchItem>();
            var sql = DatabaseController.GenerateSelectStatement("generic_accounts");
            var dataTable = DatabaseController.ExecuteSelectQuery(sql);
            foreach (System.Data.DataRow  dataRow in dataTable.Rows)
            {
                var id = Utilities.DataConverter.ToInteger(dataRow["id"]);
                var code = Utilities.DataConverter.ToString(dataRow["account_code"]);
                var title = Utilities.DataConverter.ToString(dataRow["account_title"]);
                var item = new SearchItem(id,title);
                item.ItemCode = code;

                searchItems.Add(item);
            }

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();


            if (searchByCodeWindow.DialogResult == false) return null;
            //var searchItem = new Account();
            //searchItem.Find(searchByCodeWindow.SelectedItem.ItemId);
            return searchByCodeWindow.SelectedItem;
        }

        internal static void ShowDatabaseConfigurationWindow()
        {
            var databaseConfiguration = new DatabaseConfigurationView();
            databaseConfiguration.ShowDialog();
        }

        internal static void ShowUpdatePasswordWindow()
        {
            var updatePassword = new ChangePasswordView();
            updatePassword.ShowDialog();
        }

        public static void ShowUserTransactionDateWindow()
        {
            var userDatePicker = new UserTransactionDateWindow();
            if (userDatePicker.ShowDialog() == true)
            {
                UserTransactionDate = userDatePicker.SelectedTransactionDate;
            }
        }

        internal static void ShowAboutProject()
        {
            var view = new AboutProject.AboutView();
            view.ShowDialog();
        }

        public static void ShowAdministratorWindow()
        {
            var view = new AdministratorWindow();
            view.ShowDialog();
        }

        internal static void ShowAccountVerifierGeneralLedgerWindow()
        {
            var view = new GeneralLedgerAccountVerifierWindow();
            view.Show();
        }
    }
}