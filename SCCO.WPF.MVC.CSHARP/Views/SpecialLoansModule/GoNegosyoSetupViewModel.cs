using System;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    internal class GoNegosyoSetupViewModel : INotifyPropertyChanged
    {
        private Account _apMerchandiseAccount;
        private Account _goNegosyoAccount;

        public Account GoNegosyoAccount
        {
            get { return _goNegosyoAccount; }
            set
            {
                _goNegosyoAccount = value;
                OnPropertyChanged("GoNegosyoAccount");
            }
        }

        public Account AccountsPayableMerchandiseAccount
        {
            get { return _apMerchandiseAccount; }
            set
            {
                _apMerchandiseAccount = value;
                OnPropertyChanged("AccountsPayableMerchandiseAccount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InitializeProperties()
        {
            GoNegosyoAccount = Account.FindByCode(GlobalSettings.CodeOfGoNegosyo);
            AccountsPayableMerchandiseAccount = Account.FindByCode(GlobalSettings.CodeOfAccountsPayableMerchandise);
        }

        public Result Update()
        {
            try
            {
                GlobalSettings.Update(
                    GlobalKeys.CodeOfGoNegosyo.ToKeyword(), _goNegosyoAccount.AccountCode);

                GlobalSettings.Update(
                    GlobalKeys.CodeOfAccountsPayableMerchandise.ToKeyword(), _apMerchandiseAccount.AccountCode);

                return new Result(true, "Go Negosyo setup updated");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }
    }
}