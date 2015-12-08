using System;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    /// <summary>
    ///     Interaction logic for LoanNoticesView.xaml
    /// </summary>
    public partial class LoanNoticesView
    {
        private readonly LoanDetails _loanDetails;

        public LoanNoticesView(LoanDetails loanDetails)
        {
            InitializeComponent();
            _loanDetails = loanDetails;

            LoansNonperformingButton.Click += (sender, args) => GenerateNoticesForLoansNonPerforming();

            LoansNearMaturityButton.Click += (sender, args) => GenerateNoticesForLoansNearMaturity();

            LoansOverdueButton.Click += (sender, args) => GenerateNoticesForLoansOverdue();

            LoansOverdueNonResponsiveButton.Click += (sender, args) => GenerateNoticesForNonResponsiveLoansOverdue();

            LoanNoticeForComakersButton.Click += (sender, args) => GenerateNoticesForComakers();
        }

        private void GenerateNoticesForLoansNonPerforming()
        {
            try
            {
                DateTime asOf = MainController.LoggedUser.TransactionDate;

                DataTable tableFromSql = DatabaseController.ExecuteStoredProcedure("sp_notice_loan_non_performing",
                                                                                   new SqlParameter("as_of", asOf));

                DataTable loanDetails = tableFromSql.AsEnumerable()
                                                    .Where(
                                                        row =>
                                                        row.Field<String>("member_code") == _loanDetails.MemberCode
                                                        && row.Field<String>("account_code") == _loanDetails.AccountCode)
                                                    .CopyToDataTable();

                loanDetails.TableName = "notice_loan_non_performing";
                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                    {
                        ReportFile = "notice_loan_non_performing.rpt",
                        Title = "Notice for non-performing loans",
                        DataSource = dataSet
                    };
                Result result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }

        private void GenerateNoticesForLoansNearMaturity()
        {
            try
            {
                DateTime asOf = MainController.LoggedUser.TransactionDate;

                DataTable tableFromSql = DatabaseController.ExecuteStoredProcedure("sp_notice_loan_near_maturity",
                                                                                   new SqlParameter("as_of", asOf));

                DataTable loanDetails = tableFromSql.AsEnumerable()
                                                    .Where(
                                                        row =>
                                                        row.Field<String>("member_code") == _loanDetails.MemberCode
                                                        && row.Field<String>("account_code") == _loanDetails.AccountCode)
                                                    .CopyToDataTable();

                loanDetails.TableName = "notice_loan_details";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                    {
                        ReportFile = "notice_loan_near_maturity.rpt",
                        Title = "Notice for loan near maturity",
                        DataSource = dataSet
                    };
                Result result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }

        private void GenerateNoticesForLoansOverdue()
        {
            try
            {
                DateTime asOf = MainController.LoggedUser.TransactionDate;

                DataTable tableFromSql = DatabaseController.ExecuteStoredProcedure("sp_notice_loan_overdue",
                                                                                   new SqlParameter("as_of", asOf));

                DataTable loanDetails = tableFromSql.AsEnumerable()
                                                    .Where(
                                                        row =>
                                                        row.Field<String>("member_code") == _loanDetails.MemberCode
                                                        && row.Field<String>("account_code") == _loanDetails.AccountCode)
                                                    .CopyToDataTable();

                loanDetails.TableName = "notice_loan_details";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                    {
                        ReportFile = "notice_loan_overdue.rpt",
                        Title = "Notice for overdue loans",
                        DataSource = dataSet
                    };
                Result result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }

        private void GenerateNoticesForNonResponsiveLoansOverdue()
        {
            try
            {
                DateTime asOf = MainController.LoggedUser.TransactionDate;

                DataTable tableFromSql = DatabaseController.ExecuteStoredProcedure("sp_notice_loan_overdue",
                                                                                   new SqlParameter("as_of", asOf));

                DataTable loanDetails = tableFromSql.AsEnumerable()
                                                    .Where(
                                                        row =>
                                                        row.Field<String>("member_code") == _loanDetails.MemberCode
                                                        && row.Field<String>("account_code") == _loanDetails.AccountCode)
                                                    .CopyToDataTable();

                loanDetails.TableName = "notice_loan_details";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                    {
                        ReportFile = "notice_loan_overdue_non_responsive.rpt",
                        Title = "Notice for overdue loans - Non-responsive",
                        DataSource = dataSet
                    };
                Result result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }

        private void GenerateNoticesForComakers()
        {
            try
            {
                DateTime asOf = MainController.LoggedUser.TransactionDate;

                DataTable tableFromSql = DatabaseController.ExecuteStoredProcedure("sp_notice_comakers",
                                                                                   new SqlParameter("as_of", asOf));

                DataTable loanDetails = tableFromSql.AsEnumerable()
                                                    .Where(
                                                        row =>
                                                        row.Field<String>("member_code") == _loanDetails.MemberCode
                                                        && row.Field<String>("account_code") == _loanDetails.AccountCode)
                                                    .CopyToDataTable();

                loanDetails.TableName = "notice_comakers";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                    {
                        ReportFile = "notice_comakers.rpt",
                        Title = "Notice for Comakers",
                        DataSource = dataSet
                    };
                Result result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception e)
            {
                MessageWindow.ShowAlertMessage(e.Message);
            }
        }
    }
}