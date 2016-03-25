using System.ComponentModel;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    // 1. Get account with Unearned Income
    // 2. Get the monthly amortization
    // 3. Determine which account to credit
    //    - Interest Income From Loan - Regular Member
    //    - Miscellaneous Income - Associate Loaning Member
    public class UnearnedInterestFromLoansViewModel : INotifyPropertyChanged
    {
        private OutstandingLoans _collection;
        private OutstandingLoan _selectedItem;

        public OutstandingLoans Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value;
                OnPropertyChanged("Collection");
            }
        }

        public OutstandingLoan SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void InitializeData()
        {
            var asOf = MainController.LoggedUser.TransactionDate;
            // Get list of unearned incomes
            var unearnedIncomes = AccountSummary.PerAccount(GlobalSettings.CodeOfUnearnedIncome, asOf);

            // Get list of loans
            var loanAccounts = OutstandingLoans.AsOf(asOf);

            // get list of loans that found in list of unearned incomes
            Collection = new OutstandingLoans();
            foreach (var loan in loanAccounts)
            {
                // process only active
                if (loan.EndingBalance <= 0) continue;

                // do not process overdue
                if (loan.MaturityDate < asOf) continue;

                // do not process loan if term is one month or less
                if ((loan.MaturityDate - loan.GrantedDate).TotalDays <= 31) continue;

                var memberCode = loan.MemberCode;

                // must have unearned income
                var unearnedIncome = unearnedIncomes.FirstOrDefault(ui => ui.MemberCode == memberCode);
                if (unearnedIncome != null && unearnedIncome.Balance > 0)
                {
                    Collection.Add(loan);
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}