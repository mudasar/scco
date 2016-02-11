using System;
using System.Collections.Generic;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    internal class LoanAmortizationHeader : ModelBase, IModel
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string MemberAddress { get; set; }

        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }

        public Decimal LoanAmount { get; set; }
        public decimal MonthlyAmortization { get; set; }
        public decimal MonthlyCapitalBuildUp { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string ModeOfPayment { get; set; }

        public DateTime DateGranted { get; set; }
        public DateTime DateMaturity { get; set; }
        public DateTime FirstPaymentDate { get; set; }

        public List<LoanAmortizationDetail> PaymentSchedules { get; set; }


        public LoanAmortizationHeader() : base("loan_amortization_schedules")
        {
            PaymentSchedules = new List<LoanAmortizationDetail>();
        }

        #region --- CRUD ---

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?member_code", MemberCode);
                ModelController.AddParameter(sqlParameters, "?account_code", AccountCode);
                ModelController.AddParameter(sqlParameters, "?loan_amount", LoanAmount);
                ModelController.AddParameter(sqlParameters, "?loan_term", LoanTerm);
                ModelController.AddParameter(sqlParameters, "?annual_interest_rate", AnnualInterestRate);
                ModelController.AddParameter(sqlParameters, "?date_granted", DateGranted);
                ModelController.AddParameter(sqlParameters, "?monthy_capital_build_up", MonthlyCapitalBuildUp);
                return sqlParameters;
            }
        }

        private SqlParameter ParamKey
        {
            get { return _paramKey; }
        }

        public Result Create()
        {
            Action createRecord = () =>
                {
                    List<SqlParameter> sqlParameter = Parameters;

                    string sql = DatabaseController.GenerateInsertStatement(_tableName, sqlParameter);
                    ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                {
                    SqlParameter key = ParamKey;

                    string sql = DatabaseController.GenerateDeleteStatement(_tableName, key);

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
                    DataTable dataTable = DatabaseController.FindRecord(_tableName, id);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        SetPropertiesFromDataRow(dataRow);
                    }
                };

            return ActionController.InvokeAction(findRecord);
        }

        public void ResetProperties()
        {
            ID = 0;
            MemberCode = "";
            MemberName = "";
            MemberAddress = "";

            AccountCode = "";
            AccountTitle = "";

            LoanAmount = 0m;
            MonthlyAmortization = 0m;
            MonthlyCapitalBuildUp = 0m;
            AnnualInterestRate = 0m;
            LoanTerm = 0;
            ModeOfPayment = "Monthly";

            DateGranted = DateTime.Now;
            DateMaturity = DateTime.Now;
            FirstPaymentDate = DateTime.Now;

            PaymentSchedules = null;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["id"]);
            string memberCode = DataConverter.ToString(dataRow["member_code"]);
            string accountCode = DataConverter.ToString(dataRow["account_code"]);
            decimal loanAmount = DataConverter.ToDecimal(dataRow["loan_amount"]);
            int loanTerm = DataConverter.ToInteger(dataRow["loan_term"]);
            decimal annualInterestRate = DataConverter.ToDecimal(dataRow["annual_interest_rate"]);
            DateTime dateGranted = DataConverter.ToDateTime(dataRow["date_granted"]);
            decimal monthlyCapitalBuildUp = DataConverter.ToDecimal(dataRow["monthly_capital_build_up"]);

            LoanAmortizationHeader las = LoanAmortizationController.GenerateLoanAmortization(
                memberCode,
                accountCode,
                loanAmount,
                loanTerm,
                annualInterestRate,
                dateGranted,
                monthlyCapitalBuildUp);


            MemberCode = las.MemberCode;
            MemberName = las.MemberName;
            MemberAddress = las.MemberAddress;

            AccountCode = las.AccountCode;
            AccountTitle = las.AccountTitle;

            LoanAmount = las.LoanAmount;
            MonthlyAmortization = las.MonthlyAmortization;
            MonthlyCapitalBuildUp = las.MonthlyCapitalBuildUp;
            AnnualInterestRate = las.AnnualInterestRate;
            LoanTerm = las.LoanTerm;
            ModeOfPayment = las.ModeOfPayment;

            DateGranted = las.DateGranted;
            DateMaturity = las.DateMaturity;
            FirstPaymentDate = las.FirstPaymentDate;

            PaymentSchedules = las.PaymentSchedules;
        }

        public Result Update()
        {
            if (ID == 0) return Create();

            Action updateRecord = () =>
                {
                    SqlParameter key = ParamKey;

                    List<SqlParameter> sqlParameter = Parameters;
                    sqlParameter.Add(key);

                    string sql = DatabaseController.GenerateUpdateStatement(_tableName,
                                                                            sqlParameter,
                                                                            key);

                    DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
                };

            return ActionController.InvokeAction(updateRecord);
        }

        #endregion --- CRUD ---
    }

    internal class LoanAmortizationDetail : ModelBase, IModel
    {
        public LoanAmortizationDetail() : base("loan_amortization_details")
        {
        }

        public DateTime PaymentDate { get; set; }
        public int PaymentNo { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal Payment { get; set; }
        public decimal Interest { get; set; }
        public decimal CapitalBuildUp { get; set; }
        public decimal Amortization { get; set; }
        public decimal EndingBalance { get; set; }

        #region --- CRUD ---

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?payment_date", PaymentDate);
                ModelController.AddParameter(sqlParameters, "?payment_date", PaymentNo);
                ModelController.AddParameter(sqlParameters, "?beginning_balance", BeginningBalance);
                ModelController.AddParameter(sqlParameters, "?payment", Payment);
                ModelController.AddParameter(sqlParameters, "?interest", Interest);
                ModelController.AddParameter(sqlParameters, "?capital_build_up", CapitalBuildUp);
                ModelController.AddParameter(sqlParameters, "?amortization", Amortization);
                ModelController.AddParameter(sqlParameters, "?ending_balance", EndingBalance);
                return sqlParameters;
            }
        }

        private SqlParameter ParamKey
        {
            get { return _paramKey; }
        }

        public Result Create()
        {
            Action createRecord = () =>
                {
                    List<SqlParameter> sqlParameter = Parameters;

                    string sql = DatabaseController.GenerateInsertStatement(_tableName, sqlParameter);
                    ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                {
                    SqlParameter key = ParamKey;

                    string sql = DatabaseController.GenerateDeleteStatement(_tableName, key);

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

                    DataTable dataTable = DatabaseController.FindRecord(_tableName, id);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        SetPropertiesFromDataRow(dataRow);
                    }
                };

            return ActionController.InvokeAction(findRecord);
        }

        public void ResetProperties()
        {
            ID = 0;
            PaymentDate = new DateTime();
            PaymentNo = 0;
            BeginningBalance = 0m;
            Payment = 0m;
            Interest = 0m;
            CapitalBuildUp = 0m;
            Amortization = 0m;
            EndingBalance = 0m;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["id"]);
            PaymentDate = DataConverter.ToDateTime(dataRow["payment_date"]);
            PaymentNo = DataConverter.ToInteger(dataRow["payment_no"]);
            BeginningBalance = DataConverter.ToDecimal(dataRow["beginning_balance"]);
            Payment = DataConverter.ToInteger(dataRow["payment"]);
            Interest = DataConverter.ToDecimal(dataRow["interest"]);
            CapitalBuildUp = DataConverter.ToDecimal(dataRow["capital_build_up"]);
            Amortization = DataConverter.ToDecimal(dataRow["amortization"]);
            EndingBalance = DataConverter.ToDecimal(dataRow["ending_balance"]);
        }

        public Result Update()
        {
            if (ID == 0) return Create();

            Action updateRecord = () =>
                {
                    SqlParameter key = ParamKey;

                    List<SqlParameter> sqlParameter = Parameters;
                    sqlParameter.Add(key);

                    string sql = DatabaseController.GenerateUpdateStatement(_tableName,
                                                                            sqlParameter,
                                                                            key);

                    DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
                };

            return ActionController.InvokeAction(updateRecord);
        }

        #endregion --- CRUD ---
    }
}
