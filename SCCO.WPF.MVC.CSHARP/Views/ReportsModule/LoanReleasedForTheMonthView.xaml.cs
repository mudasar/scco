using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Extensions;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.ReportsModule
{
    public partial class LoanReleasedForTheMonthView
    {
        private readonly List<ReportData> _reportData;
        private DateTime _asOf;

        public LoanReleasedForTheMonthView(List<ReportData> reportData, DateTime asOf)
        {
            InitializeComponent();
            _reportData = reportData;
            _asOf = asOf;
            SubTitleLabel.Content = string.Format("For the month of {0:MMMM yyyy}", _asOf);
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            Button1.Click += (sender, args) => ShowLoanReleasedDetailed();
            Button2.Click += (sender, args) => ShowLoanReleasedConsolidatedSummary();
            Button3.Click += (sender, args) => ShowLoanReleasedConsolidatedActual();
            Button4.Click += (sender, args) => ShowLoanReleasedConsolidatedReconstructed();
        }

        private void ShowLoanReleasedDetailed()
        {
            try
            {
                var filteredData = ByCodeRadioButton.IsChecked == true
                       ? _reportData.Where(t => t.DocumentDate.Month == _asOf.Month)
                                    .OrderBy(t => t.MemberCode)
                                    .ToList()
                       : _reportData.Where(t => t.DocumentDate.Month == _asOf.Month)
                                    .OrderBy(t => t.MemberName)
                                    .ToList();

                var reportTable = filteredData.ToDataTable();
                reportTable.TableName = "loan_report_data";
                var reportTitle = string.Format("Loans Released For The Month Of {0:MMMM yyyy}", _asOf);
                ShowReport(reportTable, reportTitle);
            }
            catch (Exception ex)
            {
                MessageWindow.ShowAlertMessage(ex.Message);
            }
        }

        private void ShowLoanReleasedConsolidatedSummary()
        {
            try
            {
                var filteredData = _reportData.Where(t => t.DocumentDate.Month == _asOf.Month).ToList();

                var reportTable = filteredData.ToDataTable();
                reportTable.TableName = "loan_report_data";
                var reportTitle = string.Format("Loans Released Summary For The Month Of {0:MMMM yyyy}", _asOf);
                
                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(reportTable);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "loan_released_consolidated.rpt",
                    Title = reportTitle,
                    DataSource = dataSet
                };
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception ex)
            {
                MessageWindow.ShowAlertMessage(ex.Message);
            }
        }

        private void ShowLoanReleasedConsolidatedActual()
        {
            try
            {
                var filteredData = _reportData.Where(t => t.DocumentDate.Month == _asOf.Month && t.DocumentType == "CV").ToList();

                var reportTable = filteredData.ToDataTable();
                reportTable.TableName = "loan_report_data";
                var reportTitle = string.Format("Loans Released Summary (Actual) For The Month Of {0:MMMM yyyy}", _asOf);

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(reportTable);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "loan_released_consolidated.rpt",
                    Title = reportTitle,
                    DataSource = dataSet
                };
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception ex)
            {
                MessageWindow.ShowAlertMessage(ex.Message);
            }
        }

        private void ShowLoanReleasedConsolidatedReconstructed()
        {
            try
            {
                var filteredData =
                    _reportData.Where(t => t.DocumentDate.Month == _asOf.Month && t.DocumentType == "JV").ToList();

                var reportTable = filteredData.ToDataTable();
                reportTable.TableName = "loan_report_data";
                var reportTitle = string.Format("Loans Released Summary (Reconstructed) For The Month Of {0:MMMM yyyy}", _asOf);

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(reportTable);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "loan_released_consolidated.rpt",
                    Title = reportTitle,
                    DataSource = dataSet
                };
                var result = ri.LoadReport();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            catch (Exception ex)
            {
                MessageWindow.ShowAlertMessage(ex.Message);
            }
        }

        private void ShowReport(DataTable loanReportData, string reportTitle)
        {
            var comp = Company.GetData();
            comp.TableName = "comp";

            var dataSet = new DataSet();
            dataSet.Tables.Add(loanReportData);
            dataSet.Tables.Add(comp);

            var ri = new ReportItem
            {
                ReportFile = "loan_released.rpt",
                Title = reportTitle,
                DataSource = dataSet
            };
            var result = ri.LoadReport();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }
    }
}