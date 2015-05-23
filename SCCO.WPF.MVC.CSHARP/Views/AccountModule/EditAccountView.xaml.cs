using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AccountModule
{
    public partial class EditAccountView
    {
        private readonly Account _account;

        public EditAccountView(int id)
        {
            InitializeComponent();
            _account = new Account();
            _account.Find(id);
            DataContext = _account;

            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => cboNature.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _account.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}