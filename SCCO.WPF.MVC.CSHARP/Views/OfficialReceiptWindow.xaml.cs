using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;
using SCCO.WPF.MVC.CS.Views.TimeDepositModule;


namespace SCCO.WPF.MVC.CS.Views
{
    public partial class OfficialReceiptWindow
    {
        private decimal _creditTotal;
        private ObservableCollection<OfficialReceipt> _currentItems;
        private DateTime _transactionDateAdmin;
        private DateTime _transactionDateUser;

        private decimal _debitTotal;
        private DateTime _voucherDate;
        private int _voucherNumber;
        private VoucherTypes _voucherType = VoucherTypes.OR;
        private bool _hasModified;

        private Account _cashOnHand;

        public OfficialReceiptWindow()
        {
            InitializeComponent();
            Closing += OnClosing;
            _transactionDateAdmin = GlobalSettings.DateOfOpenTransaction;
            _transactionDateUser = MainController.UserTransactionDate;
            _cashOnHand = Account.FindByCode(GlobalSettings.CodeOfCashOnHand);

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

            btnDenomination.Click += (sender, args) => ShowDenomination();

            btnHelp.Click += (s, e) =>
            {
                const string url = "https://github.com/Jeralane/scco/wiki/Official-Receipt";
                System.Diagnostics.Process.Start(url);
            };
        }

        private void ShowDenomination()
        {
            var cashOnHand = _currentItems.FirstOrDefault(t => t.AccountCode == _cashOnHand.AccountCode);
            var cashAndCheckBreakDown = CashAndCheckBreakDown.ExtractFromCashOnHand(cashOnHand);
            var denomination = new CashAndCheckBreakdownWindow(cashAndCheckBreakDown) {IsReadOnly = true};
            denomination.ShowDialog();
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
                if (_transactionDateUser.ToShortDateString() == _transactionDateAdmin.ToShortDateString())
                {
                    if (_voucherDate.ToShortDateString() == _transactionDateAdmin.ToShortDateString())
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
                var newItem = new OfficialReceipt();
                _currentItems.Add(newItem);
            }
            if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var currentItem = (OfficialReceipt)dgTransactionDetails.SelectedItem;
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
            foreach (OfficialReceipt currentItem in _currentItems)
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
            var currentItem = (OfficialReceipt)sender;

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
                        var currentItem = (OfficialReceipt)newItem;
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
                        var currentItem = (OfficialReceipt)oldItem;
                        currentItem.Destroy();
                    }
                    UpdateTotalDebitAndTotalCredit();
                    break;
            }
            lblRecordCount.Content = string.Format("Record Count: {0}", _currentItems.Count);
        }

        private void RefreshDisplay()
        {
            OfficialReceipt firstItem = _currentItems.FirstOrDefault();
            if (firstItem != null)
            {
                _voucherDate = firstItem.VoucherDate;
                lblCollectorName.Content= firstItem.Collector;
                CancelledLabel.Visibility = firstItem.MemberCode.ToUpper().Contains("CANCEL")
                                                ? Visibility.Visible
                                                : Visibility.Collapsed;
            }
            else
            {
                _voucherDate = _transactionDateUser;
                lblCollectorName.Content = MainController.LoggedUser.CollectorName;
            }
			
			bool allowEdit = CanModify;
			
            dgTransactionDetails.ItemsSource = _currentItems;
            dgTransactionDetails.CanUserAddRows = allowEdit;
            dgTransactionDetails.IsReadOnly = !allowEdit;

            txtDocNum.Text = Convert.ToString(_voucherNumber);
            txtTransactionDate.Text = _voucherDate.ToString("MM/dd/yyyy");

            UpdateTotalDebitAndTotalCredit();

            lblRecordCount.Content = string.Format("Record Count: {0}", _currentItems.Count);

			btnSave.IsEnabled = allowEdit;
			btnDelete.IsEnabled = allowEdit;
			btnCancelled.IsEnabled = allowEdit;
			btnPrint.IsEnabled = allowEdit;
			
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
                _debitTotal = _currentItems.Sum(or => or.Debit);
                _creditTotal = _currentItems.Sum(or => or.Credit);

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

            foreach (OfficialReceipt currentItem in _currentItems)
            {
                currentItem.Amount = 0;
                currentItem.AmountInWords = string.Empty;
            }

            var currentRecord = (OfficialReceipt)dgTransactionDetails.SelectedItem;
            if (currentRecord == null) return;

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

            OfficialReceipt.DeleteAll(_voucherNumber);

            var cancelledOfficialReceipt = new OfficialReceipt();
            cancelledOfficialReceipt.MemberCode = "CANCEL";
            cancelledOfficialReceipt.MemberName = "CANCELLED";
            cancelledOfficialReceipt.AccountCode = "CANCEL";
            cancelledOfficialReceipt.AccountTitle = "CANCELLED";
            cancelledOfficialReceipt.Debit = 0m;
            cancelledOfficialReceipt.Credit = 0m;

            cancelledOfficialReceipt.VoucherDate = _voucherDate;
            cancelledOfficialReceipt.VoucherNo = _voucherNumber;
            cancelledOfficialReceipt.VoucherType = _voucherType;

            cancelledOfficialReceipt.Collector = MainController.LoggedUser.CollectorName;
            cancelledOfficialReceipt.Create();
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
            OfficialReceipt.DeleteAll(_voucherNumber);
            Find(_voucherNumber);
        }

        private void btnExplanation_Click(object sender, RoutedEventArgs e)
        {
            var currentRecord = (OfficialReceipt)dgTransactionDetails.SelectedItem;
            if (currentRecord == null)
            {
                MessageWindow.ShowAlertMessage("Please select an item.");
                return;
            }
            var explanationWindow = new ExplanationWindow();
            explanationWindow.CanModify = CanModify;

            explanationWindow.CurrentValue = currentRecord.Explanation;
            if (explanationWindow.ShowDialog() == true)
            {
                foreach (OfficialReceipt currentItem in _currentItems)
                {
                    currentItem.Explanation = string.Empty;
                }
                currentRecord.Explanation = explanationWindow.CurrentValue;
            }
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
            int docNum = Voucher.LastDocumentNo(VoucherTypes.OR);
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

            var currentItem = (OfficialReceipt)dgTransactionDetails.SelectedItem;

            // must be TD account
            if (!currentItem.AccountCode.Contains(GlobalSettings.CodeOfTimeDeposit)) return;

            // display TdDetails
            var timeDepositDetailsWindow = new TimeDepositDetailsView(currentItem.VoucherType, currentItem.ID);
            timeDepositDetailsWindow.ShowDialog();
        }

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
            var voucherReportWindow = new VoucherReportWindow(VoucherTypes.OR, _voucherNumber);
            voucherReportWindow.TransactionDatePicker.SelectedDate = _voucherDate;
            voucherReportWindow.ShowDialog();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (_hasModified)
            {
                if(MessageWindow.ShowConfirmMessage("Do you want to save current voucher?") != MessageBoxResult.Yes) return;
                
                CommitChanges(ConfirmRequired.No);
            }
            ReportController.OfficialReceiptReports.VoucherForm(_voucherNumber);
        }

        private void dgTransactionDetails_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.Column.DisplayIndex == 0) //.Header.ToString() == "Member Code")
                {
                    var x = (OfficialReceipt)e.Row.Item;
                    var y = ((System.Windows.Controls.TextBox)e.EditingElement).Text;
                    x.MemberCode = y;
                }
                if (e.Column.DisplayIndex == 2) //.Header.ToString() == "Account Code")
                {
                    var x = (OfficialReceipt)e.Row.Item;
                    var y = ((System.Windows.Controls.TextBox)e.EditingElement).Text;
                    x.AccountCode = y;
                }
                if (e.Column.Header.ToString() == "Debit")
                {
                    var x = (OfficialReceipt)e.Row.Item;
                    var y = ((System.Windows.Controls.TextBox)e.EditingElement).Text;
                    decimal amount;
                    Decimal.TryParse(y, out amount);
                    x.Debit = amount;
                }
                if (e.Column.Header.ToString() == "Credit")
                {
                    var x = (OfficialReceipt)e.Row.Item;
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

                _currentItems = OfficialReceipt.FindByDocumentNumber(docNum);
                _currentItems.CollectionChanged += CurrentItemsCollectionChanged;

                foreach (OfficialReceipt currentItem in _currentItems)
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
            Find(Voucher.FirstDocumentNo(VoucherTypes.OR));
        }

        private void OnNavigateLast(object sender, EventArgs e)
        {
            if (!CanNavigate)
            {
                MessageWindow.AlertDebitAndCreditNotEqual();
                return;
            }

            CommitChanges(ConfirmRequired.Yes);
            Find(Voucher.LastDocumentNo(VoucherTypes.OR));
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