using System.Collections.Generic;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AccountModule
{
    public partial class AccountsPerScheduleView
    {
        private AccountCollection _lookup;
        private AccountViewModel _viewModel;

        private readonly int _scheduleNo;
        private readonly string _scheduleDescription;

        public AccountsPerScheduleView(int scheduleNo, string scheduleDescription)
        {
            InitializeComponent();

            _scheduleNo = scheduleNo;
            _scheduleDescription = scheduleDescription;

            FormTitle.Content = scheduleDescription;

            RefreshDisplay();
            txtSearch.TextChanged += (sender, args) => Search();
            btnAdd.Click += (sender, args) => Add();
            btnRemove.Click += (sender, args) => Remove();
        }

        #region Implementation of IListDetailView

        public void Add()
        {
            var account = MainController.SearchAccount();
            if(account != null)
            {
                if (account.ScheduleCode != _scheduleNo)
                {
                    account.ScheduleCode = _scheduleNo;
                    account.Update();
                    _viewModel.Collection.Add(account);
                }
                else
                {
                    MessageWindow.ShowNotifyMessage(string.Format("{0} already belongs to {1}.", account.AccountTitle,
                                                                  _scheduleDescription));
                }
            }
        }

        public void Edit()
        {
            if (_viewModel.SelectedItem == null) return;
            var editView = new EditAccountView(_viewModel.SelectedItem.ID);
            if (editView.ShowDialog() == true)
            {
                _viewModel.SelectedItem.Find(_viewModel.SelectedItem.ID);
            }
        }

        public void Remove()
        {
            if (_viewModel.SelectedItem == null) return;
            {
                var account = _viewModel.SelectedItem;
                account.ScheduleCode = 0;
                account.Update();
                _lookup.Remove(_lookup.FirstOrDefault(item => item.ID == _viewModel.SelectedItem.ID));
                _viewModel.Collection.Remove(_viewModel.SelectedItem);
            }
        }

        public void Search()
        {
            if (_lookup == null) return;
            if (!_lookup.Any()) return;

            var searchItem = txtSearch.Text;
            if (searchItem.Trim().Length == 0)
            {
                RefreshDisplay();
            }
            else
            {
                var filteredItem = from item in _lookup
                                   where item.AccountCode.ToLower().Contains(searchItem.ToLower()) ||
                                   item.AccountTitle.ToLower().Contains(searchItem.ToLower())
                                   select item;

                var viewModel = new AccountViewModel {Collection = new AccountCollection()};
                foreach (var item in filteredItem)
                {
                    viewModel.Collection.Add(item);
                }
                _viewModel = viewModel;
                DataContext = _viewModel;
            }
        }

        public void RefreshDisplay()
        {
            //_lookup = Account.CollectAll();

            var conditions = new Dictionary<string, object> { { "SCODE", _scheduleNo } };

            _lookup = Account.Where(conditions);

            _viewModel = new AccountViewModel { Collection = Account.Where(conditions) };
            DataContext = _viewModel;
        }

        #endregion
    }
}