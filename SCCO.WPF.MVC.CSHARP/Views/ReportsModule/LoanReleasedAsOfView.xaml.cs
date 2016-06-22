using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Extensions;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.ReportsModule
{
    public partial class LoanReleasedAsOfView
    {
        private readonly DateTime _asOf;
        private readonly List<ReportData> _reportData;

        public LoanReleasedAsOfView(List<ReportData> reportData, DateTime asOf)
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
        }

        private void ShowReportNormal()
        {
            try
            {
                var filteredData = ByCodeRadioButton.IsChecked == true
                                       ? _reportData.Where(t => t.DocumentDate <= _asOf)
                                                    .OrderBy(t => t.MemberCode)
                                                    .ToList()
                                       : _reportData.Where(t => t.DocumentDate <= _asOf)
                                                    .OrderBy(t => t.MemberName)
                                                    .ToList();

                var reportTable = filteredData.ToDataTable();
                reportTable.TableName = "loan_report_data";
                var reportTitle = string.Format("Loans Released As Of {0:MMMM dd, yyyy}", _asOf);
                ShowReport(reportTable, reportTitle);
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