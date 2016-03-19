using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SavingsDepositModule
{
    internal class SavingsDepositInterestPostingSetupViewModel : INotifyPropertyChanged
    {
        private decimal _amountOfInterestOnSavingsDepositRequiredBalance;
        private string _codeOfInterestExpenseOnSavingsDeposit;
        private string _codeOfSavingsDeposit;
        private decimal _rateOfInterestOnSavingsDeposit;

        public decimal RateOfInterestOnSavingsDeposit
        {
            get { return _rateOfInterestOnSavingsDeposit; }
            set
            {
                _rateOfInterestOnSavingsDeposit = value;
                OnPropertyChanged("RateOfInterestOnSavingsDeposit");
            }
        }

        public decimal AmountOfInterestOnSavingsDepositRequiredBalance
        {
            get { return _amountOfInterestOnSavingsDepositRequiredBalance; }
            set
            {
                _amountOfInterestOnSavingsDepositRequiredBalance = value;
                OnPropertyChanged("AmountOfInterestOnSavingsDepositRequiredBalance");
            }
        }

        public string CodeOfInterestExpenseOnSavingsDeposit
        {
            get { return _codeOfInterestExpenseOnSavingsDeposit; }
            set
            {
                _codeOfInterestExpenseOnSavingsDeposit = value;
                OnPropertyChanged("CodeOfInterestExpenseOnSavingsDeposit");
            }
        }

        public string CodeOfSavingsDeposit
        {
            get { return _codeOfSavingsDeposit; }
            set
            {
                _codeOfSavingsDeposit = value;
                OnPropertyChanged("CodeOfSavingsDeposit");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize()
        {
            CodeOfInterestExpenseOnSavingsDeposit = GlobalSettings.CodeOfInterestExpenseOnSavingsDeposit;
            CodeOfSavingsDeposit = GlobalSettings.CodeOfSavingsDeposit;
            AmountOfInterestOnSavingsDepositRequiredBalance =
                GlobalSettings.AmountOfInterestOnSavingsDepositRequiredBalance;
            RateOfInterestOnSavingsDeposit = GlobalSettings.RateOfInterestOnSavingsDeposit;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void Update()
        {
            GlobalSettings.Update(GlobalKeys.CodeOfInterestExpenseOnSavingsDeposit.ToKeyword(),
                                  CodeOfInterestExpenseOnSavingsDeposit);

            GlobalSettings.Update(GlobalKeys.CodeOfSavingsDeposit.ToKeyword(),
                                  CodeOfSavingsDeposit);

            GlobalSettings.Update(GlobalKeys.AmountOfInterestOnSavingsDepositRequiredBalance.ToKeyword(),
                                  AmountOfInterestOnSavingsDepositRequiredBalance);

            GlobalSettings.Update(GlobalKeys.RateOfInterestOnSavingsDeposit.ToKeyword(),
                                  RateOfInterestOnSavingsDeposit);
        }
    }
}