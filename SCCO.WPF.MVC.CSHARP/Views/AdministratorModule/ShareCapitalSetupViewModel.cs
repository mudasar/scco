using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    internal class ShareCapitalSetupViewModel : ViewModelBase
    {
        private const string _minimumBalanceKeyword = "AmountOfShareCapitalRequiredBalance";
        private const string _shareCapitalCodeKeyword = "CodeOfShareCapital";
        private string _accountCode;
        private decimal _requiredBalance;

        public string AccountCode
        {
            get { return _accountCode; }
            set
            {
                _accountCode = value;
                OnPropertyChanged("AccountCode");
            }
        }

        public decimal RequiredBalance
        {
            get { return _requiredBalance; }
            set
            {
                _requiredBalance = value;
                OnPropertyChanged("RequiredBalance");
            }
        }

        public void Update()
        {
            GlobalSettings.Update(_shareCapitalCodeKeyword, AccountCode);
            GlobalSettings.Update(_minimumBalanceKeyword, RequiredBalance);
        }

        public ShareCapitalSetupViewModel()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            RequiredBalance = GlobalSettings.AmountOfShareCapitalRequiredBalance;
            AccountCode = GlobalSettings.CodeOfShareCapital;
        }
    }
}