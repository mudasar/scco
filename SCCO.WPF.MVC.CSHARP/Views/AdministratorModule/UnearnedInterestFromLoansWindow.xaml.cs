using System;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    /// <summary>
    /// Interaction logic for UnearnedInterestFromLoansWindow.xaml
    /// 
    /// 1. Get the monthly amortization of "Unearned Interest from Loans" per currrent/non-settled loan
    ///     - Unearned Interest / Monthly Payments
    /// 2. Post in voucher:
    ///     - Debit: Unearned Interest
    ///     - Credit - Interest on Loan / Miscellaneous Income if Associate Member
    /// 
    /// </summary>
    public partial class UnearnedInterestFromLoansWindow
    {
        private readonly UnearnedInterestFromLoansViewModel _viewModel;
        public UnearnedInterestFromLoansWindow()
        {
            InitializeComponent();

            btnPost.Click += (sender, e) => PostOnClick();
            _viewModel = new UnearnedInterestFromLoansViewModel();
            _viewModel.InitializeData();
            DataContext = _viewModel;

        }

        private void PostOnClick()
        {
            var postingDate = Controllers.MainController.LoggedUser.TransactionDate;
            var view = new PostJournalVoucherView(postingDate);
            if(view.ShowDialog() == true)
            {
                Console.WriteLine(view.JournalVoucher.ToString());
                // Debit Unearned Income from borrowers
                var ui = Models.GlobalVariable.FindByKeyword("CodeOfUnearnedIncome");
                var uiAccount = Models.Account.FindByCode(ui.CurrentValue);

                // Credit the Unearned Income account (regular members)
                var il = Models.GlobalVariable.FindByKeyword("CodeOfInterestIncomeFromLoans");
                var ilAccount = Models.Account.FindByCode(il.CurrentValue);

                // Credit the Miscellaneous Income account (non-regular members)
                var mi = Models.GlobalVariable.FindByKeyword("CodeOfMiscellaneousIncome");
                var miAccount = Models.Account.FindByCode(mi.CurrentValue);

                var mt = Models.MembershipType.GetList().Find(m => m.ID == 1);
                if(mt == null)
                {
                    MessageWindow.ShowAlertMessage("Membership Types are not properly set! Please check.");
                    return;
                }

                var regularMember = mt.Description.ToUpper();

                var jvDefault = view.JournalVoucher;

                var index = 0;
                var total = _viewModel.Collection.Count;

                //progressBar1.Maximum = total;

                foreach (var item in _viewModel.Collection)
                {
                    if(!item.Flag) continue;

                    var entry = new Models.JournalVoucher();
                    entry.MemberCode = item.MemberCode;
                    entry.MemberName = item.MemberName;

                    entry.VoucherDate = jvDefault.VoucherDate;
                    entry.VoucherNo = jvDefault.VoucherNo;
                    entry.VoucherType = jvDefault.VoucherType;

                    entry.IsPosted = true;

                    // debit side (unearned income)
                    entry.AccountCode = uiAccount.AccountCode;
                    entry.AccountTitle = uiAccount.AccountTitle;
                    entry.Debit = item.InterestAmortization;
                    entry.Credit = new decimal();
                    entry.Create();

                    // credit side
                    var member = Models.Nfmb.FindByCode(item.MemberCode);
                    if(member.MembershipType.ToUpper()==regularMember)
                    {
                        entry.AccountCode = ilAccount.AccountCode;
                        entry.AccountTitle = ilAccount.AccountTitle;
                    }
                    else
                    {
                        entry.AccountCode = miAccount.AccountCode;
                        entry.AccountTitle = miAccount.AccountTitle;
                    }

                    entry.Debit = new decimal();
                    entry.Credit = item.InterestAmortization;
                    entry.Create();

                    index++;
                    Console.WriteLine(@"Processed: {0}", index);
                    // progressBar1.Value = index;
                }

                MessageWindow.ShowNotifyMessage("Posting of Unearned Interest from Loans successful.");
            }
        }

        private void dataGrid1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedItem != null)
            _viewModel.SelectedItem.Flag = !_viewModel.SelectedItem.Flag;
        }
    }
}
