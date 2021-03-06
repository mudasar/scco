﻿using System;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    internal class TimeDepositSetupViewModel : INotifyPropertyChanged
    {
        private Account _serviceFeeAccount;
        private decimal _serviceFeeRate;
        private decimal _minimumServiceFeeApplied;
        private Account _interestExpenseAccount;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Account ServiceFeeAccount
        {
            get { return _serviceFeeAccount; }
            set { _serviceFeeAccount = value; OnPropertyChanged("ServiceFeeAccount");}
        }

        public decimal ServiceFeeRate
        {
            get { return _serviceFeeRate; }
            set { _serviceFeeRate = value; OnPropertyChanged("ServiceFeeRate"); }
        }

        public decimal MinimumServiceFeeApplied
        {
            get { return _minimumServiceFeeApplied; }
            set { _minimumServiceFeeApplied = value; OnPropertyChanged("MinimumServiceFeeApplied"); }
        }

        public Account InterestExpenseAccount
        {
            get { return _interestExpenseAccount; }
            set { _interestExpenseAccount = value; OnPropertyChanged("InterestExpenseAccount"); }
        }

        public void InitializeProperties()
        {
            ServiceFeeAccount = Account.FindByCode(GlobalSettings.CodeOfServiceFee);
            ServiceFeeRate = GlobalSettings.RateOfTimeDepositServiceFee;
            MinimumServiceFeeApplied = GlobalSettings.AmountOfMinimumTimeDepositServiceFee;
            InterestExpenseAccount = Account.FindByCode(GlobalSettings.CodeOfInterestExpenseOnTimeDeposit);
        }

        public Result Update()
        {
            try
            {
                GlobalSettings.Update(
                    GlobalKeys.CodeOfInterestExpenseOnTimeDeposit.ToKeyword(),
                    _interestExpenseAccount.AccountCode);

                GlobalSettings.Update(
                    GlobalKeys.CodeOfServiceFee.ToKeyword(), _serviceFeeAccount.AccountCode);

                GlobalSettings.Update(
                    GlobalKeys.RateOfTimeDepositServiceFee.ToKeyword(),
                    _serviceFeeRate);

                GlobalSettings.Update(
                    GlobalKeys.AmountOfMinimumTimeDepositServiceFee.ToKeyword(),
                    _minimumServiceFeeApplied);

                return new Result(true, "Time Deposit setup updated");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }
    }
}
