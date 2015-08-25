using System.ComponentModel;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models.Loan;
using System.Linq;
using System;

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

        public event PropertyChangedEventHandler PropertyChanged;

        public OutstandingLoans Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
            }
        }

        public OutstandingLoan SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value; OnPropertyChanged("SelectedItem");
            }
        }

        public void InitializeData()
        {
            // Get list of unearned incomes
            var asOf = MainController.LoggedUser.TransactionDate;
            var code = Models.GlobalVariable.FindByKeyword("CodeOfUnearnedIncome");
            var unearnedIncomes = Models.AccountVerifier.AccountSummary.PerAccount(code.CurrentValue, asOf);

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
                if(unearnedIncomes.Any(ui => ui.MemberCode == memberCode))
                {
                    Collection.Add(loan);
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
