using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Utilities;
using SCCO.WPF.MVC.CS.Views.Loan;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public partial class LoanComputationWindow
    {
        private readonly LoanAmortizationHeader _loanAmortizationHeader;
        private readonly LoanComputation _loanComputation;
        private readonly LoanDetails _loanDetails;

        public LoanComputationWindow()
        {
            InitializeComponent();
        }

        internal LoanComputationWindow(LoanDetails loanDetail, LoanProduct loanProduct,
                                       LoanAmortizationHeader loanAmortizationHeader)
            : this()
        {
            _loanDetails = loanDetail;
            _loanComputation = new LoanComputation(_loanDetails.LoanAmount, loanProduct.ID) {LoanDetails = _loanDetails};

            _loanAmortizationHeader = loanAmortizationHeader;
            InitializeControls();

            DataContext = _loanComputation;
        }


        private Account FindAccount()
        {
            List<Account> accounts = Account.GetList();
            List<SearchItem> searchItems =
                accounts.Select(
                    loan =>
                    new SearchItem(loan.ID, loan.AccountTitle) {ItemCode = loan.AccountCode}).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true)
            {
                return null;
            }
            Account account = accounts.SingleOrDefault(ac => ac.ID == searchByCodeWindow.SelectedItem.ItemId);

            return account;
        }

        private void InitializeControls()
        {
            //_loanComputation must have been initialized befor this

            // charges

            #region --- Charge 1 ---

            if (_loanComputation.ChargeAmount1 == 0)
            {
                stbChargeCode1.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.ChargeCode1 = account.AccountCode;
                        _loanComputation.ChargeTitle1 = account.AccountTitle;
                    };
            }
            txtChargeAmount1.IsReadOnly = _loanComputation.ChargeAmount1 > 0;

            #endregion

            #region  --- Charge 2 ---

            if (_loanComputation.ChargeAmount2 == 0)
            {
                stbChargeCode2.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.ChargeCode2 = account.AccountCode;
                        _loanComputation.ChargeTitle2 = account.AccountTitle;
                    };
            }
            txtChargeAmount2.IsReadOnly = _loanComputation.ChargeAmount2 > 0;

            #endregion

            #region  --- Charge 3 ---

            if (_loanComputation.ChargeAmount3 == 0)
            {
                stbChargeCode3.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.ChargeCode3 = account.AccountCode;
                        _loanComputation.ChargeTitle3 = account.AccountTitle;
                    };
            }
            txtChargeAmount3.IsReadOnly = _loanComputation.ChargeAmount3 > 0;

            #endregion

            #region  --- Charge 4 ---

            if (_loanComputation.ChargeAmount4 == 0)
            {
                stbChargeCode4.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.ChargeCode4 = account.AccountCode;
                        _loanComputation.ChargeTitle4 = account.AccountTitle;
                    };
            }
            txtChargeAmount4.IsReadOnly = _loanComputation.ChargeAmount4 > 0;

            #endregion

            #region  --- Charge 5 ---

            if (_loanComputation.ChargeAmount5 == 0)
            {
                stbChargeCode5.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.ChargeCode5 = account.AccountCode;
                        _loanComputation.ChargeTitle5 = account.AccountTitle;
                    };
            }
            txtChargeAmount5.IsReadOnly = _loanComputation.ChargeAmount5 > 0;

            #endregion

            #region  --- Charge 6 ---

            if (_loanComputation.ChargeAmount6 == 0)
            {
                stbChargeCode6.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.ChargeCode6 = account.AccountCode;
                        _loanComputation.ChargeTitle6 = account.AccountTitle;
                    };
            }
            txtChargeAmount6.IsReadOnly = _loanComputation.ChargeAmount6 > 0;

            #endregion

            // deductions

            #region --- Deduction 1 ---

            if (_loanComputation.DeductAmount1 == 0)
            {
                stbDeductCode1.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.DeductCode1 = account.AccountCode;
                        _loanComputation.DeductTitle1 = account.AccountTitle;
                    };
            }
            txtDeductAmount1.IsReadOnly = _loanComputation.DeductAmount1 > 0;

            #endregion

            #region --- Deduction 2 ---

            if (_loanComputation.DeductAmount2 == 0)
            {
                stbDeductCode2.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.DeductCode2 = account.AccountCode;
                        _loanComputation.DeductTitle2 = account.AccountTitle;
                    };
            }
            txtDeductAmount2.IsReadOnly = _loanComputation.DeductAmount2 > 0;

            #endregion

            #region --- Deduction 3 ---

            if (_loanComputation.DeductAmount3 == 0)
            {
                stbDeductCode3.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.DeductCode3 = account.AccountCode;
                        _loanComputation.DeductTitle3 = account.AccountTitle;
                    };
            }
            txtDeductAmount3.IsReadOnly = _loanComputation.DeductAmount3 > 0;

            #endregion

            #region --- Deduction 4 ---

            if (_loanComputation.DeductAmount4 == 0)
            {
                stbDeductCode4.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.DeductCode4 = account.AccountCode;
                        _loanComputation.DeductTitle4 = account.AccountTitle;
                    };
            }
            txtDeductAmount4.IsReadOnly = _loanComputation.DeductAmount4 > 0;

            #endregion

            #region --- Deduction 5 ---

            if (_loanComputation.DeductAmount5 == 0)
            {
                stbDeductCode5.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.DeductCode5 = account.AccountCode;
                        _loanComputation.DeductTitle5 = account.AccountTitle;
                    };
            }
            txtDeductAmount5.IsReadOnly = _loanComputation.DeductAmount5 > 0;

            #endregion

            #region --- Deduction 6 ---

            if (_loanComputation.DeductAmount6 == 0)
            {
                stbDeductCode6.Click += delegate
                    {
                        Account account = FindAccount();
                        if (account == null) return;
                        _loanComputation.DeductCode6 = account.AccountCode;
                        _loanComputation.DeductTitle6 = account.AccountTitle;
                    };
            }
            txtDeductAmount6.IsReadOnly = _loanComputation.DeductAmount6 > 0;

            #endregion

            #region --- Net Proceeds ---

            stbNetProceedsCode.Click += delegate
                {
                    Account account = FindAccount();
                    if (account == null) return;
                    _loanComputation.NetProceedsCode = account.AccountCode;
                    _loanComputation.NetProceedsTitle = account.AccountTitle;
                };
            txtNetProceedsAmount.IsReadOnly = true;

            #endregion
        }

        private void PostButtonOnClick(object sender, RoutedEventArgs e)
        {
            Result postResult = PostToVoucher();
            if (!postResult.Success)
            {
                MessageWindow.ShowAlertMessage(postResult.Message);
            }
            else
            {
                MessageWindow.ShowNotifyMessage(postResult.Message);
                DialogResult = true;
                Close();
            }
        }

        private Result PostToVoucher()
        {
            var postingDetails = new LoanPostingDetails
            {
                VoucherType = VoucherTypes.CV,
                VoucherDate = GlobalSettings.DateOfOpenTransaction
            };

            postingDetails.VoucherNumber = Voucher.LastDocumentNo(postingDetails.VoucherType) + 1;

            postingDetails.ReleaseNumber = ModelController.Releases.MaxReleaseNumber() + 1;
            postingDetails.ReleaseDate = postingDetails.VoucherDate;

            // 1. ask user to enter voucher information
            var postingWindow = new LoanPostingWindow(postingDetails);
            if (postingWindow.ShowDialog() != true) return new Result(false, "Posting was cancelled by user.");
           
            // 1.1 Must be open transaction
            if (postingDetails.VoucherDate != GlobalSettings.DateOfOpenTransaction)
                return new Result(false, "Voucher date is locked!");
            if( postingDetails.VoucherDate != MainController.LoggedUser.TransactionDate)
             return new Result(false, "Transaction Date is not valid or not set!");

            // 2. inform user if voucher information already exist, ask if to overwrite

            if (postingDetails.VoucherType == VoucherTypes.CV)
            {
                ObservableCollection<CashVoucher> cvEntries =
                    CashVoucher.WhereDocumentNumberIs(postingDetails.VoucherNumber);
                if (cvEntries.Count > 0)
                {
                    return new Result(false, string.Format("{0} #{1} already exist!", postingDetails.VoucherType,
                                                           postingDetails.VoucherNumber));
                }
            }

            if (postingDetails.VoucherType == VoucherTypes.JV)
            {
                ObservableCollection<JournalVoucher> jvEntries =
                    JournalVoucher.WhereDocumentNumberIs(postingDetails.VoucherNumber);
                if (jvEntries.Count > 0)
                {
                    return new Result(false, string.Format("{0} #{1} already exist!", postingDetails.VoucherType,
                                                           postingDetails.VoucherNumber));
                }
            }

            // 3. post transaction details

            _loanDetails.ReleaseNo = postingDetails.ReleaseNumber;
            _loanDetails.DateReleased = postingDetails.ReleaseDate;

            //TODO: Loan Application Information
            //loanDetails.ThisMonth
            //loanDetails.DateApplied = _loanDetail.DateGranted;
            //loanDetails.DateApproved
            //loanDetails.DateCancelled;
            //loanDetails.DateReleased


            // net proceeds
            var netProceeds = new ComputationDetail
                {
                    AccountCode = _loanComputation.NetProceedsCode,
                    AccountTitle = _loanComputation.NetProceedsTitle,
                    Amount = _loanComputation.NetProceedsAmount
                };

            if (postingDetails.VoucherType == VoucherTypes.JV)
            {
                #region --- Add entry for Cash On Hand ---

                var net = new JournalVoucher
                    {
                        MemberCode = _loanDetails.MemberCode,
                        MemberName = _loanDetails.MemberName,
                        AccountCode = netProceeds.AccountCode,
                        AccountTitle = netProceeds.AccountTitle,
                        Credit = netProceeds.Amount,
                        VoucherDate = postingDetails.VoucherDate,
                        VoucherNo = postingDetails.VoucherNumber,
                    };
                net.Amount = net.Credit;
                net.AmountInWords = Converter.AmountToWords(net.Amount);
                Result postResult = net.Create();
                if (!postResult.Success)
                {
                    JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Add entries for Charges ---
                
                foreach (ComputationDetail computationDetail in _loanComputation.Charges)
                {
                    if (string.IsNullOrEmpty(computationDetail.AccountCode) || computationDetail.Amount == 0) continue;
                    var charges = new JournalVoucher
                        {
                            MemberCode = _loanDetails.MemberCode,
                            MemberName = _loanDetails.MemberName,
                            AccountCode = computationDetail.AccountCode,
                            AccountTitle = computationDetail.AccountTitle,
                            Credit = computationDetail.Amount,
                            VoucherDate = postingDetails.VoucherDate,
                            VoucherNo = postingDetails.VoucherNumber
                        };
                    postResult = charges.Create();
                    if (postResult.Success) continue;
                    JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Add entries for Deductions ---

                foreach (ComputationDetail computationDetail in _loanComputation.Deductions)
                {
                    if (string.IsNullOrEmpty(computationDetail.AccountCode) || computationDetail.Amount == 0) continue;
                    var deductions = new JournalVoucher
                        {
                            MemberCode = _loanDetails.MemberCode,
                            MemberName = _loanDetails.MemberName,
                            AccountCode = computationDetail.AccountCode,
                            AccountTitle = computationDetail.AccountTitle,
                            Credit = computationDetail.Amount,
                            VoucherDate = postingDetails.VoucherDate,
                            VoucherNo = postingDetails.VoucherNumber
                        };
                    postResult = deductions.Create();
                    if (postResult.Success) continue;
                    JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Add entry for Unearned Income ---

                Account ui = Account.FindByCode(GlobalSettings.CodeOfUnearnedIncome);
                var unearnedIncome = new JournalVoucher
                {
                    MemberCode = _loanDetails.MemberCode,
                    MemberName = _loanDetails.MemberName,
                    AccountCode = ui.AccountCode,
                    AccountTitle = ui.AccountTitle,
                    Credit = _loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.Interest),
                    VoucherDate = postingDetails.VoucherDate,
                    VoucherNo = postingDetails.VoucherNumber
                };

                postResult = unearnedIncome.Create();
                if (!postResult.Success)
                {
                    JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Add entry for Capital Build-Up ---

                Account cbu = Account.FindByCode(GlobalSettings.CodeOfCapitalBuildUp);
                var capitalBuildUp = new JournalVoucher
                {
                    MemberCode = _loanDetails.MemberCode,
                    MemberName = _loanDetails.MemberName,
                    AccountCode = cbu.AccountCode,
                    AccountTitle = cbu.AccountTitle,
                    Credit = _loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.CapitalBuildUp),
                    VoucherDate = postingDetails.VoucherDate,
                    VoucherNo = postingDetails.VoucherNumber
                };

                postResult = capitalBuildUp.Create();
                if (!postResult.Success)
                {
                    JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Finally add entry for the loan applied ---

                var loanVoucherEntry = new JournalVoucher
                {
                    MemberCode = _loanDetails.MemberCode,
                    MemberName = _loanDetails.MemberName,
                    AccountCode = _loanDetails.AccountCode,
                    AccountTitle = _loanDetails.AccountTitle,
                    Debit = _loanDetails.LoanAmount + unearnedIncome.Credit + capitalBuildUp.Credit,
                    VoucherDate = postingDetails.VoucherDate,
                    VoucherNo = postingDetails.VoucherNumber,
                    LoanDetails = _loanDetails,
                    Explanation = _loanDetails.GenerateExplanation()
                };

                postResult = loanVoucherEntry.Create();
                if (!postResult.Success)
                {
                    JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion
            }
            else
            {
                #region --- Add entry for Cash On Hand ---

                var net = new CashVoucher
                {
                    MemberCode = _loanDetails.MemberCode,
                    MemberName = _loanDetails.MemberName,
                    AccountCode = netProceeds.AccountCode,
                    AccountTitle = netProceeds.AccountTitle,
                    Credit = netProceeds.Amount,
                    VoucherDate = postingDetails.VoucherDate,
                    VoucherNo = postingDetails.VoucherNumber,
                };
                net.Amount = net.Credit;
                net.AmountInWords = Converter.AmountToWords(net.Amount);
                Result postResult = net.Create();
                if (!postResult.Success)
                {
                    CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Add entries for Charges ---

                foreach (ComputationDetail computationDetail in _loanComputation.Charges)
                {
                    if (string.IsNullOrEmpty(computationDetail.AccountCode) || computationDetail.Amount == 0) continue;
                    var charges = new CashVoucher
                    {
                        MemberCode = _loanDetails.MemberCode,
                        MemberName = _loanDetails.MemberName,
                        AccountCode = computationDetail.AccountCode,
                        AccountTitle = computationDetail.AccountTitle,
                        Credit = computationDetail.Amount,
                        VoucherDate = postingDetails.VoucherDate,
                        VoucherNo = postingDetails.VoucherNumber
                    };
                    postResult = charges.Create();
                    if (postResult.Success) continue;
                    CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Add entries for Deductions ---

                foreach (ComputationDetail computationDetail in _loanComputation.Deductions)
                {
                    if (string.IsNullOrEmpty(computationDetail.AccountCode) || computationDetail.Amount == 0) continue;
                    var deductions = new CashVoucher
                    {
                        MemberCode = _loanDetails.MemberCode,
                        MemberName = _loanDetails.MemberName,
                        AccountCode = computationDetail.AccountCode,
                        AccountTitle = computationDetail.AccountTitle,
                        Credit = computationDetail.Amount,
                        VoucherDate = postingDetails.VoucherDate,
                        VoucherNo = postingDetails.VoucherNumber
                    };
                    postResult = deductions.Create();
                    if (postResult.Success) continue;
                    CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Add entry for Unearned Income ---

                Account ui = Account.FindByCode(GlobalSettings.CodeOfUnearnedIncome);
                var unearnedIncome = new CashVoucher
                {
                    MemberCode = _loanDetails.MemberCode,
                    MemberName = _loanDetails.MemberName,
                    AccountCode = ui.AccountCode,
                    AccountTitle = ui.AccountTitle,
                    Credit = _loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.Interest),
                    VoucherDate = postingDetails.VoucherDate,
                    VoucherNo = postingDetails.VoucherNumber
                };

                postResult = unearnedIncome.Create();
                if (!postResult.Success)
                {
                    CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Add entry for Capital Build-Up ---

                Account cbu = Account.FindByCode(GlobalSettings.CodeOfCapitalBuildUp);
                var capitalBuildUp = new CashVoucher
                {
                    MemberCode = _loanDetails.MemberCode,
                    MemberName = _loanDetails.MemberName,
                    AccountCode = cbu.AccountCode,
                    AccountTitle = cbu.AccountTitle,
                    Credit = _loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.CapitalBuildUp),
                    VoucherDate = postingDetails.VoucherDate,
                    VoucherNo = postingDetails.VoucherNumber
                };

                postResult = capitalBuildUp.Create();
                if (!postResult.Success)
                {
                    CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion

                #region --- Finally add entry for the loan applied ---

                var loanVoucherEntry = new CashVoucher
                {
                    MemberCode = _loanDetails.MemberCode,
                    MemberName = _loanDetails.MemberName,
                    AccountCode = _loanDetails.AccountCode,
                    AccountTitle = _loanDetails.AccountTitle,
                    Debit = _loanDetails.LoanAmount + unearnedIncome.Credit + capitalBuildUp.Credit,
                    VoucherDate = postingDetails.VoucherDate,
                    VoucherNo = postingDetails.VoucherNumber,
                    LoanDetails = _loanDetails,
                    Explanation = _loanDetails.GenerateExplanation()
                };

                postResult = loanVoucherEntry.Create();
                if (!postResult.Success)
                {
                    CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                    return postResult;
                }

                #endregion
            }

            #region --- Voucher Log ---

            var voucherLog = new VoucherLog();
            voucherLog.Find(postingDetails.VoucherType.ToString(), postingDetails.VoucherNumber);
            voucherLog.Date = postingDetails.VoucherDate;
            voucherLog.Initials = MainController.LoggedUser.Initials;

            voucherLog.Save();

            #endregion

            return new Result(true, string.Format("A loan has been created! Please check {0} #{1}.",
                                                  postingDetails.VoucherType, postingDetails.VoucherNumber));
        }

        private void PrintButtonOnClick(object sender, RoutedEventArgs e)
        {
            ShowReport();
        }

        private void ShowReport()
        {
            //using (var dtLoanComputation = new DataTable())
            //{
            //    dtLoanComputation.Columns.Add("MemberCode", typeof (string));
            //    dtLoanComputation.Columns.Add("MemberName", typeof (string));
            //    dtLoanComputation.Columns.Add("Term", typeof (string));
            //    dtLoanComputation.Columns.Add("AccountCode", typeof (string));
            //    dtLoanComputation.Columns.Add("Title", typeof (string));
            //    dtLoanComputation.Columns.Add("Principal", typeof (decimal));
            //    dtLoanComputation.Columns.Add("TotalCharges", typeof (decimal));
            //    dtLoanComputation.Columns.Add("NetProceeds", typeof (decimal));
            //    dtLoanComputation.Columns.Add("Terms", typeof (string));
            //    dtLoanComputation.Columns.Add("ModeOfPayment", typeof (string));
            //    dtLoanComputation.Columns.Add("LoanAmount", typeof (decimal));
            //    dtLoanComputation.Columns.Add("Payment", typeof (decimal));

            //    dtLoanComputation.Columns.Add("DateGranted", typeof (DateTime));
            //    dtLoanComputation.Columns.Add("MaturityDate", typeof (DateTime));
            //    dtLoanComputation.Columns.Add("InterestRate", typeof (decimal));
            //    dtLoanComputation.Columns.Add("InterestAmount", typeof (decimal));
            //    dtLoanComputation.Columns.Add("InterestAmortization", typeof (decimal));

            //    dtLoanComputation.Columns.Add("Collateral", typeof (string));
            //    dtLoanComputation.Columns.Add("ChargesDesc1", typeof (string));
            //    dtLoanComputation.Columns.Add("ChargesDesc2", typeof (string));
            //    dtLoanComputation.Columns.Add("ChargesDesc3", typeof (string));
            //    dtLoanComputation.Columns.Add("ChargesDesc4", typeof (string));
            //    dtLoanComputation.Columns.Add("ChargesDesc5", typeof (string));
            //    dtLoanComputation.Columns.Add("ChargesDesc6", typeof (string));

            //    dtLoanComputation.Columns.Add("ChargesAmount1", typeof (decimal));
            //    dtLoanComputation.Columns.Add("ChargesAmount2", typeof (decimal));
            //    dtLoanComputation.Columns.Add("ChargesAmount3", typeof (decimal));
            //    dtLoanComputation.Columns.Add("ChargesAmount4", typeof (decimal));
            //    dtLoanComputation.Columns.Add("ChargesAmount5", typeof (decimal));
            //    dtLoanComputation.Columns.Add("ChargesAmount6", typeof (decimal));


            //    dtLoanComputation.Columns.Add("OtherDeductionDesc1", typeof (string));
            //    dtLoanComputation.Columns.Add("OtherDeductionDesc2", typeof (string));
            //    dtLoanComputation.Columns.Add("OtherDeductionDesc3", typeof (string));
            //    dtLoanComputation.Columns.Add("OtherDeductionDesc4", typeof (string));
            //    dtLoanComputation.Columns.Add("OtherDeductionDesc5", typeof (string));
            //    dtLoanComputation.Columns.Add("OtherDeductionDesc6", typeof (string));

            //    dtLoanComputation.Columns.Add("OtherDeductionAmount1", typeof (decimal));
            //    dtLoanComputation.Columns.Add("OtherDeductionAmount2", typeof (decimal));
            //    dtLoanComputation.Columns.Add("OtherDeductionAmount3", typeof (decimal));
            //    dtLoanComputation.Columns.Add("OtherDeductionAmount4", typeof (decimal));
            //    dtLoanComputation.Columns.Add("OtherDeductionAmount5", typeof (decimal));
            //    dtLoanComputation.Columns.Add("OtherDeductionAmount6", typeof (decimal));

            //    dtLoanComputation.Columns.Add("CoMaker1", typeof (string));
            //    dtLoanComputation.Columns.Add("CoMaker2", typeof (string));
            //    dtLoanComputation.Columns.Add("CoMaker3", typeof (string));

            //    DataRow dtRow = dtLoanComputation.NewRow();
            //    dtRow["MemberCode"] = _loanDetails.MemberCode;
            //    dtRow["MemberName"] = _loanDetails.MemberName;
            //    dtRow["Term"] = _loanDetails.LoanTerms;
            //    dtRow["AccountCode"] = _loanDetails.AccountCode;
            //    dtRow["Title"] = _loanDetails.AccountTitle;
            //    dtRow["Principal"] = _loanDetails.LoanAmount;
            //    dtRow["TotalCharges"] = _loanComputation.ChargeAmountTotal;
            //    dtRow["NetProceeds"] = _loanComputation.NetProceedsAmount;
            //    dtRow["Terms"] = _loanDetails.LoanTerms;
            //    dtRow["ModeOfPayment"] = _loanDetails.ModeOfPayment;
            //    dtRow["LoanAmount"] = _loanDetails.LoanAmount;
            //    dtRow["Payment"] = _loanDetails.Payment;
            //    dtRow["DateGranted"] = _loanDetails.GrantedDate;
            //    dtRow["MaturityDate"] = _loanDetails.MaturityDate;
            //    dtRow["InterestRate"] = _loanDetails.InterestRate;
            //    dtRow["InterestAmount"] = _loanDetails.InterestAmount;
            //    dtRow["InterestAmortization"] = _loanDetails.InterestAmortization;
            //    dtRow["Collateral"] = _loanDetails.Description;

            //    dtRow["ChargesDesc1"] = _loanComputation.ChargeTitle1;
            //    dtRow["ChargesDesc2"] = _loanComputation.ChargeTitle2;
            //    dtRow["ChargesDesc3"] = _loanComputation.ChargeTitle3;
            //    dtRow["ChargesDesc4"] = _loanComputation.ChargeTitle4;
            //    dtRow["ChargesDesc5"] = _loanComputation.ChargeTitle5;
            //    dtRow["ChargesDesc6"] = _loanComputation.ChargeTitle6;

            //    dtRow["ChargesAmount1"] = _loanComputation.ChargeAmount1;
            //    dtRow["ChargesAmount2"] = _loanComputation.ChargeAmount2;
            //    dtRow["ChargesAmount3"] = _loanComputation.ChargeAmount3;
            //    dtRow["ChargesAmount4"] = _loanComputation.ChargeAmount4;
            //    dtRow["ChargesAmount5"] = _loanComputation.ChargeAmount5;
            //    dtRow["ChargesAmount6"] = _loanComputation.ChargeAmount6;

            //    dtRow["OtherDeductionDesc1"] = _loanComputation.DeductTitle1;
            //    dtRow["OtherDeductionDesc2"] = _loanComputation.DeductTitle2;
            //    dtRow["OtherDeductionDesc3"] = _loanComputation.DeductTitle3;
            //    dtRow["OtherDeductionDesc4"] = _loanComputation.DeductTitle4;
            //    dtRow["OtherDeductionDesc5"] = _loanComputation.DeductTitle5;
            //    dtRow["OtherDeductionDesc6"] = _loanComputation.DeductTitle6;

            //    dtRow["OtherDeductionAmount1"] = _loanComputation.DeductAmount1;
            //    dtRow["OtherDeductionAmount2"] = _loanComputation.DeductAmount2;
            //    dtRow["OtherDeductionAmount3"] = _loanComputation.DeductAmount3;
            //    dtRow["OtherDeductionAmount4"] = _loanComputation.DeductAmount4;
            //    dtRow["OtherDeductionAmount5"] = _loanComputation.DeductAmount5;
            //    dtRow["OtherDeductionAmount6"] = _loanComputation.DeductAmount6;

            //    int count = _loanDetails.CoMakers.Count();
            //    if (count >= 1)
            //        dtRow["CoMaker1"] = _loanDetails.CoMakers[0];
            //    if (count >= 2)
            //        dtRow["CoMaker2"] = _loanDetails.CoMakers[1];
            //    if (count >= 3)
            //        dtRow["CoMaker3"] = _loanDetails.CoMakers[2];

            //    dtLoanComputation.Rows.Add(dtRow);
            //    dtLoanComputation.TableName = "loan_computation";

            DataTable dtLoanComputation = _loanComputation.CreateReportData();
            var reportItem = new ReportItem {Title = "Loan Computation"};
            //reportItem.DataSource = new DataSet();

            //var compTable = Company.GetData();

            //reportItem.DataSource.Tables.Add(compTable);
            //reportItem.DataSource.Tables.Add(dtLoanComputation);
            Result result = reportItem.LoadReport(dtLoanComputation, "loan_computation.rpt");
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }
    }
}