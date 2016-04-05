using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    internal class GlobalVariablesViewModel : INotifyPropertyChanged
    {
        private GlobalVariable _codeOfCapitalBuildUp;
        private GlobalVariable _codeOfCashOnHand;
        private GlobalVariable _codeOfCompany;
        private GlobalVariable _codeOfInterestExpenseOnSavingsDeposit;
        private GlobalVariable _codeOfInterestIncomeFromLoans;
        private GlobalVariable _codeOfLoanReceivables;
        private GlobalVariable _codeOfMiscellaneousIncome;
        private GlobalVariable _codeOfSalaryAdvance;
        private GlobalVariable _codeOfSavingsDeposit;
        private GlobalVariable _codeOfTimeDeposit;
        private GlobalVariable _codeOfUnearnedIncome;
        private GlobalVariable _rateOfTimeDepositServiceFee;
        private GlobalVariable _codeOfFinesAndPenalty;
        private GlobalVariable _rateOfFinesAndPenalty;

        public event PropertyChangedEventHandler PropertyChanged;

        public GlobalVariable CodeOfCapitalBuildUp
        {
            get { return _codeOfCapitalBuildUp; }
            set
            {
                _codeOfCapitalBuildUp = value;
                OnPropertyChanged("CodeOfCapitalBuildUp");
            }
        }

        public GlobalVariable CodeOfCashOnHand
        {
            get { return _codeOfCashOnHand; }
            set
            {
                _codeOfCashOnHand = value;
                OnPropertyChanged("CodeOfCashOnHand");
            }
        }

        public GlobalVariable CodeOfCompany
        {
            get { return _codeOfCompany; }
            set
            {
                _codeOfCompany = value;
                OnPropertyChanged("CodeOfCompany");
            }
        }

        public GlobalVariable CodeOfInterestExpenseOnSavingsDeposit
        {
            get { return _codeOfInterestExpenseOnSavingsDeposit; }
            set
            {
                _codeOfInterestExpenseOnSavingsDeposit = value;
                OnPropertyChanged("CodeOfInterestExpenseOnSavingsDeposit");
            }
        }

        public GlobalVariable CodeOfInterestIncomeFromLoans
        {
            get { return _codeOfInterestIncomeFromLoans; }
            set
            {
                _codeOfInterestIncomeFromLoans = value;
                OnPropertyChanged("CodeOfInterestIncomeFromLoans");
            }
        }

        public GlobalVariable CodeOfLoanReceivables
        {
            get { return _codeOfLoanReceivables; }
            set
            {
                _codeOfLoanReceivables = value;
                OnPropertyChanged("CodeOfLoanReceivables");
            }
        }

        public GlobalVariable CodeOfMiscellaneousIncome
        {
            get { return _codeOfMiscellaneousIncome; }
            set
            {
                _codeOfMiscellaneousIncome = value;
                OnPropertyChanged("CodeOfMiscellaneousIncome");
            }
        }

        public GlobalVariable CodeOfSalaryAdvance
        {
            get { return _codeOfSalaryAdvance; }
            set
            {
                _codeOfSalaryAdvance = value;
                OnPropertyChanged("CodeOfSalaryAdvance");
            }
        }

        public GlobalVariable CodeOfSavingsDeposit
        {
            get { return _codeOfSavingsDeposit; }
            set
            {
                _codeOfSavingsDeposit = value;
                OnPropertyChanged("CodeOfSavingsDeposit");
            }
        }

        public GlobalVariable CodeOfTimeDeposit
        {
            get { return _codeOfTimeDeposit; }
            set
            {
                _codeOfTimeDeposit = value;
                OnPropertyChanged("CodeOfTimeDeposit");
            }
        }

        public GlobalVariable CodeOfUnearnedIncome
        {
            get { return _codeOfUnearnedIncome; }
            set
            {
                _codeOfUnearnedIncome = value;
                OnPropertyChanged("CodeOfUnearnedIncome");
            }
        }

        public GlobalVariable CodeOfFinesAndPenalty
        {
            get { return _codeOfFinesAndPenalty; }
            set
            {
                _codeOfFinesAndPenalty = value;
                OnPropertyChanged("CodeOfFinesAndPenalty");
            }
        }

        public GlobalVariable RateOfTimeDepositServiceFee
        {
            get { return _rateOfTimeDepositServiceFee; }
            set
            {
                _rateOfTimeDepositServiceFee = value;
                OnPropertyChanged("RateOfTimeDepositServiceFee");
            }
        }

        public GlobalVariable RateOfFinesAndPenalty
        {
            get { return _rateOfFinesAndPenalty; }
            set { _rateOfFinesAndPenalty = value;
                OnPropertyChanged("RateOfFinesAndPenalty"); }
        }

        public void Initialize()
        {
            CodeOfCapitalBuildUp = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfCapitalBuildUp);
            CodeOfCashOnHand = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfCashOnHand);
            CodeOfCompany = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfCompany);
            CodeOfInterestExpenseOnSavingsDeposit =
                GlobalVariable.FindByKeyword(GlobalKeys.CodeOfInterestExpenseOnSavingsDeposit);
            CodeOfInterestIncomeFromLoans = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfInterestIncomeFromLoans);
            CodeOfLoanReceivables = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfLoanReceivables);
            CodeOfMiscellaneousIncome = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfMiscellaneousIncome);
            CodeOfSalaryAdvance = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfSalaryAdvance);
            CodeOfSavingsDeposit = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfSavingsDeposit);
            CodeOfTimeDeposit = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfTimeDeposit);
            CodeOfUnearnedIncome = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfUnearnedIncome);
            RateOfTimeDepositServiceFee = GlobalVariable.FindByKeyword(GlobalKeys.RateOfTimeDepositServiceFee);

            CodeOfFinesAndPenalty = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfFinesAndPenalty);
            RateOfFinesAndPenalty = GlobalVariable.FindByKeyword(GlobalKeys.RateOfFines);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}