using System;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.GeneralLedgerBalanceModule
{
    public partial class GeneralLedgerBalanceView
    {
        public GeneralLedgerBalanceView()
        {
            InitializeComponent();

            btnUpdate.Click += UpdateButtonOnClick;
            AccountCodeSearchBox.Click += AccountCodeSearchBoxOnClick;
        }

        private readonly GeneralLedgerBalance _currentItem;

        public GeneralLedgerBalanceView(int id)
            : this()
        {
            _currentItem = new GeneralLedgerBalance();
            if (id > 0)
            {
                _currentItem.Find(id); // edit mode
                btnUpdate.Content = "Update";
            }else
            {
                btnUpdate.Content = "Create";
            }

            DataContext = _currentItem;

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
                if (_currentItem.IsNewRecord())
                {
                    var loginDate = MainController.LoggedUser.TransactionDate;
                    _currentItem.DocumentDate = new DateTime(loginDate.Year - 1, 12, 31);
                    _currentItem.DocumentNo = 0;
                    _currentItem.DocumentType = VoucherTypes.BG.ToString();
                    _currentItem.Create();
                }
                else
                {
                    _currentItem.Update();
                }

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