using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public class UnearnedInterestFromLoansPostingSetupViewModel : ViewModelBase
    {
        private string _codeOfShareCapital;
        private decimal _amountOfShareCapitalRequiredBalance;
        private string _codeOfUnearnedIncome;
        private string _codeOfInterestIncomeFromLoans;
        private string _codeOfMiscellaneousIncome;

        public void InitializeData()
        {
            CodeOfShareCapital = GlobalSettings.CodeOfShareCapital;
            CodeOfInterestIncomeFromLoans = GlobalSettings.CodeOfInterestIncomeFromLoans;
            CodeOfMiscellaneousIncome = GlobalSettings.CodeOfMiscellaneousIncome;
            CodeOfUnearnedIncome = GlobalSettings.CodeOfUnearnedIncome;
            AmountOfShareCapitalRequiredBalance = GlobalSettings.AmountOfShareCapitalRequiredBalance;
        }

        public string CodeOfShareCapital
        {
            get { return _codeOfShareCapital; }
            set
            {
                _codeOfShareCapital = value;
                OnPropertyChanged("CodeOfShareCapital");
            }
        }

        public decimal AmountOfShareCapitalRequiredBalance
        {
            get { return _amountOfShareCapitalRequiredBalance; }
            set
            {
                _amountOfShareCapitalRequiredBalance = value;
                OnPropertyChanged("AmountOfShareCapitalRequiredBalance");
            }
        }

        public string CodeOfUnearnedIncome
        {
            get { return _codeOfUnearnedIncome; }
            set
            {
                _codeOfUnearnedIncome = value;
                OnPropertyChanged("CodeOfUnearnedIncome");
            }
        }

        public string CodeOfInterestIncomeFromLoans
        {
            get { return _codeOfInterestIncomeFromLoans; }
            set
            {
                _codeOfInterestIncomeFromLoans = value;
                OnPropertyChanged("CodeOfInterestIncomeFromLoans");
            }
        }

        public string CodeOfMiscellaneousIncome
        {
            get { return _codeOfMiscellaneousIncome; }
            set
            {
                _codeOfMiscellaneousIncome = value;
                OnPropertyChanged("CodeOfMiscellaneousIncome");
            }
        }
    }
}
