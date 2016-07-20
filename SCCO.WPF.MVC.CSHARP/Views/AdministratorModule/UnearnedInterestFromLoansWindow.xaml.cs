using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    /// <summary>
    ///     Interaction logic for UnearnedInterestFromLoansWindow.xaml
    ///     1. Get the monthly amortization of "Unearned Interest from Loans" per currrent/non-settled loan
    ///     - Unearned Interest / Monthly Payments
    ///     2. Post in voucher:
    ///     - Debit: Unearned Interest
    ///     - Credit - Interest on Loan / Miscellaneous Income if Associate Member
    /// </summary>
    public partial class UnearnedInterestFromLoansWindow
    {
        private readonly UnearnedInterestFromLoansViewModel _viewModel;
        private Collection<AccountSummary> _memberShareCapitalAccountSummary;

        public UnearnedInterestFromLoansWindow()
        {
            InitializeComponent();
            _viewModel = new UnearnedInterestFromLoansViewModel();
            DataContext = _viewModel;

            SetupButton.Click += OnSetup;
            RefreshButton.Click += (sender, e) => OnRefresh();
            PostButton.Click += (sender, e) => PostOnClick();
        }

        private void OnSetup(object sender, RoutedEventArgs e)
        {
            var view = new UnearnedInterestFromLoansSetupView();
            view.ShowDialog();
        }

        private void OnRefresh()
        {
            _viewModel.InitializeData();
            DataContext = _viewModel;
            UpdateTotals();
        }

        private void PostOnClick()
        {
            // check if share capital is set
            var shareCapitalCode = GlobalSettings.CodeOfUnearnedIncome;
            if (string.IsNullOrEmpty(shareCapitalCode))
            {
                MessageWindow.ShowAlertMessage("Paid Up Share Capital - Common is not set.");
                return;
            }
            var shareCapitalRequiredAmount = GlobalSettings.AmountOfShareCapitalRequiredBalance;

            // check if can post transaction
            var postingDate = MainController.LoggedUser.TransactionDate;
            if (postingDate != GlobalSettings.DateOfOpenTransaction)
            {
                MessageWindow.ShowAlertMessage("Transaction Date is locked for editing.");
                return;
            }

            if (_viewModel.Collection == null)
            {
                MessageWindow.ShowAlertMessage("There are no items to process. Click Refresh to reload data.");
                return;
            }
            // get list of regular members using their share capital
            _memberShareCapitalAccountSummary = AccountSummary.PerAccount(shareCapitalCode, postingDate);

            var view = new PostJournalVoucherView(postingDate);
            if (view.ShowDialog() == true)
            {
                // Debit Unearned Income from borrowers
                var unearnedIncome = Account.FindByCode(GlobalSettings.CodeOfUnearnedIncome);

                // Credit the Unearned Income account (regular members)
                var interestIncomeFromLoans = Account.FindByCode(GlobalSettings.CodeOfInterestIncomeFromLoans);

                // Credit the Miscellaneous Income account (non-regular members)
                var miscellaneousIncome = Account.FindByCode(GlobalSettings.CodeOfMiscellaneousIncome);

                var jvDefault = view.JournalVoucher;


                foreach (var item in _viewModel.Collection)
                {
                    if (!item.Flag) continue;

                    // debit side (unearned income)
                    var entry = new JournalVoucher
                    {
                        MemberCode = item.MemberCode,
                        MemberName = item.MemberName,
                        VoucherDate = jvDefault.VoucherDate,
                        VoucherNo = jvDefault.VoucherNo,
                        VoucherType = jvDefault.VoucherType,
                        IsPosted = true,
                        AccountCode = unearnedIncome.AccountCode,
                        AccountTitle = unearnedIncome.AccountTitle,
                        Debit = item.InterestAmortization,
                        Credit = new decimal()
                    };
                    entry.Create();

                    // credit side - check membership based on share capital
                    if (IsRegularMember(item.MemberCode, shareCapitalRequiredAmount))
                    {
                        entry.AccountCode = interestIncomeFromLoans.AccountCode;
                        entry.AccountTitle = interestIncomeFromLoans.AccountTitle;
                    }
                    else
                    {
                        entry.AccountCode = miscellaneousIncome.AccountCode;
                        entry.AccountTitle = miscellaneousIncome.AccountTitle;
                    }

                    entry.Debit = new decimal();
                    entry.Credit = item.InterestAmortization;
                    entry.Create();
                }

                MessageWindow.ShowNotifyMessage("Posting of Unearned Interest from Loans successful.");
            }
        }

        private bool IsRegularMember(string memberCode, decimal requiredBalance)
        {
            // we do not need to check if AccountSummary is null because by default it is an empty collection
            if (_memberShareCapitalAccountSummary.Any())
            {
                var memberShare = _memberShareCapitalAccountSummary.FirstOrDefault(t => t.MemberCode == memberCode);
                if (memberShare == null) return false;
                return memberShare.Balance >= requiredBalance;
            }
            return false;
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedItem != null)
            {
                _viewModel.SelectedItem.Flag = !_viewModel.SelectedItem.Flag;
                UpdateTotals();
            }
        }

        private void UpdateTotals()
        {
            var selected = from item in _viewModel.Collection
                           where item.Flag
                           select item;

            TotalLabel.Content = string.Format("Selected: {0} of {1}", selected.Count(), _viewModel.Collection.Count);
        }
    }
}