using System;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.ForwardedBalanceModule
{
    public partial class AddForwardedBalanceView
    {
        public ForwardedBalance NewItem { get { return _newItem; } }

        public AddForwardedBalanceView()
        {
            InitializeComponent();

            AddButton.Click += AddButtonOnClick;
            MemberCodeSearchBox.Click += MemberCodeSearchBoxOnClick;
            AccountCodeSearchBox.Click += AccountCodeSearchBoxOnClick;

            _newItem = new ForwardedBalance();
            DataContext = _newItem;

        }
        private readonly ForwardedBalance _newItem;


        private void MemberCodeSearchBoxOnClick(object sender, EventArgs e)
        {
            var member = MainController.SearchMember();
            if (member == null) return;
            _newItem.MemberCode = member.MemberCode;
            _newItem.MemberName = member.MemberName;
        }

        private void AccountCodeSearchBoxOnClick(object sender, EventArgs e)
        {
            var account = MainController.SearchAccount();
            if (account == null) return;
            _newItem.AccountCode = account.AccountCode;
            _newItem.AccountTitle = account.AccountTitle;
        }

        private void AddButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                _newItem.Create();
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
