using System;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using System.Data;


namespace SCCO.WPF.MVC.CS.Views.ReportsModule
{
    public partial class LoanReportsView
    {
        public LoanReportsView()
        {
            InitializeComponent();
            TransactionDatePicker.SelectedDate = Controllers.MainController.LoggedUser.TransactionDate;
            
            btnScheduleOfLoans.Click += (s, e) => ShowScheduleOfLoansReport();
            btnAgingOfLoans.Click += (s, e) => ShowAgingOfLoansReport();

            btnLoanReleasesDetailed.Click += (s, e) => ShowLoanReleasesDetailedReport();
            btnLoanReleasesSummary.Click += (s, e) => ShowLoanReleasesSummaryReport();

            btnLoanNonPerforming.Click += (s, e) => ShowLoanNonPerformingReport();
        }

        private void ShowLoanNonPerformingReport()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_loan_non_performing",
                                                                            new SqlParameter("as_of", asOf));
                loanDetails.TableName = "loan_non_performing";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "loan_non_performing.rpt";
                ri.Title = "Non-Performing Loans as of " + asOf.ToString("MMMM dd, yyyy");
                ri.DataSource = dataSet;
                var result = ri.LoadReport();
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

        private void ShowLoanReleasesSummaryReport()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_loan_released_summary",
                                                                            new SqlParameter("as_of", asOf));
                loanDetails.TableName = "loan_released_summary";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "loan_released_summary.rpt";
                ri.Title = "Loan Released - Summary " + String.Format("(for the month of {0:MMMM yyyy})", asOf);
                ri.DataSource = dataSet;
                var result = ri.LoadReport();
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

        private void ShowLoanReleasesDetailedReport()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_loan_released_detail",
                                                                            new SqlParameter("as_of", asOf));
                loanDetails.TableName = "loan_released_detail";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "loan_released_detail.rpt";
                ri.Title = "Loan Released - Detail " + String.Format("(for the month of {0:MMMM yyyy})", asOf);
                ri.DataSource = dataSet;
                var result = ri.LoadReport();
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

        private void ShowScheduleOfLoansReport()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var ri = new ReportItem();
                ri.Category = "SCHEDULES";
                ri.Description = "Shows a list of loans arranged by Member Name";
                ri.Title = "Schedule of Loans";
                ri.ReportFile = "schedule.rpt";
                ri.StoredProcedure = "sp_schedule_loan_by_member_name";
                ri.StoredProcedureParameters = new[] {new SqlParameter("as_of", asOf)};
                var result = ri.LoadReport();
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

        private void ShowAgingOfLoansReport()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime) TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_loan_details", new SqlParameter("as_of", asOf));
                loanDetails.TableName = "loan_details";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "aging_of_loans.rpt";
                ri.Title = "Aging of Loans as of " + asOf.ToString("MMMM dd, yyyy");
                ri.DataSource = dataSet;
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch(Exception ex)
            {
                MessageWindow.ShowAlertMessage(ex.Message);
            }
        }
    }
}