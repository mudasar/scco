using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    public class AccountVerifierGeneralLedgerViewModel
    {
        private Account _account;
        private GeneralLedgerAccountCollection _collection;
        private decimal _endBalance;
        private GeneralLedgerAccount _selectedItem;
        private decimal _totalCredit;
        private decimal _totalDebit;

        public GeneralLedgerAccountCollection Collection
        {
            get { return _collection; }
            set
            {
                if (_collection == value) return;
                _collection = value;
                OnPropertyChanged("Collection");
            }
        }

        public GeneralLedgerAccount SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public Account Account
        {
            get { return _account; }
            set
            {
                _account = value;
                OnPropertyChanged("Account");
            }
        }

        public Decimal EndBalance
        {
            get { return _endBalance; }
            set
            {
                _endBalance = value;
                OnPropertyChanged("EndBalance");
            }
        }

        public Decimal TotalDebit
        {
            get { return _totalDebit; }
            set
            {
                _totalDebit = value;
                OnPropertyChanged("TotalDebit");
            }
        }

        public Decimal TotalCredit
        {
            get { return _totalCredit; }
            set
            {
                _totalCredit = value;
                OnPropertyChanged("TotalCredit");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public static AccountVerifierGeneralLedgerViewModel Refresh(Account account, DateTime asOf)
        {
            string storedProcedure = "sp_account_verifier_general_ledger";
            var sqlparams = new List<SqlParameter>();


            DateTime forwardedDate = GeneralLedgerBalance.MaxDocumentDate();
            if (forwardedDate != Convert.ToDateTime(string.Format("12/31/{0}", asOf.Year - 1)))
            {
                // present_database VARCHAR(40), previous_database VARCHAR(40), cutoff_date DATE, account_code varchar(10), as_of date
                storedProcedure = "sp_account_verifier_general_ledger_opening_year";
                sqlparams.Add(new SqlParameter("present_database", DatabaseController.GetDatabaseByYear(asOf.Year)));
                sqlparams.Add(new SqlParameter("previous_database",
                                               DatabaseController.GetDatabaseByYear(forwardedDate.Year)));
                sqlparams.Add(new SqlParameter("account_code", account.AccountCode));
                sqlparams.Add(new SqlParameter("start_date", forwardedDate.AddDays(1)));
                sqlparams.Add(new SqlParameter("end_date", asOf));
            }
            else
            {
                sqlparams.Add(new SqlParameter("ts_account_code", account.AccountCode));
                sqlparams.Add(new SqlParameter("td_as_of", asOf));
            }

            DataTable dataTable = DatabaseController.ExecuteStoredProcedure(storedProcedure, sqlparams);

            var collection = new GeneralLedgerAccountCollection();
            decimal endBalance = 0m;
            decimal totalDebit = 0m;
            decimal totalCredit = 0m;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new GeneralLedgerAccount();
                item.AccountCode = account.AccountCode;
                item.AccountTitle = account.AccountTitle;
                item.VoucherDate = DataConverter.ToDateTime(dataRow["voucher_date"]);
                item.VoucherType = DataConverter.ToString(dataRow["voucher_type"]);
                item.VoucherNumber = DataConverter.ToInteger(dataRow["voucher_number"]);
                item.Debit = DataConverter.ToDecimal(dataRow["debit"]);
                item.Credit = DataConverter.ToDecimal(dataRow["credit"]);
                item.Balance = DataConverter.ToDecimal(dataRow["balance"]);
                item.CheckNumber = DataConverter.ToString(dataRow["check_number"]);

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
        private string _accountCode;
        private string _accountTitle;
        private decimal _balance;
        private string _checkNumber;
        private decimal _credit;
        private decimal _debit;
        private bool _marked;
        private DateTime _voucherDate;
        private int _voucherNumber;
        private string _voucherType;

        public bool Marked
        {
            get { return _marked; }
            set
            {
                _marked = value;
                OnPropertyChanged("Marked");
            }
        }

        public string AccountCode
        {
            get { return _accountCode; }
            set
            {
                _accountCode = value;
                OnPropertyChanged("AccountCode");
            }
        }

        public string AccountTitle
        {
            get { return _accountTitle; }
            set
            {
                _accountTitle = value;
                OnPropertyChanged("AccountTitle");
            }
        }

        public DateTime VoucherDate
        {
            get { return _voucherDate; }
            set
            {
                _voucherDate = value;
                OnPropertyChanged("VoucherDate");
            }
        }

        public string VoucherType
        {
            get { return _voucherType; }
            set
            {
                _voucherType = value;
                OnPropertyChanged("VoucherType");
            }
        }

        public int VoucherNumber
        {
            get { return _voucherNumber; }
            set
            {
                _voucherNumber = value;
                OnPropertyChanged("VoucherNumber");
            }
        }

        public decimal Debit
        {
            get { return _debit; }
            set
            {
                _debit = value;
                OnPropertyChanged("Debit");
            }
        }

        public decimal Credit
        {
            get { return _credit; }
            set
            {
                _credit = value;
                OnPropertyChanged("Credit");
            }
        }

        public decimal Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged("Balance");
            }
        }

        public string CheckNumber
        {
            get { return _checkNumber; }
            set
            {
                _checkNumber = value;
                OnPropertyChanged("CheckNumber");
            }
        }

        public string Reference
        {
            get { return string.Format("{0} {1}", VoucherType, VoucherNumber); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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