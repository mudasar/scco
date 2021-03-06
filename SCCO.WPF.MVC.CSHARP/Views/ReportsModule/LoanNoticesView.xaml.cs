﻿using System;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using System.Data;


namespace SCCO.WPF.MVC.CS.Views.ReportsModule
{
    public partial class LoanNotices
    {
        public LoanNotices()
        {
            InitializeComponent();
            TransactionDatePicker.SelectedDate = Controllers.MainController.LoggedUser.TransactionDate;

            LoansNonperformingButton.Click += (s, e) => GenerateNoticesForLoansNonPerforming();
            LoansNearMaturityButton.Click += (s, e) => GenerateNoticesForLoansNearMaturity();
            LoansOverdueButton.Click += (s, e) => GenerateNoticesForLoansOverdue();
            LoansOverdueNonResponsiveButton.Click += (s, e) => GenerateNoticesForNonResponsiveLoansOverdue();
            LoanNoticeForComakersButton.Click += (s, e) => GenerateNoticesForComakers();

        }

        private void GenerateNoticesForLoansNonPerforming()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_notice_loan_non_performing",
                                                                            new SqlParameter("as_of", asOf));
                loanDetails.TableName = "notice_loan_non_performing";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "notice_loan_non_performing.rpt";
                ri.Title = "Notice for non-performing loans";
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

        private void GenerateNoticesForLoansNearMaturity()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_notice_loan_near_maturity",
                                                                            new SqlParameter("as_of", asOf));
                loanDetails.TableName = "notice_loan_details";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "notice_loan_near_maturity.rpt";
                ri.Title = "Notice for loan near maturity";
                ri.DataSource = dataSet;
                var result = ri.LoadReport();
                if(!result.Success)
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
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_notice_loan_overdue",
                                                                            new SqlParameter("as_of", asOf));
                loanDetails.TableName = "notice_loan_details";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "notice_loan_overdue.rpt";
                ri.Title = "Notice for overdue loans";
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

        private void GenerateNoticesForNonResponsiveLoansOverdue()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_notice_loan_overdue",
                                                                            new SqlParameter("as_of", asOf));
                loanDetails.TableName = "notice_loan_details";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "notice_loan_overdue_non_responsive.rpt";
                ri.Title = "Notice for overdue loans - Non-responsive";
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

        private void GenerateNoticesForComakers()
        {
            try
            {
                if (TransactionDatePicker.SelectedDate == null)
                {
                    MessageWindow.ShowAlertMessage("Please select a date.");
                    return;
                }

                var asOf = (DateTime)TransactionDatePicker.SelectedDate;

                var loanDetails = DatabaseController.ExecuteStoredProcedure("sp_notice_comakers",
                                                                            new SqlParameter("as_of", asOf));
                loanDetails.TableName = "notice_comakers";

                DataTable comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(loanDetails);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem();
                ri.ReportFile = "notice_comakers.rpt";
                ri.Title = "Notice for Comakers";
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
    }
}