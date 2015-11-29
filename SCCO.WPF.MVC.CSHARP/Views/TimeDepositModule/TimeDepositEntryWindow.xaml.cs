using System.Windows;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositEntryWindow
    {
        private readonly AccountDetail _accountDetail;
        private bool _hasChanged;

        public TimeDepositEntryWindow(AccountDetail accountDetail)
        {
            InitializeComponent();
            _accountDetail = accountDetail;
            DataContext = _accountDetail;

            RefreshButtons();
            RefreshFieldValues();

            btnWithdraw.Click += (sender, args) => Withdraw();
        }

        private void RefreshFieldValues()
        {
            var transactionDate = Controllers.MainController.LoggedUser.TransactionDate;
            var timeDepositDetails = _accountDetail.TimeDepositDetails;
            var interestEarned = timeDepositDetails.CalculateInterestEarned(transactionDate);
            txtInterestEarned.Content = string.Format("{0:N2}", interestEarned);

            var serviceFee = timeDepositDetails.CalculateServiceFee(transactionDate);
            txtServiceFee.Content = string.Format("{0:N2}", serviceFee);
        }

        private void Withdraw()
        {
            // Show TD Withdrawal
            var view = new TimeDepositWithdrawalWindow(_accountDetail);
            if (view.ShowDialog() == true)
            {
                _hasChanged = true;
                DialogResult = true;
            }
        }

        public bool HasChanged {
            get { return _hasChanged; }
        }

        private void RollOver()
        {
            // Show TD RollOver Window
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
