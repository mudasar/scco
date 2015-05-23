using System;
using System.Windows;

namespace SCCO.WPF.MVC.CS.Views.Loan
{
    /// <summary>
    /// Interaction logic for LoanTransactionPosting.xaml
    /// </summary>
    public partial class LoanPostingWindow
    {

        public LoanPostingWindow()
        {
            InitializeComponent();
        }

        public LoanPostingWindow(Models.Loan.LoanPostingDetails loanPostingDetails):this()
        {
            _loanPostingDetails = loanPostingDetails;
            DataContext = _loanPostingDetails;
        }

        private void AcceptButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (_loanPostingDetails.VoucherNumber == 0)
            {
                MessageWindow.ShowAlertMessage("Invalid Document Number!");
                return;
            }

            if (_loanPostingDetails.ReleaseNumber == 0)
            {
                MessageWindow.ShowAlertMessage("Invalid Release Number!");
                return;
            }

            if (_loanPostingDetails.ReleaseDate == new DateTime())
            {
                MessageWindow.ShowAlertMessage("Invalid Release Date!");
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private Models.Loan.LoanPostingDetails _loanPostingDetails;
    }
}
