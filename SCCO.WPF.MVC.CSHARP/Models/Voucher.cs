using System;
using System.ComponentModel;
using System.Text;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class Voucher : INotifyPropertyChanged
    {
        private string _memberCode;
        private string _memberName;
        private string _accountCode;
        private string _accountTitle;
        private decimal _debit;
        private decimal _credit;
        private int _voucherNo;
        private VoucherTypes _voucherType;
        private DateTime _voucherDate;
        private bool _isPosted;

        public string MemberCode
        {
            get { return _memberCode; }
            set
            {
                _memberCode = value;
                OnPropertyChanged("MemberCode");
            }
        }

        public string MemberName
        {
            get { return _memberName; }
            set
            {
                _memberName = value;
                OnPropertyChanged("MemberName");
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

        public int VoucherNo
        {
            get { return _voucherNo; }
            set
            {
                _voucherNo = value;
                OnPropertyChanged("VoucherNo");
            }
        }

        public VoucherTypes VoucherType
        {
            get { return _voucherType; }
            set
            {
                _voucherType = value;
                OnPropertyChanged("VoucherType");
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

        public bool IsPosted //POSTED 
        {
            get { return _isPosted; }
            set
            {
                _isPosted = value;
                OnPropertyChanged("IsPosted");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SetMember(Nfmb member)
        {
            MemberCode = member.MemberCode;
            MemberName = member.MemberName;
        }

        public virtual void SetAccount(Account account)
        {
            AccountCode = account.AccountCode;
            AccountTitle = account.AccountTitle;
        }

        public virtual void SetDocument(VoucherDocument document)
        {
            VoucherType = document.Type;
            VoucherNo = document.Number;
            VoucherDate = document.Date;
        }

        protected internal static int FirstDocumentNo(VoucherTypes voucherType)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT IFNULL(MIN(DOC_NUM),0) FROM `{0}`", voucherType);
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString());
            return DataConverter.ToInteger(dataTable.Rows[0][0]);
        }

        protected internal static int LastDocumentNo(VoucherTypes voucherType)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT IFNULL(MAX(DOC_NUM),0) FROM `{0}`", voucherType);
            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString());
            return  DataConverter.ToInteger(dataTable.Rows[0][0]);
        }

    }

    public class VoucherCollection : System.Collections.ObjectModel.ObservableCollection<Voucher>
    {
    }
}
