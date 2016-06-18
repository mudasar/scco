using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Utilities;
using SCCO.WPF.MVC.CS.Views.LoanModule;
using SCCO.WPF.MVC.CS.Views.TimeDepositModule;


namespace SCCO.WPF.MVC.CS.Views
{
    public partial class JournalVoucherWindow
    {
        private decimal _creditTotal;
        private ObservableCollection<JournalVoucher> _currentItems;
        private DateTime _transactionDateAdmin;
        private DateTime _transactionDateUser;

        private decimal _debitTotal;
        private DateTime _voucherDate;
        private int _voucherNumber;
        private const VoucherTypes _voucherType = VoucherTypes.JV;
        private bool _hasModified;


        public JournalVoucherWindow()
        {
            InitializeComponent();
            Closing += OnClosing;
            _transactionDateAdmin = GlobalSettings.DateOfOpenTransaction;
            _transactionDateUser = MainController.UserTransactionDate;
            OnNavigateLast(this, null);

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

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            CommitChanges(ConfirmRequired.Yes);
        }

        private bool CanModify
        {
            get
            {
                bool allowEdit = false;
                if (_voucherDate.ToShortDateString() == _transactionDateAdmin.ToShortDateString())
                {
                    if (_transactionDateUser.ToShortDateString() == _transactionDateAdmin.ToShortDateString())
                    {
                        allowEdit = true;
                    }
                }
                return allowEdit;
            }
        }

        private bool CanNavigate
        {
            get { return Convert.ToDecimal(_debitTotal) == Convert.ToDecimal(_creditTotal); }
        }

        private void BaseWindowOnClosing(object sender, CancelEventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                e.Cancel = true;
            }
        }

        private void VoucherFormOnKeyDown(object sender, KeyEventArgs e)
        {
            if (!CanModify) return;

            if (e.Key == Key.I && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var newItem = new JournalVoucher();
                _currentItems.Add(newItem);
            }
            if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var currentItem = (JournalVoucher) dgTransactionDetails.SelectedItem;
                _currentItems.Remove(currentItem);
            }
        }

        private Result CommitChanges(ConfirmRequired confirmRequired)
        {
            //return immediately if there is no data
            if (_currentItems == null) return new Result(false, "No record to update");

            // return if no record was changed
            if (!_hasModified) return new Result(false, "No changes made");

            // if must ask user wether to commit changes
            if (confirmRequired == ConfirmRequired.Yes)
            {
                if (MessageWindow.ConfirmSaveChangesFirst() == MessageBoxResult.No)
                    return new Result(false, "Changes was discarded");
            }

            // update/insert records in the database
            foreach (JournalVoucher currentItem in _currentItems)
            {
                if (currentItem.ID == 0)
                {
                    var result = currentItem.Create();
                    if (result.Success == false)
                    {
                        return result;
                    }
                }
                else
                {
                    var result = currentItem.Update();
                    if (result.Success == false)
                    {
                        return result;
                    }
                }
            }

            _hasModified = false;

            #region --- Voucher Log ---

            var voucherLog = new VoucherLog();
            voucherLog.Find("JV", _voucherNumber);
            voucherLog.Date = _voucherDate;
            voucherLog.Initials = MainController.LoggedUser.Initials;

            voucherLog.Save();

            #endregion

            return new Result(true, "Record saved");
        }

        private void CurrentItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _hasModified = true;
            var currentItem = (JournalVoucher) sender;

            switch (e.PropertyName)
            {
                case "MemberCode":
                    var nfmb = Nfmb.FindByCode(currentItem.MemberCode);
                    currentItem.MemberName = nfmb.MemberName;
                    break;

                case "AccountCode":
                    var chart = Account.FindByCode(currentItem.AccountCode);
                    currentItem.AccountTitle = chart.AccountTitle;
                    break;

                case "Debit":
                case "Credit":
                    UpdateTotalDebitAndTotalCredit();
                    break;
            }
        }

        private void CurrentItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    foreach (object newItem in e.NewItems)
                    {
                        var currentItem = (JournalVoucher) newItem;
                        currentItem.VoucherDate = _voucherDate;
                        currentItem.VoucherNo = _voucherNumber;
                        currentItem.VoucherType = _voucherType;
                        currentItem.PropertyChanged += CurrentItemOnPropertyChanged;

                        //TODO: add here other default values
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:

                    foreach (object oldItem in e.OldItems)
                    {
                        var currentItem = (JournalVoucher) oldItem;
                        currentItem.Destroy();
                    }
                    UpdateTotalDebitAndTotalCredit();
                    break;
            }
            lblRecordCount.Content = string.Format("Record Count: {0}", _currentItems.Count);
        }

        private void RefreshDisplay()
        {
            JournalVoucher firstItem = _currentItems.FirstOrDefault();
            if (firstItem != null)
            {
                _voucherDate = firstItem.VoucherDate;
                CancelledLabel.Visibility = firstItem.MemberCode.ToUpper().Contains("CANCEL")
                                                ? Visibility.Visible
                                                : Visibility.Collapsed;
            }
            else
            {
                _voucherDate = _transactionDateUser;
            }

			bool allowEdit = CanModify;
			
            dgTransactionDetails.ItemsSource = _currentItems;
            dgTransactionDetails.CanUserAddRows = allowEdit;
            dgTransactionDetails.IsReadOnly = !allowEdit;

            txtDocNum.Text = Convert.ToString(_voucherNumber);
            txtTransactionDate.Text = _voucherDate.ToString("MM/dd/yyyy");

            UpdateTotalDebitAndTotalCredit();
            JournalVoucher lastItem = _currentItems.LastOrDefault();
            if (lastItem != null && lastItem.Amount != _debitTotal)
            {
                lastItem.Amount = _debitTotal;
                lastItem.AmountInWords = Converter.AmountToWords(_debitTotal);
                lastItem.Update();
            }

            lblRecordCount.Content = string.Format("Record Count: {0}", _currentItems.Count);

			btnSave.IsEnabled = allowEdit;
			btnDelete.IsEnabled = allowEdit;
			btnCancelled.IsEnabled = allowEdit;
			//btnPrint.IsEnabled = allowEdit;
			
            if (allowEdit)
            {
                imgLocked.Visibility = Visibility.Collapsed;
                imgUnlocked.Visibility = Visibility.Visible;
            }
            else
            {
                imgLocked.Visibility = Visibility.Visible;
                imgUnlocked.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateTotalDebitAndTotalCredit()
        {
            {
                _debitTotal = _currentItems.Sum(jv => jv.Debit);
                _creditTotal = _currentItems.Sum(jv => jv.Credit);

                lblDebitAmount.Content = string.Format("{0:N2}", _debitTotal);
                lblCreditAmount.Content = string.Format("{0:N2}", _creditTotal);

                decimal difference = _debitTotal - _creditTotal;
                lblDifference.Content = string.Format("{0:N}", difference);
                bdrDifference.Visibility = difference != 0m ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        #region --- UI CONTROLS EVENTS ---

        private void btnAmountInWords_Click(object sender, RoutedEventArgs e)
        {
            if (_currentItems.Count <= 0) return;

            if (!CanModify)
            {
                MessageWindow.AlertRecordIsLocked();
                return;
            }

            foreach (JournalVoucher currentItem in _currentItems)
            {
                currentItem.Amount = 0;
                currentItem.AmountInWords = string.Empty;
            }

            var currentRecord = (JournalVoucher) dgTransactionDetails.SelectedItem;
            currentRecord.Amount = currentRecord.Debit + currentRecord.Credit;
            currentRecord.AmountInWords = Converter.AmountToWords(currentRecord.Amount);
        }

        private void btnCancelled_Click(object sender, RoutedEventArgs e)
        {
            if (!CanModify)
            {
                MessageWindow.AlertRecordIsLocked();
                return;
            }

            if (MessageWindow.ConfirmCancelVoucher() != MessageBoxResult.Yes) return;

            JournalVoucher.DeleteAll(_voucherNumber);

            var cancelledJournalVoucher = new JournalVoucher
                {
                    MemberCode = "CANCEL",
                    MemberName = "CANCELLED",
                    AccountCode = "CANCEL",
                    AccountTitle = "CANCELLED",
                    Debit = 0m,
                    Credit = 0m,
                    VoucherDate = _voucherDate,
                    VoucherNo = _voucherNumber,
                    VoucherType = _voucherType
                };

            cancelledJournalVoucher.Create();
            Find(_voucherNumber);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CanModify)
            {
                MessageWindow.AlertRecordIsLocked();
                return;
            }

            if (MessageWindow.ConfirmDeleteVoucher() != MessageBoxResult.Yes) return;
            JournalVoucher.DeleteAll(_voucherNumber);
            Find(_voucherNumber);
        }

        private void btnExplanation_Click(object sender, RoutedEventArgs e)
        {
            var currentRecord = (JournalVoucher) dgTransactionDetails.SelectedItem;
            if(currentRecord == null)
            {
                MessageWindow.ShowAlertMessage("Please select an item.");
                return;
            }
            var explanationWindow = new ExplanationWindow
                {
                    CanModify = CanModify,
                    CurrentValue = currentRecord.Explanation
                };

            if (explanationWindow.ShowDialog() == true)
            {
                foreach (JournalVoucher currentItem in _currentItems)
                {
                    currentItem.Explanation = string.Empty;
                }
                currentRecord.Explanation = explanationWindow.CurrentValue;
            }
        }

        private void btnLoanDetails_Click(object sender, RoutedEventArgs e)
        {
            // must have a selected row
            if (dgTransactionDetails.SelectedItem == null) return;

            var currentItem = (JournalVoucher) dgTransactionDetails.SelectedItem;

            // must be loan account
            if (!currentItem.AccountCode.Contains(GlobalSettings.CodeOfLoanReceivables)) return;

            // display loan details
            if (currentItem.LoanDetails == null) return;
            var loanDetailsWindow = new LoanDetailsWindow(currentItem.VoucherType, currentItem.ID);
            loanDetailsWindow.ShowDialog();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (_transactionDateAdmin != _transactionDateUser)
            {
                MessageWindow.AlertAddNewRecordNotAllowed();
                return;
            }
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }
            if (CanModify)
            {
                CommitChanges(ConfirmRequired.Yes);
            }
            int docNum = Voucher.LastDocumentNo(VoucherTypes.JV);
            Find(docNum + 1);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {   
            //-- if voucher number is not valid, display last voucher number
            if (_voucherNumber == 0)
            {
                int docNum = Voucher.LastDocumentNo(VoucherTypes.OR);
                Find(docNum + 1);
                return;
            }

            //-- if editing allowed, delete all matching records in the database then insert current entries
            if (!CanModify)
            {
                MessageWindow.AlertRecordIsLocked();
                return;
            }

            var result = CommitChanges(ConfirmRequired.No);
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            Find(_voucherNumber);
            MessageWindow.NotifyVoucherSaved();
        }

        private void btnTdDetails_Click(object sender, RoutedEventArgs e)
        {
            // must have a selected row
            if (dgTransactionDetails.SelectedItem == null) return;

            var currentItem = (JournalVoucher) dgTransactionDetails.SelectedItem;

            // must be TD account
            if (!currentItem.AccountCode.Contains(GlobalSettings.CodeOfTimeDeposit)) return;

            // display TdDetails
            var timeDepositDetailsWindow = new TimeDepositDetailsView(currentItem.VoucherType, currentItem.ID);
            timeDepositDetailsWindow.ShowDialog();
        }

        //private void txtDocNum_KeyDown(object sender, KeyEventArgs e)
        //{
        //    MainController.MoveFocusToNextControlOnEnter(sender, e);
        //}

        private void txtDocNum_LostFocus(object sender, RoutedEventArgs e)
        {
            int docNum = 0;
            try
            {
                docNum = Convert.ToInt32(txtDocNum.Text);
                if (!CanNavigate)
                {
                    if (_voucherNumber != docNum)
                    {
                        MessageWindow.AlertDebitAndCreditNotEqual();
                    }
                    txtDocNum.Text = Convert.ToString(_voucherNumber);
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

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            var voucherReportWindow = new VoucherReportWindow(_voucherType, _voucherNumber)
                {
                    TransactionDatePicker = {SelectedDate = _voucherDate}
                };
            voucherReportWindow.ShowDialog();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (_hasModified)
            {
                if (MessageWindow.ShowConfirmMessage("Do you want to save current voucher?") != MessageBoxResult.Yes) return;

                CommitChanges(ConfirmRequired.No);
            }
            ReportController.JournalVoucherReports.VoucherForm(_voucherNumber);
        }

        private void dgTransactionDetails_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.Column.DisplayIndex == 0)//MemberCode
                {
                    var x = (JournalVoucher)e.Row.Item;
                    var y = ((System.Windows.Controls.TextBox)e.EditingElement).Text;
                    x.MemberCode = y;
                }
                if (e.Column.DisplayIndex == 2)//Account Code
                {
                    var x = (JournalVoucher)e.Row.Item;
                    var y = ((System.Windows.Controls.TextBox)e.EditingElement).Text;
                    x.AccountCode = y;
                }
                if (e.Column.Header.ToString() == "Debit")
                {
                    var x = (JournalVoucher)e.Row.Item;
                    var y = ((System.Windows.Controls.TextBox)e.EditingElement).Text;
                    decimal amount;
                    Decimal.TryParse(y, out amount);
                    x.Debit = amount;
                }
                if (e.Column.Header.ToString() == "Credit")
                {
                    var x = (JournalVoucher)e.Row.Item;
                    var y = ((System.Windows.Controls.TextBox)e.EditingElement).Text;
                    decimal amount;
                    Decimal.TryParse(y, out amount);
                    x.Credit = amount;
                }

            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }

        #endregion --- UI CONTROLS EVENTS ---

        #region --- record navigation ---

        private void Find(int docNum)
        {
            try
            {
                CancelledLabel.Visibility = Visibility.Collapsed;

                _currentItems = JournalVoucher.FindByDocumentNumber(docNum);
                _currentItems.CollectionChanged += CurrentItemsCollectionChanged;

                foreach (JournalVoucher currentItem in _currentItems)
                {
                    currentItem.PropertyChanged += CurrentItemOnPropertyChanged;
                }
                _voucherNumber = docNum;

                RefreshDisplay();

                DataContext = _currentItems;
                _hasModified = false;
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
            Find(Voucher.FirstDocumentNo(VoucherTypes.JV));
        }

        private void OnNavigateLast(object sender, EventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }

            CommitChanges(ConfirmRequired.Yes);
            Find(Voucher.LastDocumentNo(VoucherTypes.JV));
        }

        private void OnNavigateNext(object sender, EventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }

            CommitChanges(ConfirmRequired.Yes);
            Find(_voucherNumber + 1);
        }

        private void OnNavigatePrevious(object sender, EventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }

            CommitChanges(ConfirmRequired.Yes);
            Find(_voucherNumber - 1);
        }

        #endregion

    }


}