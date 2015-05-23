using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.BranchModule
{
    public partial class EditBranchView
    {
        private readonly Branch _branch;

        public EditBranchView()
        {
            InitializeComponent();
            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => txtBranchName.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _branch.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }

        public EditBranchView(int id)
            : this()
        {
            _branch = new Branch();
            _branch.Find(id);
            DataContext = _branch;
        }
    }
}
