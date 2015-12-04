using System;
using System.Diagnostics;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Views {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() {
            InitializeComponent();
            
            var dbBranch = Properties.Settings.Default.BranchName;
            var dbYear = MainController.LoggedUser.TransactionDate.Year;
            var dbEnv = Properties.Settings.Default.DatabaseEnvironment;

            if (dbEnv.ToLower() == "production")
            {
                Canvass.Visibility = Visibility.Visible;
                CanvassDevelopment.Visibility = Visibility.Collapsed;
            }
            else
            {
                Canvass.Visibility = Visibility.Collapsed;
                CanvassDevelopment.Visibility = Visibility.Visible;
            }

            DatabaseNameLabel.Content = string.Format("{0} {1}", dbBranch.ToUpper(), dbYear);
            if (dbYear != DateTime.Now.Year)
            {
                DatabaseNameLabel.Foreground = System.Windows.Media.Brushes.Red;
                DatabaseNameLabel.ToolTip = string.Format("Recommended database is {0}_{1}_{2}.", dbBranch,
                                                          DateTime.Now.Year, dbEnv);
            }


            var loggedUser = MainController.LoggedUser;

            MemberInformationButton.IsEnabled = loggedUser.CanAccessMemberInformation;
            MemberAccountsVerifierButton.IsEnabled = loggedUser.CanAccessAccountVerifier;
            CashVoucherButton.IsEnabled = loggedUser.CanAccessCashVoucher;
            JournalVoucherButton.IsEnabled = loggedUser.CanAccessJournalVoucher;
            OfficialReceiptsButton.IsEnabled = loggedUser.CanAccessOfficialReceipts;
            TellerCollectorButton.IsEnabled = loggedUser.CanAccessTellerCollector;
            GeneralLedgerReportsButton.IsEnabled = loggedUser.CanAccessGeneralLedgerReports;
            OtherReportsButton.IsEnabled = loggedUser.CanAccessOtherReports;
            InitialSetupButton.IsEnabled = loggedUser.CanAccessInitialSetup;
            AdministratorButton.IsEnabled = loggedUser.IsAdministrator;

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            ProductInformationLabel.Content = string.Format("Accounting System version {0}",
                                                          fileVersionInfo.ProductVersion);


            MemberInformationButton.Click += (sender, args) => MainController.ShowMemberInformationWindow();
            MemberAccountsVerifierButton.Click += (sender, args) => MainController.ShowAccountVerifierWindow();
            AccountsVerifierGLButton.Click += (sender, args) => MainController.ShowAccountVerifierGeneralLedgerWindow();
            CashVoucherButton.Click += (sender, args) => MainController.ShowCashVoucherWindow();
            JournalVoucherButton.Click += (sender, args) => MainController.ShowJournalVoucherWindow();
            OfficialReceiptsButton.Click += (sender, args) => MainController.ShowOfficialReceiptsWindow();
            TellerCollectorButton.Click += (sender, args) => MainController.ShowTellerCollectorWindow();
            GeneralLedgerReportsButton.Click += (sender, args) => MainController.ShowGeneralLedgerReportsWindow();
            OtherReportsButton.Click += (sender, args) => MainController.ShowReportsWindow();
            InitialSetupButton.Click += (sender, args) => MainController.ShowInitialSetupWindow();
            AdministratorButton.Click += (sender, args) => MainController.ShowAdministratorWindow();

            UserNameLabel.MouseDoubleClick += (sender, args) => MainController.ShowUpdateLoginWindow();
            TransactionDateLabel.MouseDoubleClick += (sender, args) => MainController.ShowUserTransactionDateWindow();
            ProductInformationLabel.MouseDoubleClick += (sender, args) => MainController.ShowAboutProject();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            DragMove();
        }
    }
}
