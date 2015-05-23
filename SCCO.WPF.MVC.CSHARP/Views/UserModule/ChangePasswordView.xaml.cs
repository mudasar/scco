using System.Windows;

namespace SCCO.WPF.MVC.CS.Views.UserModule
{
    public partial class ChangePasswordView
    {
        public ChangePasswordView()
        {
            InitializeComponent();
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            if(NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageWindow.ShowAlertMessage("New Password and Confirm Password does not match!");
                return;
            }

            Controllers.MainController.LoggedUser.Password = ConfirmPasswordBox.Password;
            var result = Controllers.MainController.LoggedUser.UpdatePassword();

            if (result.Success)
            {
                MessageWindow.ShowNotifyMessage("Password updated!");
                Close();
            }
            else
                MessageWindow.ShowAlertMessage(result.Message);
        }
    }
}
