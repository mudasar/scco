using System;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Views.TimeDepositModule;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class TellerCollectorWindow
    {
        private readonly TellerCollector _modelTellerCollector;

        private readonly DateTime _transactionDateAdmin;
        private readonly DateTime _transactionDateUser;

        private readonly DateTime _voucherDate;
        private int _voucherNo;
        private readonly VoucherTypes _voucherType;

        public TellerCollectorWindow()
        {
            InitializeComponent();

            _voucherType = VoucherTypes.OR;
            _voucherDate = MainController.UserTransactionDate;
            _voucherNo = 0;

            _transactionDateAdmin = GlobalSettings.DateOfOpenTransaction;
            _transactionDateUser = MainController.UserTransactionDate;

            _modelTellerCollector = new TellerCollector();
            _modelTellerCollector.Find(_voucherNo);
            DataContext = _modelTellerCollector;


            #region --- Quick Search ---

            KeyDown += delegate(object sender, KeyEventArgs args)
                {
                    if (args.Key == Key.F1 && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        MainController.SearchMember();
                    }
                    if (args.Key == Key.F2 && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        MainController.SearchAccount();
                    }

                };

            #endregion

        }

        #region --- UI CONTROLS EVENTS ---

        private void btnCancelled_Click(object sender, RoutedEventArgs e)
        {
            if (!CanModify)
            {
                MessageWindow.AlertRecordIsLocked();
                return;
            }

            if (MessageWindow.ConfirmCancelVoucher() != MessageBoxResult.Yes) return;

            OfficialReceipt.DeleteAll(_voucherNo);

            var cancelledOfficialReceipt = new OfficialReceipt();
            cancelledOfficialReceipt.MemberCode = "CANCEL";
            cancelledOfficialReceipt.MemberName = "CANCELLED";
            cancelledOfficialReceipt.AccountCode = "CANCEL";
            cancelledOfficialReceipt.AccountTitle = "CANCELLED";
            cancelledOfficialReceipt.Debit = 0m;
            cancelledOfficialReceipt.Credit = 0m;

            cancelledOfficialReceipt.VoucherDate = _voucherDate;
            cancelledOfficialReceipt.VoucherNo = _voucherNo;
            cancelledOfficialReceipt.VoucherType = _voucherType;

            cancelledOfficialReceipt.Collector = MainController.LoggedUser.CollectorName;
            cancelledOfficialReceipt.Create();
            Find(_voucherNo);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CanModify)
            {
                MessageWindow.AlertRecordIsLocked();
                return;
            }

            if (MessageWindow.ConfirmDeleteVoucher() != MessageBoxResult.Yes) return;
            OfficialReceipt.DeleteAll(_voucherNo);
            Find(_voucherNo);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            //-- if transaction date is not valid, exit
            if (_transactionDateAdmin != _transactionDateUser)
            {
                MessageWindow.AlertAddNewRecordNotAllowed();
                return;
            }

            //-- if voucher number is not valid, display last voucher number
            if (_voucherNo == 0)
            {
                int docNum = Voucher.LastDocumentNo(VoucherTypes.OR);
                Find(docNum + 1);
                txtMemberCode.Focus();
                return;
            }

            //-- if editing allowed, delete all matching records in the database then insert current entries
            if (!CanModify)
            {
                MessageWindow.AlertRecordIsLocked();
            }
            else
            {
                CommitChanges(ConfirmRequired.No);

                //-- automate numbering 
                if (chkSkipAutomation.IsChecked == true)
                {
                    Find(_voucherNo + 1);
                }
                else
                {
                    Find(_voucherNo);
                }
            }
        }

        private void txtVoucherNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            int docNum = 0;
            try
            {
                docNum = Convert.ToInt32(txtVoucherNumber.Text);
                if (!CanNavigate)
                {
                    if (_voucherNo != docNum)
                    {
                        MessageWindow.AlertDebitAndCreditNotEqual();
                    }
                    txtVoucherNumber.Text = Convert.ToString(_voucherNo);
                }
                else
                {
                    Find(docNum);
                }
            }
            catch
            {
                Find(docNum);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (HasModified)
            {
                if (MessageWindow.ShowConfirmMessage("Do you want to save current voucher?") != MessageBoxResult.Yes)
                    return;

                CommitChanges(ConfirmRequired.No);
            }
            var result = ReportController.OfficialReceiptReports.TellerCollectorForm(_voucherNo);
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        #endregion --- UI CONTROLS EVENTS ---

        #region --- record navigation ---

        private void Find(int docNum)
        {
            _voucherNo = docNum;
            try
            {
                _modelTellerCollector.Find(docNum);
                _signature = _modelTellerCollector.Signature;
                DataContext = _modelTellerCollector;
                RefreshDisplay();
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
            
        }

        private void OnNavigateFirst(object sender, EventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }

            CommitChanges(ConfirmRequired.Yes);
            Find(Voucher.FirstDocumentNo(_voucherType));
        }

        private void OnNavigateLast(object sender, EventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }

            CommitChanges(ConfirmRequired.Yes);
            Find(Voucher.LastDocumentNo(_voucherType));
        }

        private void OnNavigateNext(object sender, EventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }

            CommitChanges(ConfirmRequired.Yes);
            Find(_voucherNo + 1);
        }

        private void OnNavigatePrevious(object sender, EventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }

            CommitChanges(ConfirmRequired.Yes);
            Find(_voucherNo - 1);
        }

        private bool CanModify
        {
            get
            {
                bool allowEdit = false;
                if (_modelTellerCollector.VoucherDate.ToShortDateString() == _transactionDateAdmin.ToShortDateString())
                {
                    if (MainController.LoggedUser.CollectorName == _modelTellerCollector.CollectorName)
                        allowEdit = true;
                }

                return allowEdit;
            }
        }

        private bool CanNavigate
        {
            //get { return Convert.ToDecimal(_debitTotal) == Convert.ToDecimal(_creditTotal); }
            get { return true; }
        }

        #endregion

        private void CommitChanges(ConfirmRequired confirmRequired)
        {
            if (!CanModify) return;
            
            if (HasModified)
            {
                if (confirmRequired == ConfirmRequired.Yes)
                {
                    if (MessageWindow.ConfirmSaveChangesFirst() != MessageBoxResult.Yes)
                        return;
                }

                _modelTellerCollector.Destroy();
                _modelTellerCollector.Create();
            }
        }


        private string _signature;
        private bool HasModified
        {
            get 
            {
                if (_signature == null) return false;
                return _signature != _modelTellerCollector.Signature; 
            }
        }
    

        private void RefreshDisplay()
        {
            if (_modelTellerCollector.IsCancelled)
            {
                CancelledLabel.Visibility = Visibility.Visible;
            }
            else
            {
                if (CancelledLabel.Visibility == Visibility.Visible)
                    CancelledLabel.Visibility = Visibility.Hidden;
            }

            bool allowEdit = CanModify;

            btnNew.IsEnabled = allowEdit;
            btnDelete.IsEnabled = allowEdit;
            btnCancelled.IsEnabled = allowEdit;
            btnPrint.IsEnabled = allowEdit;

            if (allowEdit)
            {
                imgLocked.Visibility = Visibility.Collapsed;
                imgUnlocked.Visibility = Visibility.Visible;
                //lblVoucherDate.Foreground = System.Windows.Media.Brushes.White;
            }
            else
            {
                imgLocked.Visibility = Visibility.Visible;
                imgUnlocked.Visibility = Visibility.Collapsed;
                //lblVoucherDate.Foreground = System.Windows.Media.Brushes.Red;
            }
            if(string.IsNullOrEmpty(_modelTellerCollector.MemberCode))
            {
                txtMemberCode.Focus();
            }
        }

        #region --- REPORTS ---

        private void PrintOfficialReceipt()
        {
            if (!CanModify) return;
        }

        private Result PrintDetailedCollectionReport()
        {
            return new Result(true, "PrintDetailedCollectionReport successful");
        }

        private Result PrintSummarizedCollectionReport()
        {
            return new Result(true, "PrintSummarizedCollectionReport successful");
        }

        private Result PrintCashDenominationReport()
        {
            return new Result(true, "PrintSummarizedCollectionReport successful");
        }

        private Result PrintCheckDenominationReport()
        {
            return new Result(true, "PrintSummarizedCollectionReport successful");
        }

        #endregion

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnPosted_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnDcrDetals_Click(object sender, RoutedEventArgs e)
        {
            ReportController.OfficialReceiptReports.DailyCollectionReport.Detailed(_modelTellerCollector.VoucherDate, _modelTellerCollector.CollectorName);
        }

        private void btnDcrSummary_Click(object sender, RoutedEventArgs e)
        {
            ReportController.OfficialReceiptReports.DailyCollectionReport.Summary(_modelTellerCollector.VoucherDate, _modelTellerCollector.CollectorName);
        }

        private void btnCash_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow.ShowAlertMessage("Alert");
            MessageWindow.ShowNotifyMessage("Notify");
            MessageWindow.ShowConfirmMessage("Confirm");
            MessageWindow.ShowWarningMessage("Warning");
        }

        private void btnTdDetails_Click(object sender, RoutedEventArgs e)
        {
            string tdCode = GlobalSettings.CodeOfTimeDeposit;
            for (int index = 1; index < 12; index++)
            {
                if (_modelTellerCollector.AccountCodes(index) == tdCode)
                {
                    var timeDepositDetail = new TimeDepositDetails();
                    timeDepositDetail.DateIn = _transactionDateUser;
                    if (_modelTellerCollector.TimeDepositDetail != null)
                    {
                        timeDepositDetail.CertificateNo = _modelTellerCollector.TimeDepositDetail.CertificateNo;
                        timeDepositDetail.DateIn = _modelTellerCollector.TimeDepositDetail.DateIn;
                        timeDepositDetail.Rate = _modelTellerCollector.TimeDepositDetail.Rate;
                        timeDepositDetail.Term = _modelTellerCollector.TimeDepositDetail.Term;
                    }
                    var timeDepositDetailsWindow = new TimeDepositEditWindow(timeDepositDetail);
                    timeDepositDetailsWindow.IsReadOnly = !CanModify;
                    if (timeDepositDetailsWindow.ShowDialog() == true)
                    {
                        _modelTellerCollector.TimeDepositDetail = timeDepositDetail;
                        return;
                    }
                    return;
                }
            }
            MessageWindow.ShowNotifyMessage("No Time Deposit entry found!");
        }

        private void btnDenomination_Click(object sender, RoutedEventArgs e)
        {
            var cashAndCheckBreakDown = new CashAndCheckBreakDown();
            cashAndCheckBreakDown.Copy(_modelTellerCollector.CashAndCheckDenomimation);

            var denomination = new CashAndCheckBreakdownWindow(cashAndCheckBreakDown);
            denomination.IsReadOnly = !CanModify;
            if (denomination.ShowDialog() == true)
            {
                _modelTellerCollector.CashAndCheckDenomimation = cashAndCheckBreakDown;
            }
        }
    }
}

