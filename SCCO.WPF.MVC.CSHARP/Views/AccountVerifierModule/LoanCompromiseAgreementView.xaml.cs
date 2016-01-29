using System;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    public partial class LoanCompromiseAgreementView
    {
        private readonly LoanCompromiseAgreementViewModel _viewModel;
        private readonly JournalVoucher _journalVoucher;

        internal LoanCompromiseAgreementView(LoanCompromiseAgreementViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _journalVoucher = new JournalVoucher();
            _journalVoucher.SetAccount(Account.FindByCode(_viewModel.LoanDetails.AccountCode));
            _journalVoucher.SetMember(Nfmb.FindByCode(_viewModel.LoanDetails.MemberCode));

            InitializeLookup();
            InitializeEventsListeners();

            // initialize select to 3 months
            cboLoanTerms.SelectedIndex = 2;

            DataContext = _viewModel;
        }

        private void InitializeLookup()
        {
            #region --- LOAN TERMS ---

            cboLoanTerms.Items.Add(string.Format("{0} month", 1));
            for (int i = 2; i <= 12; i++)
            {
                cboLoanTerms.Items.Add(string.Format("{0} months", i));
            }

            #endregion --- LOAN TERMS ---
        }

        private void InitializeEventsListeners()
        {
            // loan term selection
            cboLoanTerms.SelectionChanged += (sender, args) =>
                {
                    if (cboLoanTerms.SelectedItem == null) return;
                    var selectedTerm = (string) cboLoanTerms.SelectedItem;
                    string[] jea = selectedTerm.Split();
                    short loanTerm = Convert.ToInt16(jea[0]);
                    _viewModel.LoanTerm = loanTerm;
                };

            // loan details preview
            btnDetails.Click += (sender, args) => ShowDetails();

            // post button
            btnPost.Click += (sender, args) => Post();
        }

        private void Post()
        {
            var voucherDate = MainController.LoggedUser.TransactionDate;
            var voucherNo = _viewModel.JournalVoucherNumber;
            // check if can create journal voucher
            if (voucherDate != GlobalSettings.DateOfOpenTransaction)
            {
                MessageWindow.ShowAlertMessage("Transaction Date is locked!");
                return;
            }

            // does the voucher number been used?
            if (Voucher.Exist(VoucherTypes.JV, voucherNo))
            {
                MessageWindow.ShowAlertMessage("JV No. already in use.");
                return;
            }

            if (_viewModel.FinesAndPenalty > 0)
            {
                var fapCode = GlobalVariable.FindByKeyword(GlobalKeys.CodeOfFinesAndPenalty);
                if (string.IsNullOrEmpty(fapCode.CurrentValue))
                {
                    MessageWindow.ShowAlertMessage(
                        "Code of Fines and Penalty not set. Check Global Variable in Admin module");
                    return;
                }
            }

            try
            {
                #region --- Finally add entry for the loan ---

                var document = new VoucherDocument(VoucherTypes.JV, voucherNo, voucherDate);
                var loanAccount = Account.FindByCode(_viewModel.LoanDetails.AccountCode);
                var member = Nfmb.FindByCode(_viewModel.LoanDetails.MemberCode);
                var loanAmount = _viewModel.LoanBalance;


                #region --- payment for previous loan ---

                var loanCredit = new JournalVoucher();
                loanCredit.SetMember(member);
                loanCredit.SetAccount(loanAccount);
                loanCredit.SetDocument(document);
                loanCredit.Credit = loanAmount;
                var postResult = loanCredit.Create();
                if (!postResult.Success)
                {
                    JournalVoucher.DeleteAll(document.Number);
                    MessageWindow.ShowAlertMessage(postResult.Message);
                    return;
                }

                #endregion

                #region --- fines and penalty ---

                if (_viewModel.FinesAndPenalty > 0)
                {
                    var fines = new JournalVoucher();
                    fines.SetMember(member);
                    var finesAccount = Account.FindByCode(GlobalSettings.CodeOfFinesAndPenalty);
                    fines.SetAccount(finesAccount);
                    fines.SetDocument(document);
                    fines.Credit = _viewModel.FinesAndPenalty;
                    postResult = fines.Create();
                    if (!postResult.Success)
                    {
                        JournalVoucher.DeleteAll(document.Number);
                        MessageWindow.ShowAlertMessage(postResult.Message);
                        return;
                    }
                }

                #endregion

                _journalVoucher.SetDocument(document);
                _journalVoucher.LoanDetails = LoanDetails.GenerateCompromiseLoanDetails(_journalVoucher, loanAmount,
                                                                                        _viewModel.FinesAndPenalty,
                                                                                        _viewModel.LoanTerm,
                                                                                        _viewModel.LoanDetails.CoMakers);



                var explanationBuilder = new StringBuilder();
                explanationBuilder.Append("Compromise agreement of ");
                explanationBuilder.Append(_journalVoucher.LoanDetails.GenerateExplanation());

                _journalVoucher.Explanation = explanationBuilder.ToString();

                var compromisedLoanAmount = _viewModel.LoanBalance + _viewModel.FinesAndPenalty;

                _journalVoucher.LoanDetails.ReleaseNo = ModelController.Releases.MaxReleaseNumber() + 1;
                _journalVoucher.Debit = compromisedLoanAmount;
                _journalVoucher.Amount = compromisedLoanAmount;
                _journalVoucher.AmountInWords = Converter.AmountToWords(compromisedLoanAmount);

                postResult = _journalVoucher.Create();
                if (!postResult.Success)
                {
                    JournalVoucher.DeleteAll(document.Number);
                    MessageWindow.ShowAlertMessage(postResult.Message);
                    return;
                }

                #endregion

                #region --- Voucher Log ---

                var voucherLog = new VoucherLog();
                voucherLog.Find("JV", document.Number);
                voucherLog.Date = document.Date;
                voucherLog.Initials = MainController.LoggedUser.Initials;
                voucherLog.Save();

                #endregion

                MessageWindow.ShowNotifyMessage(MessageBuilder.TransactionPosted(_journalVoucher));
                DialogResult = true;
                Close();
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }

        private void ShowDetails()
        {
            var voucherDate = MainController.LoggedUser.TransactionDate;
            var voucherNo = _viewModel.JournalVoucherNumber;
            var document = new VoucherDocument(VoucherTypes.JV, voucherNo, voucherDate);
            _journalVoucher.SetDocument(document);
            var view =
                new LoanModule.LoanDetailsWindow(LoanDetails.GenerateCompromiseLoanDetails(_journalVoucher,
                                                                                           _viewModel.LoanBalance,
                                                                                           _viewModel.FinesAndPenalty,
                                                                                           _viewModel.LoanTerm,
                                                                                           _viewModel.LoanDetails
                                                                                                     .CoMakers));
            view.ShowDialog();
        }
    }
}