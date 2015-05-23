using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class GeneralLedgerReportsWindow
    {
        public GeneralLedgerReportsWindow()
        {
            InitializeComponent();

            TransactionDatePicker.SelectedDate = MainController.LoggedUser.TransactionDate;

            chkForTheDay.Checked += (sender, args) =>
                                        {
                                            chkForTheMonth.IsChecked = !chkForTheDay.IsChecked;
                                        };
            chkForTheMonth.Checked += (sender, args) => { chkForTheDay.IsChecked = !chkForTheMonth.IsChecked; };
        }

        private void btnStatementOfFinancialCondition_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionDatePicker.SelectedDate == null)
            {
                MessageWindow.ShowAlertMessage("Please select a date.");
                return;
            }
            var asOf = (System.DateTime) TransactionDatePicker.SelectedDate;

            var result = ReportController.GenerateStatementFinancialCondition(asOf);

            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        private void btnStatementOfOperation_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionDatePicker.SelectedDate == null)
            {
                MessageWindow.ShowAlertMessage("Please select a date.");
                return;
            }
            var asOf = (System.DateTime)TransactionDatePicker.SelectedDate;

            var result = ReportController.GenerateStatementOfOperation(asOf);

            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        private void btnashFlow_Click(object sender, RoutedEventArgs e)
        {
            ReportController.GenerateStatementCashFlow();
        }
    }
}
