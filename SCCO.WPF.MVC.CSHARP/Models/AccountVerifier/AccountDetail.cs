using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.AccountVerifier
{
    public class AccountDetail : INotifyPropertyChanged, IModel
    {
        private LoanDetails _loanDetails;
        private TimeDepositDetails _timeDepositDetails;
        private bool _mark;
        //`PLINE` int(11),
        public int Pline { get; set; }

        //`LINE` int(11),
        public int Line { get; set; }

        //`REM` varchar(10),
        public string Rem { get; set; }

        //`MEM_CODE` varchar(6),
        public string MemberCode { get; set; }

        //`MEM_NAME` varchar(50),
        public string MemberName { get; set; }

        //`CHECK_NUM` varchar(10),
        public string CheckNum { get; set; }

        //`NATURE` varchar(1),
        public string Nature { get; set; }

        //`ACC_CODE` varchar(7),
        public string AccountCode { get; set; }

        //`TITLE` varchar(50),
        public string Title { get; set; }

        //`DEBIT` double,
        public decimal Debit { get; set; }

        //`CREDIT` double,
        public decimal Credit { get; set; }

        //`DOC_DATE` date,
        public DateTime VoucherDate { get; set; }

        //`DOC_TYPE` varchar(2),
        public string VoucherType { get; set; }

        //`DOC_NUM` bigint(20),
        public int VoucherNumber { get; set; }

        //`TRN_TYPE` varchar(1),
        public string TrnType { get; set; }

        //`BEG_BAL` double,
        public decimal BeginningBalance { get; set; }

        //`END_BAL` double,
        public decimal EndingBalance { get; set; }

        //`CERT_NO` varchar(10),
        public string CertificateNumber { get; set; }

        //`DATE_IN` date,
        public DateTime DateIn { get; set; }

        //`TERM` int(11),
        public int Term { get; set; }

        //`RATE` double,
        public decimal Rate { get; set; }

        //`FLAG` int(11),
        public int Flag { get; set; }

        //`MODE_PAY` varchar(1),
        public string ModePay { get; set; }

        //`MARK` varchar(1),
        public bool Mark
        {
            get { return _mark; }
            set { _mark = value; OnPropertyChanged("Mark");}
        }

        //`SPACE1` longtext,
        public string Space1 { get; set; }

        //`INITIAL` varchar(3),
        public string Initial { get; set; }

        //`REMARK` varchar(10),
        public string Remark { get; set; }

        //`TERMS` int(11),
        public int Terms { get; set; }

        //`DUE_DATE` date,
        public DateTime DueDate { get; set; }

        //`PO_NO` varchar(50),
        public string PoNo { get; set; }

        //`DAYS` int(11),
        public int Days { get; set; }

        //`RELEASE_NO` int(11),
        public int ReleaseNo { get; set; }

        //`BANK_TITLE` varchar(50),
        public string BankTitle { get; set; }

        //`CHECK_NUM1` varchar(10),
        public string CheckNum1 { get; set; }

        //`LOAN_AMT` double,
        public decimal LoanAmount { get; set; }

        //`TERMS1` int(11),
        public int Terms1 { get; set; }

        //`TERMS_MODE` varchar(2),
        public string TermsMode { get; set; }

        //`DATE_GRANT` date,
        public DateTime DateGrant { get; set; }

        //`MATURITY` date,
        public DateTime Maturity { get; set; }

        //`CUT_OFF` date,
        public DateTime CutOff { get; set; }

        //`MODE_PAY1` varchar(3),
        public string ModePay1 { get; set; }

        //`PAYMENT` double,
        public decimal Payment { get; set; }

        //`INT_RATE` double,
        public decimal IntRate { get; set; }

        //`INT_AMT` double,
        public decimal IntAmt { get; set; }

        //`INT_AMORT` double,
        public decimal IntAmort { get; set; }

        //`APPROVED` date,
        public DateTime Approved { get; set; }

        //`CANCELLED` date,
        public DateTime Cancelled { get; set; }

        //`RELEASED` date,
        public DateTime Released { get; set; }

        //`APPLIED` date,
        public DateTime Applied { get; set; }

        //`CO_CODE1` varchar(6),
        public string CoCode1 { get; set; }

        //`CO_NAME1` varchar(50),
        public string CoName1 { get; set; }

        //`CO_CODE2` varchar(6),
        public string CoCode2 { get; set; }

        //`CO_NAME2` varchar(50),
        public string CoName2 { get; set; }

        //`CO_CODE3` varchar(6),
        public string CoCode3 { get; set; }

        //`CO_NAME3` varchar(50),
        public string CoName3 { get; set; }

        //`CO_CODE4` varchar(6),
        public string CoCode4 { get; set; }

        //`CO_NAME4` varchar(50),
        public string CoName4 { get; set; }

        //`CO_CODE5` varchar(6),
        public string CoCode5 { get; set; }

        //`CO_NAME5` varchar(50),
        public string CoName5 { get; set; }

        //`THIS_MONTH` double,
        public decimal ThisMonth { get; set; }

        //`COLLECTOR` varchar(30),
        public string Collector { get; set; }

        //`NOTICE1` tinyint(4),
        public bool Notice1 { get; set; }

        //`NOTICE2` tinyint(4),
        public bool Notice2 { get; set; }

        //`NOTICE3` tinyint(4),
        public bool Notice3 { get; set; }

        //`REMARKS` varchar(10),
        public string Remarks { get; set; }

        //`COLLAT` tinyint(4),
        public bool Collat { get; set; }

        //`DESC` varchar(100),
        public string Desc { get; set; }

        //`R1` int(11)ULT NULL,
        public int R1 { get; set; }

        public string Reference
        {
            // get { return string.Format("{0} {1}", VoucherType, VoucherNumber); }
            get; set; }

        public TimeDepositDetails TimeDepositDetails
        {
            get { return _timeDepositDetails; }
            set { _timeDepositDetails = value; OnPropertyChanged("TimeDepositDetails"); }
        }

        public LoanDetails LoanDetails
        {
            get { return _loanDetails; }
            set { _loanDetails = value; OnPropertyChanged("LoanDetails"); }
        }

        public static List<AccountDetail> Find(string memberCode, 
            string accountCode, 
            string certificateNo, 
            DateTime asOf)
        {
            var sqlParams = new SqlParameter[4];
            sqlParams[0] = new SqlParameter("ts_member_code", memberCode);
            sqlParams[1] = new SqlParameter("ts_account_code", accountCode);
            sqlParams[2] = new SqlParameter("ts_certificate_no", certificateNo);
            sqlParams[3] = new SqlParameter("td_as_of", asOf);
            var dataTable = DatabaseController.ExecuteStoredProcedure("sp_account_details", sqlParams);

            var result = new List<AccountDetail>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var detail = new AccountDetail();
                detail.SetPropertiesFromDataRow(dataRow);
                
                var voucherLog = new VoucherLog();
                voucherLog.Find(detail.VoucherType, detail.VoucherNumber);
                detail.Initial = voucherLog.Initials;
                detail.Remarks = voucherLog.Remarks;

                result.Add(detail);
            }
            return result;
        }

        #region Implementation of IModel

        public Result Create()
        {
            throw new NotImplementedException();
        }

        public Result Update()
        {
            throw new NotImplementedException();
        }

        public Result Destroy()
        {
            throw new NotImplementedException();
        }

        public Result Find(int id)
        {
            throw new NotImplementedException();
        }

        public void ResetProperties()
        {
            throw new NotImplementedException();
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            Pline = DataConverter.ToInteger(dataRow["PLINE"]);
            Line = DataConverter.ToInteger(dataRow["LINE"]);
            Rem = DataConverter.ToString(dataRow["REM"]);
            MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
            MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
            CheckNum = DataConverter.ToString(dataRow["CHECK_NUM"]);
            Nature = DataConverter.ToString(dataRow["NATURE"]);
            AccountCode = DataConverter.ToString(dataRow["ACC_CODE"]);
            Title = DataConverter.ToString(dataRow["TITLE"]);
            Debit = DataConverter.ToDecimal(dataRow["DEBIT"]);
            Credit = DataConverter.ToDecimal(dataRow["CREDIT"]);
            VoucherDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]);
            VoucherType = DataConverter.ToString(dataRow["DOC_TYPE"]);
            VoucherNumber = DataConverter.ToInteger(dataRow["DOC_NUM"]);
            TrnType = DataConverter.ToString(dataRow["TRN_TYPE"]);
            BeginningBalance = DataConverter.ToDecimal(dataRow["BEG_BAL"]);
            EndingBalance = DataConverter.ToDecimal(dataRow["END_BAL"]);
            Flag = DataConverter.ToInteger(dataRow["FLAG"]);
            ModePay = DataConverter.ToString(dataRow["MODE_PAY"]);
            Mark = DataConverter.ToBoolean(dataRow["MARK"]);
            Space1 = DataConverter.ToString(dataRow["SPACE1"]);
            Initial = DataConverter.ToString(dataRow["INITIAL"]);
            Remark = DataConverter.ToString(dataRow["REMARK"]);
            Terms = DataConverter.ToInteger(dataRow["TERMS"]);
            DueDate = DataConverter.ToDateTime(dataRow["DUE_DATE"]);
            PoNo = DataConverter.ToString(dataRow["PO_NO"]);
            Days = DataConverter.ToInteger(dataRow["DAYS"]);
            ReleaseNo = DataConverter.ToInteger(dataRow["RELEASE_NO"]);
            BankTitle = DataConverter.ToString(dataRow["BANK_TITLE"]);
            CheckNum1 = DataConverter.ToString(dataRow["CHECK_NUM1"]);
            LoanAmount = DataConverter.ToDecimal(dataRow["LOAN_AMT"]);
            TermsMode = DataConverter.ToString(dataRow["TERMS_MODE"]);
            DateGrant = DataConverter.ToDateTime(dataRow["DATE_GRANT"]);
            Maturity = DataConverter.ToDateTime(dataRow["MATURITY"]);
            CutOff = DataConverter.ToDateTime(dataRow["CUT_OFF"]);

            Reference = string.Format("{0} {1}", VoucherType, VoucherNumber);

            TimeDepositDetails = new TimeDepositDetails(dataRow);   
            LoanDetails = new LoanDetails(dataRow);
            //LoanDetails.ReleaseNo = DataConverter.ToInteger(dataRow["RELEASE_NO"]);
            //LoanDetails.LoanAmount = DataConverter.ToDecimal(dataRow["LOAN_AMT"]);
            //LoanDetails.LoanTerms = DataConverter.ToInteger(dataRow["TERMS1"]);
            //LoanDetails.TermsMode = DataConverter.ToTermsMode(dataRow["TERMS1"],dataRow["TERMS_MODE"]);
            //LoanDetails.GrantedDate = DataConverter.ToDateTime(dataRow["DATE_GRANT"]);
            //LoanDetails.MaturityDate = DataConverter.ToDateTime(dataRow["MATURITY"]);
            //LoanDetails.CutOffDate = DataConverter.ToDateTime(dataRow["CUT_OFF"]);
            //LoanDetails.ModeOfPayment = DataConverter.ToModeOfPayment(dataRow["MODE_PAY1"]);
            //LoanDetails.Payment = DataConverter.ToDecimal(dataRow["PAYMENT"]);
            //LoanDetails.InterestRate = DataConverter.ToDecimal(dataRow["INT_RATE"]);
            //LoanDetails.InterestAmount = DataConverter.ToDecimal(dataRow["INT_AMT"]);
            //LoanDetails.InterestAmortization = DataConverter.ToDecimal(dataRow["INT_AMORT"]);
            //LoanDetails.DateApproved = DataConverter.ToDateTime(dataRow["APPROVED"]);
            //LoanDetails.DateCancelled = DataConverter.ToDateTime(dataRow["CANCELLED"]);
            //LoanDetails.DateReleased = DataConverter.ToDateTime(dataRow["RELEASED"]);
            //LoanDetails.DateApplied = DataConverter.ToDateTime(dataRow["APPLIED"]);

            //LoanDetails.ThisMonth = DataConverter.ToDecimal(dataRow["THIS_MONTH"]);
            //LoanDetails.Collector = DataConverter.ToString(dataRow["COLLECTOR"]);
            //LoanDetails.Remarks = DataConverter.ToString(dataRow["REMARKS"]);
            //LoanDetails.IsWithCollateral = DataConverter.ToBoolean(dataRow["COLLAT"]);
            //LoanDetails.Description = DataConverter.ToString(dataRow["DESC"]);


            //LoanDetails.CoMakers[0] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE1"]),
            //                                      DataConverter.ToString(dataRow["CO_NAME1"]));

            //LoanDetails.CoMakers[1] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE2"]),
            //                                      DataConverter.ToString(dataRow["CO_NAME2"]));

            //LoanDetails.CoMakers[2] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE3"]),
            //                                      DataConverter.ToString(dataRow["CO_NAME3"]));

            //LoanDetails.CoMakers[3] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE4"]),
            //                                      DataConverter.ToString(dataRow["CO_NAME4"]));

            //LoanDetails.CoMakers[4] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE5"]),
            //                                      DataConverter.ToString(dataRow["CO_NAME5"]));

            //LoanDetails.Notices[0] = DataConverter.ToBoolean(dataRow["NOTICE1"]);
            //LoanDetails.Notices[1] = DataConverter.ToBoolean(dataRow["NOTICE2"]);
            //LoanDetails.Notices[2] = DataConverter.ToBoolean(dataRow["NOTICE3"]);

        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
