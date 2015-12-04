using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.UserModule
{
    public partial class EditUserView
    {
        private readonly EditUserViewModel _editUserViewModel;

        public EditUserView()
        {
            InitializeComponent();
            _editUserViewModel = new EditUserViewModel();

            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => txtFullName.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _editUserViewModel.User.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }

        public EditUserView(int id)
            : this()
        {
            _editUserViewModel.User = new User();
            _editUserViewModel.User.Find(id);

            _editUserViewModel.Collectors = Models.Collections.Collectors.Collect();
            var canAccessInitialSetup = Controllers.MainController.LoggedUser.CanAccessInitialSetup;
            chkInitialSetup.Visibility = canAccessInitialSetup ? Visibility.Visible : Visibility.Collapsed;

            var isAdministrator = Controllers.MainController.LoggedUser.IsAdministrator;
            chkAdministrator.Visibility = isAdministrator ? Visibility.Visible : Visibility.Collapsed;

            DataContext = _editUserViewModel;
        }
    }
}