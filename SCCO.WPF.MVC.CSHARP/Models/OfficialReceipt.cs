using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class OfficialReceipt : Voucher, IModel
    {
        public OfficialReceipt()
        {
            ResetProperties();
        }
        private const string TABLE_NAME = "OR";
        private decimal _amount;
        private string _amountInWords;
        private decimal _bankAmount1;
        private decimal _bankAmount2;
        private decimal _bankAmount3;
        private decimal _bankAmount4;
        private decimal _bankAmount5;
        private string _bankCheck1;
        private string _bankCheck2;
        private string _bankCheck3;
        private string _bankCheck4;
        private string _bankCheck5;
        private DateTime _bankDate1;
        private DateTime _bankDate2;
        private DateTime _bankDate3;
        private DateTime _bankDate4;
        private DateTime _bankDate5;
        private string _bankDeposited;
        private int _bankDepositSlipNo;
        private string _bankName1;
        private string _bankName2;
        private string _bankName3;
        private string _bankName4;
        private string _bankName5;
        private string _bankTitle1;
        private string _collector;
        private int _deno01;
        private int _deno02;
        private int _deno03;
        private int _deno04;
        private int _deno05;
        private int _deno06;
        private int _deno07;
        private int _deno08;
        private int _deno09;
        private int _deno10;
        private DateTime _depositSlipDate;
        private int _depositSlipNo;
        private string _explanation;
        private int _id;
        private string _modePay;
        private TimeDepositDetails _timeDepositDetails;

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged("Amount"); }
        }

        public string AmountInWords //AMT_WORDS
        {
            get { return _amountInWords; }
            set
            {
                _amountInWords = value;
                OnPropertyChanged("AmountInWords");
            }
        }

        public decimal BankAmount1
        {
            get { return _bankAmount1; }
            set
            {
                _bankAmount1 = value;
                OnPropertyChanged("BankAmount1");
            }
        }

        public decimal BankAmount2
        {
            get { return _bankAmount2; }
            set
            {
                _bankAmount2 = value;
                OnPropertyChanged("BankAmount2");
            }
        }

        public decimal BankAmount3
        {
            get { return _bankAmount3; }
            set
            {
                _bankAmount3 = value;
                OnPropertyChanged("BankAmount3");
            }
        }

        public decimal BankAmount4
        {
            get { return _bankAmount4; }
            set
            {
                _bankAmount4 = value;
                OnPropertyChanged("BankAmount4");
            }
        }

        public decimal BankAmount5
        {
            get { return _bankAmount5; }
            set
            {
                _bankAmount5 = value;
                OnPropertyChanged("BankAmount5");
            }
        }

        public string BankCheck1
        {
            get { return _bankCheck1; }
            set
            {
                _bankCheck1 = value;
                OnPropertyChanged("BankCheck1");
            }
        }

        public string BankCheck2
        {
            get { return _bankCheck2; }
            set
            {
                _bankCheck2 = value;
                OnPropertyChanged("BankCheck2");
            }
        }

        public string BankCheck3
        {
            get { return _bankCheck3; }
            set
            {
                _bankCheck3 = value;
                OnPropertyChanged("BankCheck3");
            }
        }

        public string BankCheck4
        {
            get { return _bankCheck4; }
            set
            {
                _bankCheck4 = value;
                OnPropertyChanged("BankCheck4");
            }
        }

        public string BankCheck5
        {
            get { return _bankCheck5; }
            set
            {
                _bankCheck5 = value;
                OnPropertyChanged("BankCheck5");
            }
        }

        public DateTime BankDate1
        {
            get { return _bankDate1; }
            set
            {
                _bankDate1 = value;
                OnPropertyChanged("BankDate1");
            }
        }

        public DateTime BankDate2
        {
            get { return _bankDate2; }
            set
            {
                _bankDate2 = value;
                OnPropertyChanged("BankDate2");
            }
        }

        public DateTime BankDate3
        {
            get { return _bankDate3; }
            set
            {
                _bankDate3 = value;
                OnPropertyChanged("BankDate3");
            }
        }

        public DateTime BankDate4
        {
            get { return _bankDate4; }
            set
            {
                _bankDate4 = value;
                OnPropertyChanged("BankDate4");
            }
        }

        public DateTime BankDate5
        {
            get { return _bankDate5; }
            set
            {
                _bankDate5 = value;
                OnPropertyChanged("BankDate5");
            }
        }

        public string BankDeposited
        {
            get { return _bankDeposited; }
            set { _bankDeposited = value; OnPropertyChanged("BankDeposited"); }
        }

        public int BankDepositSlipNo
        {
            get { return _bankDepositSlipNo; }
            set { _bankDepositSlipNo = value; OnPropertyChanged("BankDepositSlipNo"); }
        }

        public string BankName1
        {
            get { return _bankName1; }
            set
            {
                _bankName1 = value;
                OnPropertyChanged("BankName1");
            }
        }

        public string BankName2
        {
            get { return _bankName2; }
            set
            {
                _bankName2 = value;
                OnPropertyChanged("BankName2");
            }
        }

        public string BankName3
        {
            get { return _bankName3; }
            set
            {
                _bankName3 = value;
                OnPropertyChanged("BankName3");
            }
        }

        public string BankName4
        {
            get { return _bankName4; }
            set
            {
                _bankName4 = value;
                OnPropertyChanged("BankName4");
            }
        }

        public string BankName5
        {
            get { return _bankName5; }
            set
            {
                _bankName5 = value;
                OnPropertyChanged("BankName5");
            }
        }

        public string BankTitle
        {
            get { return _bankTitle1; }
            set { _bankTitle1 = value; OnPropertyChanged("BankTitle"); }
        }

        public string Collector
        {
            get { return _collector; }
            set
            {
                _collector = value;
                OnPropertyChanged("Collector");
            }
        }

        public int Deno01
        {
            get { return _deno01; }
            set
            {
                _deno01 = value;
                OnPropertyChanged("Deno01");
            }
        }

        public int Deno02
        {
            get { return _deno02; }
            set
            {
                _deno02 = value;
                OnPropertyChanged("Deno02");
            }
        }

        public int Deno03
        {
            get { return _deno03; }
            set
            {
                _deno03 = value;
                OnPropertyChanged("Deno03");
            }
        }

        public int Deno04
        {
            get { return _deno04; }
            set
            {
                _deno04 = value;
                OnPropertyChanged("Deno04");
            }
        }

        public int Deno05
        {
            get { return _deno05; }
            set
            {
                _deno05 = value;
                OnPropertyChanged("Deno05");
            }
        }

        public int Deno06
        {
            get { return _deno06; }
            set
            {
                _deno06 = value;
                OnPropertyChanged("Deno06");
            }
        }

        public int Deno07
        {
            get { return _deno07; }
            set
            {
                _deno07 = value;
                OnPropertyChanged("Deno07");
            }
        }

        public int Deno08
        {
            get { return _deno08; }
            set
            {
                _deno08 = value;
                OnPropertyChanged("Deno08");
            }
        }

        public int Deno09
        {
            get { return _deno09; }
            set
            {
                _deno09 = value;
                OnPropertyChanged("Deno09");
            }
        }

        public int Deno10
        {
            get { return _deno10; }
            set
            {
                _deno10 = value;
                OnPropertyChanged("Deno10");
            }
        }

        public DateTime DepositSlipDate
        {
            get { return _depositSlipDate; }
            set { _depositSlipDate = value; OnPropertyChanged("DepositSlipDate"); }
        }

        public int DepositSlipNo
        {
            get { return _depositSlipNo; }
            set { _depositSlipNo = value; OnPropertyChanged("DepositSlipNo"); }
        }

        public string Explanation
        {
            get { return _explanation; }
            set
            {
                _explanation = value;
                OnPropertyChanged("Explanation");
            }
        }

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string ModePay
        {
            get { return _modePay; }
            set { _modePay = value; OnPropertyChanged("ModePay"); }
        }

        public TimeDepositDetails TimeDepositDetails
        {
            get { return _timeDepositDetails; }
            set { _timeDepositDetails = value; OnPropertyChanged("TimeDepositDetails"); }
        }

        private List<SqlParameter> SqlParameters
        {
            get
            {
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?MEM_CODE", MemberCode);
                ModelController.AddParameter(sqlParameters, "?MEM_NAME", MemberName);

                ModelController.AddParameter(sqlParameters, "?ACC_CODE", AccountCode);
                ModelController.AddParameter(sqlParameters, "?TITLE", AccountTitle);

                ModelController.AddParameter(sqlParameters, "?DOC_DATE", VoucherDate);
                ModelController.AddParameter(sqlParameters, "?DOC_TYPE", VoucherType.ToString());
                ModelController.AddParameter(sqlParameters, "?DOC_NUM", VoucherNo);

                ModelController.AddParameter(sqlParameters, "?DEBIT", Debit);
                ModelController.AddParameter(sqlParameters, "?CREDIT", Credit);

                ModelController.AddParameter(sqlParameters, "?MODE_PAY", ModePay);

                ModelController.AddParameter(sqlParameters, "?COLLECTOR", Collector);
                ModelController.AddParameter(sqlParameters, "?EXPLAIN", Explanation);

                ModelController.AddParameter(sqlParameters, "?DEN1", Deno01);
                ModelController.AddParameter(sqlParameters, "?DEN2", Deno02);
                ModelController.AddParameter(sqlParameters, "?DEN3", Deno03);
                ModelController.AddParameter(sqlParameters, "?DEN4", Deno04);
                ModelController.AddParameter(sqlParameters, "?DEN5", Deno05);
                ModelController.AddParameter(sqlParameters, "?DEN6", Deno06);
                ModelController.AddParameter(sqlParameters, "?DEN7", Deno07);
                ModelController.AddParameter(sqlParameters, "?DEN8", Deno08);
                ModelController.AddParameter(sqlParameters, "?DEN9", Deno09);
                ModelController.AddParameter(sqlParameters, "?DEN10", Deno10);

                ModelController.AddParameter(sqlParameters, "?BNAME1", BankName1);
                ModelController.AddParameter(sqlParameters, "?BDATE1", BankDate1);
                ModelController.AddParameter(sqlParameters, "?BCHECK1", BankCheck1);
                ModelController.AddParameter(sqlParameters, "?BAMT1", BankAmount1);

                ModelController.AddParameter(sqlParameters, "?BNAME2", BankName2);
                ModelController.AddParameter(sqlParameters, "?BDATE2", BankDate2);
                ModelController.AddParameter(sqlParameters, "?BCHECK2", BankCheck2);
                ModelController.AddParameter(sqlParameters, "?BAMT2", BankAmount2);

                ModelController.AddParameter(sqlParameters, "?BNAME3", BankName3);
                ModelController.AddParameter(sqlParameters, "?BDATE3", BankDate3);
                ModelController.AddParameter(sqlParameters, "?BCHECK3", BankCheck3);
                ModelController.AddParameter(sqlParameters, "?BAMT3", BankAmount3);

                ModelController.AddParameter(sqlParameters, "?BNAME4", BankName4);
                ModelController.AddParameter(sqlParameters, "?BDATE4", BankDate4);
                ModelController.AddParameter(sqlParameters, "?BCHECK4", BankCheck4);
                ModelController.AddParameter(sqlParameters, "?BAMT4", BankAmount4);

                ModelController.AddParameter(sqlParameters, "?BNAME5", BankName5);
                ModelController.AddParameter(sqlParameters, "?BDATE5", BankDate5);
                ModelController.AddParameter(sqlParameters, "?BCHECK5", BankCheck5);
                ModelController.AddParameter(sqlParameters, "?BAMT5", BankAmount5);

                ModelController.AddParameter(sqlParameters, "?WORDS", AmountInWords);
                ModelController.AddParameter(sqlParameters, "?DS_NO", DepositSlipNo);
                ModelController.AddParameter(sqlParameters, "?DS_DATE", DepositSlipDate);
                ModelController.AddParameter(sqlParameters, "?DS_NO1", BankDepositSlipNo);
                ModelController.AddParameter(sqlParameters, "?BANK", BankDeposited);
                ModelController.AddParameter(sqlParameters, "?BANK_TITLE", BankTitle);
                ModelController.AddParameter(sqlParameters, "?AMT_DEP", Amount);

                if (TimeDepositDetails != null)
                {
                    ModelController.AddParameter(sqlParameters, "?CERT_NO", TimeDepositDetails.CertificateNo);
                    ModelController.AddParameter(sqlParameters, "?TERM", TimeDepositDetails.Term);
                    ModelController.AddParameter(sqlParameters, "?RATE", TimeDepositDetails.Rate);
                    ModelController.AddParameter(sqlParameters, "?DATE_IN", TimeDepositDetails.DateIn);
                }

                return sqlParameters;
            }
        }

        public static int DeleteAll(int documentNo)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE FROM `{0}` WHERE DOC_NUM = ?DOC_NUM", TABLE_NAME);
            return DatabaseController.ExecuteNonQuery(sqlBuilder.ToString(), new SqlParameter("?DOC_NUM", documentNo));
        }

        public static ObservableCollection<OfficialReceipt> FindByDocumentNumber(int documentNo)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT * FROM `{0}` WHERE DOC_NUM = ?DOC_NUM", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), new SqlParameter("?DOC_NUM", documentNo));
            var listRecord = new ObservableCollection<OfficialReceipt>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var foundRecord = new OfficialReceipt();
                foundRecord.SetPropertiesFromDataRow(dataRow);
                listRecord.Add(foundRecord);
            }
            return listRecord;
        }

        public void ResetProperties()
        {
            ID = 0;
            MemberCode = string.Empty;
            MemberName = string.Empty;
            AccountCode = string.Empty;
            AccountTitle = string.Empty;
            Debit = 0m;
            Credit = 0m;
            VoucherDate = new DateTime();
            VoucherType = VoucherTypes.OR;
            VoucherNo = 0;
            ModePay = string.Empty;

            Collector = string.Empty;
            Explanation = string.Empty;
            IsPosted = false;
            Deno01 = 0;
            Deno02 = 0;
            Deno03 = 0;
            Deno04 = 0;
            Deno05 = 0;
            Deno06 = 0;
            Deno07 = 0;
            Deno08 = 0;
            Deno09 = 0;
            Deno10 = 0;
            BankName1 = string.Empty;
            BankName2 = string.Empty;
            BankName3 = string.Empty;
            BankName4 = string.Empty;
            BankName5 = string.Empty;
            BankDate1 = new DateTime();
            BankDate2 = new DateTime();
            BankDate3 = new DateTime();
            BankDate4 = new DateTime();
            BankDate5 = new DateTime();
            BankCheck1 = string.Empty;
            BankCheck2 = string.Empty;
            BankCheck3 = string.Empty;
            BankCheck4 = string.Empty;
            BankCheck5 = string.Empty;
            BankAmount1 = 0m;
            BankAmount2 = 0m;
            BankAmount3 = 0m;
            BankAmount4 = 0m;
            BankAmount5 = 0m;
            DepositSlipNo = 0;
            DepositSlipDate = new DateTime();
            BankDepositSlipNo = 0;
            BankDeposited = string.Empty;
            BankTitle = string.Empty;
            Amount = 0m;
            AmountInWords = string.Empty;

            TimeDepositDetails = null;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
            MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
            AccountCode = DataConverter.ToString(dataRow["ACC_CODE"]);
            AccountTitle = DataConverter.ToString(dataRow["TITLE"]);
            Debit = DataConverter.ToDecimal(dataRow["DEBIT"]);
            Credit = DataConverter.ToDecimal(dataRow["CREDIT"]);
            VoucherDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]);
            VoucherType = VoucherTypes.OR;
            VoucherNo = DataConverter.ToInteger(dataRow["DOC_NUM"]);
            ModePay = DataConverter.ToString(dataRow["MODE_PAY"]);

            Collector = DataConverter.ToString(dataRow["COLLECTOR"]);
            //Explanation = DataConverter.ToString(dataRow["EXPLAIN"]);
            Explanation = Encoding.UTF8.GetString(DataConverter.ToByteArray(dataRow["EXPLAIN"]));

            IsPosted = DataConverter.ToBoolean(dataRow["POSTED"]);
            Deno01 = DataConverter.ToInteger(dataRow["DEN1"]);
            Deno02 = DataConverter.ToInteger(dataRow["DEN2"]);
            Deno03 = DataConverter.ToInteger(dataRow["DEN3"]);
            Deno04 = DataConverter.ToInteger(dataRow["DEN4"]);
            Deno05 = DataConverter.ToInteger(dataRow["DEN5"]);
            Deno06 = DataConverter.ToInteger(dataRow["DEN6"]);
            Deno07 = DataConverter.ToInteger(dataRow["DEN7"]);
            Deno08 = DataConverter.ToInteger(dataRow["DEN8"]);
            Deno09 = DataConverter.ToInteger(dataRow["DEN9"]);
            Deno10 = DataConverter.ToInteger(dataRow["DEN10"]);
            BankName1 = DataConverter.ToString(dataRow["BNAME1"]);
            BankName2 = DataConverter.ToString(dataRow["BNAME2"]);
            BankName3 = DataConverter.ToString(dataRow["BNAME3"]);
            BankName4 = DataConverter.ToString(dataRow["BNAME4"]);
            BankName5 = DataConverter.ToString(dataRow["BNAME5"]);
            BankDate1 = DataConverter.ToDateTime(dataRow["BDATE1"]);
            BankDate2 = DataConverter.ToDateTime(dataRow["BDATE2"]);
            BankDate3 = DataConverter.ToDateTime(dataRow["BDATE3"]);
            BankDate4 = DataConverter.ToDateTime(dataRow["BDATE4"]);
            BankDate5 = DataConverter.ToDateTime(dataRow["BDATE5"]);
            BankCheck1 = DataConverter.ToString(dataRow["BCHECK1"]);
            BankCheck2 = DataConverter.ToString(dataRow["BCHECK2"]);
            BankCheck3 = DataConverter.ToString(dataRow["BCHECK3"]);
            BankCheck4 = DataConverter.ToString(dataRow["BCHECK4"]);
            BankCheck5 = DataConverter.ToString(dataRow["BCHECK5"]);
            BankAmount1 = DataConverter.ToDecimal(dataRow["BAMT1"]);
            BankAmount2 = DataConverter.ToDecimal(dataRow["BAMT2"]);
            BankAmount3 = DataConverter.ToDecimal(dataRow["BAMT3"]);
            BankAmount4 = DataConverter.ToDecimal(dataRow["BAMT4"]);
            BankAmount5 = DataConverter.ToDecimal(dataRow["BAMT5"]);
            DepositSlipNo = DataConverter.ToInteger(dataRow["DS_NO"]);
            DepositSlipDate = DataConverter.ToDateTime(dataRow["DS_DATE"]);
            BankDepositSlipNo = DataConverter.ToInteger(dataRow["DS_NO1"]);
            BankDeposited = DataConverter.ToString(dataRow["BANK"]);
            BankTitle = DataConverter.ToString(dataRow["BANK_TITLE"]);
            Amount = DataConverter.ToDecimal(dataRow["AMT_DEP"]);
            AmountInWords = DataConverter.ToString(dataRow["WORDS"]);

            TimeDepositDetails = TimeDepositDetails.ExtractFromDataRow(dataRow);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);

                List<SqlParameter> sqlParameters = SqlParameters;
                string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME, sqlParameters, key);

                sqlParameters.Add(key);
                DatabaseController.ExecuteNonQuery(sql, sqlParameters.ToArray());
            };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Create()
        {
            Action createRecord = () =>
            {
                string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, SqlParameters);
                ID = DatabaseController.ExecuteInsertQuery(sql, SqlParameters.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);

                string sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

                DatabaseController.ExecuteNonQuery(sql, key);
            };

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
            {
                ResetProperties();
                ID = id;

                var key = new SqlParameter("?ID", ID);
                string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

                DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SetPropertiesFromDataRow(dataRow);
                }
            };

            return ActionController.InvokeAction(findRecord);
        }

        //public static int FirstDocumentNo()
        //{
        //    var sqlBuilder = new StringBuilder();
        //    sqlBuilder.AppendFormat("SELECT MIN(DOC_NUM) FROM `{0}`", TableName);
        //    var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString());
        //    var docNum = 0;
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        docNum = DataConverter.ToInteger(dataTable.Rows[0][0]);
        //    }
        //    return docNum;
        //}

        //public static int LastDocumentNo()
        //{
        //    var sqlBuilder = new StringBuilder();
        //    sqlBuilder.AppendFormat("SELECT MAX(DOC_NUM) FROM `{0}`", TableName);
        //    var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString());
        //    var docNum = 0;
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        docNum = DataConverter.ToInteger(dataTable.Rows[0][0]);
        //    }
        //    return docNum;
        //}

        public static ObservableCollection<OfficialReceipt> FindByMemberCode(string memberCode) 
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT * FROM `{0}` WHERE MEM_CODE = ?MEM_CODE", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), new SqlParameter("?MEM_CODE", memberCode));
            var listRecord = new ObservableCollection<OfficialReceipt>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var foundRecord = new OfficialReceipt();
                foundRecord.SetPropertiesFromDataRow(dataRow);
                listRecord.Add(foundRecord);
            }
            return listRecord;
        }
    }
}
