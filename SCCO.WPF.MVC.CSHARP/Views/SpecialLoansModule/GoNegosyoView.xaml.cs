using System;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    public partial class GoNegosyoView
    {
        private readonly CashVoucher _cashVoucher;
        private readonly Nfmb _member;
        private readonly LoanProduct _loanProduct;

        public GoNegosyoView(Nfmb member)
        {
            InitializeComponent();
            _member = member;

            if (MainController.LoggedUser.TransactionDate != GlobalSettings.DateOfOpenTransaction)
            {
                MessageWindow.ShowAlertMessage("Cannot create transactions using current date settings.");
                btnDetails.IsEnabled = btnPost.IsEnabled = false;
                return;
            }

            var code = GlobalSettings.CodeOfGoNegosyo;
            if (string.IsNullOrEmpty(code))
            {
                MessageWindow.ShowAlertMessage("Go Negosyo code not set!");
                btnDetails.IsEnabled = btnPost.IsEnabled = false;
                return;
            }

            btnPost.Click += (sender, args) =>  Post();
            btnDetails.Click += (sender, args) => ShowDetails();

            _loanProduct = LoanProduct.FindBy("ProductCode", code);

            // initialize cash voucher entry for go negosyo
            _cashVoucher = new CashVoucher
            {
                VoucherNo = Voucher.LastDocumentNo(VoucherTypes.CV) + 1,
            };

            DataContext = _cashVoucher;
        }

        private void Post()
        {
            var loanAmount = _cashVoucher.Debit;
            var voucherNo = _cashVoucher.VoucherNo;
            var voucherDate = MainController.LoggedUser.TransactionDate;
            var document = new VoucherDocument(VoucherTypes.CV, voucherNo, voucherDate);

            // is there already a loan product for Go Negosyo?
            if (_loanProduct == null)
            {
                MessageWindow.ShowAlertMessage("No Loan Products found for Go Negosyo.");
                return;
            }

            // does user enter a valid amount?
            if (loanAmount <= 0)
            {
                MessageWindow.ShowAlertMessage("Invalid amount!");
                return;
            }

            // does the voucher number been used?
            var collection = CashVoucher.FindByDocumentNumber(voucherNo);
            if (collection.Count > 0)
            {
                MessageWindow.ShowAlertMessage("CV No. already in use.");
                return;
            }

            try
            {
                // what is the account for Go Negosyo?
                var goNegosyo = Account.FindByCode(GlobalSettings.CodeOfGoNegosyo);

                #region --- Finally add entry for the loan applied ---

                // what will be the loan details?
                var loanDetails = GenerateLoanDetails();

                _cashVoucher.SetMember(_member);
                _cashVoucher.SetAccount(goNegosyo);
                _cashVoucher.SetDocument(document);
                _cashVoucher.Explanation = loanDetails.GenerateExplanation();
                _cashVoucher.LoanDetails = loanDetails;

                Result postResult = _cashVoucher.Create();
                if (!postResult.Success)
                {
                    CashVoucher.DeleteAll(document.Number);
                    MessageWindow.ShowAlertMessage(postResult.Message);
                    return;
                }

                #endregion

                // NO CHARGES

                #region --- Add entry for Accounts Payable Merchandise ---

                var net = new CashVoucher();
                net.SetMember(_member);
                net.SetAccount(Account.FindByCode(GlobalSettings.CodeOfAccountsPayableMerchandise));
                net.SetDocument(document);
                net.Credit = loanAmount;
                net.Amount = loanAmount;
                net.AmountInWords = Converter.AmountToWords(loanAmount);
                postResult = net.Create();
                if (!postResult.Success)
                {
                    CashVoucher.DeleteAll(document.Number);
                    MessageWindow.ShowAlertMessage(postResult.Message);
                    return;
                }

                #endregion

                #region --- Voucher Log ---

                var voucherLog = new VoucherLog();
                voucherLog.Find("CV", _cashVoucher.VoucherNo);
                voucherLog.Date = _cashVoucher.VoucherDate;
                voucherLog.Initials = MainController.LoggedUser.Initials;
                voucherLog.Save();

                #endregion

                MessageWindow.ShowNotifyMessage("Go Negosyo created. Please check CV# " + voucherNo);
                DialogResult = true;
                Close();
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }

        private LoanDetails GenerateLoanDetails()
        {
            var loanAmount = _cashVoucher.Debit;
            var voucherNo = _cashVoucher.VoucherNo;
            var voucherDate = MainController.LoggedUser.TransactionDate;
            var document = new VoucherDocument(VoucherTypes.CV, voucherNo, voucherDate);

            var salaryAdvance = Account.FindByCode(_loanProduct.ProductCode);

            var loanDetails = new LoanDetails
            {
                ReleaseNo = ModelController.Releases.MaxReleaseNumber() + 1,
                LoanAmount = loanAmount,
                LoanTerms = 1,
                TermsMode = "Month",
                GrantedDate = document.Date,
                MaturityDate = document.Date.AddMonths(1),
                CutOffDate = document.Date.AddDays(7),
                Payment = loanAmount,
                ModeOfPayment = ModeOfPayments.Monthly,
                InterestRate = _loanProduct.AnnualInterestRate / 12,
                InterestAmount = loanAmount * (_loanProduct.AnnualInterestRate / 12),
                InterestAmortization = loanAmount * (_loanProduct.AnnualInterestRate / 12),
                DateReleased = document.Date
            };

            loanDetails.SetMember(_member);
            loanDetails.SetAccount(salaryAdvance);
            loanDetails.SetDocument(document);

            return loanDetails;
        }

        private void ShowDetails()
        {
             var view = new LoanModule.LoanDetailsWindow(GenerateLoanDetails());
            view.ShowDialog();
        }
    }
}