using System;
using System.Collections.Generic;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;

namespace SCCO.WPF.MVC.CS.Views.ForwardedBalanceModule
{
    public partial class EditForwardedBalanceView
    {
        public EditForwardedBalanceView()
        {
            InitializeComponent();

            _listTimeDepositCode = Account.GetListOfTimeDepositCode();
            _listLoanReceivableCode = Account.GetListOfLoanReceivableCode();

            btnUpdate.Click += UpdateButtonOnClick;
            MemberCodeSearchBox.Click += MemberCodeSearchBoxOnClick;
            AccountCodeSearchBox.Click += AccountCodeSearchBoxOnClick;

            btnDetails.Click += (sender, args) => ShowDetails();
        }

        private readonly ForwardedBalance _currentItem;
        private readonly List<string> _listTimeDepositCode;
        private readonly List<string> _listLoanReceivableCode;

        private void ShowDetails()
        {
            if (_listTimeDepositCode.Contains(_currentItem.AccountCode))
            {
                if(_currentItem.TimeDepositDetails == null)
                {
                    _currentItem.TimeDepositDetails = new TimeDepositDetails();
                }
                var view = new TimeDepositModule.TimeDepositEditWindow(_currentItem.TimeDepositDetails);
                view.ShowDialog();
                return;
            }

            if (_listLoanReceivableCode.Contains(_currentItem.AccountCode))
            {
                if (_currentItem.LoanDetails == null)
                {
                    _currentItem.LoanDetails = new LoanDetails();
                }
                var view = new LoanModule.LoanDetailsWindow(_currentItem.LoanDetails);
                view.ShowDialog();
            }
        }


        public EditForwardedBalanceView(int id):this()
        {
            _currentItem = new ForwardedBalance();
            _currentItem.Find(id);
            DataContext = _currentItem;

            if (_listTimeDepositCode.Contains(_currentItem.AccountCode) || 
                            _listLoanReceivableCode.Contains(_currentItem.AccountCode))
            {
                btnDetails.Visibility = Visibility.Visible;
            }
            else { btnDetails.Visibility = Visibility.Hidden; }
        }

        private void MemberCodeSearchBoxOnClick(object sender, EventArgs e)
        {
            var member = MainController.SearchMember();
            if (member == null) return;
            _currentItem.MemberCode = member.MemberCode;
            _currentItem.MemberName = member.MemberName;
        }

        private void AccountCodeSearchBoxOnClick(object sender, EventArgs e)
        {
            var account = MainController.SearchAccount();
            if (account == null) return;
            _currentItem.AccountCode = account.AccountCode;
            _currentItem.AccountTitle = account.AccountTitle;
        }

        private void UpdateButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                _currentItem.Update();
                DialogResult = true;
                Close();
            }
            catch (Exception exception)
            {
                MessageWindow.ShowAlertMessage(exception.Message);
            }
        }
    }
}
