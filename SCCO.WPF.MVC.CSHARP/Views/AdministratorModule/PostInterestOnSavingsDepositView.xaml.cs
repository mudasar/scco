using System;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public partial class PostInterestOnSavingsDepositView
    {
        private readonly JournalVoucher _journalVoucher;
        //private readonly VoucherCollection _voucherCollection;
        private readonly InterestOnSavingsDepositViewModel _viewModel;

        public PostInterestOnSavingsDepositView(InterestOnSavingsDepositViewModel  viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            _journalVoucher = new JournalVoucher();
            _journalVoucher.VoucherNo = Voucher.LastDocumentNo(VoucherTypes.JV) + 1;
            _journalVoucher.VoucherDate = GlobalSettings.DateOfOpenTransaction;

            DataContext = _journalVoucher;

            btnPost.Click += btnPost_Click;
        }



        private void btnPost_Click(object sender, EventArgs e)
        {
            var collection = JournalVoucher.FindByDocumentNumber(_journalVoucher.VoucherNo);
            if (collection.Count > 0)
            {
                MessageWindow.ShowAlertMessage("JV No. already in use.");
                return;
            }
            try
            {
                btnPost.Content = "Posting, please wait...";

                JournalVoucher jv;
                foreach (var item in _viewModel.Collection)
                {
                    jv = new JournalVoucher();

                    jv.MemberCode = item.MemberCode;
                    jv.MemberName = item.MemberName;
                    jv.AccountCode = item.AccountCode;
                    jv.AccountTitle = item.AccountTitle;
                    jv.Credit = item.Credit;

                    jv.VoucherDate = _journalVoucher.VoucherDate;
                    jv.VoucherNo = _journalVoucher.VoucherNo;
                    jv.VoucherType = VoucherTypes.JV;

                    jv.IsPosted = true;
                    jv.Create();
                }

                // closing
                jv  = new JournalVoucher();

                var coop = Nfmb.FindByCode(GlobalSettings.CodeOfCompany);
                jv.MemberCode = coop.MemberCode;
                jv.MemberName = coop.MemberName;
                var intExpense = _viewModel.InterestExpenseOnSavingsDepositAccount;
                jv.AccountCode = intExpense.AccountCode;
                jv.AccountTitle = intExpense.AccountTitle;
                jv.Debit = _viewModel.Collection.Sum(voucher => voucher.Credit);
                
                jv.VoucherDate = _journalVoucher.VoucherDate;
                jv.VoucherNo = _journalVoucher.VoucherNo;
                jv.VoucherType = VoucherTypes.JV;

                jv.Explanation = "Savings Deposit interest posting";

                jv.IsPosted = true;
                jv.Create();

                #region --- Voucher Log ---

                var voucherLog = new VoucherLog();
                voucherLog.Find("JV", _journalVoucher.VoucherNo);
                voucherLog.Date = _journalVoucher.VoucherDate;
                voucherLog.Initials = MainController.LoggedUser.Initials;
                voucherLog.Save();

                #endregion

                btnPost.Content = string.Format("Posting Complete");
                MessageWindow.ShowNotifyMessage("Interest on Savings Deposit succesfully posted!");
                DialogResult = true;
                Close();
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }
    }
}