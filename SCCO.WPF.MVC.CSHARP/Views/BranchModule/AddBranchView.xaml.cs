using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.BranchModule
{
    public partial class AddBranchView
    {
        private readonly Branch _newItem;
        public AddBranchView()
        {
            InitializeComponent();
            _newItem = new Branch();
            DataContext = _newItem;

            btnAdd.Click += btnAdd_Click;
            Loaded += (sender, args) => txtBranchName.Focus();
        }

        public Branch NewItem
        {
            get { return _newItem; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Branch item = Branch.FindByName(_newItem.BranchName);
            if (item == null)
            {
                var result = _newItem.Create();
                if (result.Success)
                {
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
                MessageWindow.ShowNotifyMessage("Branch already exists!");
            }
        }
    }
}
