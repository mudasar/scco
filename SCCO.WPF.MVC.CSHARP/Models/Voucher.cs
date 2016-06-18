using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            return DataConverter.ToInteger(dataTable.Rows[0][0]);
        }

        protected internal static bool Exist(VoucherTypes voucherType, int documentNo)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT COUNT(DOC_NUM) FROM `{0}` WHERE DOC_NUM = ?DOC_NUM", voucherType);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(),
                                                                        new SqlParameter("?DOC_NUM", documentNo));
            return DataConverter.ToInteger(dataTable.Rows[0][0]) > 0;
        }

        internal static string GetTableName(VoucherTypes voucherType)
        {
            switch (voucherType)
            {
                case VoucherTypes.BG:
                    return "slbal";
                case VoucherTypes.CV:
                    return "cv";
                case VoucherTypes.JV:
                    return "jv";
                case VoucherTypes.OR:
                    return "or";
            }
            throw new NotSupportedException(string.Format("Table name for {0} is not supported.", voucherType));
        }

        public static void Touch(VoucherTypes voucherType, int voucherId, int userId)
        {
            if (voucherId == 0) return;
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("UPDATE `{0}` ", GetTableName(voucherType));
            queryBuilder.AppendFormat("SET UPDATED_BY = ?UserId, ");
            queryBuilder.AppendFormat("UPDATED_AT = ?UpdatedAt ");
            queryBuilder.AppendFormat("WHERE ID = ?Id");

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("?UserId", userId),
                new SqlParameter("?UpdatedAt", DateTime.Now),
                new SqlParameter("Id", voucherId)
            };

            DatabaseController.ExecuteNonQuery(queryBuilder.ToString(), parameters.ToArray());
        }

        internal static DataTable GetExplanation(VoucherTypes voucherTypes, int documentNumber)
        {
            const string sql = "SELECT doc_date, doc_type, doc_num, " +
                               "CAST(`EXPLAIN` as char(10000) CHARACTER SET utf8 ) as `explanation` " +
                               "FROM `{0}` where doc_num = ?p1 and `EXPLAIN` is not NULL ORDER BY id desc limit 1; ";

            var param = new SqlParameter("?p1", documentNumber);
            return DatabaseController.ExecuteSelectQuery(string.Format(sql, voucherTypes), param);
        }
    }

    public class VoucherCollection : System.Collections.ObjectModel.ObservableCollection<Voucher>
    {
    }
}