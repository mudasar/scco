using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class GeneralLedgerReportsWindow
    {
        private DateTime _asOf;

        public GeneralLedgerReportsWindow()
        {
            InitializeComponent();
            TransactionDatePicker.SelectedDate = MainController.LoggedUser.TransactionDate;
            WireUpEvents();
        }

        private void ShowFinancialConditionReport()
        {
            if (!IsDateValid()) return;

            var result = ReportController.GenerateStatementFinancialCondition(_asOf);
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        private void ShowFinancialOperationReport()
        {
            if (!IsDateValid()) return;

            var result = ReportController.GenerateStatementOfOperation(_asOf);
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        private void btnashFlow_Click(object sender, RoutedEventArgs e)
        {
            ReportController.GenerateStatementCashFlow();
        }

        private bool IsDateValid()
        {
            if (TransactionDatePicker.SelectedDate == null)
            {
                MessageWindow.ShowAlertMessage("Please select a date.");
                return false;
            }
            _asOf = (DateTime) TransactionDatePicker.SelectedDate;
            return true;
        }

        private void WireUpEvents()
        {
            FinancialStatementReportsButton.Click += (s, e) => GenerateFinancialStatementExcelReport();
            EditFinancialStatementReportsTemplateButton.Click += (s, e) => EditFinancialStatementReportsTemplate();

            // old report
            btnFinancialOperation.Click += (s, e) => ShowFinancialOperationReport();
            btnFinancialCondition.Click += (s, e) => ShowFinancialConditionReport();
        }

        private void GenerateFinancialStatementExcelReport()
        {
            if (!IsDateValid()) return;

            var dlg = new SaveFileDialog
            {
                FileName = string.Format("Financial Statement ({0:yyyy-MM-dd})", _asOf),
                DefaultExt = ".xlsx",
                Filter = "Excel documents (.xlsx)|*.xlsx"
            };

            if (dlg.ShowDialog() != true) return;
            Mouse.OverrideCursor = Cursors.Wait;
            var result = FinancialReportExcelCreator.GenerateFinancialStatementReport(_asOf, dlg.FileName);
            Mouse.OverrideCursor = Cursors.Arrow;
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }

            var confirmMessage = string.Format("{0} created. Do you want to open it?", Path.GetFileName(dlg.FileName));
            if (MessageWindow.ShowConfirmMessage(confirmMessage) == MessageBoxResult.Yes)
            {
                Process.Start(dlg.FileName);
            }
        }

        private static void EditFinancialStatementReportsTemplate()
        {
            var template = FinancialReportExcelCreator.GetFinancialStatementReportsTemplate();
            if (File.Exists(template))
            {
                Process.Start(template);
            }
        }
    }
}