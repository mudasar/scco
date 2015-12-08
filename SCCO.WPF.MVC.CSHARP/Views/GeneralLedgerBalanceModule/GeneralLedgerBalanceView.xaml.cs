using System;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.GeneralLedgerBalanceModule
{
    public partial class GeneralLedgerBalanceView
    {
        private readonly GeneralLedgerBalance _currentItem;

        public GeneralLedgerBalanceView()
        {
            InitializeComponent();

            btnUpdate.Click += UpdateButtonOnClick;
            AccountCodeSearchBox.Click += AccountCodeSearchBoxOnClick;
        }

        public GeneralLedgerBalanceView(int id)
            : this()
        {
            DateTime loginDate = MainController.LoggedUser.TransactionDate;
            _currentItem = new GeneralLedgerBalance
                {
                    DocumentDate = new DateTime(loginDate.Year - 1, 12, 31),
                    DocumentNo = 0,
                    DocumentType = VoucherTypes.BG.ToString(),
                };
            if (id > 0)
            {
                _currentItem.Find(id); // edit mode
                btnUpdate.Content = "Update";
            }
            else
            {
                btnUpdate.Content = "Create";
            }

            DataContext = _currentItem;
        }

        private void AccountCodeSearchBoxOnClick(object sender, EventArgs e)
        {
            Account account = MainController.SearchAccount();
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