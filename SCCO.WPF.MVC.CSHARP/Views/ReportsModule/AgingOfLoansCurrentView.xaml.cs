using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Extensions;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.ReportsModule
{
    public partial class AgingOfLoansCurrentView
    {
        private readonly DateTime _asOf;
        private readonly List<ReportData> _reportData;

        public AgingOfLoansCurrentView(List<ReportData> reportData, DateTime asOf)
        {
            InitializeComponent();
            _reportData = reportData;
            _asOf = asOf;
            SubTitleLabel.Content = string.Format("As of {0:MMMM dd, yyyy}", _asOf);
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            Button1.Click += (s, e) => ShowReportNormal();
            Button2.Click += (s, e) => ShowReportByArea();
            Button3.Click += (s, e) => ShowReportByAreaSummary();
        }

        private void ShowReportNormal()
        {
            try
            {
                var filteredData = ByCodeRadioButton.IsChecked == true
                                       ? _reportData.Where(t => t.Status == "Current")
                                                    .OrderBy(t => t.MemberCode)
                                                    .ToList()
                                       : _reportData.Where(t => t.Status == "Current")
                                                    .OrderBy(t => t.MemberName)
                                                    .ToList();

                var reportTable = filteredData.ToDataTable();
                reportTable.TableName = "loan_report_data";
                var reportTitle = string.Format("Aging Of Current Loans As Of {0:MMMM dd, yyyy}", _asOf);

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(reportTable);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "aging_of_loans.rpt",
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

        private void ShowReportByArea()
        {
            try
            {
                var filteredData = ByCodeRadioButton.IsChecked == true
                                       ? _reportData.Where(t => t.Status == "Current")
                                                    .OrderBy(t => t.MemberCode)
                                                    .ToList()
                                       : _reportData.Where(t => t.Status == "Current")
                                                    .OrderBy(t => t.MemberName)
                                                    .ToList();

                var reportTable = filteredData.ToDataTable();
                reportTable.TableName = "loan_report_data";
                var reportTitle = string.Format("Aging Of Current Loans By Area As Of {0:MMMM dd, yyyy}", _asOf);

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(reportTable);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "aging_of_loans_by_area.rpt",
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

        private void ShowReportByAreaSummary()
        {
            try
            {
                var filteredData = ByCodeRadioButton.IsChecked == true
                                       ? _reportData.Where(t => t.Status == "Current")
                                                    .OrderBy(t => t.MemberCode)
                                                    .ToList()
                                       : _reportData.Where(t => t.Status == "Current")
                                                    .OrderBy(t => t.MemberName)
                                                    .ToList();

                var reportTable = filteredData.ToDataTable();
                reportTable.TableName = "loan_report_data";
                var reportTitle = string.Format("Aging Of Current Loans Summary By Area As Of {0:MMMM dd, yyyy}", _asOf);

                var comp = Company.GetData();
                comp.TableName = "comp";

                var dataSet = new DataSet();
                dataSet.Tables.Add(reportTable);
                dataSet.Tables.Add(comp);

                var ri = new ReportItem
                {
                    ReportFile = "aging_of_loans_by_area_summary.rpt",
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
    }
}