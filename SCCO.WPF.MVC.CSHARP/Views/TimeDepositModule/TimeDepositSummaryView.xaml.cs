using System.Windows;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositSummaryView
    {
        private readonly AccountDetail _accountDetail;
        private bool _hasChanged;

        public TimeDepositSummaryView(AccountDetail accountDetail)
        {
            InitializeComponent();
            _accountDetail = accountDetail;
            DataContext = _accountDetail;

            RefreshButtons();
            RefreshFieldValues();

            btnWithdraw.Click += (sender, args) => Withdraw();
            btnPreterminate.Click += (sender, args) => Withdraw();
            btnRollover.Click += (sender, args) => Rollover();
        }

        private void RefreshFieldValues()
        {
            var asOf = Controllers.MainController.LoggedUser.TransactionDate;
            txtSummaryDate.Content = string.Format("{0:yyyy-MMM-dd}", asOf);
            txtStatus.Content = _accountDetail.TimeDepositDetails.IsPremature(asOf) ? "Pre-Mature" : "Mature";
            var timeDepositDetails = _accountDetail.TimeDepositDetails;
            var interestEarned = timeDepositDetails.CalculateInterestEarned(asOf);
            txtInterestEarned.Content = string.Format("{0:N2}", interestEarned);

            var serviceFee = timeDepositDetails.CalculateServiceFee(asOf);
            txtServiceFee.Content = string.Format("{0:N2}", serviceFee);

            txtEndingBalance.Content =
                string.Format("{0:N2}", _accountDetail.TimeDepositDetails.EndingBalance(asOf));
        }

        private void Withdraw()
        {
            // Show TD Withdrawal
            var view = new TimeDepositWithdrawalView(_accountDetail);
            if (view.ShowDialog() == true)
            {
                _hasChanged = true;
                DialogResult = true;
            }
        }

        public bool HasChanged {
            get { return _hasChanged; }
        }

        private void Rollover()
        {
            // Show TD Rollover Window
            var view = new TimeDepositRolloverView(_accountDetail);
            if (view.ShowDialog() == true)
            {
                _hasChanged = true;
                DialogResult = true;
            }
        }

        private void PrintCertificate()
        {
            // Show Print Preview of CTD
        }

        private void Preterminate()
        {
            // Show TD Preterminate Window
        }


        private void RefreshButtons()
        {
            if (_accountDetail.TimeDepositDetails.IsPremature(GlobalSettings.DateOfOpenTransaction))
            {
                btnRollover.Visibility = Visibility.Collapsed;
                btnWithdraw.Visibility = Visibility.Collapsed;
                btnPreterminate.Visibility = Visibility.Visible;
            }
            else
            {
                btnRollover.Visibility = Visibility.Visible;
                btnWithdraw.Visibility = Visibility.Visible;
                btnPreterminate.Visibility = Visibility.Collapsed;
            }
        }
    }
}
