using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for VoucherDataEntry.xaml
    /// </summary>
    public partial class VoucherDataEntryWindow
    {
        private readonly TransactionHeader _transactionHeader;
        private readonly IVoucher _voucherView;
        private Account _account;
        private Member _member;
        private TransactionDetail _transactionDetail = new TransactionDetail();

        public VoucherDataEntryWindow(IVoucher voucherView, TransactionHeader transactionHeader,
                                      TransactionDetail transactionDetail)
        {
            InitializeComponent();

            _member = new Member();
            _account = new Account();
            _transactionDetail = (TransactionDetail)transactionDetail.Clone();

            DataContext = _transactionDetail;
            HeaderGrid.DataContext = transactionHeader;

            _voucherView = voucherView;
            _transactionHeader = transactionHeader;

            // set proper button caption
            if (_transactionDetail.TransactionDetailId > 0)
            {
                UpdateButton.Content = "Update";
                FormTitle.Content = Title = "Update Current Detail";
                
            }
            else
            {
                UpdateButton.Content = "Insert";
                FormTitle.Content = Title = "Insert New Detail";
            }

            
        }

        #region --- METHODS ---

        private void BaseWindowOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl | e.Key == Key.S)
            {
                Update(this, e);
                return;
            }
            if (e.Key == Key.LeftCtrl | e.Key == Key.N)
            {
                Create(this, e);
            }
        }

        private void BaseWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            txtMemberCode.Focus();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            var transactionDetail = new TransactionDetail
                {
                    TransactionHeaderId = _transactionHeader.TransactionHeaderId,
                    MemberCode = _member.MemberCode,
                    MemberName = _member.MemberName
                };

            DataContext = _transactionDetail = transactionDetail;
            txtMemberCode.Focus();
        }

        private void InputBoxOnGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void SearchAccount(object sender, EventArgs e)
        {
            List<Account> accounts = Account.GetList();

            // create list of search items
            List<SearchItem> searchItems =
                accounts.Select(
                    account =>
                    new SearchItem(account.AccountId, account.AccountTitle)
                        {
                            ItemCode = account.AccountCode
                        }).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true)
                return;
            _account.Find(searchByCodeWindow.SelectedItem.ItemId);
            _transactionDetail.AccountCode = _account.AccountCode;
            _transactionDetail.AccountTitle = _account.AccountTitle;

            DataContext = new TransactionDetail(); // to trigger a change in datacontext
            DataContext = _transactionDetail;
        }

        private void SearchMember(object sender, EventArgs e)
        {
            List<Member> members = Member.GetList();

            // create list of search items
            List<SearchItem> searchItems =
                members.Select(
                    member =>
                    new SearchItem(member.MemberId, member.MemberName)
                        {
                            ItemCode = member.MemberCode
                        }).
                    ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true)
                return;
            _member.Find(searchByCodeWindow.SelectedItem.ItemId);
            _transactionDetail.MemberCode = _member.MemberCode;
            _transactionDetail.MemberName = _member.MemberName;

            DataContext = new TransactionDetail();
            DataContext = _transactionDetail;
        }

        private void txtAccountCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter && e.Key != Key.Tab)
                return;

            string accountCode = txtAccountCode.Text.Trim();
            _account = Account.GetByCode(accountCode);
            txtAccountName.Text = _account.AccountTitle;
            _transactionDetail.AccountTitle = _account.AccountTitle;
            txtDebitAmount.Focus();
        }

        private void txtCreditAmount_KeyDown(object sender, KeyEventArgs e)
        {
            MainController.MoveFocusToNextControlOnEnter(sender, e);
        }

        private void txtDebitAmount_KeyDown(object sender, KeyEventArgs e)
        {
            MainController.MoveFocusToNextControlOnEnter(sender, e);
        }

        private void txtMemberCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter && e.Key != Key.Tab)
                return;
            _member = Member.GetByCode(txtMemberCode.Text);
            txtMemberName.Text = _member.MemberName;
            _transactionDetail.MemberName = _member.MemberName;
            txtAccountCode.Focus();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;
            try
            {
                if (_transactionHeader.TransactionHeaderId == 0)
                {
                    _transactionHeader.Create();
                }

                _transactionDetail.TransactionHeaderId = _transactionHeader.TransactionHeaderId;

                if (_transactionDetail.TransactionDetailId == 0)
                    _transactionDetail.Create();
                else
                    _transactionDetail.Update();
                
                DialogResult = true;

                Close();
                //_voucherView.RefreshDisplay();

                //txtMemberCode.Focus();
            }
            catch (Exception exception)
            {
                Logger.ExceptionLogger(this, exception);
            }
        }

        #endregion --- METHODS ---

        #region --- VALIDATE INPUTS AND DISPLAY APPROPRIATE ALERT MESSAGES ---

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtMemberCode.Text))
            {
                MessageWindow.ShowAlertMessage("Please enter an existing Member Code!");
                txtMemberCode.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtMemberName.Text))
            {
                MessageWindow.ShowAlertMessage("Please enter an existing Member Name!");
                txtMemberCode.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtAccountCode.Text))
            {
                MessageWindow.ShowAlertMessage("Please enter an existing Account Code!");
                txtAccountCode.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtAccountName.Text))
            {
                MessageWindow.ShowAlertMessage("Please enter an existing Account Name!");
                txtAccountCode.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtDebitAmount.Text))
            {
                MessageWindow.ShowAlertMessage("Invalid Debit Amount Entered!");
                txtDebitAmount.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCreditAmount.Text))
            {
                MessageWindow.ShowAlertMessage("Invalid Credit Amount Entered!");
                txtCreditAmount.Focus();
                return false;
            }
            return true;
        }

        #endregion --- VALIDATE INPUTS AND DISPLAY APPROPRIATE ALERT MESSAGES ---

        private void UpdateOnClick(object sender, RoutedEventArgs e)
        {
            Update(sender, e);

        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}