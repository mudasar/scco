using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.DepartmentModule
{
    public partial class AddDepartmentView
    {
        private Department _newItem;
        public AddDepartmentView()
        {
            InitializeComponent();
            _newItem = new Department();
            DataContext = _newItem;

            btnAdd.Click += btnAdd_Click;
            Loaded += (sender, args) => txtDepartmentName.Focus();
        }

        public Department NewItem
        {
            get { return _newItem; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var item = Department.FindByName(_newItem.DepartmentName);
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
                MessageWindow.ShowNotifyMessage("Department already exists!");
            }
        }
    }
}
