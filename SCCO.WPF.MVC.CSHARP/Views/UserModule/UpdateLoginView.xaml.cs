using System.Windows;

namespace SCCO.WPF.MVC.CS.Views.UserModule
{
    public partial class UpdateLoginView
    {
        public UpdateLoginView()
        {
            InitializeComponent();
            txtLoginName.Text = Controllers.MainController.LoggedUser.LoginName;
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoginName.Text))
            {
                MessageWindow.ShowAlertMessage("Login name is required!");
                return;
            }
            
            if (string.IsNullOrEmpty(NewPasswordBox.Password))
            {
                MessageWindow.ShowAlertMessage("Password is required!");
                return;
            }
            
            if (NewPasswordBox.Password.Length < 6)
            {
                MessageWindow.ShowAlertMessage("Minimum password length is 6 characters!");
                return;
            }
            
            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageWindow.ShowAlertMessage("New Password and Confirm Password does not match!");
                return;
            }

            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageWindow.ShowAlertMessage("New Password and Confirm Password does not match!");
                return;
            }

            Controllers.MainController.LoggedUser.Password = ConfirmPasswordBox.Password;
            Controllers.MainController.LoggedUser.LoginName = txtLoginName.Text;
            var result = Controllers.MainController.LoggedUser.UpdateLoginNameAndPassword();

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
    }
}
