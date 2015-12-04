using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.UserModule
{
    public partial class AddUserView
    {
        private readonly User _user;

        public AddUserView()
        {
            InitializeComponent();
            _user = new User();
            DataContext = _user;

            btnAdd.Click += btnAdd_Click;
            Loaded += (sender, args) => txtLoginName.Focus();
        }

        public User NewUser
        {
            get { return _user; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            User item = User.FindByName(_user.LoginName);
            if (item == null)
            {
                var result = _user.Create();
                if (result.Success)
                {
                    _user.ResetPassword();
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                }
            }
            else
            {
                MessageWindow.ShowNotifyMessage("Loggin Name already exists!");
            }
        }
    }
}
