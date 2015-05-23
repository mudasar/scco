using System;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class VoucherReportWindow
    {
        private readonly VoucherReportViewModel _viewModel;

        public VoucherReportWindow(VoucherTypes voucherType, int voucherNumber)
        {
            InitializeComponent();
            _viewModel = new VoucherReportViewModel();
            _viewModel.Initialize();
            _viewModel.VoucherNumber = voucherNumber;
            _viewModel.VoucherType = voucherType;

            DataContext = _viewModel;

            if (voucherType == VoucherTypes.OR)
            {
                AttachmentReportButton.Visibility = Visibility.Collapsed;
                CollectorPanel.Visibility = Visibility.Visible;
                CollectorDetailedReportButton.Click += (sender, e) => GenerateCollectorDetailedReport();
                CollectorSummaryReportButton.Click += (sender, e) => GenerateCollectorSummaryReport();
            }
            else
            {
                AttachmentReportButton.Visibility = Visibility.Visible;
                CollectorPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void GenerateCollectorDetailedReport()
        {
            ReportController.OfficialReceiptReports.PerCollectorDetailedReport(_viewModel);
        }
        private void GenerateCollectorSummaryReport()
        {
            ReportController.OfficialReceiptReports.PerCollectorSummaryReport(_viewModel);
        }
        private bool HasValidDate()
        {
            if (TransactionDatePicker.SelectedDate == null)
            {
                MessageWindow.ShowAlertMessage("Please select date.");
                return false;
            }
            return true;
        }

        private void ShowAttachmentReport(object sender, EventArgs e)
        {
            UpdateDateRange();

            switch (_viewModel.VoucherType)
            {
                case VoucherTypes.CV:
                    ReportController.CashVoucherReports.Attachment(_viewModel.VoucherNumber);
                    break;
                case VoucherTypes.JV:
                    ReportController.JournalVoucherReports.Attachment(_viewModel.VoucherNumber);
                    break;
                case VoucherTypes.OR:
                    ReportController.OfficialReceiptReports.Attachment(_viewModel.VoucherNumber);
                    break;
            }
        }

        private void ShowDetailedReport(object sender, EventArgs e)
        {
            UpdateDateRange();

            switch (_viewModel.VoucherType)
            {
                case VoucherTypes.CV:
                    ReportController.CashVoucherReports.DetailedReport(_viewModel.DateRange[0], _viewModel.DateRange[1]);
                    break;
                case VoucherTypes.JV:
                    ReportController.JournalVoucherReports.DetailedReport(_viewModel.DateRange[0], _viewModel.DateRange[1]);
                    break;
                case VoucherTypes.OR:
                    ReportController.OfficialReceiptReports.DetailedReport(_viewModel.DateRange[0], _viewModel.DateRange[1]);
                    break;
            }
        }

        private void ShowPerAccountReport(object sender, EventArgs e)
        {
            UpdateDateRange();
            string accountCode = txtAccountCode.Text;
            if (string.IsNullOrEmpty(accountCode))
            {
                MessageWindow.ShowAlertMessage("Please enter an account code!");
                return;
            }
            switch (_viewModel.VoucherType)
            {
                case VoucherTypes.CV:
                    ReportController.CashVoucherReports.PerAccount(_viewModel);
                    break;
                case VoucherTypes.JV:
                    ReportController.JournalVoucherReports.PerAccount(_viewModel);
                    break;
                case VoucherTypes.OR:
                    ReportController.OfficialReceiptReports.PerAccount(_viewModel);
                    break;
            }

        }

        private void ShowSummaryReport(object sender, RoutedEventArgs e)
        {
            UpdateDateRange();
            switch (_viewModel.VoucherType)
            {
                case VoucherTypes.CV:
                    ReportController.CashVoucherReports.SummaryReport(_viewModel);
                    break;
                case VoucherTypes.JV:
                    ReportController.JournalVoucherReports.SummaryReport(_viewModel);
                    break;
                case VoucherTypes.OR:
                    ReportController.OfficialReceiptReports.SummaryReport(_viewModel);
                    break;
            }
        }

        private void UpdateDateRange()
        {
            if (!HasValidDate()) return;

            _viewModel.UpdateReportRange();
            //if (TransactionDatePicker.SelectedDate == null) return;
            //var selectedDate = (DateTime) TransactionDatePicker.SelectedDate;

            //if (ReportRangeComboBox.SelectedIndex > 0)
            //{
            //    int year = selectedDate.Year;
            //    int month = selectedDate.Month;
            //    int days = DateTime.DaysInMonth(year, month);

            //    _dateStart = new DateTime(year, month, 1);
            //    _dateEnd = new DateTime(year, month, days);
            //}
            //else
            //{
            //    _dateEnd = _dateStart = selectedDate;
            //}
        }
    }
}