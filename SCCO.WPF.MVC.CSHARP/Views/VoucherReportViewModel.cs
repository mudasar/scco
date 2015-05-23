using System;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    class VoucherReportViewModel
    {
        private CollectorCollection _collectors;
        private Collector _selectedCollector;
        private string _accountCode;
        private DateTime _transactionDate;
        private int _reportRangeOption;
        private VoucherTypes _voucherType;
        private DateTime[] _dateRange;
        private int _voucherNumber;

        public CollectorCollection Collectors
        {
            get { return _collectors; }
            set
            {
                if (_collectors == value) return;
                _collectors = value; OnPropertyChanged("Collectors");
            }
        }

        public Collector SelectedCollector
        {
            get { return _selectedCollector; }
            set
            {
                if (_selectedCollector == value) return;
                _selectedCollector = value; OnPropertyChanged("SelectedCollector");
            }
        }

        public string AccountCode
        {
            get { return _accountCode; }
            set { _accountCode = value; OnPropertyChanged("AccountCode");}
        }

        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { _transactionDate = value; OnPropertyChanged("TransactionDate"); }
        }

        public int ReportRangeOption
        {
            get { return _reportRangeOption; }
            set { _reportRangeOption = value; OnPropertyChanged("ReportRangeOption"); }
            // 0 - for the day
            // 1 - for the month
        }

        public DateTime[] DateRange
        {
            get { return _dateRange; }
            set { _dateRange = value; OnPropertyChanged("DateRange");
            }
        }

        public void Initialize()
        {
            Collectors = CollectorCollection.SortedByName();
            TransactionDate = Controllers.MainController.LoggedUser.TransactionDate;
            UpdateReportRange();
        }

        public void UpdateReportRange()
        {
            DateTime dateStart = TransactionDate;
            DateTime dateEnd = TransactionDate;

            if(ReportRangeOption == 1)
            {
                int year = TransactionDate.Year;
                int month = TransactionDate.Month;
                int days = DateTime.DaysInMonth(year, month);

                dateStart = new DateTime(year, month, 1);
                dateEnd = new DateTime(year, month, days);
           }
            DateRange = new[]{dateStart, dateEnd};
        }

        public VoucherTypes VoucherType
        {
            get { return _voucherType; }
            set { _voucherType = value; OnPropertyChanged("VoucherType"); }
        }

        public int VoucherNumber
        {
            get { return _voucherNumber; }
            set { _voucherNumber = value; OnPropertyChanged("VoucherNumber"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
