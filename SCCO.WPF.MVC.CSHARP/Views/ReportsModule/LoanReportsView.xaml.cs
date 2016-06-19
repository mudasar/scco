using System;
using System.Collections.Generic;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.ReportsModule
{
    public partial class LoanReportsView
    {
        private DateTime _asOf;
        private List<ReportData> _reportData;

        public LoanReportsView()
        {
            InitializeComponent();
            TransactionDatePicker.SelectedDate = MainController.LoggedUser.TransactionDate;

            LoanReleasedAsOfButton.Click += (s, e) => ShowLoanReleasedAsOf();
            LoanReleasedForTheMonthButton.Click += (s, e) => ShowLoanReleasedForTheMonth();

            AgingOfLoansCurrentButton.Click += (s, e) => ShowAgingOfLoansCurrent();
        }

        private void ShowAgingOfLoansCurrent()
        {
            if (!ValidTransactionDate()) return;
            _reportData = ReportData.GetLoanDetails(_asOf);

            var view = new AgingOfLoansCurrentView(_reportData, _asOf);
            view.ShowDialog();
        }

        private void ShowLoanReleasedAsOf()
        {
            if (!ValidTransactionDate()) return;
            _reportData = ReportData.GetLoanReleases();

            var view = new LoanReleasedAsOfView(_reportData, _asOf);
            view.ShowDialog();
        }

        private void ShowLoanReleasedForTheMonth()
        {
            if (!ValidTransactionDate()) return;
            _reportData = ReportData.GetLoanReleases();

            var view = new LoanReleasedForTheMonthView(_reportData, _asOf);
            view.ShowDialog();
        }

        private bool ValidTransactionDate()
        {
            if (TransactionDatePicker.SelectedDate == null)
            {
                MessageWindow.ShowAlertMessage("Please select a date.");
                return false;
            }
            _asOf = (DateTime) TransactionDatePicker.SelectedDate;
            return true;
        }
    }
}