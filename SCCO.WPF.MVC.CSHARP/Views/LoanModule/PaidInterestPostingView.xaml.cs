using System;
using SCCO.WPF.MVC.CS.Extensions;
using SCCO.WPF.MVC.CS.Models;
using System.Linq;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public partial class PaidInterestPostingView
    {
        private readonly OfficialReceipt _or;

        public PaidInterestPostingView(OfficialReceipt model)
        {
            InitializeComponent();

            _or = model;
            DataContext = _or;

            btnPost.Click += (s, e) => Post();
            InitializeCollectorsList();
        }

        private void Post()
        {
            if (!IsValid())
            {
                return;
            }
            var result = _or.Create();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                OfficialReceipt.DeleteAll(_or.VoucherNo);
                return;
            }

            var coh = new OfficialReceipt();
            coh.SetAccount(Account.FindByCode(GlobalSettings.CodeOfCashOnHand));
            coh.MemberCode = _or.MemberCode;
            coh.MemberName = _or.MemberName;
            coh.VoucherNo = _or.VoucherNo;
            coh.VoucherDate = _or.VoucherDate;
            coh.Debit = _or.Credit;
            coh.Collector = _or.Collector;
            coh.Amount = _or.Credit;
            coh.AmountInWords = Utilities.Converter.AmountToWords(_or.Credit);
            result = coh.Create();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                OfficialReceipt.DeleteAll(_or.VoucherNo);
                return;
            }

            VoucherLog.Log(VoucherTypes.OR, _or.VoucherNo, _or.VoucherDate, _or.Collector.Initials());

            DialogResult = true;
            MessageWindow.ShowNotifyMessage("Posting successful!");
            Close();
        }

        private bool IsValid()
        {
            if (_or.VoucherNo <= 0)
            {
                MessageWindow.ShowAlertMessage("OR Number is invalid!");
                return false;
            }
            if (Voucher.Exist(VoucherTypes.OR, _or.VoucherNo))
            {
                MessageWindow.ShowAlertMessage("OR Number already in use!");
                return false;
            }
            if (string.IsNullOrEmpty(_or.Collector))
            {
                MessageWindow.ShowAlertMessage("Collector is requried!");
                return false;
            }
            if (_or.Credit <= 0)
            {
                MessageWindow.ShowAlertMessage("Amount is invalid!");
                return false;
            }
            return true;
        }

        private void InitializeCollectorsList()
        {
            var collectors = CollectorCollection.SortedByName();
            foreach (var collector in collectors)
            {
                CollectorsComboBox.Items.Add(collector.CollectorName);
            }
        }
    }
}