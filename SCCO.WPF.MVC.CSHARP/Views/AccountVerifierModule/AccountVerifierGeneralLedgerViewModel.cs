using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    public class AccountVerifierGeneralLedgerViewModel
    {
        private GeneralLedgerAccountCollection _collection;
        private GeneralLedgerAccount _selectedItem;
        private Account _account;
        private decimal _endBalance;
        private decimal _totalDebit;
        private decimal _totalCredit;

        public GeneralLedgerAccountCollection Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value; OnPropertyChanged("Collection");
            }
        }

        public GeneralLedgerAccount SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value; OnPropertyChanged("SelectedItem");
            }
        }

        public Account Account
        {
            get { return _account; }
            set { _account = value; OnPropertyChanged("Account"); }
        }

        public Decimal EndBalance
        {
            get { return _endBalance; }
            set { _endBalance = value; OnPropertyChanged("EndBalance"); }
        }

        public Decimal TotalDebit
        {
            get { return _totalDebit; }
            set { _totalDebit = value; OnPropertyChanged("TotalDebit"); }
        }

        public Decimal TotalCredit
        {
            get { return _totalCredit; }
            set { _totalCredit = value; OnPropertyChanged("TotalCredit"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public static AccountVerifierGeneralLedgerViewModel Refresh(Account account, DateTime asOf)
        {
            const string sp = "sp_account_verifier_general_ledger";
            var sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("ts_account_code", account.AccountCode);
            sqlparams[1] = new SqlParameter("td_as_of", asOf);
            var dataTable = DatabaseController.ExecuteStoredProcedure(sp, sqlparams);

            var collection = new GeneralLedgerAccountCollection();
            var endBalance = 0m;
            var totalDebit = 0m;
            var totalCredit = 0m;
            foreach (System.Data.DataRow dataRow in dataTable.Rows)
            {
                var item = new GeneralLedgerAccount();
                item.AccountCode = account.AccountCode;
                item.AccountTitle = account.AccountTitle;
                item.VoucherDate = Utilities.DataConverter.ToDateTime(dataRow["voucher_date"]);
                item.VoucherType = Utilities.DataConverter.ToString(dataRow["voucher_type"]);
                item.VoucherNumber = Utilities.DataConverter.ToInteger(dataRow["voucher_number"]);
                item.Debit = Utilities.DataConverter.ToDecimal(dataRow["debit"]);
                item.Credit = Utilities.DataConverter.ToDecimal(dataRow["credit"]);
                item.Balance = Utilities.DataConverter.ToDecimal(dataRow["balance"]);
                item.CheckNumber = Utilities.DataConverter.ToString(dataRow["check_number"]);

                endBalance = item.Balance;
                totalCredit += item.Credit;
                totalDebit += item.Debit;
                collection.Add(item);
            }
            var viewModel = new AccountVerifierGeneralLedgerViewModel();
            viewModel.Collection = collection;
            viewModel.Account = account;
            viewModel.EndBalance = endBalance;
            viewModel.TotalCredit = totalCredit;
            viewModel.TotalDebit = totalDebit;
            return viewModel;

        }
    }

    public class GeneralLedgerAccount : INotifyPropertyChanged
    {
        private bool _marked;
        private string _accountCode;
        private string _accountTitle;
        private DateTime _voucherDate;
        private string _voucherType;
        private int _voucherNumber;
        private decimal _debit;
        private decimal _credit;
        private decimal _balance;
        private string _checkNumber;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Marked
        {
            get { return _marked; }
            set { _marked = value; OnPropertyChanged("Marked"); }
        }

        public string AccountCode
        {
            get { return _accountCode; }
            set { _accountCode = value; OnPropertyChanged("AccountCode"); }
        }

        public string AccountTitle
        {
            get { return _accountTitle; }
            set { _accountTitle = value; OnPropertyChanged("AccountTitle"); }
        }

        public System.DateTime VoucherDate
        {
            get { return _voucherDate; }
            set { _voucherDate = value; OnPropertyChanged("VoucherDate"); }
        }

        public string VoucherType
        {
            get { return _voucherType; }
            set { _voucherType = value; OnPropertyChanged("VoucherType"); }
        }

        public int VoucherNumber
        {
            get { return _voucherNumber; }
            set { _voucherNumber = value; OnPropertyChanged("VoucherNumber"); }
        }

        public decimal Debit
        {
            get { return _debit; }
            set { _debit = value; OnPropertyChanged("Debit"); }
        }

        public decimal Credit
        {
            get { return _credit; }
            set { _credit = value; OnPropertyChanged("Credit"); }
        }

        public decimal Balance
        {
            get { return _balance; }
            set { _balance = value; OnPropertyChanged("Balance"); }
        }

        public string CheckNumber
        {
            get { return _checkNumber; }
            set { _checkNumber = value; OnPropertyChanged("CheckNumber"); }
        }

        public string Reference { get { return string.Format("{0} {1}", VoucherType, VoucherNumber); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class GeneralLedgerAccountCollection : ObservableCollection<GeneralLedgerAccount>
    {
    }
}