using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Models.SavingsDeposit;
using SCCO.WPF.MVC.CS.Utilities;
using SCCO.WPF.MVC.CS.Views.LoanModule;
using SCCO.WPF.MVC.CS.Views.SavingsDepositModule;
using SCCO.WPF.MVC.CS.Views.TimeDepositModule;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    public partial class AccountVerifierWindow
    {
        private readonly List<string> _listLoanReceivableCode;
        private readonly List<string> _listSavingsDepositCode;
        private readonly List<string> _listTimeDepositCode;
        private readonly AccountVerifierViewModel _viewModel;

        //private Nfmb _viewModel.Member = new Nfmb();
        private Dictionary<string, List<AccountDetail>> _accountDetailsLookup;

        // how the summary and details displayed?
        private ShownAccount _accountDisplayed;

        //private AccountSummary _viewModel.SelectedAccount;
        public AccountVerifierWindow()
        {
            InitializeComponent();

            _viewModel = new AccountVerifierViewModel();
            DataContext = _viewModel;
            _listSavingsDepositCode = Account.GetListOfSavingsDepositCode();
            _listTimeDepositCode = Account.GetListOfTimeDepositCode();
            _listLoanReceivableCode = Account.GetListOfLoanReceivableCode();

            ShowAccount(ShownAccount.Summary);

            btnOfficialReceipts.IsEnabled = MainController.LoggedUser.CanAccessTellerCollector;

            // Members
            btnSearchMember.Click += (s, e) => SearchMember();
            btnMemberInformation.Click += (s, e) => ShowMemberInformation();

            // Account Summary/Details
            btnAccountDisplayed.Click += (s, e) => SwitchAccountDisplayed();
            btnPrint.Click += (s, e) => PrintStatementOfAccount();

            // Savings Deposit
            btnSavingsDeposit.Click += (s, e) => DepositSavingsAccount();
            btnWithdrawal.Click += (sender, e) => WithdrawSavingsAccount();

            // Time Deposit
            btnTimeDeposit.Click += (s, e) => OpenTimeDeposit();
            btnShowTimeDepositDetails.Click += (s, e) => ShowTimeDepositDetails();

            // Loans
            btnLoanApplication.Click += (s, e) => ShowLoanApplication();
            btnSalaryAdvance.Click += (s, e) => ShowSalaryAdvance();
            btnLoanDetails.Click += (s, e) => ShowLoanDetails();
            btnFines.Click += (s, e) => ShowFinesRebateCalculator();

            btnOfficialReceipts.Click += (s, e) => ShowOfficialReceipts();

            btnMakers.Click += (s, e) =>
                {
                    var code = _viewModel.Member.MemberCode;
                    var date = MainController.LoggedUser.TransactionDate;
                    var result = ReportController.ShowBorrowerMakers(code, date);
                    if(!result.Success)
                    {
                        MessageWindow.ShowAlertMessage(result.Message);
                    }
                };

            // show calculator
            KeyDown += (s, e) =>
                {
                    if (e.Key == Key.F8 && Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        ShowCalculator();
                    }
                };
        }


        #region --- Members ---

        private void SearchMember()
        {
            Nfmb result = MainController.SearchMember();
            if (result != null && result.MemberCode != null)
            {
                if (result.MemberCode.Length >= 4)
                {
                    _viewModel.Member = result;
                    RefreshMemberInformation();
                    RefreshAccountInformation();
                }
            }
        }

        private void ShowMemberInformation()
        {
            var memberInformationWindow = new MemberInformationWindow(_viewModel.Member.MemberCode);
            memberInformationWindow.ShowDialog();
        }

        private void RefreshMemberInformation()
        {
            lblMemberCodeName.Content = string.Format("{0} - {1}", _viewModel.Member.MemberCode,
                                                      _viewModel.Member.MemberName);
            imgPhoto.Source = ImageTool.CreateImageSourceFromBytes(_viewModel.Member.ContactInformation.Picture);

            var infoBuilder = new StringBuilder();

            bool isMemberCanLoan = false;

            if (!string.IsNullOrEmpty(_viewModel.Member.MembershipType))
            {
                infoBuilder.AppendFormat("{0} Member\n", _viewModel.Member.MembershipType);
                switch (_viewModel.Member.MembershipType)
                {
                    case "Associate (Loaning)":
                    case "Regular":
                        isMemberCanLoan = true;
                        break;
                }
            }
            if (_viewModel.Member.IsDamayanMember)
            {
                infoBuilder.AppendFormat("PTTK Member\n");
            }
            blkMemberInformationSummary.Content = infoBuilder.ToString();

            //borrower's buttons
            btnLoanApplication.IsEnabled = isMemberCanLoan;
            btnSalaryAdvance.IsEnabled = isMemberCanLoan;
            btnMakers.IsEnabled = isMemberCanLoan;

            btnSavingsDeposit.IsEnabled = true;
            btnTimeDeposit.IsEnabled = true;
        }
        #endregion

        #region --- Account Details / Summary ---

        private void ShowAccount(ShownAccount shownAccount)
        {
            if (shownAccount == ShownAccount.Summary)
            {
                if (_viewModel.Member != null)
                {
                    btnAccountDisplayed.Content = "Show Details";
                    btnPrint.Content = "Print Summary";
                    _accountDisplayed = ShownAccount.Summary;
                    ShowAccountSummary();
                }
            }
            else
            {
                if (grdSummary.SelectedItems.Count <= 0) return;

                btnAccountDisplayed.Content = "Show Summary";
                btnPrint.Content = "Print Details";
                _accountDisplayed = ShownAccount.Details;
                ShowAccountDetails();
            }
            RefreshButtons();
        }

        private void PrintStatementOfAccount()
        {
            var asOf = MainController.LoggedUser.TransactionDate;
            Result result;
            if (_accountDisplayed == ShownAccount.Summary)
            {
                var data = _viewModel.AccountSummaries;
                result = ReportController.ShowStatementAccountSummary(data, asOf);
            }
            else
            {
                var data = _viewModel.AccountDetails;
                result = ReportController.ShowStatementAccountDetailed(data, asOf);
            }

            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        private void SwitchAccountDisplayed()
        {
            ShowAccount(_accountDisplayed == ShownAccount.Summary ? ShownAccount.Details : ShownAccount.Summary);
        }

        private void ShowAccountDetails()
        {
            _viewModel.SelectedAccount = (AccountSummary)grdSummary.SelectedItem;
            if (_viewModel.SelectedAccount == null) return;

            List<AccountDetail> accountDetails;
            string key = _viewModel.SelectedAccount.AccountCode + _viewModel.SelectedAccount.CertificateNo;
            if (_accountDetailsLookup.ContainsKey(key))
            {
                accountDetails = _accountDetailsLookup[key];
            }
            else
            {
                accountDetails = AccountDetail.Find(_viewModel.SelectedAccount.MemberCode,
                                                    _viewModel.SelectedAccount.AccountCode,
                                                    _viewModel.SelectedAccount.CertificateNo,
                                                    MainController.LoggedUser.TransactionDate);
                _accountDetailsLookup.Add(key, accountDetails);
            }

            _viewModel.AccountDetails = new ObservableCollection<AccountDetail>();
            foreach (AccountDetail accountDetail in accountDetails)
            {
                _viewModel.AccountDetails.Add(accountDetail);
            }
            //grdDetails.ItemsSource = accountDetails;
            grdSummary.Visibility = Visibility.Collapsed;
            grdDetails.Visibility = Visibility.Visible;
            grdDetails.SelectedIndex = accountDetails.Count - 1;
            //grdDetails.ScrollIntoView(grdDetails.SelectedItem);
            //var nTotalDebit = accountDetails.Sum(d => d.Debit);
            //var nTotalCredit = accountDetails.Sum(d => d.Credit);
        }

        private void ShowAccountSummary()
        {
            grdSummary.Visibility = Visibility.Visible;
            grdDetails.Visibility = Visibility.Collapsed;
        }

        private void RefreshAccountInformation()
        {
            List<AccountSummary> accountSummaries = AccountSummary.Find(_viewModel.Member.MemberCode, MainController.LoggedUser.TransactionDate);
            _accountDetailsLookup = new Dictionary<string, List<AccountDetail>>();

            _viewModel.AccountSummaries = new ObservableCollection<AccountSummary>();
            foreach (AccountSummary accountSummary in accountSummaries)
            {
                _viewModel.AccountSummaries.Add(accountSummary);
            }

            //grdSummary.ItemsSource = accountSummaries;
            ShowAccount(ShownAccount.Summary);
        }

        #endregion

        #region --- Savings Deposit ---

        private void DepositSavingsAccount()
        {
            if (_listSavingsDepositCode == null) return;
            if (_listSavingsDepositCode.Count == 0) return;

            IOrderedEnumerable<AccountSummary> savings = from account in _viewModel.AccountSummaries
                                                         where _listSavingsDepositCode.Contains(account.AccountCode)
                                                         orderby account.AccountCode
                                                         select account;

            if (!savings.Any()) return;

            Account sd = Account.FindByCode(savings.First().AccountCode);

            var view = new PostSavingsDepositView(_viewModel.Member, sd);
            if (view.ShowDialog() == true)
            {
                RefreshAccountInformation();
            }
        }

        private void WithdrawSavingsAccount()
        {
            var selectedAccount = (AccountDetail) grdDetails.SelectedItem;
            if (selectedAccount == null) return;

            //1. must be active
            if (_viewModel.Member.IsAccountClosed)
            {
                MessageWindow.ShowAlertMessage("Closed account!");
                return;
            }
            //2. must be withdrawable account
            if (!_listSavingsDepositCode.Contains(selectedAccount.AccountCode.Trim()))
            {
                MessageWindow.ShowAlertMessage("Not a withdrawable account!");
                return;
            }

            //3. must have sufficient balance
            decimal maintainingBalance = GlobalSettings.AmountOfSavingsDepositMaintainingBalance;
            if (selectedAccount.EndingBalance <= maintainingBalance)
            {
                MessageWindow.ShowAlertMessage("Fund not sufficient");
                return;
            }

            //3. must have sufficient balance
            if (MainController.LoggedUser.TransactionDate != GlobalSettings.DateOfOpenTransaction)
            {
                MessageWindow.ShowAlertMessage(
                    "Record is locked or Transaction Date not set. Please consult your system administrator.");
                return;
            }

            var withdrawalModel = new Withdrawal();
            withdrawalModel.InitializeProperties();
            withdrawalModel.AccountInfo.CurrentBalance = _viewModel.SelectedAccount.Balance;
            withdrawalModel.AccountInfo.MemberCode = selectedAccount.MemberCode;
            withdrawalModel.AccountInfo.MemberName = selectedAccount.MemberName;
            withdrawalModel.AccountInfo.AccountCode = selectedAccount.AccountCode;
            withdrawalModel.AccountInfo.AccountTitle = selectedAccount.Title;

            var withdrawal = new WithdrawalView(withdrawalModel);
            if (withdrawal.ShowDialog() == true)
            {
                RefreshAccountInformation();
            }
        }

        #endregion

        #region --- Time Deposit  ---

        private void OpenTimeDeposit()
        {
            if (_viewModel.Member == null)
            {
                MessageWindow.ShowAlertMessage("Please select a member first.");
                return;
            }

            var openTimeDepositView = new OpenTimeDepositView(_viewModel.Member);
            if (openTimeDepositView.ShowDialog() == true)
            {
                RefreshAccountInformation();
            }
        }

        private void ShowTimeDepositDetails()
        {
            var currentItem = (AccountDetail)grdDetails.SelectedItem;
            if (currentItem == null) return;

            var timeDepositDetailsWindow = new TimeDepositEntryWindow(currentItem.TimeDepositDetails);
            timeDepositDetailsWindow.ShowDialog();
        }

        #endregion

        #region --- Loans ---

        private void ShowLoanApplication()
        {
            LoanApplicationWindow loanAmortizationWindow = _viewModel.Member.MemberCode == null
                                                               ? new LoanApplicationWindow()
                                                               : new LoanApplicationWindow(_viewModel.Member);
            loanAmortizationWindow.ShowDialog();
        }

        private void ShowSalaryAdvance()
        {
            var view = new SalaryAdvanceModule.SalaryAdvanceView(_viewModel.Member);
            view.ShowDialog();
        }

        private void ShowLoanDetails()
        {
            var currentItem = (AccountDetail) grdDetails.SelectedItem;
            if (currentItem == null) return;

            LoanDetails loanDetails = currentItem.LoanDetails;
            if (loanDetails.LoanAmount == 0) return;
            var loanDetailsWindow = new LoanDetailsWindow(loanDetails);
            loanDetailsWindow.ShowDialog();
        }

        private void ShowFinesRebateCalculator()
        {
            if (!_listLoanReceivableCode.Contains(_viewModel.SelectedAccount.AccountCode)) return;
            if (!_viewModel.AccountDetails.Any()) return;

            var loanDetails = from detail in _viewModel.AccountDetails
                              where detail.LoanDetails != null
                              orderby detail.LoanDetails.GrantedDate ascending
                              select detail;

            if (!loanDetails.Any()) return;

            AccountDetail last = loanDetails.LastOrDefault();
            if (last == null) return;

            var model = new FinesRebateCalculatorViewModel();
            model.LoanDetails = last.LoanDetails;
            model.LoanBalance = _viewModel.SelectedAccount.Balance;
            model.ProcessDate = MainController.LoggedUser.TransactionDate;
            //model.FinesRatePerMonth = 2/100; // 2% per month
            //model.FinesRatePerMonth = GlobalSettings.RateOfFinesPerMonth;

            model.Calculate();
            var view = new FinesRebateCalculatorWindow(model);
            view.ShowDialog();
        }

        #endregion

        private void ShowOfficialReceipts()
        {
            var tellerCollectorWindow = new TellerCollectorWindow();
            tellerCollectorWindow.ShowDialog();
        }

        private void PrintPassbook()
        {
            IEnumerable<AccountVerifierDetail> accountDetails = grdDetails.Items.OfType<AccountVerifierDetail>();
            ReportController.GeneratePassbook(
                Converter.ConvertToDataTable(accountDetails.ToList().Where(o => o.IsMarked).ToList()));
        }

        private void RefreshButtons()
        {
            bool isMemberShown = _viewModel.Member != null;

            btnAccountDisplayed.IsEnabled = isMemberShown;
            btnMemberInformation.IsEnabled = isMemberShown;
            btnPrint.IsEnabled = isMemberShown;
            btnOfficialReceipts.IsEnabled = isMemberShown;

            if (_accountDisplayed == ShownAccount.Summary)
            {
                // savings deposit buttons
                btnWithdrawal.IsEnabled = false;

                // time deposit buttons
                btnShowTimeDepositDetails.IsEnabled = false;

                // loans
                btnLoanDetails.IsEnabled = false;
                btnFines.IsEnabled = false;
                btnMakers.IsEnabled = false;
                return;
            }

            if (_viewModel.SelectedAccount == null) return;

            if (_listSavingsDepositCode.Contains(_viewModel.SelectedAccount.AccountCode))
            {
                btnWithdrawal.IsEnabled = true;
            }
            if (_listTimeDepositCode.Contains(_viewModel.SelectedAccount.AccountCode))
            {
                btnShowTimeDepositDetails.IsEnabled = true;
            }
            if (_listLoanReceivableCode.Contains(_viewModel.SelectedAccount.AccountCode))
            {
                btnLoanDetails.IsEnabled = true;
                btnFines.IsEnabled = true;
                btnMakers.IsEnabled = true;
            }
        }

        private void ShowCalculator()
        {
            Process process = Process.Start("calc.exe");
            if (process != null) process.WaitForInputIdle();
        }

        // how the withdrawal and deposit displayed
        private void grdDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdDetails.SelectedItem == null) return;
            var selectedItem = (AccountDetail) grdDetails.SelectedItem;
            selectedItem.Mark = !selectedItem.Mark;
        }

        private enum ShownAccount
        {
            Summary,
            Details
        }
    }
}