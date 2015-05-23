using System;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Models.SavingsDeposit
{
    public class DailyWithdrawalSettings : INotifyPropertyChanged
    {
        private decimal _totalWithdrawableAmount;
        private DateTime _transactionDate;
        private decimal _withdrawableAmount;
        private decimal _maintainingBalance;
        private int _withdrawalVoucherNo;

        public Result InitializeProperties()
        {
            try
            {
                //CashVoucherNo = GlobalVariable.CashVoucherNo;
                //TransactionDate = GlobalSettings.DateOfOpenTransaction;
                //TotalWithdrawableAmount = GlobalVariable.TotalWithdrawableAmount;
                //MaintainingBalance = GlobalVariable.MaintainingBalance;
                //WithdrawableAmount = GlobalVariable.WithdrawableAmount;

                WithdrawalVoucherNo = GlobalSettings.NumberOfSavingsDepositWithdrawalCashVoucher;
                TransactionDate = GlobalSettings.DateOfSavingsDepositWithdrawalCashVoucher;
                MaximumDailyWithdrawals = GlobalSettings.AmountOfSavingsDepositMaximumDailyWithdrawals;
                MaintainingBalance = GlobalSettings.AmountOfSavingsDepositMaintainingBalance;
                WithdrawableAmount = GlobalSettings.AmountOfSavingsDepositWithdrawable;

                return new Result(true, "Success");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal MaximumDailyWithdrawals
        {
            get { return _totalWithdrawableAmount; }
            set
            {
                _totalWithdrawableAmount = value;
                OnPropertyChanged("TotalWithdrawableAmount");
            }
        }

        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set
            {
                _transactionDate = value;
                OnPropertyChanged("TransactionDate");
            }
        }

        public decimal WithdrawableAmount
        {
            get { return _withdrawableAmount; }
            set
            {
                _withdrawableAmount = value;
                OnPropertyChanged("WithdrawableAmount");
            }
        }

        public decimal MaintainingBalance
        {
            get { return _maintainingBalance; }
            set
            {
                _maintainingBalance = value;
                OnPropertyChanged("MaintainingBalance");
            }
        }

        public int WithdrawalVoucherNo
        {
            get { return _withdrawalVoucherNo; }
            set
            {
                _withdrawalVoucherNo = value;
                OnPropertyChanged("CashVoucherNo");
            }
        }

        public Result Update()
        {
            try
            {
                //var sqlCommand = string.Format("UPDATE `global_variables` SET CurrentValue = ?CurrentValue WHERE Keyword = ?Keyword");

                ////CashVoucherNo
                //var currentValue = new SqlParameter("?CurrentValue", CashVoucherNo);
                //var keyword = new SqlParameter("?Keyword", "CashVoucherNo");
                //DatabaseController.ExecuteNonQuery(sqlCommand, currentValue, keyword);

                ////TransactionDate
                //currentValue = new SqlParameter("?CurrentValue", TransactionDate);
                //keyword = new SqlParameter("?Keyword", "TransactionDate");
                //DatabaseController.ExecuteNonQuery(sqlCommand, currentValue, keyword);

                ////TotalWithdrawableAmount
                //currentValue = new SqlParameter("?CurrentValue", TotalWithdrawableAmount);
                //keyword = new SqlParameter("?Keyword", "TotalWithdrawableAmount");
                //DatabaseController.ExecuteNonQuery(sqlCommand, currentValue, keyword);

                ////MaintainingBalance
                //currentValue = new SqlParameter("?CurrentValue", MaintainingBalance);
                //keyword = new SqlParameter("?Keyword", "MaintainingBalance");
                //DatabaseController.ExecuteNonQuery(sqlCommand, currentValue, keyword);

                ////WithdrawableAmount
                //currentValue = new SqlParameter("?CurrentValue", WithdrawableAmount);
                //keyword = new SqlParameter("?Keyword", "WithdrawableAmount");
                //DatabaseController.ExecuteNonQuery(sqlCommand, currentValue, keyword);

                GlobalSettings.Update(
                    GlobalKeys.NumberOfSavingsDepositWithdrawalCashVoucher.ToKeyword(),
                    _withdrawalVoucherNo);

                GlobalSettings.Update(
                    GlobalKeys.DateOfSavingsDepositWithdrawalCashVoucher.ToKeyword(), _transactionDate);

                GlobalSettings.Update(
                    GlobalKeys.AmountOfSavingsDepositMaximumDailyWithdrawals.ToKeyword(),
                    _totalWithdrawableAmount);

                GlobalSettings.Update(
                    GlobalKeys.AmountOfSavingsDepositMaintainingBalance.ToKeyword(),
                    _maintainingBalance);

                GlobalSettings.Update(
                    GlobalKeys.AmountOfSavingsDepositWithdrawable.ToKeyword(),
                    _withdrawableAmount);

                return new Result(true, "Withdrawal setup updated");
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}