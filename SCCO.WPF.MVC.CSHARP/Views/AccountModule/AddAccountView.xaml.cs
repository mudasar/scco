using System;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AccountModule
{
    public partial class AddAccountView
    {
        private Account _newItem;

        public AddAccountView()
        {
            InitializeComponent();
            _newItem = new Account();
            _newItem.Nature = "D";

            DataContext = _newItem;
            btnAdd.Click += Add;
            Loaded += (sender, args) => txtAccountCode.Focus();
        }

        public Account NewItem
        {
            get { return _newItem; }
            set { _newItem = value; }
        }

        private void Add(object sender, EventArgs e)
        {
            var item = Account.FindByCode(_newItem.AccountCode);
            if (item.AccountCode != null)
            {
                MessageWindow.ShowAlertMessage("Account Code already exists!");
                return;
            }
            item = Account.FindByName(_newItem.AccountTitle);
            if (item.AccountTitle != null)
            {
                MessageWindow.ShowAlertMessage("Account Title already exists!");
                return;
            }

            var result = _newItem.Create();
            if (result.Success)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }
    }
}