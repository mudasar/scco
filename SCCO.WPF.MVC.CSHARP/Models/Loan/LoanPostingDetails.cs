using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class LoanPostingDetails : INotifyPropertyChanged
    {
        private VoucherTypes _voucherType;
        private DateTime _voucherDate;
        private int _voucherNumber;
        private int _releaseNumber;
        private DateTime _releaseDate;

        public VoucherTypes VoucherType
        {
            get { return _voucherType; }
            set { _voucherType = value; OnPropertyChanged("VoucherType");}
        }

        public DateTime VoucherDate
        {
            get { return _voucherDate; }
            set { _voucherDate = value; OnPropertyChanged("VoucherDate");}
        }

        public int VoucherNumber
        {
            get { return _voucherNumber; }
            set { _voucherNumber = value; OnPropertyChanged("VoucherNumber");}
        }

        public int ReleaseNumber
        {
            get { return _releaseNumber; }
            set { _releaseNumber = value; OnPropertyChanged("ReleaseNumber"); }
        }

        public DateTime ReleaseDate
        {
            get { return _releaseDate; }
            set { _releaseDate = value; OnPropertyChanged("ReleaseDate"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
