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
