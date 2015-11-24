using System;
using System.Text;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.SavingsDeposit;

namespace SCCO.WPF.MVC.CS.Views.SavingsDepositModule
{
    /// <summary>
    /// Interaction logic for WithdrawalView.xaml
    /// </summary>
    public partial class WithdrawalView
    {
        private readonly Withdrawal _withdrawal;

        public WithdrawalView(Withdrawal withdrawal)
        {
            InitializeComponent();
            _withdrawal = withdrawal;
            DataContext = _withdrawal;
        }

        private void PostButtonOnClick(object sender, RoutedEventArgs e)
        {
            var result = _withdrawal.Validate();
           if(!result.Success)
           {
               MessageWindow.ShowAlertMessage(result.Message);
               return;
           }

           #region --- Withdrawal Credit Side ---

           var voucher = new Voucher
               {
                   VoucherDate = _withdrawal.WithdrawalSettings.TransactionDate,
                   VoucherNo = _withdrawal.WithdrawalSettings.WithdrawalVoucherNo,
                   VoucherType = VoucherTypes.CV
               };
            var totalWithdrawals = Withdrawal.TotalWithdrawals(voucher.VoucherNo) + _withdrawal.WithdrawalAmount;
            result = Withdrawal.ReBalanceWithdrawals(voucher, totalWithdrawals);
           if (!result.Success)
           {
               MessageWindow.ShowAlertMessage(result.Message);
               return;
           }

           #endregion

            #region --- Withdrawal Debit Side ---

            var cv = new CashVoucher();
            var member = Nfmb.FindByCode(_withdrawal.AccountInfo.MemberCode);
            cv.MemberCode = member.MemberCode;
            cv.MemberName = member.MemberName;
            var account = Account.FindByCode(_withdrawal.AccountInfo.AccountCode);
            cv.AccountCode = account.AccountCode;
            cv.AccountTitle = account.AccountTitle;

            cv.Debit = _withdrawal.WithdrawalAmount;

            cv.VoucherDate = voucher.VoucherDate;
            cv.VoucherNo = voucher.VoucherNo;
            cv.VoucherType = voucher.VoucherType;

            cv.WithdrawalSlipNo = _withdrawal.WithdrawalSlipNo;

            cv.Create();

            #endregion

            #region --- Voucher Log ---

            var voucherLog = new VoucherLog();
            voucherLog.Find("CV", cv.VoucherNo);
            voucherLog.Date = cv.VoucherDate;
            voucherLog.Initials = MainController.LoggedUser.Initials;
            voucherLog.Save();

            #endregion

            var validationBuilder = new StringBuilder();
            validationBuilder.AppendFormat("{0} {1} {2} {3} {4} {5:yyyy-MM-dd hh:mm:ss} {6}",
                cv.MemberCode,
                cv.AccountCode,
                cv.VoucherNo,
                cv.WithdrawalSlipNo, 
                cv.Debit,
                DateTime.Now,
                MainController.LoggedUser.Initials);

            if (MessageWindow.ShowConfirmMessage("Print withdrawal validation?") == MessageBoxResult.Yes)
            {
                ReportController.PrintWitdrawalValidation(cv);
            }

            DialogResult = true;
            Close();
        }

        private void AmountBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            decimal amount;
            try
            {
                amount = Convert.ToDecimal(AmountBox.Text);
            }
            catch (Exception)
            {
                amount = 0;
            }

            AmountBox.Text = string.Format("{0:N2}", amount);
        }
    }
}
