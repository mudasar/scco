using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.DepartmentModule
{
    public partial class EditDepartmentView
    {
        private readonly Department _department;

        public EditDepartmentView()
        {
            InitializeComponent();
            btnUpdate.Click += btnUpdate_Click;
            Loaded += (sender, args) => txtDepartmentName.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = _department.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            DialogResult = true;
            Close();
        }

        public EditDepartmentView(int collectorId)
            : this()
        {
            _department = new Department();
            _department.Find(collectorId);
            DataContext = _department;
        }
    }
}
