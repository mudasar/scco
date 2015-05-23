using System;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class PostTimeDepositView
    {
        public PostTimeDepositView()
        {
            InitializeComponent();
            btnPost.Click += btnPost_Click;
            btnDeno.Click += btnDeno_Click;
        }

        private readonly OfficialReceipt _officialReceipt;
        private CashAndCheckBreakDown _denomonation;
        private readonly Nfmb _member;
        private readonly TimeDepositViewModel _viewModel;

        public PostTimeDepositView(Nfmb member, TimeDepositViewModel viewModel) : this()
        {
            _officialReceipt = new OfficialReceipt();
            _member = member;
            _viewModel = viewModel;
            _officialReceipt.VoucherNo = Voucher.LastDocumentNo(VoucherTypes.OR) + 1;
            DataContext = _officialReceipt;
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            var collection = OfficialReceipt.WhereDocumentNumberIs(_officialReceipt.VoucherNo);
            if (collection.Count > 0)
            {
                MessageWindow.ShowAlertMessage("OR No. already in use.");
                return;
            }
            try
            {
                _officialReceipt.MemberCode = _member.MemberCode;
                _officialReceipt.MemberName = _member.MemberName;

                var td = Account.FindByCode(_viewModel.SelectedItem.ProductCode);
                _officialReceipt.AccountCode = td.AccountCode;
                _officialReceipt.AccountTitle = td.AccountTitle;

                _officialReceipt.VoucherDate = _viewModel.DateIn;
                //_officialReceipt.VoucherNo = _officialReceipt.VoucherNo; // binded to textbox
                _officialReceipt.VoucherType = VoucherTypes.OR;

                _officialReceipt.Credit = _viewModel.Amount;

                _officialReceipt.Collector = MainController.LoggedUser.CollectorName;
                _officialReceipt.IsPosted = true;

                _officialReceipt.TimeDepositDetails = _viewModel.GenerateTimeDepositDetails();
                _officialReceipt.Create();

                // cash on hand
                var or = new OfficialReceipt
                             {MemberCode = _officialReceipt.MemberCode, MemberName = _officialReceipt.MemberName};

                var coh = Account.FindByCode(GlobalSettings.CodeOfCashOnHand);
                or.AccountCode = coh.AccountCode;
                or.AccountTitle = coh.AccountTitle;

                // insert denomination details
                or.Debit = _officialReceipt.Credit;

                if (_denomonation != null)
                {
                    #region --- Cash CashAndCheckBreakDown ---

                    //JEA: Since deno09 is not available (0.50), I use it as lieu to 200
                    or.Deno01 = _denomonation.Deno01; //1000
                    or.Deno02 = _denomonation.Deno02; //500
                    or.Deno03 = _denomonation.Deno03; //100
                    or.Deno04 = _denomonation.Deno04; //50
                    or.Deno05 = _denomonation.Deno05; //20
                    or.Deno06 = _denomonation.Deno06; //10
                    or.Deno07 = _denomonation.Deno07; //5
                    or.Deno08 = _denomonation.Deno08; //1
                    or.Deno09 = _denomonation.Deno09; //.5 -> 200
                    or.Deno10 = _denomonation.Deno10; //.25

                    #endregion --- Cash CashAndCheckBreakDown ---

                    #region --- Check CashAndCheckBreakDown ---

                    or.BankName1 = _denomonation.BankName1;
                    or.BankDate1 = _denomonation.BankDate1;
                    or.BankCheck1 = _denomonation.BankCheck1;
                    or.BankAmount1 = _denomonation.BankAmount1;

                    or.BankName2 = _denomonation.BankName2;
                    or.BankDate2 = _denomonation.BankDate2;
                    or.BankCheck2 = _denomonation.BankCheck2;
                    or.BankAmount2 = _denomonation.BankAmount2;

                    or.BankName3 = _denomonation.BankName3;
                    or.BankDate3 = _denomonation.BankDate3;
                    or.BankCheck3 = _denomonation.BankCheck3;
                    or.BankAmount3 = _denomonation.BankAmount3;

                    or.BankName4 = _denomonation.BankName4;
                    or.BankDate4 = _denomonation.BankDate4;
                    or.BankCheck4 = _denomonation.BankCheck4;
                    or.BankAmount4 = _denomonation.BankAmount4;

                    or.BankName5 = _denomonation.BankName5;
                    or.BankDate5 = _denomonation.BankDate5;
                    or.BankCheck5 = _denomonation.BankCheck5;
                    or.BankAmount5 = _denomonation.BankAmount5;

                    #endregion
                }

                or.AmountInWords = Converter.AmountToWords(_officialReceipt.Credit);
                or.Amount = _officialReceipt.Credit;

                or.VoucherDate = _officialReceipt.VoucherDate;
                or.VoucherNo = _officialReceipt.VoucherNo;
                or.VoucherType = VoucherTypes.OR;

                or.Collector = MainController.LoggedUser.CollectorName;
                or.IsPosted = true;

                or.Create();

                #region --- Voucher Log ---

                var voucherLog = new VoucherLog();
                voucherLog.Find("OR", _officialReceipt.VoucherNo);
                voucherLog.Date = _officialReceipt.VoucherDate;
                voucherLog.Initials = MainController.LoggedUser.Initials;
                if (_denomonation != null && _denomonation.HasCheckDeposit)
                {
                    voucherLog.Remarks = "CHK";
                }

                voucherLog.Save();

                #endregion

                MessageWindow.ShowNotifyMessage("Transaction posted!");
                DialogResult = true;
                Close();
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }

        private void btnDeno_Click(object sender, EventArgs e)
        {
            var cashAndCheckBreakDown = new CashAndCheckBreakDown();
            var denomination = new CashAndCheckBreakdownWindow(cashAndCheckBreakDown);
            if (denomination.ShowDialog() == true)
            {
                _denomonation = cashAndCheckBreakDown;
            }
        }
    }
}