using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.UserModule
{
    public partial class UserListDetailView : IListDetailView
    {
        private UserCollection _lookup;
        private UserViewModel _viewModel;

        public UserListDetailView()
        {
            InitializeComponent();

            RefreshDisplay();

            txtSearch.TextChanged += (sender, args) => Search();

            btnAdd.Click += (sender, args) => Add();
            btnEdit.Click += (sender, args) => Edit();
            btnDelete.Click += (sender, args) => Delete();
            btnResetPassword.Click += ResetPassword;
        }

        private void ResetPassword(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedItem == null) return;
            if (MessageWindow.ShowConfirmMessage("Reset password?") != MessageBoxResult.Yes) return;

            var result = _viewModel.SelectedItem.ResetPassword();
            if (result.Success)
            {
                MessageWindow.ShowNotifyMessage("Password reset!");
            }
            else
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        #region Implementation of IListDetailView

        public void Add()
        {
            var addUserView = new AddUserView();
            if (addUserView.ShowDialog() == true)
            {
                _viewModel.Collection = _lookup = User.CollectAll();

            }
        }

        public void Edit()
        {
            if (_viewModel.SelectedItem == null) return;
            var editView = new EditUserView(_viewModel.SelectedItem.ID);
            if (editView.ShowDialog() == true)
            {
                _viewModel.SelectedItem.Find(_viewModel.SelectedItem.ID);
            }
        }

        public void Delete()
        {
            if (_viewModel.SelectedItem == null) return;
            if (MessageWindow.ConfirmDeleteRecord() == MessageBoxResult.Yes)
            {
                _viewModel.SelectedItem.Destroy();
                _viewModel.Collection = _lookup = User.CollectAll();
            }
        }

        public void Search()
        {
            if (_viewModel.Collection == null) return;
            if (!_viewModel.Collection.Any()) return;

            var searchItem = txtSearch.Text;
            if (searchItem.Trim().Length == 0)
            {
                RefreshDisplay();
            }
            else
            {
                var filteredItem = from item in _lookup
                                   where item.UserName.ToLower().Contains(searchItem.ToLower())
                                   select item;

                var viewModel = new UserViewModel {Collection = new UserCollection()};
                foreach (var User in filteredItem)
                {
                    viewModel.Collection.Add(User);
                }
                _viewModel = viewModel;
                DataContext = _viewModel;
            }
        }

        public void RefreshDisplay()
        {
            _viewModel = new UserViewModel();
            _viewModel.Collection = _lookup = User.CollectAll();
            DataContext = _viewModel;
        }

        #endregion
    }
}