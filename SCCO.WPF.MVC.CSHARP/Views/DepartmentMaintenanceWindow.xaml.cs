using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views {
    /// <summary>
    /// Interaction logic for DepartmentMaintenanceWindow.xaml
    /// </summary>
    public partial class DepartmentMaintenanceWindow {
        public DepartmentMaintenanceWindow() {
            InitializeComponent();
            _currentDepartment = new Department();
            DataContext = _currentDepartment;
        }

        private Department _currentDepartment = new Department();


        private void Create(object sender, RoutedEventArgs e) {
            DataContext = _currentDepartment = new Department();
        }

        private void Read(object sender, RoutedEventArgs e) {
            List<Department> departments = Department.GetList();
            List<SearchItem> searchItems =
                departments.Select(department => new SearchItem(department.DepartmentId, department.DepartmentName)).ToList();

            var searchWindow = new SearchWindow(searchItems);
            searchWindow.ShowDialog();
            if (searchWindow.DialogResult == true) {
                DataContext = _currentDepartment = new Department(searchWindow.SelectedItem.ItemId);
            }
        }

        private void Update(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(_currentDepartment.DepartmentName)) {
                MessageWindow.ShowAlertMessage("Department Name must not be empty!");
                return;
            }
            if (_currentDepartment.DepartmentId == 0) {
                _currentDepartment.Create();
                return;
            }
            _currentDepartment.Update();
        }

        private void Delete(object sender, RoutedEventArgs e) {
            if (
                MessageWindow.ShowConfirmMessage(
                    "You are about to delete current Department information. Do you want to proceed?") ==
                MessageBoxResult.Yes) {
                _currentDepartment.Destroy();
                MessageWindow.ShowNotifyMessage("Department information deleted!");
                DataContext = _currentDepartment = new Department(0);
            }
        }
    }
}
