using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.UserModule
{
    public partial class UpdateLoginView
    {
        private readonly User _user;

        public UpdateLoginView()
        {
            InitializeComponent();
            _user = MainController.LoggedUser;
            txtLoginName.Text = _user.LoginName;
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            Result result = IsLoginNameValid();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            _user.LoginName = txtLoginName.Text;

            if (IsChangePassword())
            {
                result = IsNewPasswordAndConfirmPasswordValid();
                if (!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                    return;
                }
                _user.Password = ConfirmPasswordBox.Password;
                result = _user.UpdateLoginNameAndPassword();
            }
            else
            {
                result = _user.Update();
            }

            if (result.Success)
            {
                MessageWindow.ShowNotifyMessage("Login updated!");
                Close();
            }
            else
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        private Result IsNewPasswordAndConfirmPasswordValid()
        {
            if (NewPasswordBox.Password.Length < 6)
            {
                return new Result(false, "Minimum password length is 6 characters!");
            }

            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                return new Result(false, "New Password and Confirm Password does not match!");
            }
            return new Result(true, "New Password and Confirm Password are valid.");
        }

        private bool IsChangePassword()
        {
            return !string.IsNullOrEmpty(NewPasswordBox.Password) || !string.IsNullOrEmpty(ConfirmPasswordBox.Password);
        }

        private Result IsLoginNameValid()
        {
            if (string.IsNullOrEmpty(txtLoginName.Text))
            {
                return new Result(false, "Login name is required.");
            }
            if (txtLoginName.Text.Length < 4)
            {
                return new Result(false, "Login name must be atleast 4 characters.");
            }
            return new Result(true, "Login name is valid.");
        }
    }
}