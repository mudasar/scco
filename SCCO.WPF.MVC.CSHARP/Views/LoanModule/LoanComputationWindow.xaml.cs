using System;
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
            _loanComputation = new LoanComputation(_loanDetails, loanProduct);

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
            //_loanComputation must have been initialized before this

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
            //txtChargeAmount1.IsReadOnly = _loanComputation.ChargeAmount1 > 0;

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
            //txtChargeAmount2.IsReadOnly = _loanComputation.ChargeAmount2 > 0;

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
            //txtChargeAmount3.IsReadOnly = _loanComputation.ChargeAmount3 > 0;

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
            //txtChargeAmount4.IsReadOnly = _loanComputation.ChargeAmount4 > 0;

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
            //txtChargeAmount5.IsReadOnly = _loanComputation.ChargeAmount5 > 0;

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
            //txtChargeAmount6.IsReadOnly = _loanComputation.ChargeAmount6 > 0;

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
            //txtDeductAmount1.IsReadOnly = _loanComputation.DeductAmount1 > 0;

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
            //txtDeductAmount2.IsReadOnly = _loanComputation.DeductAmount2 > 0;

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
            //txtDeductAmount3.IsReadOnly = _loanComputation.DeductAmount3 > 0;

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
            //txtDeductAmount4.IsReadOnly = _loanComputation.DeductAmount4 > 0;

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
            //txtDeductAmount5.IsReadOnly = _loanComputation.DeductAmount5 > 0;

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
            //txtDeductAmount6.IsReadOnly = _loanComputation.DeductAmount6 > 0;

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
            if (postingDetails.VoucherDate != MainController.LoggedUser.TransactionDate)
                return new Result(false, "Transaction Date is not valid or not set!");

            // 2. inform user if voucher information already exist, ask if to overwrite
            if (postingDetails.VoucherType == VoucherTypes.CV)
            {
                ObservableCollection<CashVoucher> cvEntries =
                    CashVoucher.FindByDocumentNumber(postingDetails.VoucherNumber);
                if (cvEntries.Count > 0)
                {
                    return new Result(false, string.Format("{0} #{1} already exist!", postingDetails.VoucherType,
                                                           postingDetails.VoucherNumber));
                }
            }

            if (postingDetails.VoucherType == VoucherTypes.JV)
            {
                ObservableCollection<JournalVoucher> jvEntries =
                    JournalVoucher.FindByDocumentNumber(postingDetails.VoucherNumber);
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
            Result postResult;
            var netProceeds = new ComputationDetail
                {
                    AccountCode = _loanComputation.NetProceedsCode,
                    AccountTitle = _loanComputation.NetProceedsTitle,
                    Amount = _loanComputation.NetProceedsAmount
                };

            if (postingDetails.VoucherType == VoucherTypes.JV)
            {
                #region --- Add entry for Cash On Hand ---

                if (netProceeds.Amount != 0)
                {
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
                    postResult = net.Create();
                    if (!postResult.Success)
                    {
                        JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                        return postResult;
                    }
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

                decimal sumUnearnedIncome =
                    Math.Round(_loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.Interest), 2);
                if (sumUnearnedIncome > 0)
                {
                    Account ui = Account.FindByCode(GlobalSettings.CodeOfUnearnedIncome);
                    var unearnedIncome = new JournalVoucher
                        {
                            MemberCode = _loanDetails.MemberCode,
                            MemberName = _loanDetails.MemberName,
                            AccountCode = ui.AccountCode,
                            AccountTitle = ui.AccountTitle,
                            Credit = sumUnearnedIncome,
                            VoucherDate = postingDetails.VoucherDate,
                            VoucherNo = postingDetails.VoucherNumber
                        };

                    postResult = unearnedIncome.Create();
                    if (!postResult.Success)
                    {
                        JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                        return postResult;
                    }
                }

                #endregion

                #region --- Add entry for Capital Build-Up ---

                decimal sumCapitalBuildUp =
                    Math.Round(_loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.CapitalBuildUp), 2);
                if (sumCapitalBuildUp > 0)
                {
                    Account cbu = Account.FindByCode(GlobalSettings.CodeOfCapitalBuildUp);
                    var capitalBuildUp = new JournalVoucher
                        {
                            MemberCode = _loanDetails.MemberCode,
                            MemberName = _loanDetails.MemberName,
                            AccountCode = cbu.AccountCode,
                            AccountTitle = cbu.AccountTitle,
                            Credit = sumCapitalBuildUp,
                            VoucherDate = postingDetails.VoucherDate,
                            VoucherNo = postingDetails.VoucherNumber
                        };

                    postResult = capitalBuildUp.Create();
                    if (!postResult.Success)
                    {
                        JournalVoucher.DeleteAll(postingDetails.VoucherNumber);
                        return postResult;
                    }
                }

                #endregion

                #region --- Finally add entry for the loan applied ---

                var loanVoucherEntry = new JournalVoucher
                    {
                        MemberCode = _loanDetails.MemberCode,
                        MemberName = _loanDetails.MemberName,
                        AccountCode = _loanDetails.AccountCode,
                        AccountTitle = _loanDetails.AccountTitle,
                        Debit = _loanDetails.LoanAmount + sumUnearnedIncome + sumCapitalBuildUp,
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

                if (netProceeds.Amount != 0)
                {
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
                    postResult = net.Create();
                    if (!postResult.Success)
                    {
                        CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                        return postResult;
                    }
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

                decimal sumUnearnedIncome =
                    Math.Round(_loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.Interest), 2);
                if (sumUnearnedIncome > 0)
                {
                    Account ui = Account.FindByCode(GlobalSettings.CodeOfUnearnedIncome);
                    var unearnedIncome = new CashVoucher
                        {
                            MemberCode = _loanDetails.MemberCode,
                            MemberName = _loanDetails.MemberName,
                            AccountCode = ui.AccountCode,
                            AccountTitle = ui.AccountTitle,
                            Credit = sumUnearnedIncome,
                            VoucherDate = postingDetails.VoucherDate,
                            VoucherNo = postingDetails.VoucherNumber
                        };

                    postResult = unearnedIncome.Create();
                    if (!postResult.Success)
                    {
                        CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                        return postResult;
                    }
                }

                #endregion

                #region --- Add entry for Capital Build-Up ---

                decimal sumCapitalBuildUp =
                    Math.Round(_loanAmortizationHeader.PaymentSchedules.Sum(sched => sched.CapitalBuildUp), 2);
                if (sumCapitalBuildUp > 0)
                {
                    Account cbu = Account.FindByCode(GlobalSettings.CodeOfCapitalBuildUp);
                    var capitalBuildUp = new CashVoucher
                        {
                            MemberCode = _loanDetails.MemberCode,
                            MemberName = _loanDetails.MemberName,
                            AccountCode = cbu.AccountCode,
                            AccountTitle = cbu.AccountTitle,
                            Credit = sumCapitalBuildUp,
                            VoucherDate = postingDetails.VoucherDate,
                            VoucherNo = postingDetails.VoucherNumber
                        };

                    postResult = capitalBuildUp.Create();
                    if (!postResult.Success)
                    {
                        CashVoucher.DeleteAll(postingDetails.VoucherNumber);
                        return postResult;
                    }
                }

                #endregion

                #region --- Finally add entry for the loan applied ---

                var loanVoucherEntry = new CashVoucher
                    {
                        MemberCode = _loanDetails.MemberCode,
                        MemberName = _loanDetails.MemberName,
                        AccountCode = _loanDetails.AccountCode,
                        AccountTitle = _loanDetails.AccountTitle,
                        Debit = _loanDetails.LoanAmount + sumUnearnedIncome + sumCapitalBuildUp,
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
            DataTable dtLoanComputation = _loanComputation.CreateReportData();
            var reportItem = new ReportItem {Title = "Loan Computation"};
            Result result = reportItem.LoadReport(dtLoanComputation, "loan_computation.rpt");
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }
    }
}