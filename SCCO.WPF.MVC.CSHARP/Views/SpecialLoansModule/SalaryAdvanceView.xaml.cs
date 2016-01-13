using System;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    public partial class SalaryAdvanceView
    {
        private readonly CashVoucher _cashVoucher;
        private readonly Nfmb _member;
        private readonly LoanProduct _loanProduct;

        private SalaryAdvanceView()
        {
            InitializeComponent();

            if (MainController.LoggedUser.TransactionDate != GlobalSettings.DateOfOpenTransaction)
            {
                MessageWindow.ShowAlertMessage("Cannot create transactions using current date settings.");
                btnDetails.IsEnabled = btnPost.IsEnabled = false;
                return;
            }

            var code = GlobalSettings.CodeOfSalaryAdvance;
            if (string.IsNullOrEmpty(code))
            {
                MessageWindow.ShowAlertMessage("Salary Advance code not set!");
                btnDetails.IsEnabled = btnPost.IsEnabled = false;
                return;
            }

            btnPost.Click += btnPost_Click;
            btnDetails.Click += btnDetails_Click;

            _loanProduct = LoanProduct.GetList().First(a => a.ProductCode == code);

            // initialize cash voucher entry for salary advance
            _cashVoucher = new CashVoucher
            {
                VoucherNo = Voucher.LastDocumentNo(VoucherTypes.CV) + 1,
            };

            DataContext = _cashVoucher;
        }

        public SalaryAdvanceView(Nfmb member) : this()
        {
            _member = member;
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            var loanAmount = _cashVoucher.Debit;
            var voucherNo = _cashVoucher.VoucherNo;
            var voucherDate = MainController.LoggedUser.TransactionDate;
            var document = new VoucherDocument(VoucherTypes.CV, voucherNo, voucherDate);

            // is there already a loan product for salary advance?
            if (_loanProduct == null)
            {
                MessageWindow.ShowAlertMessage("No Loan Products found for Salary Advance.");
                return;
            }

            // does user enter a valid amount?
            if (loanAmount <= 0)
            {
                MessageWindow.ShowAlertMessage("Invalid amount!");
                return;
            }

            // does the voucher number been used?
            var collection = CashVoucher.WhereDocumentNumberIs(voucherNo);
            if (collection.Count > 0)
            {
                MessageWindow.ShowAlertMessage("CV No. already in use.");
                return;
            }

            try
            {
                // what is the account for salary advance?
                var salaryAdvance = Account.FindByCode(GlobalSettings.CodeOfSalaryAdvance);

                var cashOnHandReceived = loanAmount;

                #region --- Finally add entry for the loan applied ---

                // what will be the loan details?
                var loanDetails = GenerateLoanDetails();

                _cashVoucher.SetMember(_member);
                _cashVoucher.SetAccount(salaryAdvance);
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

                #region --- Interest Charge ---

                var interest = new CashVoucher();
                interest.SetMember(_member);
                interest.SetAccount(Account.FindByCode(GlobalSettings.CodeOfMiscellaneousIncome));
                interest.SetDocument(document);
                interest.Credit = loanDetails.InterestAmount;
                postResult = interest.Create();
                if (!postResult.Success)
                {
                    CashVoucher.DeleteAll(document.Number);
                    MessageWindow.ShowAlertMessage(postResult.Message);
                    return;
                }
                cashOnHandReceived -= interest.Credit;

                #endregion

                #region --- Add entries for Charges ---

                foreach (var charge in _loanProduct.LoanCharges)
                {
                    var entry = new CashVoucher();
                    entry.SetMember(_member);
                    entry.SetAccount(Account.FindByCode(charge.AccountCode));
                    entry.SetDocument(document);
                    entry.Credit = loanAmount * charge.Rate;

                    postResult = entry.Create();
                    if (postResult.Success)
                    {
                        cashOnHandReceived -= entry.Credit;
                        continue;
                    }

                    CashVoucher.DeleteAll(document.Number);
                    MessageWindow.ShowAlertMessage(postResult.Message);
                    return;
                }

                #endregion

                #region --- Add entry for Cash On Hand ---

                var net = new CashVoucher();
                net.SetMember(_member);
                net.SetAccount(Account.FindByCode(GlobalSettings.CodeOfCashOnHand));
                net.SetDocument(document);
                net.Credit = cashOnHandReceived;
                net.Amount = cashOnHandReceived;
                net.AmountInWords = Converter.AmountToWords(cashOnHandReceived);
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

                MessageWindow.ShowNotifyMessage("Salary Advance created. Please check CV# " + voucherNo);
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

        private void btnDetails_Click(object sender, EventArgs e)
        {
             var view = new LoanModule.LoanDetailsWindow(GenerateLoanDetails());
            view.ShowDialog();
        }
    }
}