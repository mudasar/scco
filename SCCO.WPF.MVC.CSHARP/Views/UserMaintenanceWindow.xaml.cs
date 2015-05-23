using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for UsersMaintenanceWindow.xaml
    /// </summary>
    public partial class UserMaintenanceWindow
    {
        public UserMaintenanceWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = _currentUser;
        }

        public UserMaintenanceWindow() {
            InitializeComponent();
            _currentUser = new User(1);
            DataContext = _currentUser;
        }

        private User _currentUser = new User();


        private void Create(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = _currentUser = new User();
        }

        private void Read(object sender, System.Windows.RoutedEventArgs e)
        {
            List<User> users = User.GetList();
            List<SearchItem> searchItems = users.Select(user => new SearchItem(user.UserId, user.FullName)).ToList();

            var searchWindow = new SearchWindow(searchItems);
            searchWindow.ShowDialog();
            if (searchWindow.DialogResult == true) {
                DataContext = _currentUser = new User(searchWindow.SelectedItem.ItemId);
            }
        }

        private void Update(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentUser.UserName)) {
                MessageWindow.ShowAlertMessage("User Name must not be empty!");
                return;
            }
            if(_currentUser.UserId == 0)
            {
                _currentUser.Create();
                return;
            }
            _currentUser.Update();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (
                MessageWindow.ShowConfirmMessage(
                    "You are about to delete current user information. Do you want to proceed?") == MessageBoxResult.Yes)
            {
                _currentUser.Destroy();
                MessageWindow.ShowNotifyMessage("User information deleted!");
                DataContext = _currentUser = new User(0);
            }
        }

        private void Password(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }
    }
}
