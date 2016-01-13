using System;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    internal class SalaryAdvanceSetupViewModel : INotifyPropertyChanged
    {
        private Account _cashOnHandAccount;
        private Account _miscellaneousIncomeAccount;
        private Account _salaryAdvanceAccount;

        public Account SalaryAdvanceAccount
        {
            get { return _salaryAdvanceAccount; }
            set
            {
                _salaryAdvanceAccount = value;
                OnPropertyChanged("SalaryAdvanceAccount");
            }
        }

        public Account MiscellaneousIncomeAccount
        {
            get { return _miscellaneousIncomeAccount; }
            set
            {
                _miscellaneousIncomeAccount = value;
                OnPropertyChanged("MiscellaneousIncomeAccount");
            }
        }

        public Account CashOnHandAccount
        {
            get { return _cashOnHandAccount; }
            set
            {
                _cashOnHandAccount = value;
                OnPropertyChanged("CashOnHandAccount");
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
            SalaryAdvanceAccount = Account.FindByCode(GlobalSettings.CodeOfSalaryAdvance);
            MiscellaneousIncomeAccount = Account.FindByCode(GlobalSettings.CodeOfMiscellaneousIncome);
            CashOnHandAccount = Account.FindByCode(GlobalSettings.CodeOfCashOnHand);
        }

        public Result Update()
        {
            try
            {
                GlobalSettings.Update(
                    GlobalKeys.CodeOfSalaryAdvance.ToKeyword(), _salaryAdvanceAccount.AccountCode);

                GlobalSettings.Update(
                    GlobalKeys.CodeOfMiscellaneousIncome.ToKeyword(), _miscellaneousIncomeAccount.AccountCode);

                GlobalSettings.Update(
                    GlobalKeys.CodeOfCashOnHand.ToKeyword(), _cashOnHandAccount.AccountCode);

                return new Result(true, "Salary Advance setup updated");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }
    }
}